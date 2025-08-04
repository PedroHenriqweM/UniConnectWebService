using SistemaAcademico.Application.Interfaces;
using SistemaAcademico.Domain.Entities;
using SistemaAcademico.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace SistemaAcademico.Application.Services
{
    public class CursoService : ICursoService
    {
        private readonly AppDbContext _context;

        public CursoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Curso>> GetCursosAsync()
        {
            return await _context.Cursos.OrderBy(c => c.Id).ToListAsync();
        }

        public async Task<Curso?> GetCursoByIdAsync(int id)
        {
            return await _context.Cursos.FindAsync(id);
        }

        public async Task AddCursoAsync(Curso curso)
        {
            await _context.Cursos.AddAsync(curso);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCursoAsync(Curso curso)
        {
            _context.Entry(curso).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCursoAsync(int id)
        {
            var curso = await _context.Cursos.FindAsync(id);
            if (curso != null)
            {
                // Passo 1: Apagar as matrículas das disciplinas do curso
                var disciplinasDoCurso = _context.Disciplinas.Where(d => d.CursoId == id).ToList();
                var idsDisciplinas = disciplinasDoCurso.Select(d => d.Id);
                var matriculasParaApagar = _context.Matriculas.Where(m => idsDisciplinas.Contains(m.DisciplinaId));
                _context.Matriculas.RemoveRange(matriculasParaApagar);

                // Passo 2: Apagar as disciplinas do curso
                _context.Disciplinas.RemoveRange(disciplinasDoCurso);

                // Passo 3: "Zerar" o CursoId dos alunos
                var alunosDoCurso = _context.Alunos.Where(a => a.CursoId == id);
                await alunosDoCurso.ForEachAsync(a => a.CursoId = 0);

                // Passo 4: Apagar o curso
                _context.Cursos.Remove(curso);

                await _context.SaveChangesAsync();
            }
        }
    }
}