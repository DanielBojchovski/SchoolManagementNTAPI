﻿namespace SchoolManagementNTAPI.Professor.Requests
{
    public class CreateProfessorRequest
    {
        public string Name { get; set; } = string.Empty;
        public int SchoolId { get; set; }
    }
}
