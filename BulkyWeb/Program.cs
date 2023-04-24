using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;
using Bulky.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.DataAccess.Repository;
using Microsoft.AspNetCore.Identity.UI.Services;
using BulkyBook.Utility;
using Bulky.Utility;
using Stripe;

var builder = WebApplication.CreateBuilder(args);


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
builder.Services.AddRazorPages();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmailSender, EmailSender>(); 

var app = builder.Build();

app.UseStaticFiles();
StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();
app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");
app.Run();
