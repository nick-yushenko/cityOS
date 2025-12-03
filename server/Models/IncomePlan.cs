using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CityOS.Models
{
    /// <summary>
    /// Модель планового дохода
    /// </summary>
    [Table("income_plan")]
    [Index(nameof(RevisionId), nameof(IncomeSourceId), IsUnique = true, Name = "ux_budget_income_revision_source")]
    public class IncomePlan
    {
        /// <summary>
        /// ID записи планового дохода
        /// </summary>
        [Key]
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// Ссылка на редакцию бюджета (budget_revision.id)
        /// </summary>
        [Required]
        [Column("revision_id")]
        public int RevisionId { get; set; }

        /// <summary>
        /// Ссылка на источник дохода (income_source.id)
        /// </summary>
        [Required]
        [Column("income_source_id")]
        public int IncomeSourceId { get; set; }

        /// <summary>
        /// Плановая сумма, тыс. руб. (утвержденная по бюджету для источника)
        /// </summary>
        [Required]
        [Column("value", TypeName = "decimal(18,2)")]
        public decimal Value { get; set; }

        /// <summary>
        /// Доля источника в общем объёме доходов, % (если приходит в данных)
        /// </summary>
        [Column("share_percent", TypeName = "decimal(7,4)")]
        public decimal? SharePercent { get; set; }

        /// <summary>
        /// Дата и время добавления записи
        /// </summary>
        [Column("created_at", TypeName = "timestamp")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Дата и время последнего обновления записи
        /// </summary>
        [Column("updated_at", TypeName = "timestamp")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Навигационные свойства
        [ForeignKey(nameof(RevisionId))]
        public virtual BudgetRevision BudgetRevision { get; set; } = null!;

        [ForeignKey(nameof(IncomeSourceId))]
        public virtual IncomeSource IncomeSource { get; set; } = null!;
    }
}

