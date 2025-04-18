using Microsoft.AspNetCore.Mvc.Filters;

namespace TrustZoneAPI.MiddleWares;
public class CurrentUserIdActionFilter : IActionFilter
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserIdActionFilter(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        // استخراج الـ UserId من التوكن وتخزينه في قيمة
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst("Uid")?.Value;

        //if (string.IsNullOrEmpty(userId))
        //{
        //    context.Result = new UnauthorizedResult(); 
        //    return;
        //}


        // تخزينه في HttpContext لتتمكن من الوصول إليه في كل مرة
        context.HttpContext.Items["UserId"] = userId;
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}
