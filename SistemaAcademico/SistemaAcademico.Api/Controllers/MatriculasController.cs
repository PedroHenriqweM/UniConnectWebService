using Microsoft.AspNetCore.Mvc;
using SistemaAcademico.Application.Interfaces;
using SistemaAcademico.Domain.Entities;

namespace SistemaAcademico.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatriculasController : ControllerBase
    {
        private readonly IMatriculaService _matriculaService;

        public MatriculasController(IMatriculaService matriculaService)
        {
            _matriculaService = matriculaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Matricula>>> GetMatriculas()
        {
            var matriculas = await _matriculaService.GetMatriculasAsync();
            return Ok(matriculas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Matricula>> GetMatricula(int id)
        {
            var matricula = await _matriculaService.GetMatriculaByIdAsync(id);
            if (matricula == null)
            {
                return NotFound();
            }
            return Ok(matricula);
        }

        [HttpPost]
        public async Task<ActionResult<Matricula>> PostMatricula(Matricula matricula)
        {
            await _matriculaService.AddMatriculaAsync(matricula);
            return CreatedAtAction(nameof(GetMatricula), new { id = matricula.Id }, matricula);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMatricula(int id, Matricula matricula)
        {
            if (id != matricula.Id)
            {
                return BadRequest();
            }
            await _matriculaService.UpdateMatriculaAsync(matricula);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMatricula(int id)
        {
            await _matriculaService.DeleteMatriculaAsync(id);
            return NoContent();
        }
    }
}