using Joy_Template.Models;
using Joy_Template.Sources.Repository;
using Microsoft.AspNetCore.Mvc;
using MVCTemplate.Data;
using System.Diagnostics;

namespace Joy_Template.Controllers {
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepositoryProvider _repositoryProvider;

        public HomeController(ILogger<HomeController> logger, IRepositoryProvider repositoryProvider) {
            _logger = logger;
            _repositoryProvider = repositoryProvider;
        }

        public async Task<IActionResult> Index() {
            var tbTest = _repositoryProvider.GetTbTest();
            await tbTest.InsertAsync(new TbTest {
                Name = "Nursat"
            });
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