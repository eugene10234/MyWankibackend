﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace prjWankibackend.Models.Database;

public partial class TMatch
{
    public int FMatchId { get; set; }

    public int? FHelpId { get; set; }

    public string FMemberId { get; set; }

    public DateTime? FMatchDateTime { get; set; }

    public int? FPoint { get; set; }

    public int? FMatchStatus { get; set; }

    public int? FGrade { get; set; }

    public DateTime? FGradeDateTime { get; set; }

    public string FMessage { get; set; }

    public string FHelperEmail { get; set; }

    public string FHelperName { get; set; }
}