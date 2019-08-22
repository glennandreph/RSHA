using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RSHA.Data;
using RSHA.Models;
using RSHA.Models.ViewModels;
using RSHA.Utilities;

namespace RSHA.Areas.Mechanic.Controllers
{
    [Authorize(Roles = StaticDetails.MechanicEndUser)]
    [Area("Mechanic")]
    public class FeedbackController : Controller
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public FeedbacksViewModel FeedbacksVM { get; set; }

        public FeedbackController(ApplicationDbContext db)
        {
            _db = db;
            FeedbacksVM = new FeedbacksViewModel()
            {
                Feedbacks = _db.Feedbacks.ToList(),
                Feedback = new Models.Feedbacks()
            };
        }

        // INDEX Action Method      --------------------------------    INDEX
        public async Task<IActionResult> Index()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var feedbacks = _db.Feedbacks.Where(f => f.UserId == id);
            FeedbacksVM.Feedbacks = await Task.FromResult(feedbacks.ToList());

            //List<Feedbacks> test = await Task.FromResult(feedbacks.ToList());
            //FeedbacksVM.Feedbacks = test;

            //return View(await feedbacks.ToListAsync());
            return View(FeedbacksVM);
        }

        // POST Create Action Method
        [HttpPost, ActionName("Index")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePOST()
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }

            FeedbacksVM.Feedback.FeedbackCreated = DateTime.Now;

            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            FeedbacksVM.Feedback.UserId = id;
            FeedbacksVM.Feedback.SenderName = User.Identity.Name;

            _db.Feedbacks.Add(FeedbacksVM.Feedback);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        // GET Bug Action Method
        public IActionResult BugReport()
        {
            var bugReport = new BugRapports();

            return View(bugReport);
        }


        // POST Bug Action Method
        [HttpPost, ActionName("BugReport")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BugReportPOST(BugRapports bugReport)
        {
            if (!ModelState.IsValid)
            {
                return View(bugReport);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _db.ApplicationUser.Find(userId);
            bugReport.UserID = user.Id;

            _db.BugRapports.Add(bugReport);
            await _db.SaveChangesAsync();

            //return RedirectToAction("Index", "Home");
            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }
    }
}