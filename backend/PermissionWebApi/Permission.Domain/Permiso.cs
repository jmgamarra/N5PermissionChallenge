namespace Permission.Domain
{
    public class Permiso
    {
        public int Id { get; set; }
        public string NombreEmpleado { get; set; }
        public string ApellidoEmpleado { get; set; }
        public DateTime FechaPermiso { get; set; }
        public int TipoPermisoId { get; set;}
        public TipoPermiso TipoPermiso { get; set; }
    }
}
