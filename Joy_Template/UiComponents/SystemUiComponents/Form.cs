using Joy_Template.UiComponents.Base;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Text;

namespace Joy_Template.UiComponents.SystemUiComponents {
    public class Form : PairedHtmlTag {
        public string AspAction { get; set; }
        public string Method { get; set; }
        public string AspController { get; set; }
        public Form(string aspAction, string aspController = null, string method = null, string cssClass = null, string text = null) : base(text) {
            CssClass = cssClass;
            Text = text;
            AspAction = aspAction;
            Method = method;
            AspController = aspController;
        }
        public override HtmlString ToHtmlString(IHtmlHelper htmlHelper) {
            var sb = new StringBuilder();
            if (!string.IsNullOrEmpty(AspController)) {
                sb.Append($"<form asp-action='{AspAction}' asp-controller='{AspController}'  method='{Method}' class='{CssClass ?? string.Empty}' ");
            } else {
                sb.Append($"<form asp-action='{AspAction}' method='{Method}' class='{CssClass ?? string.Empty}' ");
            }
            Attributes.ToList().ForEach(attr => sb.Append($"{attr.Key}='{attr.Value}' "));
            sb.Append('>');
            if (Children.Count > 0) {
                Children.ForEach(x => sb.Append(x.ToHtmlString(htmlHelper)));
            }
            if (!string.IsNullOrEmpty(Text)) {
                sb.Append(Text);
            }
            sb.Append("</form>");
            return new HtmlString(sb.ToString());
        }
    }
}
