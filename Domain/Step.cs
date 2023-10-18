using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Eassistance.Domain
{
    public class Step:Base
    {
        [Required]
        public string Name { get; set; }
        [ForeignKey("OperationId")]
        public Guid OperationId { get; set; }
        public string Body { get; set; }
        public Operation Operation { get; set; }
    }
}
