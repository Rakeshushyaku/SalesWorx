<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="Rep_TravelReport.aspx.vb" Inherits="SalesWorx_BO.Rep_TravelReport" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../scripts/jquery-1.8.2.min.js" type="text/javascript"></script>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
    <telerik:RadScriptBlock runat="server" ID="rs">
        <script type="text/javascript">
            function OnCalendarViewChanging(sender, eventArgs) {
                sender.set_autoPostBack(false);
            }
            function OnDateClick(sender, eventArgs) {
                sender.set_autoPostBack(true);
            }
            function alertCallBackFn(arg) {

            }
        </script>
        <%-- <script type="text/javascript">
              function handleBegin() {
                  locationList = [];
              }

              function handleEnd() {
              }

              Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(handleBegin);
              Sys.WebForms.PageRequestManager.getInstance().add_endRequest(handleEnd);


              // Setting Default value of Activity combo
              $(document).ready(function () {
                  var combo = $find('<%=ddlOptions.ClientID%>');
             combo.set_text('Customer Visited');

             // Hide Map Area in initial load
             $("#map_canvas").hide();

         });

    </script>--%>
        <%--    <script type="text/javascript" >
          

            function OnPopupOpening(sender, args) {
                sender.get_calendar().raise_dayRender();
                //  $("#MainContent_BtnDummy").click()
                return false
            }

            // ****  For Option dropdown ///
            onCheckBoxClick = function (chk) {

                //Prevent second RadComboBox from closing.
                cancelDropDownClosing = true;
                var text = "";
                var values = "";
                var combo = $find('<%=ddlOptions.ClientID%>');

                var items = combo.get_items();


                for (var i = 0; i < items.get_count() ; i++) {
                    var item = items.getItem(i);
                    var chk1 = $get(combo.get_id() + "_i" + i + "_chk1");
                    if (chk1.checked) {
                        text += item.get_text() + ",";
                        values += item.get_value() + ",";
                    }
                }


                text = removeLastComma(text);
                values = removeLastComma(values);

                if (text.length > 0) {
                    combo.set_text(text);
                }
                else {
                    combo.set_text("");
                }

            }

            function removeLastComma(str) {
                return str.replace(/,$/, "");
            }

            $(".rcPrev").live("click", function () {
                // $("#MainContent_BtnDummy").click()

                //  var datpicker = $find('<%=RadCalendar1.ClientID%>'); 
        //   datpicker.get_calendar().raise_dayRender();
        //   alert(datpicker);
        return false;
    });


    // ****  For Option dropdown ends here ///


</script> --%>
        <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?sensor=false&key=AIzaSyAOpIPxhcIJ4PCHkIAl5ed_eP2cfu5HYsg"></script>
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
                //span.style.cssText = 'position: relative; left: -50%; top: -8px; ' +
                //     'white-space: nowrap; border: 1px solid blue; ' +
                  //   'padding: 2px; background-color: white';

                //var div = this.div_ = document.createElement('div');
                //div.appendChild(span);
               // div.style.cssText = 'position: absolute; display: none';
            }
            Label.prototype = new google.maps.OverlayView();

            // Implement onAdd
            Label.prototype.onAdd = function () {
                var pane = this.getPanes().floatPane;
                //pane.appendChild(this.div_);

                // Ensures the label is redrawn if the text or position is changed.
                var me = this;
                this.listeners_ = [
    google.maps.event.addListener(this, 'position_changed',
        function () { me.draw(); }),
    google.maps.event.addListener(this, 'text_changed',
        function () { me.draw(); })
                ];
            };

            // Implement onRemove
            Label.prototype.onRemove = function () {
                var i, I;
                this.div_.parentNode.removeChild(this.div_);

                // Label is removed from the map, stop updating its position/text.
                for (i = 0, I = this.listeners_.length; i < I; ++i) {
                    google.maps.event.removeListener(this.listeners_[i]);
                }
            };

            // Implement draw
            Label.prototype.draw = function () {
                var projection = this.getProjection();
                var position = projection.fromLatLngToDivPixel(this.get('position'));

                //var div = this.div_;
              //  div.style.left = position.x + 'px';
               // div.style.top = position.y + 'px';
              //  div.style.display = 'block';

               // this.span_.innerHTML = this.get('text').toString();
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

                google.maps.event.addListener(marker, 'click', function () {
                    infowindow.setContent(contentString + "<br>" + marker.getPosition().toUrlValue(6));
                    infowindow.open(map, marker);
                });
                return marker;

            }

            var prev_infowindow = false;
            function initialize() {
                $("#MainContent_tblSummary").show()
                $("#map_canvas").show()
                
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
                        zoom: 14,
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
                            else if (i == locationList.length - 1) {
                                var image = "http://maps.google.com/mapfiles/ms/icons/red.png";
                            }
                            else if (i < locationList.length - 1 && i > 0) {
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

                                    var argsnext = locationList[i + 1].split(',');

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
                                        lng: parseFloat(args[1])
                                    });

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
                                        strokeColor: '#00b3fd',
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

            function clearOverlays() {
                initialize()
                
            }


            function putMarkerOnRouteHalf2(percentage, dist, flightPath, Time) {

                var distance = (percentage / 100) * dist;
                // var time = ((percentage / 100) * totalTime / 60).toFixed(2);
                var time = Math.round(Time / 60.0, 2)
                // alert("Time:"+time+" totalTime:"+totalTime+" totalDist:"+totalDist+" dist:"+distance);
                //alert(distance)
                marker = null
                if (!marker) {
                    marker = createMarker(flightPath.GetPointAtDistance(distance), "time: " + time, "marker");
                    var myLabel = new Label();
                    myLabel.bindTo('position', marker, 'position');
                    //myLabel.set('text', 'Time: ' + time + ' Min <br/>Distance:' + Math.round(dist / 1000.0, 2) + ' KM');
                    myLabel.set('text', 'Time: ' + time + ' Min');
                    myLabel.setMap(map);
                } else {
                    marker.setPosition(flightPath.GetPointAtDistance(distance));
                    marker.setTitle("time:" + time);
                }

            }
            var rad = function (x) {
                return x * Math.PI / 180;
            };

            var getDistance = function (p1, p2) {
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
            {
                content: message[number],
                size: new google.maps.Size(250, 50)
            });

                google.maps.event.addListener(marker, 'click', function () {
                    if (prev_infowindow) {
                        prev_infowindow.close();
                    }

                    prev_infowindow = infowindow;
                    infowindow.open(map, marker);
                }
        );

            }

            

            // Hide Map Area
            function HideMap() {
                $("#map_canvas").hide()
            }


        </script>
    </telerik:RadScriptBlock>
    <style>
        #contentofpage a:link
        {
            color: black !important;
            text-decoration: none !important;
        }

        .RadComboBox_Simple .rcbInner.rcbReadOnly
        {
            border-color: transparent !important;
            padding: 0 !important;
            background-color: #fff !important;
        }

        div.RadComboBoxDropDown_Simple .rcbHovered
        {
            padding: 4px 10px !important;
            border: 0px solid transparent !important;
        }
    </style>



</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h4>Travel Report</h4>

    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <contenttemplate>--%>
                <telerik:RadAjaxPanel runat ="server" ID="rap">
               <telerik:RadWindowManager ID="RadWindowManager1" Skin="Simple" runat="server" EnableShadow="true">
    </telerik:RadWindowManager>
     <asp:HiddenField ID="hfDefLat" runat="server" />
                                        <asp:HiddenField ID="hfDefLng" runat="server" />
                 <telerik:RadPanelBar runat="server" ID="rpbFilter" Skin="Simple" Width="100%"
                            ExpandMode="MultipleExpandedItems">
                            <Items>
                                <telerik:RadPanelItem Expanded="True" Text=" ">

                                    <ContentTemplate>
                                        <div class="row">
                                            <div class="col-sm-10 col-md-10 col-lg-10">
                                                <div class="row">
                                                    <div class="col-sm-6">
                                                        <div class="form-group">
                                                            <label>Organization<em><span>&nbsp;</span>*</em></label>
                                                            <telerik:RadComboBox Skin="Simple" Filter="Contains" ID="ddOraganisation" Width="100%"
                                                                runat="server" DataTextField="Description" DataValueField="MAS_Org_ID"
                                                                AutoPostBack="True">
                                                            </telerik:RadComboBox>

                                                        </div>
                                                    </div>
                                                    <div class="col-sm-6">
                                                        <div class="form-group">
                                                            <label> Visited By<em><span>&nbsp;</span>*</em></label>
                                                            <telerik:RadComboBox Skin="Simple" Filter="Contains" ID="ddlVan" Width="100%" 
                                                                runat="server" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID" AutoPostBack="true" >
                                                            </telerik:RadComboBox>
                                                        </div>
                                                    </div>

                                                </div>
                                             
                                          

                                            </div>


                                            <div class="col-sm-2">
                                                <div class="form-group">
                                                    <label>&nbsp;</label>
                                                    <asp:Button CssClass="btn btn-sm btn-block btn-primary" ID="SearchBtn" runat="server" Text="Search" />
                                                    <asp:Button  CssClass ="btn btn-sm btn-block btn-default"  ID="ClearBtn" runat="server" Text="Clear"  />  
                                                </div>
                          
                                            </div>
                                               <div class="row">
                                                    <div class="col-sm-6">
                                                        <div class="form-group">
                                                            <div id="tblSummary" runat ="server"  visible="false"  >
                                                             <div id="raddiv" runat="server">
                                                            <label> Visited Dates<em><span>&nbsp;</span>*</em></label>
                                                            <telerik:RadCalendar ID="RadCalendar1" Skin="Simple" runat="server" TitleFormat="MMMM-yyyy"
                                            EnableMultiSelect="false" OnDayRender="CustomizeDay" AutoPostBack ="true"   OnSelectionChanged ="RadCalendar1_SelectionChanged" >
                                           <ClientEvents  OnDateClick="OnDateClick" />    
                                        </telerik:RadCalendar>
                                                                 </div>
                                                                 </div>
                                                        </div>
                                                    </div>
                                                 </div>
                                        </div>



                                    </ContentTemplate>
                                </telerik:RadPanelItem>
                            </Items>
                        </telerik:RadPanelBar>

                <div id="Args" runat="server" visible="false">
                            <div id="popoverblkouter">
                                Hover on icon to view search criteria <i class="fa fa-info-circle">
                                    <span class="popoverblk">
                                        <p>
                                            <strong>Organisation: </strong>
                                            <asp:Label ID="lbl_org" runat="server" Text=""></asp:Label>
                                        </p>
                                        <p>
                                            <strong>Van: </strong>
                                            <asp:Label ID="lbl_van" runat="server" Text=""></asp:Label>
                                        </p>
                                        <p>
                                            <strong>Visit Date: </strong>
                                            <asp:Label ID="lbl_from" runat="server" Text=""></asp:Label>
                                        </p>
                                       <%-- <p>
                                            <strong>Activity: </strong>
                                            <asp:Label ID="lbl_Activity" runat="server" Text=""></asp:Label>
                                        </p>--%>
                                    </span>
                                </i>
                            </div>
                        </div>

              

                                                         
                                                                  
                                                    
                                                            
                                                           
                                                                  
                                                                   <div id="divmap" runat="server" visible="false" >
                                                                   <div id="map_canvas" style ="height:478px;width:100%;">
                </div></div>
                                                                
                                                          
          <asp:Button style="display:none " CssClass="btn btn-sm btn-block btn-primary" ID="BtnDummy" runat="server" Text="Search" />

                  </telerik:RadAjaxPanel>                                                                              
                                                     
   







    <asp:UpdateProgress ID="UpdateProgress2" runat="server" DisplayAfter="10">
        <progresstemplate>
                        <asp:Panel ID="Panel11" CssClass="overlay" runat="server">

                            <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="vertical-align: middle;" />
                            <span style="font-size: 12px; font-weight: bold; color: #3399ff;">Processing... </span>
                        </asp:Panel>



          </progresstemplate>
    </asp:UpdateProgress>





   
</asp:Content>
