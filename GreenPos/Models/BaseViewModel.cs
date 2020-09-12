using GreenPOS.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenPOS.Models
{
    public class BaseViewModel<T> : IViewModel<T>
    {
        public T Id { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }

        public bool IsActive { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
