using Joy_Template.UiComponents.SystemUiComponents;
using Microsoft.AspNetCore.Mvc;

namespace Joy_Template.Wizard {
    [Route("application/wizard")]
    public class ApplicationWizard : WizardBase<ApplicationModel> {
        public ApplicationWizard() : base(new ApplicationModel("Nursat", 1)) {
        }

        public override StepsCollection<ApplicationModel> Steps(IWizardBuilder<ApplicationModel> builder) {
            return builder
                .Step("Step 1", action => action
                    .OnRendering(re => {
                        return new Div("card card-body border")
                            .Append(new Div()
                                .Append(new Label("form-label", "Name"))
                                .Append(new Input("text", "form-control"))
                            )
                            .Append(new Div()
                                .Append(new Label("form-label", "Age"))
                                .Append(new Input("text", "form-control"))
                            );
                    })
                    .OnValidating(ve => {
                        return true;
                    })
                )
                .Step("Step 2", action => action
                    .OnRendering(re => {
                        return new Div("container")
                            .Append(new Input("text","form-group", value: "hello 2"))
                            .Append(new Div("container", "Hello from step 2"));
                    })
                    .OnValidating(ve => {
                        return true;
                    })
                ).Build();
        }
    }

    public record ApplicationModel(string Name, int Age);

}
