using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using GHUtils.Services;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace GHUtils.Controllers
{
    public class HomeController : Controller
    {
        private GitHubService _gitHubService;

        public HomeController(GitHubService service)
        {
            _gitHubService = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateLabels(string repositories, string label, string color)
        {
            await _gitHubService.CreateLabelAsync(repositories, label, color.TrimStart('#'), Context.User);
            return View("Index");
        }
    }
}
