using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirlineTicketingSystemAPI.Model
{
    [Table("Plane")]
    public class Plane
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public required string Code { get; set; }
        [Required]
        [Range(100, 800)]
        public int Capacity { get; set; }
        [Required]
        [Range(100, 700)]
        public double Speed { get; set; } // Miles per hour 
    }
}
