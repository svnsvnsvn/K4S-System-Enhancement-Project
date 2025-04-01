// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.RazorPages;
// using Microsoft.AspNetCore.Authorization;

// namespace CS395SI_Spring2023_Group1.Pages
// {
//     [Authorize]
//     public class IndexModel : PageModel
//     {
//         private readonly ILogger<IndexModel> _logger;

//         public IndexModel(ILogger<IndexModel> logger)
//         {
//             _logger = logger;
//         }

//         public void OnGet()
//         {
//             if (!HttpContext.User.IsInRole("Admin"))
//             {
//                 HttpContext.Session.SetString("studentEmail", HttpContext.User.Identity.Name);
                
//             }
//         }
//     }
// }
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient; // Correct SQL Client namespace

namespace CS395SI_Spring2023_Group1.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _configuration;

        public string? UserDisplayName { get; set; } // Make nullable

        public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            UserDisplayName = null; // Initialize to null
        }

        public void OnGet()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                string userEmail = User.Identity.Name ?? string.Empty;
                
                if (!User.IsInRole("Admin"))
                {
                    // Null check for session
                    if (HttpContext.Session != null)
                    {
                        HttpContext.Session.SetString("studentEmail", userEmail);
                    }
                    
                    // Get user info from the database
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
                            // Find the actual table and column names in your database
                            // Based on your screenshots, this might be the ASP.NET Users table or a custom table
                            // Adjust this query to match your actual database schema
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
                                    // Get the user's name for display
                                    UserDisplayName = reader["Name"].ToString();
                                    
                                    // Check if user is approved
                                    string? applicationStatus = reader["ApplicationStatus"].ToString();
                                    ViewData["IsUserApproved"] = (applicationStatus == "Approved");
                                }
                                else
                                {
                                    UserDisplayName = userEmail; // Default to email if not found
                                    ViewData["IsUserApproved"] = false;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error retrieving user information for {Email}", userEmail);
                        UserDisplayName = userEmail; // Default to email on error
                        
                        // Extract name from email as a fallback
                        if (userEmail.Contains("@"))
                        {
                            string nameFromEmail = userEmail.Split('@')[0];
                            if (!string.IsNullOrEmpty(nameFromEmail))
                            {
                                // Capitalize first letter and make the rest lowercase
                                UserDisplayName = char.ToUpper(nameFromEmail[0]) + nameFromEmail.Substring(1).ToLower();
                            }
                        }
                        
                        ViewData["IsUserApproved"] = true; // Default to approved on error for better user experience
                    }
                }
                else
                {
                    // For admin users, just use their email as display name
                    UserDisplayName = userEmail;
                    ViewData["IsUserApproved"] = true; // Admins are always "approved"
                }
            }
        }
    }
}