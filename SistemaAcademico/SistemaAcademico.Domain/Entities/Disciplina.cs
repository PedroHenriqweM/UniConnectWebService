using System.ComponentModel.DataAnnotations;

namespace SistemaAcademico.Domain.Entities
{
    public class Disciplina
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Nome { get; set; }

        [Required]
        [MaxLength(10)]
        public required string Codigo { get; set; }

        [Required]
        [Range(1, 100)] // Exemplo de range
        public required int CargaHoraria { get; set; }

        [Required]
        public required int CursoId { get; set; }

        // Propriedades de navegação
        public Curso? Curso { get; set; }
        public ICollection<Matricula> Matriculas { get; set; } = new List<Matricula>();
    }
}