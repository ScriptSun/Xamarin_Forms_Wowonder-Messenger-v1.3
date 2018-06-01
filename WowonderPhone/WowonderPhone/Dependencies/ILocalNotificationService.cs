using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WowonderPhone.Dependencies
{
    public interface ILocalNotificationService
    {
        void CreateLocalNotification(string text, string username ,string image, string userid, string Kindoff);
        void CloseNotification(string Id);
    }
}
