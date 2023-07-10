using Joy_Template.UiComponents.Base;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;

namespace Joy_Template.UiComponents.SystemUiComponents {
    public class Input : UnpairedHtmlTag {
        public string Value { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public Input(string type = null, string cssClass = null, string value = null, string name = null) {
            CssClass = cssClass;
            Value = value;
            Type = type ?? "text";
            Name = name;
        }
        public override HtmlString ToHtmlString(IHtmlHelper htmlHelper) {
            var sb = new StringBuilder();
            sb.Append($"<input type='{Type}' name='{Name}' class='{CssClass}' value='{Value}' ");
            Attributes.ToList().ForEach(attr => sb.Append($"{attr.Key}='{attr.Value}' "));
            sb.Append(" />");
            return new HtmlString(sb.ToString());
        }
    }
}
