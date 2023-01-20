// <copyright file="NestedContentConverterTreeController.cs" company="Umbraco Community">
// Copyright (c) Dave Woestenborghs and contributors
// </copyright>

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Actions;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Trees;
using Umbraco.Cms.Web.BackOffice.Trees;
using Umbraco.Cms.Web.Common.Attributes;

namespace Umbraco.Community.NestedContentConverter.BackOffice.Controllers.Trees
{
    /// <summary>
    /// The nested content converter tree controller that will render in the settings section.
    /// </summary>
    [Tree(
        Cms.Core.Constants.Applications.Settings,
        Constants.BackOffice.TreeAlias,
        TreeGroup = Constants.BackOffice.TreeGroup,
        TreeTitle = Constants.BackOffice.TreeName,
        SortOrder = 35)]
    [PluginController(Constants.BackOffice.PluginName)]
    public sealed class NestedContentConverterTreeController : TreeController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NestedContentConverterTreeController"/> class.
        /// </summary>
        /// <param name="localizedTextService">A <see cref="ILocalizationService"/>.</param>
        /// <param name="umbracoApiControllerTypeCollection">A <see cref="UmbracoApiControllerTypeCollection"/>.</param>
        /// <param name="eventAggregator">A <see cref="IEventAggregator"/>.</param>
        public NestedContentConverterTreeController(ILocalizedTextService localizedTextService, UmbracoApiControllerTypeCollection umbracoApiControllerTypeCollection, IEventAggregator eventAggregator)
            : base(localizedTextService, umbracoApiControllerTypeCollection, eventAggregator)
        {
        }

        /// <inheritdoc/>
        protected override ActionResult<TreeNodeCollection> GetTreeNodes(string id, FormCollection queryStrings) => new(new TreeNodeCollection());

        /// <inheritdoc />
        protected override ActionResult<MenuItemCollection> GetMenuForNode(string id, FormCollection queryStrings) =>
            new(new MenuItemCollection(new ActionCollection(Enumerable.Empty<IAction>)));

        /// <inheritdoc/>
        protected override ActionResult<TreeNode?> CreateRootNode(FormCollection queryStrings)
        {
            var result = base.CreateRootNode(queryStrings);

            if (result.Value is null)
            {
                return result;
            }

            result.Value.RoutePath = $"{this.SectionAlias}/{Constants.BackOffice.TreeAlias}/dashboard";
            result.Value.Icon = "icon-thumbnail-list";
            result.Value.HasChildren = false;
            result.Value.MenuUrl = null;

            return result.Value;
        }
    }
}
