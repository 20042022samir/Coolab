using CoolApp.Core.Entities;

namespace CoolAppProject.ViewModels
{
    public class BlogViewModel
    {
        public IEnumerable<Blog> Blogs { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}
