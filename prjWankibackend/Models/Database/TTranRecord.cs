﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace prjWankibackend.Models.Database;

public partial class TTranRecord
{
    public int FTranId { get; set; }

    public string FMemberId { get; set; }

    public string FStockId { get; set; }

    public string FBrokerId { get; set; }

    public string FTranType { get; set; }

    public string FBuySell { get; set; }

    public int FStockQty { get; set; }

    public decimal FStockPrice { get; set; }

    public DateTime FTranTime { get; set; }
}