using System.Collections.Immutable;

namespace Joy_Template.Wizard_2._0 {
    public interface IWizardBuilder2<TModel> {
        public IWizardStep2<TModel> Step(string step, Func<IWizardRenderAction<TModel>, IWizardActionHandler<TModel>> action);
    }
    public class WizardBuilder2<TModel> : IWizardBuilder2<TModel> {
        public IWizardStep2<TModel> Step(string step, Func<IWizardRenderAction<TModel>, IWizardActionHandler<TModel>> action) {
            var actionHandler = action.Invoke(new WizardRenderAction<TModel>());
            return new WizardStep2<TModel>(new List<WizardStepInfo<TModel>>() {
                new WizardStepInfo<TModel>(1, step, actionHandler)
            });
        }
    }

    public interface IWizardStep2<TModel> {
        public IWizardStep2<TModel> Step(string step, Func<IWizardRenderAction<TModel>, IWizardActionHandler<TModel>> action);

        public WizardStepCollection<TModel> Build();
    }

    public class WizardStep2<TModel> : IWizardStep2<TModel> {
        private List<WizardStepInfo<TModel>> _wizardStepInfoList;
        public WizardStep2(List<WizardStepInfo<TModel>> wizardStepInfoList) {
            _wizardStepInfoList = wizardStepInfoList;
        }

        public WizardStepCollection<TModel> Build() {
            return new WizardStepCollection<TModel>(_wizardStepInfoList.Count, _wizardStepInfoList.ToImmutableArray());
        }

        public IWizardStep2<TModel> Step(string step, Func<IWizardRenderAction<TModel>, IWizardActionHandler<TModel>> action) {
            var actionHandler = action.Invoke(new WizardRenderAction<TModel>());
            _wizardStepInfoList.Add(new WizardStepInfo<TModel>(_wizardStepInfoList.Count + 1, step, actionHandler));
            return new WizardStep2<TModel>(_wizardStepInfoList);
        }
    }



    public record WizardStepCollection<TModel>(int StepCount, ImmutableArray<WizardStepInfo<TModel>> Steps);

    public record WizardStepInfo<TModel>(int Number, string Name, IWizardActionHandler<TModel> ActionHandler);
}
