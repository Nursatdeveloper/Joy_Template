using Joy_Template.Environments;
using Joy_Template.UiComponents.Base;

namespace Joy_Template.Wizard_2._0 {
    public interface IWizardRenderAction<TModel> {
        public IWizardValidationAction<TModel> OnRendering(Func<RenderEnvironment<TModel>, HtmlBase> re);
    }

    public class WizardRenderAction<TModel> : IWizardRenderAction<TModel> {
        public IWizardValidationAction<TModel> OnRendering(Func<RenderEnvironment<TModel>, HtmlBase> re) {
            return new WizardValidationAction<TModel>(re);
        }
    }

    public interface IWizardValidationAction<TModel> {
        public IWizardProcessingAction<TModel> OnValidating(Action<ValidationEnvironment<TModel>> ve);
    }

    public class WizardValidationAction<TModel> : IWizardValidationAction<TModel> {
        private Func<RenderEnvironment<TModel>, HtmlBase> _re;
        public WizardValidationAction(Func<RenderEnvironment<TModel>, HtmlBase> re) {
            _re = re;
        }
        public IWizardProcessingAction<TModel> OnValidating(Action<ValidationEnvironment<TModel>> ve) {
            return new WizardProcessingAction<TModel>(_re, ve);
        }
    }

    public interface IWizardProcessingAction<TModel> {
        public IWizardActionHandler<TModel> OnProcessing(Action<ProcessingEnvironment<TModel>> pe);
    }

    public class WizardProcessingAction<TModel> : IWizardProcessingAction<TModel> {
        private Func<RenderEnvironment<TModel>, HtmlBase> _re;
        private Action<ValidationEnvironment<TModel>> _ve;

        public WizardProcessingAction(Func<RenderEnvironment<TModel>, HtmlBase> re, Action<ValidationEnvironment<TModel>> ve) {
            _re = re;
            _ve = ve;
        }
        public IWizardActionHandler<TModel> OnProcessing(Action<ProcessingEnvironment<TModel>> pe) {
            return new WizardActionHandler<TModel>(_re, _ve, pe);
        }
    }

    public interface IWizardActionHandler<TModel> {
        public Func<RenderEnvironment<TModel>, HtmlBase> RenderAction { get; }
        public Action<ValidationEnvironment<TModel>> ValidationAction { get; }
        public Action<ProcessingEnvironment<TModel>> ProcessingAction { get; }
    }

    public class WizardActionHandler<TModel> : IWizardActionHandler<TModel> {
        private Func<RenderEnvironment<TModel>, HtmlBase> _re;
        private Action<ValidationEnvironment<TModel>> _ve;
        private Action<ProcessingEnvironment<TModel>> _pe;

        public Func<RenderEnvironment<TModel>, HtmlBase> RenderAction { get => _re; }
        public Action<ValidationEnvironment<TModel>> ValidationAction { get => _ve; }
        public Action<ProcessingEnvironment<TModel>> ProcessingAction { get => _pe; }

        public WizardActionHandler(Func<RenderEnvironment<TModel>, HtmlBase> re, Action<ValidationEnvironment<TModel>> ve, Action<ProcessingEnvironment<TModel>> pe) {
            _re = re;
            _ve = ve;
            _pe = pe;
        }
    }
}
