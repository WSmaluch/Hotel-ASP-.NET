using Microsoft.EntityFrameworkCore;
using Hotel.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<HotelContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("HotelContext") ?? throw new InvalidOperationException("Connection string 'HotelContext' not found.")));

IronPdf.License.LicenseKey = "IRONSUITE.CEMEX41860.GOSARLAR.COM.27935-DA55CA586E-CMWALMS-MISGQTS2H47N-F7AM3SMJRFZ3-AJ4JO2FQAECZ-JETWZI7GBNEV-4XW7NY7C4OZN-NBFL3G2HIL5U-XHBH7D-TQW7UTHBQZOLUA-DEPLOYMENT.TRIAL-NBFM2J.TRIAL.EXPIRES.02.MAR.2024";

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
