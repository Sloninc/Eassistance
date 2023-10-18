using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Eassistance.Domain
{
    public class Operation:Base
    {
        [Required]
        public string Name { get; set; }
        [ForeignKey("EquipmentId")]
        public Guid EquipmentId { get; set; }
        public Equipment Equipment { get; set; }
    }
}
