using CommunityToolkit.Mvvm.Messaging.Messages;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopOrqApp.Messages
{
    public class LoginSuccessMessage(DataPosition position) : ValueChangedMessage<DataPosition>(position);

}
