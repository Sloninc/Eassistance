using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Eassistance.Domain
{
    public class Equipment:Base
    {
        [Required]
        public string Name { get; set; }
        public string SerialNumber { get; set; }
        [ForeignKey("UnitId")]
        public Guid UnitId { get; set; }
        public Unit Unit { get; set; }
    }
}
