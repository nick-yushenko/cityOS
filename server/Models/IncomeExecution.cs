using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CityOS.Models
{
    /// <summary>
    /// Модель исполнения дохода
    /// </summary>
    [Table("income_execution")]
    [Index(nameof(RevisionId), nameof(IncomeSourceId), nameof(ReportPeriod), IsUnique = true, Name = "ux_execution_revision_source_period")]
    public class IncomeExecution
    {
        /// <summary>
        /// ID записи исполнения дохода
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
        /// Отчетный период (обычно 1-е число месяца)
        /// </summary>
        [Required]
        [Column("report_period", TypeName = "date")]
        public DateTime ReportPeriod { get; set; }

        /// <summary>
        /// Фактическое исполнение, тыс. руб. на конец отчетного периода
        /// </summary>
        [Required]
        [Column("value", TypeName = "decimal(18,2)")]
        public decimal Value { get; set; }

        /// <summary>
        /// Исполнение, % от плановой суммы на конец отчетного периода
        /// </summary>
        [Column("execution_percent", TypeName = "decimal(7,4)")]
        public decimal? ExecutionPercent { get; set; }

        /// <summary>
        /// Дата и время добавлениязаписи
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

