using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeatherDemo_WebForms2.General
{
    public static class GeneralFunctions
    {
        /// <summary>
        /// Converts Epoch time to UTC
        /// </summary>
        /// <param name="unixTime"></param>
        /// <returns></returns>
        public static DateTime FromUnixTime(long unixTime)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }

        public static Double convertTemperature(Double temperature, string unit)
        {
            Double newTemperature = 0;
            if(unit == "C")
            {
                newTemperature = convertKelvinToCelsius(temperature);
            }
            else if (unit == "F")
            {
                newTemperature = convertKelvinToCelsius(temperature);
                newTemperature = convertCelsiusToFarenheit(newTemperature);
            }

            return Math.Round(newTemperature, 2);
        }
        public static string convertTemperatureAndFormat(Double temperature, string unit)
        {
            Double newTemperature = 0;
            if (unit == "C")
            {
                newTemperature = convertKelvinToCelsius(temperature);
            }
            else if (unit == "F")
            {
                newTemperature = convertKelvinToCelsius(temperature);
                newTemperature = convertCelsiusToFarenheit(newTemperature);
            }

            return Math.Round(newTemperature, 2).ToString() + " °" + unit;
        }

        public static Double convertKelvinToCelsius(Double kelvinTemperature)
        {
            return kelvinTemperature - 273.15;
        }

        public static Double convertCelsiusToFarenheit(Double celsiusTemperature)
        {
            //(0°C × 9/5) + 32 
            return (celsiusTemperature * 9/5) + 32;
        }
        public static Double convertKelvinToFarenheit(Double temp)
        {
            return convertCelsiusToFarenheit(convertKelvinToCelsius(temp));
        }
    }
}