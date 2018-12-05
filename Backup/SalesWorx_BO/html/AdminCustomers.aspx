<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master" CodeBehind="AdminCustomers.aspx.vb" Inherits="SalesWorx_BO.AdminCustomers" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

     
     

    <style type="text/css">
        .style1
        {
            color: #000000;
            text-decoration: none;
            font-weight: bold;
            font-style: normal;
            font-variant: normal;
            font-size: 12px;
            line-height: normal;
            font-family: Calibri;
            height: 37px;
        }
        .style2
        {
            height: 37px;
        }
        #ctl00_ContentPlaceHolder1_Panel{
        	margin: 15px;
            padding: 10px;
            background: #fff;
        }
        .style3
        {
            color: #000000;
            text-decoration: none;
            font-weight: bold;
            font-style: normal;
            font-variant: normal;
            font-size: 12px;
            line-height: normal;
            font-family: Calibri;
            width: 67px;
        }
    </style>
    <script type="text/javascript" src="http://maps.google.com/maps/api/js?sensor=false"></script>
    <script >
       
        function SetLocation() {
            var txtLatSrc = $find('<%= txtLoc_Latitude.ClientID %>');
            var txtLngSrc = $find('<%= txtLoc_Long.ClientID %>');
            $("#ctl00_ContentPlaceHolder1_MapWindow_C_txt_ShipLat").val(txtLatSrc.get_value())
            longit = $("#ctl00_ContentPlaceHolder1_MapWindow_C_txt_ShipLong").val(txtLngSrc.get_value())
            $("#ctl00_ContentPlaceHolder1_MapWindow_C_details").css('display', 'block')
            $("#ctl00_ContentPlaceHolder1_MapWindow_C_map").css('display', 'none')
            return false
        }
        
        function showmap() {
          
            $("#ctl00_ContentPlaceHolder1_MapWindow_C_details").css('display', 'none')
           
            $("#ctl00_ContentPlaceHolder1_MapWindow_C_map").css('display','block')
            initialize()
            return false 
        }
        function HideMap() {
            
            $("#ctl00_ContentPlaceHolder1_MapWindow_C_details").css('display', 'block')
            $("#ctl00_ContentPlaceHolder1_MapWindow_C_map").css('display', 'none')
            return false
        }
        function checkconfirm(status) {
            if (status == "Y")
              return confirm("Are you sure to disable this ship address?")
            else
                return confirm("Are you sure to enable this ship address?")
        }
        
        function CloseWindow() {
            $find("<%= MapWindow.ClientID%>").close()
        }
        function NumericOnly(e) {

            var keycode;

            if (window.event) {
                keycode = window.event.keyCode;
            } else if (e) {
                keycode = e.which;
            }
            if ((parseInt(keycode) >= 48 && parseInt(keycode) <= 57) || parseInt(keycode) == 8 ||   parseInt(keycode) == 46 || parseInt(keycode) == 0)
                return true;

            return false;
        }
        function IntegerOnly(e) {

            var keycode;

            if (window.event) {
                keycode = window.event.keyCode;
            } else if (e) {
                keycode = e.which;
            }
            
            if ((parseInt(keycode) >= 48 && parseInt(keycode) <= 57) || parseInt(keycode) == 8 || parseInt(keycode) == 0)
                return true;

            return false;
        }


        function initialize() {
            var map;
            var location;


            var myOptions;
            var markersArray = [];
            var lati,longit
            lati = $("#ctl00_ContentPlaceHolder1_MapWindow_C_txt_ShipLat").val()
            longit = $("#ctl00_ContentPlaceHolder1_MapWindow_C_txt_ShipLong").val() 
            if(lati=="")
            {
            lati=25.000000
            }
            
            if(longit=="")
            {
            longit=55.000000
        }

        
        var txtLat = $find('<%= txtLoc_Latitude.ClientID %>');
        txtLat.set_value(lati);
        var txtLng = $find('<%= txtLoc_Long.ClientID %>');
        txtLng.set_value(longit);
            var myLatlng = new google.maps.LatLng(lati,longit );
            myOptions = {
                    zoom: 8,
                    center: myLatlng,
                    mapTypeId: google.maps.MapTypeId.ROADMAP
                }
                map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);
   
                        var image = "http://maps.google.com/mapfiles/ms/icons/red.png";
                        location = new google.maps.LatLng(lati, longit)
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
                google.maps.event.addListener(map, "click", function(event) {
                    var lat = event.latLng.lat();
                    var lng = event.latLng.lng();

                    clearOverlays();


                    var txtLat = $find('<%= txtLoc_Latitude.ClientID %>');
                    txtLat.set_value(lat);
                    var txtLng = $find('<%= txtLoc_Long.ClientID %>');
                    txtLng.set_value(lng);
                    // populate yor box/field with lat, lng
                    // alert("Lat=" + txtLat.get_value() + "; Lng=" + txtLng.get_value());
                    location = new google.maps.LatLng(event.latLng.lat(), event.latLng.lng())
                    marker = new google.maps.Marker({
                        position: location,
                        map: map
                    });
                    markersArray.push(marker);
                });

                google.maps.event.addListenerOnce(map, 'idle', function() {
                    google.maps.event.trigger(map, 'resize');
                });
            }
        


               
    </script>
   
   

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   
    <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
	<tr>
	
	<td id="contentofpage" width="100%" height="76%" class="topshadow">
		<div class="pgtitileposition">
	        <span class="pgtitile3">Customer Management</span>
	</div> 
	<telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true">   </telerik:RadWindowManager>
	<table width="auto" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
	<tr>
<td style="padding:6px 12px">
 
                        
       
  <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
              
         <telerik:RadWindow ID="MapWindow" Title ="Customer Ship Address" runat="server"  Behaviors="Move,Close" 
         width="850px" height="500px"  ReloadOnShow="false"  Modal ="true"  VisibleStatusbar="false"  Overlay="true"  OnClientActivate="EnableVanlist" >
               <ContentTemplate>
             
               <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
        <ContentTemplate >
         
         <asp:Panel runat="server" id="details">
               <table  cellpadding ="4" cellspacing ="4">
                
                 <tr>
               <td class ="txtSMBold" >Customer No *</td>
               <td>
                   <asp:TextBox ID="Txt_ShipCustNo" runat="server" CssClass="inputSM" 
                       MaxLength="100" ReadOnly="true" TabIndex="2"></asp:TextBox>
                     </td>
                <td class ="txtSMBold" >Customer Name *</td>
               <td>
                   <asp:TextBox ID="Txt_ShipCustName" runat="server" CssClass="inputSM" 
                       MaxLength="100" TabIndex="2" Width="250px"></asp:TextBox>
                     </td>
               </tr> 
               
                <tr>
                
               <td class ="txtSMBold" >Location</td>
               
                <td >
                
                    <asp:TextBox ID="Txt_ShipLocation" runat="server" CssClass="inputSM" 
                        MaxLength="100" TabIndex="2" Width="250px"></asp:TextBox>
                          
            </td>
              
               
                    <td class="txtSMBold">
                        Address</td>
                    <td>
                        <asp:TextBox ID="Txt_ShipAddress" runat="server" CssClass="inputSM" 
                            MaxLength="100" TabIndex="2" Width="250px"></asp:TextBox>
                    </td>
              
               
               </tr> 
               
               
                  
               
               
                  <tr>
               
                               <td class="txtSMBold"> City</td>
                               
                               <td>
                                   <asp:TextBox ID="Txt_ShipCity" runat="server" CssClass="inputSM" 
                                       MaxLength="100" TabIndex="2"></asp:TextBox>
                               </td>
                               <td class="txtSMBold">
                                   Postal Code</td>
                               <td>
                                   <asp:TextBox ID="Txt_ShipPO" runat="server" CssClass="inputSM" MaxLength="100" 
                                       TabIndex="2"></asp:TextBox>
                               </td>
                               
               </tr> 
             
               <tr>
               <td class ="txtSMBold" >Customer Segment </td>
               <td>
                   <asp:DropDownList ID="ddl_Segment" runat="server" CssClass="inputSM">
                   </asp:DropDownList>
                   </td>
                 <td class ="txtSMBold" >Sales District</td>
               <td>
                   <asp:DropDownList ID="ddl_SalesDistrict" runat="server" CssClass="inputSM">
                   </asp:DropDownList>
                   </td>
               </tr> 
               
                  <tr>
               <td class ="txtSMBold" >Latitude</td>
               <td><asp:TextBox ID="txt_ShipLat" runat="server" CssClass="inputSM" MaxLength="100" 
                          TabIndex="2" onKeypress='return NumericOnly(event)' ></asp:TextBox></td>
                <td class ="txtSMBold" >Longitude</td>
               <td><asp:TextBox ID="txt_ShipLong" runat="server" CssClass="inputSM" MaxLength="100" 
                          TabIndex="2" onKeypress='return NumericOnly(event)' ></asp:TextBox>
                          <asp:LinkButton ID="LnksetLocation" Text="Set Location" runat="server" ></asp:LinkButton>
                          </td>
               </tr> 
               
                
               
                 <tr>
                 <td colspan="4">
                  <table id="VanlistTokenBox" runat="server" visible="false">
                  <tr>
                  <td class ="txtSMBold" style="width:100px" >
                       <asp:Label ID="lable1" runat="server" Visible="false">Vans</asp:Label>
                     </td>
               <td class="txtSMBold" colspan="3">
                    Select the Van
                  <%-- <ati:ASPTokenInput ID="VanList" runat="server" 
                        PreventDuplicates="true" 
                       Theme="facebook"  />--%>
                       
                       <asp:Panel ID="Panel1" runat="server" Height="143px" ScrollBars="Auto" BorderStyle="Groove"
                                        BorderWidth="1px" Visible="true" Width="514px">
                                        <asp:CheckBoxList ID="VanList" runat="server" RepeatColumns="3" 
                                            Visible="true" Font-Bold="False" CellPadding="2" CellSpacing="4">
                                        </asp:CheckBoxList>
                        </asp:Panel>
                 
                     </td>
                     </tr>
                  </table>
                  </td>
                   </tr>
              
              
                  <tr>
                 <td colspan="4" >
                    <asp:HiddenField ID="HVanList" runat="server" />
                     <asp:Button ID="btnSaveship" runat="server" CssClass="btnInputGreen" 
                         OnClick="btnSaveship_Click" Text="Save" />
                     <asp:Button ID="btnCancelship" runat="server" CssClass="btnInputGrey" 
                         OnClientClick="javascript:CloseWindow()" Text="Cancel" />
                      </td>
               </tr> 
             
               </table>
            
          </asp:Panel>        
           
        <asp:Panel id="map" runat="server"  style="display:none" >
                  <table>
                  <tr><td>Latitude:</td><td><telerik:RadNumericTextBox runat="server" ID="txtLoc_Latitude" Skin="Sunset" 
                                                 TabIndex ="1" IncrementSettings-InterceptMouseWheel="false"
                                                                IncrementSettings-InterceptArrowKeys="false" Width="70px"
                                                Font-Names="Monda" Font-Size="13px" MinValue="0" autocomplete="off" NumberFormat-DecimalDigits="4">
                                            </telerik:RadNumericTextBox></td><td>Longitude:</td><td><telerik:RadNumericTextBox runat="server" ID="txtLoc_Long" Skin="Sunset" 
                                                 TabIndex ="1" IncrementSettings-InterceptMouseWheel="false"
                                                                IncrementSettings-InterceptArrowKeys="false" Width="70px"
                                                Font-Names="Monda" Font-Size="13px" MinValue="0" autocomplete="off" NumberFormat-DecimalDigits="4">
                                            </telerik:RadNumericTextBox>
                                             <asp:Button   ID="btnUpdateLoc" ValidationGroup="valsum" runat="server" causesValidation="false"
                                   OnClientClick="javascript:SetLocation()"  Text="Update"  />
                                                         <asp:Button   ID="btnCancelLoc" runat="server" causesValidation="false"
                                  OnClientClick="javascript:HideMap()"  Text="Cancel"  />
                                            </td></tr>
                  </table>
                <div id="map_canvas" style="height:440px;"> </div>
                
                </asp:Panel>
               
  </ContentTemplate>
                <Triggers>
                 <asp:AsyncPostBackTrigger ControlID="ddlOrganization" EventName="SelectedIndexChanged" />
                 <asp:AsyncPostBackTrigger ControlID="btnSaveship" EventName="Click" />
                 <asp:AsyncPostBackTrigger ControlID="LnksetLocation" EventName="Click" />
                 <asp:AsyncPostBackTrigger ControlID="btnUpdateLoc" EventName="Click" />
                 <asp:AsyncPostBackTrigger ControlID="btnCancelLoc" EventName="Click" />
                </Triggers> 
                 
                  </asp:UpdatePanel>
                  
                  
              
                </ContentTemplate>
                
          </telerik:RadWindow>
<table width="100%" border="0" cellspacing="2" cellpadding="2">
          
          <tr>
          <td>
          <table cellspacing="3" cellpadding="3" >
          
             <tr><td class="txtSMBold">Organization *</td><td> 
              <asp:DropDownList CssClass="inputSM" ID="ddlOrganization" 
                    runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" 
                     TabIndex="1" AutoPostBack="true">
                </asp:DropDownList> 
                 </td>
              
                 <td class="txtSMBold"> &nbsp;</td>
             <td> 
                 &nbsp;</td>
             <td class="style3"> &nbsp;</td>
             <td> 
                 &nbsp;</td>
             
              </tr>
              
              
              <tr>
                  <td class="txtSMBold">
                      Customer Name * </td>
                  <td>
                      <asp:TextBox ID="txt_CusName" runat="server" CssClass="inputSM" MaxLength="150" 
                          TabIndex="2"  Width ="250px"></asp:TextBox>
                     
                  </td>
                  <td class="txtSMBold">
                      Customer No *</td>
                  <td>
                      <asp:TextBox ID="txt_CusNo" runat="server" CssClass="inputSM" MaxLength="10" 
                          TabIndex="3"  Width ="170px"></asp:TextBox>
                  </td>
                  <td class="style3">
                      Contact:</td>
                  <td>
                      <asp:TextBox ID="txt_Contact" runat="server" CssClass="inputSM" MaxLength="120" 
                          TabIndex="4"></asp:TextBox>
                  </td>
              </tr>
              
             
              
              
              <tr><td class="txtSMBold">Cash Customer</td><td> 
                  <asp:RadioButtonList ID="Rdo_CashCust" runat="server" AutoPostBack="true"
                      RepeatDirection="Horizontal">
                      <asp:ListItem Selected="True" Value="Y">Yes</asp:ListItem>
                      <asp:ListItem Value="N">No</asp:ListItem>
                  </asp:RadioButtonList>
              </td>
              
                 <td class="txtSMBold"> Credit Limit<span id="reqCr2" runat="server">*</span></td>
             <td> 
              <asp:TextBox ID="txt_CreditLimit" runat="server"  CssClass="inputSM" MaxLength="7" Enabled="false"  Width ="170px" onKeypress='return NumericOnly(event)'></asp:TextBox>
              </td>
              <td class="style3"> Credit Period <span id="reqCr1" runat="server">*</span></td>
             <td> 
              <asp:TextBox ID="txt_CreditPeriod" runat="server" Width="65px" CssClass="inputSM" MaxLength="8" Enabled="false" onKeypress='return IntegerOnly(event)'></asp:TextBox>
                 <span class="txtSMBold" >(Days)</span></td>
              </tr>
               <tr><td class="txtSMBold">Location</td><td> 
              <asp:TextBox ID="txt_CustLocation" runat="server" Width ="250px" CssClass="inputSM" MaxLength="40" ></asp:TextBox>
              </td>
              
                 <td class="txtSMBold"> City</td>
             <td> 
              <asp:TextBox ID="txt_CustCity" runat="server" CssClass="inputSM" MaxLength="60" Width ="170px"></asp:TextBox>
              </td>
              <td class="style3"> Phone</td>
             <td> 
              <asp:TextBox ID="txt_phone" runat="server" CssClass="inputSM"></asp:TextBox>
              </td>
              </tr>
              
               <tr><td class="txtSMBold">Address</td><td> 
              <asp:TextBox ID="txt_Custaddress" runat="server" Width ="250px" CssClass="inputSM" MaxLength="240" ></asp:TextBox>
              </td>
              
                 <td class="txtSMBold" id="tdpr1" runat="server"> Price List *</td>
             <td id="tdpr2" runat="server"> 
                 <asp:DropDownList ID="ddl_Pricelist" runat="server" CssClass="inputSM"  Width ="170px">
                 </asp:DropDownList>
              </td>
              <td class="style3"> &nbsp;</td>
             <td> 
              <asp:TextBox ID="txt_availBalance" runat="server" CssClass="inputSM" onKeypress='return NumericOnly(event)' Width="65px" Visible="false"></asp:TextBox>
              </td>
              </tr>
              <tr>
                  <td class="txtSMBold">
                      Credit Hold</td>
                  <td class="style2">
                      <asp:RadioButtonList ID="rdo_CreditHold" runat="server" 
                          RepeatDirection="Horizontal">
                          <asp:ListItem Value="Y">Yes</asp:ListItem>
                          <asp:ListItem Value="N" Selected="True" >No</asp:ListItem>
                      </asp:RadioButtonList>
                  </td>
                  <td class="style1" colspan="4">
                      
                      </td>
                      
              </tr>
              <tr>
                  <td class="style1">
                      &nbsp;</td>
                  <td class="style2">
                      &nbsp;</td>
                  <td class="style1" colspan="4">
                      <asp:Button ID="BtnAdd" runat="server" CssClass="btnInputBlue" Text="Save" />
                      <asp:Button ID="Btncancel" runat="server" CssClass="btnInputGrey" 
                          Text="Cancel" />
                      <asp:Button ID="BtnAddShip" runat="server" CssClass="btnInputBlue" 
                          Text="Add Shipping Location" Visible="false" />
                  </td>
              </tr>
          </table>
          <asp:HiddenField ID="opt" Value="1" runat="server" /> <asp:HiddenField ID="FSR_CUST_REL" Value="" runat="server" />
                <asp:HiddenField ID="Customer_ID" Value="" runat="server" />
                <asp:HiddenField ID="SiteUse_ID" Value="" runat="server" />
              <asp:HiddenField ID="SiteUse_IDShip" runat="server" Value="" />
               <asp:HiddenField ID="OptShip" runat="server" Value="" />
                 <asp:HiddenField ID="ShipCount" runat="server" Value="0" />
          </td>
          </tr>
       
        </table>
       
	
 
  </ContentTemplate>
                 
                  </asp:UpdatePanel>
        
                  <asp:UpdateProgress ID="UpdateProgress1"   AssociatedUpdatePanelID="UpdatePanel1" runat="server" DisplayAfter="10">
     <ProgressTemplate>
     <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
     
         <img src="../images/Progress.gif" alt="Processing..." style="vertical-align:middle;" />            
           <span style="font-size:12px; font-weight:700px; color:#3399ff;">Processing... </span>
       </asp:Panel>
           </div>
       
         
    </ProgressTemplate>
            </asp:UpdateProgress>  
                    
</td>
</tr>
	<tr>
       <td>
        <asp:UpdatePanel ID="Panel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
              <table style="display:none">
              
              <tr>
              <td class="txtSMBold">Customer name:</td> <td>
                  <asp:TextBox ID="txt_filterName" runat="server"  CssClass="inputSM"></asp:TextBox>
                  </td> 
                  
                     <td class="txtSMBold"> Customer no:
                      </td>
                      <td>
                          <asp:TextBox ID="txt_filterNo" runat="server"  CssClass="inputSM"></asp:TextBox>
                          <asp:Button ID="BtnFilter" runat="server" CssClass="btnInputBlue" Text="Filter" />
                          <asp:Button ID="BtnClearFilter" runat="server" CssClass="btnInputGrey" Text="Clear Filter" />
                      </td>
                  
              </tr>
              </table>
              <table style="width:100%">
               <tr>
              <td class="txtSMBold">Customer Ship Addresses:</td> 
                  
              </tr>
              <tr>
              <td  style="width:100%">
              <asp:GridView Width="100%"  ID="GVShipAddress" runat="server" EmptyDataText="No records to display" EmptyDataRowStyle-Font-Bold="true" 
                 AutoGenerateColumns="False" AllowPaging="True" AllowSorting="true"  DataKeyNames ="Site_Use_ID"
                  PageSize="25" CellPadding="0" CellSpacing="0" CssClass="tablecellalign">
                  
                  <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
                  <Columns>
                  <asp:TemplateField>
                  <ItemTemplate>
                    <%--This is a placeholder for the details GridView--%>
                  </ItemTemplate> 
                </asp:TemplateField>
                 
                   
                    
                    <asp:BoundField DataField="Customer_Name" HeaderText="Name"  SortExpression="Customer_Name"   NullDisplayText="N/A" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"  >
                      <ItemStyle Wrap="False" />
                    </asp:BoundField>
                    
                     <asp:BoundField DataField="City" HeaderText="City"  SortExpression="City"   NullDisplayText="N/A" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"  >
                      <ItemStyle Wrap="False" />
                    </asp:BoundField>
                     
                     
                    <asp:BoundField DataField="Cust_Lat" HeaderText="Latitude"  SortExpression="Cust_Lat" NullDisplayText="N/A" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                      <ItemStyle Wrap="False" />
                    </asp:BoundField> 
                    <asp:BoundField DataField="Cust_Long" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="Longitude" SortExpression="Cust_Long">
                      
                     </asp:BoundField>
                     <asp:BoundField DataField="CustStatus" SortExpression="CustStatus"  HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"  HeaderText="Status" >
                       
                     </asp:BoundField>
                   
                   <asp:TemplateField HeaderText=""  HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false">
                        <ItemTemplate>
                      
                                                 <asp:LinkButton  runat="server" ID="lbEdit"  Text="Edit" 
                                               
                                                   OnClick="lbEdit_Click"
                                                 
                                                    ></asp:LinkButton>
                                                      
                        </ItemTemplate>
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText=""  HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false">
                        <ItemTemplate>
                      
                                                  <asp:LinkButton  runat="server" ID="lbChangeStatus"  Text='<%# Bind("Action") %>'  OnClientClick=<%# "javascript:checkconfirm('" + Eval("Cust_Status") + "');" %> OnClick ="lbChangeStatus_Click"></asp:LinkButton>
                                                    <asp:Label ID="lblStatus" runat ="server" Visible ="false"  Text='<%# Bind("Cust_Status") %>'></asp:Label>
                                                    <asp:Label ID="lblCustomer_ID" runat ="server" Visible ="false"  Text='<%# Bind("Customer_ID") %>'></asp:Label>
                                                    <asp:Label ID="lblSite_Use_ID_Ship" runat ="server" Visible ="false"  Text='<%# Bind("Site_Use_ID") %>'></asp:Label><asp:Label ID="lbETime" runat ="server" Visible ="false"  Text='<%# Bind("Customer_No") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                                    
                                                    </Columns><PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
              </asp:GridView></td>
              </tr>
              </table>
              
            

         
           <asp:Button ID="btnHidden" runat="Server" Style="display: none" />
                                                <ajaxtoolkit:modalpopupextender ID="MpInfoError" runat="server" BackgroundCssClass="modalBackground"
                                                    TargetControlID="btnHidden" PopupControlID="pnlConfirm" CancelControlID="btnClose"
                                                    Drag="true" />
                                                <asp:Panel ID="pnlConfirm" Width="400" runat="server" CssClass="modalPopup" Style="display: none">
                                                    <table id="tableinPopupErr" width="400" cellpadding="10" style="background-color: White;">
                                                        <tr align="center">
                                                            <td align="center" style="cursor: move; background-color: #3399ff; text-align: center;
                                                                border: solid 1px #3399ff; color: White; padding: 3px">
                                                                <asp:Label ID="lblinfo" runat="server" Font-Size ="13px"></asp:Label></td></tr><tr>
                                                            <td align="center" style="text-align: center">
                                                                <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="List" />
                                                                <asp:Label ID="lblMessage" runat="server" Font-Size ="13px"></asp:Label></td></tr><tr>
                                                            <td align="center" style="text-align: center;">
                                                                <asp:Button ID="btnClose" runat="server" Text="Ok" CssClass="btnInputBlue" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
          </ContentTemplate>
         
        </asp:UpdatePanel>      <asp:UpdateProgress ID="UpdateProgress2"   AssociatedUpdatePanelID="Panel" runat="server" DisplayAfter="10">
     <ProgressTemplate>
     <asp:Panel ID="Panel19" CssClass="overlay" runat="server">
     
         <img src="../images/Progress.gif" alt="Processing..." style="vertical-align:middle;" />            
           <span style="font-size:12px; font-weight:700px; color:#3399ff;">Processing... </span>
       </asp:Panel>
           </div>
       
         
    </ProgressTemplate>
            </asp:UpdateProgress>                  
       </td>              
	</tr>
  
    </table>
	
	</td> 
	</tr>
	</table>
	<script>
	    function EnableVanlist() {
//	        var FSR_CUST_REL = document.getElementById("ctl00_ContentPlaceHolder1_FSR_CUST_REL").value

//	        console.debug(FSR_CUST_REL)
//	        if (FSR_CUST_REL == 'Y') {
//	            $("#VanlistTokenBox").css("display", "block");
//	        }
//	        else {
//	            console.debug($("#VanlistTokenBox"))
//	            $("#VanlistTokenBox").css("display", "none");
//	            console.debug($("#VanlistTokenBox").css("display"))
//	        }
	    }
</script>
</asp:Content>

