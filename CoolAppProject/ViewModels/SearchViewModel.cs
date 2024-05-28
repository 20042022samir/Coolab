using CoolApp.Core.Entities;

namespace CoolAppProject.ViewModels
{
    public class SearchViewModel
    {
        public List<Course> Courses { get; set; }
        public List<Teacher> Teachers { get; set; }
        public List<Event> Events { get; set; } 
        public List<Blog> Blogs { get; set; }
    }
}
