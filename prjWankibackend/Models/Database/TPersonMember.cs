﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace prjWankibackend.Models.Database;

public partial class TPersonMember
{
    public int FPersonSid { get; set; }

    public string FMemberId { get; set; }

    public string FAccount { get; set; }

    public string FPassword { get; set; }

    public string FUserName { get; set; }

    public string FFirstName { get; set; }

    public string FLastName { get; set; }

    public string FEmail { get; set; }

    public string FIdentification { get; set; }

    public DateOnly? FBirthDate { get; set; }

    public string FSex { get; set; }

    public DateTime? FRegDate { get; set; }

    public int? FTotalHelpPoint { get; set; }

    public int? FTotalAsset { get; set; }

    public int? FDistrictId { get; set; }

    public string FStatus { get; set; }

    public int? FPermissions { get; set; }

    public string FIp { get; set; }

    public byte[] FMemberImage { get; set; }

    public string FMemberImagePath { get; set; }

    public bool? FEmailVerified { get; set; }

    public string FProvider { get; set; }

    public string FSubId { get; set; }

    public DateTime? FLastLoginAt { get; set; }
}