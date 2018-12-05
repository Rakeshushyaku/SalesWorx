<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="Rep_ExpenseLocation.aspx.vb" Inherits="SalesWorx_BO.Rep_ExpenseLocation" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        #contentofpage a:link
        {
            color: black !important;
            text-decoration: none !important;
        }

        .ajax__calendar_container
        {
            background: #fff;
        }
    </style>
    <script src="../scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function alertCallBackFn(arg) {

        }

        function clickExportExcel() {
            $("#MainContent_BtnExportExcel").click()
            return false

        }
        function clickExportPDF() {
            $("#MainContent_BtnExportPDF").click()
            return false
        }

    </script>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <telerik:RadScriptBlock runat="server" ID="rs">
        <script type="text/javascript" src="http://maps.google.com/maps/api/js?sensor=false&key=AIzaSyAOpIPxhcIJ4PCHkIAl5ed_eP2cfu5HYsg"></script>

        <script type='text/javascript'>
            var markers = [];
            var map;
            var prev_infowindow = false;
            function initialize() {

                var Deflat = document.getElementById('<%= hfDefLat.ClientID%>');
            var Deflng = document.getElementById('<%= hfDefLng.ClientID%>');

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
                    var myLatlng = new google.maps.LatLng(Deflat.value, Deflng.value);



                }

                var myLatlng = new google.maps.LatLng(Deflat.value, Deflng.value);

                var myOptions = {
                    zoom: 8,
                    center: myLatlng,
                    mapTypeId: google.maps.MapTypeId.ROADMAP
                }


                map = new google.maps.Map(document.getElementById('map_canvas'), myOptions);
                //  document.getElementById('map_canvas').style.height = '730px';

                //  var markerBounds = new google.maps.LatLngBounds();

                if (locationList.length > 0) {


                    // midArrows(locationList);

                    for (var i = 0; i < locationList.length; i++) {

                        var args = locationList[i].split(',');

                        if (args[0] == '') {
                            args[0] = 0;
                        }
                        if (args[1] == '') {
                            args[1] = 0;
                        }
                        var image = "http://maps.google.com/mapfiles/ms/icons/green.png";

                        if (args[0] != '0.0000000000' & args[0] != Deflat.value) {
                            // var latlng = new google.maps.LatLng(args[0], args[1]);
                            // markerBounds.extend(latlng);

                            var location = new google.maps.LatLng(args[0], args[1])
                            var marker = new google.maps.Marker({
                                position: location,
                                map: map,
                                icon: image
                            });
                            markers.push(marker);
                        }




                        var j = i + 1;
                        if (markers.length > 0) {
                            marker.setAnimation(google.maps.Animation.DROP);

                            attachMessage(marker, i);
                        }

                        //Showing Map Area
                       // $("#map_canvas").show()

                    }

                    var bounds = new google.maps.LatLngBounds();
                    if (markers.length > 1) {

                        for (var i = 0; i < markers.length; i++) {
                            bounds.extend(markers[i].getPosition());
                        }
                        map.fitBounds(bounds);
                    }
                }
            }
        }

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
        function clearOverlays() {
            for (var i = 0; i < markers.length; i++) {
                markers[i].setMap(null);
            }
            markers = [];
            initialize();
        }

        // Hide Map Area
        function HideMap() {
          
            $("#map_canvas").hide()
        }

        function OnClientTabSelected(sender, eventArgs) {
            var tab = eventArgs.get_tab();

            //if (tab.get_index() == 0)
            //    initialize()


        }

        </script>

    </telerik:RadScriptBlock>
    <h4>Fuel Expense Location Report</h4>
    <telerik:RadAjaxPanel runat="server" ID="raf">
        <telerik:RadWindowManager ID="RadWindowManager1" Skin="Simple" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <asp:HiddenField ID="hfDefLat" runat="server" />
        <asp:HiddenField ID="hfDefLng" runat="server" />
        <%--   <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <contenttemplate>
               <Triggers >
                         <asp:PostBackTrigger ControlID ="btnFilter"   />
                         <asp:PostBackTrigger ControlID ="btnReset"   />
                  </Triggers>--%>


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
                                            <label>Van/FSR </label>
                                            <telerik:RadComboBox Skin="Simple" Filter="Contains" ID="ddlVan" Width="100%"
                                                runat="server" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID">
                                            </telerik:RadComboBox>
                                        </div>
                                    </div>

                                </div>
                                <div class="row">
                                    <div class="col-sm-6">
                                        <div class="row">
                                            <div class="col-sm-6">
                                                <div class="form-group">
                                                    <label>From Date </label>
                                                    <telerik:RadDatePicker ID="txtFromDate" Width="100%" runat="server">
                                                        <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy">
                                                        </DateInput>

                                                        <Calendar AutoPostBack="false" ID="Calendar2" runat="server">
                                                            <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                        </Calendar>

                                                    </telerik:RadDatePicker>

                                                </div>
                                            </div>

                                            <div class="col-sm-6">
                                                <div class="form-group">

                                                    <label>To Date </label>
                                                    <telerik:RadDatePicker ID="txtToDate" Width="100%" runat="server">
                                                        <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy">
                                                        </DateInput>

                                                        <Calendar AutoPostBack="false" ID="Calendar1" runat="server">
                                                            <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                        </Calendar>

                                                    </telerik:RadDatePicker>


                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>


                            </div>


                            <div class="col-sm-2">
                                <div class="form-group">
                                    <label>&nbsp;</label>
                                    <asp:Button CssClass="btn btn-sm btn-block btn-primary" ID="btnFilter" runat="server" Text="Search" />
                                    <asp:Button  CssClass ="btn btn-sm btn-block btn-default"  ID="ClearBtn" runat="server" Text="Clear"  />  


                                </div>
                                <div class="form-group fontbig text-center">
                                    <label>&nbsp;</label>
                                    <asp:HyperLink href="" CssClass="" ID="BtnExportDummyExcel" runat="server" OnClick="return clickExportExcel()"><i class="fa fa-file-excel-o text-success"></i></asp:HyperLink>
                                    <asp:HyperLink href="" CssClass="" ID="BtnExportDummyPDF" runat="server" OnClick="return clickExportPDF()"><i class="fa fa-file-pdf-o text-danger"></i></asp:HyperLink>

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
                            <strong>From Date: </strong>
                            <asp:Label ID="lbl_from" runat="server" Text=""></asp:Label>
                        </p>
                        <p>
                            <strong>To Date: </strong>
                            <asp:Label ID="lbl_To" runat="server" Text=""></asp:Label>
                        </p>
                    </span>
                </i>
            </div>
        </div>



        <telerik:RadTabStrip ID="LocationTab" Visible="false" runat="server" Skin="Windows7" MultiPageID="RadMultiPage21" OnClientTabSelected="OnClientTabSelected"
            SelectedIndex="0">
            <Tabs>
                <telerik:RadTab Text="Fuel Expenses Location" runat="server">
                </telerik:RadTab>

                <telerik:RadTab Text="Fuel Expenses" runat="server">
                </telerik:RadTab>

            </Tabs>
        </telerik:RadTabStrip>

        <telerik:RadMultiPage ID="RadMultiPage21" runat="server" SelectedIndex="0">
            <telerik:RadPageView ID="RadPageView1" runat="server">

                <div id="map_canvas" style="height: 478px; width: 100%;">
                </div>


            </telerik:RadPageView>

            <telerik:RadPageView ID="RadPageView2" runat="server">

                <%-- <asp:UpdatePanel ID="uPInner" runat="server" UpdateMode="Conditional"  >
                             <ContentTemplate>--%>
                <telerik:RadAjaxPanel runat="server" ID="pa">
                    <div class="table-responsive">
                        <div id="divCurrency" runat="server" visible="false">
                            <h5 class='text-right'>Currency <span class='text-blue'><strong>
                                <asp:Label ID="lbl_Currency" runat="server" Text=""></asp:Label></strong></span></h5>
                        </div>

                        <telerik:RadGrid ID="gvRep" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                            PageSize="11" AllowPaging="True" runat="server"
                            GridLines="None">

                            <GroupingSettings GroupContinuesFormatString="" CaseSensitive="false"></GroupingSettings>
                            <ClientSettings EnableRowHoverStyle="true">
                            </ClientSettings>
                            <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                                PageSize="11">


                                <PagerStyle PageSizeControlType="None" Mode="NextPrevAndNumeric"></PagerStyle>
                                <Columns>
                                    <%--           <telerik:GridBoundColumn  HeaderStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" DataField="SalesRep_name" HeaderText="SalesRep_name"
                                            SortExpression="SalesRep_name">
                                            <ItemStyle Wrap="False" />
                                        </telerik:GridBoundColumn>--%>

                                    <telerik:GridBoundColumn HeaderStyle-VerticalAlign="Top" ItemStyle-Width="25%" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" DataField="Logged_At" HeaderText="Logged At"
                                        SortExpression="Logged_At" DataFormatString="{0:dd-MMM-yyyy hh:mm tt}">
                                        <ItemStyle Wrap="False" />
                                    </telerik:GridBoundColumn>


                                    <telerik:GridBoundColumn HeaderStyle-VerticalAlign="Top" ItemStyle-Width="25%" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Left" DataField="Odo_Reading" HeaderText="Odo Reading"
                                        SortExpression="Odo_Reading" DataFormatString="">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </telerik:GridBoundColumn>

                                    <telerik:GridBoundColumn HeaderStyle-VerticalAlign="Top" ItemStyle-Width="25%" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Left" DataField="Fuel_Qty" HeaderText="Fuel Qty"
                                        SortExpression="Fuel_Qty" DataFormatString="">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </telerik:GridBoundColumn>


                                    <telerik:GridBoundColumn HeaderStyle-VerticalAlign="Top" ItemStyle-Width="25%" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Left" DataField="Fuel_Amount" HeaderText="Fuel Amount"
                                        SortExpression="Fuel_Amount" DataFormatString="">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </telerik:GridBoundColumn>


                                </Columns>

                                <GroupByExpressions>
                                    <telerik:GridGroupByExpression>
                                        <SelectFields>

                                            <telerik:GridGroupByField FieldAlias="Van" FieldName="SalesRep_name"></telerik:GridGroupByField>
                                        </SelectFields>
                                        <GroupByFields>

                                            <telerik:GridGroupByField FieldName="SalesRep_name"></telerik:GridGroupByField>
                                        </GroupByFields>
                                    </telerik:GridGroupByExpression>
                                </GroupByExpressions>

                            </MasterTableView>
                        </telerik:RadGrid>
                    </div>


                    <asp:HiddenField ID="hfCurrency" runat="server" Value=''></asp:HiddenField>
                    <asp:HiddenField ID="hfDecimal" runat="server" Value='0'></asp:HiddenField>
                </telerik:RadAjaxPanel>
                <%--     </ContentTemplate>
                         </asp:UpdatePanel>--%>
            </telerik:RadPageView>

        </telerik:RadMultiPage>

        <%--    </contenttemplate>
    </asp:UpdatePanel>--%>
    </telerik:RadAjaxPanel>
    <div style="display: none">
        <asp:Button CssClass="btn btn-primary" ID="BtnExportExcel" runat="server" Text="Export" />
        <asp:Button CssClass="btn btn-primary" ID="BtnExportPDF" runat="server" Text="Export" />
    </div>
    <asp:UpdateProgress ID="UpdateProgress2" runat="server" DisplayAfter="10">
        <progresstemplate>
                        <asp:Panel ID="Panel11" CssClass="overlay" runat="server">

                            <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="vertical-align: middle;" />
                            <span style="font-size: 12px; font-weight: bold; color: #3399ff;">Processing... </span>
                        </asp:Panel>



          </progresstemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="10">
        <progresstemplate>
                        <asp:Panel ID="Panel1" CssClass="overlay" runat="server">

                            <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="vertical-align: middle;" />
                            <span style="font-size: 12px; font-weight: bold; color: #3399ff;">Processing... </span>
                        </asp:Panel>



          </progresstemplate>
    </asp:UpdateProgress>



    <%--  <script type="text/javascript">
        var postBackElement;
        function handleBegin() {
            if (postBackElement != null && postBackElement.id.includes('btnFilter')) 
             locationList = [];          
        }

        function handleEnd(sender, args) {
            // Re initializing due to map resides inside the update panel
             
           // if (postBackElement != null && postBackElement.id.includes('btnFilter')) {
           //     alert('Call')
                initialize()
           // }
        }

        Sys.WebForms.PageRequestManager.getInstance().add_initializeRequest(InitializeRequest)
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(handleBegin);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(handleEnd);

        function InitializeRequest(sender, args) {

            if (Sys.WebForms.PageRequestManager.getInstance().get_isInAsyncPostBack())
                args.set_cancel(true);
            postBackElement = args.get_postBackElement();
        }


        // Setting Default value of Activity combo
        $(document).ready(function () {
                       // Hide Map Area in initial load
            $("#map_canvas").hide();

         });

    </script>--%>
</asp:Content>
