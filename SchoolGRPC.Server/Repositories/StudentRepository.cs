// SchoolGRPC.Server/Repositories/StudentRepository.cs
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
    public class StudentRepository : IStudentRepository
    {
        public async Task<Student?> GetByIdAsync(int id)
        {
            using var session = NHibernateHelper.OpenSession();
            return await session.GetAsync<Student>(id);
        }

        public async Task<Student?> GetByIdWithDetailsAsync(int id)
        {
            using var session = NHibernateHelper.OpenSession();
            return await session.Query<Student>()
                                .Where(s => s.Id == id)
                                .Fetch(s => s.ClassRoom) 
                                .ThenFetch(cr => cr.Teacher) 
                                .SingleOrDefaultAsync();
        }

        public async Task<IList<Student>> GetAllAsync()
        {
            using var session = NHibernateHelper.OpenSession();
            return await session.Query<Student>().ToListAsync();
        }

        public async Task<IList<Student>> GetAllWithDetailsAsync()
        {
            using var session = NHibernateHelper.OpenSession();
            return await session.Query<Student>()
                                .Fetch(s => s.ClassRoom)
                                .ThenFetch(cr => cr.Teacher)
                                .ToListAsync();
        }

        public async Task AddAsync(Student student)
        {
            using var session = NHibernateHelper.OpenSession();
            using var transaction = session.BeginTransaction();
            await session.SaveAsync(student);
            await transaction.CommitAsync();
        }

        public async Task UpdateAsync(Student student)
        {
            using var session = NHibernateHelper.OpenSession();
            using var transaction = session.BeginTransaction();
            await session.UpdateAsync(student);
            await transaction.CommitAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var session = NHibernateHelper.OpenSession();
            using var transaction = session.BeginTransaction();
            var student = await session.GetAsync<Student>(id);
            if (student != null)
            {
                await session.DeleteAsync(student);
                await transaction.CommitAsync();
            }
        }

        public async Task<IList<Student>> GetByClassRoomIdAsync(int classRoomId)
        {
            using var session = NHibernateHelper.OpenSession();
            return await session.Query<Student>()
                                .Where(s => s.ClassRoom.Id == classRoomId)
                                .Fetch(s => s.ClassRoom)
                                .ThenFetch(cr => cr.Teacher)
                                .ToListAsync();
        }
    }
}