<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ShowMap.aspx.vb" Inherits="SalesWorx_BO.ShowMap" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html lang="en">
<head id="Head1" runat="server">
    <meta charset="utf-8" />
    <title><%: Page.Title %></title>
    <asp:PlaceHolder ID="PlaceHolder1" runat="server">
        <%: Scripts.Render("~/bundles/modernizr") %>
    </asp:PlaceHolder>

   
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <link rel="icon" href="../images/favicon.ico" type="image/x-icon" />


    <link href="../assets/css/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="../assets/css/font-awesome.css" rel="stylesheet" type="text/css" />
    <link href="../assets/css/style.css" rel="stylesheet" type="text/css" />
    <link href="../assets/css/responsive.css" rel="stylesheet" type="text/css" />
    <style>
        body {
            margin:0;
            padding:0;
        }
        #tips {
            background: #fff;
            border-radius: 2px;
            box-shadow: 0 1px 4px -1px rgba(0, 0, 0, 0.3);
            font-family: "Segoe UI","Trebuchet MS",Arial;
            font-size: 12px;
            line-height: 18px;
            margin: 10px;
            padding: 5px 5px 4px 2px;
            position: absolute;
            right: 0;
            z-index: 1;
        }
        #tips img{
            width:21px;
            height:auto;
            vertical-align:bottom;
        }
    </style>
    <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?sensor=false&key=AIzaSyAOpIPxhcIJ4PCHkIAl5ed_eP2cfu5HYsg"></script>
     <script type="text/javascript">
         function RefreshChart() {
             initialize1()
         }

         function ShowVisit() {
             initialize2()
         }
         

         function initialize2() {
             var markers = [];
             var map;
             var location;
             var Deflat = '25.000000'
             var Deflng = '55.000000'

            var myOptions;
            var markersArray = [];
            var lat = document.getElementById('<%= hflat.ClientID%>');
             var lng = document.getElementById('<%= hfLng.ClientID%>');
             var Custlat = document.getElementById('<%= CustLat.ClientID%>');
             var Custlng = document.getElementById('<%= CustLng.ClientID%>');

            if (lat.value != 0.0000000000) {

                var myLatlng = new google.maps.LatLng(lat.value, lng.value);
            }
            else {
                var myLatlng = new google.maps.LatLng(Deflat.value, Deflng.value);

                //lat = "25.000000";
                //lng = "55.000000";
                lat = Deflat.value;
                lng = Deflng.value;
            }



            myOptions = {
                zoom: 13,
                center: myLatlng,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            }
            map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);

            document.getElementById("map_canvas").style.height = '550px';

            var image = "http://maps.google.com/mapfiles/ms/icons/red.png";
            var image1 = "http://maps.google.com/mapfiles/ms/icons/green.png";

            if (lat.value != 0.0000000000) {
                var location = new google.maps.LatLng(lat.value, lng.value)
                var marker = new google.maps.Marker({
                    position: location,
                    map: map,
                    icon: image
                });

                markers.push(marker);

                
            }
            
            if (Custlat.value != 0.0000000000) {
              
                var location1 = new google.maps.LatLng(Custlat.value, Custlng.value)
                var marker1 = new google.maps.Marker({
                    position: location1,
                    map: map,
                    icon: image1
                });
                markers.push(marker1);
            }
          
            if (Custlat.value != 0.0000000000 && lat.value != 0.0000000000) {
                var bounds = new google.maps.LatLngBounds();
                if (markers.length > 0) {

                    for (var i = 0; i < markers.length; i++) {
                        bounds.extend(markers[i].getPosition());
                    }
                    map.fitBounds(bounds);
                }
            }

          
        }

        function alertCallBackFn(arg) {

        }

        function initialize1() {
            var markers = [];
            var map;
            var location;
            var Deflat = '25.000000'
            var Deflng = '55.000000'

            var myOptions;
            var markersArray = [];
            var lat = document.getElementById('<%= hflat.ClientID%>');
            var lng = document.getElementById('<%= hfLng.ClientID%>');
             if (lat.value != '0.0000000000') {

                 var myLatlng = new google.maps.LatLng(lat.value, lng.value);
             }
             else {
                 var myLatlng = new google.maps.LatLng(Deflat.value, Deflng.value);

                 //lat = "25.000000";
                 //lng = "55.000000";
                 lat = Deflat.value;
                 lng = Deflng.value;
             }



             myOptions = {
                 zoom: 13,
                 center: myLatlng,
                 mapTypeId: google.maps.MapTypeId.ROADMAP
             }
             map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);

             document.getElementById("map_canvas").style.height = '550px';

             var image = "http://maps.google.com/mapfiles/ms/icons/red.png";
             

             if (lat.value != 0.0000000000) {
                 var location = new google.maps.LatLng(lat.value, lng.value)
                 var marker = new google.maps.Marker({
                     position: location,
                     map: map,
                     icon: image
                 });


                 markers.push(marker);

                
             }

             //var bounds = new google.maps.LatLngBounds();
             //if (markers.length > 0) {

             //    for (var i = 0; i < markers.length; i++) {
             //        bounds.extend(markers[i].getPosition());
             //    }
             //    map.fitBounds(bounds);
             //}
             
         }
    </script>
</head>
<body>
<form id="form1" runat="server">
    <div>
            <asp:HiddenField ID="hflat" runat="server"  Value="0.0000000000" />
            <asp:HiddenField ID="hfLng" runat="server"  Value="0.0000000000" />
            <asp:HiddenField ID="CustLat" runat="server" Value="0.0000000000" />
            <asp:HiddenField ID="CustLng" runat="server"  Value="0.0000000000" />
            <asp:Label runat="server" ID="lblNoMap" CssClass="txt" ForeColor="Red" Text="Latitude and Logitude are not defined"></asp:Label>
   
            <div id="mapwrap" runat="server"> 
                                             
            <div id="tips" runat="server">
                <img src="http://maps.google.com/mapfiles/ms/icons/red.png" /><span> Visit Location</span>
                <img src="http://maps.google.com/mapfiles/ms/icons/green.png" /><span> Customer Location</span>
            </div> 
                                              
            <div id="map_canvas"></div>
        </div>
    </div>
</form>
</body>
</html>
