using Microsoft.Extensions.Primitives;
using System.Collections.Immutable;

namespace Joy_Template.Environments {
    public record ValidationError(string Name, string ErrorMessage);
    public class ValidationEnvironment<TModel> : EnvironmentBase<TModel> {
        private List<ValidationError> _errors;
        public FormHandler<TModel> Form { get; set; }
        public ImmutableArray<ValidationError> Errors => _errors.ToImmutableArray();
        public ValidationEnvironment(TModel model, IFormCollection form) : base(model) {
            _errors = new List<ValidationError>();
            Form = new FormHandler<TModel>(form, model);
        }

        public void AddError(string name, string error) {
            _errors.Add(new ValidationError(name, error));
        }
    }

    public class FormHandler<TModel> {
        private IFormCollection _form;
        private TModel _model;
        public FormHandler(IFormCollection form, TModel model) {
            _form = form;
            _model = model;
        }
        public string GetVal(Func<TModel, string> getVal) {
            return getVal.Invoke(_model);
        }
        public string? GetValOrNull(Func<TModel, string?> getVal) {
            return getVal.Invoke(_model);
        }
        public DateTime GetVal(Func<TModel, DateTime> getVal) {
            return getVal.Invoke(_model);
        }
        public DateTime? GetValOrNull(Func<TModel, DateTime?> getVal) {
            return getVal.Invoke(_model);
        }
        public int GetVal(Func<TModel, int> getVal) {
            return getVal.Invoke(_model);
        }
        public int? GetValOrNull(Func<TModel, int?> getVal) {
            return getVal.Invoke(_model);
        }
        public decimal GetVal(Func<TModel, decimal> getVal) {
            return getVal.Invoke(_model);
        }
        public decimal? GetValOrNull(Func<TModel, decimal?> getVal) {
            return getVal.Invoke(_model);
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
