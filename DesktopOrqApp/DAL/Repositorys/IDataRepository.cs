using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;

namespace DAL.Repositorys
{
    public interface IDataRepository
    {
        Task<DataPosition> GetUserByEmailAndPassword(string email,string password);
        Task<DataPosition> GetInfoByUserId(int userId);
        Task<UserRole> GetUserRoleByUserId(int userid);
        Task<List<WorkPosition>> GetAllWorkPositions();
        Task<WorkPosition> GetWorkPositionById(int id);
        Task<bool> UpdateWorkPosition(WorkPosition workPosition);
        Task<bool> DeleteWorkPosition(int id);
        Task<bool> AddWorkPosition(WorkPosition workPosition);
        Task<List<User_WorkPosition>> GetAllUserWorkPositions();
        Task<bool> AddUserWorkPosition(User_WorkPosition userworkPosition);
        Task<int> GetWorkPositionMaxId();
        Task<int> GetUserWorkPositionMaxId();
        Task<bool> CheckUserWorkPosition(User_WorkPosition position);
    }
}
