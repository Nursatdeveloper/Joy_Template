﻿using Joy_Template.UiComponents.Base;
using Microsoft.AspNetCore.Html;
using System.Text;

namespace Joy_Template.UiComponents.SystemUiComponents {
    public class Label : PairedHtmlTag {
        public Label(string cssClass = null, string text = null) : base(text) {
            CssClass = cssClass;
            Text = text;
        }
        public override HtmlString ToHtmlString() {
            var sb = new StringBuilder();
            sb.Append($"<label class='{CssClass ?? string.Empty}' ");
            Attributes.ToList().ForEach(attr => sb.Append($"{attr.Key}='{attr.Value}' "));
            sb.Append('>');
            if (Children.Count > 0) {
                Children.ForEach(x => sb.Append(x.ToHtmlString()));
            }
            if (!string.IsNullOrEmpty(Text)) {
                sb.Append(Text);
            }
            sb.Append("</label>");
            return new HtmlString(sb.ToString());
        }
    }
}
