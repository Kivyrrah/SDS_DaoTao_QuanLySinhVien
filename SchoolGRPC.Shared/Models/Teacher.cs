// SchoolGRPC.Server/Models/Teacher.cs
using System;
using System.Collections.Generic;

namespace SchoolGRPC.Shared.Models
{
    public class Teacher
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual DateTime Birthday { get; set; }
        public virtual IList<ClassRoom> ClassRooms { get; set; } = new List<ClassRoom>();
    }
}