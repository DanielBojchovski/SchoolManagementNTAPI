﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace SchoolManagementNTAPI.Data.Entities;

public partial class Professor
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int SchoolId { get; set; }

    public virtual School School { get; set; }
}