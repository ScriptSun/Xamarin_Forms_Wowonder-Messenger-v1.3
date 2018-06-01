using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WowonderPhone.Classes;
using WowonderPhone.Pages.CustomCells;
using Xamarin.Forms;



namespace WowonderPhone
{
     class MessageDataTemplate : Xamarin.Forms.DataTemplateSelector
    {
        private readonly DataTemplate comingDataTemplate;
        private readonly DataTemplate goingDataTemplate;
        private readonly DataTemplate goingImageDataTemplate;
        private readonly DataTemplate commingImageDataTemplate;
        private readonly DataTemplate commingSoundDataTemplate;
        private readonly DataTemplate goingSoundDataTemplate;
        private readonly DataTemplate commingvideoDataTemplate;
        private readonly DataTemplate goingvideoDataTemplate;
        private readonly DataTemplate goingcontactDataTemplate;
        private readonly DataTemplate commingcontactDataTemplate;
        private readonly DataTemplate goingstickerDataTemplate;
        private readonly DataTemplate commingstickerDataTemplate;
       


        public MessageDataTemplate()
        {
            // Retain instances!
            this.goingDataTemplate = new DataTemplate(typeof(GoingMessage));
            this.comingDataTemplate = new DataTemplate(typeof(CommingMessage));
            this.goingImageDataTemplate = new DataTemplate(typeof(GoingImage));
            this.commingImageDataTemplate = new DataTemplate(typeof(CommingImage));
            this.commingSoundDataTemplate = new DataTemplate(typeof(CommingSound));
            this.goingSoundDataTemplate = new DataTemplate(typeof(GoingSound));
            this.commingvideoDataTemplate = new DataTemplate(typeof(CommingVideo));
            this.goingvideoDataTemplate = new DataTemplate(typeof(GoingVideo));
            this.goingcontactDataTemplate = new DataTemplate(typeof(GoingContact));
            this.commingcontactDataTemplate = new DataTemplate(typeof(CommingContact));
            this.goingstickerDataTemplate = new DataTemplate(typeof(GoingStickers));
            this.commingstickerDataTemplate = new DataTemplate(typeof(CommingStickers));


        }

      
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var messageVm = item as MessageViewModal;
            if (messageVm == null)
            {
                return null;
            }
                

            if (messageVm.Type == "right_text")
            {
                return goingDataTemplate;
            }
             else if (messageVm.Type == "left_text")
            {
                return comingDataTemplate;
            }
            else if (messageVm.Type == "right_image")
            {
                return goingImageDataTemplate;
            }
            else if (messageVm.Type == "left_image")
            {
                return commingImageDataTemplate;
            }
            else if (messageVm.Type == "left_audio")
            {
                return commingSoundDataTemplate;
            }
            else if (messageVm.Type == "right_audio")
            {
                
                return goingSoundDataTemplate;
            }
             else if (messageVm.Type == "left_video")
            {
                return commingvideoDataTemplate;
            }
            else if (messageVm.Type == "right_video")
            {
                return goingvideoDataTemplate;
            }
            else if (messageVm.Type == "right_contact")
            {
                return goingcontactDataTemplate;
            }
            else if (messageVm.Type == "left_contact")
            {
                return commingcontactDataTemplate;
            }
            else if (messageVm.Type == "right_sticker")
            {
                return goingstickerDataTemplate;
            }
            else
            {
                return commingstickerDataTemplate;
            }

        }
        


    }
}
