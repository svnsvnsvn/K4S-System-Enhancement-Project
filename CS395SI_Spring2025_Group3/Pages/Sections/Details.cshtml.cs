using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CS395SI_Spring2023_Group1.Data;
using CS395SI_Spring2023_K4S.Model;

namespace CS395SI_Spring2023_Group1.Pages.Sections
{
    public class DetailsModel : PageModel
    {
        private readonly CS395SI_Spring2023_Group1Context _context;

        public DetailsModel(CS395SI_Spring2023_Group1Context context)
        {
            _context = context;
        }

        public Spring2024_Group2_Sections Spring2024_Group2_Sections { get; set; } = default!;
        public string ServiceName { get; set; } = string.Empty;
        public string ServiceID { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Spring2024_Group2_Sections == null)
            {
                return NotFound();
            }

            var section = await _context.Spring2024_Group2_Sections
                .FirstOrDefaultAsync(m => m.sectionID == id);

            if (section == null)
            {
                return NotFound();
            }

            // Assign retrieved section
            Spring2024_Group2_Sections = section;
            ServiceID = section.serviceID;

            // Fetch the corresponding service name
            ServiceName = await _context.Spring2023_Group1_Services
                .Where(s => s.ServiceID == section.serviceID)
                .Select(s => s.ServiceName)
                .FirstOrDefaultAsync() ?? "Unknown Service";

            return Page();
        }
    }
}
