using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GreenPOS.Entity
{
    public class Quote : BaseEntity<long>
    {
        
        public string CustomerFirstName { get; set; }

        
        public string CustomerLastName { get; set; }

        
        public string CustomerContact { get; set; }

        
        public string CustomerEmail { get; set; }

        public string CustomerAddress { get; set; }


        public string CustomerProjectAddress { get; set; }


        public string Budget { get; set; }


        public string Supervisor { get; set; }

        public string FloorPlan { get; set; }

        public string Facade { get; set; }
        public string Status { get; set; }
        public string SupervisorContact { get; set; }
        public string Notes { get; set; }
        public int ValidDays { get; set; }

        public DateTime? DepositDate { get; set; }
        public DateTime? ValidUntil { get; set; }
        public long? DepositAmount { get; set; }

        public long? InclusionId { get; set; }

        public int? HouseDesignId { get; set; }
        public int? FacadeId { get; set; }
        public decimal? BuildPrice { get; set; }

        public string GarageHand { get; set; }

        public string FrontAlignment { get; set; }

        public string DocUrl { get; set; }

        public string PdfUrl { get; set; }

        public bool? IsTender { get; set; }
    }
}
