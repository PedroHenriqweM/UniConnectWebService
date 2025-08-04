using SistemaAcademico.Application.Interfaces;
using SistemaAcademico.Domain.Entities;
using SistemaAcademico.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace SistemaAcademico.Application.Services
{
    public class MatriculaService : IMatriculaService
    {
        private readonly AppDbContext _context;

        public MatriculaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Matricula>> GetMatriculasAsync()
        {
            return await _context.Matriculas.OrderBy(c => c.Id).ToListAsync();
        }

        public async Task<Matricula?> GetMatriculaByIdAsync(int id)
        {
            return await _context.Matriculas.FindAsync(id);
        }

        public async Task AddMatriculaAsync(Matricula matricula)
        {
            await _context.Matriculas.AddAsync(matricula);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMatriculaAsync(Matricula matricula)
        {
            _context.Entry(matricula).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMatriculaAsync(int id)
        {
            var matricula = await _context.Matriculas.FindAsync(id);
            if (matricula != null)
            {
                _context.Matriculas.Remove(matricula);
                await _context.SaveChangesAsync();
            }
        }
    }
}