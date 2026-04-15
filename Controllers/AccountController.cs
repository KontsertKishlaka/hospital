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
            // Пытаемся найти пользователя по логину (UserName)
            var result = await _signInManager.PasswordSignInAsync(model.Login, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return RedirectToLocal(returnUrl);
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
                Login = model.Login
                // Пароль в Patient храним как есть (или не храним, если используем Identity), 
                // но ТЗ просит заполнить Patient.
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
            
            // Если ошибка при создании пользователя - откатываем пациента (в идеале использовать транзакцию)
            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
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
