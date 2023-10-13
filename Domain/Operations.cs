using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Eassistance.Domain
{
    public class Operations
    {
        [Required]
        public string Name { get; set; }
        public List<string> Steps { get; set; }
        public Guid EquipmentId { get; set; }

        [ForeignKey("EquipmentId")]
        public Equipments Equipment { get; set; }
    }
}
