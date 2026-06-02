using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using protipo_sprint_tareas.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

// Sembrar Roles y Usuarios por defecto
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var dbContext = services.GetRequiredService<ApplicationDbContext>();

        dbContext.Database.EnsureCreated();

        // Crear Roles si no existen
        string[] roleNames = { "Director", "Docente" };
        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
                await roleManager.CreateAsync(new IdentityRole(roleName));
        }

        // Crear usuario Director por defecto
        var directorEmail = "director@sprint.com";
        if (await userManager.FindByEmailAsync(directorEmail) == null)
        {
            var user = new IdentityUser { UserName = directorEmail, Email = directorEmail, EmailConfirmed = true };
            var result = await userManager.CreateAsync(user, "Director123!");
            if (result.Succeeded) await userManager.AddToRoleAsync(user, "Director");
        }

        // Crear usuario Docente por defecto
        var docenteEmail = "docente@sprint.com";
        if (await userManager.FindByEmailAsync(docenteEmail) == null)
        {
            var user = new IdentityUser { UserName = docenteEmail, Email = docenteEmail, EmailConfirmed = true };
            var result = await userManager.CreateAsync(user, "Docente123!");
            if (result.Succeeded) await userManager.AddToRoleAsync(user, "Docente");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error al sembrar roles y usuarios: {ex.Message}");
    }
}

app.Run();
