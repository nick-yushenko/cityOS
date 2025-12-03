using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CityOS.Models
{
    /// <summary>
    /// Модель источника данных
    /// </summary>
    [Table("data_source")]
    public class DataSource
    {
        /// <summary>
        /// ID источника данных
        /// </summary>
        [Key]
        [Column("id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)] // Игнорируем Id = 0 при сериализации ответа, но принимаем при десериализации
        public int Id { get; set; }

        /// <summary>
        /// Для какого города этот источник
        /// </summary>
        [Required]
        [Column("city_id")]
        public int CityId { get; set; }

        /// <summary>
        /// Тип набора: PLAN_INCOME или EXECUTION_INCOME и т.п.
        /// </summary>
        [Required]
        [MaxLength(50)]
        [Column("dataset_kind")]
        public string DatasetKind { get; set; } = string.Empty;

        /// <summary>
        /// Прямая ссылка на скачивание архива с данными
        /// </summary>
        [Required]
        [Column("source_url", TypeName = "text")]
        public string SourceUrl { get; set; } = string.Empty;

        /// <summary>
        /// csv / xlsx / zip (архив, внутри csv/xlsx)
        /// </summary>
        [Required]
        [MaxLength(10)]
        [Column("file_type")]
        public string FileType { get; set; } = string.Empty;

        /// <summary>
        /// Название источника данных
        /// </summary>
        [Column("name", TypeName = "text")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Комментарий: откуда набор, как устроен, примечания
        /// </summary>
        [Column("description", TypeName = "text")]
        public string? Description { get; set; }

        /// <summary>
        /// Использовать ли этот источник сейчас
        /// </summary>
        [Required]
        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Когда мы в последний раз успешно обновлялись из этого источника
        /// </summary>
        [Column("last_loaded_at", TypeName = "timestamp")]
        public DateTime? LastLoadedAt { get; set; }

        /// <summary>
        /// Хэш файла/ETag, чтобы понять, что файл изменился (опционально)
        /// </summary>
        [MaxLength(255)]
        [Column("last_checksum")]
        public string? LastChecksum { get; set; }

        // Навигационные свойства
        [ForeignKey(nameof(CityId))]
        [JsonIgnore] // Не сериализуем навигационное свойство в API
        [ValidateNever] // Не валидируем навигационное свойство
        public virtual City City { get; set; } = null!;
    }

    /// <summary>
    /// DTO для частичного обновления источника данных (PATCH)
    /// Все поля опциональны для частичного обновления
    /// </summary>
    public class UpdateDataSourcePayload
    {
        public int? CityId { get; set; }
        public string? DatasetKind { get; set; }
        public string? SourceUrl { get; set; }
        public string? FileType { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public string? LastChecksum { get; set; }
    }


}


