<%@ Page Title="Visits Tracking Report" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master"
    CodeBehind="FSRVisitTracking.aspx.vb" Inherits="SalesWorx_BO.FSRVisitTracking" %>

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

        <script type='text/javascript'>


            var map;
            var prev_infowindow = false;

            var markers = [];


            function initialize() {
               
                //Showing Map Area
                $("#map_canvas").show()

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

                  
                  

                   // var markerBounds = new google.maps.LatLngBounds();

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
                            var image = args[2];

                           

                            if (args[0] != '0.0000000000' & args[0] != Deflat.value) {

                              //  var latlng = new google.maps.LatLng(args[0], args[1]);
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
                                marker.setAnimation(google.maps.Animation.DROP); attachMessage(marker, i);
                            }










                        }
                        //var bounds = new google.maps.LatLngBounds();
                        //if (markers.length > 1) {
                        //    alert(markers.length);
                        //    for (var q = 0; q < markers.length; q++) {

                        //        bounds.extend(markers[q].getPosition());
                        //    }
                        //    map.fitBounds(bounds);
                        //}
                        var bounds = new google.maps.LatLngBounds();
                        if (markers.length >0) {

                            for (var i = 0; i < markers.length; i++) {
                                bounds.extend(markers[i].getPosition());
                            }
                            map.fitBounds(bounds);
                           
                        }
                       
                    }
                }
            }
            function clearOverlays() {
               
                for (var i = 0; i < markers.length; i++) {
                    markers[i].setMap(null);
                }
                markers = [];
                initialize();
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

    <h4>Visits Tracking Report</h4>

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
                                                                runat="server" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID">
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

              <asp:Button style="display:none " CssClass="btn btn-sm btn-block btn-primary" ID="BtnDummy" runat="server" Text="Search" />
          
     
  
        
             
                                                       <table width ="100%" cellpadding ="2" cellspacing ="2" id="tblSummary" runat ="server" >

                                                           <tr valign ="top">
                                                               <td width="17%" valign="top" >
                                                       <label>Visited Dates <em><span>&nbsp;</span>*</em></label>
                                                          <%--  <telerik:RadDatePicker ID="RadCalendar1" runat="server"  AutoPostBack="true"  >
                                                                <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy">
                                                                </DateInput>
                                                                  <ClientEvents OnPopupOpening="OnPopupOpening" /> 
                                                                <Calendar AutoPostBack="false"  ID="Calendar2" runat="server"  OnDayRender="RadCalendar2_DayRender">
                                                                    <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                                </Calendar>
                                                                
                                                            </telerik:RadDatePicker>--%>
                                                             <telerik:RadCalendar ID="RadCalendar1" Skin="Simple" runat="server" TitleFormat="MMMM-yyyy"
                                            EnableMultiSelect="false" OnDayRender="CustomizeDay" AutoPostBack ="true"   OnSelectionChanged ="RadCalendar1_SelectionChanged" >
                                           <ClientEvents  OnDateClick="OnDateClick" />    
                                        </telerik:RadCalendar>


                                                           <%--  <asp:Button Style="display:none" CssClass="btn btn-primary" ID="BtnExportExcel" runat="server" Text="Export" />--%>

                                                            
                                                           
                                                                   </td> 
                                                               <td  rowspan ="2" valign="top" width="1%"></td>
                                                               <td width="82%" rowspan ="2" valign="top" >
                                                                   <div id="map_canvas" style ="height:478px;width:100%;">
                </div>
                                                               </td>
                                                    
                                                     </tr> 

                                                      <tr  valign ="top">
                                                       <td width="17%" valign ="top" rowspan ="2" >
                                                         
                                                        <label>Activity<em><span>&nbsp;</span>*</em> </label>
                                                                              <telerik:RadGrid ID="rgvLegend" DataSourceID="sqlLegend"
                                            AllowSorting="false" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                                            PageSize="10" AllowPaging="false" runat="server" AllowFilteringByColumn="false" ShowHeader ="false" 
                                            GridLines="None">

                                            <GroupingSettings CaseSensitive="false"></GroupingSettings>
                                            <ClientSettings EnableRowHoverStyle="true">
                                            </ClientSettings>
                                            <MasterTableView AutoGenerateColumns="false" TableLayout="Fixed" Width="100%" GridLines="None" BorderColor="LightGray"
                                                DataSourceID="sqlLegend" AllowFilteringByColumn="false"
                                                PageSize="10">


                                                <PagerStyle Mode="NextPrevAndNumeric"  AlwaysVisible="true"></PagerStyle>

                                                <Columns>
                                                    <telerik:GridTemplateColumn UniqueName="EditColumn" AllowFiltering="false"
                                                        InitializeTemplatesFirst="false">


                                                        <ItemTemplate>

                                                            <asp:CheckBox ID="chkSelected" runat="server" CausesValidation="false"
                                                                Checked='<%# Bind("IsSelected")%>' OnCheckedChanged ="chkSelected_CheckedChanged" AutoPostBack ="true"  />

                                                        </ItemTemplate>
                                                        <HeaderStyle Width="20px" />
                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    </telerik:GridTemplateColumn>
                                                           <telerik:GridTemplateColumn UniqueName="Img" AllowFiltering="false" 
                                                        HeaderStyle-Font-Size="12px" HeaderStyle-Font-Bold="true"
                                                        InitializeTemplatesFirst="false">


                                                        <ItemTemplate>

                                                            <asp:Image ID="imgon" Width="18px" Height="18px" ImageUrl='<%# Bind("Img")%>' runat="server" />
                                                            <asp:Label runat ="server" ID="lbTranCode" Text ='<%# Bind("Code")%>'  Visible ="false" ></asp:Label>
                                                        </ItemTemplate>

                                                        <HeaderStyle HorizontalAlign="Center" wrap="false"  Width="20px" />
                                                        <ItemStyle HorizontalAlign="Center"  Width="20px" ></ItemStyle>
                                                    </telerik:GridTemplateColumn>

                                                    <telerik:GridBoundColumn UniqueName="ColumName" HeaderStyle-Font-Bold="true"
                                                        SortExpression="ColumName" HeaderText="" DataField="ColumName"
                                                        ShowFilterIcon="false">
                                                        <ItemStyle Wrap="false" Width="90px" />
                                                        <HeaderStyle Wrap ="false" Width="90px" />
                                                    </telerik:GridBoundColumn>




                                             
                                                 

                                                </Columns>

















                                            </MasterTableView>


                                        </telerik:RadGrid>
                                        <asp:SqlDataSource ID="sqlLegend" runat="server" ConnectionString="<%$ ConnectionStrings:SWXConnectionString %>"
                                            SelectCommand="app_LoadVisitLegend" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                                                   </td> 
                                                          </tr> 

                                                             
                                                           </table> 
           <%--  <telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="ddlOptions"   Visible ="false" 
                                                                DataValueField="OptionID" DataTextField="Description" EmptyMessage="Select Activity" HighlightTemplatedItems="true"
                                                                Width="100%" 
                                                                  Skin="Simple">
                                                                <ItemTemplate>
                                                                    <div >
                                                                        <label>
                                                                            <asp:CheckBox  runat="server" ID="chk1" onclick="onCheckBoxClick(this)" Checked='<%# Eval("IsChecked")%>'/>
                                                                            <img height="24px" width="24px" src='<%# Eval("ImagePath")%>' alt="icon" />&nbsp;&nbsp; <%# Eval("Description")%>
                                                                        </label>  
                                                                    </div>
                                                                </ItemTemplate>
                                                            </telerik:RadComboBox>--%>

                  </telerik:RadAjaxPanel>                                                                              
                                                     
    <%--    </contenttemplate>
    </asp:UpdatePanel>--%>







    <asp:UpdateProgress ID="UpdateProgress2" runat="server" DisplayAfter="10">
        <progresstemplate>
                        <asp:Panel ID="Panel11" CssClass="overlay" runat="server">

                            <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="vertical-align: middle;" />
                            <span style="font-size: 12px; font-weight: bold; color: #3399ff;">Processing... </span>
                        </asp:Panel>



          </progresstemplate>
    </asp:UpdateProgress>





    <%--<div style="display:none">
    <div style ="float:left;">
                                 <asp:CheckBox ID="chkVisit" runat ="server" Text ="Customer Visited"   />
                                    </div> 
                                                             <div style ="float:right;">
                                    <asp:Image ID="Image2" Width ="20px" Height ="20px" ImageUrl="http://maps.google.com/mapfiles/ms/icons/green.png" runat="server" />
                                    </div> 

                                                             <div style ="float:left;">
                                      <asp:CheckBox ID="chkInvoice" runat ="server" Text ="Invoice" />
                                    </div> 
                                                             <div style ="float:right;">
                                    <asp:Image ID="Image5" Width ="20px" Height ="20px" ImageUrl="http://maps.google.com/mapfiles/ms/icons/purple.png" runat="server" />
                                    </div> 


                                                             <div style ="float:left;">
                                       <asp:CheckBox ID="chkOrder" runat ="server" Text ="Bulk Order" />
                                   </div> 
                                                             <div style ="float:right;">
                                    <asp:Image ID="Image9" Width ="20px" Height ="20px" ImageUrl="http://maps.google.com/mapfiles/ms/icons/yellow.png" runat="server" />
                                    </div> 
                                                             <div style ="float:left;">
                                      <asp:CheckBox ID="chkRMA" runat ="server" Text ="Credit Notes" />
                                    </div> 
                                                             <div style ="float:right;">
                                    <asp:Image ID="Image6" Width ="20px" Height ="20px" ImageUrl="http://maps.google.com/mapfiles/ms/icons/orange.png" runat="server" />
                                    </div> 

                                                             <div style ="float:left;">
                                        <asp:CheckBox ID="chkDC" runat ="server" Text ="Distribution Check"  />
                                   </div> 
                                                             <div style ="float:right;">
                                    <asp:Image ID="Image7" Width ="20px" Height ="20px" ImageUrl="http://maps.google.com/mapfiles/ms/icons/blue.png" runat="server" />
                                    </div> 

                                                             <div style ="float:left;">
                                        <asp:CheckBox ID="chkCollection" runat ="server" Text ="Collection" />
                                    </div> 
                                                             <div style ="float:right;">
                                    <asp:Image ID="Image8" Width ="20px" Height ="20px" ImageUrl="http://maps.google.com/mapfiles/ms/icons/pink.png" runat="server" />
                                    </div>
</div>--%>
</asp:Content>
