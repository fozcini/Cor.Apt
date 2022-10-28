using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Html;

namespace Cor.Apt.Helpers
{
    public static class MyHTMLHelpers
    {
        public static IHtmlContent IsActive(this IHtmlHelper htmlHelper, string controllers, string actions, string cssClass = "active")
        {
            string currentAction = htmlHelper.ViewContext.RouteData.Values["action"] as string;
            string currentController = htmlHelper.ViewContext.RouteData.Values["controller"] as string;
            IEnumerable<string> acceptedActions = (actions ?? currentAction).Split(',');
            IEnumerable<string> acceptedControllers = (controllers ?? currentController).Split(',');

            return new HtmlString(acceptedActions.Contains(currentAction) && acceptedControllers.Contains(currentController) ? cssClass : String.Empty);
        }
    }
}