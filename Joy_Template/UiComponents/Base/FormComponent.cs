using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Encodings.Web;
using System.Text;

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
        public string Controller { get; set; }
        public string Action { get; set; }
        public HttpContext Context { get; set; }
        public FormComponent Render(Func<HtmlBase> func)
        {
            HtmlBase = func.Invoke();
            return this;
        }


        public FormComponent SetAction(string controller, string action)
        {
            Controller = controller.Replace("Controller", "");
            Action = action;
            return this;
        }
        public HtmlString GetHtml()
        {
            var antiforgery = getString(HtmlHelper.AntiForgeryToken());
            var sb = new StringBuilder();
            sb.Append($"<form method='post' action='{Controller}/{Action}'>" +
                $"{HtmlBase.ToHtmlString(HtmlHelper)}");
            sb.Append(antiforgery);
            sb.Append("<input type='submit' class='btn btn-primary' value='Save' />");
            sb.Append("</form>");
            return new HtmlString(sb.ToString());
        }
        public void ToHtmlString(out HtmlString html)
        {
            var antiforgery = getString(HtmlHelper.AntiForgeryToken());
            var sb = new StringBuilder();
            sb.Append($"<form method='post' action='{Controller}/{Action}'>" +
                $"{HtmlBase.ToHtmlString(HtmlHelper)}");
            sb.Append(antiforgery);
            sb.Append("<input type='submit' class='btn btn-primary' value='Save' />");
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
