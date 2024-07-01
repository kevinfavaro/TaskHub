namespace TaskHub.Models
{
    public class Usuario
    {
        public int UsuarioId { get; set; }
        public string Funcao { get; set; }
        public bool Ativo { get; set; } = true;
    }
}
