using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Joy_Template.UiComponents.Base {
    public abstract class ComponentBase: TagHelper, IViewContextAware {

        public ComponentBase(IHtmlHelper htmlHelper) {
            HtmlHelper = htmlHelper;
        }
        public IHtmlHelper HtmlHelper { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public void Contextualize(ViewContext viewContext) {
            if(HtmlHelper is IViewContextAware) {
                ((IViewContextAware)HtmlHelper).Contextualize(viewContext);
            }
        }
    }
}
