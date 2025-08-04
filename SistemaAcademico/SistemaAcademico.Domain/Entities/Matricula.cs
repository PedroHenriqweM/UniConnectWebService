using System.ComponentModel.DataAnnotations;

namespace SistemaAcademico.Domain.Entities
{
    public class Matricula
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required int AlunoId { get; set; }

        [Required]
        public required int DisciplinaId { get; set; }

        [Required]
        [MaxLength(10)]
        public required string Periodo { get; set; }

        [Range(0, 10)] // A nota final pode ser opcional e ter um range
        public double? NotaFinal { get; set; }

        // Propriedades de navegação
        public Aluno? Aluno { get; set; }
        public Disciplina? Disciplina { get; set; }
    }
}