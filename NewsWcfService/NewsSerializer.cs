using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using System.IO;
using System.Net;

namespace NewsWcfService
{
    public static class NewsSerializer
    {
        public static NewsCollection Deserialize()
        {
            NewsCollection news = null;
            string url = "http://lenta.ru/rss";
            WebRequest request=WebRequest.Create(url);
            request.Timeout=30*60*1000;
            request.UseDefaultCredentials=true;
            request.Proxy.Credentials=request.Credentials;
            WebResponse response=(WebResponse)request.GetResponse();
            using (Stream s = response.GetResponseStream())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(NewsCollection));
                StreamReader reader = new StreamReader(s);
                news = (NewsCollection)serializer.Deserialize(reader);
                reader.Close();
                return news;
            }
        }
    }

    [Serializable()]
    [System.Xml.Serialization.XmlRoot("rss")]
    public class NewsCollection
    {
        [XmlArray("channel")]
        [XmlArrayItem("item", typeof(New))]
        public New[] New { get; set; }
    }
}