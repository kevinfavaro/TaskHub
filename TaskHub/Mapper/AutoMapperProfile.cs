using AutoMapper;
using TaskHub.DTO;
using TaskHub.Models;

namespace TaskHub.Mapper
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ProjetoCreateDTO, Projeto>();
            CreateMap<TarefaCreateDTO, Tarefa>();
            CreateMap<TarefaUpdateDTO, Tarefa>();
            CreateMap<ComentarioCreateDTO, Comentario>();
        }
    }
}
