namespace TaskHub.Models
{
    public class Comentario
    {
        public int ComentarioId { get; set; }
        public int TarefaId { get; set; }
        public string Texto { get; set; }
        public DateTime DataComentario { get; set; }
        public int? UsuarioId { get; set; }
        public bool Ativo { get; set; } = true;
        public virtual Tarefa Tarefa { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
