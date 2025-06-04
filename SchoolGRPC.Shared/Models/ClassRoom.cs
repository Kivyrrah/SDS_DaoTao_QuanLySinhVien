// SchoolGRPC.Server/Models/ClassRoom.cs
using System.Collections.Generic;

namespace SchoolGRPC.Shared.Models
{
    public class ClassRoom
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Subject { get; set; }
        public virtual Teacher Teacher { get; set; } 
        public virtual IList<Student> Students { get; set; } = new List<Student>();
    }
}