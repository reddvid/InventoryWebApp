using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ASPNETWebApp48.Models
{
    public class ActivityLog
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Activity { get; set; }
        public string Action { get; set; }
        public DateTime DateTime { get; set; }
    }

    public class TransactionLodge
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string Activity { get; set; }

        public int Quantity { get; set; }

        public decimal CurrentWarehouseQty { get; set; }

        public decimal CurrentStoreQty { get; set; }

        [NotMapped] public decimal Balance => CurrentWarehouseQty + CurrentStoreQty;
    }
}