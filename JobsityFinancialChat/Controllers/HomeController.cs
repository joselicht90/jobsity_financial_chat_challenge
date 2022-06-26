using Josbity.Models;
using JobsityFinancialChat.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace JobsityFinancialChat.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public readonly JobsityContext _context;
        public readonly UserManager<JobsityUser> _userManager;

        public HomeController(JobsityContext context, UserManager<JobsityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser != null)
            {
                if (User.Identity?.IsAuthenticated != null && User.Identity.IsAuthenticated)
                {
                    ViewBag.CurrentUserName = currentUser.UserName;
                }

                var messages = await _context.Messages.OrderByDescending(m => m.When).Take(50).ToListAsync();

                return View(messages);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Message message)
        {
            if (ModelState.IsValid && User.Identity != null)
            {
                message.UserName = User.Identity.Name;
                var sender = await _userManager.GetUserAsync(User);
                message.UserID = sender.Id;
                await _context.Messages.AddAsync(message);
                await _context.SaveChangesAsync();
                var messages = await _context.Messages.OrderByDescending(m => m.When).Take(50).ToListAsync();
                return Ok();
            }
            return Error();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}