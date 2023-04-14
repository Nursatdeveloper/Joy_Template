using Microsoft.AspNetCore.Html;

namespace Joy_Template.UiComponents.Base {
    public abstract class HtmlBase {
        public string CssClass { get; set; }
        public Dictionary<string, string> Attributes { get; set; }

        public HtmlBase() {
            Attributes = new Dictionary<string, string>();
        }
        public HtmlBase WithAttr(string key, string value) {
            Attributes[key] = value; return this;
        }
        public abstract HtmlString ToHtmlString();
    }

    public abstract class PairedHtmlTag : HtmlBase {
        public PairedHtmlTag(string text = null) {
            Children = new List<HtmlBase>();
            Text = text;
        }
        public List<HtmlBase> Children { get; set; }
        public string Text { get; set; }

        public PairedHtmlTag Append(HtmlBase html) {
            Children.Add(html);
            return this;
        }
    }

    public abstract class UnpairedHtmlTag : HtmlBase {

    }
}
