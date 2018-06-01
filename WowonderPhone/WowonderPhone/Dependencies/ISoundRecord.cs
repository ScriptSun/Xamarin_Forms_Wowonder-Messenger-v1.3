using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WowonderPhone.Dependencies
{
    public  interface ISoundRecord
    {
        void RecordingFunction(string Sound, string Userid);
        Stream GetRecordedSound( string Userid);
        string GetRecordedSoundPath(string Userid);
        string DeleteRecordedSoundPath(string Userid);
        string SoundPlay(string SoundStatus, string messageid , string SoundMCondition, string status , string CurrentSliderValue);
        void SaveAudioToDisk(string Url, string Userid, string messageid);
    }
}
