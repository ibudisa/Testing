using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopOrqApp.ViewModels
{
    public class TabItemViewModel
    {
        public required string Header { get; set; }

        public required ViewModelBase Content { get; set; }
    }
}
