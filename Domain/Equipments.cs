using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Eassistance.Domain
{
    public class Equipments:Base
    {
        [Required]
        public string Name { get; set; }
        public string SerialNumber { get; set; }
        public Guid UnitId { get; set; }

        [ForeignKey("UnitId")]
        public Units Unit { get; set; }
    }
}
