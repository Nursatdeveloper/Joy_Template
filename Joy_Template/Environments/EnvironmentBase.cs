namespace Joy_Template.Environments {
    public class EnvironmentBase<TModel> {
        public TModel Model { get; set; }
        public EnvironmentBase(TModel model) {
            Model = model;
        }
    }

    public enum EnvironmentAction {
        Render,
        Validate,
        Process
    }
}
