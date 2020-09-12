using System.ComponentModel.DataAnnotations;

namespace GreenPOS.Entity
{
    public class ScreenPermission : BaseEntity<long>
    {
        public int ScreenId { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }
    }
}
