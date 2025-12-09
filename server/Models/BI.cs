using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityOS.Models
{
    /// <summary>
    /// Модель города   
    /// </summary>
    [Table("bi_entity")]
    public class BIEntity
    {
        /// <summary>
        /// Внутренний ID BI сущности
        /// </summary>
        [Key]
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// Название BI сущности
        /// </summary>
        [Required]
        [MaxLength(255)]
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// код BI сущности (ОКТМО/ОКАТО или внутренний код)
        /// </summary>
        [MaxLength(50)]
        [Column("code")]
        public string? Code { get; set; }

        /// <summary>
        /// Комментарий: откуда набор, как устроен, примечания
        /// </summary>
        [Column("description", TypeName = "text")]
        public string? Description { get; set; }

    }


    [Table("bi_field")]
    public class BIField
    {
        /// <summary>
        /// Внутренний ID BI поля
        /// </summary>
        [Key]
        [Column("id")]
        public int Id { get; set; }

       
        /// <summary>
        /// Внутренний ID сущности
        /// </summary>
        [Column("entity_id")]
        public int EntityId { get; set; }

        /// <summary>
        /// Название BI поля
        /// </summary>
        [Required]
        [MaxLength(255)]
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Код BI поля (используется для связей и импорта)
        /// Системное имя поля: year, value, income_source_id
        /// </summary>
        [MaxLength(255)]
        [Column("code")]
        public string? Code { get; set; }

        /// <summary>
        /// Тип данных BI поля
        /// string / number / date / boolean / ref
        /// </summary>
        [Required]
        [MaxLength(255)]
        [Column("data_type")]
        public string DataType { get; set; } = string.Empty;

        /// <summary>
        /// Поле обязательно для заполнения
        /// </summary>  
        [Required]
        [Column("is_required")]
        public bool IsRequired { get; set; } = false;

        /// <summary>
        /// Поле используется как ключ
        /// Если поле является ключевым, то оно является уникальным и не может быть null
        /// Если поле не является ключевым, то оно может быть null  
        /// </summary>  
        [Required]
        [Column("is_key")]
        public bool IsKey { get; set; } = false;

        /// <summary>
        /// Порядковый номер поля
        /// </summary>
        [Column("order_index")]
        public int OrderIndex { get; set; } = 0;

        /// <summary>
        /// Внутренний ID ссылочной сущности
        /// </summary>
        [Column("ref_entity_id")]
        public int RefEntityId { get; set; }

    }

    [Table("bi_relation")]
    public class BIRelation
    {
        /// <summary>
        /// Внутренний ID связи
        /// </summary>
        [Key]
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// ID исходной сущности
        /// </summary>
        [Column("from_entity_id")]
        public int FromEntityId { get; set; }

        /// <summary>
        /// ID целевой сущности
        /// </summary>
        [Column("to_entity_id")]
        public int ToEntityId { get; set; }

        /// <summary>
        /// Код поля исходной сущности
        /// </summary>
        [Column("from_field_code")]
        public string FromFieldCode { get; set; } = string.Empty;

        /// <summary>
        /// Код поля целевой сущности
        /// </summary>
        [Column("to_field_code")]
        public string ToFieldCode { get; set; } = string.Empty;

        /// <summary>
        /// Тип связи
        /// one_to_one / one_to_many / many_to_one / many_to_many
        /// </summary>
        [Column("relation_type")]
        public string RelationType { get; set; } = string.Empty;
    }

    // Универсальное хранилище строк сущностей (данные в JSON)
    [Table("bi_entity_row")]
    public class BIEntityRow
    {
        /// <summary>
        /// Внутренний ID строки
        /// </summary>
        [Key]
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// Внутренний ID сущности
        /// </summary>
        [Column("entity_id")]
        public int EntityId { get; set; }

        /// <summary>
        /// Строка данных в JSON
        /// </summary>
        [Column("data", TypeName = "jsonb")]
        public string Data { get; set; } = "{}";
        
        /// <summary>
        /// Дата и время создания
        /// </summary>
        [Column("created_at", TypeName = "timestamp")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }


}