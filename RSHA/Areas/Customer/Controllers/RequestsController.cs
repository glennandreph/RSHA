using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RSHA.Data;
using RSHA.Models;
using RSHA.Models.ViewModels;
using RSHA.Utilities;

namespace RSHA.Areas.Customer.Controllers
{
    //[Authorize(Roles = StaticDetails.CustomerEndUser)]
    [Area("Customer")]
    public class RequestsController : Controller
    {
        private readonly ApplicationDbContext _db;
        //private int PageSize = 2;
        private IConfiguration _configuration { get; }

        [BindProperty]
        public RequestsViewModel RequestsVM { get; set; }

        public RequestsController(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            RequestsVM = new RequestsViewModel()
            {
                ProblemTypes = _db.ProblemTypes.ToList(),
                Requests = new Models.Requests()
            };
            _configuration = configuration;
        }

        // INDEX Action Method      --------------------------------    INDEX
        //public async Task<IActionResult> Index(int productPage = 1)
        public async Task<IActionResult> Index(int productPage = 1)
        {
            //StringBuilder param = new StringBuilder();
            //param.Append("/Customer/Requests?productsPage=:");

            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var requests = _db.Requests.Where(u => u.CustomerId == id).Include(m => m.ProblemTypes);
            //IQueryable<Models.Requests> 
            //var requests = _db.Requests.Where(u => u.CustomerId == id).ToList();

            //var count = requests.Count();

            //requests = requests.OrderBy(r => r.RequestCreated)
            //    .Skip((productPage - 1) * PageSize)
            //    .Take(PageSize).ToList();

            //PagingInfo pagingInfo = new PagingInfo
            //{
            //    CurrentPage = productPage,
            //    RequestsPerPage = PageSize,
            //    TotalRequests = count,
            //    urlParam = param.ToString()
            //};

            return View(await requests.ToListAsync());
            //return View(requests);
        }

        // GET Create Action Method --------------------------------    CREATE
        public IActionResult Create(int id)
        {
            var mech = _db.Mechanics.Find(id);
            string mechName = mech.Name;
             
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _db.ApplicationUser.Find(userId);
            
            List<string> userProfileInfo = new List<string>();
            userProfileInfo.AddRange(new List<string>
            {
                new string(user.CarModel),
                new string(user.CarLicensePlate),
                new string(user.PhoneNumber),
                new string(user.FirstName),
                new string(user.LastName)
            });

            ViewBag.userProfileInfo = userProfileInfo;
            ViewBag.mechName = mechName;
            ViewBag.id = id;

            return View(RequestsVM);
        }

        // POST Create Action Method
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePOST(int? id)
        {
            if (!ModelState.IsValid)
            {
                //return View(RequestsVM);
                return RedirectToAction("Create", id);
            }
            
            RequestsVM.Requests.RequestScheduledDate = RequestsVM.Requests.RequestScheduledDate
                .AddHours(RequestsVM.Requests.RequestScheduledTime.Hour)
                .AddMinutes(RequestsVM.Requests.RequestScheduledTime.Minute);

            RequestsVM.Requests.RequestCreated = DateTime.Now;

            if (RequestsVM.Requests.RequestScheduledDate.AddMinutes(1) < RequestsVM.Requests.RequestCreated)
            {
                return RedirectToAction("Create", id);
            }

            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            RequestsVM.Requests.CustomerId = customerId;

            _db.Requests.Add(RequestsVM.Requests);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET Edit Action Method   --------------------------------    EDIT
        public async Task<IActionResult> Edit(int? id)
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

            var mech = _db.Mechanics.Find(RequestsVM.Requests.MechanicAssigned);
            string mechName = mech.Name;

            ViewBag.mechName = mechName;

            return View(RequestsVM);
        }

        // POST Edit Action Method   --------------------------------    EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {
            if (ModelState.IsValid)
            {
                var requestFromDb = _db.Requests.Where(m => m.Id == RequestsVM.Requests.Id).FirstOrDefault();

                RequestsVM.Requests.RequestScheduledDate = RequestsVM.Requests.RequestScheduledDate
                        .AddHours(RequestsVM.Requests.RequestScheduledTime.Hour)
                        .AddMinutes(RequestsVM.Requests.RequestScheduledTime.Minute);

                if (RequestsVM.Requests.RequestScheduledDate.AddMinutes(1) < DateTime.Now)
                {
                    return RedirectToAction("Edit", id);
                }

                requestFromDb.FirstName = RequestsVM.Requests.FirstName;
                requestFromDb.LastName = RequestsVM.Requests.LastName;
                requestFromDb.PhoneNumber = RequestsVM.Requests.PhoneNumber;
                requestFromDb.Location = RequestsVM.Requests.Location;
                requestFromDb.CarLicensePlate = RequestsVM.Requests.CarLicensePlate;
                requestFromDb.CarModel = RequestsVM.Requests.CarModel;
                requestFromDb.ProblemId = RequestsVM.Requests.ProblemId;
                requestFromDb.Message = RequestsVM.Requests.Message;
                requestFromDb.RequestScheduledDate = RequestsVM.Requests.RequestScheduledDate;

                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(RequestsVM);
        }

        // GET Details Action Method    --------------------------------    DETAILS
        public async Task<IActionResult> Details(int? id)
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

            var mech = _db.Mechanics.Find(RequestsVM.Requests.MechanicAssigned);
            string mechName = mech.Name;

            ViewBag.mechName = mechName;

            return View(RequestsVM);
        }

        // GET Delete Action Method --------------------------------        DELETE
        public async Task<IActionResult> Delete(int? id)
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

            var mech = _db.Mechanics.Find(RequestsVM.Requests.MechanicAssigned);
            string mechName = mech.Name;

            ViewBag.mechName = mechName;

            return View(RequestsVM);
        }

        // POST Delete Action Method --------------------------------        DELETE
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
                _db.Requests.Remove(request);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
        }

        // GET Locate Action Method --------------------------------        LOCATE
        public async Task<IActionResult> Locate(string searchName = null, string searchCity = null, int? searchPostalCode = null, string searchState = null)
        {
            var mechanicList = await _db.Mechanics.ToListAsync();

            if (searchName != null) { mechanicList = mechanicList.Where(m => m.Name.ToLower().Contains(searchName.ToLower())).ToList(); }
            if (searchCity != null) { mechanicList = mechanicList.Where(m => m.City.ToLower().Contains(searchCity.ToLower())).ToList(); }
            if (searchPostalCode != null) { mechanicList = mechanicList.Where(m => m.PostalCode == searchPostalCode).ToList(); }
            if (searchState != null) { mechanicList = mechanicList.Where(m => m.State.ToLower().Contains(searchState.ToLower())).ToList(); }

            return View(mechanicList);
        }

        public async Task<IActionResult> Gmap()
        {
            ViewBag.apikey = _configuration["RSHAGmap:Apikey"];

            var mechanics = await _db.Mechanics.ToListAsync();

            //string[] mechanicHouses = new string[] { "{ lat: 59, lng: 10 }", "{ lat: 59, lng: 11 }" };
            //string[] mechanicHouses = new string[5];

            //foreach (var item in mechanics)
            //{
            //    string itemInString = "{ lat: " + item.Latitude + ", lng: " + item.Longitude + " }*" + item.Name;
            //    mechanicHouses.Append(itemInString);
            //}

            return View(mechanics);
        }


    }
}