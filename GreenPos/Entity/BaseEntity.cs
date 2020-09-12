using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GreenPOS.Interfaces;

namespace GreenPOS.Entity
{
    public class BaseEntity<T> : ICommonEntity<T>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public T Id { get; set; }

        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; } = true;

        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
