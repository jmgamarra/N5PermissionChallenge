namespace Permission.Domain
{
    public class Permiso
    {
        public int Id { get; set; }
        public string NombreEmpleado { get; set; } = string.Empty;
        public string ApellidoEmpleado { get; set; } = string.Empty;
        public DateTime FechaPermiso { get; set; }
        public int TipoPermisoId { get; set;}
        public TipoPermiso TipoPermiso { get; set; } = new TipoPermiso();
    }
}
