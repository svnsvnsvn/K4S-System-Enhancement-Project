using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CS395SI_Spring2023_Group1.Data;
using CS395SI_Spring2023_K4S.Model;
using Microsoft.AspNetCore.Authorization;

namespace CS395SI_Spring2023_Group1.Pages.Registration
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly CS395SI_Spring2023_Group1.Data.CS395SI_Spring2023_Group1Context _context;

        public IndexModel(CS395SI_Spring2023_Group1.Data.CS395SI_Spring2023_Group1Context context)
        {
            _context = context;
        }

        public IList<Spring2023_Group1_Profile_Sys> Spring2023_Group1_Profile_Sys { get;set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string? SearchQuery { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? FilterBy { get; set; }

        public async Task OnGetAsync()
        {
            if (_context.Spring2023_Group1_Profile_Sys == null)
            {
                Spring2023_Group1_Profile_Sys = new List<Spring2023_Group1_Profile_Sys>();
                return;
            }

            var students = _context.Spring2023_Group1_Profile_Sys.AsQueryable();

            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                Spring2023_Group1_Profile_Sys = await students.ToListAsync();
                return;
            }
            
            switch (FilterBy)
            {
                case "Email":
                    students = students.Where(s => s.Email.Contains(SearchQuery));
                    break;
                case "PhoneNum":
                    students = students.Where(s => s.PhoneNum.Contains(SearchQuery));
                    break;
                case "Address":
                    students = students.Where(s => s.Address.Contains(SearchQuery));
                    break;
                case "ApplicationStatus":
                    students = students.Where(s => s.ApplicationStatus.Contains(SearchQuery));
                    break;
                default: // Default is name search
                    students = students.Where(s => s.Name.Contains(SearchQuery));
                    break;
            }

            Spring2023_Group1_Profile_Sys = await students.ToListAsync();
        }

        public IActionResult OnPost(string StudentEmail)
        {
            HttpContext.Session.SetString("studentEmail", StudentEmail);
            return RedirectToPage("../../StudentSchedule/Index");
        }

    }
}


// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.RazorPages;
// using Microsoft.EntityFrameworkCore;
// using CS395SI_Spring2023_Group1.Data;
// using CS395SI_Spring2023_K4S.Model;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.Extensions.Logging; // Add Logging

// namespace CS395SI_Spring2023_Group1.Pages.Registration
// {
//     [Authorize]
//     public class IndexModel : PageModel
//     {
//         private readonly CS395SI_Spring2023_Group1.Data.CS395SI_Spring2023_Group1Context _context;
//         private readonly ILogger<IndexModel> _logger; // Inject Logger

//         public IndexModel(CS395SI_Spring2023_Group1.Data.CS395SI_Spring2023_Group1Context context, ILogger<IndexModel> logger)
//         {
//             _context = context;
//             _logger = logger;
//         }

//         // public IList<Spring2023_Group1_Profile_Sys> Spring2023_Group1_Profile_Sys { get; set; } = default!;
//         public IList<Spring2023_Group1_Profile_Sys> Spring2023_Group1_Profile_Sys { get; set; } = new List<Spring2023_Group1_Profile_Sys>();

//         [BindProperty(SupportsGet = true)]
//         public string? SearchQuery { get; set; }

//         [BindProperty(SupportsGet = true)]
//         public string? FilterBy { get; set; }

//         public async Task OnGetAsync()
//         {
//             try
//             {
//                 if (_context.Spring2023_Group1_Profile_Sys == null)
//                 {
//                     _logger.LogWarning("Database table 'Spring2023_Group1_Profile_Sys' is null.");
//                     Spring2023_Group1_Profile_Sys = new List<Spring2023_Group1_Profile_Sys>();
//                     return;
//                 }

//                 var students = _context.Spring2023_Group1_Profile_Sys.AsQueryable();

//                 _logger.LogInformation("Fetching student profiles. Total records before filter: {Count}", await students.CountAsync());

//                 if (!string.IsNullOrWhiteSpace(SearchQuery))
//                 {
//                     switch (FilterBy)
//                     {
//                         case "Email":
//                             students = students.Where(s => s.Email != null && s.Email.Contains(SearchQuery));
//                             break;
//                         case "PhoneNum":
//                             students = students.Where(s => s.PhoneNum != null && s.PhoneNum.Contains(SearchQuery));
//                             break;
//                         case "Address":
//                             students = students.Where(s => s.Address != null && s.Address.Contains(SearchQuery));
//                             break;
//                         case "ApplicationStatus":
//                             students = students.Where(s => s.ApplicationStatus != null && s.ApplicationStatus.Contains(SearchQuery));
//                             break;
//                         default: // Default is name search
//                             students = students.Where(s => s.Name != null && s.Name.Contains(SearchQuery));
//                             break;
//                     }
//                 }

//                 Spring2023_Group1_Profile_Sys = await students.ToListAsync();

//                 _logger.LogInformation("Students fetched successfully. Total records after filter: {Count}", Spring2023_Group1_Profile_Sys.Count);

//                 // Check for any NULL values
//                 foreach (var student in Spring2023_Group1_Profile_Sys)
//                 {
//                     if (student.Name == null)
//                     {
//                         _logger.LogWarning("NULL value detected in 'Name' field for Student Name: {student.Name}", student.Name);
//                     }
//                     if (student.Email == null)
//                     {
//                         _logger.LogWarning("NULL value detected in 'Email' field for Student Name: {student.Name}", student.Name);
//                     }
//                 }
//             }
//             // catch (SqlNullValueException ex)
//             // {
//             //     _logger.LogError(ex, "A NULL value was encountered in the database query.");
//             //     ModelState.AddModelError(string.Empty, "An unexpected error occurred while retrieving student data. Some records might have missing values.");
//             // }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "An unexpected error occurred while retrieving student data.");
//                 ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");
//             }
//         }

//         public IActionResult OnPost(string StudentEmail)
//         {
//             try
//             {
//                 if (string.IsNullOrWhiteSpace(StudentEmail))
//                 {
//                     _logger.LogWarning("Attempted to store an empty StudentEmail in session.");
//                     ModelState.AddModelError(string.Empty, "Invalid email address.");
//                     return Page();
//                 }

//                 HttpContext.Session.SetString("studentEmail", StudentEmail);
//                 _logger.LogInformation("Student email '{Email}' stored in session.", StudentEmail);

//                 return RedirectToPage("../../StudentSchedule/Index");
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error while storing student email in session.");
//                 ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
//                 return Page();
//             }
//         }
//     }
// }
