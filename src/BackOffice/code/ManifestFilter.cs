// <copyright file="ManifestFilter.cs" company="Umbraco Community">
// Copyright (c) Dave Woestenborghs and contributors
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Umbraco.Cms.Core.Manifest;

namespace Umbraco.Community.NestedContentConverter.BackOffice
{
    /// <summary>
    /// Manifest filter to register package with the back office.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal sealed class ManifestFilter : IManifestFilter
    {
        /// <inheritdoc/>
        public void Filter(List<PackageManifest> manifests)
        {
            var assembly = typeof(ManifestFilter).Assembly;

            manifests.Add(new PackageManifest()
            {
                AllowPackageTelemetry = true,
                PackageName = Constants.BackOffice.TreeName,
                Version = assembly.GetName().Version?.ToString(3) ?? "1.0.0",
                BundleOptions = BundleOptions.Default,
                Scripts =
                    new[] { "/App_Plugins/NestedContentConverter/backoffice/nestedcontentconverter/resource.js", "/App_Plugins/NestedContentConverter/backoffice/nestedcontentconverter/controller.js" },
            });
        }
    }
}
