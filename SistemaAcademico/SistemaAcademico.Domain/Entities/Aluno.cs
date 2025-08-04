using System.ComponentModel.DataAnnotations;

namespace SistemaAcademico.Domain.Entities
{
    public class Aluno
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Nome { get; set; }

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required DateTime DataNascimento { get; set; }

        [Required]
        [MaxLength(20)]
        public required string Matricula { get; set; }

        [Required]
        public required int CursoId { get; set; }

        // Propriedade de navegação
        public Curso? Curso { get; set; }
    }
}