namespace Permission.Domain
{
    public class TipoPermiso
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public List<Permiso> Permisos { get; set; } = new List<Permiso>();
    }
}
