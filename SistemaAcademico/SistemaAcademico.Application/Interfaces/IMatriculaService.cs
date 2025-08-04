using SistemaAcademico.Domain.Entities;

namespace SistemaAcademico.Application.Interfaces
{
    public interface IMatriculaService
    {
        Task<IEnumerable<Matricula>> GetMatriculasAsync();
        Task<Matricula?> GetMatriculaByIdAsync(int id);
        Task AddMatriculaAsync(Matricula matricula);
        Task UpdateMatriculaAsync(Matricula matricula);
        Task DeleteMatriculaAsync(int id);
    }
}