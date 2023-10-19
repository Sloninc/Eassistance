using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Eassistance.Domain
{
    public class Operation : Base
    {
        [Required]
        public string Name { get; set; }
        public Guid EquipmentId { get; set; }
        [ForeignKey("EquipmentId")]
        public Equipment Equipment { get; set; }
    }
}
