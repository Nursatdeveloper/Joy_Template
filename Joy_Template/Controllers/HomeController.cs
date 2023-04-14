using Joy_Template.Models;
using Joy_Template.Sources.Repository;
using Joy_Template.UiComponents.SystemUiComponents;
using Microsoft.AspNetCore.Mvc;
using MVCTemplate.Sources.Repository;
using System.Diagnostics;

namespace Joy_Template.Controllers {
    public class Employee : TbBase {
        public string Name { get; set; }
        public int Age { get; set; }
    }
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepositoryProvider _repositoryProvider;

        public HomeController(ILogger<HomeController> logger, IRepositoryProvider repositoryProvider) {
            _logger = logger;
            _repositoryProvider = repositoryProvider;
        }

        public async Task<IActionResult> Index() {

            var html = new Form("Register", "Home", "post")
                .Append(
                    new Div("text-danger")
                        .WithAttr("asp-validation-summary", "All")
                )
                .Append(
                    new Div("form-group")
                        .Append(new Label(text: "Name").WithAttr("asp-for", "Name"))
                        .Append(new Input(cssClass: "form-control").WithAttr("asp-for", "Name"))
                        .Append(new Span().WithAttr("asp-validation-for", "Name"))
                ).ToHtmlString();

            ViewData["form"] = html;
            var employees = new List<Employee>() {
                new Employee() {Name = "Adam", Age = 18, CreatedAt = DateTime.Now},
                new Employee() {Name = "Sandler", Age = 28, CreatedAt = DateTime.Now},
                new Employee() {Name = "Tom Ford", Age = 38, CreatedAt = DateTime.Now},
            };

            var table = new Table<Employee>(employees)
                .Header(new[] {
                    "Name",
                    "Age",
                    "BirthDate"
                })
                .Column(
                    m => m.Name,
                    m => m.Age,
                    m => m.CreatedAt
                ).ToHtmlString();

            ViewData["table"] = table;
            return View();

        }

        //[HttpPost]
        //public async Task<IActionResult> Register(Company company) {
        //    var tbTest = _repositoryProvider.GetTbTest();
        //    await tbTest.InsertAsync(new TbTest {
        //        Name = "Nursat"
        //    });
        //    if (ModelState.IsValid) {
        //        new CreateOperation<TbCompany, ApplicationDbContext>()
        //            .SetTable(new TbCompany())
        //            .SetValues(
        //                x => x.CEO = company.CEO,
        //                x => x.Bin = company.Bin,
        //                x => x.EmployeeNumber = company.EmployeeNumber,
        //        x => x.Name = company.Name
        //            ).Self(out var createOp);

        //        var result = await createOp.ExecuteAsync(_context);
        //        if (result.IsSuccess) {
        //            return View();
        //        }
        //    }

        //    return Error();

        //}

        public IActionResult Privacy() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}