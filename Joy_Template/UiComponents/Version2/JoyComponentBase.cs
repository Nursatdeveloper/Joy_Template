using Joy_Template.Controllers;
using Joy_Template.UiComponents.Base;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection.Metadata.Ecma335;
using System.Text.Encodings.Web;

namespace Joy_Template.UiComponents.Version2 {
    public interface IJoyUi {
        public JoyComponent Components();
    }
    public class JoyUi: IJoyUi {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public JoyUi(IHttpContextAccessor httpContextAccessor) {
            _httpContextAccessor= httpContextAccessor;
        }
        public JoyComponent Components() {
            return new JoyComponent(_httpContextAccessor.HttpContext);
        }
    }
    public static class HtmlHelperExtensions {
        public static JoyComponent Components<TModel>(this IHtmlHelper<TModel> htmlHelper) {
            return new JoyComponent();
        }

        public static JoyComponent Components<TModel>(this IHtmlHelper htmlHelper) {
            return new JoyComponent();
        }
    }
    public class JoyComponent {
        private List<TagBuilder> _tagBuilderList = new List<TagBuilder>();
        private HttpContext _httpContext;
        public JoyComponent() {

        }

        public JoyComponent(TagBuilder tagBuilder) {
            _tagBuilderList.Add(tagBuilder);
        }

        public JoyComponent(HttpContext httpContext) {
            _httpContext = httpContext;
        }
        public FormHtmlAttributes<TModel> Form<TModel>() {
            var formTag = new TagBuilder("form");
            var htmlHelperFactory = _httpContext.RequestServices.GetRequiredService<IHtmlHelperFactory<TModel>>();
            var htmlHelper = htmlHelperFactory.Create();
            return new FormHtmlAttributes<TModel>(formTag, htmlHelper);
        }
        public JoyComponentBase Panel() {
            return new JoyComponentBase();
        }

        public HtmlString Render() {
            var parent = new TagBuilder("div");
            foreach(var tag in _tagBuilderList) {
                if(tag.TagName == "form") {
                    tag.InnerHtml.AppendHtml("<input type='submit' class='btn btn-primary' />");
                }
                var str = tag.GetString();
                parent.InnerHtml.AppendHtml(str);
            }
            return parent.GetHtmlString();
        }
    }
    public record FormHtmlAttributeInfo(string Action, string Method);
    public static class IHtmlContentExtensions {
        public static string GetString(this IHtmlContent content) {
            using(var writer = new System.IO.StringWriter()) {
                content.WriteTo(writer, HtmlEncoder.Default);
                return writer.ToString();
            }
        }

        public static HtmlString GetHtmlString(this TagBuilder tagBuilder) {
            using(var writer = new System.IO.StringWriter()) {
                tagBuilder.WriteTo(writer, HtmlEncoder.Default);
                return new HtmlString(writer.ToString());
            }
        }
    }
    public class FormHtmlAttributes<TModel> {
        private IHtmlHelper<TModel> _htmlHelper;

        private TagBuilder _formTag;
        public FormHtmlAttributes(TagBuilder formTag, IHtmlHelper<TModel> htmlHelper) {
            _formTag = formTag;   
            _htmlHelper = htmlHelper;
        }

        public HtmlItems<TModel> HtmlAttributes(FormHtmlAttributeInfo attributeInfo) {
            _formTag.MergeAttribute("asp-action", attributeInfo.Action);
            _formTag.MergeAttribute("method", attributeInfo.Method);
            return new HtmlItems<TModel>(_formTag, _htmlHelper);
        }
    }

    public class HtmlItems<TModel> {
        private TagBuilder _parentTag;
        private IHtmlHelper<TModel> _htmlHelper;
        public HtmlItems(TagBuilder parentTag, IHtmlHelper<TModel> htmlHelper) {
            _parentTag = parentTag;
            _htmlHelper = htmlHelper;
        }
        public class HtmlItemsOption<TModel> {
            public List<IHtmlContent> HtmlContents { get; set; }
            private IHtmlHelper<TModel> _htmlHelper;
            public HtmlItemsOption(IHtmlHelper<TModel> htmlHelper) {
                _htmlHelper= htmlHelper;
                HtmlContents = new List<IHtmlContent>();
            }
            public HtmlItemsOption<TModel> Add(Func<IHtmlHelper<TModel>, IHtmlContent> func) {
                var htmlContent = func.Invoke(_htmlHelper);
                HtmlContents.Add(htmlContent);
                return this;
            }
        }
        public JoyComponent Items(Func<HtmlItemsOption<TModel>, HtmlItemsOption<TModel>> func) {
            var htmlContents = func.Invoke(new HtmlItemsOption<TModel>(_htmlHelper)).HtmlContents;
            foreach(var content in htmlContents) {
                _parentTag.InnerHtml.AppendHtml(content);
            }
            return new JoyComponent(_parentTag);
        }
    }
    public class JoyComponentBase {
        public string Tag { get; init; }
        public string CssClassValue { get; set; }
        public Dictionary<string, string> AttributeValues { get; set; }

        public JoyComponentBase() {
        }
        public JoyComponentBase CssClass(string cssClass) {
            CssClassValue = cssClass;
            return this;
        }

        public JoyComponentBase Attributes(Dictionary<string, string> attributes) {
            AttributeValues = attributes;
            return this;
        }


    }


}
