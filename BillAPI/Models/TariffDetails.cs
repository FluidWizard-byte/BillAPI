namespace BillAPI.Models
{
    public class TariffDetails
    {
        public int tariffDetailsID { get; set; }
        public int startUnit { get; set; }
        public int endUnit { get; set; }
        public decimal energyRate { get; set; }
        public decimal serviceCharge { get; set; }
    }
}
