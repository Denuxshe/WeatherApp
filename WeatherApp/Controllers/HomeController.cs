using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;
using WeatherApp.Models;

namespace WeatherApp.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult showWeather()
        {
            return View();
        }
        public ActionResult Weather()
        {

            string IP = "";
            string city = "";

            string strHostName = "";
            strHostName = System.Net.Dns.GetHostName();

            IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);

            IPAddress[] addr = ipEntry.AddressList;

            IP = addr[2].ToString();



            String URLString = "http://api.ipstack.com/" + IP + "?access_key=" + "5998453809bee05aec1562f4d8edaf27";
            XmlTextReader reader = new XmlTextReader(URLString);


            var json = new WebClient().DownloadString("http://api.ipstack.com/" + IP + "?access_key=" + "22715e4603057889f8605e933d7652b7");

            dynamic json1 = JObject.Parse(json);
            string cityname = json1.city;


            ViewBag.cityname = cityname;
            ViewBag.IP = IP;
            ViewBag.contry = json1.country_name;






            return View();
        }

        [HttpPost]
        public String WeatherDetail(string City)
        {

            //Assign API KEY which received from OPENWEATHERMAP.ORG  
            string appId = "b81cd3e18bd5f71679161fd60d7ec325";

            //API path with CITY parameter and other parameters.  
            string url = string.Format("http://api.openweathermap.org/data/2.5/weather?q={0}&units=metric&cnt=1&APPID={1}", City, appId);

            using (WebClient client = new WebClient())
            {
                string json = client.DownloadString(url);
                RootObject weatherInfo = (new JavaScriptSerializer()).Deserialize<RootObject>(json);

                WeatherModelView rslt = new WeatherModelView();

                rslt.Country = weatherInfo.sys.country;
                rslt.City = weatherInfo.name;
                rslt.Lat = Convert.ToString(weatherInfo.coord.lat);
                rslt.Lon = Convert.ToString(weatherInfo.coord.lon);
                rslt.Description = weatherInfo.weather[0].description;
                rslt.Humidity = Convert.ToString(weatherInfo.main.humidity);
                rslt.Temp = Convert.ToString(weatherInfo.main.temp);
                rslt.TempFeelsLike = Convert.ToString(weatherInfo.main.feels_like);
                rslt.TempMax = Convert.ToString(weatherInfo.main.temp_max);
                rslt.TempMin = Convert.ToString(weatherInfo.main.temp_min);
                rslt.WeatherIcon = weatherInfo.weather[0].icon;

                //Converting OBJECT to JSON String   
                var jsonstring = new JavaScriptSerializer().Serialize(rslt);

                //Return JSON string.  
                return jsonstring;
            }
        }
    }
}