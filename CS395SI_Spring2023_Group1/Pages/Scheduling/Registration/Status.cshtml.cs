using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CS395SI_Spring2023_Group1.Data;
using CS395SI_Spring2023_K4S.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace CS395SI_Spring2023_Group1.Pages.Registration
{
    [Authorize]  // Only require the user to be logged in
    public class StatusModel : PageModel
    {
        private readonly CS395SI_Spring2023_Group1.Data.CS395SI_Spring2023_Group1Context _context;
        private readonly UserManager<IdentityUser> _userManager;

        public StatusModel(
            CS395SI_Spring2023_Group1.Data.CS395SI_Spring2023_Group1Context context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Spring2023_Group1_Profile_Sys Profile { get; set; }
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                // Get current user
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToPage("/Identity/Account/Login");
                }

                // Find the profile by email
                Profile = await _context.Spring2023_Group1_Profile_Sys
                    .FirstOrDefaultAsync(m => m.Email == user.Email);

                if (Profile == null)
                {
                    // No profile found, redirect to create page
                    return RedirectToPage("./Create");
                }

                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = "An error occurred: " + ex.Message;
                return Page();
            }
        }
    }
}