using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;

namespace Joy_Template.JoyWebPage {
    public abstract class JoyForm<TModel> : Controller {
        public abstract HtmlString Render();
        public abstract void  Validate(TModel model);
        public abstract void Process(TModel model);

        [HttpGet]
        public IActionResult Index() {
            ViewData["html"] = Render();
            return View();
        }

        [HttpPost]
        public IActionResult Index(TModel model) {
            { }
            return View();
        }
    }
}
