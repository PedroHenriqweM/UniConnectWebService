using SistemaAcademico.Domain.Entities;

namespace SistemaAcademico.Application.Interfaces
{
    public interface IAlunoService
    {
        Task<IEnumerable<Aluno>> GetAlunosAsync();
        Task<Aluno?> GetAlunoByIdAsync(int id);
        Task AddAlunoAsync(Aluno aluno);
        Task UpdateAlunoAsync(Aluno aluno);
        Task DeleteAlunoAsync(int id);
    }
}