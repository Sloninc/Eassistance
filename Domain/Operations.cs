using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Eassistance.Domain
{
    public class Operations
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid UnitId { get; set; }

        [ForeignKey("UnitId")]
        public Units Unit { get; set; }
    }
}
