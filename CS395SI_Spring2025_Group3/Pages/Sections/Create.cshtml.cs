using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CS395SI_Spring2023_Group1.Data;
using CS395SI_Spring2023_K4S.Model;

namespace CS395SI_Spring2023_Group1.Pages.Sections
{
    public class CreateModel : PageModel
    {
        private readonly CS395SI_Spring2023_Group1Context _context;

        public CreateModel(CS395SI_Spring2023_Group1Context context)
        {
            _context = context;
        }

        [BindProperty]
        public Spring2024_Group2_Sections Spring2024_Group2_Sections { get; set; } = new Spring2024_Group2_Sections();

        public string ServiceID { get; set; } = string.Empty;
        public string ServiceName { get; set; } = string.Empty;

        public IActionResult OnGet(string serviceID)
        {
            if (string.IsNullOrEmpty(serviceID))
            {
                return RedirectToPage("/Services/Index"); // Redirect if no serviceID is provided
            }

            ServiceID = serviceID;

            // Retrieve the service name from the database
            ServiceName = _context.Spring2023_Group1_Services
                .Where(s => s.ServiceID == serviceID)
                .Select(s => s.ServiceName)
                .FirstOrDefault() ?? "Unknown Service";

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string serviceID)
        {
            if (!ModelState.IsValid || _context.Spring2024_Group2_Sections == null)
            {
                return Page();
            }

            // Assign the service ID before saving
            Spring2024_Group2_Sections.serviceID = serviceID;

            _context.Spring2024_Group2_Sections.Add(Spring2024_Group2_Sections);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index", new { serviceID = serviceID });
        }
    }
}
