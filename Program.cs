using CDR_Worship.Data;
using CDR_Worship.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using CDR_Worship.Services.Interfaces;
using CDR_Worship.Services;
using Microsoft.AspNetCore.Identity.UI.Services;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
var connectionString = DataUtility.GetConnectionString(builder.Configuration) ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<AppUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddDefaultUI()
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<ISongDocumentService, SongDocumentService>();
builder.Services.AddScoped<IChordDocumentService, ChordDocumentService>();
builder.Services.AddScoped<IScheduledSongsService, ScheduledSongsService>();
builder.Services.AddScoped<ICalendarService, CalendarService>();
builder.Services.AddScoped<IEmailSender, EmailService>();
// builder.Services.AddScoped<ISmsService, SmsService>();

// Bind the email settings to the EmailSettings object
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddControllersWithViews();



var app = builder.Build();


var scope = app.Services.CreateScope();
await DataUtility.ManageDataAsync(scope.ServiceProvider);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
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
app.MapRazorPages();

app.Run();
