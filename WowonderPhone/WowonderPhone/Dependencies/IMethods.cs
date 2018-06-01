using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WowonderPhone.Dependencies
{
    public interface IMethods
    {
        void OpenImage(string Directory, string image, string Userid);

        void OpenWebsiteUrl(string Website);

        string UploudAttachment(Stream stream, string Filepath, string user_id, string recipient_id, string session, string text, string time2);

        void SaveContactName(string Number, string Name);

        void OpenMessengerApp(string Packegename);
    }
}
