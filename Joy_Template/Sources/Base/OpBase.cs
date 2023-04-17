using Joy_Template.Sources.Repository;
using Joy_Template.Sources.Users.Ops;
using Joy_Template.UiComponents.Base;
using Joy_Template.UiComponents.SystemUiComponents;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Bson;

namespace Joy_Template.Sources.Base
{
    public abstract class RenderableOperation<TModel, DbContext>: FormComponent {
        public RenderableOperation(IHtmlHelper htmlHelper, HttpContext httpContext) : base(htmlHelper, httpContext) {
            
        }

        public abstract Task SetModel(TModel model, DbContext context);
    }
}
