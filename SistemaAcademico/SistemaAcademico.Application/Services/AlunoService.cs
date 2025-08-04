using SistemaAcademico.Application.Interfaces;
using SistemaAcademico.Domain.Entities;
using SistemaAcademico.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace SistemaAcademico.Application.Services
{
    public class AlunoService : IAlunoService
    {
        private readonly AppDbContext _context;

        public AlunoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Aluno>> GetAlunosAsync()
        {
            return await _context.Alunos.OrderBy(c => c.Id).ToListAsync();
        }

        public async Task<Aluno?> GetAlunoByIdAsync(int id)
        {
            return await _context.Alunos.FindAsync(id);
        }

        public async Task AddAlunoAsync(Aluno aluno)
        {
            await _context.Alunos.AddAsync(aluno);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAlunoAsync(Aluno aluno)
        {
            _context.Entry(aluno).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAlunoAsync(int id)
        {
            var aluno = await _context.Alunos.FindAsync(id);
            if (aluno != null)
            {
                // Regra de Negócio: Apaga as matrículas do aluno antes de apagar o aluno
                var matriculasDoAluno = _context.Matriculas.Where(m => m.AlunoId == id);
                _context.Matriculas.RemoveRange(matriculasDoAluno);

                _context.Alunos.Remove(aluno);
                await _context.SaveChangesAsync();
            }
        }
    }
}