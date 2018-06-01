using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using WowonderPhone.Classes;
using WowonderPhone.Dependencies;
using WowonderPhone.Languish;
using WowonderPhone.SQLite;
using WowonderPhone.SQLite.Tables;
using Xamarin.Forms;

namespace WowonderPhone.Controls
{
    public class MessageController
    {


        public static void MessageType_Insert(JToken MessageInfo ,string recipient_id)
        {
            JObject ChatlistUserdata = JObject.FromObject(MessageInfo);
            var Blal = ChatlistUserdata["messageUser"];
            var avatar = Blal["avatar"].ToString();
            var from_id = MessageInfo["from_id"].ToString();
            var to_id = MessageInfo["to_id"].ToString();
            var media = MessageInfo["media"].ToString();
            var mediaFileName = MessageInfo["mediaFileName"].ToString();
            var deleted_one = MessageInfo["mediaFileName"].ToString();
            var deleted_two = MessageInfo["mediaFileName"].ToString();
            var Messageid = MessageInfo["id"].ToString();
            int Messageidx = Int32.Parse(Messageid);
            var Position = MessageInfo["position"].ToString();
            var TextMsg = MessageInfo["text"].ToString();
            var Type = MessageInfo["type"].ToString();
            var TimeMsg = MessageInfo["time_text"].ToString();
            var msgID = MessageInfo["id"].ToString();
            var isVisible = true;

            if (TextMsg == ""){ isVisible = false; }
            else{TextMsg = Functions.DecodeString(TextMsg); }

            try
            {
                    if (Position == "left")
                    {
                        var ImageProfile = ImageSource.FromFile(DependencyService.Get<IPicture>() .GetPictureFromDisk(avatar, recipient_id));

                        #region left_text

                        if (Type == "left_text")
                        {
                            Functions.Messages.Add(new MessageViewModal()
                            {
                                Content = TextMsg,
                                Type = Type,
                                Position = Position,
                                CreatedAt = TimeMsg,
                                messageID = msgID,
                                UserImage = ImageProfile,
                                CommingBackroundBoxColor = Settings.MS_CommingBackroundBox_Color

                            });
                        }

                        #endregion

                        #region Left Image

                        if (Type == "left_image")
                        {
                            var ImageUrl = MessageInfo["media"].ToString();
                            DependencyService.Get<IPicture>().SavePictureToGalery(ImageUrl);
                            var ImageMediaFile =
                                ImageSource.FromFile(
                                    DependencyService.Get<IPicture>().GetPictureFromGalery(ImageUrl));

                            if (DependencyService.Get<IPicture>().GetPictureFromGalery(ImageUrl) ==
                                "File Dont Exists")
                            {
                                ImageMediaFile = new UriImageSource
                                {
                                    Uri = new Uri(ImageUrl)
                                };
                            }

                            Functions.Messages.Add(new MessageViewModal()
                            {
                                UserImage = ImageProfile,
                                Visibilty = isVisible,
                                Content = TextMsg,
                                Type = Type,
                                Position = Position,
                                CreatedAt = TimeMsg,
                                ImageMedia = ImageMediaFile,
                                messageID = msgID,
                                ImageUrl = ImageUrl,
                                CommingBackroundBoxColor = Settings.MS_CommingBackroundBox_Color
                            });

                        }

                        #endregion

                        #region left Audio

                        if (Type == "//left_audio")
                        {
                            var audioUrl = MessageInfo["media"].ToString();

                            if (Settings.DownloadSoundFiles_Automatically_Without_Click)
                            {
                                DependencyService.Get<ISoundRecord>()
                                    .SaveAudioToDisk(audioUrl, Settings.User_id, msgID);
                            }

                            Functions.Messages.Add(new MessageViewModal()
                            {
                                UserImage = ImageProfile,
                                Visibilty = isVisible,
                                Content = TextMsg,
                                Type = Type,
                                Position = Position,
                                CreatedAt = TimeMsg,
                                messageID = msgID,
                                Media = audioUrl,
                                ImageMedia = Settings.MS_DownloadBlackicon,
                                DownloadFileUrl = "NotDownload",
                                MediaName = AppResources.Label_DownloadSound,
                                from_id = from_id,
                                SliderSoundValue = "0",
                                SliderMaxDuration = "100",
                                CommingBackroundBoxColor = Settings.MS_CommingBackroundBox_Color
                            });

                        }

                        #endregion

                        #region Left video

                        if (Type == "left_video")
                        {
                            var videoUrl = MessageInfo["media"].ToString();

                            Functions.Messages.Add(new MessageViewModal()
                            {
                                UserImage = ImageProfile,
                                Visibilty = isVisible,
                                Content = TextMsg,
                                Type = Type,
                                Position = Position,
                                CreatedAt = TimeMsg,
                                messageID = msgID,
                                Media = videoUrl,
                                ImageMedia = Settings.MS_DownloadBlackicon,
                                DownloadFileUrl = "NotDownload",
                                MediaName = AppResources.Label_DownloadVideo,
                                from_id = from_id,
                                CommingBackroundBoxColor = Settings.MS_CommingBackroundBox_Color
                            });

                        }

                        #endregion

                        #region left_contact

                        if (Type == "left_contact")
                        {
                            TextMsg = TextMsg.Replace("{", "").Replace("}", "").Replace("Key:", "").Replace("Value:", "");
                            string[] Con = TextMsg.Split(',');
                            var dd = Con[0].ToString();
                            Functions.Messages.Add(new MessageViewModal()
                            {
                                Content = Con[0],
                                Type = Type,
                                Position = Position,
                                CreatedAt = TimeMsg,
                                messageID = msgID,
                                ImageMedia = "UserContact.png",
                                ContactName = Con[0].ToString(),
                                ContactNumber = Con[1].ToString(),
                                CommingBackroundBoxColor = Settings.MS_CommingBackroundBox_Color,
                                UserImage = ImageProfile,
                            });
                        }

                        #endregion

                        #region left_sticker

                        if (Type == "left_sticker")
                        {
                            
                            Functions.Messages.Add(new MessageViewModal()
                            {
                                Type = Type,
                                Position = Position,
                                CreatedAt = TimeMsg,
                                messageID = msgID,
                                ImageMedia = media,
                                CommingBackroundBoxColor = Settings.MS_CommingBackroundBox_Color,
                                UserImage = ImageProfile,
                            });
                        }

                        #endregion

                }
                else
                    {
                        var ImageProfile = ImageSource.FromFile(DependencyService.Get<IPicture>()
                            .GetPictureFromDisk(avatar, Settings.User_id));
                        if (TextMsg == "")
                        {
                            isVisible = false;
                        }

                        #region right_text

                        if (Type == "right_text")
                        {

                            Functions.Messages.Add(new MessageViewModal()
                            {
                                Content = TextMsg,
                                Type = Type,
                                Position = Position,
                                CreatedAt = TimeMsg,
                                messageID = msgID,
                                UserImage = ImageProfile,
                                GoingBackroundBoxColor = Settings.MS_GoingBackroundBox_Color
                            });
                        }

                        #endregion

                        #region Left Image

                        if (Type == "right_image")
                        {
                            var ImageUrl = MessageInfo["media"].ToString();

                            var ImageMediaFile =
                                ImageSource.FromFile(
                                    DependencyService.Get<IPicture>()
                                        .GetPictureFromDisk(ImageUrl, Settings.User_id));
                            if (
                                DependencyService.Get<IPicture>()
                                    .GetPictureFromDisk(ImageUrl, Settings.User_id) ==
                                "File Dont Exists")
                            {
                                DependencyService.Get<IPicture>()
                                    .SavePictureToDisk(ImageUrl, Settings.User_id);
                                ImageMediaFile = new UriImageSource {Uri = new Uri(ImageUrl)};
                            }

                            Functions.Messages.Add(new MessageViewModal()
                            {
                                UserImage = ImageProfile,
                                Visibilty = isVisible,
                                Content = TextMsg,
                                Type = Type,
                                Position = Position,
                                CreatedAt = TimeMsg,
                                ImageMedia = ImageMediaFile,
                                messageID = msgID,
                                ImageUrl = ImageUrl,
                                GoingBackroundBoxColor = Settings.MS_GoingBackroundBox_Color
                            });

                        }

                        #endregion

                        #region right Sound

                        if (Type == "//right_audio")
                        {
                            var audioUrl = MessageInfo["media"].ToString();

                            if (Settings.DownloadSoundFiles_Automatically_Without_Click)
                            {
                                DependencyService.Get<ISoundRecord>()
                                    .SaveAudioToDisk(audioUrl, Settings.User_id, msgID);
                            }

                            Functions.Messages.Add(new MessageViewModal()
                            {
                                UserImage = ImageProfile,
                                Visibilty = isVisible,
                                Content = TextMsg,
                                Type = Type,
                                Position = Position,
                                CreatedAt = TimeMsg,
                                messageID = msgID,
                                Media = audioUrl,
                                ImageMedia = Settings.MS_DownloadWhiteicon,
                                DownloadFileUrl = "NotDownload",
                                MediaName = AppResources.Label_DownloadSound,
                                from_id = from_id,
                                SliderSoundValue = "0",
                                SliderMaxDuration = "100",
                                GoingBackroundBoxColor = Settings.MS_GoingBackroundBox_Color
                            });

                        }

                        #endregion

                        #region right Video

                        if (Type == "right_video")
                        {
                            var videoUrl = MessageInfo["media"].ToString();
                            // DependencyService.Get<ISoundRecord>().SaveAudioToDisk(audioUrl, Settings.User_id , Messageid);

                            Functions.Messages.Add(new MessageViewModal()
                            {
                                UserImage = ImageProfile,
                                Visibilty = isVisible,
                                Content = TextMsg,
                                Type = Type,
                                Position = Position,
                                CreatedAt = TimeMsg,
                                messageID = msgID,
                                Media = videoUrl,
                                ImageMedia = Settings.MS_DownloadWhiteicon,
                                DownloadFileUrl = "NotDownload",
                                MediaName = AppResources.Label_DownloadVideo,
                                from_id = from_id,
                                GoingBackroundBoxColor = Settings.MS_GoingBackroundBox_Color
                            });

                        }

                        #endregion

                        #region right_contact

                        if (Type == "right_contact")
                        {
                            TextMsg = TextMsg.Replace("{", "").Replace("}", "").Replace("Key:", "").Replace("Value:", "");
                             string[] Con = TextMsg.Split(',');

                            Functions.Messages.Add(new MessageViewModal()
                            {
                                Content = Con[0],
                                Type = Type,
                                Position = Position,
                                CreatedAt = TimeMsg,
                                messageID = msgID,
                                ImageMedia = "UserContact.png",
                                ContactName = Con[0],
                                ContactNumber = Con[1],
                                GoingBackroundBoxColor = Settings.MS_GoingBackroundBox_Color,
                                UserImage = ImageProfile,
                            });
                        }

                    #endregion

                        #region right_sticker

                        if (Type == "right_sticker")
                        {

                            Functions.Messages.Add(new MessageViewModal()
                            {
                                Type = Type,
                                Position = Position,
                                CreatedAt = TimeMsg,
                                messageID = msgID,
                                ImageMedia = media,
                                UserImage = ImageProfile,
                            });
                        }

                        #endregion
                }

            }
            catch (Exception)
            {

            }

            try
            {
                #region Insert to database
                var ImageMedia = "";
                var DownloadFileUrl = "NotDownload";

                if (Type == "//left_audio")
                {
                    ImageMedia = Settings.MS_DownloadBlackicon;
                    mediaFileName = AppResources.Label_DownloadSound;
                }
                else if (Type == "//right_audio")
                {
                    ImageMedia = Settings.MS_DownloadWhiteicon;
                    mediaFileName = AppResources.Label_DownloadSound;
                }
                else if (Type == "right_video")
                {
                    ImageMedia = Settings.MS_DownloadWhiteicon;
                    mediaFileName = AppResources.Label_DownloadVideo;
                }
                else if (Type == "left_video")
                {
                    ImageMedia = Settings.MS_DownloadBlackicon;
                    mediaFileName = AppResources.Label_DownloadVideo;
                }

              
                if (SQL_Commander.CheckMessage(Messageid) == "0")
                {
                    if (Type == "//right_audio" || Type == "//left_audio")
                    {
                        SQL_Commander.InsertMessage(new MessagesDB
                        {
                            message_id = Messageidx,
                            from_id = from_id,
                            to_id = to_id,
                            media = media,
                            mediafilename = mediaFileName,
                            deleted_one = deleted_one,
                            deleted_two = deleted_two,
                            text = TextMsg,
                            time = TimeMsg,
                            type = Type,
                            position = Position,
                            avatar = avatar,
                            ImageMedia = ImageMedia,
                            MediaName = mediaFileName,
                            DownloadFileUrl = DownloadFileUrl
                        });
                    }
                    else if (Type == "right_video" || Type == "left_video")
                    {
                        SQL_Commander.InsertMessage(new MessagesDB
                        {
                            message_id = Messageidx,
                            from_id = from_id,
                            to_id = to_id,
                            media = media,
                            mediafilename = mediaFileName,
                            deleted_one = deleted_one,
                            deleted_two = deleted_two,
                            text = TextMsg,
                            time = TimeMsg,
                            type = Type,
                            position = Position,
                            avatar = avatar,
                            ImageMedia = ImageMedia,
                            MediaName = AppResources.Label_DownloadVideo,
                            DownloadFileUrl = DownloadFileUrl
                        });
                    }
                    else if (Type == "right_contact" || Type == "left_contact")
                    {
                        TextMsg = TextMsg.Replace("{", "").Replace("}", "").Replace("Key:", "").Replace("Value:", "");
                        string[] Con = TextMsg.Split(',');

                        SQL_Commander.InsertMessage(new MessagesDB
                        {
                            message_id = Messageidx,
                            from_id = from_id,
                            to_id = to_id,
                            media = "UserContact.png",
                            mediafilename = "UserContact.png",
                            deleted_one = deleted_one,
                            deleted_two = deleted_two,
                            text = Con[0],
                            time = TimeMsg,
                            type = Type,
                            position = Position,
                            avatar = avatar,
                            ImageMedia = "UserContact.png",
                            MediaName = mediaFileName,
                            contactName = Con[0],
                            contactNumber = Con[1],
                            DownloadFileUrl = DownloadFileUrl
                        });
                    }
                    else if(Type == "left_sticker" || Type == "right_sticker")
                    {
                        SQL_Commander.InsertMessage(new MessagesDB
                        {
                            message_id = Messageidx,
                            from_id = from_id,
                            to_id = to_id,
                            media = media,
                            mediafilename = mediaFileName,
                            deleted_one = deleted_one,
                            deleted_two = deleted_two,
                            text = TextMsg,
                            time = TimeMsg,
                            type = Type,
                            position = Position,
                            avatar = avatar,
                            ImageMedia = ImageMedia,
                            MediaName = mediaFileName,
                            DownloadFileUrl = DownloadFileUrl
                        });
                    }
                    else
                    {
                        SQL_Commander.InsertMessage(new MessagesDB
                        {
                            message_id = Messageidx,
                            from_id = from_id,
                            to_id = to_id,
                            media = media,
                            mediafilename = mediaFileName,
                            deleted_one = deleted_one,
                            deleted_two = deleted_two,
                            text = TextMsg,
                            time = TimeMsg,
                            type = Type,
                            position = Position,
                            avatar = avatar,
                            ImageMedia = ImageMedia,
                            MediaName = mediaFileName,
                            DownloadFileUrl = DownloadFileUrl
                        });
                    }

                }
                #endregion
            }
            catch (Exception)
            {
                
            }
        }

        public static void MessageType_InsertFrom_DataBase(string recipientid)
        {
            #region Load from Database
            try
            {
                
                    var messages = SQL_Commander.GetMessageList(Settings.User_id, recipientid, "0");

                    if (messages.Count() > 0)
                    {
                        foreach (var Message in messages)
                        {
                            var isVisible = true;
                            if (Message.text == "")
                            {
                                isVisible = false;
                            }

                            var SS = Functions.Messages.Select(a => a.messageID == Message.message_id.ToString()).FirstOrDefault();
                            if (SS == false)
                            {
                                if (Message.position == "left")
                                {
                                    var ImageProfile =
                                        ImageSource.FromFile(
                                            DependencyService.Get<IPicture>()
                                                .GetPictureFromDisk(Message.avatar, recipientid));

                                    #region left_text

                                    if (Message.type == "left_text")
                                    {

                                        Functions.Messages.Add(new MessageViewModal()
                                        {
                                            Content = Message.text,
                                            Type = Message.type,
                                            Position = Message.position,
                                            CreatedAt = Message.time,
                                            messageID = Message.message_id.ToString(),
                                            UserImage = ImageProfile,
                                            CommingBackroundBoxColor = Settings.MS_CommingBackroundBox_Color
                                        });
                                    }

                                    #endregion

                                    #region Left Image

                                    if (Message.type == "left_image")
                                    {
                                        var ImageUrl = Message.media;

                                        var ImageMediaFile =
                                            ImageSource.FromFile(
                                                DependencyService.Get<IPicture>().GetPictureFromGalery(ImageUrl));

                                        if (DependencyService.Get<IPicture>().GetPictureFromGalery(ImageUrl) ==
                                            "File Dont Exists")
                                        {
                                            ImageMediaFile = new UriImageSource
                                            {
                                                Uri = new Uri(ImageUrl)
                                            };
                                        }

                                        Functions.Messages.Add(new MessageViewModal()
                                        {
                                            UserImage = ImageProfile,
                                            Visibilty = isVisible,
                                            Content = Message.text,
                                            Type = Message.type,
                                            Position = Message.position,
                                            CreatedAt = Message.time,
                                            ImageMedia = ImageMediaFile,
                                            messageID = Message.message_id.ToString(),
                                            ImageUrl = ImageUrl,
                                            DownloadFileUrl = Message.DownloadFileUrl,
                                            CommingBackroundBoxColor = Settings.MS_CommingBackroundBox_Color
                                        });

                                    }

                                    #endregion

                                    #region Left Sound

                                    if (Message.type == "//left_audio")
                                    {

                                        Functions.Messages.Add(new MessageViewModal()
                                        {
                                            UserImage = ImageProfile,
                                            Type = Message.type,
                                            Position = Message.position,
                                            CreatedAt = Message.time,
                                            messageID = Message.message_id.ToString(),
                                            Media = Message.media,
                                            DownloadFileUrl = Message.DownloadFileUrl,
                                            ImageMedia = Message.ImageMedia,
                                            MediaName = Message.MediaName,
                                            SliderSoundValue = "0",
                                            SliderMaxDuration = "100",
                                            CommingBackroundBoxColor = Settings.MS_CommingBackroundBox_Color

                                        });

                                    }

                                    #endregion

                                    #region Left Video

                                    if (Message.type == "left_video")
                                    {
                                        Functions.Messages.Add(new MessageViewModal()
                                        {
                                            UserImage = ImageProfile,
                                            Type = Message.type,
                                            Position = Message.position,
                                            CreatedAt = Message.time,
                                            messageID = Message.message_id.ToString(),
                                            Media = Message.media,
                                            DownloadFileUrl = Message.DownloadFileUrl,
                                            ImageMedia = Message.ImageMedia,
                                            MediaName = Message.MediaName,
                                            SliderSoundValue = "0",
                                            SliderMaxDuration = "100",
                                            CommingBackroundBoxColor = Settings.MS_CommingBackroundBox_Color
                                        });

                                    }

                                    #endregion

                                    #region left_contact
                                    if (Message.type == "left_contact")
                                    {
                                        Functions.Messages.Add(new MessageViewModal()
                                        {
                                            Content = Message.text,
                                            Type = Message.type,
                                            UserImage = Message.avatar,
                                            Position = Message.position,
                                            CreatedAt = Message.time,
                                            messageID = Message.message_id.ToString(),
                                            
                                            ImageMedia = "UserContact.png",
                                            ContactNumber = Message.contactNumber,
                                            ContactName = Message.contactName,
                                            CommingBackroundBoxColor = Settings.MS_CommingBackroundBox_Color
                                        });
                                    }
                                    #endregion

                                    if (Message.type == "left_sticker")
                                    {
                                        Functions.Messages.Add(new MessageViewModal()
                                        {
                                            Content = Message.text,
                                            Type = Message.type,
                                            Position = Message.position,
                                            CreatedAt = Message.time,
                                            messageID = Message.message_id.ToString(),
                                            ImageUrl = Message.media,
                                            ImageMedia = Message.media,
                                            UserImage = ImageProfile,
                                        
                                        });
                                    }

                                }
                                else
                                {
                                    var ImageProfile = ImageSource.FromFile(DependencyService.Get<IPicture>()
                                        .GetPictureFromDisk(Message.avatar, Settings.User_id));

                                    #region right_text

                                    if (Message.type == "right_text")
                                    {

                                        Functions.Messages.Add(new MessageViewModal()
                                        {
                                            Content = Message.text,
                                            Type = Message.type,
                                            Position = Message.position,
                                            CreatedAt = Message.time,
                                            messageID = Message.message_id.ToString(),
                                            UserImage = ImageProfile,
                                            GoingBackroundBoxColor = Settings.MS_GoingBackroundBox_Color
                                        });
                                    }

                                    #endregion

                                    #region Left Image

                                    if (Message.type == "right_image")
                                    {
                                        var ImageUrl = Message.media;
                                        var ImageMediaFile =
                                            ImageSource.FromFile(
                                                DependencyService.Get<IPicture>()
                                                    .GetPictureFromDisk(ImageUrl, Settings.User_id));
                                        if (
                                            DependencyService.Get<IPicture>()
                                                .GetPictureFromDisk(ImageUrl, Settings.User_id) == "File Dont Exists")
                                        {
                                            DependencyService.Get<IPicture>()
                                                .SavePictureToDisk(ImageUrl, Settings.User_id);
                                            ImageMediaFile = new UriImageSource { Uri = new Uri(ImageUrl) };
                                        }

                                        Functions.Messages.Add(new MessageViewModal()
                                        {
                                            UserImage = ImageProfile,
                                            Visibilty = isVisible,
                                            Content = Message.text,
                                            Type = Message.type,
                                            Position = Message.position,
                                            CreatedAt = Message.time,
                                            ImageMedia = ImageMediaFile,
                                            messageID = Message.message_id.ToString(),
                                            ImageUrl = ImageUrl,
                                            DownloadFileUrl = Message.DownloadFileUrl,
                                            GoingBackroundBoxColor = Settings.MS_GoingBackroundBox_Color
                                        });

                                    }

                                    #endregion

                                    #region right Sound

                                    if (Message.type == "//right_audio")
                                    {

                                        Functions.Messages.Add(new MessageViewModal()
                                        {
                                            UserImage = ImageProfile,
                                            Type = Message.type,
                                            Position = Message.position,
                                            CreatedAt = Message.time,
                                            messageID = Message.message_id.ToString(),
                                            Media = Message.MediaName,
                                            ImageMedia = Message.ImageMedia,
                                            DownloadFileUrl = Message.DownloadFileUrl,
                                            MediaName = Message.MediaName,
                                            SliderSoundValue = "0",
                                            SliderMaxDuration = "100",
                                            GoingBackroundBoxColor = Settings.MS_GoingBackroundBox_Color
                                        });

                                    }

                                    #endregion

                                    #region right Video

                                    if (Message.type == "right_video")
                                    {

                                        Functions.Messages.Add(new MessageViewModal()
                                        {
                                            UserImage = ImageProfile,
                                            Type = Message.type,
                                            Position = Message.position,
                                            CreatedAt = Message.time,
                                            messageID = Message.message_id.ToString(),
                                            Media = Message.media,
                                            ImageMedia = Message.ImageMedia,
                                            DownloadFileUrl = Message.DownloadFileUrl,
                                            MediaName = Message.MediaName,
                                            SliderSoundValue = "0",
                                            SliderMaxDuration = "100",
                                            GoingBackroundBoxColor = Settings.MS_GoingBackroundBox_Color
                                        });

                                    }

                                    #endregion

                                    #region right contact

                                    if (Message.type == "right_contact")
                                    {

                                        Functions.Messages.Add(new MessageViewModal()
                                        {
                                            UserImage = ImageProfile,
                                            Type = Message.type,
                                            Position = Message.position,
                                            CreatedAt = Message.time,
                                            messageID = Message.message_id.ToString(),
                                            Media = Message.media,
                                            DownloadFileUrl = Message.DownloadFileUrl,
                                            MediaName = Message.MediaName,
                                            ImageMedia = "UserContact.png",
                                            GoingBackroundBoxColor = Settings.MS_GoingBackroundBox_Color,
                                            ContactNumber = Message.contactNumber,
                                            Content = Message.contactName,
                                            ContactName = Message.contactName,
                                           
                                        });

                                    }
                                    #endregion

                                    if (Message.type == "right_sticker")
                                    {
                                        Functions.Messages.Add(new MessageViewModal()
                                        {
                                            Content = Message.text,
                                            Type = Message.type,
                                            Position = Message.position,
                                            CreatedAt = Message.time,
                                            messageID = Message.message_id.ToString(),
                                            ImageUrl = Message.media,
                                            ImageMedia = Message.media,
                                            UserImage = ImageProfile,
                                        });
                                    }

                                }
                            }
                           

                        }
                    }
                
               
            }
            catch (Exception)
            {

            }
            #endregion
        }
    }
}
