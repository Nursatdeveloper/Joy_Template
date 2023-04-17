using Joy_Template.UiComponents.Base;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;

namespace Joy_Template.UiComponents.SystemUiComponents {
    public static class CssClass {
        public static string Card = "shadow-sm p-3 mb-5 bg-white rounded";
    }
    public class Div : PairedHtmlTag {
        public Div(string cssClass = null, string text = null) : base(text) {
            CssClass = cssClass;
            Text = text;
        }
        public override HtmlString ToHtmlString(IHtmlHelper htmlHelper) {
            var sb = new StringBuilder();
            sb.Append($"<div class='{CssClass ?? string.Empty}' ");
            Attributes.ToList().ForEach(attr => sb.Append($"{attr.Key}='{attr.Value}' "));
            sb.Append('>');
            if (Children.Count > 0) {
                Children.ForEach(x => sb.Append(x.ToHtmlString(htmlHelper)));
            }
            if (!string.IsNullOrEmpty(Text)) {
                sb.Append(Text);
            }
            sb.Append("</div>");
            return new HtmlString(sb.ToString());
        }
    }
}
