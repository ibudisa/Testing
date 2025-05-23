using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DataPosition
    {
        public string UserRoleInfo { get; set; }
        public string WorkPositionInfo { get; set; }

        public DataPosition()
        {

        }
        public DataPosition(string _userRoleInfo, string _workPositionInfo)
        {
            UserRoleInfo = _userRoleInfo;
            WorkPositionInfo = _workPositionInfo;
        }
    }
}
