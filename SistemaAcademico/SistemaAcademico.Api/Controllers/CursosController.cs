using Microsoft.AspNetCore.Mvc;
using SistemaAcademico.Application.Interfaces;
using SistemaAcademico.Domain.Entities;

namespace SistemaAcademico.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CursosController : ControllerBase
    {
        private readonly ICursoService _cursoService;

        public CursosController(ICursoService cursoService)
        {
            _cursoService = cursoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Curso>>> GetCursos()
        {
            var cursos = await _cursoService.GetCursosAsync();
            return Ok(cursos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Curso>> GetCurso(int id)
        {
            var curso = await _cursoService.GetCursoByIdAsync(id);
            if (curso == null)
            {
                return NotFound();
            }
            return Ok(curso);
        }

        [HttpPost]
        public async Task<ActionResult<Curso>> PostCurso(Curso curso)
        {
            // Adicione esta verificação para validar o modelo
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _cursoService.AddCursoAsync(curso);
            return CreatedAtAction(nameof(GetCurso), new { id = curso.Id }, curso);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCurso(int id, Curso curso)
        {
            if (id != curso.Id)
            {
                return BadRequest();
            }

            // Adicione a mesma verificação para a atualização
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _cursoService.UpdateCursoAsync(curso);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCurso(int id)
        {
            await _cursoService.DeleteCursoAsync(id);
            return NoContent();
        }
    }
}