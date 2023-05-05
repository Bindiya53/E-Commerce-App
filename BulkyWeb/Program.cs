using Bulky.DataAccess.DbInitializer;
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Utility;
using BulkyBook.Utility;
using E_Commerce_App.Bulky.DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Stripe;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
    ));

builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
// builder.Services.AddDefaultIdentity<IdentityUser>()
//     .AddEntityFrameworkStores<ApplicationDbContext>();

    builder.Services.AddIdentity<IdentityUser,IdentityRole>(options =>
{
    //option settings...
})
.AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
// .AddDefaultUI()
builder.Services.ConfigureApplicationCookie(options => {
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});

// builder.Services.AddAuthentication().AddMicrosoftAccount(option => {
//     option.ClientId = "c54394c1-20c6-4fb2-aaa3-a8bfebc8af78";
//     option.ClientSecret = "C1v8Q~~axqOTHWq_GuqNDksTTKa-dUu6lgW68bhU";
// });

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => {
    options.IdleTimeout =  TimeSpan.FromMinutes(100);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddScoped<IDbInitializer, DbInitializer>();

builder.Services.AddRazorPages();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmailSender, EmailSender>(); 

var app = builder.Build();

app.UseStaticFiles();
StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();
app.UseSession();
SeedDatabase();
app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");
app.Run();

void SeedDatabase() {
    using (var scope = app.Services.CreateScope()) {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        dbInitializer.Initialize();
    }
}
