using CoolApp.Core.Entities;

namespace CoolAppProject.ViewModels
{
    public class EventDetailViewModel
    {
        public int EventId { get;set; }
        public Event Event { get; set; }    
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public bool IsStudent { get; set; }
        public bool IsTeacher { get; set; }
        public bool IsEmployee { get;set; }
        public string SchoolUniName { get; set; }
    }
}
