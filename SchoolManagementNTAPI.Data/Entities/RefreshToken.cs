﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace SchoolManagementNTAPI.Data.Entities;

public partial class RefreshToken
{
    public int Id { get; set; }

    public string AspNetUserId { get; set; }

    public bool IsValid { get; set; }

    public string TokenHash { get; set; }
}