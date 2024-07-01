namespace TaskHub.Models
{
    public class Projeto
    {
        public int ProjetoId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int UsuarioId { get; set; }
        public bool Ativo { get; set; } = true;
        public virtual Usuario Usuario { get; set; }
    }
}
