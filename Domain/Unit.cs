using System.ComponentModel.DataAnnotations;
namespace Eassistance.Domain
{
    public class Unit:Base
    {
        [Required]
        public string Name { get; set; }
    }
}
