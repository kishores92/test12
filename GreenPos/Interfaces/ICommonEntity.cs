using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenPOS.Interfaces
{
    public interface ICommonEntity<T>
    {
        [Key]
        T Id { get; set; }

        long CreatedBy { get; set; }
        DateTime CreatedOn { get; set; }
        bool IsActive { get; set; }
        long? ModifiedBy { get; set; }
        DateTime? ModifiedOn { get; set; }

    }
}
