using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vpop.ViewModels
{
    public class OrderSnacksViewModel
    {
        public string Custname { get; set; }
        public string Item { get; set; }
        public double Price { get; set; }
        public string Category { get; set; }
        public string Currdate { get; set; }
        public Dictionary<string, string> Snacks1 { get; set; }
        public Dictionary<string, string> Snacks2 { get; set; }

        public OrderSnacksViewModel(Dictionary<string, string> snacks1, Dictionary<string, string> snacks2)
        {
            Snacks1 = snacks1;
            Snacks2 = snacks2;
        }
        public OrderSnacksViewModel() { }
    }
}
