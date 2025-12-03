using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityOS.Models
{
    /// <summary>
    /// Модель редакции бюджета
    /// </summary>
    [Table("budget_revision")]
    public class BudgetRevision
    {
        /// <summary>
        /// ID редакции бюджета
        /// </summary>
        [Key]
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// Ссылка на бюджет (budget.id)
        /// </summary>
        [Required]
        [Column("budget_id")]
        public int BudgetId { get; set; }

        /// <summary>
        /// Описание редакции бюджета (например, «Бюджет 2025, ред. от 01.03.2025»)
        /// </summary>
        [MaxLength(255)]
        [Column("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Дата утверждения/внесения редакции бюджета (если есть в данных)
        /// </summary>
        [Column("revision_date", TypeName = "date")]
        public DateTime? RevisionDate { get; set; }

        // Навигационные свойства
        [ForeignKey(nameof(BudgetId))]
        public virtual Budget Budget { get; set; } = null!;

        public virtual ICollection<IncomePlan> IncomePlans { get; set; } = new List<IncomePlan>();
        public virtual ICollection<IncomeExecution> IncomeExecutions { get; set; } = new List<IncomeExecution>();
    }
}

