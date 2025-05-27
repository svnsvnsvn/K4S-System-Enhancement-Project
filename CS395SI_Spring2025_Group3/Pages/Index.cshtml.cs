using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

namespace CS395SI_Spring2023_Group1.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _configuration;

        public string? UserDisplayName { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            UserDisplayName = null;
        }


        public void OnGet()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                string userEmail = User.Identity.Name ?? string.Empty;

                // Set session for nonadmin users
                if (!User.IsInRole("Admin") && HttpContext.Session != null)
                {
                    HttpContext.Session.SetString("studentEmail", userEmail);
                }

                try
                {
                    string connectionString = _configuration.GetConnectionString("CS395SI_Spring2023_Group1Context");
                    if (string.IsNullOrEmpty(connectionString))
                    {
                        throw new InvalidOperationException("Connection string 'CS395SI_Spring2023_Group1Context' not found.");
                    }

                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        // For all users, including admins, try to get their name from the db
                        var command = new SqlCommand(
                            @"SELECT TOP 1 [Name], [ApplicationStatus] 
                    FROM [dbo].[Spring2023_Group1_Profile_Sys] 
                    WHERE [Email] = @Email",
                            connection);
                        command.Parameters.AddWithValue("@Email", userEmail);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Get the users name for display
                                UserDisplayName = reader["Name"].ToString();

                                string? applicationStatus = reader["ApplicationStatus"].ToString();
                                ViewData["IsUserApproved"] = (applicationStatus == "Approved") || User.IsInRole("Admin");
                            }
                            else
                            {
                                // No profile found, use name extraction from email
                                ExtractNameFromEmail(userEmail);
                                ViewData["IsUserApproved"] = User.IsInRole("Admin");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error retrieving user information for {Email}", userEmail);

                    // Always try to extract a name from email rather than using raw email
                    ExtractNameFromEmail(userEmail);

                    // Admins are always approved, others depend on config
                    ViewData["IsUserApproved"] = User.IsInRole("Admin") ? true : true;
                }
            }
        }

        // Helper method to extract name from email
        private void ExtractNameFromEmail(string email)
        {
            if (email.Contains("@"))
            {
                string nameFromEmail = email.Split('@')[0];
                if (!string.IsNullOrEmpty(nameFromEmail))
                {
                    // Convert format like "john.doe" to "John Doe"
                    string[] nameParts = nameFromEmail.Split(new[] { '.', '_', '-' });
                    for (int i = 0; i < nameParts.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(nameParts[i]))
                        {
                            nameParts[i] = char.ToUpper(nameParts[i][0]) + nameParts[i].Substring(1).ToLower();
                        }
                    }
                    UserDisplayName = string.Join(" ", nameParts);
                }
                else
                {
                    UserDisplayName = "User"; // Fallback if no name part can be extracted
                }
            }
            else
            {
                UserDisplayName = "User"; // Fallback if email format is invalid
            }
        }
    }
}