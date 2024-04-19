using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AnswerCube.UI.MVC.Controllers;

public class BaseController : Controller
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        var controller = filterContext.RouteData.Values["controller"].ToString();

        if (controller == "CircularFlow")
        {
            ViewData["Layout"] = "_CircularFlow";
        }
        else
        {
            ViewData["Layout"] = "_Layout";
        }

        base.OnActionExecuting(filterContext);
    }
}