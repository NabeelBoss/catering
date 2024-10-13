using System;
using System.Collections.Generic;

namespace OnlineCatering.Models
{
    public partial class CardInfo
    {
        public int CardId { get; set; }
        public long? CardNumber { get; set; }
        public string? CardName { get; set; }
        public int? CardExMm { get; set; }
        public int? CardExYy { get; set; }
        public int? CardCvv { get; set; }
    }
}
