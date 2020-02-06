using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using WeatherDemo_WebForms2.Models;

namespace WeatherDemo_WebForms2.Interfaces
{
    public static class OpenWeather
    {
        public static String GetOpenWeatherAsync(string latitude, string longitude)
        {
            string Preuri = ConfigurationManager.AppSettings["OpenWeatherURI"];
            string appID = ConfigurationManager.AppSettings["OpenWeatherAppID"];
            //string latitude = this.latitude.Text;
            //string longitude = this.longitude.Text;

            string uri = Preuri + "?lat=" + latitude + "&lon=" + longitude + "&appid=" + appID;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            //request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        public static OpenWeatherReponse GetOpenWeatherObjectAsync(string latitude, string longitude)
        {
            string stringObject = GetOpenWeatherAsync(latitude, longitude);
            return new JavaScriptSerializer().Deserialize<OpenWeatherReponse>(stringObject);
        }

        public static String GetOpenWeatherHourlyAsync(string latitude, string longitude)
        {
            string Preuri = ConfigurationManager.AppSettings["OpenWeatherHourlyURI"];
            string appID = ConfigurationManager.AppSettings["OpenWeatherAppID"];
            //string latitude = this.latitude.Text;
            //string longitude = this.longitude.Text;

            string uri = Preuri + "?lat=" + latitude + "&lon=" + longitude + "&appid=" + appID;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            //request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        public static OpenWeatherHourlyResponse GetOpenWeatherHourlyObjectAsync(string latitude, string longitude)
        {
            string stringObject = GetOpenWeatherHourlyAsync(latitude, longitude);
            return new JavaScriptSerializer().Deserialize<OpenWeatherHourlyResponse>(stringObject);
        }
    }
}