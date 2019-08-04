namespace GameStore.Models.Entity
{
    public class PaymentSetting
    {
        public int Id { get; set; }
        public int Tax { get; set; }
        public decimal ShippingCharge { get; set; }
        public decimal Credit_Ratio { get; set; }
    }
}
