using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityOS.Models
{
    /// <summary>
    /// Модель источника дохода
    /// </summary>
    [Table("income_source")]
    public class IncomeSource
    {
        /// <summary>
        /// ID источника дохода
        /// </summary>
        [Key]
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// Наименование дохода
        /// </summary>
        [Required]
        [MaxLength(255)]
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Внешний код источника из набора данных (если есть)
        /// </summary>
        [MaxLength(100)]
        [Column("external_code")]
        public string? ExternalCode { get; set; }

        /// <summary>
        /// Описание источника дохода (опционально)
        /// </summary>
        [Column("description", TypeName = "text")]
        public string? Description { get; set; }

        // Навигационные свойства
        public virtual ICollection<IncomePlan> IncomePlans { get; set; } = new List<IncomePlan>();
        public virtual ICollection<IncomeExecution> IncomeExecutions { get; set; } = new List<IncomeExecution>();
    }
}

