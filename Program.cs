using HospitalApp.Data;
using HospitalApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<HospitalDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => {
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 3;
})
    .AddEntityFrameworkStores<HospitalDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Проверка подключения к БД при старте
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    try
    {
        var context = services.GetRequiredService<HospitalDbContext>();
        logger.LogInformation("Проверка подключения к БД...");
        if (await context.Database.CanConnectAsync())
        {
            logger.LogInformation("Успешное подключение к БД: {Database}", context.Database.GetDbConnection().Database);
            
            // Проверка наличия таблиц Identity
            try
            {
                var tablesExist = await context.Database.ExecuteSqlRawAsync("SELECT 1 FROM AspNetUsers");
                logger.LogInformation("Таблицы Identity обнаружены.");
            }
            catch (Exception)
            {
                logger.LogWarning("Таблицы Identity (AspNetUsers) не найдены. Пытаюсь создать их автоматически...");
                
                var scriptPath = Path.Combine(Directory.GetCurrentDirectory(), "docs", "tech", "identity_tables.sql");
                if (File.Exists(scriptPath))
                {
                    var sqlContent = await File.ReadAllTextAsync(scriptPath);
                    // Удаляем USE и GO, разбиваем на команды
                    var commands = sqlContent
                        .Replace("USE [Hospital_BukarevBedin_2ISP11-41]", "")
                        .Replace("GO", ";")
                        .Split(';', StringSplitOptions.RemoveEmptyEntries);

                    foreach (var command in commands)
                    {
                        if (!string.IsNullOrWhiteSpace(command))
                        {
                            await context.Database.ExecuteSqlRawAsync(command);
                        }
                    }
                    logger.LogInformation("Таблицы Identity успешно созданы!");
                }
                else
                {
                    logger.LogError("Файл identity_tables.sql не найден по пути: {Path}", scriptPath);
                }
            }
        }
        else
        {
            logger.LogError("Не удалось подключиться к БД. Проверьте строку подключения в appsettings.json.");
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Ошибка при проверке подключения к БД.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Более детальные ошибки
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
