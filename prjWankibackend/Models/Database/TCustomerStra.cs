﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace prjWankibackend.Models.Database;

public partial class TCustomerStra
{
    public int FStraId { get; set; }

    public string FMemberId { get; set; }

    public int FStockId { get; set; }

    public decimal? FPriceSet { get; set; }

    public string FTranType { get; set; }

    public bool? FBuySell { get; set; }

    public decimal? F5Ma { get; set; }

    public decimal? F10Ma { get; set; }

    public decimal? F20Ma { get; set; }

    public int? FKvalue { get; set; }

    public int? FDvalue { get; set; }
}