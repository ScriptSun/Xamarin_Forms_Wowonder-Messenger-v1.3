using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WowonderPhone.Languish;
using WowonderPhone.Pages;

//WOWONDER ANDROID MESSENGER V1.3
//==============================
//This is A sample of xamarin forms chat application 
//This solution is not allowed to be shared on Google play 
//You can get the styles and Xaml codes and use it as you like 
//Devloped by Elin Doughouz
//Follow me on linkded >> https://www.linkedin.com/in/elin-doughouz-1b7458131
//Follow me on Facebook >> https://www.facebook.com/Elindoughous

namespace WowonderPhone.Controls
{
    
    public class Wo_API
    {
        public static string recipient_id = "";

        public static async Task<JArray> Get_User_Messages(string lastid)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var formContent = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("user_id", Settings.User_id),
                        new KeyValuePair<string, string>("recipient_id", recipient_id),
                        new KeyValuePair<string, string>("before_message_id", "0"),
                        new KeyValuePair<string, string>("after_message_id", lastid),
                        new KeyValuePair<string, string>("s", Settings.Session)
                    });

                    var response = await client.PostAsync(
                        Settings.Website + "/app_api.php?application=phone&type=get_user_messages", formContent);
                    response.EnsureSuccessStatusCode();
                    string json = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                    string apiStatus = data["api_status"].ToString();
                    if (apiStatus == "200")
                    {
                        var messages = JObject.Parse(json).SelectToken("messages").ToString();
                        JArray chatMessages = JArray.Parse(messages);
                        return chatMessages;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception EX)
            {
                EX.ToString();
                return null;
            }  
        }

        public static async Task<JArray> Send_User_Message(string text, string contact, string stickerurl,string stickerid)
        {
            try
            {
                string Time = ChatWindow.unixTimestamp.ToString();
                var client = new HttpClient();
                var multi = new MultipartFormDataContent();
                var values = new[]
                {
                    new KeyValuePair<string, string>("user_id", Settings.User_id),
                    new KeyValuePair<string, string>("recipient_id", recipient_id),
                    new KeyValuePair<string, string>("s", Settings.Session),
                    new KeyValuePair<string, string>("text", text),
                    new KeyValuePair<string, string>("send_time", Time),
                    new KeyValuePair<string, string>("contact", contact),
                    new KeyValuePair<string, string>("image_url",stickerurl),
                    new KeyValuePair<string, string>("sticker_id",stickerid),
                };

                foreach (var keyValuePair in values)
                {
                    multi.Add(new StringContent(keyValuePair.Value), keyValuePair.Key);
                }

                client.BaseAddress = new Uri(Settings.Website);
                var result = client.PostAsync("/app_api.php?application=phone&type=new_message", multi).Result;
                string json = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
                var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                string apiStatus = data["api_status"].ToString();

                if (apiStatus == "200")
                {
                    var messages = JObject.Parse(json).SelectToken("messages").ToString();
                    JArray chatMessage = JArray.Parse(messages);
                    return chatMessage;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e); 
                return null;
            }
            
        }
    }
}
