
namespace PuntosColombia.MissingNumbers.Models.Searchs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class SearchViewModel:BaseViewModel
    {
        public int SearchId { get; set; }
        public int QuantityListOne { get; set; }
        public string ListOne { get; set; }
        public int QuantityListTwo { get; set; }
        public string ListTwo { get; set; }
        public string Result { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
    }
}
