namespace TaskHub.Models
{
    public class Historico
    {
        public int HistoricoId { get; set; }
        public int TarefaId { get; set; }
        public string Detalhes { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public int? UsuarioId { get; set; }
        public virtual Tarefa Tarefa { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
