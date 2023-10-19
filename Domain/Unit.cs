using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eassistance.Domain
{
    public class Unit:Base
    {
        [Required]
        public string Name { get; set; }
    }
}
