namespace MyProductApi.Models
{
    public class product
    {
        public int product_id { get; set; } // TODO: double check
        public string name { get; set; }
        public string slug { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }
        public string SKU { get; set; }
        public string stock_quantity { get; set; }
        public string img_url { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }

    }
}
