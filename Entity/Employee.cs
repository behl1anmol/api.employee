using System;
using System.Collections.Generic;

namespace api.employee.Entity
{
    public partial class Employee
    {
        public int Empid
        {
            get; set;
        }
        public string Empname { get; set; } = null!;
        public DateTime Empdob
        {
            get; set;
        }
        public string? Empphonenumber
        {
            get; set;
        }
        public string? Emplocation
        {
            get; set;
        }
    }
}
