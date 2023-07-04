using Joy_Template.UiComponents.Base;
using Joy_Template.UiComponents.SystemUiComponents.Table;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;
using System.Text.Encodings.Web;

namespace Joy_Template.Wizard {
    public abstract class WizardBase<TModel> : Controller {
        public TModel Model { get; set; }
        //private readonly IHtmlHelper _htmlHelper;

        //public WizardBase(TModel model, IHtmlHelper htmlHelper) {
        //    Model = model;
        //    _htmlHelper = htmlHelper;
        //}
        public WizardBase(TModel model) {
            Model = model;
        }

        public abstract StepsCollection<TModel> Steps(IWizardBuilder<TModel> builder);

        [HttpGet]
        public IActionResult Index() {
            var steps = Steps(new WizardBuilder<TModel>(Model, HttpContext));
            var htmlHelper = HttpContext.RequestServices.GetRequiredService<IHtmlHelperFactory>().Create();
            var renderable = steps.StepInfo[0].RenderHtml;
            var wizardForm = new WizardForm(nameof(Index), "WizardBase")
                .SetStep(2)
                .Append(renderable)
                .ToHtmlString(htmlHelper);

            ViewData["html"] = wizardForm;
            return new ViewResult() {
                ViewName = "WizardView",
                ViewData = ViewData
            };
        }

        [HttpPost]
        public IActionResult Index(int step) {
            var steps = Steps(new WizardBuilder<TModel>(Model, HttpContext));

            var htmlHelper = HttpContext.RequestServices.GetRequiredService<IHtmlHelperFactory>().Create();
            var renderable = steps.StepInfo[step-1].RenderHtml;
            var wizardForm = new WizardForm(nameof(Index), "WizardBase")
                .SetStep(step+1)
                .Append(renderable)
                .ToHtmlString(htmlHelper);

            ViewData["html"] = wizardForm;
            return new ViewResult() {
                ViewName = "WizardView",
                ViewData = ViewData
            };
        }

    }


    #region Environments
    public class EnvironmentBase<TModel> {
        public TModel Model { get; set; }
        public EnvironmentBase(TModel model, HttpContext httpContext) {
            Model = model;
        }
    }
    public class RenderEnvironment<TModel> : EnvironmentBase<TModel> {
        public RenderEnvironment(TModel model, HttpContext httpContext) : base(model, httpContext) {
        }
    }

    public class ValidationEnvironment<TModel> : EnvironmentBase<TModel> {
        public ValidationEnvironment(TModel model, HttpContext httpContext) : base(model, httpContext) {
        }
    }
    #endregion

    #region Actions
    public interface IWizardActionBuilder<TModel> {
        public IWizardActionHandler<TModel> OnRendering(Func<RenderEnvironment<TModel>, HtmlBase> re);
    }
    public abstract class WizardActionBuilderBase : TagHelper, IViewContextAware {

        public WizardActionBuilderBase(IHtmlHelper htmlHelper) {
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
    public class WizardActionBuilder<TModel>: WizardActionBuilderBase, IWizardActionBuilder<TModel> {
        private HtmlBase _html;
        private RenderEnvironment<TModel> _renderEnv;
        public WizardActionBuilder(TModel model, HttpContext httpContext) : base(httpContext.RequestServices.GetRequiredService<IHtmlHelperFactory>().Create()) {
            _renderEnv = new RenderEnvironment<TModel>(model, httpContext);
        }

        public IWizardActionHandler<TModel> OnRendering(Func<RenderEnvironment<TModel>, HtmlBase> re) {
            _html = re.Invoke(_renderEnv);
            return new WizardActionHandler<TModel>(_html);
        }

        private HtmlString getHtml() {
            var antiforgery = getString(HtmlHelper.AntiForgeryToken());
            var sb = new StringBuilder();
            sb.Append($"<form method='post' asp-action=''>" +
                $"{_html.ToHtmlString(HtmlHelper)}");
            sb.Append(antiforgery);
            sb.Append($"<input type='hidden' id='refresh-state-hidden' name='Refresh' value='false' />");
            sb.Append($"<input type='submit' class='btn btn-primary btn-sm mt-2 mb-2' value='Submit' />");
       
            sb.Append("</form>");
            return new HtmlString(sb.ToString());
        }

        private static string getString(IHtmlContent content) {
            using(var writer = new StringWriter()) {
                content.WriteTo(writer, HtmlEncoder.Default);
                return writer.ToString();
            }
        }
    }

    public interface IWizardActionHandler<TModel> {
        public HtmlBase Html { get; set; }
        public IWizardActionHandler<TModel> OnValidating(Func<ValidationEnvironment<TModel>, bool> ve);
    }

    public class WizardActionHandler<TModel> : IWizardActionHandler<TModel> {
        public WizardActionHandler(HtmlBase html) {
            Html = html;
        }
        private HtmlBase _html;
        public HtmlBase Html { get => _html; set => _html = value; }

        public IWizardActionHandler<TModel> OnValidating(Func<ValidationEnvironment<TModel>, bool> ve) {
            return new WizardActionHandler<TModel>(_html);
        }
    }
    #endregion

    #region Wizard Form
    public interface IWizardForm {
      
    }

    public class WizardForm: PairedHtmlTag, IWizardForm {
        private string _aspAction;
        private string _aspController;
        private int _stepNumber;
        public WizardForm(string action, string controller) {
            _aspAction = action;
            _aspController = controller;
        }
        public WizardForm SetStep(int number) {
            _stepNumber= number;
            return this;
        }
        public override HtmlString ToHtmlString(IHtmlHelper html) {
            var sb = new StringBuilder();
            if(!string.IsNullOrEmpty(_aspController)) {
                sb.Append($"<form asp-action='{_aspAction}' asp-controller='{_aspController}'  method='post' class='{CssClass ?? string.Empty}' ");
            } else {
                sb.Append($"<form asp-action='{_aspAction}' method='post' class='{CssClass ?? string.Empty}' ");
            }
            Attributes.ToList().ForEach(attr => sb.Append($"{attr.Key}='{attr.Value}' "));
            sb.Append('>');
            if(_stepNumber == 0) {
                throw new InvalidOperationException();
            } else {
                sb.Append($"<input name='step' type='hidden' value='{_stepNumber}' />");
            }
            if(Children.Count > 0) {
                Children.ForEach(x => sb.Append(x.ToHtmlString(html)));
            }
            if(!string.IsNullOrEmpty(Text)) {
                sb.Append(Text);
            }
            sb.Append($"<input type='submit' value='Submit' />");

            sb.Append("</form>");
            return new HtmlString(sb.ToString());
        }
    }
    #endregion

    public record StepsCollection<TModel>(StepInfo<TModel>[] StepInfo, int Count);

    public record StepInfo<TModel>(int Number, string Name, HtmlBase RenderHtml, StepValidation<TModel> StepValidation);
    public record StepValidation<TModel>(Predicate<TModel>[] Validations);

}
