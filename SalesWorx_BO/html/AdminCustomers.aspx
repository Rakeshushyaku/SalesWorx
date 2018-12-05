<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="AdminCustomers.aspx.vb" Inherits="SalesWorx_BO.AdminCustomers" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="FeaturedContent" runat="server">


     
     

    <style type="text/css">

        .popupcontentblkwider{
	padding:25px 15px 15px;
	position:relative;
	width:750px;

}
.popupcontentblkwider > p,
.popupcontenterror{
	display: block;
    font-size: 12px;
    left: 15px;
    margin: 0;
    position: absolute;
    top: 3px;
}
.popupcontentblkwider label{
	padding:5px 0 0;
}
.popupcontentblkwider .form-group {
    margin-bottom: 10px;
}
.popupcontentblkwider .form-group .inputSM input[type="radio"], 
.popupcontentblkwider .form-group .inputSM input[type="checkbox"]{
	margin-top:8px;
}
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
        
    </style>
    <script type="text/javascript" src="http://maps.google.com/maps/api/js?sensor=false&key=AIzaSyAOpIPxhcIJ4PCHkIAl5ed_eP2cfu5HYsg"></script>
    <script >
       
        function SetLocation() {
         
            var x = document.getElementById("map_div");
          
            x.style.display = "none";


            var D = document.getElementById("details_div");
            if (D.style.display === "none") {
                D.style.display = "block";
            } 


           // alert("SetLocation");
            var txtLatSrc = $find("<%= txtLoc_Latitude.ClientID %>");
            var txtLngSrc = $find("<%= txtLoc_Long.ClientID %>");
        
            
            txt_ShipLat = document.getElementById("<%= txt_ShipLat.ClientID%>");
            txt_ShipLat.value = txtLatSrc.get_value();

            txt_ShipLong = document.getElementById("<%= txt_ShipLong.ClientID%>");
            txt_ShipLong.value = txtLngSrc.get_value();

  
            $("#MainContent_MapWindow_C_details").css('display', 'block')
            $("#MainContent_MapWindow_C_map").css('display', 'none')
            return false
        }
        
        function showmap() {
                    

            var x = document.getElementById("details_div");
            x.style.display = "none";

            var D = document.getElementById("map_div");
            D.style.display = "block";
            //if (x.style.display === "none") {
            //    x.style.display = "block";
            //} else {
            //    x.style.display = "none";
            //}

            var mappanel = document.getElementById("<%=map.ClientID%>");
            mappanel.style.visibility = "visible";
            $("#MainContent_MapWindow_C_details").css('display', 'none')
           
            $("#MainContent_MapWindow_C_map").css('display', 'block')
            initialize()
            return false 
        }
        function HideMap() {
            
            $("#MainContent_MapWindow_C_details").css('display', 'block')
            $("#MainContent_MapWindow_C_map").css('display', 'none')


            var x = document.getElementById("map_div");

            x.style.display = "none";


            var D = document.getElementById("details_div");
            if (D.style.display === "none") {
                D.style.display = "block";
            }
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
            lati = $("#MainContent_MapWindow_C_txt_ShipLat").val()
            longit = $("#MainContent_MapWindow_C_txt_ShipLong").val()

           
           
            longit = $find("<%= txt_ShipLong.ClientID%>");

            var TestVar = document.getElementById('<%= txt_ShipLat.ClientID%>').value;
            lati = document.getElementById('<%= txt_ShipLat.ClientID%>').value;
            longit = document.getElementById('<%= txt_ShipLong.ClientID%>').value;


            if(lati=="")
            {
            lati=25.000000
            }
            
            if(longit=="")
            {
            longit=55.000000
        }
           // lati = 25.000000
            //   longit = 55.000000



        var txtLat = $find('<%= txtLoc_Latitude.ClientID %>');
        txtLat.set_value(lati);
        var txtLng = $find('<%= txtLoc_Long.ClientID %>');
        txtLng.set_value(longit);
        var myLatlng = new google.maps.LatLng(lati, longit);

      
        if (lati == 0.000000 & longit == 0.000000)
        {
            lati = document.getElementById('<%= HLat_Default.ClientID%>').value;
            longit = document.getElementById('<%= HLong_Default.ClientID%>').value;
            document.getElementById('<%= txtLoc_Latitude.ClientID%>').value = lati;
            document.getElementById('<%= txtLoc_Long.ClientID%>').value = longit;
        }
           


            myOptions = {
                    zoom: 3,
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

        function confirmLinkButton(button) {
            function linkButtonCallbackFn(arg) {
                if (arg) {
                    //obtains a __doPostBack() with the correct UniqueID as rendered by the framework
                    eval(button.href);

                    //can be used in a simpler environment so that event validation is not triggered.
                    //__doPostBack(button.id, "");
                }
            }
             
            
            radconfirm("Are you sure you want to enable the ship address", linkButtonCallbackFn, 330, 180, null, "Confirm");
        }

        function confirmLinkButtondis(button) {
            function linkButtonCallbackFn(arg) {
                if (arg) {
                    //obtains a __doPostBack() with the correct UniqueID as rendered by the framework
                    eval(button.href);

                    //can be used in a simpler environment so that event validation is not triggered.
                    //__doPostBack(button.id, "");
                }
            }


            radconfirm("Are you sure you want to disable the ship address?", linkButtonCallbackFn, 330, 180, null, "Confirm");
        }



               
    </script>
   
   <script type="text/javascript">
       $(window).resize(function () {
           var win = $find('<%= MapWindow.ClientID %>');
            if (win) {
                if (!win.isClosed()) {
                    win.center();
                }
            }

        });
    </script>

</asp:Content>
 <asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   
    <h4>Customer Management </h4>
	  <asp:HiddenField ID="HCust_Type" runat="server" />
	<telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true" Skin="Simple">   </telerik:RadWindowManager>

 
                        
       
  <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
              
         <telerik:RadWindow ID="MapWindow" Title ="Customer Shipping Address" runat="server"  Behaviors="Move,Close" 
         AutoSize="true"  ReloadOnShow="false"  Modal ="true"  VisibleStatusbar="false"  Overlay="true"  OnClientActivate="EnableVanlist"  Skin="Windows7" >
               <ContentTemplate>
             
               <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
        <ContentTemplate >
         <div class="popupcontentblkwider">
             <div id="details_div">
         <asp:Panel runat="server" id="details">
             <div class="row">
                                         <div class="col-sm-12">
                                            <div class="form-group">&nbsp;
                                                <asp:Label ID="lbl_ship_msg" runat="server" Text="" ForeColor="Red" ></asp:Label>
                                                </div>
                                             </div>
                 </div>
                 <div class="row">
                                         <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Customer No *</label>  
               
                   <asp:TextBox ID="Txt_ShipCustNo" runat="server" CssClass="inputSM" 
                       MaxLength="100" ReadOnly="true"  Width="100%"></asp:TextBox>
                   </div>
                                             </div>
                     
                <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Customer Name *</label>
                
                   <asp:TextBox ID="Txt_ShipCustName" runat="server" CssClass="inputSM" 
                       MaxLength="100" TabIndex="1" Width="100%"></asp:TextBox>
                     </div>
                    </div>
                     <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>
               
                 Location</label>
               
                 
                
                    <asp:TextBox ID="Txt_ShipLocation" runat="server" CssClass="inputSM" 
                        MaxLength="40" TabIndex="2" Width="100%"></asp:TextBox>
                          
           </div>
                         </div>
              
               
                    <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>
                        Address</label>
                    
                        <asp:TextBox ID="Txt_ShipAddress" runat="server" CssClass="inputSM" 
                            MaxLength="100" TabIndex="3" Width="100%"></asp:TextBox>
                    </div>
                        </div>

               
               
                  
               
               
                 <div class="col-sm-4">
                                            <div class="form-group">
                                                <label> City</label>
                               
                               
                                   <asp:TextBox ID="Txt_ShipCity" runat="server" CssClass="inputSM" 
                                       MaxLength="60" TabIndex="4" Width="100%"></asp:TextBox>
                               </div>
                     </div>
                 <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>
                                   Postal Code </label>
                               
                                   <asp:TextBox ID="Txt_ShipPO" runat="server" CssClass="inputSM" MaxLength="60" 
                                       TabIndex="5" Width="100%"></asp:TextBox>
                               </div>
                        </div>
             
               <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Customer Segment</label>
                
                   <telerik:RadComboBox Skin="Simple" Filter="Contains" ID="ddl_Segment" runat="server" CssClass="inputSM" Width="100%" TabIndex="6">
                   </telerik:RadComboBox>
                    </div>
                   </div>
               <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>
                 Sales District</label>
                 
                   <telerik:RadComboBox Skin="Simple" Filter="Contains" ID="ddl_SalesDistrict" runat="server" CssClass="inputSM" Width="100%" TabIndex="7">
                  </telerik:RadComboBox>
                    </div>
                         </div>
               
               

               <div class="col-sm-4">
                                            <div class="form-group">  <label>Beacon Major</label>
                <asp:TextBox ID="txt_BeaconMajor" runat="server" CssClass="inputSM" MaxLength="100" 
                          TabIndex="8" onKeypress='return NumericOnly(event)'  Width="100%"></asp:TextBox> 
                                                </div>
                   </div>
               <div class="col-sm-4">
                                            <div class="form-group">  <label>Beacon Minor </label>
                <asp:TextBox ID="txt_BeaconMinor" runat="server" CssClass="inputSM" MaxLength="100" 
                          TabIndex="9" onKeypress='return NumericOnly(event)'  Width="100%"></asp:TextBox>
                        
                          </div>
                   </div>

                     
               <div class="col-sm-4">
                                            <div class="form-group">  <label>Beacon UUID</label>
                <asp:TextBox ID="txt_BeaconUUID" runat="server" CssClass="inputSM" MaxLength="100" 
                          TabIndex="10"   Width="100%"></asp:TextBox> 
                                                </div>
                   </div>

               <div class="col-sm-4">
                                            <div class="form-group">  <label>Latitude</label>
                <asp:TextBox ID="txt_ShipLat" runat="server" CssClass="inputSM" MaxLength="100" 
                          TabIndex="11" onKeypress='return NumericOnly(event)'  Width="100%"></asp:TextBox> 
                                                </div>
                   </div>
               <div class="col-sm-4">
                                            <div class="form-group">  <label>Longitude </label>
                <asp:TextBox ID="txt_ShipLong" runat="server" CssClass="inputSM" MaxLength="100" 
                          TabIndex="12" onKeypress='return NumericOnly(event)'  Width="100%"></asp:TextBox>
                          <asp:LinkButton ID="LnksetLocation" Text="Set Location" runat="server" ></asp:LinkButton>
                          </div>
                   </div>

               
                </div>
                <div class="row" id="VanlistTokenBox"  runat="server" visible="false">
                  
                     <div class="col-sm-12">
                                            <div class="form-group">  <label><asp:Label ID="lable1" runat="server" Visible="false">Vans</asp:Label></label>

                  
                   
                
                       
                       
                                        <telerik:RadListBox ID="VanList" runat="server" CssClass="multiColumn"  Visible="true"   TabIndex="4"  
                                      CheckBoxes="true"  Skin ="Default" BackColor ="White"  BorderStyle ="None" BorderColor ="LightGray" BorderWidth ="1px" Width="100%" Height="140px" >
                                         </telerik:RadListBox>
                      
                 
                      
                  </div>
                         </div>
              
              
                  </div>

                 <div class="row" >
                     <div class="col-sm-12">
                        <div class="form-group">
                            <asp:HiddenField ID="HVanList" runat="server" />
                               <asp:HiddenField ID="HLat_Default" runat="server" />
                               <asp:HiddenField ID="HLong_Default" runat="server" />
                           
                     <asp:Button ID="btnSaveship" runat="server"  CssClass="btn btn-success"
                         OnClick="btnSaveship_Click" Text="Save" />
                     <asp:Button ID="btnCancelship" runat="server" CssClass ="btn btn-default"
                         OnClientClick="javascript:CloseWindow()" Text="Cancel" />
                            </div>
                         </div>
                 </div>
                    
             <div class="row">
                 <div class="col-sm-4"><br /><br />
                                            <div class="form-group"></div></div>
             </div>
          </asp:Panel>    
                 </div>    
            <div id="map_div" style="display:none;">
        <asp:Panel id="map"  runat="server" visibility="true"  style="visibility :hidden ">
              
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
                                   OnClientClick="javascript:SetLocation()"  Text="Update"  CssClass="btn btn-success" />
                                                         <asp:Button   ID="btnCancelLoc" runat="server" causesValidation="false"
                                  OnClientClick="javascript:HideMap()"  Text="Cancel" CssClass ="btn btn-default"  />
                        </div>
                    </div>
                </div>
                  
                <div id="map_canvas" style="height:440px;"> </div>
                
                </asp:Panel>
                </div>
               </div>
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
 <div class="row">
<div class="col-sm-4">
         <div class="form-group">
                  <label>  Organization *</label>
                   <telerik:RadComboBox Skin="Simple" Filter="Contains" ID="ddlOrganization" 
                     runat ="server" DataTextField="Description" DataValueField="MAS_Org_ID" 
                     TabIndex="1" AutoPostBack="true" Width="100%">
                   </telerik:RadComboBox>
          </div>
     </div>
 
     <div class="col-sm-4">
         <div class="form-group">
              <label>Customer No * </label>
              <asp:TextBox ID="txt_CusNo" runat="server" CssClass="inputSM" MaxLength="12" 
                          TabIndex="2"  Width ="100%" ></asp:TextBox>
          </div>
      </div>

     
     
     <div class="col-sm-4">
          <div class="form-group">
                <label>Customer Name * </label>
                <asp:TextBox ID="txt_CusName" runat="server" CssClass="inputSM" MaxLength="150" 
                          TabIndex="3"  Width ="100%"></asp:TextBox>
                     
           </div>
    </div></div>
             <div class="row">

    <div class="col-sm-4">
        <div class="form-group">
            <label>Contact</label>
            <asp:TextBox ID="txt_Contact" runat="server" CssClass="inputSM" MaxLength="120" 
                          TabIndex="4" Width ="100%"></asp:TextBox>
         </div>
   </div>

    <div class="col-sm-4">
         <div class="form-group">
               <label>Address</label>
               <asp:TextBox ID="txt_Custaddress" runat="server" TabIndex="5" Width ="100%" CssClass="inputSM" MaxLength="240" ></asp:TextBox>
         </div>
    </div>
             
    <div class="col-sm-4">
                       <div class="form-group">
                           <label>City</label>
                          <asp:TextBox ID="txt_CustCity" runat="server" TabIndex="6" CssClass="inputSM" MaxLength="60" Width ="100%"></asp:TextBox>
                        </div>
                 </div>   </div>
             <div class="row">
     
    <div class="col-sm-4">
                      <div class="form-group">
                           <label>Phone</label>
                           <asp:TextBox ID="txt_phone" runat="server" TabIndex="7" CssClass="inputSM"  MaxLength="30" Width ="100%"></asp:TextBox>
                      </div>
      </div>  
         
       <div class="col-sm-4">
        <div class="form-group form-inline-blk">
              <label>Is Cash Customer</label>
              <asp:RadioButtonList ID="Rdo_CashCust" runat="server" AutoPostBack="true" TabIndex="8"
                   RepeatDirection="Horizontal" >
                   <asp:ListItem Selected="True" Value="Y">Yes</asp:ListItem>
                   <asp:ListItem Value="N">No</asp:ListItem>
               </asp:RadioButtonList>
       </div>
   </div>

    <div class="col-sm-4">
     

            <div class="form-group form-inline-blk">
              <label>Generic Cash</label>
              <asp:RadioButtonList ID="rdo_GenericCash" runat="server" AutoPostBack="true" TabIndex="9"
                   RepeatDirection="Horizontal"  >
                   <asp:ListItem  Value="Y">Yes</asp:ListItem>
                   <asp:ListItem Value="N" Selected="True">No</asp:ListItem>
               </asp:RadioButtonList>
       </div>


   </div>  
                 </div>
             <div class="row">
   <div class="col-sm-4" >
                            <div class="form-group">
           <label>Credit Limit<span id="reqCr2" runat="server">*</span> </label>
           <asp:TextBox ID="txt_CreditLimit" runat="server" TabIndex="10" CssClass="inputSM" MaxLength="7" Enabled="false" Width ="100%" onKeypress='return NumericOnly(event)'></asp:TextBox>
         </div>                 
                  </div>
               
    

    <div class="col-sm-4">
                 <div class="form-group">
                                                <label>Available Balance <span id="req3" runat="server">*</span></label>
              <asp:TextBox ID="txt_availBalance" TabIndex="11" runat="server" CssClass="inputSM" onKeypress='return NumericOnly(event)' Width="100%" Visible="true"></asp:TextBox>
             </div>                     
       </div>

     <div class="col-sm-4">
       
              <div class="form-group">
                  <label>Credit Period <span id="reqCr1" runat="server">*</span><small>(Days)</small></label>
                  <asp:TextBox ID="txt_CreditPeriod" runat="server" TabIndex="12" Width ="100%" CssClass="inputSM" MaxLength="8" Enabled="false" onKeypress='return IntegerOnly(event)'></asp:TextBox>
            </div>



     </div></div>
             <div class="row">
     
    <div class="col-sm-4">
         <div class="form-group">
                  <label> Customer Class</label>
                   <telerik:RadComboBox Skin="Simple" Filter="Contains" ID="ddlCustClass" 
                     runat ="server" DataTextField="Code_Description" DataValueField="Code_Value" 
                     TabIndex="13" AutoPostBack="true" Width="100%">
                   </telerik:RadComboBox>
          </div>
     </div>

    <div class="col-sm-4">
                

             <div class="form-group">
                  <label>  Customer Type</label>
                   <telerik:RadComboBox Skin="Simple" Filter="Contains" ID="ddlCustType" 
                     runat ="server" DataTextField="Code_Description" DataValueField="Code_Value" 
                     TabIndex="14" AutoPostBack="true" Width="100%">
                   </telerik:RadComboBox>
          </div>
             </div>

    <div class="col-sm-4">
                  <div class="form-group">
                     <label>Collection Group </label>
                     <asp:TextBox ID="txt_collectiongroup" runat="server" TabIndex="15" Width ="100%" CssClass="inputSM" MaxLength="40"  ></asp:TextBox>
                  </div>
             </div>

             </div>
             <div class="row">

    <div class="col-sm-4">
                  <div class="form-group">
                     <label>TRN </label>
                     <asp:TextBox ID="txt_trn" runat="server" TabIndex="16" Width ="100%" CssClass="inputSM" MaxLength="40" ></asp:TextBox>
                  </div>
             </div>

    <div class="col-sm-4"  >
     

         <div class="form-group">
                     <label>Parent </label>
                     <asp:TextBox ID="txt_CustLocation" runat="server" TabIndex="17" Width ="100%" CssClass="inputSM" MaxLength="40" ></asp:TextBox>
                  </div>

    </div>
        
  
    <div class="col-sm-4">

          <div class="form-group form-inline-blk">
               <label>Credit Hold </label>
                <asp:RadioButtonList ID="rdo_CreditHold" runat="server" TabIndex="18"
                          RepeatDirection="Horizontal"  >
                          <asp:ListItem Value="Y">Yes</asp:ListItem>
                          <asp:ListItem Value="N" Selected="True" >No</asp:ListItem>
                 </asp:RadioButtonList>
         </div>
   </div>
              
                 
                       
                  </div>
            
                  
            <div class="row">
                <div class="col-sm-8">
                    <asp:Button ID="BtnAdd" runat="server"  CssClass="btn btn-success" Text="Save" />
                      <asp:Button ID="Btncancel" runat="server" CssClass ="btn btn-default" 
                          Text="Cancel" />
                      <asp:Button ID="BtnAddShip" runat="server" CssClass ="btn btn-primary"
                          Text="Add Shipping Location" Visible="false" />
                     
                </div>
                 <div class="col-sm-4" id="tdpr1" runat="server">
                                            <div class="form-group">
                                                <label>Price List *</label>
              
                 <asp:DropDownList ID="ddl_Pricelist" runat="server" CssClass="inputSM"  Width ="100%">
                 </asp:DropDownList>
            </div></div>
                 </div>
          <asp:HiddenField ID="opt" Value="1" runat="server" /> <asp:HiddenField ID="FSR_CUST_REL" Value="" runat="server" />
                <asp:HiddenField ID="Customer_ID" Value="" runat="server" />
                <asp:HiddenField ID="SiteUse_ID" Value="" runat="server" />
              <asp:HiddenField ID="SiteUse_IDShip" runat="server" Value="" />
               <asp:HiddenField ID="OptShip" runat="server" Value="" />
                 <asp:HiddenField ID="ShipCount" runat="server" Value="0" />
          
       
	
 
  </ContentTemplate>
                 
                  </asp:UpdatePanel>
        
                  <asp:UpdateProgress ID="UpdateProgress1"   AssociatedUpdatePanelID="UpdatePanel1" runat="server" DisplayAfter="10">
     <ProgressTemplate>
     <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
     
         <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="vertical-align:middle;" />            
           <span>Processing... </span>
       </asp:Panel>
          
       
         
    </ProgressTemplate>
            </asp:UpdateProgress>  
                    

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


             <hr />
            <p class="other-titles"><strong>Customer Ship Addresses</strong></p>

              <asp:GridView Width="100%"  ID="GVShipAddress" runat="server" EmptyDataText="No records to display" EmptyDataRowStyle-Font-Bold="true" 
                 AutoGenerateColumns="False" AllowPaging="True" AllowSorting="true"  DataKeyNames ="Site_Use_ID"
                  PageSize="25" CellPadding="0" CellSpacing="0" CssClass="tablecellalign">
                  
                  <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
                  <Columns>
                  <asp:TemplateField >
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
                      
                                                  <asp:LinkButton  runat="server" ID="lblDisable" text="Disable"  visible='<%# Bind("ShowDisable")%>'  OnClientClick="confirmLinkButtondis(this); return false;" OnClick ="lbChangeStatus_Click"></asp:LinkButton>
                                                  <asp:LinkButton  runat="server" ID="lblEnable" text="Enable" visible='<%# Bind("ShowEnable")%>'  OnClientClick="confirmLinkButton(this); return false;" OnClick ="lbChangeStatus_Click"></asp:LinkButton>
                                                                        
                              <asp:Label ID="lblStatus" runat ="server" Visible ="false"  Text='<%# Bind("Cust_Status") %>'></asp:Label>
                                                    <asp:Label ID="lblCustomer_ID" runat ="server" Visible ="false"  Text='<%# Bind("Customer_ID") %>'></asp:Label>
                                                    <asp:Label ID="lblSite_Use_ID_Ship" runat ="server" Visible ="false"  Text='<%# Bind("Site_Use_ID") %>'></asp:Label><asp:Label ID="lbETime" runat ="server" Visible ="false"  Text='<%# Bind("Customer_No") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                                    
                                                    </Columns><PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
              </asp:GridView>
              
            

         
            
          </ContentTemplate>
         
        </asp:UpdatePanel>      <asp:UpdateProgress ID="UpdateProgress2"   AssociatedUpdatePanelID="Panel" runat="server" DisplayAfter="10">
     <ProgressTemplate>
     <asp:Panel ID="Panel19" CssClass="overlay" runat="server">
     
         <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="vertical-align:middle;" />            
           <span>Processing... </span>
       </asp:Panel>
           </div>
       
         
    </ProgressTemplate>
            </asp:UpdateProgress>                  
       
	
	
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

