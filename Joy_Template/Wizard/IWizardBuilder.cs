namespace Joy_Template.Wizard {
    public interface IWizardBuilder<TModel> {
        public IWizardStep<TModel> Step(string step, Func<IWizardActionBuilder<TModel>, IWizardActionHandler<TModel>> action);
    }

    public class WizardBuilder<TModel> : IWizardBuilder<TModel> {

        public TModel Model { get; set; }
        public HttpContext Context { get; set; }
        private const int stepNumber = 1;

        public WizardBuilder(TModel model, HttpContext context) {
            Model = model;
            Context = context;
        }
        //public IWizardStep<TModel> Step(string step, Func<IWizardActionBuilder<TModel>, IWizardActionHandler<TModel>> action) {
        //    var actionResult = action.Invoke(new WizardActionBuilder<TModel>(Model, Context));
        //    var renderResult = actionResult.Html;
        //    var validationResult = actionResult.Errors;
        //    var processingResult = actionResult.Model;
        //    return new WizardStep<TModel>(Model, new List<StepInfo<TModel>>() {
        //        new StepInfo<TModel>(stepNumber, step, renderResult, validationResult, processingResult, action)
        //    }, stepNumber + 1, Context);
        //}
        public IWizardStep<TModel> Step(string step, Func<IWizardActionBuilder<TModel>, IWizardActionHandler<TModel>> action) {
            return new WizardStep<TModel>(Model, new List<StepInfo<TModel>>() {
                new StepInfo<TModel>(stepNumber, step, action)
            }, stepNumber + 1, Context);
        }

    }

    public interface IWizardStep<TModel> {
        public IWizardStep<TModel> Step(string step, Func<IWizardActionBuilder<TModel>, IWizardActionHandler<TModel>> action);
        public StepsCollection<TModel> Build();
    }



    public class WizardStep<TModel> : IWizardStep<TModel> {
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

        //public IWizardStep<TModel> Step(string step, Func<IWizardActionBuilder<TModel>, IWizardActionHandler<TModel>> action) {
        //    var actionResult = action.Invoke(new WizardActionBuilder<TModel>(Model, Context));
        //    var renderResult = actionResult.Html;
        //    var validationResult = actionResult.Errors;
        //    var processingResult = actionResult.Model;
        //    StepInfos.Add(new StepInfo<TModel>(_stepNumber, step, renderResult, validationResult, processingResult, action));
        //    return new WizardStep<TModel>(Model, StepInfos, _stepNumber + 1, Context);
        //}

        public IWizardStep<TModel> Step(string step, Func<IWizardActionBuilder<TModel>, IWizardActionHandler<TModel>> action) {
            StepInfos.Add(new StepInfo<TModel>(_stepNumber, step, action));
            return new WizardStep<TModel>(Model, StepInfos, _stepNumber + 1, Context);
        }
    }
    public record StepsCollection<TModel>(StepInfo<TModel>[] StepInfo, int Count);

    //public record StepInfo<TModel>(int Number, string Name, HtmlBase RenderHtml, ImmutableArray<ValidationError> ValidationErrors, TModel Model, Func<IWizardActionBuilder<TModel>, IWizardActionHandler<TModel>> Action);
    public record StepInfo<TModel>(int Number, string Name, Func<IWizardActionBuilder<TModel>, IWizardActionHandler<TModel>> GetActionFunc);

}
