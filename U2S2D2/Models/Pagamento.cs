using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace U2S2D2.Models
{
    public class Pagamento
    {
        public int ID { get; set; }
        public int IDDipendente { get; set; }
        public string PeriodoPagamento { get; set; }
        public decimal AmmontarePagamento { get; set; }
        public string TipoPagamento { get; set; }
    }
}