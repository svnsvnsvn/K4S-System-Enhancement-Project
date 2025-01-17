using System;
using System.Collections.Generic;
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
        private readonly CS395SI_Spring2023_Group1.Data.CS395SI_Spring2023_Group1Context _context;

        public DetailsModel(CS395SI_Spring2023_Group1.Data.CS395SI_Spring2023_Group1Context context)
        {
            _context = context;
        }

      public Spring2024_Group2_Sections Spring2024_Group2_Sections { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Spring2024_Group2_Sections == null)
            {
                return NotFound();
            }

            var spring2024_group2_sections = await _context.Spring2024_Group2_Sections.FirstOrDefaultAsync(m => m.sectionID == id);
            if (spring2024_group2_sections == null)
            {
                return NotFound();
            }
            else 
            {
                Spring2024_Group2_Sections = spring2024_group2_sections;
            }
            return Page();
        }
    }
}
