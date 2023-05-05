namespace TagPlugin.Domain
{
    public class AppleCart
    {
        public AppleCart()
        {
            Apples = new List<Apple>();
        }
        public int Id { get; set; }
        public string Variety { get; set; }
        public DateTime ExpiryDate { get; set; }
        public List<Apple> Apples { get; set; }
    }
}
