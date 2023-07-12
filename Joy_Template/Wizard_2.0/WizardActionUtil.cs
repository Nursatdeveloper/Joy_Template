using Joy_Template.Environments;
using Joy_Template.UiComponents.Base;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Immutable;

namespace Joy_Template.Wizard_2._0 {
    public class WizardActionUtils<TModel> {
        public IFormCollection FormCollection { get; private set; }
        public HttpContext HttpContext { get; private set; }
        public WizardStepCollection<TModel> StepCollection { get; private set; }
        public IHtmlHelper HtmlHelper { get; set; }
        public TModel Model { get; set; }
        public int StepCount { get; private set; }
        public int CurrentStep { get; private set; }
        public WizardFormModel<TModel> FormModel { get; set; }
        public WizardActionUtils(HttpContext httpContext, WizardStepCollection<TModel> stepCollection, TModel model) {
            FormCollection = httpContext.Request.HasFormContentType ? httpContext.Request.Form : null;
            HttpContext = httpContext;
            StepCollection = stepCollection;
            HtmlHelper = httpContext.RequestServices.GetRequiredService<IHtmlHelperFactory<TModel>>().Create();
            Model = httpContext.Request.HasFormContentType ? getFormModel(httpContext).Model : model;
            StepCount = stepCollection.StepCount;
            CurrentStep = getCurrentStep(httpContext);
            FormModel = httpContext.Request.HasFormContentType ? getFormModel(httpContext) : null;
        }

        public WizardHeaderViewModel[] GetHeaderViewModel() {
            return StepCollection.Steps.Select(x => new WizardHeaderViewModel(x.Number, x.Name)).ToArray();
        }

        public WizardStepInfo<TModel> GetCurrentStepInfo() {
            return StepCollection.Steps.First(x => x.Number == CurrentStep);
        }
        public WizardStepInfo<TModel> GetNextStepInfo() {
            return StepCollection.Steps.First(x => x.Number == FormModel.NextStepNumber);
        }

        private WizardFormModel<TModel> getFormModel(HttpContext httpContext) {
            var form = httpContext.Request.Form;
            var wizardModel = JsonConvert.DeserializeObject<WizardFormModel<TModel>>(form["wizardModel"]);
            if(wizardModel is null) {
                throw new InvalidOperationException();
            } else {
                return wizardModel;
            }
        }
        private int getCurrentStep(HttpContext httpContext) {
            if(httpContext.Request.HasFormContentType) {
                var form = httpContext.Request.Form;
                var wizardModel = JsonConvert.DeserializeObject<WizardFormModel<TModel>>(form["wizardModel"]);
                if(wizardModel is null) {
                    throw new InvalidOperationException();
                } else {
                    return wizardModel.CurrentStep;
                }
            } else {
                return 1;
            }
        }

    }
    public record WizardFormModel<TModel>(TModel Model, int CurrentStep, int NextStepNumber, int TotalSteps, int PrevStepNumber);
    public record WizardViewModel(WizardStepViewModel WizardStep, WizardHeaderViewModel[] WizardHeader, int CurrentStep, string NextWizardFormModel, string BackWizardFormModel);
    public record WizardStepViewModel(int Number, string Name, HtmlString RenderHtml, ImmutableArray<ValidationError> Errors);
    public record WizardHeaderViewModel(int Number, string Name);
}
