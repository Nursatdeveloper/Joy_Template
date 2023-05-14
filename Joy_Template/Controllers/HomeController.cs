using Joy_Template.Data.Tables;
using Joy_Template.Models;
using Joy_Template.Sources.Repository;
using Joy_Template.Sources.Users.Ops;
using Joy_Template.UiComponents.Base;
using Joy_Template.UiComponents.SystemUiComponents;
using Joy_Template.UiComponents.SystemUiComponents.Table;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCTemplate.Data;
using System.Diagnostics;
using static Program;

namespace Joy_Template.Controllers
{
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IHtmlHelper _htmlHelper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepositoryProvider _repositoryProvider;
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        public HomeController(IDbContextFactory<ApplicationDbContext> contextFactory, ILogger<HomeController> logger, ApplicationDbContext context, IHtmlHelper htmlHelper, IHttpContextAccessor httpContextAccessor, IRepositoryProvider repositoryProvider) {
            _logger = logger;
            _context = context;
            _htmlHelper = htmlHelper;
            _httpContextAccessor = httpContextAccessor;
            _repositoryProvider = repositoryProvider;
            _contextFactory = contextFactory;
        }

        public async Task<IActionResult> Index() {
            var createUserOpRender = new RegisterUserOp(_htmlHelper, _httpContextAccessor.HttpContext)
                .OnSubmit(new SubmitArgs(nameof(HomeController), nameof(Index), "Submit"))
                .GetHtml();
            ViewData["render"] = createUserOpRender;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(RegisterUserOpModel model) {
            new RegisterUserOp(_htmlHelper, _httpContextAccessor.HttpContext)
                .SetModel(model, _contextFactory.CreateDbContext());
            return View(model);
        }
        public async Task<IActionResult> Test(IFormCollection formCollection) {
            new FormComponent(_htmlHelper, _httpContextAccessor.HttpContext)
                .OnSubmit(new SubmitArgs(nameof(HomeController), nameof(Test), "Submit"))
                .Render(() =>
                    new Div("form-group")
                        .Append(new Label(text: "FirstName").WithAttr("asp-for", "FirstName"))
                        .Append(new Input(cssClass: "form-control").WithAttr("name", "FirstName"))
                ).ToHtmlString(out var html);
            ViewData["render"] = html;
            return View(nameof(Index));
        }

        public async Task<IActionResult> Users(IFormCollection formCollection) {
            var context = _contextFactory.CreateDbContext();

            new SearchTable<TbUser>(context)
                .OnSearch(new SubmitArgs(nameof(HomeController), nameof(Users), "Search"))
                .Filter(new FilterData<TbUser>[] {
                    new("Email", nameof(TbUser.Email), FieldType.Text, SearchType.Equals, f => f.Email),
                    new("ИИН", nameof(TbUser.Iin), FieldType.Text, SearchType.Equals, f => f.Iin)
                })
                .Header(new[] {
                    "Email", "ИИН"
                })
                .Presentation(
                    c => c.Email,
                    c => c.Iin
                )
                .Print(_htmlHelper, HttpContext, formCollection, out var panel);

            ViewData["render"] = panel;
            return View();
        }

        public IActionResult Privacy() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}