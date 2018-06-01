using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WowonderPhone.Dependencies
{
    public  interface IVideo
    {
        Stream GetRecordedVideo( string Userid);
        string GetVideoPath(string Userid);
        string DeleteVideoPath(string Userid);
        string VedioPlay(string VidioUrl);
        void SaveVideoToGalery(string Url, string Userid, string messageid);
    }
}
