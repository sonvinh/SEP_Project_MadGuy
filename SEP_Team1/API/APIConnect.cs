using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using SEP_Team1.Models;

namespace SEP_Team1.API
{
    public class APIConnect
    {
        sep21t21Entities db = new sep21t21Entities();
        private string urlAddress = "https://entool.azurewebsites.net/SEP21";
        private string urlConnect;
        private string data;
        private string Url(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (response.CharacterSet == null)
                {
                    readStream = new StreamReader(receiveStream);
                }
                else
                {
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                }
                string data = readStream.ReadToEnd();
                response.Close();
                readStream.Close();
                return data;
            }
            return "";
        }
        public List<Info> GetStudent(string id)
        {
            urlConnect = urlAddress + "/GetMembers?courseID={0}";
            urlConnect = string.Format(urlConnect, id);
            data = Url(urlConnect);
            if (data != "")
            {
                //tao noi luu tru vao model
                var Student = new List<Info>();
                //parse data json
                //get data json type array
                StudentAPI items = JsonConvert.DeserializeObject<StudentAPI>(data);
                foreach (var item in items.data)
                {
                    Student.Add(item);
                }
                return Student;
            }
            return null;
        }
        public string Login (string username, string password)
        {
            urlConnect = urlAddress + "/Login?Username={0}&Password={1}";
            urlConnect = string.Format(urlConnect, username, password);
            data = Url(urlConnect);

            if (data != "")
            {
                dynamic log = JsonConvert.DeserializeObject(data);
                string code = log.code;

                if (int.Parse(code) == 0)
                {
                    string id = log.data.id;
                    urlConnect = "";
                    return id;
                }
            }
            urlConnect = "";
            return "";
        }
    }
}