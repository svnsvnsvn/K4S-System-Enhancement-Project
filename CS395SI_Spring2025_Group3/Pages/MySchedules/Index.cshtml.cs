﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CS395SI_Spring2023_Group1.Data;
using CS395SI_Spring2023_K4S.Model;

namespace CS395SI_Spring2023_Group1.Pages.MySchedules
{
    public class IndexModel : PageModel
    {
        private readonly CS395SI_Spring2023_Group1.Data.CS395SI_Spring2023_Group1Context _context;

        public IndexModel(CS395SI_Spring2023_Group1.Data.CS395SI_Spring2023_Group1Context context)
        {
            _context = context;
        }

        public IList<Spring2024_Group2_Schedule> Spring2024_Group2_Schedule { get;set; } = default!;

        public async Task OnGetAsync()
        {
            string studentEmail = HttpContext.Session.GetString("studentEmail");

            if (!string.IsNullOrEmpty(studentEmail))
            {
                Spring2024_Group2_Schedule = await _context.Spring2024_Group2_Schedule
                    .Where(s => s.StudentEmail == studentEmail)
                    .ToListAsync();
            }




        }
    }
}
