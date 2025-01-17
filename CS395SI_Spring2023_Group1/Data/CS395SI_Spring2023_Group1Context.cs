using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CS395SI_Spring2023_K4S.Model;

namespace CS395SI_Spring2023_Group1.Data
{
    public class CS395SI_Spring2023_Group1Context : DbContext
    {
        public CS395SI_Spring2023_Group1Context (DbContextOptions<CS395SI_Spring2023_Group1Context> options)
            : base(options)
        {
        }

        public DbSet<CS395SI_Spring2023_K4S.Model.Spring2023_Group1_Profile_Sys> Spring2023_Group1_Profile_Sys { get; set; } = default!;
        public DbSet<CS395SI_Spring2023_K4S.Model.Spring2023_Group1_Services> Spring2023_Group1_Services { get; set; } = default!;
        public DbSet<CS395SI_Spring2023_K4S.Model.Spring2023_Group1_Scheduling_Form> Spring2023_Group1_Scheduling_Form { get; set; } = default!;
        public DbSet<CS395SI_Spring2023_K4S.Model.Spring2023_Group1_Schedules>? Spring2023_Group1_Schedules { get; set; }
        public DbSet<CS395SI_Spring2023_K4S.Model.Spring2024_Group2_Schedule>? Spring2024_Group2_Schedule { get; set; }
        public DbSet<CS395SI_Spring2023_K4S.Model.Spring2024_Group2_Sections>? Spring2024_Group2_Sections { get; set; }
        public DbSet<CS395SI_Spring2023_K4S.Model.Spring2024_Group2_Session>? Spring2024_Group2_Session { get; set; }
    }
}
