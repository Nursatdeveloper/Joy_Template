using Joy_Template.Data.Tables;
using Joy_Template.Models;
using Joy_Template.Sources.Base;
using Joy_Template.Sources.Repository;
using Joy_Template.Sources.Users.Ops;
using Joy_Template.UiComponents.SystemUiComponents;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCTemplate.Data;
using MVCTemplate.Sources.Repository;
using System.Diagnostics;

namespace Joy_Template.Controllers {
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
                .SetAction(nameof(HomeController), nameof(Index))
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

        public async Task<IActionResult> Users() {
            var users = _contextFactory.CreateDbContext().TbUsers.ToList();
            var panel = new Div(CssClass.Card)
                .Append(new Table<TbUser>(users)
                .Filter(new[] {
                    new FilterData(nameof(TbUser.Email), FieldType.Text),
                    new FilterData(nameof(TbUser.Iin), FieldType.Text),
                    new FilterData(nameof(TbUser.BirthDate), FieldType.DateTime)
                })
                .Header(new[] {
                    "Id",
                    "Iin",
                    "Email",
                    "Name",
                    "BirthDate"
                })
                .Column(
                    c => c.Id,
                    c => c.Iin,
                    c => c.Email,
                    c => c.Firstname,
                    c => c.BirthDate
                )).ToHtmlString(_htmlHelper);
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