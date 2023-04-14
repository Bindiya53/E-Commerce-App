using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;
using Bulky.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.DataAccess.Repository;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
    ));
// builder.Services.AddDefaultIdentity<IdentityUser>()
//     .AddEntityFrameworkStores<ApplicationDbContext>();

    builder.Services.AddIdentity<IdentityUser,IdentityRole>(options =>
{
    //option settings...
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultUI()
.AddDefaultTokenProviders();
builder.Services.AddRazorPages();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();
app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");
app.Run();
