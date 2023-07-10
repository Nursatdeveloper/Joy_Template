using Microsoft.Extensions.Primitives;
using System.Collections.Immutable;

namespace Joy_Template.Environments {
    public record ValidationError(string Name, string ErrorMessage);
    public class ValidationEnvironment<TModel> : EnvironmentBase<TModel> {
        private List<ValidationError> _errors;
        public FormHandler Form { get; set; }
        public ImmutableArray<ValidationError> Errors => _errors.ToImmutableArray();
        public ValidationEnvironment(TModel model, IFormCollection form) : base(model) {
            _errors = new List<ValidationError>();
            Form = new FormHandler(form);
        }

        public void AddError(string name, string error) {
            _errors.Add(new ValidationError(name, error));
        }
    }

    public class FormHandler {
        IFormCollection _form;
        public FormHandler(IFormCollection form) {
            _form = form;
        }
        public string GetStringVal(string fieldName) {
            var value = getValue(fieldName);
            return value.ToString();
        }

        public int GetIntVal(string fieldName) {
            var value = getValue(fieldName);
            return int.Parse(value);
        }

        public DateTime GetDateTimeVal(string fieldName) {
            var value = getValue(fieldName);
            return DateTime.Parse(value);
        }

        private StringValues getValue(string fieldName) {
            var value = _form[fieldName];
            return value;
        }
    }

}
