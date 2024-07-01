using Microsoft.EntityFrameworkCore;
using TaskHub.Models;

namespace TaskHub.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Projeto> Projetos { get; set; }
        public DbSet<Tarefa> Tarefas { get; set; }
        public DbSet<Historico> Historico { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Projeto>()
                .HasOne(p => p.Usuario)
                .WithMany()
                .HasForeignKey(p => p.UsuarioId);

            modelBuilder.Entity<Tarefa>()
                .HasOne(p => p.Usuario)
                .WithMany()
                .HasForeignKey(p => p.UsuarioId);

            modelBuilder.Entity<Tarefa>()
                .HasOne(p => p.Projeto)
                .WithMany()
                .HasForeignKey(p => p.ProjetoId);

            modelBuilder.Entity<Historico>()
                .HasOne(p => p.Usuario)
                .WithMany()
                .HasForeignKey(p => p.UsuarioId);

            modelBuilder.Entity<Historico>()
                .HasOne(p => p.Tarefa)
                .WithMany()
                .HasForeignKey(p => p.TarefaId);

            modelBuilder.Entity<Comentario>()
                .HasOne(p => p.Usuario)
                .WithMany()
                .HasForeignKey(p => p.UsuarioId);

            modelBuilder.Entity<Comentario>()
                .HasOne(p => p.Tarefa)
                .WithMany()
                .HasForeignKey(p => p.TarefaId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
