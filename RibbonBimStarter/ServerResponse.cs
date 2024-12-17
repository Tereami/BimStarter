using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RibbonBimStarter
{
    [Serializable]
    public class ServerResponse
    {
        public int Statuscode;
        public string Message;

        public ServerResponse()
        {

        }

        public ServerResponse(int statuscode, string message)
        {
            Statuscode = statuscode;
            Message = message;
        }

        public override string ToString()
        {
            return "status " + Statuscode.ToString() + ", message " + Message;
        }

        public static ServerResponse CreateByJson(string json)
        {
            try
            {
                ServerResponse sr = Newtonsoft.Json.JsonConvert.DeserializeObject<ServerResponse>(json);
                if(sr != null)
                {
                    return sr;
                }
            }
            catch (Exception ex)
            {
                return new ServerResponse() { Statuscode = 520, Message = ex.Message + Environment.NewLine + json };
            }
            return new ServerResponse() { Statuscode = 520, Message = "Bad response" + Environment.NewLine + json };
        }
    }
}
