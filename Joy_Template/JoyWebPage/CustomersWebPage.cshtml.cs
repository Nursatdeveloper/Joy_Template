using Joy_Template.UiComponents.Version2;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Joy_Template.JoyWebPage {
    public record Customer(string Name, string Iin, DateTime BirthDate);

    [Route("customer")]
    public class CustomersWebPage: JoyForm<Customer> {
        private readonly IJoyUi _joyUi;
        public CustomersWebPage(IJoyUi joyUi) {
            _joyUi= joyUi;
        }
        public override void Process(Customer model) {
            throw new NotImplementedException();
        }

        public override HtmlString Render()
            => _joyUi.Components().Form<Customer>()
                .HtmlAttributes(new FormHtmlAttributeInfo("Index", "POST"))
                .Items(items => items
                    .Add(h => h.LabelFor(m => m.Iin))
                    .Add(h => h.TextBoxFor(m => m.Iin))
                    .Add(h => h.LabelFor(m => m.Name))
                    .Add(h => h.TextBoxFor(m => m.Name))
                    .Add(h => h.LabelFor(m => m.BirthDate))
                    .Add(h => h.TextBoxFor(m => m.BirthDate))
                ).Render();

        public override void Validate(Customer model) {
            throw new NotImplementedException();
        }
    }
}
