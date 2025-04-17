using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CS395SI_Spring2023_Group1.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CS395SI_Spring2023_Group1.Pages.InstructorDashboard
{
    [Authorize(Roles = "Instructor,Admin")]
    public class IndexModel : PageModel
    {
        private readonly CS395SI_Spring2023_Group1.Data.CS395SI_Spring2023_Group1Context _context;
        private readonly UserManager<IdentityUser> _userManager;

        public IndexModel(
            CS395SI_Spring2023_Group1.Data.CS395SI_Spring2023_Group1Context context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public string InstructorName { get; set; }
        public int StudentsCount { get; set; }
        public int PendingApplicationsCount { get; set; }
        public int ApprovedApplicationsCount { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            InstructorName = user.UserName;

            // Get actual counts from your database using your specific entities
            // These are counting from the Spring2023_Group1_Profile_Sys table
            StudentsCount = _context.Spring2023_Group1_Profile_Sys.Count();
            PendingApplicationsCount = _context.Spring2023_Group1_Profile_Sys
                .Count(p => p.ApplicationStatus == "Pending");
            ApprovedApplicationsCount = _context.Spring2023_Group1_Profile_Sys
                .Count(p => p.ApplicationStatus == "Approved");

            return Page();
        }
    }
}