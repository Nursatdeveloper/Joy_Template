using Joy_Template.Environments;
using Joy_Template.UiComponents.Base;
using Joy_Template.Wizard;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Joy_Template.Wizard_2._0 {
    public abstract class Wizard<TModel> : Controller {
        private TModel _model;
        public Wizard(TModel model) {
            _model = model;
        }

        public abstract WizardStepCollection<TModel> Steps(IWizardBuilder2<TModel> builder);

        [HttpGet]
        public IActionResult Index() {
            var htmlHelper = HttpContext.RequestServices.GetRequiredService<IHtmlHelperFactory>().Create();

            var steps = Steps(new WizardBuilder2<TModel>());
            var firstStep = steps.Steps.First();
            var actionHandler = firstStep.ActionHandler;

            var rEnv = new RenderEnvironment<TModel>(_model, HttpContext);
            var renderResult = actionHandler.RenderAction.Invoke(rEnv);
            var modelEncrypted = JsonConvert.SerializeObject(_model);

            var wizardForm = new WizardForm(nameof(Index), "Wizard")
                .SetStepInfo(1, steps.StepCount)
                .SetModel(modelEncrypted)
                .Append(renderResult)
                .ToHtmlString(htmlHelper);

            var wizardStepViewModel = new WizardStepViewModel(1, firstStep.Name, wizardForm, Array.Empty<string>());
            var wizardHeader = steps.Steps.Select(x => new WizardHeaderViewModel(x.Number, x.Name)).ToArray();
            var wizardViewModel = new WizardViewModel(wizardStepViewModel, 1, wizardHeader);

            return View("WizardView2", wizardViewModel);
        }

        [HttpPost]
        public IActionResult Index(int step, IFormCollection form) {
            var htmlHelper = HttpContext.RequestServices.GetRequiredService<IHtmlHelperFactory>().Create();
            _model = JsonConvert.DeserializeObject<TModel>(form["wizardModel"]);
            var steps = Steps(new WizardBuilder2<TModel>());
            var currentStep = steps.Steps.FirstOrDefault(x => x.Number == step);
            var actionHandler = currentStep.ActionHandler;

            var vEnv = new ValidationEnvironment<TModel>(_model, form);

            actionHandler.ValidationAction.Invoke(vEnv);
            if (vEnv.Errors.Length > 0) {
                var rEnv = new RenderEnvironment<TModel>(_model, HttpContext);
                var renderResult = actionHandler.RenderAction.Invoke(rEnv);
                var modelEncrypted = JsonConvert.SerializeObject(_model);

                var wizardForm = new WizardForm(nameof(Index), "Wizard")
                    .SetStepInfo(step, steps.StepCount)
                    .SetFormErrors(vEnv.Errors)
                    .SetModel(modelEncrypted)
                    .Append(renderResult)
                    .ToHtmlString(htmlHelper);

                var wizardStepViewModel = new WizardStepViewModel(currentStep.Number, currentStep.Name, wizardForm, Array.Empty<string>());
                var wizardHeader = steps.Steps.Select(x => new WizardHeaderViewModel(x.Number, x.Name)).ToArray();
                var wizardViewModel = new WizardViewModel(wizardStepViewModel, step, wizardHeader);

                return View("WizardView2", wizardViewModel);
            } else {
                var pEnv = new ProcessingEnvironment<TModel>(_model, form);
                actionHandler.ProcessingAction.Invoke(pEnv);
                _model = pEnv.Model;

                var nextStep = steps.Steps.FirstOrDefault(x => x.Number == step + 1);

                var rEnv = new RenderEnvironment<TModel>(_model, HttpContext);
                var renderResult = nextStep.ActionHandler.RenderAction.Invoke(rEnv);
                var modelEncrypted = JsonConvert.SerializeObject(_model);

                var wizardForm = new WizardForm(nameof(Index), "Wizard")
                    .SetStepInfo(step + 1, steps.StepCount)
                    .SetModel(modelEncrypted)
                    .Append(renderResult)
                    .ToHtmlString(htmlHelper);

                var wizardStepViewModel = new WizardStepViewModel(currentStep.Number, currentStep.Name, wizardForm, Array.Empty<string>());
                var wizardHeader = steps.Steps.Select(x => new WizardHeaderViewModel(x.Number, x.Name)).ToArray();
                var wizardViewModel = new WizardViewModel(wizardStepViewModel, step + 1, wizardHeader);

                return View("WizardView2", wizardViewModel);
            }
        }
    }
    public record WizardViewModel(WizardStepViewModel WizardStep, int CurrentStep, WizardHeaderViewModel[] WizardHeader);
    public record WizardStepViewModel(int Number, string Name, HtmlString RenderHtml, string[] Errors);
    public record WizardHeaderViewModel(int Number, string Name);

}
