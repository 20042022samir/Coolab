using CoolApp.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CoolAppProject.Contexts
{
    public class CoolAppDbContext : IdentityDbContext<AppUser>
    {
        public CoolAppDbContext(DbContextOptions<CoolAppDbContext> options) : base(options)
        {

        }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<EventContact> EventContacts { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<CourseContact> CourseContacts { get; set; }
        public DbSet<Teacher> teachers { get; set; }
        public DbSet<StudentFeedback> StudentFeddPacks { get; set; }
        public DbSet<Aboutt> Aboutts { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<BlogUser> UserBlogs { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BlogCategory> BlogCategories { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<PageData> PageDatas { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<EventMessage> EventMessages { get; set; }
        public DbSet<CooursePrice> Priices { get; set; }
        public DbSet<EmailUser> EmailUsers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<HomePageWords> HomeWords { get; set; }
        public DbSet<SeminarsPageWords> SeminarPageWords { get; set; }
        public DbSet<BlogPageWords> BlogWords { get; set; }
        public DbSet<ContactPageWords> ContactWords { get; set; }
        public DbSet<MetaTag> MetaTags { get; set; }
    }
}
