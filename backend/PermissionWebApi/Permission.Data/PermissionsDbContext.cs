using Microsoft.EntityFrameworkCore;
using Permission.Domain;

public class PermissionsDbContext : DbContext
{
    public PermissionsDbContext(DbContextOptions<PermissionsDbContext> options) : base(options)
    {
    }

    public DbSet<Permiso> Permisos { get; set; }
    public DbSet<TipoPermiso> TiposPermiso { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TipoPermiso>().HasData(
             new TipoPermiso { Id = 1, Descripcion = "Tipo 1" },
             new TipoPermiso { Id = 2, Descripcion = "Tipo 2" }
         );

        //modelBuilder.Entity<Permiso>().HasData(
        //    new Permiso { Id = 1, NombreEmpleado = "Jose", ApellidoEmpleado="Soto",
        //        FechaPermiso= new DateTime(2024, 3, 14, 13, 46, 40, 620, DateTimeKind.Utc).AddTicks(4487), 
        //        TipoPermisoId = 1 }
        //);

        modelBuilder.Entity<Permiso>()
            .HasOne(p => p.TipoPermiso)
            .WithMany(t => t.Permisos)
            .HasForeignKey(p => p.TipoPermisoId);

        base.OnModelCreating(modelBuilder);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("N5Connection", options =>
                options.MigrationsAssembly("PermissionsWeb.Data"));
        }
    }
}
