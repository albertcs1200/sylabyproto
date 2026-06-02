using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using protipo_sprint_tareas.Models;

namespace protipo_sprint_tareas.Controllers;

[Authorize]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        var tasks = new List<SprintTask>
        {
            new SprintTask
            {
                Id = 1,
                Title = "Configurar base de datos SQLite",
                Description = "Establecer la conexión con SQLite y realizar migraciones iniciales para Identity.",
                Status = Models.TaskStatus.Done,
                Priority = TaskPriority.Alta,
                Assignee = "Albert Córdova",
                StoryPoints = 3,
                DueDate = DateTime.Now.AddDays(2)
            },
            new SprintTask
            {
                Id = 2,
                Title = "Diseño de login premium",
                Description = "Maquetar la interfaz de Login con CSS premium, degradados fluidos y glassmorphism.",
                Status = Models.TaskStatus.InProgress,
                Priority = TaskPriority.Critica,
                Assignee = "Antigravity AI",
                StoryPoints = 5,
                DueDate = DateTime.Now.AddDays(1)
            },
            new SprintTask
            {
                Id = 3,
                Title = "Implementar registro de usuarios",
                Description = "Añadir vista y controlador para registro de nuevos miembros del equipo.",
                Status = Models.TaskStatus.Done,
                Priority = TaskPriority.Media,
                Assignee = "Albert Córdova",
                StoryPoints = 2,
                DueDate = DateTime.Now.AddDays(3)
            },
            new SprintTask
            {
                Id = 4,
                Title = "Pruebas unitarias de autenticación",
                Description = "Validar que la cookie de autenticación se cree correctamente y restrinja el acceso.",
                Status = Models.TaskStatus.ToDo,
                Priority = TaskPriority.Alta,
                Assignee = "María Flores",
                StoryPoints = 3,
                DueDate = DateTime.Now.AddDays(5)
            },
            new SprintTask
            {
                Id = 5,
                Title = "Refinar diseño responsive del sprint board",
                Description = "Asegurar que las columnas del tablero de scrum se adapten a móviles y tabletas.",
                Status = Models.TaskStatus.ToDo,
                Priority = TaskPriority.Baja,
                Assignee = "Juan Pérez",
                StoryPoints = 1,
                DueDate = DateTime.Now.AddDays(7)
            }
        };

        return View(tasks);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
