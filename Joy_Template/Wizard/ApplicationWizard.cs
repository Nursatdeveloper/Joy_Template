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
                        return "<div></div>";
                    })
                    .OnValidating(ve => {

                    })
                )
                .Step("Step 2", action => action
                    .OnRendering(re => {

                    })
                    .OnValidating(ve => {

                    })
                ).Build();
        }
    }

    public record ApplicationModel(string Name, int Age);

}
