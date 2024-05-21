using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MyBGList.Models
{
    [Table("Publishers")]
    public class Publisher
    {
        [Key]
        [NotNull] 
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        [Required]
        public DateTime LastModifiedDate { get; set; }

        public ICollection<BoardGame>? BoardGames { get; set; }
    }
}
