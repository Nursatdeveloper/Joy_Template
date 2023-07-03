using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;

namespace Joy_Template.Wizard {
    public record ApplicationModel(string Name, int Age);

    [Route("application/wizard")]
    public class ApplicationWizard: WizardBase<ApplicationModel> {
        public ApplicationWizard() : base(new ApplicationModel("Nursat", 1)) {
        }

        public override void Steps(IWizardBuilder<ApplicationModel> builder) {
            builder
            .Step("Step 1", action => action.
                
            )
        }
    }
    public abstract class WizardBase<TModel> : Controller{
        public TModel Model { get; set; }
        public WizardBase(TModel model) {
            Model = model;
        }

        public abstract void Steps(IWizardBuilder<TModel> builder);

        [HttpGet]
        public IActionResult Index(int step) {
            Console.WriteLine("Test");
            ViewData["step1"] = "<div>Step1</div>";
            ViewData["step2"] = "<div>Step1</div>";
            return new ViewResult() {
                ViewName= "WizardView"
            };
        }
    }

    public interface IWizardBuilder<TModel> {
        public IWizardStep<TModel> Step(string step, IWizardActionBuilder<TModel> action);
    }

    public class WizardBuilder<TModel>: IWizardBuilder<TModel> {
        public RenderEnvironment<TModel> RenderEnv { get; set; }
        public ValidationEnvironment<TModel> ValidationEnv { get; set; }
        private const int stepNumber = 1;

        public WizardBuilder(TModel model) {
            RenderEnv = new RenderEnvironment<TModel>(model);
            ValidationEnv = new ValidationEnvironment<TModel>(model);
        }
        public IWizardStep<TModel> Step(string step, IWizardActionBuilder<TModel> action) {
            return new WizardStep<TModel>(new List<StepInfo<TModel>>() {
                new StepInfo<TModel>(stepNumber, step, "", new StepValidation<TModel>(null))
            }, stepNumber + 1);
        }
    }

    public interface IWizardStep<TModel> {
        public IWizardStep<TModel> Step(string step, IWizardActionBuilder<TModel> action);
        public StepsCollection<TModel> Build();
    }

    public class WizardStep<TModel>: IWizardStep<TModel> {
        public List<StepInfo<TModel>> StepInfos { get; set; }
        private int _stepNumber;
        public WizardStep(List<StepInfo<TModel>> stepInfos, int stepNumber) {
            StepInfos = stepInfos;
            _stepNumber = stepNumber;
        }

        public StepsCollection<TModel> Build() {
            throw new NotImplementedException();
        }

        public IWizardStep<TModel> Step(string step, IWizardActionBuilder<TModel> action) {
            return new WizardStep<TModel>(new List<StepInfo<TModel>>() {
                new StepInfo<TModel>(_stepNumber, step, "", new StepValidation<TModel>(null))
            }, _stepNumber + 1);
        }
    }


    #region Environments
    public class EnvironmentBase<TModel> {
        public TModel Model { get; set; }
        public EnvironmentBase(TModel model) {
            Model = model;
        }
    }
    public class RenderEnvironment<TModel>: EnvironmentBase<TModel> {
        public RenderEnvironment(TModel model) : base(model) {
        }
    }

    public class ValidationEnvironment<TModel>: EnvironmentBase<TModel> {
        public ValidationEnvironment(TModel model) : base(model) {
        }
    }
    #endregion

    #region Actions
    public interface IWizardActionBuilder<TModel> {
        public IWizardActionHandler<TModel> OnRendering(Func<RenderEnvironment<TModel>, string> re);
    }

    public interface IWizardActionHandler<TModel> {
        public IWizardActionHandler<TModel> OnValidating(Func<ValidationEnvironment<TModel>, bool> ve);   
    }
    #endregion

    public record StepsCollection<TModel>(StepInfo<TModel> StepInfo, int Count);

    public record StepInfo<TModel>(int Number, string Name, string RenderHtml, StepValidation<TModel> StepValidation);
    public record StepValidation<TModel>(Predicate<TModel>[] Validations);

}
