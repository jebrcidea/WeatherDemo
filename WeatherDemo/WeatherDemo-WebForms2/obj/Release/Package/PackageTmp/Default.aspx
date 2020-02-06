<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WeatherDemo_WebForms2._Default" %>

  
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<asp:UpdatePanel ID="UpdatePanel1" runat="server">
		<ContentTemplate>
            <div class="row">
                <div class="col-12 col-md-6 col-lg-4 weatherBackground">
                    <div class="weatherTitleText text-center">Current Weather in your location</div>
			        <div runat="server" class="alertPadding"><b>Location:</b> <span id="name" runat="server">No Data</span></div>
			        <div runat="server" class="alertPadding"><b>Country:</b> <span id="country" runat="server">No Data</span></div>
			        <%--<div runat="server" class="alertPadding"><b>Feels like:</b> <span id="feels_like" runat="server">No Data</span></div>--%>
                    <%--<div runat="server">Humidity: <span id="humidity" runat="server"></span></div>
			        <div runat="server">Pressure: <span id="pressure" runat="server"></span></div>--%>
			        <div runat="server" class="alertPadding"><b>Temperature:</b> <span id="temp" runat="server">No Data</span></div>
			        <%--<div runat="server" class="alertPadding"><b>Max temperature:</b> <span id="temp_max" runat="server">No Data</span></div>
			        <div runat="server" class="alertPadding"><b>Min temperature:</b> <span id="temp_min" runat="server">No Data</span></div>--%>
			        <div runat="server" class="centerVertically"><b>Weather:</b>
                        <span id="weatherDescription" runat="server"></span>
                        <img id="weatherIcon" runat="server" class="img-fluid weatherIcon fondoAzul" />
			        </div>
                    <div runat="server" class="centerVertically"><b>Unit:</b>
                        <asp:DropDownList runat="server" ID="dwlMeasureUnit" OnSelectedIndexChanged="dwlMeasureUnit_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Text="Celsius" Value="C" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Farenheit" Value="F"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
			        <%--<div runat="server">Weather API: <span id="weather" runat="server"></span></div>--%>
			        <div runat="server" class="alertPadding"><b>Latitude:</b> 
                        <asp:TextBox runat="server" ID="latitude" CssClass="textBoxClass" onkeypress="return NumberFloatAndOneDOTSign(this)"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1"
                            ControlToValidate="latitude" runat="server"
                            ErrorMessage="Only float Numbers allowed"
                            ValidationExpression="^-?[0-9]\d*(\.\d+)?$" CssClass="alert alert-danger">
                        </asp:RegularExpressionValidator>
			        </div>
                    
			        <div runat="server" class="alertPadding"><b>Longitude: </b>
                        <asp:TextBox runat="server" ID="longitude" CssClass="textBoxClass" onkeypress="return NumberFloatAndOneDOTSign(this)"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2"
                            ControlToValidate="longitude" runat="server"
                            ErrorMessage="Only float Numbers allowed"
                            ValidationExpression="^-?[0-9]\d*(\.\d+)?$"  CssClass="alert alert-danger">
                        </asp:RegularExpressionValidator>
			        </div>
                    <br />
                    <div runat="server" class="alert" id="spanMessage" ></div>
			        <asp:Button runat="server" Text="Update weather" OnClick="btnUpdate_Click" ID="btnUpdate" 
                        UseSubmitBehavior="false" OnClientClick='showOverlay();' BackColor="PaleVioletRed" />
                    
                </div>
                
                 <div class="col-12 col-md-6 col-lg-7 centerVertically ">
                     <div class="weatherTitleText text-center">Forecast</div>
                     <div >
                         <div class="row fondoAzul">
                             <asp:HiddenField runat="server" ID="array1Values" OnValueChanged="Page_Load"/>
                             <div id="curve_chart" class="chart"></div>
                         </div>
                         <div class="row gridBackground" >
                             <asp:DataGrid runat="server" ID="gridForecast" AllowPaging="false" AutoGenerateColumns="false" PageSize="8" CssClass="dataGridStyle" Width="100%">
                                 <Columns>
                                     <asp:BoundColumn HeaderText=" Date & time " DataField="datetime" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center"></asp:BoundColumn>
                                     <asp:BoundColumn HeaderText=" Temperature " DataField="temperature" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center"></asp:BoundColumn>
                                     <asp:BoundColumn HeaderText=" Weather " DataField="weather" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center"></asp:BoundColumn>
                                     <asp:TemplateColumn HeaderText="Icon"  ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center">
                                         <ItemTemplate>
                                             <div class="fondoAzulSimple">
                                                <asp:Image runat="server" ID="imgProduct" ImageUrl=<%# Eval("Icon") %> Width="30px" />
                                             </div>
                                         </ItemTemplate>
                                    </asp:TemplateColumn>
                                 </Columns>
                             </asp:DataGrid>
                         </div>
                     </div>
                 </div>
            </div>
            

            
	        <div id="overlay">
		        <div class="cv-spinner">
			        <span class="spinner"></span>
		        </div>
	        </div>
			
		</ContentTemplate>
	</asp:UpdatePanel>
    <asp:UpdateProgress id="updateProgress" runat="server">
        <ProgressTemplate>
            <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #000000; opacity: 0.7;">
                <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="resources/img/ajax-loading.gif" AlternateText="Loading ..." ToolTip="Loading ..." style="padding: 10px;position:fixed;top:45%;left:50%;" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

	<script>

        //if there's not lat and long (first time), it gets the user gps
        if (document.getElementById('<%= latitude.ClientID %>').value === "") {
            //if the browser supports it
            if (navigator.geolocation) {
                $("#overlay").fadeIn(300);
                navigator.geolocation.getCurrentPosition(showPosition, showError);

            }
            //if the browser doesn't supports it
            else {
                var spanMessage = document.getElementById('<%= spanMessage.ClientID %>');
                spanMessage.innerHTML = "Geolocation is not supported by this browser. Please enter coordinates";
                $("#overlay").fadeOut(300);
            }
        }

        //gets the user position and forces a click to call the API
        function showPosition(position) {
            var latitude = document.getElementById('<%= latitude.ClientID %>');
            var longitude = document.getElementById('<%= longitude.ClientID %>');
            var btnUpdate = document.getElementById('<%= btnUpdate.ClientID %>');

            latitude.value = position.coords.latitude;
            longitude.value = position.coords.longitude;
            $("#overlay").fadeIn(300);
            btnUpdate.click();
            var spanMessage = document.getElementById('<%= spanMessage.ClientID %>');
            spanMessage.classList.remove("alert-danger");
            spanMessage.classList.add("alert-success");
            spanMessage.innerHTML = "Weather updated successfully";
            $("#overlay").fadeOut(300);
        }
        //calls the animation every ajax send
        $(document).ajaxSend(function () {
            $("#overlay").fadeIn(300);
        });

        //if user denied geolocalization permissions or something else failed
        function showError(error) {
            switch (error.code) {
                case error.PERMISSION_DENIED:
                    var spanMessage = document.getElementById('<%= spanMessage.ClientID %>');
                    spanMessage.classList.remove("alert-success");
                    spanMessage.classList.add("alert-danger");
                    spanMessage.innerHTML = "User denied the request for Geolocation. Please enter coordinates manually";
                    $("#overlay").fadeOut(300);
                    break;
                case error.POSITION_UNAVAILABLE:
                    var spanMessage = document.getElementById('<%= spanMessage.ClientID %>');
                    spanMessage.classList.remove("alert-success");
                    spanMessage.classList.add("alert-danger");
                    spanMessage.innerHTML = "Location information is unavailable. Please enter coordinates manually";
                    $("#overlay").fadeOut(300);
                    break;
                case error.TIMEOUT:
                    var spanMessage = document.getElementById('<%= spanMessage.ClientID %>');
                    spanMessage.classList.remove("alert-success");
                    spanMessage.classList.add("alert-danger");
                    spanMessage.innerHTML = "The request to get user location timed out.";
                    $("#overlay").fadeOut(300);
                    break;
                case error.UNKNOWN_ERROR:
                    var spanMessage = document.getElementById('<%= spanMessage.ClientID %>');
                    spanMessage.classList.remove("alert-success");
                    spanMessage.classList.add("alert-danger");
                    spanMessage.innerHTML = "An unknown error occurred. Please enter coordinates manually";
                    $("#overlay").fadeOut(300);
                    break;
            }
        }

        function showOverlay() {
            $("#overlay").fadeOut(300);
        }

        function hideOverlay() {
            $("#overlay").fadeOut(300);
        }

        //function to validate the format number is correct
        function NumberFloatAndOneDOTSign(CurrentElement)
        {
            var charCode = (event.which) ? event.which : event.keyCode;

            //if it's not a number and not . or -
            if (charCode != 46 && charCode != 45 && charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            //if dot sign entered more than once then don't allow to enter dot sign again. 46 is the code for dot sign or 45 -
            if ((charCode == 46 && CurrentElement.value.indexOf('.') >= 0) || (charCode == 45 && CurrentElement.value.indexOf('-') >= 0))
                return false;
            //if the - is not the first element
            else if (charCode == 45 && CurrentElement.value.length != 0) //allow to enter dot sign if not entered.
                return false;
            else if ((charCode == 46 && CurrentElement.value.indexOf('.') <= 0) || (charCode == 45 && CurrentElement.value.indexOf('-') <= 0 )) //allow to enter dot sign if not entered.
                return true;
            else
                return true;
        }

        function weatherGraph() {
            //gets the info obtained by the WS
            var arreglo = document.getElementById('<%= array1Values.ClientID%>').value;
            if (arreglo != "") {
                //converts it to an array
                var arregloArray = JSON.parse(arreglo);

                var array1= new Array;

                //prepares chart's data
                array1.push(['Date & Time', 'Temperature']);
                var cont = 1;
                arregloArray.forEach(function (element) {
                    array1[cont] = element;
                    cont += 1;
                });
                //array1.push(arregloArray);
                //console.log(array1);

                google.charts.load('current', { 'packages': ['line'] });
                //google.charts.setOnLoadCallback(drawChart);
                google.charts.setOnLoadCallback(function () { drawChart(array1); });
            }
           
        }
        //draws the chart
        function drawChart(arreglo) {

            var data = new google.visualization.arrayToDataTable(arreglo);
            //data.addColumn('DateTime', 'Temperature in °C');
            //console.log("drawChart(): " + arreglo);
            //data.addRows(arreglo);

            var options = {
                chart: {
                    title: 'Temperature',
                    subtitle: '',
                    position:'center'
                },
                legend: {
                    position: 'none'
                },
                height: 200/*,
                width: 900,
                height: 500*/
            };

            var chart = new google.charts.Line(document.getElementById('curve_chart'));

            chart.draw(data, google.charts.Line.convertOptions(options));
        }
	</script>
    
    <script>
        //after every ajax request, it updates the graph
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function (s, e) {
            weatherGraph();
        });

        //if the window is resized, the graph gets updated
        $(window).resize(function () {
            weatherGraph();
        });


    </script>
</asp:Content>
			


    
