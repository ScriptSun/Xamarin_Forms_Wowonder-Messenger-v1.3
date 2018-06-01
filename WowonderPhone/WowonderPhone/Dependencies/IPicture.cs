using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WowonderPhone.Dependencies
{
    public interface IPicture
    {
        void SavePictureToDisk(string imagename, string Userid);

        string GetPictureFromDisk(string imagename, string Userid);

        void DeletePictureFromDisk(string imagename, string Userid);

        void SavePictureToGalery(string imagename);

        string GetPictureFromGalery(string imagename);


    }
}
