using Joy_Template.UiComponents.Base;
using Joy_Template.UiComponents.SystemUiComponents;

namespace Joy_Template.Environments {
    public class RenderEnvironment<TModel> : EnvironmentBase<TModel> {
        public HtmlRenderer<TModel> Html { get; set; }
        public IFormCollection FormCollection { get; set; }
        public RenderEnvironment(TModel model, HttpContext httpContext) : base(model) {
            Html = new HtmlRenderer<TModel>(model);
        }
    }

    public class HtmlRenderer<TModel> {
        private TModel _model;
        private List<string> _fieldNames;
        public string[] FieldNames => _fieldNames.ToArray();
        public HtmlRenderer(TModel model) {
            _model = model;
            _fieldNames = new List<string>();
        }
        public void LabelFor(string fieldName, string displayText, PairedHtmlTag panel) {
            panel.Append(new Label("form-label", displayText) { FieldName = fieldName });
        }

        public void TextBoxFor(string fieldName, Func<TModel, string> getValue, PairedHtmlTag panel) {
            var valueString = getValue(_model);
            panel.Append(new Input("text", "form-control", valueString, fieldName));
            _fieldNames.Add(fieldName);
        }
        public void TextBoxFor(string fieldName, Func<TModel, int?> getValue, PairedHtmlTag panel) {
            var valueInt = getValue(_model);
            panel.Append(new Input("number", "form-control", valueInt?.ToString(), fieldName));
            _fieldNames.Add(fieldName);
        }

        public void TextBoxFor(string fieldName, Func<TModel, DateTime?> getValue, PairedHtmlTag panel) {
            var valueDateTime = getValue(_model);
            panel.Append(new Input("date", "form-control", valueDateTime?.ToString(), fieldName));
            _fieldNames.Add(fieldName);
        }
    }
}
