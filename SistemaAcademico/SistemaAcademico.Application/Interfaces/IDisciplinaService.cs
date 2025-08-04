using SistemaAcademico.Domain.Entities;

namespace SistemaAcademico.Application.Interfaces
{
    public interface IDisciplinaService
    {
        Task<IEnumerable<Disciplina>> GetDisciplinasAsync();
        Task<Disciplina?> GetDisciplinaByIdAsync(int id);
        Task AddDisciplinaAsync(Disciplina disciplina);
        Task UpdateDisciplinaAsync(Disciplina disciplina);
        Task DeleteDisciplinaAsync(int id);
    }
}