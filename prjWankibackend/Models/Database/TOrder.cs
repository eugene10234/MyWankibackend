﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace prjWankibackend.Models.Database;

public partial class TOrder
{
    public int FOrderId { get; set; }

    public string FPersonSid { get; set; }

    public int? FTotalHelpPoint { get; set; }

    public int? FStatus { get; set; }

    public DateTime? FOrderTime { get; set; }

    public int? FExecStatus { get; set; }

    public DateTime? FBeginTime { get; set; }

    public DateTime? FFinishTime { get; set; }

    public byte[] FProof { get; set; }
}