using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Shared.Helpers;

public class PaginationHeaderFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var headers = context.HttpContext.Request.Headers;

        var pageNumber = headers.ContainsKey("X-Page-Number")
            ? int.TryParse(headers["X-Page-Number"], out var page) ? page : 1
            : 1;

        var pageSize = headers.ContainsKey("X-Page-Size")
            ? int.TryParse(headers["X-Page-Size"], out var size) ? size : 10
            : 10;

        var paginationParams = new PaginationParams
        {
            pageNumber = pageNumber,
            pageSize = pageSize
        };

        context.ActionArguments["paginationParams"] = paginationParams;
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}