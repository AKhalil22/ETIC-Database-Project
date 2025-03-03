using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class Expense
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")] // Limit ints: 18 digits, 2 decimal places
        public decimal Amount { get; set; }

        [Required]
        public string? Description { get; set; }
    }
}