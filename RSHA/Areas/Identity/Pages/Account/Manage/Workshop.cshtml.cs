using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RSHA.Data;
using RSHA.Models;

namespace RSHA.Areas.Identity.Pages.Account.Manage
{
    public class WorkshopModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public WorkshopModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public Mechanics Mechanic { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (id == null)
            {
                return NotFound($"Unable to load user with ID '{id}'.");
            }

            var mechanic = await _db.Mechanics.FirstOrDefaultAsync(m => m.UserId == id);
            if (mechanic == null)
            {
                StatusMessage = "This user is not bound to any mechanic workshop. Please input your workshop details.";

                return Page();
            }

            Mechanic = new Mechanics
            {
                Name = mechanic.Name,
                Street = mechanic.Street,
                City = mechanic.City,
                PostalCode = mechanic.PostalCode,
                State = mechanic.State
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (id == null)
            {
                return NotFound($"Unable to load user with ID '{id}'.");
            }

            var mechanic = await _db.Mechanics.FirstOrDefaultAsync(m => m.UserId == id);

            //var mechanic = new Mechanics();

            if (mechanic == null)
            {
                var mechanicEdit = new Mechanics();

                mechanicEdit.Name = Mechanic.Name;
                mechanicEdit.Street = Mechanic.Street;
                mechanicEdit.City = Mechanic.City;
                mechanicEdit.PostalCode = Mechanic.PostalCode;
                mechanicEdit.State = Mechanic.State;
                mechanicEdit.UserId = id;

                _db.Mechanics.Add(mechanicEdit);
                await _db.SaveChangesAsync();

                return RedirectToPage();
            }
            else
            {
                mechanic.Name = Mechanic.Name;
                mechanic.Street = Mechanic.Street;
                mechanic.City = Mechanic.City;
                mechanic.PostalCode = Mechanic.PostalCode;
                mechanic.State = Mechanic.State;  
            }

            
            await _db.SaveChangesAsync();

            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }

    }
}