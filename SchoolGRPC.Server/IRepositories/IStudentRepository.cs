// SchoolGRPC.Server/Repositories/IStudentRepository.cs
using SchoolGRPC.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolGRPC.Server.IRepositories
{
    public interface IStudentRepository
    {
        Task<Student?> GetByIdAsync(int id);
        Task<Student?> GetByIdWithDetailsAsync(int id); 
        Task<IList<Student>> GetAllAsync();
        Task<IList<Student>> GetAllWithDetailsAsync(); 
        Task AddAsync(Student student);
        Task UpdateAsync(Student student);
        Task DeleteAsync(int id);
        Task<IList<Student>> GetByClassRoomIdAsync(int classRoomId);
    }
}