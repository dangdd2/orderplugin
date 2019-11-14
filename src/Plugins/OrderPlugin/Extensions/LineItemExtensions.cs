using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EPiServer;
using EPiServer.Commerce.Catalog;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.Linking;
using EPiServer.Commerce.Order;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using Mediachase.Commerce;
using Mediachase.Commerce.Catalog;

namespace Dc.EpiServerOrderPlugin.Extensions
{
    public static class LineItemExtensions
    {
        private static Injected<ReferenceConverter> _referenceConverter;
        private static Injected<ThumbnailUrlResolver> _thumbnailUrlResolver;

        public static string GetUrl(this ILineItem lineItem)
        {
            return lineItem.GetEntryContent()?.GetUrl();
        }

        public static string GetFullUrl(this ILineItem lineItem)
        {
            var rightUrl = lineItem.GetUrl();
            var baseUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
            return new Uri(new Uri(baseUrl), rightUrl).ToString();
        }

        public static string GetThumbnailUrl(this ILineItem lineItem)
        {
            return GetThumbnailUrl(lineItem.Code);
        }
        

        private static string GetThumbnailUrl(string code)
        {
            var entryContentLink = _referenceConverter.Service.GetContentLink(code);
            return ContentReference.IsNullOrEmpty(entryContentLink) ?
                string.Empty : 
                _thumbnailUrlResolver.Service.GetThumbnailUrl(entryContentLink, "thumbnail");
        }

        #region Extra Variant Info
        public static Tuple<string, string, string[], string, string> GetExtraInfo(this ILineItem lineItem)
        {
            return GetExtraInfo(lineItem.Code);
        }

        private static Tuple<string, string, string[], string, string> GetExtraInfo(string code)
        {
            string size, color, model, brand;
            XhtmlString description;
            var contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();

            var contentLink = _referenceConverter.Service.GetContentLink(code);

            //get product - variant details
            var variantDetails = contentLoader.Get<EntryContentBase>(contentLink);
            var product = GetParentProduct(variantDetails, contentLoader);

            product.Property.TryGetPropertyValue("Brand", out brand);
            product.Property.TryGetPropertyValue("Description", out description);
            var desValue = description.ToString();

            variantDetails.Property.TryGetPropertyValue("Color", out color);
            variantDetails.Property.TryGetPropertyValue("Size", out size);
            variantDetails.Property.TryGetPropertyValue("Model", out model);

            var categoryLinks = variantDetails.GetCategoryLinks(contentLoader);

            return new Tuple<string, string, string[], string, string>(size, color, categoryLinks.ToArray(), brand, desValue);
        }

        public static ProductContent GetParentProduct(EntryContentBase entry, IContentLoader _contentLoader)
        {
            var _relationRepository = ServiceLocator.Current.GetInstance<IRelationRepository>();
            return Get(entry.GetParentProducts(_relationRepository).SingleOrDefault(), _contentLoader);
        }

        public static ProductContent Get(ContentReference contentLink, IContentLoader _contentLoader)
        {
            var currentMarket = ServiceLocator.Current.GetInstance<ICurrentMarket>();
            var cm = currentMarket.GetCurrentMarket();
            return _contentLoader.Get<ProductContent>(contentLink, cm.DefaultLanguage);
        }

        public static IList<string> GetCategoryLinks(this EntryContentBase product, IContentLoader contentLoader)
        {
            var data = product.GetCategories()
                .SelectMany(parentLink => contentLoader.GetAncestors(parentLink)
                    .OfType<NodeContent>()
                    .Select(node => node.ContentLink)
                    .Concat(new[] { parentLink })
                ).Distinct();


            var categories = new List<string>();
            foreach (var contentLink in data)
            {
                var node = contentLoader.Get<NodeContent>(contentLink);
                categories.Add(node.DisplayName);
            }

            return categories;
        }

        #endregion
    }
}