using HospitalApp.Data;
using HospitalApp.Models;
using HospitalApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalApp.Controllers;

[Authorize]
public class AppointmentsController : Controller
{
    private readonly HospitalDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public AppointmentsController(HospitalDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> Upcoming()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null || user.PatientId == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var upcomingAppointments = await _context.Orders
            .Where(o => o.IDPatient == user.PatientId && o.Appointment.DateService >= DateTime.Now)
            .OrderBy(o => o.Appointment.DateService)
            .Select(o => new UpcomingAppointmentViewModel
            {
                DateService = o.Appointment.DateService,
                ServiceTitle = o.Appointment.MedicalService.TitleService,
                DoctorShortName = $"{o.Appointment.Employee.LName} {o.Appointment.Employee.FName.Substring(0, 1)}. {(o.Appointment.Employee.MName != null ? o.Appointment.Employee.MName.Substring(0, 1) + "." : "")}"
            })
            .ToListAsync();

        return View(upcomingAppointments);
    }

    [HttpGet]
    public async Task<IActionResult> History()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null || user.PatientId == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var historyAppointments = await _context.Orders
            .Where(o => o.IDPatient == user.PatientId && o.Appointment.DateService < DateTime.Now)
            .OrderByDescending(o => o.Appointment.DateService)
            .Select(o => new AppointmentHistoryViewModel
            {
                DateService = o.Appointment.DateService,
                ServiceTitle = o.Appointment.MedicalService.TitleService,
                DoctorShortName = $"{o.Appointment.Employee.LName} {o.Appointment.Employee.FName.Substring(0, 1)}. {(o.Appointment.Employee.MName != null ? o.Appointment.Employee.MName.Substring(0, 1) + "." : "")}",
                Cost = o.TotalPrice
            })
            .ToListAsync();

        return View(historyAppointments);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewBag.Services = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await _context.MedicalServices.ToListAsync(), "ID", "TitleService");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AppointmentCreateViewModel model)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null || user.PatientId == null)
        {
            return RedirectToAction("Login", "Account");
        }

        if (model.DateService < DateTime.Now)
        {
            ModelState.AddModelError("DateService", "Дата и время не могут быть в прошлом.");
        }

        if (ModelState.IsValid)
        {
            // 1. Ищем услугу
            var service = await _context.MedicalServices.FindAsync(model.IDMedicalService);
            if (service == null)
            {
                ModelState.AddModelError("IDMedicalService", "Услуга не найдена.");
            }
            else
            {
                // 2. Выбираем врача (первого из Employee)
                var doctor = await _context.Employees.FirstOrDefaultAsync();
                if (doctor == null)
                {
                    ModelState.AddModelError(string.Empty, "В данный момент нет свободных врачей.");
                }
                else
                {
                    // 3. Создаем Appointment
                    var appointment = new Appointment
                    {
                        IDEmployee = doctor.ID,
                        IDMedicalService = service.ID,
                        DateService = model.DateService
                    };

                    _context.Appointments.Add(appointment);
                    await _context.SaveChangesAsync();

                    // 4. Создаем Order
                    var order = new Order
                    {
                        IDAppointment = appointment.ID,
                        IDPatient = user.PatientId.Value,
                        TotalPrice = service.Cost
                    };

                    _context.Orders.Add(order);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Вы успешно записаны на услугу!";
                    return RedirectToAction("Index", "Home");
                }
            }
        }

        ViewBag.Services = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await _context.MedicalServices.ToListAsync(), "ID", "TitleService");
        return View(model);
    }
}
