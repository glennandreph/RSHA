using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RSHA.Data;
using RSHA.Models;
using RSHA.Models.ViewModels;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using RSHA.Functions;
using RSHA.Utilities;
using Microsoft.AspNetCore.Authorization;

namespace RSHA.Areas.Mechanic.Controllers
{
    [Authorize(Roles = StaticDetails.MechanicEndUser)]
    [Area("Mechanic")]
    public class ReceivedRequestsController : Controller
    {
        private readonly ApplicationDbContext _db;

        private IConfiguration _configuration { get; }

        [BindProperty]
        public RequestsViewModel RequestsVM { get; set; }

        private readonly UserManager<IdentityUser> _userManager;

        public ReceivedRequestsController(ApplicationDbContext db, UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _db = db;
            RequestsVM = new RequestsViewModel()
            {
                ProblemTypes = _db.ProblemTypes.ToList(),
                Requests = new Models.Requests()
            };
            _userManager = userManager;
            _configuration = configuration;
        }

        // INDEX Action Method      --------------------------------    INDEX
        public async Task<IActionResult> Index()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var mechanic = await _db.Mechanics.FirstOrDefaultAsync(u => u.UserId == id);
            //var mechanic = _db.Mechanics.Where(u => u.UserId == id);

            //var requests = await _db.Requests.FirstOrDefaultAsync(m => m.MechanicAssigned == m.Id);
            var requests = _db.Requests.Where(m => m.MechanicAssigned == mechanic.Id).Include(m => m.ProblemTypes);

            return View(await requests.ToListAsync());
        }

        // GET ACCEPT Action Method      --------------------------------    ACCEPT
        public async Task<IActionResult> Accept(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RequestsVM.Requests = await _db.Requests.Include(m => m.ProblemTypes).SingleOrDefaultAsync(m => m.Id == id);

            if (RequestsVM.Requests == null)
            {
                return NotFound();
            }

            return View(RequestsVM);
            //need to be put in a list or somehting, and possbile have stages of progress. Also comment from mech?
        }

        // POST ACCEPT Action Method   --------------------------------    ACCEPT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Accept(int id)
        {
            if (ModelState.IsValid)
            {
                var requestFromDb = _db.Requests.Where(m => m.Id == RequestsVM.Requests.Id).FirstOrDefault();
                var customerUser = _db.ApplicationUser.Where(u => u.Id == requestFromDb.CustomerId).Single();

                var mechanic = _db.Mechanics.Where(m => m.Id == requestFromDb.MechanicAssigned).Single();
                var mechanicUser = _db.ApplicationUser.Where(u => u.Id == mechanic.UserId).Single();
                //var customerUser = await _userManager.FindByIdAsync(requestFromDb.CustomerId);
                //var customerEmail = await _userManager.GetEmailAsync(customerUser);
                //if no user, try catch some error

                requestFromDb.AcceptedByMechanic = RequestsVM.Requests.AcceptedByMechanic;

                await _db.SaveChangesAsync();

                string textInEmail = @"Hello, " + customerUser.LastName + " Your " + requestFromDb.ProblemTypes.Name + " request on the date " + requestFromDb.RequestScheduledDate + " was accepted by " + mechanic.Name + ".\r" + "Here's the messaged that was sent to " + mechanic.Name + ":\r" + requestFromDb.Message + ".\r\r" + "Have a pleasant day.";

                var sendEmail = new SendEmail();
                sendEmail.RequestNotification(mechanic, mechanicUser, customerUser, requestFromDb, textInEmail, _configuration["RSHAEmail:EmailPassword"]);

                return RedirectToAction(nameof(Index));
            }
            
            return View(RequestsVM);
        }

        // DELETE POST Action Method       --------------------------------    DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Requests request = await _db.Requests.FindAsync(id);

            if (request == null)
            {
                return NotFound();
            }
            else
            {
                var requestFromDb = _db.Requests.Where(m => m.Id == request.Id).FirstOrDefault();
                var customerUser = _db.ApplicationUser.Where(u => u.Id == requestFromDb.CustomerId).Single();

                var mechanic = _db.Mechanics.Where(m => m.Id == requestFromDb.MechanicAssigned).Single();
                var mechanicUser = _db.ApplicationUser.Where(u => u.Id == mechanic.UserId).Single();

                _db.Requests.Remove(request);
                await _db.SaveChangesAsync();

                string textInEmail = @"Hello, " + customerUser.LastName + " Your " + requestFromDb.ProblemTypes.Name + " request on the date " + requestFromDb.RequestScheduledDate + " was NOT accepted or CANCELED by " + mechanic.Name + ".\r" + "Here's the messaged that was sent to " + mechanic.Name + ":\r" + requestFromDb.Message + ".\rPlease send a new request to another mechanic if you still need assistance." + "\r\r" + "Have a pleasant day.";
                var sendEmail = new SendEmail();
                sendEmail.RequestNotification(mechanic, mechanicUser, customerUser, requestFromDb, textInEmail, _configuration["RSHAEmail:EmailPassword"]);

                return RedirectToAction(nameof(Index));
            }
            //just delete request and send notification and email to customer notifying them of cannon accept this request
        }

        // FINISH POST Action Method       --------------------------------    FINISH
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Finish(int id)
        {
            RequestsVM.Requests = await _db.Requests.Include(m => m.ProblemTypes).SingleOrDefaultAsync(m => m.Id == id);

            if (RequestsVM.Requests == null)
            {
                return NotFound();
            }
            else
            {
                var requestFromDb = _db.Requests.Where(m => m.Id == RequestsVM.Requests.Id).Single();

                var customerUser = _db.ApplicationUser.Where(u => u.Id == requestFromDb.CustomerId).Single();
                var mechanic = _db.Mechanics.Where(m => m.Id == requestFromDb.MechanicAssigned).Single();
                var mechanicUser = _db.ApplicationUser.Where(u => u.Id == mechanic.UserId).Single();

                //requestFromDb.Completed = RequestsVM.Requests.Completed;
                requestFromDb.Completed = true;

                await _db.SaveChangesAsync();

                string textInEmail = @"Hello, " + customerUser.LastName + " Your " + requestFromDb.ProblemTypes.Name + " request on the date " + requestFromDb.RequestScheduledDate + " was finished by " + mechanic.Name + ".\r" + "Here's the messaged that was sent to " + mechanic.Name + ":\r" + requestFromDb.Message + ".\r\r" + "Have a pleasant day.";
                var sendEmail = new SendEmail();
                sendEmail.RequestNotification(mechanic, mechanicUser, customerUser, requestFromDb, textInEmail, _configuration["RSHAEmail:EmailPassword"]);

                return RedirectToAction(nameof(Index));
            }
        }

    }
}