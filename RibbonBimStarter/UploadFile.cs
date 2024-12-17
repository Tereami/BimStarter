using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RibbonBimStarter
{
    public class UploadFile
    {
        public string Postname { get; set; }
        public string Filename { get; set; }
        public string ContentType { get; set; }
        public Stream Stream { get; set; }

        public UploadFile(string postname, string filepath, Stream stream, string contentType = "application/octet-stream")
        {
            Postname = postname;
            Filename = filepath;
            Stream = stream;
            ContentType = contentType;
        }
    }
}
