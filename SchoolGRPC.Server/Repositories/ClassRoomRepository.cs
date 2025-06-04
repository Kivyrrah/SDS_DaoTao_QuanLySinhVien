// SchoolGRPC.Server/Repositories/ClassRoomRepository.cs
using NHibernate;
using NHibernate.Linq;
using SchoolGRPC.Server.Data;
using SchoolGRPC.Server.IRepositories;
using SchoolGRPC.Shared.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGRPC.Server.Repositories
{
    public class ClassRoomRepository : IClassRoomRepository
    {
        public async Task<ClassRoom?> GetByIdAsync(int id)
        {
            using var session = NHibernateHelper.OpenSession();
            return await session.GetAsync<ClassRoom>(id);
        }

        public async Task<ClassRoom?> GetByIdWithTeacherAsync(int id)
        {
            using var session = NHibernateHelper.OpenSession();
            return await session.Query<ClassRoom>()
                                .Fetch(cr => cr.Teacher) 
                                .Where(cr => cr.Id == id)
                                .SingleOrDefaultAsync();
        }


        public async Task<IList<ClassRoom>> GetAllAsync()
        {
            using var session = NHibernateHelper.OpenSession();
            return await session.Query<ClassRoom>().ToListAsync();
        }

        public async Task<IList<ClassRoom>> GetAllWithTeachersAsync()
        {
            using var session = NHibernateHelper.OpenSession();
            return await session.Query<ClassRoom>()
                                .Fetch(cr => cr.Teacher) 
                                .ToListAsync();
        }


        public async Task AddAsync(ClassRoom classRoom)
        {
            using var session = NHibernateHelper.OpenSession();
            using var transaction = session.BeginTransaction();
            await session.SaveAsync(classRoom);
            await transaction.CommitAsync();
        }

        public async Task UpdateAsync(ClassRoom classRoom)
        {
            using var session = NHibernateHelper.OpenSession();
            using var transaction = session.BeginTransaction();
            await session.UpdateAsync(classRoom); 
            await transaction.CommitAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var session = NHibernateHelper.OpenSession();
            using var transaction = session.BeginTransaction();
            var classRoom = await session.GetAsync<ClassRoom>(id);
            if (classRoom != null)
            {
                await session.DeleteAsync(classRoom);
                await transaction.CommitAsync();
            }
        }

        public async Task<IList<ClassRoom>> GetByTeacherIdAsync(int teacherId)
        {
            using var session = NHibernateHelper.OpenSession();
            return await session.Query<ClassRoom>()
                                .Where(cr => cr.Teacher.Id == teacherId)
                                .Fetch(cr => cr.Teacher) 
                                .ToListAsync();
        }
    }
}