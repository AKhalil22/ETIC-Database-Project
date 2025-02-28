using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Expense Entity
// used to create table in db

namespace SpendSmart.Models
{
    public class Expense

    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // auto increment 1, 2, 3...
        public int Id { get; set;}

        [Required]
        public decimal Value { get; set;}

        [Required]
        public string Description { get; set;}


    }
}
