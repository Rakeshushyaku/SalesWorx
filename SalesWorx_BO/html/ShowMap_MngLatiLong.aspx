<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ShowMap_MngLatiLong.aspx.vb" Inherits="SalesWorx_BO.ShowMap_MngLatiLong" %>
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

         function ConfirmUpdate() {
             var geo_mod = document.getElementById('<%= hgeo_mod.ClientID%>');
                     
             if (geo_mod.value == "E") {
                 return confirm('Do you really want to update explicit capture location as current location ?');
             }
             else {
                 return confirm('Do you really want to update last visit as current location ?');
             }
         }
         function NumericOnly(e) {

             var keycode;

             if (window.event) {
                 keycode = window.event.keyCode;
             } else if (e) {
                 keycode = e.which;
             }
             if ((parseInt(keycode) >= 48 && parseInt(keycode) <= 57) || parseInt(keycode) == 8 || parseInt(keycode) == 46 || parseInt(keycode) == 0)
                 return true;

             return false;
         }

         function closePopup() {
            
             window.close();
         }
         function RefreshChart() {
             initialize1()
         }

         function ShowVisit(lat, lng, Custlat, Custlng) {
             initialize2(lat, lng, Custlat, Custlng)
             GetRadWindow().BrowserWindow.clickSearch();
         }

         function GetRadWindow() {
             var oWindow = null;
             if (window.radWindow) oWindow = window.radWindow;
             else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
             return oWindow;
         }


         function initialize2(lat, lng, Custlat, Custlng) {
          
             var markers = [];
             var map;
             var location;
             var Deflat = '25.3284'
             var Deflng = '55.5123'

             var myOptions;
             var markersArray = [];
             var lat = document.getElementById('<%= txtLocLatitude.ClientID%>'); //hflat
             var lng = document.getElementById('<%= txtLocLong.ClientID%>'); //hfLng
             var Custlat = document.getElementById('<%= CustLat.ClientID%>');
             var Custlng = document.getElementById('<%= CustLng.ClientID%>');

             if (lat.value != 0.0000000000) {
               
                 var myLatlng = new google.maps.LatLng(lat.value, lng.value);
             }
             else {
                
                 var myLatlng = new google.maps.LatLng("25.3284", "55.5123");
                
                 lat = "25.3284";
                 lng = "55.5123";
                 //lat = Deflat.value;
                 //lng = Deflng.value;
             }


            
             myOptions = {
                zoom: 13,
                 center: myLatlng,
                 mapTypeId: google.maps.MapTypeId.ROADMAP
             }
             map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);
            
             google.maps.event.addListenerOnce(map, 'bounds_changed', function () {
                 map.setZoom(7);
             });
             document.getElementById("map_canvas").style.height = '460px';

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
           
             if (parseFloat(Custlat.value) != 0.0000000000 && parseFloat(Custlng.value) != 0.0000000000) {

                     var location1 = new google.maps.LatLng(Custlat.value, Custlng.value)
                     var marker1 = new google.maps.Marker({
                         position: location1,
                         map: map,
                         icon: image1
                     });
                     markers.push(marker1);
                 }
         
             if (parseFloat( Custlat.value) != 0.0000000000 && parseFloat( lat.value) != 0.0000000000) {
                 var bounds = new google.maps.LatLngBounds();
                 if (markers.length > 0) {

                     for (var i = 0; i < markers.length; i++) {
                         bounds.extend(markers[i].getPosition());
                     }
                     map.fitBounds(bounds);
                 }
             }

             function clearOverlays() {
                 for (var i = 0; i < markers.length; i++) {
                     markers[i].setMap(null);
                 }
                 markers.length = 0;
             }





             google.maps.event.addListener(map, "click", function (event) {
                 var lat = event.latLng.lat();
                 var lng = event.latLng.lng();

                 clearOverlays();


                 var txtLat = document.getElementById('<%= txtLocLatitude.ClientID%>');
                          txtLat.value = lat

                          var txtLng = document.getElementById('<%= txtLocLong.ClientID%>');
            txtLng.value = lng

            location = new google.maps.LatLng(event.latLng.lat(), event.latLng.lng())
            marker = new google.maps.Marker({
                position: location,
                map: map
            });
            markers.push(marker);
            var Custlat = document.getElementById('<%= CustLat.ClientID%>');
            var Custlng = document.getElementById('<%= CustLng.ClientID%>');
                          var image1 = "http://maps.google.com/mapfiles/ms/icons/green.png";
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


                      });

                      google.maps.event.addListenerOnce(map, 'idle', function () {
                          google.maps.event.trigger(map, 'resize');
                      });






         }

         function alertCallBackFn(arg) {

         }

         function initialize1() {
             var markers = [];
             var map;
             var location;
             var Deflat = '25.3284'
             var Deflng = '55.5123'

             var myOptions;
             var markersArray = [];
             var lat = document.getElementById('<%= hflat.ClientID%>');
            var lng = document.getElementById('<%= hfLng.ClientID%>');
            if (lat.value != '0.0000000000') {

                var myLatlng = new google.maps.LatLng(lat.value, lng.value);
            }
            else {
                var myLatlng = new google.maps.LatLng(Deflat.value, Deflng.value);

                //lat = "25.3284";
                //lng = "55.5123";
                lat = Deflat.value;
                lng = Deflng.value;
            }



            myOptions = {
               zoom: 13,
                center: myLatlng,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            }
            map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);
            
            google.maps.event.addListenerOnce(map, 'bounds_changed', function () {
                map.setZoom(7);
            });

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
         function alertCallBackFn(arg) {
             HideRadWindow()
         }

         function HideRadWindow() {

             var elem = $('a[class=rwCloseButton');

             if (elem != null && elem != undefined) {
                 $('a[class=rwCloseButton')[0].click();
             }

             $("#frm").find("iframe").hide();
         }
    </script>
</head>
<body>
<form id="form1" runat="server">
<asp:ScriptManager ID="ScriptManager1" runat="server"  AsyncPostBackTimeOut="36000">
            <Scripts>
                <%--To learn more about bundling scripts in ScriptManager see http://go.microsoft.com/fwlink/?LinkID=272931&clcid=0x409 --%>
                <%--Framework scripts--%>
                <asp:ScriptReference Name="MsAjaxBundle" />
                <asp:ScriptReference Name="jquery" />
                <asp:ScriptReference Name="jquery.ui.combined" />
            <%--    <asp:ScriptReference Name="WebForms.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebForms.js" />
                <asp:ScriptReference Name="WebUIValidation.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebUIValidation.js" />
                <asp:ScriptReference Name="MenuStandards.js" Assembly="System.Web" Path="~/Scripts/WebForms/MenuStandards.js" />
                <asp:ScriptReference Name="GridView.js" Assembly="System.Web" Path="~/Scripts/WebForms/GridView.js" />
                <asp:ScriptReference Name="DetailsView.js" Assembly="System.Web" Path="~/Scripts/WebForms/DetailsView.js" />
                <asp:ScriptReference Name="TreeView.js" Assembly="System.Web" Path="~/Scripts/WebForms/TreeView.js" />
                <asp:ScriptReference Name="WebParts.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebParts.js" />
                <asp:ScriptReference Name="Focus.js" Assembly="System.Web" Path="~/Scripts/WebForms/Focus.js" />--%>
              <%--  <asp:ScriptReference Name="WebFormsBundle" />--%>
                <%--Site scripts--%>
            </Scripts>
        </asp:ScriptManager>
    <div class="col-xs-12">
         <p><asp:Label ID="lbl_msg"  runat ="server" ForeColor ="Red" ></asp:Label></p>  
            <asp:HiddenField ID="hflat" runat="server"  Value="0.0000000000" />
            <asp:HiddenField ID="hfLng" runat="server"  Value="0.0000000000" />
            <asp:HiddenField ID="CustLat" runat="server" Value="0.0000000000" />
            <asp:HiddenField ID="CustLng" runat="server"  Value="0.0000000000" />
            <asp:HiddenField ID="CustID" runat="server" Value="-1" />
              <asp:HiddenField ID="SiteID" runat="server" Value="-1" />
         <asp:HiddenField ID="hgeo_mod" runat="server"  Value="0" />
        <div class="row">
            <div class="col-sm-3">
                <div class="form-group">
                    <label>Latitude</label>
                    <asp:TextBox ID="txtLocLatitude" Width ="100%" CssClass="inputSM" onKeypress="return NumericOnly(event)" runat="server"></asp:TextBox>
                </div>
            </div>
              <div class="col-sm-3">
                  <div class="form-group">
                   <label>Longitude</label>
                   <asp:TextBox ID="txtLocLong" Width ="100%" CssClass="inputSM" onKeypress="return NumericOnly(event)" runat="server"></asp:TextBox>
                    </div>
              </div>
             <div class="col-sm-6">
                 <div class="form-group">
                     <label class="hidden-xs">&nbsp;</label>
                  <asp:Button   ID="btnUpdateLoc" ValidationGroup="valsum" runat="server" causesValidation="false"
                                     Text="Update"  CssClass="btn btn-success" OnClick="btnSet_Click"  />
                            <asp:Button   ID="btnUpdateLastVisit" ValidationGroup="valsum" runat="server" causesValidation="false"
                                     Text="Update From Last Visit"  CssClass="btn btn-info"  OnClientClick="return ConfirmUpdate();"  OnClick ="btnUpdateLastVisit_Click"  />
                                                         <asp:Button   ID="btnCancelLoc" runat="server" causesValidation="false"
                                     Text="Cancel" CssClass ="btn btn-default" OnClientClick ="closePopup();" />
                      
                     </div>
              </div>
        </div>
            
            

            <div id="mapwrap" runat="server"> 
                                             
            <div id="tips" runat="server">
                <img src="http://maps.google.com/mapfiles/ms/icons/red.png" /><span > Current Location</span>
                <img src="http://maps.google.com/mapfiles/ms/icons/green.png" /><span id="span2" runat ="server" > Last Visited Location</span>
            </div> 
                                              
            <div id="map_canvas"></div>
        </div>
    </div>
</form>
</body>
</html>
