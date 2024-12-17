#region License
/*Данный код опубликован под лицензией Creative Commons Attribution-NonСommercial-ShareAlike.
Разрешено использовать, распространять, изменять и брать данный код за основу для производных 
в некоммерческих целях, при условии указания авторства и если производные лицензируются на тех же условиях.
Код поставляется "как есть". Автор не несет ответственности за возможные последствия использования.
Зуев Александр, 2021, все права защищены.
This code is listed under the Creative Commons Attribution-NonСommercial-ShareAlike license.
You may use, redistribute, remix, tweak, and build upon this work non-commercially,
as long as you credit the author by linking back and license your new creations under the same terms.
This code is provided 'as is'. Author disclaims any implied warranty.
Zuev Aleksandr, 2021, all rigths reserved.*/
#endregion
#region usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net;
using System.IO;
using Newtonsoft.Json;
#endregion

namespace RibbonBimStarter
{
    public class WebConnection
    {
        //public int status;
        //public string error;
        //public string lastResponseString;

        //private byte[] credentialsByteArray;
        private string _website;
        private string _email;
        private string _password;


        public WebConnection(string email, string password, string website = "https://bim-starter.com/")
        {
            _website = website;
            _email = email;
            _password = password;
            string credentialsString = "email=" + email + "&password=" + password;
        }

        public ServerResponse Request(string url, Dictionary<string, string> postData = null)
        {
            ServerResponse sr = new ServerResponse(520, string.Empty);

            if (postData == null)
            {
                postData = new Dictionary<string, string>();
            }
            postData.Add("email", _email);
            postData.Add("password", _password);

            HttpWebRequest request = CreateRequest(url, postData);
            if (request == null)
            {
                sr.Statuscode = 400;
                sr.Message = "Unable to create request";
                return sr;
            }

            Debug.WriteLine("Go to Webresponse");

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    sr.Statuscode = (int)response.StatusCode;
                    if (sr.Statuscode >= 400)
                    {
                        return sr;
                    }
                    string responseString = string.Empty;
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string responsestr = reader.ReadToEnd();
                            sr = ServerResponse.CreateByJson(responsestr);
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                sr.Message += ex.Message;
                HttpWebResponse exResponse = ex.Response as HttpWebResponse;
                sr.Statuscode = exResponse == null ? 520 : (int)exResponse.StatusCode;
                Debug.WriteLine("Webresponse error " + url + sr.ToString());
                return sr;
            }

            Debug.WriteLine("Webresponse success " + url + " status " + sr.Statuscode);
            return sr;
        }

        public ServerResponse DownloadFamily(string guid, string familyname)
        {
            ServerResponse sr = new ServerResponse(520, string.Empty);
            Dictionary<string, string> postData = new Dictionary<string, string>();
            postData.Add("email", _email);
            postData.Add("password", _password);
            HttpWebRequest request = CreateRequest("familydownloadapp/?guid=" + guid, postData);
            if (request == null)
            {
                return new ServerResponse(520, "Request is null");
            }
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Debug.WriteLine("HttpWebResponse success");
                    WebHeaderCollection headers = response.Headers;
                    sr.Statuscode = (int)response.StatusCode;
                    if (sr.Statuscode >= 400)
                    {
                        sr.Message += headers["Message"];
                        Debug.WriteLine("Headers: " + headers.ToString());
                        Debug.WriteLine(sr.ToString());
                        return sr;
                    }

                    if (headers.AllKeys.Contains("RbsStatus"))
                    {
                        int statusByHeader = int.Parse(headers["RbsStatus"]);
                        if (statusByHeader >= 400)
                        {
                            return new ServerResponse(statusByHeader, headers["Message"]);
                        }
                    }

                    if (headers.AllKeys.Contains("Message"))
                    {
                        System.Windows.Forms.MessageBox.Show(headers["Message"]);
                    }

                    //string checkFilename0 = headers["Content-Disposition"].Split('=').Last();

                    string headerContent = headers["Content-Disposition"];
                    string checkFilename0 = Encoding.UTF8.GetString(Encoding.GetEncoding("iso-8859-1").GetBytes(headerContent));

                    string filename0 = checkFilename0.Split('=').Last();
                    filename0 = filename0.Trim('"');

                    string filename = familyname + ".rfa";

                    string pathToSave = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "bim-starter", filename);
                    Debug.WriteLine("File: " + pathToSave);
                    if (File.Exists(pathToSave))
                    {
                        Debug.WriteLine("File exists, try to delete");
                        System.IO.File.Delete(pathToSave);
                    }

                    Debug.WriteLine("Go to save file by FileStream");
                    byte[] buffer = new byte[1024];
                    int read;
                    using (FileStream download = new FileStream(pathToSave, FileMode.Create))
                    {
                        using (Stream stream = response.GetResponseStream())
                        {
                            while ((read = stream.Read(buffer, 0, buffer.Length)) != 0)
                            {
                                download.Write(buffer, 0, read);
                            }
                        }
                    }
                    long filesizeFromHeader = long.Parse(headers["Content-Length"]);
                    long downloadedSize = new FileInfo(pathToSave).Length;
                    if (filesizeFromHeader != downloadedSize)
                    {
                        Debug.WriteLine("Error download! " + filesizeFromHeader + " but " + downloadedSize);
                        sr.Statuscode = 416;
                        sr.Message = "Incorrect file size!";
                        return sr;
                    }
                    else
                    {
                        sr.Statuscode = 200;
                        sr.Message = pathToSave;
                        Debug.WriteLine("File download successfully");
                        return sr;
                    }
                }
            }
            catch (Exception ex)
            {
                sr.Statuscode = 400;
                sr.Message = "Error: " + ex.Message;
                Debug.WriteLine("Error http response! " + ex.Message);
                return sr;
            }
        }

        private string GetUrl(string website, string url)
        {
            website = website.Trim('/').Trim('\\');
            url = url.Trim('/').Trim('\\');
            string result = website + "/" + url;
            return result;
        }

        private HttpWebRequest CreateRequest(string url, Dictionary<string, string> postData)
        {
            Debug.WriteLine("webrequest start, url " + url);
            string fullurl = GetUrl(_website, url);
            HttpWebRequest request = WebRequest.Create(fullurl) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            string postDataString = String.Join("&", postData.ToList().Select(kvp => kvp.Key + "=" + kvp.Value).ToList());

            byte[] postDataByteArray = System.Text.Encoding.UTF8.GetBytes(postDataString);
            request.ContentLength = postDataByteArray.Length;
            request.Timeout = 60000;

            //if (s.Proxy != ProxyType.No)
            //{
            //    request.Proxy = GetProxy(s);
            //}
            try
            {
                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(postDataByteArray, 0, postDataByteArray.Length);
                    Debug.WriteLine("DataStream write successfully");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error dataStream write" + ex.Message);
                System.Windows.Forms.MessageBox.Show(ex.Message);
                return null;
            }
            return request;
        }


        public ServerResponse UploadFamily(string name, string guid, string group, string categoryid, string hostid, string description, string munufacturer, string document,
            string rfapath, string jpgpath300, string jpgpath140, List<string> nestedguidswithversion)
        {
            using (FileStream rfastream = File.Open(rfapath, FileMode.Open))
            using (FileStream jpgstream300 = File.Open(jpgpath300, FileMode.Open))
            using (FileStream jpgstream140 = File.Open(jpgpath140, FileMode.Open))
            {
                List<UploadFile> files = new List<UploadFile>
                {
                    new UploadFile("file_rfa", Path.GetFileName(rfapath), rfastream),
                    new UploadFile("file_jpg300", Path.GetFileName(jpgpath300), jpgstream300),
                    new UploadFile("file_jpg140", Path.GetFileName(jpgpath140), jpgstream140)
                };

                Dictionary<string, string> values = new Dictionary<string, string>();
                values.Add("email", _email);
                values.Add("password", _password);
                values.Add("group", group);
                values.Add("guid", guid);
                values.Add("name", name);
                values.Add("category", categoryid);
                values.Add("host", hostid);
                values.Add("revitversion", App.revitVersion);
                values.Add("description", description);

                if (!string.IsNullOrEmpty(munufacturer))
                    values.Add("munufacturer", munufacturer);
                if (!string.IsNullOrEmpty(document))
                    values.Add("document", document);

                if (nestedguidswithversion != null && nestedguidswithversion.Count > 0)
                {
                    values.Add("nested", String.Join(",", nestedguidswithversion));
                }

                byte[] response = UploadFiles(GetUrl(_website, "familycreateapp"), files, values);
                string responseString = Encoding.UTF8.GetString(response);
                return ServerResponse.CreateByJson(responseString);
            }
        }

        public ServerResponse UploadFamilyVersion(
            string guid, int newVersionNumber, string versionDescription, string rfapath, List<string> nestedguidswithversion)
        {
            ServerResponse sr = new ServerResponse(400, string.Empty);

            using (FileStream rfastream = File.Open(rfapath, FileMode.Open))
            {
                List<UploadFile> files = new List<UploadFile>
                {
                    new UploadFile("file_rfa", Path.GetFileName(rfapath), rfastream),
                };
                Dictionary<string, string> values = new Dictionary<string, string>();
                values.Add("email", _email);
                values.Add("password", _password);
                values.Add("guid", guid);
                values.Add("newversionnumber", newVersionNumber.ToString());
                values.Add("changes", versionDescription);

                if (nestedguidswithversion != null && nestedguidswithversion.Count > 0)
                {
                    values.Add("nested", String.Join(",", nestedguidswithversion));
                }

                byte[] response = UploadFiles(GetUrl(_website, "familycreateversionapp"), files, values);
                string responseString = Encoding.UTF8.GetString(response);
                return ServerResponse.CreateByJson(responseString);
            }
        }

        public static byte[] UploadFiles(string url, IEnumerable<UploadFile> files, Dictionary<string, string> keyvalue)
        {
            WebRequest request = WebRequest.Create(url);
            request.Method = "POST";
            var boundary = "---------------------------"
                + DateTime.Now.Ticks.ToString("x", System.Globalization.NumberFormatInfo.InvariantInfo);

            request.ContentType = "multipart/form-data; boundary=" + boundary;
            boundary = "--" + boundary;

            using (var requestStream = request.GetRequestStream())
            {
                foreach (var kvp in keyvalue)
                {
                    byte[] buffer = Encoding.ASCII.GetBytes(boundary + Environment.NewLine);
                    requestStream.Write(buffer, 0, buffer.Length);
                    buffer = Encoding.ASCII.GetBytes(
                        string.Format("Content-Disposition: form-data; name=\"{0}\"{1}{1}",
                        kvp.Key, Environment.NewLine));
                    requestStream.Write(buffer, 0, buffer.Length);
                    buffer = Encoding.UTF8.GetBytes(kvp.Value + Environment.NewLine);
                    requestStream.Write(buffer, 0, buffer.Length);
                }
                foreach (UploadFile file in files)
                {
                    byte[] buffer = Encoding.ASCII.GetBytes(boundary + Environment.NewLine);
                    requestStream.Write(buffer, 0, buffer.Length);
                    buffer = Encoding.UTF8.GetBytes(
                        string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"{2}",
                        file.Postname, file.Filename, Environment.NewLine));
                    requestStream.Write(buffer, 0, buffer.Length);
                    buffer = Encoding.ASCII.GetBytes(
                        string.Format("Content-Type: {0}{1}{1}", file.ContentType, Environment.NewLine));
                    requestStream.Write(buffer, 0, buffer.Length);
                    file.Stream.CopyTo(requestStream);
                    buffer = Encoding.ASCII.GetBytes(Environment.NewLine);
                    requestStream.Write(buffer, 0, buffer.Length);
                }
                byte[] boundaryBuffer = Encoding.ASCII.GetBytes(boundary + "--");
                requestStream.Write(boundaryBuffer, 0, boundaryBuffer.Length);

            }
            using (var response = request.GetResponse())
            using (var responseStream = response.GetResponseStream())
            using (var stream = new System.IO.MemoryStream())
            {
                responseStream.CopyTo(stream);
                return stream.ToArray();
            }
        }
    }
}
