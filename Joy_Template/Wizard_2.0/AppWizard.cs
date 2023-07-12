
using Joy_Template.UiComponents.Base;
using Joy_Template.UiComponents.SystemUiComponents;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace Joy_Template.Wizard_2._0 {

    [Route("app/wizard")]
    public class AppWizard : Wizard<AppModel> {
        public AppWizard(IHtmlHelperFactory<AppModel> htmlHelperFactory) : base(AppModel.Empty, htmlHelperFactory) {
        }

        public override WizardStepCollection<AppModel> Steps(IWizardBuilder2<AppModel> builder)
            => builder
            .Step("Step 1", action => action
                .OnRendering(re => {
                    var htmlContent = HtmlHelper.TextBoxFor(m => m.Iin);
                    var div = new TagBuilder("div");
                    div.AddCssClass("card card-body");
                    div.InnerHtml.Append(htmlContent.ToString());
                    var panel = new Div("card card-body");
                    re.Html.LabelFor(nameof(re.Model.Fio), "Fio", panel);
                    re.Html.TextBoxFor(nameof(re.Model.Fio), m => m.Fio, panel);
                    re.Html.LabelFor(nameof(re.Model.Iin), "Iin", panel);
                    re.Html.TextBoxFor(nameof(re.Model.Iin), m => m.Iin, panel);
                    return panel;
                })
                .OnValidating(ve => {
                    var iin = ve.Form.GetStringVal(nameof(ve.Model.Iin));
                    if (string.IsNullOrEmpty(iin)) {
                        ve.AddError("Iin", "Iin cannot be empty");
                    }
                    if (iin.Length != 12) {
                        ve.AddError("Iin", "Length must be 12");
                    }
                })
                .OnProcessing(pe => {
                    pe.Model = pe.Model with {
                        Fio = pe.Form.GetStringVal(nameof(pe.Model.Fio)),
                        Iin = pe.Form.GetStringVal(nameof(pe.Model.Iin))
                    };
                })
            )
            .Step("Step 2", action => action
                .OnRendering(re => {
                    var panel = new Div("card card-body");
                    re.Html.LabelFor(nameof(re.Model.Age), "Age", panel);
                    re.Html.TextBoxFor(nameof(re.Model.Age), m => m.Age, panel);
                    return panel;
                })
                .OnValidating(ve => {
                    var age = ve.Form.GetIntVal(nameof(ve.Model.Age));
                    if (age <= 0) {
                        ve.AddError("Age", "Invalid age!");
                    }
                })
                .OnProcessing(pe => {
                    pe.Model = pe.Model with {
                        Age = pe.Form.GetIntVal(nameof(pe.Model.Age))
                    };
                })
            )
            .Step("Step 3", action => action
                .OnRendering(re => {
                    var panel = new Div("card card-body");
                    re.Html.LabelFor(nameof(re.Model.Birthdate), "Birthdate", panel);
                    re.Html.TextBoxFor(nameof(re.Model.Birthdate), m => m.Birthdate, panel);
                    return panel;
                })
                .OnValidating(ve => {
                    var birthdate = ve.Form.GetDateTimeVal(nameof(ve.Model.Birthdate));

                })
                .OnProcessing(pe => {
                    pe.Model = pe.Model with {
                        Birthdate = pe.Form.GetDateTimeVal(nameof(pe.Model.Birthdate))
                    };
                })
            )
            .Step("Step 4", action => action
                .OnRendering(re => {
                    var panel = new Div("card card-body");
                    re.Html.LabelFor(nameof(re.Model.Fio), "Fio", panel);
                    re.Html.TextBoxFor(nameof(re.Model.Fio), m => m.Fio, panel);
                    re.Html.LabelFor(nameof(re.Model.Iin), "Iin", panel);
                    re.Html.TextBoxFor(nameof(re.Model.Iin), m => m.Iin, panel);
                    re.Html.LabelFor(nameof(re.Model.Age), "Age", panel);
                    re.Html.TextBoxFor(nameof(re.Model.Age), m => m.Age, panel);
                    re.Html.LabelFor(nameof(re.Model.Birthdate), "Birthdate", panel);
                    re.Html.TextBoxFor(nameof(re.Model.Birthdate), m => m.Birthdate, panel);
                    return panel;
                })
                .OnValidating(ve => {
                })
                .OnProcessing(pe => {
                })
            )
            .Build();
    }

    public record AppModel(string Fio, string Iin, int? Age, decimal? Balance, DateTime? Birthdate) {
        public static AppModel Empty => new(null, null, null, null, null);
    }
}
