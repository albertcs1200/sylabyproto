using System.ComponentModel.DataAnnotations;

namespace protipo_sprint_tareas.Models;

public class SyllabusUpdateProposal
{
    public int Id { get; set; }

    // ProposerName se asigna automáticamente desde el servidor (no validar desde el formulario)
    [Display(Name = "Proponente")]
    public string ProposerName { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre de la asignatura es obligatorio.")]
    [Display(Name = "Curso / Asignatura")]
    public string CourseName { get; set; } = string.Empty;

    [Required(ErrorMessage = "El tipo de modificación sugerida es obligatorio.")]
    [Display(Name = "Tipo de Modificación")]
    public string ChangeType { get; set; } = "Contenido"; // Contenido, Bibliografía, Metodología, Evaluación

    [Required(ErrorMessage = "La descripción detallada de la propuesta es obligatoria.")]
    [StringLength(1000, MinimumLength = 20, ErrorMessage = "La propuesta debe ser detallada (mínimo 20 caracteres).")]
    [Display(Name = "Descripción Detallada de la Propuesta")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "La justificación académica es obligatoria.")]
    [StringLength(500, MinimumLength = 15, ErrorMessage = "La justificación debe tener al menos 15 caracteres.")]
    [Display(Name = "Justificación Académica")]
    public string Justification { get; set; } = string.Empty;

    [Required(ErrorMessage = "La fecha propuesta es obligatoria.")]
    [DataType(DataType.Date)]
    [Display(Name = "Fecha de Propuesta")]
    public DateTime ProposalDate { get; set; } = DateTime.Now;

    [Display(Name = "¿Aprobada por el Director?")]
    public bool IsApproved { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
