using System.ComponentModel.DataAnnotations;

namespace protipo_sprint_tareas.Models;

public class SyllabusCompliance
{
    public int Id { get; set; }

    // TeacherName se asigna automáticamente desde el servidor (no validar desde el formulario)
    [Display(Name = "Docente")]
    public string TeacherName { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre del curso/asignatura es obligatorio.")]
    [Display(Name = "Curso / Asignatura")]
    public string CourseName { get; set; } = string.Empty;

    [Required(ErrorMessage = "El tema o unidad del sílabo es obligatorio.")]
    [Display(Name = "Tema / Unidad Cumplida")]
    public string Topic { get; set; } = string.Empty;

    [Required(ErrorMessage = "El porcentaje de avance es obligatorio.")]
    [Range(0, 100, ErrorMessage = "El porcentaje debe estar entre 0 y 100.")]
    [Display(Name = "Porcentaje de Avance (%)")]
    public int CompliancePercentage { get; set; }

    [Required(ErrorMessage = "La fecha y hora de la clase son obligatorias.")]
    [DataType(DataType.DateTime)]
    [Display(Name = "Fecha y Hora de la Clase")]
    public DateTime ClassDate { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "Las observaciones o comentarios son obligatorios para validar los datos completos.")]
    [StringLength(500, MinimumLength = 10, ErrorMessage = "Las observaciones deben tener entre 10 y 500 caracteres.")]
    [Display(Name = "Observaciones / Evidencias de Avance")]
    public string Comments { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
