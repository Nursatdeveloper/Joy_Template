using Joy_Template.Environments;
using Joy_Template.UiComponents.Base;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Immutable;
using System.Text;

namespace Joy_Template.Wizard {
    public abstract class WizardBase<TModel> : Controller {
        public TModel Model { get; set; }

        public WizardBase(TModel model) {
            Model = model;
        }

        public abstract StepsCollection<TModel> Steps(IWizardBuilder<TModel> builder);

        [HttpGet]
        public IActionResult Index() {
            var steps = Steps(new WizardBuilder<TModel>(Model, HttpContext));

            var htmlHelper = HttpContext.RequestServices.GetRequiredService<IHtmlHelperFactory>().Create();
            var stepInfos = steps.StepInfo.Select(x => {
                var wizardForm = new WizardForm(nameof(Index), "WizardBase")
                    .SetStepInfo(1, steps.Count)
                    .SetFormErrors(x.ValidationErrors)
                    .Append(x.RenderHtml)
                    .ToHtmlString(htmlHelper);
                return new WizardStepSystemInfo(x.Number, x.Name, wizardForm, x.ValidationErrors);
            }).ToArray();

            var wizardSystemModel = new WizardSystemModel(stepInfos, stepInfos.Length, 1);

            return View("WizardView", wizardSystemModel);
        }

        [HttpPost]
        public IActionResult Index(int step, IFormCollection form) {
            var steps = Steps(new WizardBuilder<TModel>(Model, HttpContext));

            var htmlHelper = HttpContext.RequestServices.GetRequiredService<IHtmlHelperFactory>().Create();
            var stepInfos = steps.StepInfo.Select(x => {
                var wizardForm = new WizardForm(nameof(Index), "WizardBase")
                    .SetStepInfo(step, steps.Count)
                    .SetFormErrors(x.ValidationErrors)
                    .Append(x.RenderHtml)
                    .ToHtmlString(htmlHelper);
                return new WizardStepSystemInfo(x.Number, x.Name, wizardForm, x.ValidationErrors);
            }).ToArray();

            var wizardSystemModel = new WizardSystemModel(stepInfos, stepInfos.Length, step);

            return View("WizardView", wizardSystemModel);
        }

    }

    public record WizardSystemModel(WizardStepSystemInfo[] StepInfos, int StepsNumber, int CurrentStep);
    public record WizardStepSystemInfo(int StepNumber, string StepName, HtmlString RenderHtml, ImmutableArray<ValidationError> ValidationErrors);


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
            if (HtmlHelper is IViewContextAware) {
                ((IViewContextAware)HtmlHelper).Contextualize(viewContext);
            }
        }
    }
    public class WizardActionBuilder<TModel> : WizardActionBuilderBase, IWizardActionBuilder<TModel> {
        private HtmlBase _html;
        private RenderEnvironment<TModel> _renderEnv;
        private TModel _model;
        public WizardActionBuilder(TModel model, HttpContext httpContext) : base(httpContext.RequestServices.GetRequiredService<IHtmlHelperFactory>().Create()) {
            _renderEnv = new RenderEnvironment<TModel>(model, httpContext);
            _model = model;
        }

        public IWizardActionHandler<TModel> OnRendering(Func<RenderEnvironment<TModel>, HtmlBase> re) {
            _html = re.Invoke(_renderEnv);
            return new WizardActionHandler<TModel>(_model, _html, _renderEnv.FormCollection);
        }
    }

    public interface IWizardActionHandler<TModel> {
        public HtmlBase Html { get; set; }
        public ImmutableArray<ValidationError> Errors { get; }
        public IWizardActionHandler<TModel> OnValidating(Action<ValidationEnvironment<TModel>> ve);
        public IWizardActionHandler<TModel> OnProcessing(Action<ProcessingEnvironment<TModel>> pe);
    }

    public class WizardActionHandler<TModel> : IWizardActionHandler<TModel> {

        private HtmlBase _html;
        private TModel _model;
        private ValidationEnvironment<TModel> _validationEnv;

        public HtmlBase Html { get => _html; set { _html = value; } }
        public ImmutableArray<ValidationError> Errors => _validationEnv.Errors;

        public WizardActionHandler(TModel model, HtmlBase html, IFormCollection form) {
            Html = html;
            _model = model;
            _validationEnv = new ValidationEnvironment<TModel>(model, form);
        }

        private WizardActionHandler(HtmlBase html, TModel model, ValidationEnvironment<TModel> validationEnv) {
            Html = html;
            _model = model;
            _validationEnv = validationEnv;
        }

        public IWizardActionHandler<TModel> OnValidating(Action<ValidationEnvironment<TModel>> ve) {
            ve.Invoke(_validationEnv);
            return new WizardActionHandler<TModel>(_html, _model, _validationEnv);
        }

        public IWizardActionHandler<TModel> OnProcessing(Action<ProcessingEnvironment<TModel>> pe) {
            throw new NotImplementedException();
        }
    }
    #endregion

    #region Wizard Form
    public interface IWizardForm {

    }

    public class WizardForm : PairedHtmlTag, IWizardForm {
        private string _aspAction;
        private string _aspController;

        private int _totalStepNumber;
        private int _currentStepNumber;
        private int _nextStepNumber;
        private int _previousStepNumber;

        private ImmutableArray<ValidationError> _errors;

        public WizardForm(string action, string controller) {
            _aspAction = action;
            _aspController = controller;
        }

        public WizardForm SetStepInfo(int currentStepNumber, int totalStepNumber) {
            _currentStepNumber = currentStepNumber;
            _nextStepNumber = currentStepNumber + 1;
            _previousStepNumber = currentStepNumber - 1;
            _totalStepNumber = totalStepNumber;
            return this;
        }

        public WizardForm SetFormErrors(ImmutableArray<ValidationError> errors) {
            _errors = errors;
            return this;
        }

        public override HtmlString ToHtmlString(IHtmlHelper html) {
            var sb = new StringBuilder();
            if (!string.IsNullOrEmpty(_aspController)) {
                sb.Append($"<form asp-action='{_aspAction}' asp-controller='{_aspController}'  method='post' class='{CssClass ?? string.Empty}' ");
            } else {
                sb.Append($"<form asp-action='{_aspAction}' method='post' class='{CssClass ?? string.Empty}' ");
            }
            Attributes.ToList().ForEach(attr => sb.Append($"{attr.Key}='{attr.Value}' "));
            sb.Append('>');

            if (_errors.Length > 0) {
                sb.Append("<ul class='text-danger'>");
                foreach (var error in _errors) { sb.Append($"<li>{error.Name}: {error.ErrorMessage}</li>"); }
                sb.Append("</ul>");
            }

            if (_nextStepNumber == 0 || _totalStepNumber == 0) {
                throw new InvalidOperationException();
            } else {
                sb.Append($"<input name='step' type='hidden' value='{_nextStepNumber}' />");
            }
            if (Children.Count > 0) {
                Children.ForEach(x => sb.Append(x.ToHtmlString(html)));
            }
            if (!string.IsNullOrEmpty(Text)) {
                sb.Append(Text);
            }
            var btnText = _currentStepNumber == _totalStepNumber ? "Submit" : "Next";
            sb.Append($"<input type='submit' value='{btnText}' />");

            if (_currentStepNumber > 1) {
                sb.Append("</form>");
                sb.Append($"<form asp-action='{_aspAction}' method='post'>");
                sb.Append($"<input type='hidden' name='step' value='{_previousStepNumber}' />");
                sb.Append("<input type='submit' value='Back'  />");
                sb.Append("</form>");
            }
            return new HtmlString(sb.ToString());
        }
    }
    #endregion

    public record StepsCollection<TModel>(StepInfo<TModel>[] StepInfo, int Count);

    public record StepInfo<TModel>(int Number, string Name, HtmlBase RenderHtml, ImmutableArray<ValidationError> ValidationErrors);

}
