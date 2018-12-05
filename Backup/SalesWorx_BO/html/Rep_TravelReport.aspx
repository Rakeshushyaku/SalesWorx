<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/ReportMaster.Master" CodeBehind="Rep_TravelReport.aspx.vb" Inherits="SalesWorx_BO.Rep_TravelReport" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<style>
    #contentofpage a:link {
   color: black !important;
  text-decoration: none !important;
}
.ajax__calendar_container{background:#fff;}
</style>



<script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?&signed_in=true">
</script>
        <script type ="text/javascript" src="../js/v3_epoly.js"></script>

        

            <script type='text/javascript'>

                var directionsService;
                var directionsService;
                var map
                var polyline = null;
                var infowindow = new google.maps.InfoWindow();
                var flightPath

                function Label(opt_options) {
                    // Initialization
                    this.setValues(opt_options);

                    // Label specific
                    var span = this.span_ = document.createElement('span');
                    span.style.cssText = 'position: relative; left: -50%; top: -8px; ' +
                         'white-space: nowrap; border: 1px solid blue; ' +
                         'padding: 2px; background-color: white';

                    var div = this.div_ = document.createElement('div');
                    div.appendChild(span);
                    div.style.cssText = 'position: absolute; display: none';
                }
                Label.prototype = new google.maps.OverlayView();

                // Implement onAdd
                Label.prototype.onAdd = function() {
                    var pane = this.getPanes().floatPane;
                    pane.appendChild(this.div_);

                    // Ensures the label is redrawn if the text or position is changed.
                    var me = this;
                    this.listeners_ = [
        google.maps.event.addListener(this, 'position_changed',
            function() { me.draw(); }),
        google.maps.event.addListener(this, 'text_changed',
            function() { me.draw(); })
    ];
                };

                // Implement onRemove
                Label.prototype.onRemove = function() {
                    var i, I;
                    this.div_.parentNode.removeChild(this.div_);

                    // Label is removed from the map, stop updating its position/text.
                    for (i = 0, I = this.listeners_.length; i < I; ++i) {
                        google.maps.event.removeListener(this.listeners_[i]);
                    }
                };

                // Implement draw
                Label.prototype.draw = function() {
                    var projection = this.getProjection();
                    var position = projection.fromLatLngToDivPixel(this.get('position'));

                    var div = this.div_;
                    div.style.left = position.x + 'px';
                    div.style.top = position.y + 'px';
                    div.style.display = 'block';

                    this.span_.innerHTML = this.get('text').toString();
                };


                function createMarker(latlng, label, html) {

                    var contentString = '<b>' + label + '</b><br>' + html;
                    var marker = new google.maps.Marker({
                        position: latlng,
                        map: map,
                        title: label,
                        visible: false,
                        zIndex: Math.round(latlng.lat() * -100000) << 5
                    });
                    marker.myname = label;
                    // gmarkers.push(marker);

                    google.maps.event.addListener(marker, 'click', function() {
                        infowindow.setContent(contentString + "<br>" + marker.getPosition().toUrlValue(6));
                        infowindow.open(map, marker);
                    });
                    return marker;

                }
                var prev_infowindow = false;
                function initialize() {
                    if (locationList) {
                        if (locationList.length > 0) {
                            var args = locationList[0].split(',');
                            if (args[0] == '') {
                                args[0] = 0;
                            }
                            if (args[1] == '') {
                                args[1] = 0;
                            }
                            var myLatlng = new google.maps.LatLng(args[0], args[1]);
                        }
                        else {
                            var myLatlng = new google.maps.LatLng(25.000000, 55.000000);
                        }

                        
                        var myOptions = {
                            zoom: 12,
                            center: myLatlng,
                            mapTypeId: google.maps.MapTypeId.ROADMAP
                        }


                        map = new google.maps.Map(document.getElementById('map_canvas'), myOptions);
                        document.getElementById('map_canvas').style.height = '730px';
                        if (locationList.length > 1) {


                            // midArrows(locationList);
                          
                            for (var i = 0; i < locationList.length; i++) {

                                var args = locationList[i].split(',');
                               
                                if (args[0] == '') {
                                    args[0] = 0;
                                }
                                if (args[1] == '') {
                                    args[1] = 0;
                                }
                                if (i == 0) {
                                    var image = "http://maps.google.com/mapfiles/ms/icons/green.png";
                                }
                                else if (i == locationList.length - 1 ) {
                                    var image = "http://maps.google.com/mapfiles/ms/icons/red.png";
                                }
                                else if (i < locationList.length - 1 && i > 0 ) {
                                    var image = "http://maps.google.com/mapfiles/ms/icons/blue.png";
                                }
                                if (args[0] != '0.000000') {

                                    var location = new google.maps.LatLng(args[0], args[1])
                                    var marker = new google.maps.Marker({
                                        position: location,
                                        map: map,
                                        icon: image
                                    });

                                    if (i < locationList.length - 1) {
                                       
                                        var argsnext = locationList[i+1].split(',');

                                        if (argsnext[0] == '') {
                                            argsnext[0] = 0;
                                        }
                                        if (argsnext[1] == '') {
                                            argsnext[1] = 0;

                                        }

                                        var lineSymbol = {
                                            path: google.maps.SymbolPath.FORWARD_CLOSED_ARROW
                                        };


                                        var locationnext = new google.maps.LatLng(argsnext[0], argsnext[1])
                                    var flightPlanCoordinates = []
                                    
                                     flightPlanCoordinates.push({
                                     lat: parseFloat(args[0]),
                             lng: parseFloat(args[1])     });
                             
                              flightPlanCoordinates.push({
                              lat: parseFloat(argsnext[0]),
                              lng: parseFloat(argsnext[1])
                          });
                                    
//
                             //                                    alert(flightPlanCoordinates)
                             //console.debug(flightPlanCoordinates)
                                    
                                        flightPath = new google.maps.Polyline({
                                        path: flightPlanCoordinates,
                                        icons: [{
                                            icon: lineSymbol,
                                            offset: '100%'
}],
                                            geodesic: true,
                                            strokeColor: '#C93F27',
                                            strokeOpacity: 1.0,
                                            strokeWeight: 2
                                        });

                                       flightPath.setMap(map);

                                        var dist = getDistance(location, locationnext)
                                        console.debug(args[2])
                                        putMarkerOnRouteHalf2(50, dist, flightPath, args[2])
                                    }
                                }




                                var j = i + 1;
                                marker.setAnimation(google.maps.Animation.DROP);

                                attachMessage(marker, i);



                            }


                        }
                    }
                }

                function putMarkerOnRouteHalf2(percentage, dist, flightPath,Time) {

                    var distance = (percentage / 100) * dist;
                    // var time = ((percentage / 100) * totalTime / 60).toFixed(2);
                    var time =  Math.round(Time/60.0,2)
                    // alert("Time:"+time+" totalTime:"+totalTime+" totalDist:"+totalDist+" dist:"+distance);
                    //alert(distance)
                    marker = null
                    if (!marker) {
                        marker = createMarker(flightPath.GetPointAtDistance(distance), "time: " + time, "marker");
                        var myLabel = new Label();
                        myLabel.bindTo('position', marker, 'position');
                        myLabel.set('text', 'Time: ' + time + ' Min <br/>Distance:' + Math.round(dist/1000.0,2) + ' KM' );
                        myLabel.setMap(map);
                    } else {
                    marker.setPosition(flightPath.GetPointAtDistance(distance));
                        marker.setTitle("time:" + time);
                    }
   
                }
                var rad = function(x) {
                    return x * Math.PI / 180;
                };

                var getDistance = function(p1, p2) {
                    var R = 6378137; // Earth’s mean radius in meter
                    var dLat = rad(p2.lat() - p1.lat());
                    var dLong = rad(p2.lng() - p1.lng());
                    var a = Math.sin(dLat / 2) * Math.sin(dLat / 2) +
    Math.cos(rad(p1.lat())) * Math.cos(rad(p2.lat())) *
    Math.sin(dLong / 2) * Math.sin(dLong / 2);
                    var c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
                    var d = R * c;
                    return d; // returns the distance in meter

                };
                function attachMessage(marker, number) {

                    var infowindow = new google.maps.InfoWindow(
                { content: message[number],
                    size: new google.maps.Size(250, 50)
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
 <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
                <tr>
                    <td id="contentofpage" width="100%" height="76%" class="topshadow">
                        <div class="pgtitileposition">
                            <span class="pgtitile3">Travel Report</span></div>
                       
                        
                            
                
                       
            <table width ="100%"   border="0" cellspacing="0" cellpadding="0" >
                       <tr>
                       <td width="4%" height="100%"  valign ="top">
                          <asp:UpdatePanel ID="UPModal" runat="server"   >
                       <Triggers >
                          <asp:PostBackTrigger ControlID ="btnFilter"   />
                            <asp:PostBackTrigger ControlID ="btnReset"   />
                          </Triggers>
                        
    <ContentTemplate>
                    <div style ="float:left;margin:0px;">
             
                    <table cellpadding="2" cellspacing ="0" width="4%" >
                    
                     <tr>
                        <td width="3%" valign ="top" class ="txtSMBold" >
                         Organization<br />
                            <asp:DropDownList ID="ddOraganisation"  AutoPostBack="true" runat="server"  Width ="200px">
                                    </asp:DropDownList>
                                       
                        </td> 
                       
                    </tr>
                    
                    
                    <tr>
                        <td width="3%" valign ="top" class ="txtSMBold"  >
                          Van<br />
                           
                                    <asp:DropDownList ID="ddlVan" AutoPostBack="true" Width ="200px" runat="server" >
                                    </asp:DropDownList>
                                    
                        </td> 
                       
                    </tr>
                        <tr>
                           <td width="3%" valign ="top" class ="txtSMBold" >
                                    Visit Date<br />
                              
                                
                            <asp:TextBox  ID="txtFromDate"   Width ="150px" CssClass="inputSM" runat="server"></asp:TextBox>&nbsp;
                <ajaxToolkit:CalendarExtender  CssClass="cal_Theme1" ID="calendarButtonExtender" Format="dd-MMM-yyyy" runat="server" TargetControlID="txtFromDate" PopupButtonID="txtFromDate"  />                

                                
                               
                                 
                                   
                                </td>
                           
                         
                              
                        </tr>
                    
                           
                  
                                   <tr>
                                   <td  valign="top" class ="txtSMBold"  >
                                   <div style ="float:left;">
                                       
                                            <asp:Button ID="btnFilter" runat="server" Text="Filter" CssClass="btnInput"  />
                                              <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="btnInput" />
                                            </div> 
                                   </td>
                                   </tr>
                 
                    </table>
             
                    </div> 
                    
                            <table width="auto" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td>
                                <asp:Button ID="btnHidden" runat="Server" Style="display: none" />
                                <ajaxToolkit:ModalPopupExtender ID="MpInfoError" runat="server" BackgroundCssClass="modalBackground"
                                    TargetControlID="btnHidden" PopupControlID="pnlConfirm" CancelControlID="btnClose"
                                    Drag="true" />
                                <asp:Panel ID="pnlConfirm" Width="400" runat="server" CssClass="modalPopup" Style="cursor: move;
                                    background-color: #3399ff; text-align: center; border: solid 1px #3399ff; color: White;
                                    padding: 3px; display: none;">
                                    <table id="tableinPopupErr" width="400" cellpadding="10" style="background-color: White;">
                                        <tr align="center">
                                            <td align="center" style="cursor: move; background-color: #3399ff; text-align: center;
                                                border: solid 1px #3399ff; color: White; padding: 3px">
                                                <asp:Label ID="lblinfo" runat="server" Font-Size ="13px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" style="text-align: center">
                                                                                                <asp:Label ID="lblMessage"  Font-Size ="13px" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" style="text-align: center;">
                                                            <asp:Button ID="btnClose" runat="server" Text="Ok" CssClass="btnInputBlue" />
                                            </td>
                                        </tr>
                                    </table>
                                     </asp:Panel>
        </td>
        </tr>                        
                          
                    </table>
    </ContentTemplate> 
</asp:UpdatePanel>
                    </td> 
                    
                    <td width="96%" valign ="top"  height="100%">
                 
                 
                                 <div id="map_canvas" >
 

   
                           
                     </div> 
                    
                     </td> 
                     </tr>  
                     </table> 
               
                        </td> 
                        </tr> 
                        </table> 
                     
               
           <asp:UpdatePanel runat="server" ID="TopPanel" UpdateMode ="Conditional" >
                          
     </asp:UpdatePanel> 
  
  
    
  
           <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="UpModal"
                    runat="server">
                    <ProgressTemplate>
                        <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                            <img src="../images/Progress.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                            <span style="font-size: 12px; color: #3399ff;">Processing... </span>
                        </asp:Panel>
                    </ProgressTemplate>
                </asp:UpdateProgress>
              
         
</asp:Content>
