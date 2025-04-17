using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CS395SI_Spring2023_Group1.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class UserRolesModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRolesModel(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public SelectList Users { get; set; }
        public SelectList Roles { get; set; }

        public class InputModel
        {
            [Required]
            public string UserId { get; set; }

            [Required]
            public string RoleName { get; set; }
        }

        public void OnGet()
        {
            PopulateSelectLists();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                PopulateSelectLists();
                return Page();
            }

            var user = await _userManager.FindByIdAsync(Input.UserId);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "User not found");
                PopulateSelectLists();
                return Page();
            }

            var result = await _userManager.AddToRoleAsync(user, Input.RoleName);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = $"User assigned to role {Input.RoleName} successfully.";
                return RedirectToPage();
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            PopulateSelectLists();
            return Page();
        }

        private void PopulateSelectLists()
        {
            Users = new SelectList(_userManager.Users.OrderBy(u => u.Email), "Id", "Email");
            Roles = new SelectList(_roleManager.Roles.OrderBy(r => r.Name), "Name", "Name");
        }
    }
}