<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master" CodeBehind="Rep_ExpenseLocation.aspx.vb" Inherits="SalesWorx_BO.Rep_ExpenseLocation" %>
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



 <script type="text/javascript" src="http://maps.google.com/maps/api/js?sensor=false"></script>

        

            <script type='text/javascript'>

                var map;
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

                        var myLatlng = new google.maps.LatLng(25.000000, 55.000000);

                        var myOptions = {
                            zoom: 8,
                            center: myLatlng,
                            mapTypeId: google.maps.MapTypeId.ROADMAP
                        }


                        map = new google.maps.Map(document.getElementById('map_canvas'), myOptions);
                        document.getElementById('map_canvas').style.height = '730px';
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

                                if (args[0] != '0.0000000000') {

                                    var location = new google.maps.LatLng(args[0], args[1])
                                    var marker = new google.maps.Marker({
                                        position: location,
                                        map: map,
                                        icon: image
                                    });
                                }




                                var j = i + 1;
                                marker.setAnimation(google.maps.Animation.DROP);

                                attachMessage(marker, i);



                            }


                        }
                    }
                }

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
                            <span class="pgtitile3">Fuel Expense Location Report</span></div>
                       
                        
                            
                
                       
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
                                    From Date<br />
                              
                                
                            <asp:TextBox  ID="txtFromDate"   Width ="150px" CssClass="inputSM" runat="server"></asp:TextBox>&nbsp;
                <ajaxToolkit:CalendarExtender  CssClass="cal_Theme1" ID="calendarButtonExtender" Format="dd-MMM-yyyy" runat="server" TargetControlID="txtFromDate" PopupButtonID="txtFromDate"  />                

                                
                               
                                 
                                   
                                </td>
                           
                         
                              
                        </tr>
                         <tr>
                           <td width="3%" valign ="top" class ="txtSMBold" >
                                    To Date<br />
                              
                                
                            <asp:TextBox  ID="txtToDate"   Width ="150px" CssClass="inputSM" runat="server"></asp:TextBox>&nbsp;
                <ajaxToolkit:CalendarExtender  CssClass="cal_Theme1" ID="CalendarExtender1" Format="dd-MMM-yyyy" runat="server" TargetControlID="txtToDate" PopupButtonID="txtToDate"  />                

                                
                               
                                   
                                        
                                   
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
