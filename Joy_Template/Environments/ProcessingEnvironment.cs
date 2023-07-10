namespace Joy_Template.Environments {
    public class ProcessingEnvironment<TModel> : EnvironmentBase<TModel> {
        public FormHandler Form { get; set; }

        public ProcessingEnvironment(TModel model, IFormCollection form) : base(model) {
            Form = new FormHandler(form);
        }
    }
}
