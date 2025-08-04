using SistemaAcademico.Application.Interfaces;
using SistemaAcademico.Domain.Entities;
using SistemaAcademico.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace SistemaAcademico.Application.Services
{
    public class DisciplinaService : IDisciplinaService
    {
        private readonly AppDbContext _context;

        public DisciplinaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Disciplina>> GetDisciplinasAsync()
        {
            return await _context.Disciplinas.OrderBy(c => c.Id).ToListAsync();
        }

        public async Task<Disciplina?> GetDisciplinaByIdAsync(int id)
        {
            return await _context.Disciplinas.FindAsync(id);
        }

        public async Task AddDisciplinaAsync(Disciplina disciplina)
        {
            await _context.Disciplinas.AddAsync(disciplina);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDisciplinaAsync(Disciplina disciplina)
        {
            _context.Entry(disciplina).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDisciplinaAsync(int id)
        {
            var disciplina = await _context.Disciplinas.FindAsync(id);
            if (disciplina != null)
            {
                // Regra de Negócio: Apaga as matrículas da disciplina antes de apagar a disciplina
                var matriculasDaDisciplina = _context.Matriculas.Where(m => m.DisciplinaId == id);
                _context.Matriculas.RemoveRange(matriculasDaDisciplina);

                _context.Disciplinas.Remove(disciplina);
                await _context.SaveChangesAsync();
            }
        }
    }
}