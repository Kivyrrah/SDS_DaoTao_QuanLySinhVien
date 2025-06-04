namespace SchoolGRPC.Shared.Models 
{
    public class TeacherClassRoomStudentCountPoco
    {
        public virtual string TeacherName { get; set; } = string.Empty;
        public virtual string ClassRoomName { get; set; } = string.Empty;
        public virtual int ClassRoomID { get; set; } 
        public virtual int StudentCount { get; set; }
    }
}