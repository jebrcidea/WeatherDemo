# WeatherDemo
Demo available until february 10th 2020 at https://enviosms.com.mx/weatherDemo/
<br /><br />

Bellow you'll find the instructions for running this Demo. One for debugging or running locally and another one for publishing to an IIS server.
<br /><br />
<b>Instructions for debugging (using Visual Studio):</b><br />
1. Download the repository either zip download or using git<br />
2.Go to the WeatherDemo folder and open WeatherDemo.sln using Visual Studio, preferably 2019.<br />
3.On the upper center of the the interface, you'll see a run button with a green arrow. click it<br />
4.Visual studio will open a new browse window with the webpage<br />
5. Test and enjoy<br />
<br /><br />
<b>Instructions for publishing on Windows Server</b><br />
1.Make sure your server/computer has IIS. If you're not sure follow this instructions to find out (https://www.itechtics.com/check-iis-version/). If you dont have it you can get it following the next instructions: https://www.howtogeek.com/112455/how-to-install-iis-8-on-windows-8/<br />
2. Download the repository either zip download or using git<br />
3. Go to WeatherDemo/WeatherDemo-WebForms2/bin/Release/Publish which is a pre-published version of the application<br />
4. Either copy this folder's content to another one or use this one as your application folder<br />
5. Open IIS<br />
6. Expand SERVERNAME<br />
7a. Add it to IIS as a new web page using this guide https://support.microsoft.com/en-us/help/323972/how-to-set-up-your-first-iis-web-site<br />
7b. Add it to IIS as a new application to a web page using this guide https://docs.microsoft.com/en-us/iis/configuration/system.applicationhost/sites/site/application/<br />
8. Navigate your application either clicking on IIS link or opening your browser and going to the address you gave it
9. Test and enjoy<br />
<br /><br />

Note: Using user's current geolocation only works on https for security reasons
