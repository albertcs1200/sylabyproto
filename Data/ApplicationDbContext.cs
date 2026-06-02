using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using protipo_sprint_tareas.Models;

namespace protipo_sprint_tareas.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<SyllabusCompliance> SyllabusCompliances { get; set; }
    public DbSet<SyllabusUpdateProposal> SyllabusUpdateProposals { get; set; }
}
