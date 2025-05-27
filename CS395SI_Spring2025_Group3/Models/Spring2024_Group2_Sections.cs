using System.ComponentModel.DataAnnotations;

namespace CS395SI_Spring2023_K4S.Model
{
    public class Spring2024_Group2_Sections
    {
        [Key]
        public int sectionID { get; set; }
        public string serviceID {  get; set; }
        public string? serviceName {  get; set; } 
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string weekDay { get; set; }
        public TimeSpan? startTime { get; set; }
        public TimeSpan? endTime { get; set; }
        public string status { get; set; }
    }
}
