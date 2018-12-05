<%@ Page Title="Customer Location" Language="vb" AutoEventWireup="false" EnableEventValidation="false"
    CodeBehind="CustomerLocation.aspx.vb"
    Inherits="SalesWorx_BO.CustomerLocation" %>


<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
   <script type="text/javascript" src="../js/jquery-1.3.2.min.js"></script>
<link href="../facebox/facebox.css" media="screen" rel="stylesheet" type="text/css"/>
<script src="../facebox/facebox.js" type="text/javascript"></script> 
<link href="../styles/UpdateProgress.css" rel="stylesheet" type="text/css">
<link href="../styles/swxstyle.css" rel="stylesheet" type="text/css">
<link href="../styles/salesworx.css" rel="stylesheet" type="text/css">
<link rel="stylesheet" type="text/css" href="../styles/superfish.css" media="screen">

    <style type="text/css">
        .formatText
        {
            color: Maroon;
            font-size: 11px;
            font-family: Calibri;
            font-weight: bold;
        }
    </style>

  
</head> 
<body>

                          <div class="pgtitile3" style ="width:250px">
                                    Customer Visit Location
                                    </div>
                                    
                                      <table runat ="server" cellspacing ="15"  id="Table2" style="font-size:11px;margin: 0px;  background:#fff; text-align: center;padding-left:200px;">
                                    <tr>
                                    <td runat ="server" id="customercol">
                                       
          
            <asp:Image ID="Image1" ImageAlign ="Top"  ImageUrl ="http://maps.google.com/mapfiles/ms/icons/red.png"  Width ="16px" Height ="16px" runat="server" style="vertical-align:middle;" />Actual Customer Location
         </td>
         <td runat ="server" id="startcol">
         <asp:Image ID="Image5"  ImageAlign ="Top"  ImageUrl ="http://maps.google.com/mapfiles/ms/icons/green.png" Width ="16px" Height ="16px" runat="server" style="vertical-align:middle;" />Visit Start
         </td> 
         <td runat ="server" id="endcol">
            <asp:Image ID="Image6" ImageAlign ="Top"  ImageUrl ="http://maps.google.com/mapfiles/ms/icons/blue.png"  Width ="16px" Height ="16px" runat="server" style="vertical-align:middle;" />Visit End 
            </td> 
            </tr> 
            </table> 
                                    
                                    
                                   
                                 
 

  <script type="text/javascript" src="http://maps.google.com/maps/api/js?sensor=false"></script>

    <script type="text/javascript">

        var map;
        var prev_infowindow = false;


        function initialize() {

         
            if (locationList) {

             
                if (locationList.length > 0) {
                    var args = locationList[0].split(",");
                    if (args[0] == "") {
                        args[0] = 0;
                    }
                    if (args[1] == "") {
                        args[1] = 0;
                    }
                    var myLatlng = new google.maps.LatLng(args[0], args[1]);
                }
                else {
                    var myLatlng = new google.maps.LatLng(25.000000, 55.000000);



                }
                var myOptions = {
                    zoom: 8,
                    center: myLatlng,
                    mapTypeId: google.maps.MapTypeId.ROADMAP
                }
                map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);

                map.getCenter();
                if (locationList.length > 0) {
                  
                    for (var i = 0; i < locationList.length; i++) {
                                              var args1 = locationList[i].split(",");
                                              
                        if (args1[0] == "") {
                            args1[0] = 0;
                        }
                        if (args1[1] == "") {
                            args1[1] = 0;
                        }
                        if (args1[2] == "CUSTOMER") {

                         
                            var image = "http://maps.google.com/mapfiles/ms/icons/red.png";
                        }
                        if (args1[2] == "START") {
                        
                            var image = "http://maps.google.com/mapfiles/ms/icons/green.png";
                           
                        }
                        if (args1[2] == "END") {
                            
                            var image = "http://maps.google.com/mapfiles/ms/icons/blue.png";

                        }
                      
                        if (args1[0] != '0.000000') {

                            var location = new google.maps.LatLng(args1[0], args1[1])
                         
                            var marker = new google.maps.Marker({
                                position: location,
                                map: map,
                                icon: image


                            });
                          

                            attachMessage(marker, i);
                       }
                        var j = i + 1;

                                             //marker.setAnimation(google.maps.Animation.DROP);

                       
                    }

                   


                  //  google.maps.event.addListenerOnce(map, 'idle', function() {
                       // google.maps.event.trigger(map, 'resize');
                   // });

                }
            }

        }






        function attachMessage(marker, number) {

            var infowindow = new google.maps.InfoWindow(
                { content: message[number],
                    size: new google.maps.Size(50, 50)
                });

            google.maps.event.addListener(marker, 'click', function() {
                if (prev_infowindow) {
                    prev_infowindow.close();
                }

                prev_infowindow = infowindow;
                infowindow.open(map, marker);
            }
            );

        }

    </script>

     <form id="Form1" runat="server" class="outerform">
   
    <table cellpadding="0" width="100%" id="Table1" valign="top">
    
                <tr valign="top">
                   
                    <td width="100%" height="100%" valign="top" id="rht" runat="server">
                    
                        
                              <div style ="margin:0px;">
		   <div id="map_canvas" style="height:600px;margin:0px;"></div> 
		  </div>
                       
                    </td>
                </tr>
       
    </table>
  </form> 
</body> 
</html> 
 

