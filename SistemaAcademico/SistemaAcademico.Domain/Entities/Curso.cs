using System.ComponentModel.DataAnnotations;

namespace SistemaAcademico.Domain.Entities
{
    public enum Modalidade
    {
        Presencial,
        Online,
        Hibrido
    }

    public class Curso
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Nome { get; set; }

        [Required]
        [MaxLength(250)]
        public required string Descricao { get; set; }

        [Required]
        [Range(1, 20)]
        public required int DuracaoSemestres { get; set; }

        [Required]
        public required Modalidade Modalidade { get; set; }

        // Propriedade de navegação
        public ICollection<Disciplina> Disciplinas { get; set; } = new List<Disciplina>();
    }
}