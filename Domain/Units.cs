using System.ComponentModel.DataAnnotations;
namespace Eassistance.Domain
{
    public class Units:Base
    {
        [Required]
        public string Name { get; set; }
    }
}
