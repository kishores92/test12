using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenPOS.Interfaces
{
    public interface IViewModel<T>
    {
        T Id { get; set; }

        [Required]
        long CreatedBy { get; set; }
        DateTime CreatedOn { get; set; }

        [DefaultValue(true)]
        bool IsActive { get; set; }
        long? ModifiedBy { get; set; }
        DateTime? ModifiedOn { get; set; }
    }
}
