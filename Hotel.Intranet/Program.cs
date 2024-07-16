using Microsoft.EntityFrameworkCore;
using Hotel.Data;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<HotelContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("HotelContext") ?? throw new InvalidOperationException("Connection string 'HotelContext' not found.")));

IronPdf.License.LicenseKey = "IRONSUITE.YXJ31230.TCCHO.COM.24574-CFC8FD2977-AAQ6YTU-L6DLHSDXSGWW-732LQJW4KW4G-RNHUSUC5HCNJ-KS2J2XZIMFRX-KRJCAURCKAIB-AJ7IJELNNDIW-U3HNB4-TDSAP27Y4H6NEA-DEPLOYMENT.TRIAL-H3WFYP.TRIAL.EXPIRES.06.AUG.2024";

builder.Services.AddScoped<ImgurService>();
builder.Services.AddHttpClient();
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
