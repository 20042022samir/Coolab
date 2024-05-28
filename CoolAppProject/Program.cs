using CoolApp.Core.Entities;
using CoolAppProject.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.Configure<IdentityOptions>(option =>
{
    option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
    option.Lockout.MaxFailedAccessAttempts = 3;
    option.Lockout.AllowedForNewUsers = true;
    option.Password.RequireDigit = false;
    option.Password.RequiredLength = 3;
    option.User.RequireUniqueEmail = true;
    option.SignIn.RequireConfirmedEmail = false;
	option.Password.RequireNonAlphanumeric = false; 
});


//Database
builder.Services.AddDbContext<CoolAppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

//User
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddDefaultTokenProviders() //<-- it must be added for tokens
    .AddEntityFrameworkStores<CoolAppDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}
app.UseDeveloperExceptionPage();




app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
	endpoints.MapAreaControllerRoute(
		name: "Admin",
		areaName: "Admin",
		pattern: "admin/{controller=Home}/{action=Index}/{id?}"
	);

	// other areas configurations go here 

	endpoints.MapControllerRoute(
			name: "default",
			pattern: "{controller=Home}/{action=Index}/{id?}"
		  );


});




app.Run();
