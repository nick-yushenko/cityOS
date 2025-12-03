using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityOS.Models
{
    /// <summary>
    /// Модель города
    /// </summary>
    [Table("city")]
    public class City
    {
        /// <summary>
        /// Внутренний ID города
        /// </summary>
        [Key]
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// Название города
        /// </summary>
        [Required]
        [MaxLength(255)]
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Официальный/внешний код города (ОКТМО/ОКАТО или внутренний код)
        /// </summary>
        [MaxLength(50)]
        [Column("code")]
        public string? Code { get; set; }

        // Навигационные свойства
        public virtual ICollection<Budget> Budgets { get; set; } = new List<Budget>();
        public virtual ICollection<DataSource> DataSources { get; set; } = new List<DataSource>();
    }
}