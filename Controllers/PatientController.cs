using HospitalApp.Data;
using HospitalApp.Models;
using HospitalApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalApp.Controllers;

[Authorize]
public class PatientController : Controller
{
    private readonly HospitalDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public PatientController(HospitalDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null || user.PatientId == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var patient = await _context.Patients
            .Include(p => p.Gender)
            .FirstOrDefaultAsync(p => p.ID == user.PatientId);

        if (patient == null)
        {
            return NotFound("Данные пациента не найдены.");
        }

        var model = new PatientProfileViewModel
        {
            ID = patient.ID,
            FName = patient.FName,
            LName = patient.LName,
            MName = patient.MName,
            DateOfBirthday = patient.DateOfBirthday,
            GenderName = patient.Gender.GenderName,
            Phone = patient.Phone,
            Email = patient.Email,
            Address = patient.Address
        };

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null || user.PatientId == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var patient = await _context.Patients
            .FirstOrDefaultAsync(p => p.ID == user.PatientId);

        if (patient == null)
        {
            return NotFound("Данные пациента не найдены.");
        }

        var model = new PatientEditViewModel
        {
            FName = patient.FName,
            LName = patient.LName,
            MName = patient.MName,
            DateOfBirthday = patient.DateOfBirthday,
            IDGender = patient.IDGender,
            Address = patient.Address,
            Phone = patient.Phone,
            Email = patient.Email
        };

        ViewBag.Genders = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await _context.Genders.ToListAsync(), "ID", "GenderName");
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(PatientEditViewModel model)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null || user.PatientId == null)
        {
            return RedirectToAction("Login", "Account");
        }

        if (ModelState.IsValid)
        {
            var patient = await _context.Patients.FindAsync(user.PatientId);
            if (patient == null)
            {
                return NotFound("Пациент не найден.");
            }

            patient.FName = model.FName;
            patient.LName = model.LName;
            patient.MName = model.MName;
            patient.DateOfBirthday = model.DateOfBirthday;
            patient.IDGender = model.IDGender;
            patient.Address = model.Address;
            patient.Phone = model.Phone;
            patient.Email = model.Email;

            _context.Update(patient);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Profile));
        }

        ViewBag.Genders = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await _context.Genders.ToListAsync(), "ID", "GenderName");
        return View(model);
    }
}
