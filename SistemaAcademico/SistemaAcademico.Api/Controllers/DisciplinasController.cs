using Microsoft.AspNetCore.Mvc;
using SistemaAcademico.Application.Interfaces;
using SistemaAcademico.Domain.Entities;

namespace SistemaAcademico.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DisciplinasController : ControllerBase
    {
        private readonly IDisciplinaService _disciplinaService;

        public DisciplinasController(IDisciplinaService disciplinaService)
        {
            _disciplinaService = disciplinaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Disciplina>>> GetDisciplinas()
        {
            var disciplinas = await _disciplinaService.GetDisciplinasAsync();
            return Ok(disciplinas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Disciplina>> GetDisciplina(int id)
        {
            var disciplina = await _disciplinaService.GetDisciplinaByIdAsync(id);
            if (disciplina == null)
            {
                return NotFound();
            }
            return Ok(disciplina);
        }

        [HttpPost]
        public async Task<ActionResult<Disciplina>> PostDisciplina(Disciplina disciplina)
        {
            await _disciplinaService.AddDisciplinaAsync(disciplina);
            return CreatedAtAction(nameof(GetDisciplina), new { id = disciplina.Id }, disciplina);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDisciplina(int id, Disciplina disciplina)
        {
            if (id != disciplina.Id)
            {
                return BadRequest();
            }
            await _disciplinaService.UpdateDisciplinaAsync(disciplina);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDisciplina(int id)
        {
            await _disciplinaService.DeleteDisciplinaAsync(id);
            return NoContent();
        }
    }
}