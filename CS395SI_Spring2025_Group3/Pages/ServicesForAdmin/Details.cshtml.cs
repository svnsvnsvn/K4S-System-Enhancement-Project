﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CS395SI_Spring2023_Group1.Data;
using CS395SI_Spring2023_K4S.Model;

namespace CS395SI_Spring2023_Group1.Pages.ServicesForAdmin
{
    public class DetailsModel : PageModel
    {
        private readonly CS395SI_Spring2023_Group1.Data.CS395SI_Spring2023_Group1Context _context;

        public DetailsModel(CS395SI_Spring2023_Group1.Data.CS395SI_Spring2023_Group1Context context)
        {
            _context = context;
        }

      public Spring2023_Group1_Services Spring2023_Group1_Services { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || _context.Spring2023_Group1_Services == null)
            {
                return NotFound();
            }

            var spring2023_group1_services = await _context.Spring2023_Group1_Services.FirstOrDefaultAsync(m => m.ServiceID == id);
            if (spring2023_group1_services == null)
            {
                return NotFound();
            }
            else 
            {
                Spring2023_Group1_Services = spring2023_group1_services;
            }
            return Page();
        }
    }
}
