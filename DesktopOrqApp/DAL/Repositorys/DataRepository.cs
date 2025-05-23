using DAL.Entities;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositorys
{
    public class DataRepository : IDataRepository
    {
        private readonly UserDBContext _ctx;
        public DataRepository(UserDBContext ctx) 
        {
            _ctx = ctx;
        }
        public async Task<bool> AddUserWorkPosition(User_WorkPosition userworkPosition)
        {
            bool value=await CheckUserWorkPosition(userworkPosition);
            if (value)
            {
                var uw = await _ctx.User_WorkPositions.AddAsync(userworkPosition);
                _ctx.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }

        }
        public async Task<bool> CheckUserWorkPosition(User_WorkPosition position)
        {
            bool checkUser=await _ctx.Users.AnyAsync(p=>p.Id==position.UserId);
            bool checkPosition=await _ctx.WorkPositions.AnyAsync(p=>p.Id==position.WorkPositionId);

            bool result = checkUser && checkPosition;

            return result;
        }
        public async Task<bool> AddWorkPosition(WorkPosition workPosition)
        {
            var w= await _ctx.WorkPositions.AddAsync(workPosition);
            _ctx.SaveChanges();
            return true;
        }

        public async Task<bool> DeleteWorkPosition(int id)
        {
            var result = await _ctx.WorkPositions
                 .FirstOrDefaultAsync(c => c.Id == id);
            if (result != null)
            {
                _ctx.WorkPositions.Remove(result);
                _ctx.SaveChanges();
            }
            return true;
        }

        public async Task<List<User_WorkPosition>> GetAllUserWorkPositions()
        {
            var list = await _ctx.User_WorkPositions.ToListAsync();    
            return list;
        }

        public async Task<List<WorkPosition>> GetAllWorkPositions()
        {
            var list =await _ctx.WorkPositions.ToListAsync();
            return list;
        }

        public async Task<DataPosition> GetInfoByUserId(int userId)
        {
            var role =await GetUserRoleByUserId(userId);
            var authinfo = role.Value;
            var user_workposition =await _ctx.User_WorkPositions.FirstOrDefaultAsync(c => c.Id == userId);
            var wpid = user_workposition.WorkPositionId;
            var workposition =await _ctx.WorkPositions.FirstOrDefaultAsync(w => w.Id == wpid);
            var positionname = workposition.Name;

            DataPosition dataPosition = new DataPosition(authinfo, positionname);

            return dataPosition;
        }

        public async Task<DataPosition> GetUserByEmailAndPassword(string email, string password)
        {
            DataPosition userdata = new DataPosition();
            var user=_ctx.Users.FirstOrDefault(c => c.Email.Equals(email)&& c.Password.Equals(password));
            if (user != null)
            {
                userdata=await GetInfoByUserId(user.Id);
            }
            return userdata;
        }

        public async Task<UserRole> GetUserRoleByUserId(int userid)
        {
            var roleid = 0;
            var user_userrole=await _ctx.User_UserRoles.FirstOrDefaultAsync(c => c.UserId == userid);
            if (user_userrole != null)
                roleid = user_userrole.UserRoleId;
            var role=await _ctx.UserRoles.FirstOrDefaultAsync(p=>p.Id==roleid);
            return role;

        }

        public async Task<WorkPosition> GetWorkPositionById(int id)
        {
            var pos=await _ctx.WorkPositions.FirstOrDefaultAsync(c=>c.Id==id);
            return pos;
        }

        public async Task<int> GetWorkPositionMaxId()
        {
            int Id =await _ctx.WorkPositions.MaxAsync(u => u.Id);
            return Id;
        }

        public async Task<int> GetUserWorkPositionMaxId()
        {
            int Id = await _ctx.User_WorkPositions.MaxAsync(u => u.Id);
            return Id;
        }
        public async Task<bool> UpdateWorkPosition(WorkPosition workPosition)
        {
            var result =await _ctx.WorkPositions.FirstOrDefaultAsync(c => c.Id == workPosition.Id);
            if (result != null)
            {
                result.Name = workPosition.Name;
                result.Description = workPosition.Description;
              
                _ctx.SaveChanges();
            }
            return true;
        }
    }
}
