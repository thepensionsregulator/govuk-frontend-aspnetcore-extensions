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
	/// <summary>Text input settings</summary>
	[PublishedModel("govukTextInputSettings")]
	public partial class GovukTextInputSettings : PublishedElementModel, IGovukCssClasses, IGovukGrid, IGovukGridColumnClasses, IGovukLabelIsPageHeading, IGovukModelProperty, IGovukValidationCustom, IGovukValidationRequired, IGovukValidationText
	{
		// helpers
#pragma warning disable 0109 // new is redundant
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.4.1+0c6632e")]
		public new const string ModelTypeAlias = "govukTextInputSettings";
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.4.1+0c6632e")]
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.4.1+0c6632e")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public new static IPublishedContentType GetModelContentType(IPublishedSnapshotAccessor publishedSnapshotAccessor)
			=> PublishedModelUtility.GetModelContentType(publishedSnapshotAccessor, ModelItemType, ModelTypeAlias);
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.4.1+0c6632e")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public static IPublishedPropertyType GetModelPropertyType<TValue>(IPublishedSnapshotAccessor publishedSnapshotAccessor, Expression<Func<GovukTextInputSettings, TValue>> selector)
			=> PublishedModelUtility.GetModelPropertyType(GetModelContentType(publishedSnapshotAccessor), selector);
#pragma warning restore 0109

		private IPublishedValueFallback _publishedValueFallback;

		// ctor
		public GovukTextInputSettings(IPublishedElement content, IPublishedValueFallback publishedValueFallback)
			: base(content, publishedValueFallback)
		{
			_publishedValueFallback = publishedValueFallback;
		}

		// properties

		///<summary>
		/// Prefix: For example, a £ sign before a currency field.
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.4.1+0c6632e")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("prefix")]
		public virtual string Prefix => this.Value<string>(_publishedValueFallback, "prefix");

		///<summary>
		/// Suffix: For example, a % sign after a percentage field.
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.4.1+0c6632e")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("suffix")]
		public virtual string Suffix => this.Value<string>(_publishedValueFallback, "suffix");

		///<summary>
		/// Text input width
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.4.1+0c6632e")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("textInputWidth")]
		public virtual string TextInputWidth => this.Value<string>(_publishedValueFallback, "textInputWidth");

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

		///<summary>
		/// The label is the page heading
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.4.1+0c6632e")]
		[ImplementPropertyType("labelIsPageHeading")]
		public virtual bool LabelIsPageHeading => global::Umbraco.Cms.Web.Common.PublishedModels.GovukLabelIsPageHeading.GetLabelIsPageHeading(this, _publishedValueFallback);

		///<summary>
		/// Model property: The name of the property on the view model being bound to in the code.
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.4.1+0c6632e")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("modelProperty")]
		public virtual object ModelProperty => global::Umbraco.Cms.Web.Common.PublishedModels.GovukModelProperty.GetModelProperty(this, _publishedValueFallback);

		///<summary>
		/// Custom error 1: Sets the message displayed by a custom validator.
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.4.1+0c6632e")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("customError1")]
		public virtual string CustomError1 => global::Umbraco.Cms.Web.Common.PublishedModels.GovukValidationCustom.GetCustomError1(this, _publishedValueFallback);

		///<summary>
		/// Custom error 2: Sets the message displayed by a custom validator.
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.4.1+0c6632e")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("customError2")]
		public virtual string CustomError2 => global::Umbraco.Cms.Web.Common.PublishedModels.GovukValidationCustom.GetCustomError2(this, _publishedValueFallback);

		///<summary>
		/// Custom error 3: Sets the message displayed by a custom validator.
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.4.1+0c6632e")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("customError3")]
		public virtual string CustomError3 => global::Umbraco.Cms.Web.Common.PublishedModels.GovukValidationCustom.GetCustomError3(this, _publishedValueFallback);

		///<summary>
		/// Required: Sets the message displayed if the field is set by the code to be required.
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.4.1+0c6632e")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("errorMessageRequired")]
		public virtual string ErrorMessageRequired => global::Umbraco.Cms.Web.Common.PublishedModels.GovukValidationRequired.GetErrorMessageRequired(this, _publishedValueFallback);

		///<summary>
		/// Compare to another field: Sets the error message displayed if the field is set to be the same as another (like when you're asked to re-enter an email address).
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.4.1+0c6632e")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("errorMessageCompare")]
		public virtual string ErrorMessageCompare => global::Umbraco.Cms.Web.Common.PublishedModels.GovukValidationText.GetErrorMessageCompare(this, _publishedValueFallback);

		///<summary>
		/// Email address: Sets the message displayed if the field is set by the code to require an email address.
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.4.1+0c6632e")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("errorMessageEmail")]
		public virtual string ErrorMessageEmail => global::Umbraco.Cms.Web.Common.PublishedModels.GovukValidationText.GetErrorMessageEmail(this, _publishedValueFallback);

		///<summary>
		/// Minimum and maximum length: Sets the message displayed if the field is set by the code to require text between a minimum and maximum length.
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.4.1+0c6632e")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("errorMessageLength")]
		public virtual string ErrorMessageLength => global::Umbraco.Cms.Web.Common.PublishedModels.GovukValidationText.GetErrorMessageLength(this, _publishedValueFallback);

		///<summary>
		/// Maximum length: Sets the message displayed if the field is set by the code to require text of a maximum length.
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.4.1+0c6632e")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("errorMessageMaxLength")]
		public virtual string ErrorMessageMaxLength => global::Umbraco.Cms.Web.Common.PublishedModels.GovukValidationText.GetErrorMessageMaxLength(this, _publishedValueFallback);

		///<summary>
		/// Minimum length: Sets the message displayed if the field is set by the code to require text of a minimum length.
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.4.1+0c6632e")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("errorMessageMinLength")]
		public virtual string ErrorMessageMinLength => global::Umbraco.Cms.Web.Common.PublishedModels.GovukValidationText.GetErrorMessageMinLength(this, _publishedValueFallback);

		///<summary>
		/// Phone number: Sets the message displayed if the field is set by the code to require a phone number.
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.4.1+0c6632e")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("errorMessagePhone")]
		public virtual string ErrorMessagePhone => global::Umbraco.Cms.Web.Common.PublishedModels.GovukValidationText.GetErrorMessagePhone(this, _publishedValueFallback);

		///<summary>
		/// Numeric range: Sets the message displayed if the field is set by the code to require a number in a given range.
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.4.1+0c6632e")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("errorMessageRange")]
		public virtual string ErrorMessageRange => global::Umbraco.Cms.Web.Common.PublishedModels.GovukValidationText.GetErrorMessageRange(this, _publishedValueFallback);

		///<summary>
		/// Pattern: Sets the message displayed if the field is set by the code to require a regular expression pattern to be matched.
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.4.1+0c6632e")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("errorMessageRegex")]
		public virtual string ErrorMessageRegex => global::Umbraco.Cms.Web.Common.PublishedModels.GovukValidationText.GetErrorMessageRegex(this, _publishedValueFallback);
	}
}
