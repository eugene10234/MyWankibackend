﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace prjWankibackend.Models.Database;

public partial class TEmployeeMember
{
    public int FEmployeeSid { get; set; }

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

    public DateOnly? FRegDate { get; set; }

    public string FStatus { get; set; }

    public int? FPermissions { get; set; }

    public string FIp { get; set; }

    public byte[] FMemberImage { get; set; }

    public string FMemberImagePath { get; set; }
}