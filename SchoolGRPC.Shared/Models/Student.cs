// SchoolGRPC.Server/Models/Student.cs
using System;

namespace SchoolGRPC.Shared.Models
{
    public class Student
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual DateTime Birthday { get; set; }
        public virtual string Address { get; set; }
        public virtual ClassRoom ClassRoom { get; set; } 
    }
}