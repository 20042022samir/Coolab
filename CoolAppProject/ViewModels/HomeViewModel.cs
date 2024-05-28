using CoolApp.Core.Entities;

namespace CoolAppProject.ViewModels
{
	public class HomeViewModel
	{
		public IEnumerable<Teacher> Teachers { get; set; }	
		public IEnumerable<StudentFeedback> feedbacks { get; set; }
		public IEnumerable<Aboutt> abouts { get; set; }
		public List<Slider> sliders { get; set; }
		public IEnumerable<Subject> Subjects { get; set; }
		public IEnumerable<Course> Courses { get; set; }
		public IEnumerable<Blog> Blogs { get; set; }
		public IEnumerable<Event> Events { get; set; }
		public IEnumerable<Comment> Comments { get; set; }
		public Event SpecialEvent { get; set; }
		public Course specialCourse { get;set; }

		public IEnumerable<Event> specialEvents { get; set; }
		public IEnumerable<Course> specialCourses { get; set; }
		public HomePageWords homeWords { get; set; }	
	
	}
}
