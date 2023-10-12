using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Eassistance.Domain
{
    public class Equipments
    {
        [Required]
        public string Name { get; set; }
        public string SerialNumber { get; set; }
        public Guid EquipmentId { get; set; }

        [ForeignKey("EquipmentId")]
        public Equipments Equipment { get; set; }
    }
}
