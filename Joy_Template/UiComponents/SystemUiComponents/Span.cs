﻿using Joy_Template.UiComponents.Base;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;

namespace Joy_Template.UiComponents.SystemUiComponents {
    public class Span : PairedHtmlTag {
        public Span(string cssClass = null, string text = null) : base(text) {
            CssClass = cssClass;
            Text = text;
        }
        public override HtmlString ToHtmlString(IHtmlHelper htmlHelper) {
            var sb = new StringBuilder();
            sb.Append($"<span class='{CssClass ?? string.Empty}' ");
            Attributes.ToList().ForEach(attr => sb.Append($"{attr.Key}='{attr.Value}' "));
            sb.Append('>');
            if (Children.Count > 0) {
                Children.ForEach(x => sb.Append(x.ToHtmlString(htmlHelper)));
            }
            if (!string.IsNullOrEmpty(Text)) {
                sb.Append(Text);
            }
            sb.Append("</span>");
            return new HtmlString(sb.ToString());
        }
    }
}
