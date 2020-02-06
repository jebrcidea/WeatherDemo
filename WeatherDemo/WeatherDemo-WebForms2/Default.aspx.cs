using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeatherDemo_WebForms2.Models;
using WeatherDemo_WebForms2.Interfaces;
using System.Data;
using WeatherDemo_WebForms2.General;

namespace WeatherDemo_WebForms2
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        /// <summary>
        /// Updates all weather data when user clicks button
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">arguments</param>
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                //Updates current weather
                updateWeather();
                //Updates forecast
                updateForecastHourly();

                //shows sucessful message
                this.spanMessage.Attributes.Add("class", "alert alert-success");
                spanMessage.InnerText = "Weather updated successfully";
            }
            catch
            {
                //if something went wrong, shows an error message
                this.spanMessage.Attributes.Add("class", "alert alert-danger");
                spanMessage.InnerText = "An error ocurred trying to update the weather. Please try again.";

                Page.ClientScript.RegisterStartupScript(this.GetType(), "hideOverlay", "hideOverlay();", true);
            }
        }
        /// <summary>
        /// Function to get current weather and update the elements. Comments work in case 
        /// you want to keep that info, but at least in México is mostly empty
        /// </summary>
        protected void updateWeather()
        {
            try
            {
                //gets clients georeference
                string latitude = this.latitude.Text;
                string longitude = this.longitude.Text;
                string unit = this.dwlMeasureUnit.SelectedValue;

                //gets current weather via OpenWeather API
                OpenWeatherReponse weatherResponse = OpenWeather.GetOpenWeatherObjectAsync(latitude, longitude);

                //if (weatherResponse.Main.FeelsLike != 0)
                //{
                //    feels_like.InnerText = (weatherResponse.Main.FeelsLike - 273.15).ToString() + " °C";
                //}
                //else
                //{
                //    feels_like.InnerText = "No Data";
                //}
                //humidity.InnerText = (weatherResponse.Main.Humidity).ToString();
                //pressure.InnerText = (weatherResponse.Main.Pressure).ToString();
                
                //Updates the aspx elements
                if (weatherResponse.Main.Temp != 0)
                {
                    temp.InnerText = GeneralFunctions.convertTemperatureAndFormat(weatherResponse.Main.Temp, unit); //(weatherResponse.Main.Temp - 273.15).ToString() + " °C";
                }
                else
                {
                    temp.InnerText = "No Data";
                }
                //if (weatherResponse.Main.TempMax != 0)
                //{
                //    temp_max.InnerText = GeneralFunctions.convertTemperature(weatherResponse.Main.TempMax, unit); //(weatherResponse.Main.TempMax - 273.15).ToString() + " °C";
                //}
                //else
                //{
                //    temp_max.InnerText = "No Data";
                //}
                //if (weatherResponse.Main.TempMin != 0)
                //{
                //    temp_min.InnerText = GeneralFunctions.convertTemperature(weatherResponse.Main.TempMin, unit); //(weatherResponse.Main.TempMin - 273.15).ToString() + " °C";
                //}
                //else
                //{
                //    temp_min.InnerText = "No Data";
                //}
                name.InnerText = weatherResponse.Name;
                CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
                TextInfo textInfo = cultureInfo.TextInfo;
                weatherDescription.InnerText = textInfo.ToTitleCase(weatherResponse.Weather[0].Description);
                weatherIcon.Src = "http://openweathermap.org/img/wn/" + weatherResponse.Weather[0].Icon + ".png";
                country.InnerText = weatherResponse.Sys.Country;
            }
            catch(Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// function to get and updata elements of hourly weather
        /// </summary>
        protected void updateForecastHourly()
        {
            try
            {
                //gets user georeference
                string latitude = this.latitude.Text;
                string longitude = this.longitude.Text;
                //gets OpenWeather hourly info
                OpenWeatherHourlyResponse weatherResponse = OpenWeather.GetOpenWeatherHourlyObjectAsync(latitude, longitude);
                //prepares info to draw the graph
                generateGraphMatrix(weatherResponse.List);
                //Generates hourly info grid data source
                var dt = generareWeatherGrid(weatherResponse.List);
                gridForecast.DataSource = dt;
                gridForecast.DataBind();
                ;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// Prepares the info to send to Google Chart and draw the line chart
        /// </summary>
        /// <param name="weatherList">The list of weather info</param>
        protected void generateGraphMatrix(List<List> weatherList)
        {
            string unit = this.dwlMeasureUnit.SelectedValue;
            List<Object[]> graphMatrix = new List<object[]>();
            try
            {
                //takes the first 8 elements to draw them
                for (int i=0; i<8; i++ )
                {
                    string date = GeneralFunctions.FromUnixTime(weatherList[i].Dt).ToLocalTime().ToString("yyyy-MM-dd HH:mm");
                    Double temp = GeneralFunctions.convertTemperature(weatherList[i].Main.Temp, unit); // weatherList[i].Main.Temp - 273.15;

                    Object[] ArrayOfObjects = new Object[] { date, temp };
                    graphMatrix.Add(ArrayOfObjects);
                }
            }
            catch(Exception e)
            {
                throw e;
            }
            
            //prepares the info for javascript format
            string strArrayG1 = (new JavaScriptSerializer()).Serialize(graphMatrix);
            //sends it to javascript
            this.array1Values.Value = strArrayG1;
        }
        /// <summary>
        /// Generates weather grid data source
        /// </summary>
        /// <param name="weatherList"></param>
        /// <returns></returns>
        protected DataTable generareWeatherGrid(List<List> weatherList)
        {
            string unit = this.dwlMeasureUnit.SelectedValue;
            DataTable response = new DataTable();

            //generates table structure
            DataColumn Datetime = new DataColumn();
            Datetime.DataType = System.Type.GetType("System.String");
            Datetime.ColumnName = "Datetime";
            Datetime.AutoIncrement = false;
            response.Columns.Add(Datetime);

            DataColumn Temperature = new DataColumn();
            Temperature.DataType = System.Type.GetType("System.String");
            Temperature.ColumnName = "Temperature";
            Temperature.AutoIncrement = false;
            response.Columns.Add(Temperature);

            DataColumn Weather = new DataColumn();
            Weather.DataType = System.Type.GetType("System.String");
            Weather.ColumnName = "Weather";
            Weather.AutoIncrement = false;
            response.Columns.Add(Weather);

            DataColumn Icon = new DataColumn();
            Icon.DataType = System.Type.GetType("System.String");
            Icon.ColumnName = "Icon";
            Icon.AutoIncrement = false;
            response.Columns.Add(Icon);


            try
            {
                //takes the first 8 elements
                for (int count = 0; count < 8; count++)
                {
                    DataRow row;
                    row = response.NewRow();

                    // formats the new row
                    row["Datetime"] = " " + GeneralFunctions.FromUnixTime(weatherList[count].Dt).ToLocalTime().ToString("yyyy-MM-dd HH:mm")+" ";
                    row["Temperature"] = GeneralFunctions.convertTemperatureAndFormat(weatherList[count].Main.Temp, unit); //weatherList[count].Main.Temp - 273.15;
                    row["Weather"] = weatherList[count].Weather[0].Description;
                    row["Icon"] = "http://openweathermap.org/img/wn/" + weatherList[count].Weather[0].Icon + ".png";
                    response.Rows.Add(row);
                }
            }
            catch
            {

            }
            return response;
        }

        /// <summary>
        /// updates everything via the button click function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dwlMeasureUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnUpdate_Click(sender, e);
        }
    }
}