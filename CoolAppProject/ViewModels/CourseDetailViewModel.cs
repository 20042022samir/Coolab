using CoolApp.Core.Entities;

namespace CoolAppProject.ViewModels
{
    public class CourseDetailViewModel
    {

        public bool IsTeacher { get; set; }
        public bool IsStudent { get;set; }  
        public string SchoolUniName { get; set; }
        public Course? Course { get; set; }
        public string? Message { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public int CourseId { get; set; }
        public string FullName { get; set; }
        public string CommentUserName { get; set; }
        public string CommentUserMessage { get; set; }
    }
}
