using System;
using System.ComponentModel.DataAnnotations;

namespace GreenPOS.Entity
{
    public class Contact : BaseEntity<long>
    {
        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string LastName { get; set; }
        [MaxLength(100)]
        public string AccountName { get; set; }
        [MaxLength(100)]
        public string VendorName { get; set; }
        public string Email { get; set; }
        [MaxLength(250)]
        public string Title { get; set; }
        [MaxLength(11)]
        public string Phone { get; set; }
        [MaxLength(100)]
        public string Department { get; set; }
        [MaxLength(11)]
        public string HomePhone { get; set; }
        [MaxLength(11)]
        public string HomePhone2 { get; set; }
        [MaxLength(11)]
        public string OtherContect { get; set; }
        [MaxLength(11)]
        public string Mobile { get; set; }
        [MaxLength(11)]
        public string Fax { get; set; }
        [MaxLength(50)]
        public string Assistant { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [MaxLength(100)]
        public string BelongTo { get; set; }
        [MaxLength(11)]
        public string AssistPhone { get; set; }
        public string EmailOpt { get; set; }
        [MaxLength(50)]
        public string City { get; set; }
        [MaxLength(50)]
        public string State { get; set; }
        [MaxLength(50)]
        public string Country { get; set; }

        [MaxLength(50)]
        public string MailingStreet { get; set; }
        [MaxLength(50)]
        public string MailingCity { get; set; }
        [MaxLength(50)]
        public string MailingState { get; set; }
        [MaxLength(50)]
        public string MailingCountry { get; set; }

        [MaxLength(10)]
        public string MailingZip { get; set; }

        public long UserId { get; set; }

    }
}
