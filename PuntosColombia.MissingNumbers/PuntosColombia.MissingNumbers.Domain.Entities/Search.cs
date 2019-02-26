
namespace PuntosColombia.MissingNumbers.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class Search
    {
        public int SearchId { get; set; }
        public int QuantityListOne { get; set; }
        public string ListOne { get; set; }
        public int QuantityListTwo { get; set; }
        public string ListTwo { get; set; }
        public string Result { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
