using Joy_Template.UiComponents.Base;
using Joy_Template.UiComponents.Version2;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Encodings.Web;

namespace Joy_Template.Controllers {
    public record MyModel(string Fio, string Iin, int Age);
    [Route("component")]
    public class ComponentController: Controller {
        private readonly IHtmlHelper htmlHelper;
        private readonly IJoyUi _joyUI;
        public ComponentController(IHtmlHelperFactory htmlHelperFactory, IJoyUi joyUI) {
            htmlHelper = htmlHelperFactory.Create();
            _joyUI = joyUI;
        }
        [HttpGet]
        public IActionResult Index() {
            var html = _joyUI.Components()
                .Form<MyModel>()
                    .HtmlAttributes(new FormHtmlAttributeInfo(nameof(Index), "POST"))
                    .Items(items => items
                        .Add(h => h.LabelFor(m => m.Iin))
                        .Add(h => h.TextBoxFor(m => m.Iin))
                        .Add(h => h.LabelFor(m => m.Fio))
                        .Add(h => h.TextBoxFor(m => m.Fio))
                    ).Render();

            ViewData["html"] = html;
            return View();
        }

        [HttpPost]
        public IActionResult Index(MyModel model) {
            { }
            return View();
        }
    }


}
