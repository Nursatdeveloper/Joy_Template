namespace Joy_Template.Wizard {
    public interface IWizardBuilder<TModel> {
        public IWizardStep<TModel> Step(string step, Func<IWizardActionBuilder<TModel>, IWizardActionHandler<TModel>> action);
    }

    public class WizardBuilder<TModel>: IWizardBuilder<TModel> {

        public TModel Model { get; set; }
        public HttpContext Context { get; set; }
        private const int stepNumber = 1;

        public WizardBuilder(TModel model, HttpContext context) {
            Model = model;
            Context = context;
        }
        public IWizardStep<TModel> Step(string step, Func<IWizardActionBuilder<TModel>, IWizardActionHandler<TModel>> action) {
            var actionResult = action.Invoke(new WizardActionBuilder<TModel>(Model, Context));
            return new WizardStep<TModel>(Model, new List<StepInfo<TModel>>() {
                new StepInfo<TModel>(stepNumber, step, actionResult.Html, new StepValidation<TModel>(null))
            }, stepNumber + 1, Context);
        }
    }

    public interface IWizardStep<TModel> {
        public IWizardStep<TModel> Step(string step, Func<IWizardActionBuilder<TModel>, IWizardActionHandler<TModel>> action);
        public StepsCollection<TModel> Build();
    }

    public class WizardStep<TModel>: IWizardStep<TModel> {
        public List<StepInfo<TModel>> StepInfos { get; set; }
        public TModel Model { get; set; }
        public HttpContext Context { get; set; }
        private int _stepNumber;
        public WizardStep(TModel model, List<StepInfo<TModel>> stepInfos, int stepNumber, HttpContext context) {
            StepInfos = stepInfos;
            Model = model;
            _stepNumber = stepNumber;
            Context = context;
        }

        public StepsCollection<TModel> Build() {
            return new StepsCollection<TModel>(StepInfos.ToArray(), StepInfos.Count);
        }

        public IWizardStep<TModel> Step(string step, Func<IWizardActionBuilder<TModel>, IWizardActionHandler<TModel>> action) {
            var actionResult = action.Invoke(new WizardActionBuilder<TModel>(Model, Context));
            StepInfos.Add(new StepInfo<TModel>(_stepNumber, step, actionResult.Html, new StepValidation<TModel>(null)));
            return new WizardStep<TModel>(Model, StepInfos, _stepNumber + 1, Context);
        }
    }
}
