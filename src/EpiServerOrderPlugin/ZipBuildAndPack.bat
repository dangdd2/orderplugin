@echo off
cls

echo Creating the module zip file..
powershell -command "Compress-Archive -Path .\Views\, .\module.config -DestinationPath .\Modules\_protected\EpiServerOrderPlugin\EpiServerOrderPlugin -CompressionLevel Optimal -Force"
echo.

echo Creating NuGet package..
echo.
nuget pack EpiServerOrderPlugin.csproj -Build -Properties Configuration=Release -Verbosity detailed

echo.
echo.
echo Done!