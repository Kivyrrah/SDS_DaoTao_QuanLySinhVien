// SchoolGRPC.Server/Repositories/IClassRoomRepository.cs
using SchoolGRPC.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolGRPC.Server.IRepositories
{
    public interface IClassRoomRepository
    {
        Task<ClassRoom?> GetByIdAsync(int id);
        Task<ClassRoom?> GetByIdWithTeacherAsync(int id); 
        Task<IList<ClassRoom>> GetAllAsync();
        Task<IList<ClassRoom>> GetAllWithTeachersAsync(); 
        Task AddAsync(ClassRoom classRoom);
        Task UpdateAsync(ClassRoom classRoom);
        Task DeleteAsync(int id);
        Task<IList<ClassRoom>> GetByTeacherIdAsync(int teacherId);
    }
}
