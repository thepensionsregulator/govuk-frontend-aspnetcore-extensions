//------------------------------------------------------------------------------
// <auto-generated>
//   This code was generated by a tool.
//
//    Umbraco.ModelsBuilder.Embedded v10.4.1+0c6632e
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
	/// <summary>Inset text settings</summary>
	[PublishedModel("govukInsetTextSettings")]
	public partial class GovukInsetTextSettings : PublishedElementModel, IGovukCssClasses, IGovukGrid, IGovukGridColumnClasses
	{
		// helpers
#pragma warning disable 0109 // new is redundant
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.4.1+0c6632e")]
		public new const string ModelTypeAlias = "govukInsetTextSettings";
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.4.1+0c6632e")]
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.4.1+0c6632e")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public new static IPublishedContentType GetModelContentType(IPublishedSnapshotAccessor publishedSnapshotAccessor)
			=> PublishedModelUtility.GetModelContentType(publishedSnapshotAccessor, ModelItemType, ModelTypeAlias);
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.4.1+0c6632e")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public static IPublishedPropertyType GetModelPropertyType<TValue>(IPublishedSnapshotAccessor publishedSnapshotAccessor, Expression<Func<GovukInsetTextSettings, TValue>> selector)
			=> PublishedModelUtility.GetModelPropertyType(GetModelContentType(publishedSnapshotAccessor), selector);
#pragma warning restore 0109

		private IPublishedValueFallback _publishedValueFallback;

		// ctor
		public GovukInsetTextSettings(IPublishedElement content, IPublishedValueFallback publishedValueFallback)
			: base(content, publishedValueFallback)
		{
			_publishedValueFallback = publishedValueFallback;
		}

		// properties

		///<summary>
		/// CSS classes
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.4.1+0c6632e")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("cssClasses")]
		public virtual string CssClasses => global::Umbraco.Cms.Web.Common.PublishedModels.GovukCssClasses.GetCssClasses(this, _publishedValueFallback);

		///<summary>
		/// CSS classes for row
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.4.1+0c6632e")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("cssClassesForRow")]
		public virtual string CssClassesForRow => global::Umbraco.Cms.Web.Common.PublishedModels.GovukGrid.GetCssClassesForRow(this, _publishedValueFallback);

		///<summary>
		/// Column size
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.4.1+0c6632e")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("columnSize")]
		public virtual string ColumnSize => global::Umbraco.Cms.Web.Common.PublishedModels.GovukGridColumnClasses.GetColumnSize(this, _publishedValueFallback);

		///<summary>
		/// Column size (from desktop): Defaults to 'two-thirds' if both column size properties are left blank.
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.4.1+0c6632e")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("columnSizeFromDesktop")]
		public virtual string ColumnSizeFromDesktop => global::Umbraco.Cms.Web.Common.PublishedModels.GovukGridColumnClasses.GetColumnSizeFromDesktop(this, _publishedValueFallback);

		///<summary>
		/// CSS classes for column
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.4.1+0c6632e")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("cssClassesForColumn")]
		public virtual string CssClassesForColumn => global::Umbraco.Cms.Web.Common.PublishedModels.GovukGridColumnClasses.GetCssClassesForColumn(this, _publishedValueFallback);
	}
}
