using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Encodings.Web;
using System.Text;
using System.Security.Policy;
using Joy_Template.UiComponents.SystemUiComponents.Table;

namespace Joy_Template.UiComponents.Base
{
    public class FormComponent : ComponentBase
    {
        public FormComponent(IHtmlHelper htmlHelper, HttpContext httpContext) : base(htmlHelper)
        {
            HtmlHelper = httpContext.RequestServices.GetRequiredService<IHtmlHelperFactory>().Create();
        }

        public HtmlString Html { get; set; }
        public HtmlBase HtmlBase { get; set; }
        public SubmitArgs OnSubmitArgs { get; set; }
        public SubmitArgs OnRefreshArgs { get; set; }
        public HttpContext Context { get; set; }
        public FormComponent Render(Func<HtmlBase> func)
        {
            HtmlBase = func.Invoke();
            return this;
        }

        
        public FormComponent OnSubmit(SubmitArgs submitArgs)
        {
            OnSubmitArgs = submitArgs;
            return this;
        }

        public FormComponent OnRefresh(SubmitArgs refreshArgs) {
            OnRefreshArgs = refreshArgs;
            return this;
        }
        public HtmlString GetHtml()
        {
            var antiforgery = getString(HtmlHelper.AntiForgeryToken());
            var sb = new StringBuilder();
            sb.Append($"<form method='post' asp-action='{OnSubmitArgs.Action}'>" +
                $"{HtmlBase.ToHtmlString(HtmlHelper)}");
            sb.Append(antiforgery);
            sb.Append($"<input type='hidden' id='refresh-state-hidden' name='Refresh' value='false' />");
            sb.Append($"<input type='submit' class='{OnSubmitArgs.BtnCssClass} mt-2 mb-2' value='{OnSubmitArgs.BtnText}' />");
            if(OnRefreshArgs != null) {
                sb.Append($"<input type='submit' onclick='refreshForm()' class='{OnRefreshArgs.BtnCssClass} m-2' value='{OnRefreshArgs.BtnText}' />");
            }
            sb.Append("</form>");
            return new HtmlString(sb.ToString());
        }
        public void ToHtmlString(out HtmlString html)
        {
            var antiforgery = getString(HtmlHelper.AntiForgeryToken());
            var sb = new StringBuilder();
            sb.Append($"<form method='post' asp-action='{OnSubmitArgs.Action}'>" +
                $"{HtmlBase.ToHtmlString(HtmlHelper)}");
            sb.Append(antiforgery);
            sb.Append($"<input type='hidden' id='refresh-state-hidden' name='refresh' value='false' />");
            sb.Append($"<input type='submit' class='{OnSubmitArgs.BtnCssClass} mt-2 mb-2' value='{OnSubmitArgs.BtnText}' />");
            if(OnRefreshArgs != null) {
                sb.Append($"<input type='submit' onclick='refreshForm()' class='{OnRefreshArgs.BtnCssClass} m-2' value='{OnRefreshArgs.BtnText}' />");
            }
            sb.Append("</form>");
            html = new HtmlString(sb.ToString());
        }

        private static string getString(IHtmlContent content)
        {
            using (var writer = new StringWriter())
            {
                content.WriteTo(writer, HtmlEncoder.Default);
                return writer.ToString();
            }
        }
    }
}
