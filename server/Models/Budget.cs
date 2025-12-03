using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CityOS.Models
{
    /// <summary>
    /// Модель бюджета города на год
    /// </summary>
    [Table("budget")]
    [Index(nameof(CityId), nameof(Year), IsUnique = true, Name = "ux_budget_city_year")]
    public class Budget
    {
        /// <summary>
        /// ID бюджета
        /// </summary>
        [Key]
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// Ссылка на город (city.id)
        /// </summary>
        [Required]
        [Column("city_id")]
        public int CityId { get; set; }

        /// <summary>
        /// Год бюджета
        /// </summary>
        [Required]
        [Column("year")]
        public int Year { get; set; }

        /// <summary>
        /// Название бюджета (например, "Бюджет 2025 года")
        /// </summary>
        [MaxLength(255)]
        [Column("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Дата и время создания записи
        /// </summary>
        [Column("created_at", TypeName = "timestamp")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Дата и время последнего обновления записи
        /// </summary>
        [Column("updated_at", TypeName = "timestamp")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Навигационные свойства
        [ForeignKey(nameof(CityId))]
        public virtual City City { get; set; } = null!;

        public virtual ICollection<BudgetRevision> BudgetRevisions { get; set; } = new List<BudgetRevision>();
    }
}

