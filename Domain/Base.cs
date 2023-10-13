using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Eassistance.Domain
{
    public class Base
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
