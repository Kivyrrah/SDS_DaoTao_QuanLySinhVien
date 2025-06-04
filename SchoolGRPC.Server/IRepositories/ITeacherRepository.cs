// SchoolGRPC.Server/Repositories/ITeacherRepository.cs
using SchoolGRPC.Server.Data;
using SchoolGRPC.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolGRPC.Server.IRepositories
{
    public interface ITeacherRepository
    {
        Task<Teacher?> GetByIdAsync(int id);
        Task<IList<Teacher>> GetAllAsync();
        Task AddAsync(Teacher teacher);
        Task UpdateAsync(Teacher teacher);
        Task DeleteAsync(int id);
    }
}