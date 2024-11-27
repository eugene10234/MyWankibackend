// DTO for TOrderDetail
public class OrderDetailDto
{
    public int FOrderDetailId { get; set; }
    public int FOrderId { get; set; }
    public int? FProductId { get; set; }
    public int? FAmount { get; set; }
    public int? FHelpPoint { get; set; }
}

// DTO for TOrder
public class OrderDto
{
    public int FOrderId { get; set; }
    public string fPersonSId { get; set; }
    public int? FTotalHelpPoint { get; set; }
    public int? FStatus { get; set; }
    public DateTime? FOrderTime { get; set; }
    public int? FExecStatus { get; set; }
    public DateTime? FBeginTime { get; set; }
    public DateTime? FFinishTime { get; set; }
    public byte[] FProof { get; set; }
}
