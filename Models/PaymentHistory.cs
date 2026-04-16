using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmyOptimizer.Models
{
    public class PaymentHistory
    {
        public decimal amount { get; set; }
        public string currency { get; set; }
        public int tokens { get; set; }

        public string status { get; set; }

        public string date { get; set; }

        public string description { get; set; }


        public string DateFormatted
        {
            get
            {
                if (DateTime.TryParse(date, out var dt))
                    return dt.ToString("MMM dd, yyyy HH:mm");

                return date;
            }
        }
    }
}
