$CurrentDir = Split-Path $MyInvocation.MyCommand.Path
$RootDir = Split-Path -Path $CurrentDir -Parent
$SourceDir = "$RootDir\src"
$DocsDir = "$RootDir\docs"
$ExamplesDir = "$RootDir\doc-examples"
$PublishDir = "$RootDir\dist"
$CoreDllPath = "$PublishDir\DotControl.Umbraco.SeoTools.Core.dll"
$WebDllPath = "$PublishDir\DotControl.Umbraco.SeoTools.Web.dll"

# See https://ejball.com/XmlDocMarkdown/ for docs
dotnet tool install xmldocmd -g

cd $SourceDir

dotnet publish --configuration Release -o $PublishDir --self-contained --runtime win-x64
dotnet build

xmldocmd $CoreDllPath "$DocsDir\Core" --obsolete --toc --clean
xmldocmd $WebDllPath "$DocsDir\Web" --obsolete --toc --clean

if (Test-Path -Path $PublishDir) {
    Remove-Item -LiteralPath $PublishDir -Force -Recurse
}

cd $CurrentDir