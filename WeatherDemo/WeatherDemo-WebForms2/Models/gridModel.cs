using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeatherDemo_WebForms2.Models
{
    public class gridModel
    {
        public Double temperature;
        public string weather;

        public gridModel(Double _temp, string _description)
        {
            this.temperature = _temp;
            this.weather = _description;
        }
    }
}