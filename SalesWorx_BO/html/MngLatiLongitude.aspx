<%@ Page Title="Geolocation Management" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="MngLatiLongitude.aspx.vb" Inherits="SalesWorx_BO.MngLatiLongitude" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
      <style>
       
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
    <script type="text/javascript" src="http://maps.google.com/maps/api/js?sensor=false&key=AIzaSyAOpIPxhcIJ4PCHkIAl5ed_eP2cfu5HYsg"></script>
    <script >

       
        function clickSearch() {
            $("#MainContent_btnFilter").click()
        }
        function OpenLocWindow1(Lat, Long, CustLat, CustLong) {

            var URL
            URL = 'ShowMap_MngLatiLong.aspx?Lat=' + Lat + '&Long=' + Long + '&Type=Visits&CustLat=' + CustLat + '&CustLong=' + CustLong;

            var oWnd = radopen(URL, null);
            oWnd.SetSize(900, 600);
            oWnd.set_behaviors(Telerik.Web.UI.WindowBehaviors.move + Telerik.Web.UI.WindowBehaviors.Close); //Close:4
            oWnd.SetModal(true);
            oWnd.Center;
            oWnd.set_visibleStatusbar(false)
            return false

        }
        function OpenLocWindow(Lat, Long, CustLat, CustLong, CustID, SiteID, geo_mod) {
              
      
            var URL
            URL = 'ShowMap_MngLatiLong.aspx?Lat=' + Lat + '&Long=' + Long + '&Type=Visits&CustLat=' + CustLat + '&CustLong=' + CustLong + '&CustID=' + CustID + '&SiteID=' + SiteID + '&geo_mod=' + geo_mod;
            var oWnd = radopen(URL, null);
            oWnd.SetSize(1000, 600);
            oWnd.set_behaviors(Telerik.Web.UI.WindowBehaviors.Close); //Close:4
            oWnd.SetModal(true);
            oWnd.Center;
            return false
        }
        
        function initialize2() {
            var markers = [];
            var map;
            var location;
            var Deflat = '25.000000'
            var Deflng = '55.000000'

            var myOptions;
            var markersArray = [];
            var lat = document.getElementById('<%= txtLoc_Latitude.ClientID%>');
            var lng = document.getElementById('<%= txtLoc_Long.ClientID%>');
                      var Custlat = document.getElementById('<%= hd_last_Lat.ClientID%>');
                      var Custlng = document.getElementById('<%= hd_last_Long.ClientID%>');

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
                    
                      if (parsefloat( Custlat.value) != 0.0000000000) {

                          var location1 = new google.maps.LatLng(Custlat.value, Custlng.value)
                          var marker1 = new google.maps.Marker({
                              position: location1,
                              map: map,
                              icon: image1
                          });
                          markers.push(marker1);
                      }

                      if (parsefloat(Custlat.value) != 0.0000000000 && parsefloat(lat.value) != 0.0000000000) {
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


                          var txtLat = document.getElementById('<%= txtLoc_Latitude.ClientID%>');
            txtLat.value = lat

            var txtLng = document.getElementById('<%= txtLoc_Long .ClientID%>');
            txtLng.value = lng

            location = new google.maps.LatLng(event.latLng.lat(), event.latLng.lng())
            marker = new google.maps.Marker({
                position: location,
                map: map
            });
            markers.push(marker);
                          
                          var Custlat = document.getElementById('<%= hd_last_Lat.ClientID%>');
                          var Custlng = document.getElementById('<%= hd_last_Long.ClientID%>');
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

        function initialize() {
            var map;
            var location;


            var myOptions;
            var markersArray = [];
            var lati, longit
            lati = document.getElementById('<%= txtLoc_Latitude.ClientID%>').value 
            longit = document.getElementById('<%= txtLoc_Long .ClientID%>').value
            if (parseFloat(lati) == 0  ) {
                lati = 25.000000
            }

            if (parseFloat(longit) == 0) {
                longit = 55.000000
            }


            var txtLat = document.getElementById('<%= txtLoc_Latitude.ClientID%>');
            txtLat.value = lati

            var txtLng = document.getElementById('<%= txtLoc_Long .ClientID%>');
            txtLng.value=longit 

            var myLatlng = new google.maps.LatLng(lati, longit);
           
        myOptions = {
            zoom: 8,
            center: myLatlng,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        }

        var bounds = new google.maps.LatLngBounds();
        location = new google.maps.LatLng(lati, longit)
      
        map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);
        
        map.setCenter(new google.maps.LatLng(lati, longit))
         
        var image = "http://maps.google.com/mapfiles/ms/icons/red.png";
        

        

        marker = new google.maps.Marker({
            position: location,
            map: map,
            icon: image
        });

        
        markersArray.push(marker);
        
         

        function clearOverlays() {
            for (var i = 0; i < markersArray.length; i++) {
                markersArray[i].setMap(null);
            }
            markersArray.length = 0;
        }

        google.maps.event.addListener(map, "click", function (event) {
            var lat = event.latLng.lat();
            var lng = event.latLng.lng();
            
            clearOverlays();


            var txtLat = document.getElementById('<%= txtLoc_Latitude.ClientID%>');
            txtLat.value = lat

            var txtLng = document.getElementById('<%= txtLoc_Long .ClientID%>');
            txtLng.value=lng
                     
                    location = new google.maps.LatLng(event.latLng.lat(), event.latLng.lng())
                    marker = new google.maps.Marker({
                        position: location,
                        map: map
                    });
                    markersArray.push(marker);
                });

                google.maps.event.addListenerOnce(map, 'idle', function () {
                    google.maps.event.trigger(map, 'resize');
                });

                
            }

       
        
        function showmap() {
           
            initialize2()
           
            return false 
        }
         

       </script>   
<script type="text/javascript">
        var TargetBaseControl = null;

        window.onload = function() {
            try {
                TargetBaseControl =
           document.getElementById('<%= Me.ClassUpdatePnl.ClientID %>');
            }
            catch (err) {
                TargetBaseControl = null;
            }
        }
        function TestCheckBox() {
          if (TargetBaseControl == null) return false;
          var TargetChildControl = "chkDelete";
             var TimeCon = document.getElementById('<%= Me.ClassUpdatePnl.ClientID %>');

          var Inputs = TargetBaseControl.getElementsByTagName("input");

           for (var n = 0; n < Inputs.length; ++n)
               if (Inputs[n].type == 'checkbox' && Inputs[n].id.indexOf(TargetChildControl, 0) >= 0 && Inputs[n].checked) {
                  return confirm('Would you like to delete the selected record?');
                  return true;
               }
        alert('Select at least one record!');
                return false;

          }

        function CheckAll(cbSelectAll) {
            if (TargetBaseControl == null) return false;
            var TargetChildControl = "chkDelete";
            var TimeCon = document.getElementById('<%= Me.ClassUpdatePnl.ClientID %>');

            var Inputs = TargetBaseControl.getElementsByTagName("input");

            for (var n = 0; n < Inputs.length; ++n)
                if (Inputs[n].type == 'checkbox' && Inputs[n].id.indexOf(TargetChildControl, 0) >= 0) {
                Inputs[n].checked = cbSelectAll.checked;
            }

        }


      

        var prm = Sys.WebForms.PageRequestManager.getInstance();

        prm.add_initializeRequest(InitializeRequest);
        prm.add_endRequest(EndRequest);
        var postBackElement;
        function InitializeRequest(sender, args) {

            if (prm.get_isInAsyncPostBack())
                args.set_cancel(true);
            postBackElement = args.get_postBackElement();

            var Filter = /ddFilterBy/
            var AddString = postBackElement.id.search(Filter);
            if (AddString == -1) {
                var myRegExp = /_btnUpdate/;
                var myRegExp1 = /btnSave/
                var AddString = postBackElement.id.search(myRegExp);
                var EditString = postBackElement.id.search(myRegExp1);
                if (AddString != -1 || EditString != -1) {
                   
                }
                else {
                    $get('<%= Me.UpdateProgress1.ClientID %>').style.display = 'block';
                }
                postBackElement.disabled = true;
            }
        }


        function EndRequest(sender, args) {
            var Filter = /ddFilterBy/
            var AddString = postBackElement.id.search(Filter);
            if (AddString == -1) {
                var myRegExp = /_btnUpdate/;
                var myRegExp1 = /btnSave/
                var AddString = postBackElement.id.search(myRegExp);
                var EditString = postBackElement.id.search(myRegExp1);
                var myRegExp2 = /btnCancel/
                var cancelString = postBackElement.id.search(myRegExp2);
                if (AddString != -1 || EditString != -1) {
                    
                }
                else {
                    $get('<%= Me.UpdateProgress1.ClientID %>').style.display = 'none';
                }
                postBackElement.disabled = false;
                if (cancelString != -1) {
                    HideRadWindow();
                }
            }
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
    <script type="text/javascript">
        $(window).resize(function () {
            var win = $find('<%= MPEDetails.ClientID %>');
            if (win) {
                if (!win.isClosed()) {
                    win.center();
                }
            }

        });
    </script>
    </asp:Content>
     <asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

     <h4>Geolocation Management</h4>

         <telerik:RadAjaxLoadingPanel  ID="AjaxLoadingPanel1" runat="server" >
             
             

<div ID="ProgressPanel" class ="overlay"  >
<img src="../assets/img/ajax-loader.gif" alt="Processing..."  />
</div>
</telerik:RadAjaxLoadingPanel>  

        <asp:Panel ID="Panel1" runat="server">      </asp:Panel> 
                         
 
    
                                         
                     <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
                         
                         
                         

         </telerik:RadWindowManager> 
            

                            <asp:UpdatePanel ID="TopPanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="row">
	                                    <div class="col-sm-4">
		                                    <div class="form-group">
                                                <label>Organization</label>
                                                <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddlOrganization"  Width ="100%"
                    runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" >
                </telerik:RadComboBox>
                                               </div>
                                            </div>
                                        </div>

                                    <label>Filter By</label>
                                    <div class="row">
	                                    <div class="col-sm-4">
		                                    <div class="form-group">
                                                <telerik:RadComboBox Skin="Simple"  ID="ddFilterBy" Width="100%" Height="250px" TabIndex="2" runat="server">
                                                 <Items>

                                                    <telerik:RadComboBoxItem Selected="True"  Value="0" Text="Select Filter"></telerik:RadComboBoxItem>
                                                    <telerik:RadComboBoxItem Value="Customer_No" Text="Customer No"></telerik:RadComboBoxItem>
                                                    <telerik:RadComboBoxItem Value="Customer_Name" Text="Customer Name"></telerik:RadComboBoxItem>
                                                   <telerik:RadComboBoxItem Value="Address" Text="Address"></telerik:RadComboBoxItem>
                                                 </Items>
                                                 </telerik:RadComboBox> 
                                            </div>
                                        </div>
                                        <div class="col-sm-4">
		                                    <div class="form-group">
                                                <telerik:RadTextBox  runat="server" ID="txtFilterVal" EmptyMessage="Enter Filter Value"  Width="100%"></telerik:RadTextBox>
                                            </div>
                                        </div>
                                        <div class="col-sm-4">
		                                    <div class="form-group">
                                                <asp:Button ID="btnFilter" runat="server" CausesValidation="False"  CssClass ="btn btn-primary"
                                                    OnClick="btnFilter_Click" TabIndex="4" Text="Filter" />
                                                    
                                                  <asp:Button ID="btnReset" runat="server" CausesValidation="False"  CssClass ="btn btn-default" 
                                                    TabIndex="5" Text="Reset" />
                                                    
                                                <asp:Button ID="btnAdd" runat="server" CausesValidation="false"  CssClass="btn btn-primary2" 
                                                    OnClick="btnAdd_Click" TabIndex="1" Text="Import" />
                                            </div>
                                        </div>
                                    </div>
                                    
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        <div class="table-responsive">
                            <asp:UpdatePanel ID="ClassUpdatePnl" runat="server" UpdateMode="conditional">
                                <ContentTemplate>
                                  
                                                <asp:GridView Width="100%" ID="gvLatitude" runat="server" EmptyDataText="No records to Display"
                                                    EmptyDataRowStyle-Font-Bold="true"  AutoGenerateColumns="False"
                                                    AllowPaging="True" AllowSorting="true"  PageSize="25" CellPadding="0" CellSpacing="0" CssClass="tablecellalign">
                                                   
                                                   
                                                    <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
                                                    <Columns>
                                                        <asp:TemplateField>
                                                           <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                                                                           
                                                                <asp:ImageButton ID="btnEdit" ToolTip="Edit Geolocation Data" runat="server" CausesValidation="false"
                                                                       ImageUrl="~/images/edit-13.png"    OnClick="btnEdit_Click" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Customer_No" HeaderText="Customer No" SortExpression="Customer_No">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="CustName" HeaderText="Customer Name" SortExpression="CustName">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        
                                                         <asp:BoundField DataField="Address" HeaderText="Address" SortExpression="Address">
                                                          
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="CustLat" DataFormatString="{0:N6}" HeaderText="Latitude"
                                                            SortExpression="CustLat">
                                                            <ItemStyle Wrap="False" HorizontalAlign ="Left" />
                                                               <HeaderStyle HorizontalAlign ="Left" />
                                                        </asp:BoundField>
                                                         <asp:BoundField DataField="CustLong" DataFormatString="{0:N6}" HeaderText="Longitude"
                                                            SortExpression="CustLong" >
                                                            <ItemStyle Wrap="False" HorizontalAlign ="Left" />
                                                            <HeaderStyle HorizontalAlign ="Left" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCusId" runat="server" Text='<%# Bind("Customer_ID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSiteId" runat="server" Text='<%# Bind("Site_Use_ID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false"  
                                                                HeaderText="Set" >
                                                             <ItemTemplate>
                                                                <asp:LinkButton ID="Lnk_RefID" runat="server" Text='Set Location' ForeColor="SteelBlue" Font-Underline="true"   OnClick="btnSetLocation_Click"   ></asp:LinkButton>
                                                            
                                                                
                                                                 
                                                                  </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                   <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
                                                </asp:GridView>
                                      
                                  
                                        <telerik:RadWindow ID="MPEDetails" Title= "Edit Geolocation Data" runat="server" Skin="Windows7" Behaviors="Move,Close"
                                                AutoSize="true" ReloadOnShow="false" VisibleStatusbar="false" Overlay="true" Modal="true">
                                              <ContentTemplate>

                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="conditional">
                                <ContentTemplate>  
                                    <div class="popupcontentblk"> 
                                      <p><asp:Label ID="lblMessage"  runat ="server" ForeColor ="Red" ></asp:Label></p>                             
                                        <asp:HiddenField ID="HidVal" runat="server" Value="-1" />
                                        <asp:HiddenField ID="hidUseId" runat="server" Value="-1" />
                                        <div class="row">
		                                    <div class="col-sm-5">
			                                    <label>Latitude</label>
		                                    </div>
		                                    <div class="col-sm-7">
			                                    <div class="form-group">
                                                    <asp:TextBox ID="txtLatitude" runat="server" TabIndex="1" CssClass="inputSM" Width="100%"></asp:TextBox>
			                                    </div>
		                                    </div>
	                                    </div>
                                        <div class="row">
		                                    <div class="col-sm-5">
			                                    <label>Longitude</label>
		                                    </div>
		                                    <div class="col-sm-7">
			                                    <div class="form-group">
                                                    <asp:TextBox  ID="txtLongitude" TabIndex="2" CssClass="inputSM" runat="server" Width="100%"></asp:TextBox>
                                                    <%--<asp:RequiredFieldValidator Display="None" Text=" " ControlToValidate="txtDescription"
                                                    ID="ReqDescription" runat="server" ErrorMessage="Description Required"></asp:RequiredFieldValidator>--%>
			                                    </div>
		                                    </div>
	                                    </div>
                                        <div class="row">
		                                    <div class="col-sm-5"></div>
		                                    <div class="col-sm-7">
			                                    <div class="form-group">
                                                    <asp:Button ID="btnUpdate"  CssClass ="btn btn-success" TabIndex="3"  OnClick="btnUpdate_Click"
                                                        runat="server" Text="Update"  />
                                                    <asp:Button ID="btnCancel"  CssClass ="btn btn-default"  TabIndex="4"  
                                                        runat="server" CausesValidation="false" Text="Cancel" OnClick="btnCancel_Click"/>
			                                    </div>
		                                    </div>
	                                    </div>
                                            <asp:Panel ID="Panel12" runat="server" Style="display: none" CssClass="overlay">
                                                <img alt="Processing..." src="../assets/img/ajax-loader.gif"  />
                                                <span>Processing... </span>
                                            </asp:Panel>
                                        </div>
                                    </ContentTemplate>
                            </asp:UpdatePanel>
                                              </ContentTemplate>
                                                    </telerik:RadWindow> 
                                
                                 <telerik:RadWindow ID="MapWindow" Title ="Edit Geolocation Data" runat="server" Skin="Windows7" Behaviors="Move,Close"
                                                AutoSize="true" ReloadOnShow="false" VisibleStatusbar="false" Overlay="true" Modal="true" Width="1000px" Height="500px" >
               <ContentTemplate>
             
               <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
        <ContentTemplate >
         
       
            <p><asp:Label ID="lbl_msg"  runat ="server" ForeColor ="Red" ></asp:Label></p>  
              <asp:HiddenField ID="CustID" runat="server" Value="-1" />
              <asp:HiddenField ID="SiteID" runat="server" Value="-1" />
             <asp:HiddenField ID="HLat" runat="server" Value="-1" />
              <asp:HiddenField ID="HLong" runat="server" Value="-1" />
                <div class="col-xs-12">
                <div class="row" >
                     <div class="col-sm-4">
                        <div class="form-group">
                            <label>Latitude</label>

                            <telerik:RadNumericTextBox runat="server" ID="txtLoc_Latitude" Skin="Sunset" 
                                                 TabIndex ="1" IncrementSettings-InterceptMouseWheel="false"
                                                                IncrementSettings-InterceptArrowKeys="false" Width="100%"
                                                 MinValue="0" autocomplete="off" NumberFormat-DecimalDigits="4">
                                            </telerik:RadNumericTextBox>
                        </div>
                    </div>
                    <div class="col-sm-4">
                        <div class="form-group">
                            <label>Longitude</label>

                             <asp:HiddenField ID="hd_last_Lat" runat="server" Value="-1" />
                                        <asp:HiddenField ID="hd_last_Long" runat="server" Value="-1" />
                          
                            <telerik:RadNumericTextBox runat="server" ID="txtLoc_Long" Skin="Sunset" 
                                                 TabIndex ="1" IncrementSettings-InterceptMouseWheel="false"
                                                                IncrementSettings-InterceptArrowKeys="false" Width="100%"
                                                 MinValue="0" autocomplete="off" NumberFormat-DecimalDigits="4">
                                            </telerik:RadNumericTextBox>
                        </div>
                    </div>
                    <div class="col-sm-4">
                        <div class="form-group">
                             <label>&nbsp;</label>
                            <asp:Button   ID="btnUpdateLoc" ValidationGroup="valsum" runat="server" causesValidation="false"
                                     Text="Update"  CssClass="btn btn-success" OnClick="btnSet_Click" />
                            <asp:Button   ID="btnUpdateLastVisit" ValidationGroup="valsum" runat="server" causesValidation="false"
                                     Text="LastVisit"  CssClass="btn btn-success" OnClick ="btnUpdateLastVisit_Click"  />
                                                         <asp:Button   ID="btnCancelLoc" runat="server" causesValidation="false"
                                     Text="Cancel" CssClass ="btn btn-default" OnClick="btnCancelLoc_Click"  />
                        </div>
                    </div>
                </div>
                    <div class="row" >
                        <div class="col-xs-12">
                            <div style="height:500px;width:800px;">
                                  <div id="tips" runat="server">
                                    <img src="http://maps.google.com/mapfiles/ms/icons/red.png" /><span> Current Location</span>
                                    <img src="http://maps.google.com/mapfiles/ms/icons/green.png" /><span> Last Visited Location</span>
                                   </div> 
                                <div id="map_canvas" style="height:100%;width:100%;"></div>
                            </div> 
                        </div>
                    </div> 
                </div>
            
                
  </ContentTemplate>
                <Triggers>
                  
                
                 <asp:AsyncPostBackTrigger ControlID="btnUpdateLoc" EventName="Click" />
                 <asp:AsyncPostBackTrigger ControlID="btnCancelLoc" EventName="Click" />
                </Triggers> 
                 
                  </asp:UpdatePanel>
                  
                  
              
                </ContentTemplate>
                
          </telerik:RadWindow>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                     </div>  
                <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="ClassUpdatePnl"
                    runat="server">
                    <ProgressTemplate>
                        <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                            <img src="../assets/img/ajax-loader.gif" alt="Processing..." />
                            <span>Processing... </span>
                        </asp:Panel>
                    </ProgressTemplate>
                </asp:UpdateProgress>
               
          
</asp:Content>
