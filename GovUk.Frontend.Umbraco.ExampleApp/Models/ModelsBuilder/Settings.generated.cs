//------------------------------------------------------------------------------
// <auto-generated>
//   This code was generated by a tool.
//
//    Umbraco.ModelsBuilder.Embedded v10.8.3+e04a41b
//
//   Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Linq.Expressions;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Infrastructure.ModelsBuilder;
using Umbraco.Cms.Core;
using Umbraco.Extensions;

namespace Umbraco.Cms.Web.Common.PublishedModels
{
	/// <summary>Settings</summary>
	[PublishedModel("settings")]
	public partial class Settings : PublishedContentModel, IGovukPhaseBanner, IGovukSkipLink, ITprBackToTop, ITprContextBar1, ITprContextBar2, ITprContextBar3, ITprFooter, ITprHeader
	{
		// helpers
#pragma warning disable 0109 // new is redundant
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.8.3+e04a41b")]
		public new const string ModelTypeAlias = "settings";
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.8.3+e04a41b")]
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.8.3+e04a41b")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public new static IPublishedContentType GetModelContentType(IPublishedSnapshotAccessor publishedSnapshotAccessor)
			=> PublishedModelUtility.GetModelContentType(publishedSnapshotAccessor, ModelItemType, ModelTypeAlias);
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.8.3+e04a41b")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public static IPublishedPropertyType GetModelPropertyType<TValue>(IPublishedSnapshotAccessor publishedSnapshotAccessor, Expression<Func<Settings, TValue>> selector)
			=> PublishedModelUtility.GetModelPropertyType(GetModelContentType(publishedSnapshotAccessor), selector);
#pragma warning restore 0109

		private IPublishedValueFallback _publishedValueFallback;

		// ctor
		public Settings(IPublishedContent content, IPublishedValueFallback publishedValueFallback)
			: base(content, publishedValueFallback)
		{
			_publishedValueFallback = publishedValueFallback;
		}

		// properties

		///<summary>
		/// Phase
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.8.3+e04a41b")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("govukPhase")]
		public virtual string GovukPhase => global::Umbraco.Cms.Web.Common.PublishedModels.GovukPhaseBanner.GetGovukPhase(this, _publishedValueFallback);

		///<summary>
		/// Phase banner text
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.8.3+e04a41b")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("govukPhaseBannerText")]
		public virtual global::Umbraco.Cms.Core.Strings.IHtmlEncodedString GovukPhaseBannerText => global::Umbraco.Cms.Web.Common.PublishedModels.GovukPhaseBanner.GetGovukPhaseBannerText(this, _publishedValueFallback);

		///<summary>
		/// Skip link text: Defaults to 'Skip to main content' if left blank.
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.8.3+e04a41b")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("govukSkipLinkText")]
		public virtual string GovukSkipLinkText => global::Umbraco.Cms.Web.Common.PublishedModels.GovukSkipLink.GetGovukSkipLinkText(this, _publishedValueFallback);

		///<summary>
		/// Back to top text: Defaults to 'Back to top' if left blank.
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.8.3+e04a41b")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("tprBackToTopText")]
		public virtual string TprBackToTopText => global::Umbraco.Cms.Web.Common.PublishedModels.TprBackToTop.GetTprBackToTopText(this, _publishedValueFallback);

		///<summary>
		/// Context 1: Typically the name of the application or the mode it's in.
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.8.3+e04a41b")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("tprContext1")]
		public virtual global::Umbraco.Cms.Core.Strings.IHtmlEncodedString TprContext1 => global::Umbraco.Cms.Web.Common.PublishedModels.TprContextBar1.GetTprContext1(this, _publishedValueFallback);

		///<summary>
		/// Context 2: Typically the name of the entity being edited in the application.
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.8.3+e04a41b")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("tprContext2")]
		public virtual global::Umbraco.Cms.Core.Strings.IHtmlEncodedString TprContext2 => global::Umbraco.Cms.Web.Common.PublishedModels.TprContextBar2.GetTprContext2(this, _publishedValueFallback);

		///<summary>
		/// Context 3: Typically a reference number for the entity being edited in the application.
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.8.3+e04a41b")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("tprContext3")]
		public virtual global::Umbraco.Cms.Core.Strings.IHtmlEncodedString TprContext3 => global::Umbraco.Cms.Web.Common.PublishedModels.TprContextBar3.GetTprContext3(this, _publishedValueFallback);

		///<summary>
		/// Content: A small number of links, or blank.
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.8.3+e04a41b")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("tprFooterContent")]
		public virtual global::Umbraco.Cms.Core.Strings.IHtmlEncodedString TprFooterContent => global::Umbraco.Cms.Web.Common.PublishedModels.TprFooter.GetTprFooterContent(this, _publishedValueFallback);

		///<summary>
		/// Copyright: Use {{year}} to include the current year.
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.8.3+e04a41b")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("tprFooterCopyright")]
		public virtual string TprFooterCopyright => global::Umbraco.Cms.Web.Common.PublishedModels.TprFooter.GetTprFooterCopyright(this, _publishedValueFallback);

		///<summary>
		/// Logo alternative text: Defaults to 'The Pensions Regulator home page' if left blank.
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.8.3+e04a41b")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("tprFooterLogoAlt")]
		public virtual string TprFooterLogoAlt => global::Umbraco.Cms.Web.Common.PublishedModels.TprFooter.GetTprFooterLogoAlt(this, _publishedValueFallback);

		///<summary>
		/// Logo links to where?: Typically the TPR website home page, https://www.thepensionsregulator.gov.uk
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.8.3+e04a41b")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("tprFooterLogoHref")]
		public virtual global::Umbraco.Cms.Core.Models.Link TprFooterLogoHref => global::Umbraco.Cms.Web.Common.PublishedModels.TprFooter.GetTprFooterLogoHref(this, _publishedValueFallback);

		///<summary>
		/// Content: A small number of links, or blank.
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.8.3+e04a41b")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("tprHeaderContent")]
		public virtual global::Umbraco.Cms.Core.Strings.IHtmlEncodedString TprHeaderContent => global::Umbraco.Cms.Web.Common.PublishedModels.TprHeader.GetTprHeaderContent(this, _publishedValueFallback);

		///<summary>
		/// Label: Typically set to 'Making workplace pensions work'
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.8.3+e04a41b")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("tprHeaderLabel")]
		public virtual string TprHeaderLabel => global::Umbraco.Cms.Web.Common.PublishedModels.TprHeader.GetTprHeaderLabel(this, _publishedValueFallback);

		///<summary>
		/// Logo alternative text: Defaults to 'The Pensions Regulator home page' if left blank.
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.8.3+e04a41b")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("tprHeaderLogoAlt")]
		public virtual string TprHeaderLogoAlt => global::Umbraco.Cms.Web.Common.PublishedModels.TprHeader.GetTprHeaderLogoAlt(this, _publishedValueFallback);

		///<summary>
		/// Logo links to where?: Typically the TPR website home page, https://www.thepensionsregulator.gov.uk
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.8.3+e04a41b")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("tprHeaderLogoHref")]
		public virtual global::Umbraco.Cms.Core.Models.Link TprHeaderLogoHref => global::Umbraco.Cms.Web.Common.PublishedModels.TprHeader.GetTprHeaderLogoHref(this, _publishedValueFallback);
	}
}
