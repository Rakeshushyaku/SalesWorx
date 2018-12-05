<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="testmap.aspx.vb" Inherits="SalesWorx_BO.testmap" %>
<%@ Register Assembly="DropCheck" Namespace="xMilk" TagPrefix="cc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <title>Route Planner - Customer List Test</title>
    <script type="text/javascript" src="../js/jquery-1.3.2.min.js"></script>

<link href="../facebox/facebox.css" media="screen" rel="stylesheet" type="text/css"/>
<script src="../facebox/facebox.js" type="text/javascript"></script> 
<link href="../styles/UpdateProgress.css" rel="stylesheet" type="text/css">
<link href="../assets/css/bootstrap.css" rel="stylesheet" type="text/css">
<%--<link href="../assets/css/style.css" rel="stylesheet" type="text/css">
<link href="../styles/salesworx.css" rel="stylesheet" type="text/css">
<link rel="stylesheet" type="text/css" href="../styles/superfish.css" media="screen">--%>
</head>
<body>
    <form id="form1" runat="server">
        <div id="map" style="width:100%;height:320px"></div>
           <%-- <script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyAOpIPxhcIJ4PCHkIAl5ed_eP2cfu5HYsg&callback=clearOverlays1"></script>--%>

<script>
    function myMap() {
        var mapCanvas = document.getElementById("map");
        var mapOptions = {
            center: new google.maps.LatLng(47.3921611, 8.4957963),
            zoom: 14
        }

        var contentString = '<div id="content">' +
          '<div id="siteNotice">' +
          '</div>' +
          '<h1 id="firstHeading" class="firstHeading">Uluru</h1>' +
          '<div id="bodyContent">' +
          '<p><b>Uluru</b>, also referred to as <b>Ayers Rock</b>, is a large ' +
          'sandstone rock formation in the southern part of the ' +
          'Northern Territory, central Australia. It lies 335&#160;km (208&#160;mi) ' +
          'south west of the nearest large town, Alice Springs; 450&#160;km ' +
          '(280&#160;mi) by road. Kata Tjuta and Uluru are the two major ' +
          'features of the Uluru - Kata Tjuta National Park. Uluru is ' +
          'sacred to the Pitjantjatjara and Yankunytjatjara, the ' +
          'Aboriginal people of the area. It has many springs, waterholes, ' +
          'rock caves and ancient paintings. Uluru is listed as a World ' +
          'Heritage Site.</p>' +
          '<p>Attribution: Uluru, <a href="https://en.wikipedia.org/w/index.php?title=Uluru&oldid=297882194">' +
          'https://en.wikipedia.org/w/index.php?title=Uluru</a> ' +
          '(last visited June 22, 2009).</p>' +
          '</div>' +
          '</div>';

        var infowindow = new google.maps.InfoWindow({
            content: contentString
        });

        var map = new google.maps.Map(mapCanvas, mapOptions);
        var image = 'http://maps.google.com/mapfiles/ms/micons/blue.png';
        var beachMarker = new google.maps.Marker({
            position: map.getCenter(),
            map: map,
            icon: image,
        });
        beachMarker.addListener('click', function () {
            infowindow.open(map, beachMarker);
        });
        return false;
    }
</script>
<%--<script src="https://maps.googleapis.com/maps/api/js?callback=myMap"></script>--%>
<script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyAOpIPxhcIJ4PCHkIAl5ed_eP2cfu5HYsg&callback=myMap"></script>
      <%--  <script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyAOpIPxhcIJ4PCHkIAl5ed_eP2cfu5HYsg"></script>--%>
    <div>
    
    </div>
    </form>
</body>
</html>
