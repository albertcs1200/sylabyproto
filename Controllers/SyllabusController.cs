using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using protipo_sprint_tareas.Data;
using protipo_sprint_tareas.Models;

namespace protipo_sprint_tareas.Controllers;

[Authorize]
public class SyllabusController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public SyllabusController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // Dashboard del Sílabo
    public async Task<IActionResult> Index()
    {
        var isDirector = User.IsInRole("Director");

        var compliances = isDirector
            ? await _context.SyllabusCompliances.OrderByDescending(c => c.ClassDate).ToListAsync()
            : await _context.SyllabusCompliances.Where(c => c.TeacherName == User.Identity!.Name).OrderByDescending(c => c.ClassDate).ToListAsync();

        var proposals = isDirector
            ? await _context.SyllabusUpdateProposals.OrderByDescending(p => p.ProposalDate).ToListAsync()
            : await _context.SyllabusUpdateProposals.Where(p => p.ProposerName == User.Identity!.Name).OrderByDescending(p => p.ProposalDate).ToListAsync();

        ViewBag.IsDirector = isDirector;
        ViewBag.Compliances = compliances;
        ViewBag.Proposals = proposals;

        return View();
    }

    // --- REGISTRO DE CUMPLIMIENTO DEL SÍLABO ---
    [HttpGet]
    public IActionResult RegisterCompliance()
    {
        var model = new SyllabusCompliance
        {
            TeacherName = User.Identity?.Name ?? string.Empty,
            ClassDate = DateTime.Now
        };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RegisterCompliance(SyllabusCompliance model)
    {
        // 1. VALIDAR AUTOR: asignar servidor antes de validar
        var authorName = User.Identity?.Name ?? string.Empty;
        model.TeacherName = authorName;
        ModelState.Remove("TeacherName"); // Limpiar error previo porque se asigna server-side

        // 2. VALIDAR FECHA Y HORA
        if (model.ClassDate == default)
        {
            ModelState.AddModelError("ClassDate", "La fecha y hora de la clase son obligatorias.");
        }
        else
        {
            // Fecha no puede ser más de 7 días en el futuro
            if (model.ClassDate > DateTime.Now.AddDays(7))
            {
                ModelState.AddModelError("ClassDate", "❌ La fecha de la clase no puede ser mayor a 7 días en el futuro.");
            }
            // Fecha no puede ser hace más de 1 año
            if (model.ClassDate < DateTime.Now.AddYears(-1))
            {
                ModelState.AddModelError("ClassDate", "❌ La fecha de la clase es demasiado antigua (máximo 1 año atrás).");
            }
        }

        // 3. VALIDAR DATOS COMPLETOS antes de guardar
        if (string.IsNullOrWhiteSpace(authorName))
        {
            ModelState.AddModelError(string.Empty, "❌ No se pudo identificar al autor. Verifica tu sesión activa.");
        }

        if (model.CompliancePercentage < 0 || model.CompliancePercentage > 100)
        {
            ModelState.AddModelError("CompliancePercentage", "❌ El porcentaje debe estar entre 0 y 100.");
        }

        if (ModelState.IsValid)
        {
            model.CreatedAt = DateTime.Now;
            _context.Add(model);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"✅ Cumplimiento registrado correctamente. Autor: {model.TeacherName} | Fecha: {model.ClassDate:dd/MM/yyyy HH:mm}";
            return RedirectToAction(nameof(Index));
        }

        return View(model);
    }

    // --- PROPUESTAS DE ACTUALIZACIÓN DEL SÍLABO ---
    [HttpGet]
    public IActionResult CreateProposal()
    {
        var model = new SyllabusUpdateProposal
        {
            ProposerName = User.Identity?.Name ?? string.Empty,
            ProposalDate = DateTime.Now
        };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateProposal(SyllabusUpdateProposal model)
    {
        // 1. VALIDAR AUTOR: asignar servidor antes de validar
        var authorName = User.Identity?.Name ?? string.Empty;
        model.ProposerName = authorName;
        ModelState.Remove("ProposerName"); // Limpiar error previo porque se asigna server-side

        // 2. VALIDAR FECHA
        if (model.ProposalDate == default)
        {
            ModelState.AddModelError("ProposalDate", "La fecha de la propuesta es obligatoria.");
        }
        else if (model.ProposalDate > DateTime.Now.AddDays(30))
        {
            ModelState.AddModelError("ProposalDate", "❌ La fecha de la propuesta no puede ser mayor a 30 días en el futuro.");
        }
        else if (model.ProposalDate < DateTime.Now.AddYears(-1))
        {
            ModelState.AddModelError("ProposalDate", "❌ La fecha de la propuesta es demasiado antigua.");
        }

        // 3. VALIDAR DATOS COMPLETOS
        if (string.IsNullOrWhiteSpace(authorName))
        {
            ModelState.AddModelError(string.Empty, "❌ No se pudo identificar al proponente. Verifica tu sesión activa.");
        }

        if (ModelState.IsValid)
        {
            model.IsApproved = false; // Disponibilidad para revisión del Director: siempre comienza como pendiente
            model.CreatedAt = DateTime.Now;
            _context.Add(model);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"✅ Propuesta registrada. Autor: {model.ProposerName} | Fecha: {model.ProposalDate:dd/MM/yyyy} | Estado: Pendiente de revisión por el Director";
            return RedirectToAction(nameof(Index));
        }

        return View(model);
    }

    // --- ACCESO EXCLUSIVO PARA ROLES: APROBAR PROPUESTAS (DIRECTOR) ---
    [HttpPost]
    [Authorize(Roles = "Director")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ApproveProposal(int id)
    {
        var proposal = await _context.SyllabusUpdateProposals.FindAsync(id);
        if (proposal == null)
        {
            TempData["ErrorMessage"] = "❌ La propuesta no fue encontrada.";
            return RedirectToAction(nameof(Index));
        }

        proposal.IsApproved = true;
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = $"✅ La propuesta de \"{proposal.CourseName}\" ha sido Aprobada por el Director.";
        return RedirectToAction(nameof(Index));
    }

    // --- RECHAZAR PROPUESTA (DIRECTOR) ---
    [HttpPost]
    [Authorize(Roles = "Director")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RejectProposal(int id)
    {
        var proposal = await _context.SyllabusUpdateProposals.FindAsync(id);
        if (proposal == null)
        {
            TempData["ErrorMessage"] = "❌ La propuesta no fue encontrada.";
            return RedirectToAction(nameof(Index));
        }

        _context.SyllabusUpdateProposals.Remove(proposal);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = $"🗑️ La propuesta de \"{proposal.CourseName}\" ha sido Rechazada y eliminada.";
        return RedirectToAction(nameof(Index));
    }

    // --- ACCESO EXCLUSIVO PARA ROLES: PANEL DE ADMINISTRACIÓN (DIRECTOR) ---
    [HttpGet]
    [Authorize(Roles = "Director")]
    public async Task<IActionResult> AdminPanel()
    {
        var allCompliances = await _context.SyllabusCompliances.OrderByDescending(c => c.CreatedAt).ToListAsync();
        var allProposals = await _context.SyllabusUpdateProposals.OrderByDescending(p => p.CreatedAt).ToListAsync();

        ViewBag.Compliances = allCompliances;
        ViewBag.Proposals = allProposals;

        return View();
    }
}
