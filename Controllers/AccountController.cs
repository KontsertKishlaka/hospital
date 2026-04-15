using HospitalApp.Data;
using HospitalApp.Models;
using HospitalApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HospitalApp.Controllers;

public class AccountController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly HospitalDbContext _context;

    public AccountController(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        HospitalDbContext context)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _context = context;
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        if (ModelState.IsValid)
        {
            try
            {
                // 1. Пробуем стандартный вход через Identity
                var result = await _signInManager.PasswordSignInAsync(model.Login, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToLocal(returnUrl);
                }
            }
            catch (Exception ex)
            {
                // Если таблицы Identity не созданы, PasswordSignInAsync упадет с ошибкой.
                // В этом случае продолжаем попытку входа через таблицу Patient.
            }

            try
            {
                // 2. Проверяем существующего пациента в таблице Patient (миграция/legacy вход)
                var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Login == model.Login && p.Password == model.Password);
                if (patient != null)
                {
                    // Проверяем, существует ли уже Identity пользователь для этого логина
                    var user = await _userManager.FindByNameAsync(patient.Login ?? "");
                    if (user == null)
                    {
                        // Если Identity пользователя нет, создаем его "на лету"
                        user = new ApplicationUser 
                        { 
                            UserName = patient.Login, 
                            Email = patient.Email,
                            PatientId = patient.ID 
                        };
                        
                        var createResult = await _userManager.CreateAsync(user, model.Password);
                        if (createResult.Succeeded)
                        {
                            await _signInManager.SignInAsync(user, isPersistent: model.RememberMe);
                            return RedirectToLocal(returnUrl);
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Ошибка при создании записи аутентификации.");
                            return View(model);
                        }
                    }
                    else
                    {
                        // Если пользователь Identity есть, но пароль не подошел в PasswordSignInAsync, 
                        // значит в таблице AspNetUsers другой пароль (хешированный).
                        // Но если в Patient.Password он совпал, мы можем его обновить или выдать ошибку.
                        ModelState.AddModelError(string.Empty, "Неверный пароль в системе аутентификации.");
                        return View(model);
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Ошибка базы данных: {ex.Message}. Проверьте подключение в SSMS.");
                return View(model);
            }

            ModelState.AddModelError(string.Empty, "Неверный логин или пароль.");
        }

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Register()
    {
        ViewBag.Genders = new SelectList(await _context.Genders.ToListAsync(), "ID", "GenderName");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                // 1. Создаем пациента
                var patient = new Patient
                {
                    FName = model.FName,
                    LName = model.LName,
                    MName = model.MName,
                    DateOfBirthday = model.DateOfBirthday,
                    IDGender = model.IDGender,
                    Address = model.Address,
                    Phone = model.Phone,
                    Email = model.Email,
                    Login = model.Login,
                    Password = model.Password
                };

                _context.Patients.Add(patient);
                await _context.SaveChangesAsync();

                // 2. Создаем пользователя Identity
                var user = new ApplicationUser 
                { 
                    UserName = model.Login, 
                    Email = model.Email,
                    PatientId = patient.ID 
                };
                
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                
                // Если ошибка при создании пользователя - откатываем пациента
                _context.Patients.Remove(patient);
                await _context.SaveChangesAsync();

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Произошла ошибка при регистрации: {ex.Message}");
            // Логируем ошибку, если у нас есть ILogger
        }

        ViewBag.Genders = new SelectList(await _context.Genders.ToListAsync(), "ID", "GenderName");
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction(nameof(Login));
    }

    private IActionResult RedirectToLocal(string? returnUrl)
    {
        if (Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }
        else
        {
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
