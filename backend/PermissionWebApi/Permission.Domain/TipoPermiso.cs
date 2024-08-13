namespace Permission.Domain
{
    public class TipoPermiso
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public List<Permiso> Permisos { get; set; }
    }
}
