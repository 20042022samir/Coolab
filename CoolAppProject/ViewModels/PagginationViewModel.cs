using CoolApp.Core.Entities;

namespace CoolAppProject.ViewModels
{
    public class PagginationViewModel<W>
    {
        public List<W> Items { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public List<Category>? Catgeories { get; set; }
        public List<Blog>? ExtraBlogs { get; set; }
        public int SubjectId { get;set; }
        public int PriceId { get; set; }

        public int CreatedByUserNumber { get;set; }
        public int CategoryId { get;set; }
        public SeminarsPageWords words { get;set; }
        public BlogPageWords blogWords { get;set; }

        public ContactPageWords contactWords { get;set; }
        
    }
}
