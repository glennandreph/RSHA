using Microsoft.AspNetCore.Mvc;
using RSHA.Data;
using RSHA.Models;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RSHA.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ProfileController(ApplicationDbContext db)
        {
            _db = db;
        }

        //Get Index: Show profile page with present data about user
        public async Task<IActionResult> Index()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userFromDb = await _db.ApplicationUser.FindAsync(id);

            if (userFromDb == null)
            {
                return RedirectToAction("Profile", "Home");
            }

            return View(userFromDb);
        }

        //Post Index
        [HttpPost, ActionName("Index")]
        [ValidateAntiForgeryToken]
        public IActionResult IndexPOST (string id, ApplicationUser applicationUser)
        {
            if (id != applicationUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                ApplicationUser userFromDb = _db.ApplicationUser.Where(u => u.Id == id).FirstOrDefault();
                userFromDb.FirstName = applicationUser.FirstName;
                userFromDb.LastName = applicationUser.LastName;
                userFromDb.PhoneNumber = applicationUser.PhoneNumber;
                userFromDb.Email = applicationUser.Email;
                userFromDb.CarLicensePlate = applicationUser.CarLicensePlate;
                userFromDb.CarModel = applicationUser.CarModel;

                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }



            return View(applicationUser);
        }
    }
}