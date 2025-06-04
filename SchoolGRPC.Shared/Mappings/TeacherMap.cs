// SchoolGRPC.Server/Mappings/TeacherMap.cs
using FluentNHibernate.Mapping;
using SchoolGRPC.Shared.Models;

namespace SchoolGRPC.Shared.Mappings
{
    public class TeacherMap : ClassMap<Teacher>
    {
        public TeacherMap()
        {
            Table("Teacher");
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Name).Not.Nullable().Length(50);
            Map(x => x.Birthday).Not.Nullable();
            HasMany(x => x.ClassRooms)
                .KeyColumn("TeacherID") 
                .Inverse() 
                .Cascade.AllDeleteOrphan(); 
        }
    }
}