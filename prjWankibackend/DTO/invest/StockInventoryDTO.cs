namespace prjWankibackend.DTO.invest
{
    public class StockInventoryDTO
    {
        public int fInStockId { get; set; }
        public string fMemberId { get; set; }
        public string fBrokerId { get; set; }
        public string fStockId { get; set; }
        public string fStockName { get; set; }
        public string fTrantype { get; set; }
        public int fStockNow { get; set; }
        public int fStockTran { get; set; }
        public decimal fStockPriceN { get; set; } // 現價 (將從 API 更新)
        public decimal fStockPriceT { get; set; }
        public decimal fStockPriceTS { get; set; }
        public decimal fStockCost { get; set; }
        public decimal fEstPro { get; set; }
        public double fEstProP { get; set; }
        public decimal fBalancePrice { get; set; }


    }
}
