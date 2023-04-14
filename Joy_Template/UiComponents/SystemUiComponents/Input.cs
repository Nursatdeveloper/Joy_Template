using Joy_Template.UiComponents.Base;
using Microsoft.AspNetCore.Html;
using System.Text;

namespace Joy_Template.UiComponents.SystemUiComponents {
    public class Input : UnpairedHtmlTag {
        public string Value { get; set; }
        public string Type { get; set; }
        public Input(string type = null, string cssClass = null, string value = null) {
            CssClass = cssClass;
            Value = value;
            Type = type ?? "text";
        }
        public override HtmlString ToHtmlString() {
            var sb = new StringBuilder();
            sb.Append($"<input type='{Type}' class='{CssClass}' value='{Value}' ");
            Attributes.ToList().ForEach(attr => sb.Append($"{attr.Key}='{attr.Value}' "));
            sb.Append(" />");
            return new HtmlString(sb.ToString());
        }
    }
}
