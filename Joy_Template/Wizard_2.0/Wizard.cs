using Joy_Template.Environments;
using Joy_Template.UiComponents.Base;
using Joy_Template.Wizard;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Immutable;
using System.Text;

namespace Joy_Template.Wizard_2._0 {
    
    public abstract class Wizard<TModel> : Controller {
        private TModel _model;
        public IHtmlHelper<TModel> HtmlHelper { get; set; }
        public Wizard(TModel model, IHtmlHelperFactory<TModel> htmlHelperFactory) {
            _model = model;
            HtmlHelper = htmlHelperFactory.Create();
        }

        public abstract WizardStepCollection<TModel> Steps(IWizardBuilder2<TModel> builder);

        [HttpGet]
        public IActionResult Index() {
            var wizard = new WizardActionUtils<TModel>(HttpContext, Steps(new WizardBuilder2<TModel>()), _model);
            var firstStep = wizard.StepCollection.Steps.First();
            var actionHandler = firstStep.ActionHandler;

            var rEnv = new RenderEnvironment<TModel>(wizard.Model, wizard.HttpContext);
            var renderResult = actionHandler.RenderAction.Invoke(rEnv).ToHtmlString(wizard.HtmlHelper);

            var nextWizardFormModel = new WizardFormModel<TModel>(wizard.Model, 1, 2, wizard.StepCount, 0);
            var wizardFormModelJson = JsonConvert.SerializeObject(nextWizardFormModel);
            var wizardStepViewModel = new WizardStepViewModel(1, firstStep.Name, renderResult, ImmutableArray<ValidationError>.Empty);
            var wizardViewModel = new WizardViewModel(wizardStepViewModel, wizard.GetHeaderViewModel(), 1, wizardFormModelJson, null);

            return View("WizardView2", wizardViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(string wizardModel) {
            var wizard = new WizardActionUtils<TModel>(HttpContext, Steps(new WizardBuilder2<TModel>()), _model);

            var currentStep = wizard.GetCurrentStepInfo();
            var actionHandler = currentStep.ActionHandler;

            var vEnv = new ValidationEnvironment<TModel>(wizard.Model, wizard.FormCollection);

            var isNextAction = wizard.FormModel.CurrentStep > wizard.FormModel.PrevStepNumber;
            if(isNextAction) {
                actionHandler.ValidationAction.Invoke(vEnv);
            }
            if (vEnv.Errors.Length > 0) {
                var rEnv = new RenderEnvironment<TModel>(wizard.Model, wizard.HttpContext);
                var renderResult = actionHandler.RenderAction.Invoke(rEnv).ToHtmlString(wizard.HtmlHelper);
                var nextWizardFormModel = JsonConvert.SerializeObject(new WizardFormModel<TModel>(wizard.Model, wizard.CurrentStep, wizard.CurrentStep+1, wizard.StepCount, wizard.CurrentStep-1));
                var backWizardFormModel = JsonConvert.SerializeObject(new WizardFormModel<TModel>(wizard.Model, wizard.CurrentStep, wizard.CurrentStep-1, wizard.StepCount, wizard.CurrentStep+1));

                var wizardStepViewModel = new WizardStepViewModel(currentStep.Number, currentStep.Name, renderResult, vEnv.Errors);
                var wizardViewModel = new WizardViewModel(wizardStepViewModel, wizard.GetHeaderViewModel(), wizard.CurrentStep, nextWizardFormModel, backWizardFormModel);

                return View("WizardView2", wizardViewModel);
            } else {
                var pEnv = new ProcessingEnvironment<TModel>(wizard.Model, wizard.FormCollection);

                if(isNextAction) {
                    actionHandler.ProcessingAction.Invoke(pEnv);
                }

                var nextStep = wizard.GetNextStepInfo();

                var rEnv = new RenderEnvironment<TModel>(wizard.Model, wizard.HttpContext);
                var renderResult = nextStep.ActionHandler.RenderAction.Invoke(rEnv).ToHtmlString(wizard.HtmlHelper);

                var nextWizardFormModel = JsonConvert.SerializeObject(new WizardFormModel<TModel>(pEnv.Model, nextStep.Number, nextStep.Number + 1, wizard.StepCount, nextStep.Number-1));
                var backWizardFormModel = JsonConvert.SerializeObject(new WizardFormModel<TModel>(pEnv.Model, nextStep.Number, nextStep.Number - 1, wizard.StepCount, nextStep.Number+1));

                var wizardStepViewModel = new WizardStepViewModel(nextStep.Number, nextStep.Name, renderResult, ImmutableArray<ValidationError>.Empty);
                var wizardViewModel = new WizardViewModel(wizardStepViewModel, wizard.GetHeaderViewModel(), nextStep.Number, nextWizardFormModel, backWizardFormModel);

                return View("WizardView2", wizardViewModel);
            }
        }
    }


}
