namespace Vpop.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Custname { get; set; }
        public string Item { get; set; }
        public double Price { get; set; }
        public string Category { get; set; }
        public string Currdate { get; set; }
        public Order() { }
        public Order(string custname, string item, string category, string currdate)
        {
            Custname = custname;
            Item = item;
            Category = category;
            Currdate = currdate;
        }
    }
}
