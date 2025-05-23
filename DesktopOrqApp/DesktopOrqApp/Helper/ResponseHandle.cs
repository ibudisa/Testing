using DesktopOrqApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
using Refit;

namespace DesktopOrqApp.Helper
{
    public class ResponseHandle
    {
        private readonly IApiService _service;

        public ResponseHandle(IApiService service)
        {
            _service = service;
        }
        public List<WorkPosition> GetWP()
        {
            var list = _service.GetPositions().Result;
            var data = new List<WorkPosition>();
            data = list.Content;

            return data;
        }

        public List<User_WorkPosition> GetUserWP()
        {
            List<User_WorkPosition> list=new List<User_WorkPosition>();
            list = _service.GetUserWorkPositions().Result.Content;
            return list;
        }

        public bool AddWP(WorkPosition wp)
        {
            bool result = false;
            result = _service.AddWorkPosition(wp).Result.Content;
            return result;
        }

        public bool AddUserWP(User_WorkPosition userwp)
        {
            bool result = false;
            result = _service.AddUserWorkPosition(userwp).Result.Content;
            return result;
        }
        public bool DeleteWP(int id)
        {
            bool result = false;
            result = _service.DeleteWorkPosition(id).Result.Content;
            return result;
        }

        public bool UpdateWP(WorkPosition position)
        {
            bool result = false;
            result = _service.UpdateWorkPosition(position).Result.Content;
            return result;
        }

        public int GetWPMaxId()
        {
            int result = _service.GetWorkPositionMaxId().Result.Content;
            return (int)result;
        }
        public int GetUWPMaxId()
        {
            int result = _service.GetUserWorkPositionMaxId().Result.Content;
            return (int)result;
        }
    }
}
