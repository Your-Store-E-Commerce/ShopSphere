namespace ShopSphere.Data.Entities.Order
{
    public class ProductOrderItem
    {
        private ProductOrderItem()
        {

        }
        public ProductOrderItem(int productId, string productName, string productPictureUrl)
        {
            ProductId = productId;
            ProductName = productName;
            ProductPictureUrl = productPictureUrl;
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string ProductPictureUrl { get; set; } = null!;
    }
}