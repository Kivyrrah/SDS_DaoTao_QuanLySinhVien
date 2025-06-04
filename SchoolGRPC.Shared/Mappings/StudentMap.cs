// SchoolGRPC.Server/Mappings/StudentMap.cs
using FluentNHibernate.Mapping;
using SchoolGRPC.Shared.Models;

namespace SchoolGRPC.Shared.Mappings
{
    public class StudentMap : ClassMap<Student>
    {
        public StudentMap()
        {
            Table("Student");
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Name).Not.Nullable().Length(50);
            Map(x => x.Birthday).Not.Nullable();
            Map(x => x.Address).Not.Nullable().Length(100);

            References(x => x.ClassRoom)
                .Column("ClassRoomID") 
                .Not.Nullable();
        }
    }
}