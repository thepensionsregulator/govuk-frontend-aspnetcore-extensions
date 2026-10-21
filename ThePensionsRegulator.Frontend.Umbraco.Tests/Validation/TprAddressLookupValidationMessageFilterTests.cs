using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Moq;
using ThePensionsRegulator.Frontend.HtmlGeneration;
using ThePensionsRegulator.Frontend.Umbraco.Models;
using ThePensionsRegulator.Frontend.Umbraco.Services;
using ThePensionsRegulator.Frontend.Umbraco.Validation;

namespace ThePensionsRegulator.Frontend.Umbraco.Tests.Validation;

public class TprAddressLookupValidationMessageFilterTests
{
    [Fact]
    public void OnActionExecuting_resolves_messages_for_each_address_and_preserves_other_errors()
    {
        var model = new AddressFormModel();
        var modelState = new ModelStateDictionary();
        var metadataProvider = new EmptyModelMetadataProvider();
        modelState.AddModelError(
            "ShippingAddress.AddressLine1",
            DefaultAttributeMessage<RequiredAttribute>(metadataProvider, nameof(UmbracoTprAddress.AddressLine1)));
        modelState.AddModelError(
            "ShippingAddress.AddressLine2",
            DefaultAttributeMessage<MaxLengthAttribute>(metadataProvider, nameof(UmbracoTprAddress.AddressLine2)));
        modelState.AddModelError("ShippingAddress.AddressLine3", "Explicit validation override");
        var bindingException = new FormatException("Invalid country value");
        modelState.SetModelValue("ShippingAddress.CountryId", null, null);
        modelState["ShippingAddress.CountryId"]!.Errors.Add(new ModelError(bindingException));
        modelState.AddModelError(
            "BillingAddress.AddressLine1",
            DefaultAttributeMessage<RequiredAttribute>(metadataProvider, nameof(UmbracoTprAddress.AddressLine1)));

        var shippingContent = new TprAddressLookupContent
        {
            AddressLine1RequiredErrorMessage = "Shipping address line 1 is required",
            AddressLine2MaxLengthErrorMessage = "Shipping address line 2 is too long"
        };
        var billingContent = new TprAddressLookupContent
        {
            AddressLine1RequiredErrorMessage = "Billing address line 1 is required"
        };
        var resolver = new Mock<ITprAddressLookupValidationMessageResolver>();
        resolver.Setup(x => x.Resolve("ShippingAddress")).Returns(shippingContent);
        resolver.Setup(x => x.Resolve("BillingAddress")).Returns(billingContent);
        var filter = new TprAddressLookupValidationMessageFilter(resolver.Object, metadataProvider);
        var context = CreateActionExecutingContext(modelState, new Dictionary<string, object?> { ["form"] = model });

        filter.OnActionExecuting(context);

        Assert.Equal("Shipping address line 1 is required", modelState["ShippingAddress.AddressLine1"]!.Errors[0].ErrorMessage);
        Assert.Equal("Shipping address line 2 is too long", modelState["ShippingAddress.AddressLine2"]!.Errors[0].ErrorMessage);
        Assert.Equal("Explicit validation override", modelState["ShippingAddress.AddressLine3"]!.Errors[0].ErrorMessage);
        Assert.Same(bindingException, modelState["ShippingAddress.CountryId"]!.Errors[0].Exception);
        Assert.Equal("Billing address line 1 is required", modelState["BillingAddress.AddressLine1"]!.Errors[0].ErrorMessage);
        resolver.Verify(x => x.Resolve("ShippingAddress"), Times.Once);
        resolver.Verify(x => x.Resolve("BillingAddress"), Times.Once);
    }

    [Fact]
    public void OnActionExecuting_does_not_resolve_messages_for_non_post_requests()
    {
        var modelState = new ModelStateDictionary();
        modelState.AddModelError("ShippingAddress.AddressLine1", "Required");
        var resolver = new Mock<ITprAddressLookupValidationMessageResolver>();
        var filter = new TprAddressLookupValidationMessageFilter(resolver.Object, new EmptyModelMetadataProvider());
        var context = CreateActionExecutingContext(modelState, new Dictionary<string, object?>(), HttpMethods.Get);

        filter.OnActionExecuting(context);

        resolver.Verify(x => x.Resolve(It.IsAny<string>()), Times.Never);
        Assert.Equal("Required", modelState["ShippingAddress.AddressLine1"]!.Errors[0].ErrorMessage);
    }

    private static string DefaultAttributeMessage<TAttribute>(IModelMetadataProvider metadataProvider, string propertyName)
        where TAttribute : ValidationAttribute
    {
        var attribute = typeof(UmbracoTprAddress).GetProperty(propertyName)!
            .GetCustomAttributes(typeof(TAttribute), inherit: true)
            .Cast<ValidationAttribute>()
            .Single();
        var displayName = metadataProvider.GetMetadataForProperty(typeof(UmbracoTprAddress), propertyName).GetDisplayName();
        return attribute.FormatErrorMessage(displayName);
    }

    private static ActionExecutingContext CreateActionExecutingContext(
        ModelStateDictionary modelState,
        IDictionary<string, object?> actionArguments,
        string method = "POST")
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Method = method;
        var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor(), modelState);
        return new ActionExecutingContext(
            actionContext,
            new List<IFilterMetadata>(),
            actionArguments,
            Mock.Of<Controller>());
    }

    private sealed class AddressFormModel
    {
        public UmbracoTprAddress ShippingAddress { get; } = new();
        public UmbracoTprAddress BillingAddress { get; } = new();
    }
}