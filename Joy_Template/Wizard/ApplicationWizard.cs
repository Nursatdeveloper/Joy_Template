using Joy_Template.UiComponents.SystemUiComponents;
using Microsoft.AspNetCore.Mvc;

namespace Joy_Template.Wizard {
    [Route("application/wizard")]
    public class ApplicationWizard : WizardBase<ApplicationModel> {

        public ApplicationWizard() : base(ApplicationModel.Empty) {
        }

        public override StepsCollection<ApplicationModel> Steps(IWizardBuilder<ApplicationModel> builder) {
            return builder
                .Step("Step 1", action => action
                    .OnRendering(re => {
                        var panel = new Div("card card-body");
                        re.Html.LabelFor(nameof(re.Model.FirstName), "Firstname", panel);
                        re.Html.TextBoxFor(nameof(re.Model.FirstName), m => m.FirstName, panel);

                        re.Html.LabelFor(nameof(re.Model.LastName), "Lastname", panel);
                        re.Html.TextBoxFor(nameof(re.Model.LastName), m => m.LastName, panel);

                        re.Html.LabelFor(nameof(re.Model.Iin), "Iin", panel);
                        re.Html.TextBoxFor(nameof(re.Model.Iin), m => m.Iin, panel);
                        return panel;
                    })
                    .OnValidating(ve => {
                        var firstName = ve.Form.GetStringVal(nameof(ve.Model.FirstName));
                        var lastName = ve.Form.GetStringVal(nameof(ve.Model.LastName));
                        var iin = ve.Form.GetStringVal(nameof(ve.Model.Iin));
                        if (string.IsNullOrEmpty(iin)) {
                            ve.AddError("Iin", "Some validationError");
                            if (iin.Length != 12) {
                                ve.AddError("Iin", "Lenght of iin must be 12");
                            }
                        }
                    })
                    .OnProcessing(pe => {
                        pe.Model = pe.Model with {
                            FirstName = pe.Form.GetStringVal(nameof(pe.Model.FirstName)),
                            LastName = pe.Form.GetStringVal(nameof(pe.Model.LastName)),
                            Iin = pe.Form.GetStringVal(nameof(pe.Model.Iin))
                        };
                    })
                )
                .Step("Step 2", action => action
                    .OnRendering(re => {
                        return new Div("card card-body border")
                            .Append(new Div()
                                .Append(new Label("form-label", "Name"))
                                .Append(new Input("text", "form-control", value: "Hello from step 2"))
                            )
                            .Append(new Div()
                                .Append(new Label("form-label", "Age"))
                                .Append(new Input("text", "form-control"))
                            );
                    })
                    .OnValidating(ve => {
                        ve.AddError("Name", "Some validationError From step 2");

                    })
                )
                .Step("Step 3", action => action
                    .OnRendering(re => {
                        return new Div("card card-body border")
                            .Append(new Div()
                                .Append(new Label("form-label", "Name"))
                                .Append(new Input("text", "form-control", value: "Hello from step 3"))
                            )
                            .Append(new Div()
                                .Append(new Label("form-label", "Age"))
                                .Append(new Input("text", "form-control"))
                            );
                    })
                    .OnValidating(ve => {
                        ve.AddError("Name", "Some validationError From step 3");

                    })
                ).Build();
        }
    }

    public record ApplicationModel(string FirstName, string LastName, string Iin, int? Age, decimal? Balance, DateTime? BirthDate) {
        public static ApplicationModel Empty => new(null, null, null, null, null, null);
    }


}
