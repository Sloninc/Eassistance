using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Eassistance.Domain
{
    public class Steps:Base
    {
        [Required]
        public string Name { get; set; }
        public Guid OperationId { get; set; }
        public string Step { get; set; }

        [ForeignKey("OperationId")]
        public Operations Operation { get; set; }
    }
}
