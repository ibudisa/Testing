using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DAL.Entities;

namespace DesktopOrqApp.ViewModels
{
    public partial class UserPrivilegeViewModel:ViewModelBase
    {
        [ObservableProperty] private string _workpos;

        public UserPrivilegeViewModel(DataPosition pos)
        {
            Workpos = pos.WorkPositionInfo;
        }
    }
}
