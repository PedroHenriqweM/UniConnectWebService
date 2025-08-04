using SistemaAcademico.Domain.Entities;

namespace SistemaAcademico.Application.Interfaces
{
    public interface ICursoService
    {
        Task<IEnumerable<Curso>> GetCursosAsync();
        Task<Curso?> GetCursoByIdAsync(int id);
        Task AddCursoAsync(Curso curso);
        Task UpdateCursoAsync(Curso curso);
        Task DeleteCursoAsync(int id);
    }
}