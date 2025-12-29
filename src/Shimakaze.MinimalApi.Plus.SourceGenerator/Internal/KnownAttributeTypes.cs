namespace Shimakaze.MinimalApi.Plus.SourceGenerator.Internal;

internal static class KnownAttributeTypes
{
    public const string Produces = "global::Microsoft.AspNetCore.Mvc.ProducesAttribute";
    public const string ProducesResponseType = "global::Microsoft.AspNetCore.Mvc.ProducesResponseTypeAttribute";
    public const string AutoValidateAntiforgeryToken = "global::Microsoft.AspNetCore.Mvc.AutoValidateAntiforgeryTokenAttribute";
    public const string ValidateAntiForgeryToken = "global::Microsoft.AspNetCore.Mvc.ValidateAntiForgeryTokenAttribute";
    public const string IgnoreAntiforgeryToken = "global::Microsoft.AspNetCore.Mvc.IgnoreAntiforgeryTokenAttribute";
    public const string ApiExplorerSettings = "global::Microsoft.AspNetCore.Mvc.ApiExplorerSettingsAttribute";
    public const string EndpointDescription = "global::Microsoft.AspNetCore.Http.EndpointDescriptionAttribute";
    public const string EndpointSummary = "global::Microsoft.AspNetCore.Http.EndpointSummaryAttribute";
    public const string Tags = "global::Microsoft.AspNetCore.Http.TagsAttribute";
    public const string Authorize = "global::Microsoft.AspNetCore.Authorization.AuthorizeAttribute";
    public const string AllowAnonymous = "global::Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute";
    public const string DisableHttpMetrics = "global::Microsoft.AspNetCore.Http.DisableHttpMetricsAttribute";
    public const string Host = "global::Microsoft.AspNetCore.Routing.HostAttribute";
    public const string EndpointName = "global::Microsoft.AspNetCore.Routing.EndpointNameAttribute";
    public const string EndpointGroupName = "global::Microsoft.AspNetCore.Routing.EndpointGroupNameAttribute";
    public const string ExcludeFromDescription = "global::Microsoft.AspNetCore.Routing.ExcludeFromDescriptionAttribute";
    // MapMethod
    public const string AcceptVerbs = "global::Microsoft.AspNetCore.Mvc.AcceptVerbsAttribute";
    public const string HttpDelete = "global::Microsoft.AspNetCore.Mvc.HttpDeleteAttribute";
    public const string HttpGet = "global::Microsoft.AspNetCore.Mvc.HttpGetAttribute";
    public const string HttpHead = "global::Microsoft.AspNetCore.Mvc.HttpHeadAttribute";
    public const string HttpOptions = "global::Microsoft.AspNetCore.Mvc.HttpOptionsAttribute";
    public const string HttpPatch = "global::Microsoft.AspNetCore.Mvc.HttpPatchAttribute";
    public const string HttpPost = "global::Microsoft.AspNetCore.Mvc.HttpPostAttribute";
    public const string HttpPut = "global::Microsoft.AspNetCore.Mvc.HttpPutAttribute";
    public const string Route = "global::Microsoft.AspNetCore.Mvc.RouteAttribute";
    // Others
    public const string FromBody = "global::Microsoft.AspNetCore.Mvc.FromBodyAttribute";
    public const string FromForm = "global::Microsoft.AspNetCore.Mvc.FromFormAttribute";
    public const string FromHeader = "global::Microsoft.AspNetCore.Mvc.FromHeaderAttribute";
    public const string FromQuery = "global::Microsoft.AspNetCore.Mvc.FromQueryAttribute";
    public const string FromRoute = "global::Microsoft.AspNetCore.Mvc.FromRouteAttribute";
    public const string FromServices = "global::Microsoft.AspNetCore.Mvc.FromServicesAttribute";
    public const string FromKeyedServices = "global::Microsoft.Extensions.DependencyInjection.FromKeyedServicesAttribute";
    // 
    public const string Description = "global::System.ComponentModel.DescriptionAttribute";

    public const string ActionName = "global::Microsoft.AspNetCore.Mvc.ActionNameAttribute";
    public const string NonAction = "global::Microsoft.AspNetCore.Mvc.NonActionAttribute";

    public const string Obsolete = "global::System.ObsoleteAttribute";
    // 这是个抽象类
    public const string HttpMethod = "global::Microsoft.AspNetCore.Mvc.Routing.HttpMethodAttribute";
    // Not Supported
    public const string RequireHttps = "global::Microsoft.AspNetCore.Mvc.RequireHttpsAttribute";
    public const string AllowCookieRedirect = "global::Microsoft.AspNetCore.Http.AllowCookieRedirectAttribute";
    public const string ApiController = "global::Microsoft.AspNetCore.Mvc.ApiControllerAttribute";
    public const string Controller = "global::Microsoft.AspNetCore.Mvc.ControllerAttribute";
    public const string ControllerContext = "global::Microsoft.AspNetCore.Mvc.ControllerContextAttribute";
    public const string ActionContext = "global::Microsoft.AspNetCore.Mvc.ActionContextAttribute";
    public const string NonController = "global::Microsoft.AspNetCore.Mvc.NonControllerAttribute";
    public const string Area = "global::Microsoft.AspNetCore.Mvc.AreaAttribute";
    public const string ViewComponent = "global::Microsoft.AspNetCore.Mvc.ViewComponentAttribute";
    public const string ViewData = "global::Microsoft.AspNetCore.Mvc.ViewDataAttribute";
    public const string NonViewComponent = "global::Microsoft.AspNetCore.Mvc.NonViewComponentAttribute";
    public const string Bind = "global::Microsoft.AspNetCore.Mvc.BindAttribute";
    public const string BindProperties = "global::Microsoft.AspNetCore.Mvc.BindPropertiesAttribute";
    public const string BindProperty = "global::Microsoft.AspNetCore.Mvc.BindPropertyAttribute";
    public const string HiddenInput = "global::Microsoft.AspNetCore.Mvc.HiddenInputAttribute";
    public const string Remote = "global::Microsoft.AspNetCore.Mvc.RemoteAttribute";
    public const string PageRemote = "global::Microsoft.AspNetCore.Mvc.PageRemoteAttribute";
    public const string TempData = "global::Microsoft.AspNetCore.Mvc.TempDataAttribute";
    public const string FormatFilter = "global::Microsoft.AspNetCore.Mvc.FormatFilterAttribute";
    public const string MiddlewareFilter = "global::Microsoft.AspNetCore.Mvc.MiddlewareFilterAttribute";
    public const string ServiceFilter = "global::Microsoft.AspNetCore.Mvc.ServiceFilterAttribute";
    public const string TypeFilter = "global::Microsoft.AspNetCore.Mvc.TypeFilterAttribute";
    public const string ApiConventionMethod = "global::Microsoft.AspNetCore.Mvc.ApiConventionMethodAttribute";
    public const string ApiConventionType = "global::Microsoft.AspNetCore.Mvc.ApiConventionTypeAttribute";
    public const string ModelBinder = "global::Microsoft.AspNetCore.Mvc.ModelBinderAttribute";
    public const string ModelMetadataType = "global::Microsoft.AspNetCore.Mvc.ModelMetadataTypeAttribute";
    public const string SkipStatusCodePages = "global::Microsoft.AspNetCore.Mvc.SkipStatusCodePagesAttribute";
    public const string ResponseCache = "global::Microsoft.AspNetCore.Mvc.ResponseCacheAttribute";
    public const string RequestFormLimits = "global::Microsoft.AspNetCore.Mvc.RequestFormLimitsAttribute";
    public const string RequestSizeLimit = "global::Microsoft.AspNetCore.Mvc.RequestSizeLimitAttribute";
    public const string DisableRequestSizeLimit = "global::Microsoft.AspNetCore.Mvc.DisableRequestSizeLimitAttribute";
    public const string RouteValue = "global::Microsoft.AspNetCore.Mvc.Routing.RouteValueAttribute";
    public const string Consumes = "global::Microsoft.AspNetCore.Mvc.ConsumesAttribute";
    public const string ProducesDefaultResponseType = "global::Microsoft.AspNetCore.Mvc.ProducesDefaultResponseTypeAttribute";
    public const string ProducesErrorResponseType = "global::Microsoft.AspNetCore.Mvc.ProducesErrorResponseTypeAttribute";
    public const string AsParameters = "global::Microsoft.AspNetCore.Http.AsParametersAttribute";
}