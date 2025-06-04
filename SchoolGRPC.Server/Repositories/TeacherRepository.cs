// SchoolGRPC.Server/Repositories/TeacherRepository.cs
using NHibernate;
using NHibernate.Linq;
using SchoolGRPC.Server.Data;
using SchoolGRPC.Server.IRepositories;
using SchoolGRPC.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolGRPC.Server.Repositories
{
    public class TeacherRepository : ITeacherRepository
    {
        public async Task<Teacher?> GetByIdAsync(int id)
        {
            using var session = NHibernateHelper.OpenSession();
            return await session.GetAsync<Teacher>(id);
        }

        public async Task<IList<Teacher>> GetAllAsync()
        {
            using var session = NHibernateHelper.OpenSession();
            return await session.Query<Teacher>().ToListAsync();
        }

        public async Task AddAsync(Teacher teacher)
        {
            using var session = NHibernateHelper.OpenSession();
            using var transaction = session.BeginTransaction();
            await session.SaveAsync(teacher);
            await transaction.CommitAsync();
        }

        public async Task UpdateAsync(Teacher teacher)
        {
            using var session = NHibernateHelper.OpenSession();
            using var transaction = session.BeginTransaction();
            await session.UpdateAsync(teacher);
            await transaction.CommitAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var session = NHibernateHelper.OpenSession();
            using var transaction = session.BeginTransaction();
            var teacher = await session.GetAsync<Teacher>(id);
            if (teacher != null)
            {
                await session.DeleteAsync(teacher);
                await transaction.CommitAsync();
            }
        }
    }
}