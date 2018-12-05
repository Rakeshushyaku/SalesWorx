<%@ Page  Language="vb" AutoEventWireup="false" CodeBehind="ReviewRoutePlan_View.aspx.vb" Inherits="SalesWorx_BO.ReviewRoutePlan_View" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

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
                            //if (i == 0) {

                            //    alert(args[3]);
                            //    if (args[3] == 'S') {
                            //        var image = "http://maps.google.com/mapfiles/ms/icons/green.png";
                            //    }
                            //}
                            //else if (i == locationList.length - 1) {

                            //    if (args[3] == 'E') {
                            //        var image = "http://maps.google.com/mapfiles/ms/icons/red.png";
                            //    }
                            //}
                            //else if (i < locationList.length - 1 && i > 0) {
                            //    var image = "http://maps.google.com/mapfiles/ms/icons/blue.png";
                            //}
                           
                            if (args[3] == 'S') {
                                var image = "http://maps.google.com/mapfiles/ms/icons/green.png";
                            }
                            else if (args[3] == 'E') {
                                var image = "http://maps.google.com/mapfiles/ms/icons/red.png";
                            }
                            else if (args[3] == 'I') {
                            
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
          
            </Scripts>
        </asp:ScriptManager>
    <telerik:RadAjaxPanel ID="rap" runat ="server" >
     <div class="table-responsive" id="Detailed"  runat="server">

          <telerik:RadTabStrip ID="Salestab" runat="server" Skin="Windows7" MultiPageID="RadMultiPage21"
                    SelectedIndex="0">
                    <Tabs>
                        <telerik:RadTab Text="View Details" runat="server">
                        </telerik:RadTab>

                         <telerik:RadTab Text="Map View " runat="server">
                        </telerik:RadTab>

                                              

                    </Tabs>
                </telerik:RadTabStrip>
          
           <telerik:RadMultiPage ID="RadMultiPage21" runat="server" SelectedIndex="0">


                    <telerik:RadPageView ID="RadPageView1" runat="server">
                      <asp:HiddenField id="hfPlanID" runat="server"></asp:HiddenField>
                      <p style="padding-top:15px;"><asp:Label ID="lblDesc1" Text="" runat="server" ></asp:Label></p>
                       <telerik:RadAjaxPanel ID="raf" runat="server">
                            
                         <telerik:RadGrid ID="rgVisits" DataSourceID="sqlVisits" 
                            AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                             PageSize="10"  AllowPaging="false" runat="server" AllowFilteringByColumn="false"
                            GridLines="None">
                                                
                                    <ClientSettings EnableRowHoverStyle="true">
           
                            </ClientSettings>
                                    <MasterTableView AutoGenerateColumns="false" TableLayout="Fixed" Width="100%" GridLines="None" BorderColor="LightGray" DataSourceID="sqlVisits" PageSize="10" >
                                               <GroupByExpressions>
                                                <telerik:GridGroupByExpression>
                                                    <SelectFields>
                                                        <telerik:GridGroupByField  FieldAlias="Date"  HeaderValueSeparator=" : "    FieldName="Visit_Date"></telerik:GridGroupByField>
                                                    </SelectFields>
                                                    <GroupByFields>
                                                        <telerik:GridGroupByField FieldName="Visit_Date"      SortOrder="Descending"></telerik:GridGroupByField>
                                                        
                                                    </GroupByFields>
                                                    
                                                </telerik:GridGroupByExpression>
                                            
                                            </GroupByExpressions>

                                <PagerStyle Mode="NextPrevAndNumeric"  AlwaysVisible="true"></PagerStyle>

                                <Columns>
                                     <telerik:GridBoundColumn UniqueName="Customer_No" 
                                        
                                        SortExpression="Customer_No" HeaderText="Customer No" DataField="Customer_No"
                                        ShowFilterIcon="false">
                                        <HeaderStyle Width="110px" Wrap="false" />
                                        <ItemStyle Wrap="false" />
                                    </telerik:GridBoundColumn>
                                       <telerik:GridBoundColumn UniqueName="CustomerName" 
                                        SortExpression="Customer_Name" HeaderText="Customer Name" DataField="Customer_Name"
                                        ShowFilterIcon="false" 
                                          >
                                        <HeaderStyle Width="270px" Wrap="false" />
                                        <ItemStyle Wrap="false" />
                                    </telerik:GridBoundColumn>

                                  <telerik:GridTemplateColumn UniqueName="StatusColumn" HeaderText="Visit Status" AllowFiltering="false" SortExpression ="VisitStatus"
                        InitializeTemplatesFirst="false">


                        <ItemTemplate>
                            <asp:Image ID="imgGreen" runat ="server" ToolTip ="Visited"  ImageUrl ="~/assets/img/Green.png" Visible='<%# Bind("GreenVisible")%>' />

                            <asp:Image ID="imgred" runat ="server" ToolTip ="Not Visited"  ImageUrl ="~/assets/img/Red.png" Visible='<%# Bind("RedVisible")%>' />

                        </ItemTemplate>
                        <HeaderStyle Width="100px" />
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                      <HeaderStyle HorizontalAlign ="Center" />
                    </telerik:GridTemplateColumn>


                                     <telerik:GridBoundColumn UniqueName="VisitStatus"  Visible ="false" 
                                        SortExpression="VisitStatus" HeaderText="Visit Status" DataField="VisitStatus"
                                        ShowFilterIcon="false">
                                        <HeaderStyle Width="110px" Wrap="false" />
                                        <ItemStyle Wrap="false" />
                                    </telerik:GridBoundColumn>
                                  
                                                                   
                                </Columns>
                            </MasterTableView>


                        </telerik:RadGrid>
                        <asp:SqlDataSource ID="sqlVisits" runat="server" ConnectionString="<%$ ConnectionStrings:SWXConnectionString %>"
                            SelectCommand="app_GetFSRVisitStatus" SelectCommandType="StoredProcedure">

                            <SelectParameters>


                                <asp:ControlParameter Name="FSRPlanID" ControlID="hfPlanID" Type="Int64" DefaultValue ="0"  />
                               
                            </SelectParameters>
                            </asp:SqlDataSource> 
                 </telerik:RadAjaxPanel> 
                                         
                                             </telerik:RadPageView>



                     <telerik:RadPageView ID="RadPageView2" runat="server">
                          <div class="row">
                                <div class="col-sm-6">
                                                        <div class="form-group">
                                                            <label>Date</label>
                                                            <telerik:RadDatePicker ID="txtVisitDate" runat="server" Width ="100%">
                                                                <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy">
                                                                </DateInput>
                                                                <Calendar ID="Calendar2" runat="server">
                                                                <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="OK" />
                                                                </Calendar>
                                                            </telerik:RadDatePicker>
                                                        </div>
                                                    </div>

                               <div class="col-sm-6">
                                                        <div class="form-group">
                                                            <label>&nbsp;</label>
                                                               <telerik:RadButton TabIndex="3" CausesValidation="false" ID="btnSearch" Skin="Simple" runat="server" Text="Search" CssClass="btn btn-primary" />
                                                                <asp:Label ID="lbl_errMsg" Text="" runat="server" ForeColor="Red" ></asp:Label>
                                                        </div>
                                                    </div>

                              </div>

                          <div class="row">
                                <div class="col-sm-12">
                                     <asp:Label ID="lbl_custmapmsg" Text="" runat="server" ForeColor="Red" ></asp:Label>
                                    </div>
                              </div>

                          <div class="row">
                                <div class="col-sm-2">
                                    <div style ="overflow :scroll;height:478px;">
                                    <telerik:RadGrid ID="rgCustomer" 
                                            AllowSorting="false" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                                            PageSize="10" AllowPaging="false" runat="server" AllowFilteringByColumn="false"
                                            GridLines="None">

                                            <GroupingSettings CaseSensitive="false"></GroupingSettings>
                                            <ClientSettings EnableRowHoverStyle="true">
                                            </ClientSettings>
                                            <MasterTableView AutoGenerateColumns="false" TableLayout="Fixed" Width="100%" GridLines="None" BorderColor="LightGray"
                                                AllowFilteringByColumn="false"
                                                PageSize="10">
                                                <PagerStyle Mode="NextPrevAndNumeric"  AlwaysVisible="true"></PagerStyle>
                                                 <Columns>
                                                
                                                  <telerik:GridBoundColumn UniqueName="Customer" HeaderStyle-Font-Bold="true"
                                                        SortExpression="Customer" HeaderText="Customer List" DataField="Customer"
                                                        ShowFilterIcon="false">
                                                        <ItemStyle Wrap="true" />
                                                        <HeaderStyle Wrap ="false" />
                                                    </telerik:GridBoundColumn>
                                                </Columns>
                                            </MasterTableView>


                                        </telerik:RadGrid>
                                        </div>
                                </div>
                             
                              <div class="col-sm-10">
                                    
                                
                                               <div id="divmap" runat="server" visible="false" >
                                                <div id="map_canvas" style ="height:478px;width:100%;"> </div></div>

                                  </div>

                                     </div>
               
         </telerik:RadPageView>

               </telerik:RadMultiPage>


         </div>
 </telerik:RadAjaxPanel>
</form>
</body>
</html>

