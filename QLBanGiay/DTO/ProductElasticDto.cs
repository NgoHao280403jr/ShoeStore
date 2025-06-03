namespace QLBanGiay.DTO
{
    public class ProductElasticDto
    {
        public long Productid { get; set; }
        public string Productname { get; set; }
        public string? Productdescription { get; set; }
        public long? Price { get; set; }

        public string Categoryname { get; set; }
        public string Parentcategoryname { get; set; }
    }
}
