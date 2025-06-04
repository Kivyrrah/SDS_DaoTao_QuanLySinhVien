// SchoolGRPC.Server/Mappings/ClassRoomMap.cs
using FluentNHibernate.Mapping;
using SchoolGRPC.Shared.Models;

namespace SchoolGRPC.Shared.Mappings
{
    public class ClassRoomMap : ClassMap<ClassRoom>
    {
        public ClassRoomMap()
        {
            Table("ClassRoom");
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Name).Not.Nullable().Length(50);
            Map(x => x.Subject).Not.Nullable().Length(100);

            References(x => x.Teacher)
                .Column("TeacherID") 
                .Not.Nullable();

            HasMany(x => x.Students)
                .KeyColumn("ClassRoomID") 
                .Inverse()
                .Cascade.AllDeleteOrphan();
        }
    }
}