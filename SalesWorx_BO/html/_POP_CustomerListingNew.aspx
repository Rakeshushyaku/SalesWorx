<%@ Page Language="vb" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="_POP_CustomerListingNew.aspx.vb" Inherits="SalesWorx_BO._POP_CustomerListingNew" %>
<%@ Register Assembly="DropCheck" Namespace="xMilk" TagPrefix="cc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
     <title>Route Planner - Customer List </title>
   
<script type="text/javascript" src="../js/jquery-1.3.2.min.js"></script>
<%--    <script src="https://code.jquery.com/jquery-1.8.0.min.js"></script>--%>
<link href="../facebox/facebox.css" media="screen" rel="stylesheet" type="text/css"/>
<script src="../facebox/facebox.js" type="text/javascript"></script> 
<link href="../styles/UpdateProgress.css" rel="stylesheet" type="text/css">
<link href="../assets/css/bootstrap.css" rel="stylesheet" type="text/css">
 <link href="../assets/css/style.css" rel="stylesheet" type="text/css"> 
<link href="../styles/salesworx.css" rel="stylesheet" type="text/css">
<link rel="stylesheet" type="text/css" href="../styles/superfish.css" media="screen">

<%--      <script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyAOpIPxhcIJ4PCHkIAl5ed_eP2cfu5HYsg"></script>--%>
    <%-- <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?sensor=true&key=AIzaSyAOpIPxhcIJ4PCHkIAl5ed_eP2cfu5HYsg"></script> --%>
    <script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyAOpIPxhcIJ4PCHkIAl5ed_eP2cfu5HYsg&callback=clearOverlays1"></script>
     <style>
        #map {
            height: 290px;
            width: 100%;
        }
        .p-l5-r5 {
            padding-left:5px;
            padding-right:5px;
        }
        .error {
            display: inherit !important;
            padding: 0 !important;
        }
         .popupcontenterror span {
             font-size: 12px !important;
         }
         .td-breaking-word {
             word-break: break-all;
         }
    </style>
 
     <script language="javascript" type="text/javascript">

         function InfoTest(id) {

             var args = id.split('~');

             document.getElementById('MapMode').value = args[0];
             document.getElementById('CustID').value = args[1];
             document.getElementById('CustSiteID').value = args[2];
             clickSearch()

         }



         function PlotMapp() {

             //Showing Map Area
             $("#map_canvas").show()

             var Deflat = 0;
             var Deflng = 0;

             Deflat = 23.4241;
             Deflng = 53.8478;



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
                     zoom: 4,
                     center: myLatlng,
                     mapTypeId: google.maps.MapTypeId.ROADMAP

                 }


                 map = new google.maps.Map(document.getElementById('map'), myOptions);



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
                         //var image = args[2];
                         var CustName = args[2];
                         var args2 = args[2].split('~');

                         var image = "";


                         if (args2[3] == "1 ") {

                             image = "http://maps.google.com/mapfiles/ms/icons/red.png";

                         }

                         if (args2[3] == "0 ") {

                             image = "http://maps.google.com/mapfiles/ms/icons/blue.png";

                         }


                         if (args[0] != '0.0000000000' & args[0] != 25.000000 & args2[0] != '"') {


                             //  var latlng = new google.maps.LatLng(args[0], args[1]);
                             // markerBounds.extend(latlng);
                             // $('#markers').append('<a class="marker-link" data-markerid="' + i + '" href="#">Marker ' + i + '</a> ');


                             var location = new google.maps.LatLng(args[0], args[1])
                             var marker = new google.maps.Marker({
                                 position: location,
                                 //label: CustName,
                                 map: map,
                                 icon: image,

                                 title: args2[0]

                             });
                             markers.push(marker);



                             //google.maps.event.addListener(marker, 'click', function () {
                             //    alert("Marker Click");
                             //});

                         }





                         var j = i + 1;
                         if (markers.length > 0) {
                             marker.setAnimation(google.maps.Animation.DROP); attachMessage(marker, i, args2[0], args2[1], args2[2], args2[3]);

                         }










                     }
                     google.maps.event.trigger(map, "resize");
                     //var bounds = new google.maps.LatLngBounds();
                     //if (markers.length > 1) {
                     //    alert(markers.length);
                     //    for (var q = 0; q < markers.length; q++) {

                     //        bounds.extend(markers[q].getPosition());
                     //    }
                     //    map.fitBounds(bounds);
                     //}
                     var bounds = new google.maps.LatLngBounds();
                     if (markers.length > 0) {

                         for (var i = 0; i < markers.length; i++) {
                             bounds.extend(markers[i].getPosition());
                         }
                         map.fitBounds(bounds);

                     }

                 }
             }
         }

         function clearOverlays1() {
             //  alert("clearOverlays");
             for (var i = 0; i < markers.length; i++) {
                 markers[i].setMap(null);
             }
             markers = [];

             PlotMapp();
             return false;
         }




         function attachMessage(marker, number, Cname, CId, CSiteId, Visit) {

             if (Visit != 0) {
                 var infowindow = new google.maps.InfoWindow(
                                     {
                                         content: "<div><b>" + Cname + "</b></div><div> <input id='deletecust~" + CId + "~" + CSiteId + "' type='button' value='Delete From Visit' onclick='InfoTest(this.id);'/> </div>",
                                         size: new google.maps.Size(250, 150)
                                     });
             }
             else {

                 var infowindow = new google.maps.InfoWindow(
             {
                 // content: "<b>" + str + "</b><img src='~/images/add-button.png'>", Width ="18px" Height ="18px"
                 content: "<div><b>" + Cname + "</b></div><div> <input id='addcust~" + CId + "~" + CSiteId + "' type='button' value='Add To Visit' onclick='InfoTest(this.id);'/> </div>",


                 //content: "<div><b>" + Cname + "</b></div><div><img Width ='18px' Height ='18px'  src='/images/add-button.png'></div><div> <input id='addcust~" + CId + "~" + CSiteId + "' type='button' value='Add To Visit' onclick='InfoTest(this.id);'/> </div>",
                 //content:"blah blah",
                 size: new google.maps.Size(250, 150)
             });
             }
             google.maps.event.addListener(marker, 'click', function () {
                 // alert("Marker Click");
                 if (prev_infowindow) {
                     prev_infowindow.close();
                 }

                 prev_infowindow = infowindow;
                 infowindow.open(map, marker);
             }
     );





             //google.maps.event.addDomListener(infowindow,'click',function() {

             //    alert('clicked ');
             //    }
             //);





         }





         //var prm = Sys.WebForms.PageRequestManager.getInstance();

         //prm.add_initializeRequest(InitializeRequest);
         //prm.add_endRequest(EndRequest);
         //var postBackElement;


         //function InitializeRequest(sender, args) {

         //    if (prm.get_isInAsyncPostBack())
         //        args.set_cancel(true);
         //    postBackElement = args.get_postBackElement();
         //    if (postBackElement.id == 'Btn_Filter') {
         //        $get('UpdateProgress1').style.display = 'block';
         //    }



         //    if (postBackElement.id == 'Panel2Trigger')
         //        $get('UpdateProgress2').style.display = 'block';
         //    if (postBackElement.id == 'MoveBtn')
         //        $get('UpdateProgress2').style.display = 'block';
         //    if (postBackElement.id == 'CopyBtn')
         //        $get('UpdateProgress2').style.display = 'block';

         //    postBackElement.disabled = true;


         //}

         function EndRequest(sender, args) {
             if (postBackElement.id == 'Btn_Filter') {
                 $get('UpdateProgress1').style.display = 'none';
             }

             if (postBackElement.id == 'Panel2Trigger')
                 $get('UpdateProgress2').style.display = 'none';
             if (postBackElement.id == 'MoveBtn')
                 $get('UpdateProgress2').style.display = 'none';
             if (postBackElement.id == 'CopyBtn')
                 $get('UpdateProgress2').style.display = 'none';



             postBackElement.disabled = false;
         }

         function ValidateVisit() {

             //var gridViewRowCount = null;
             var gridViewRowCount = document.getElementById("<%= CustomerGrid.ClientID%>").rows.length;
             if (gridViewRowCount <= 1) {
                 $("#ErrMsg").text('Please add at least one customer!');
                 //radalert('Please add at least one customer!', 330, 180, 'Validation', alertCallBackFn);
                 return false
             }

         }

         var TargetBaseControl = null;

         var map;
         var prev_infowindow = false;

         var markers = [];


         window.onload = function () {
             try {

             }
             catch (err) {
                 TargetBaseControl = null;
             }
         }


         function GetRadWindow() {
             var oWindow = null;
             if (window.radWindow) oWindow = window.radWindow;
             else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
             return oWindow;
         }


         function clickSearch() {
             $("#Button1").click()
         }
</script>
    <%--   <script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyAOpIPxhcIJ4PCHkIAl5ed_eP2cfu5HYsg"></script>--%>
</head>
<body>
    <form id="frmPopupCustList" runat="server" class="outerform">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
         <input id="DayText" name="DayText" type="hidden"  runat="server"/>
      <input id="ComVal" name="ComVal" type="hidden"  runat="server"/>
      <input id="DayRef" name="DayRef" type="hidden"  runat="server"/>
        <input id="ComString" name="ComString" type="hidden"  runat="server"/>

         <input id="CustID" name="CustID" type="hidden"  runat="server"/>
        <input id="CustSiteID" name="CustSiteID" type="hidden"  runat="server"/>
        <input id="MapMode" name="MapMode" type="hidden"  runat="server"/>
     
  
        <div style="display:none">
        <asp:Button ID="Button1" runat="server" Text="Button"  /></div>
     
        <div id="contentofpage" class="popupcontentblk" style="width:auto;">
	    <p class="popupcontenterror"><asp:Label ID="ErrMsg" runat="server" ForeColor="Red"></asp:Label></p>
	   
        
	    <div class="row">
           
            <div class="col-sm-6 col-md-8">
               

                 <telerik:RadAjaxPanel runat ="server" ID="rap">

                      
                   <asp:HiddenField ID="hfDefLat" runat="server" />
                   <asp:HiddenField ID="hfDefLng" runat="server" />
                
                        <div id="map"></div>
                     
                      <div id="markers"></div>
                  
                
                </telerik:RadAjaxPanel>
               
            </div>
	    

		    <asp:Panel ID="MoveCopyPanel" runat="server" Visible="False">
                <div class="col-sm-6 col-md-4">
                    <div class ="row">
                     <div class="col-sm-12">
                          
                          <div class="form-group">
                              <label>Comments</label>
                    <asp:TextBox ID="UserComments" CssClass ="inputSM" Height="98px" TextMode="MultiLine" Width="100%" runat="server"></asp:TextBox>
                    <asp:CheckBox ID="chk_Optmize" runat="server" Visible="False" />
                </div>

                   
                     </div>

                         <div class="col-sm-12">
                              <div class="form-group form-inline-blk">
                                   <label>Date</label>
                              <asp:Panel ID="Panel1" runat="server" Height="88px"  ScrollBars="Auto" BorderColor="#cccccc" BorderStyle="Solid" BorderWidth="1px" Width="100%" CssClass="p-l5-r5">
                            <asp:CheckBoxList ID="MultiCheck" Width="100" DataTextField="DateStr" DataValueField="Date_ID" runat="server" ></asp:CheckBoxList>
                        </asp:Panel>
                    </div>                                     
                       <div class="form-group form-inline-blk">
                        <asp:Button ID="MoveBtn" runat="server" Text="Move" CssClass="btn btn-warning" />
                        <asp:Button ID="CopyBtn" runat="server"  Text="Copy" CssClass="btn btn-warning" />
                    </div> 
                             </div>
                    </div>
                   
                </div> 
            </asp:Panel>
        </div>
									                             
		
        		   <hr />	
            <div class="row">
                <div class="col-sm-6">
                    <div class="form-group">
                    <asp:Button ID="ResetAllBtn" runat="server" Text="Reset All" CssClass="btn btn-primary2" />
					<asp:Button ID="ResetBtn" runat="server" Text="Reset Day" CssClass="btn btn-primary2" />
                    </div>
                </div>
                <div class="col-sm-6">
                    <div class="form-group text-right">
                    <asp:Button ID="DayOffBtn" runat="server" Text="Day Off" CssClass="btn btn-danger" />
                    <asp:Button ID="SetVisitsBtn" runat="server" Text="Set Visits" CssClass="btn btn-success"  OnClientClick="return ValidateVisit()"  />   
                    </div>
                </div>
            </div>  
            <hr style="margin-top:10px;" />  
		<div class="row">
            <div class="col-sm-12">
					<div class="form-group form-inline">	    	
					    <span>Filter by</span>
						<asp:DropDownList CssClass ="form-control" ID="FilterType" runat="server" 
                                DataTextField="Customer_No" 
                                AutoPostBack="false"> 
                                <asp:ListItem Value="">Select Filter</asp:ListItem>
                            <asp:ListItem Value="CustomerNo">Customer No.</asp:ListItem>
                            <asp:ListItem Value="CustomerName">Customer Name</asp:ListItem>
                            <asp:ListItem Value="Address">Address</asp:ListItem>
                            <asp:ListItem Value="City">City</asp:ListItem>
                            <asp:ListItem Value="Class">Class</asp:ListItem>
                        </asp:DropDownList>
                        <asp:TextBox ID="txt_Filter" runat="server" CssClass ="form-control"></asp:TextBox> 
                        <asp:Button ID="Btn_Filter" CssClass ="btn btn-primary" runat="server" Text="Search" />
						<asp:Button ID="Btn_Clear" CssClass ="btn btn-default" runat="server" Text="Clear" />
					</div>
		    </div>
        </div>

				     <div class="row">
						
								<input id="ValueHolder" runat="server" type="hidden" />
							 <input id="ValueNotHolder" runat="server" type="hidden" />
						 <asp:UpdatePanel ID="TimeSelPanel" runat="server" UpdateMode="Conditional">
							<ContentTemplate>
				
							    <asp:CheckBox ID="TimeSelection" runat="server"     Text="Enable Time Selection" style="display:none" AutoPostBack="false"    />
                                      
                                <div class="col-sm-6">
                               <p><strong>Actual Customers List</strong></p>
                              	   <asp:Panel runat ="server" ID="pngrid" >
                         
                                <asp:GridView ID="FilterCustomerGrid" runat="server" AutoGenerateColumns="False" 
                                           EnableViewState="true"   AllowPaging="true" PageSize="10"  
                                            EmptyDataText="No Data Available" CellPadding="0" CellSpacing="0" CssClass="tablecellalign">
                                 <Columns>
                                 <asp:TemplateField >
                                   <HeaderTemplate >Cust.No </HeaderTemplate>
                                            <ItemTemplate>
                                            
                                    <asp:HiddenField ID="Customer_ID" runat="server" Value='<%# Bind("Customer_ID") %>'/>
                                  <asp:HiddenField ID="SiteID" runat="server" Value='<%# Bind("Site_Use_ID") %>'/>
                                   <asp:HiddenField ID="Customer_No" runat="server" Value='<%# Bind("Customer_No") %>'/>
                                                <asp:Label  ID="lbl_CustomerNo" runat="server" Text='<%# Bind("Customer_No") %>'></asp:Label>

                                    </ItemTemplate>
                                 
                                    </asp:TemplateField>
                                    <asp:TemplateField >
                                    <HeaderTemplate>Cust.Name </HeaderTemplate>
                                            <ItemTemplate>
                                            
                                   <asp:Label  ID="Customer_Name" runat="server" Text='<%# Bind("Customer_Name") %>'></asp:Label>

                                    </ItemTemplate>
                                    
                                    </asp:TemplateField> 

                                             <asp:TemplateField >
                                                   <ItemStyle CssClass="td-breaking-word" />
                                        <ItemTemplate>
                                            <asp:Label ID="NoOfVisits_F" runat="server">
                                            
                                            </asp:Label>
                                        </ItemTemplate>
                                         <HeaderTemplate>
                                             Days Planned
                                            </HeaderTemplate>
                                        </asp:TemplateField>

                                      <asp:TemplateField > 
                                     <ItemTemplate>
                                        <asp:ImageButton ToolTip="Add Visit Planning" ID="AddtoVisit"     OnClick="btnAddtoVisit"                                     
                                                runat="server" CausesValidation="false" ImageUrl="~/images/add-button.png" Width ="18px" Height ="18px" />
                                        </ItemTemplate>    
                                      
                                         </asp:TemplateField>    
                                        
                                 </Columns>
                                   <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle   />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle"  />  
                                </asp:GridView>
                                </asp:Panel> 
                            
                              </div> 
                        
                               <div class="col-sm-6">
                               <p><strong>Planned Customers List</strong></p>
                              	   <asp:Panel runat ="server" ID="Panel2">
                                      
                                <asp:GridView ID="CustomerGrid"  runat="server" AutoGenerateColumns="False" 
                                           EnableViewState="true"  AllowPaging="false" PageSize="6" CellPadding="0" CellSpacing="0" CssClass="tablecellalign"  EmptyDataText="No visits planned">
                                    <Columns>
                                   
                                        <asp:TemplateField>
                                           <ItemTemplate>
                                  <asp:HiddenField ID="CustomerNo" runat="server" Value='<%# Bind("Customer_No") %>'/>
                                                <asp:Label  ID="Label1" runat="server" Text='<%# Bind("Customer_No") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderTemplate>
                                             Cust.No
                                            </HeaderTemplate>
                                        </asp:TemplateField>
                                       
                                       <asp:TemplateField>
                                           <ItemTemplate>
                                  <asp:HiddenField ID="CustomerName" runat="server" Value='<%# Bind("Customer_Name") %>'/>
                                   <asp:HiddenField ID="Sequence" runat="server" Value='<%# Bind("Sequence") %>'/>
                                                <asp:Label  ID="Label2" runat="server" Text='<%# Bind("Customer_Name") %>'></asp:Label>
                                            </ItemTemplate>
                                             <HeaderTemplate>
                                             Cust. Name
                                            </HeaderTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Customer_Class" Visible="False" />
                                        <asp:BoundField DataField="Address" Visible="False" />
                                        <asp:BoundField DataField="City" Visible="False" />
                                        <asp:TemplateField HeaderText="Start Time" Visible ="false">
                                        <ItemTemplate>
                                         <asp:Label ID="STimeLbl" runat="server" Text="Start Time" Visible="false"></asp:Label>
                                                &nbsp;
                                             <asp:DropDownList ID="StartHH" Visible ="false" runat="server" CssClass="txtSM" style="display:none">
                                        </asp:DropDownList>&nbsp;<asp:DropDownList ID="StartMM" Visible ="false" runat="server" CssClass="txtSM" style="display:none">
                                         </asp:DropDownList>
                            &nbsp; </ItemTemplate>
                                        
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="End Time" Visible ="false">
                                        <ItemTemplate >
                          <asp:Label ID="ETimeLbl"  runat="server" Text="End Time" Visible="false"  />&nbsp;
                                  <asp:DropDownList ID="EndHH" runat="server" CssClass="txtSM" Visible ="false" style="display:none">
                                </asp:DropDownList>&nbsp;<asp:DropDownList Visible ="false" ID="EndMM" runat="server" CssClass="txtSM" style="display:none">
                                </asp:DropDownList>
                                        </ItemTemplate>
                                        
                                        </asp:TemplateField>
                                       
                                        <asp:TemplateField >
                                            <ItemStyle CssClass="td-breaking-word" />
                                        <ItemTemplate>

                                            <asp:Label ID="NoOfVisits" runat="server">
                                            
                                            </asp:Label>
                                            
                                        </ItemTemplate>
                                         <HeaderTemplate>
                                             Days Planned
                                            </HeaderTemplate>


                                        </asp:TemplateField>
                                            
                                        <asp:BoundField DataField="Sequence" />
                                        <asp:TemplateField>
                                        <ItemTemplate>
                                        <asp:ImageButton ToolTip="Remove Visit Planning" ID="RemoveFromVisitPlan" OnClick="RemovefromVisitPlan"                                          
                                                runat="server" CausesValidation="false" ImageUrl= "~/images/delete-13.png" Width ="16px" Height ="16px"  />
                                        </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField>
                                        <ItemTemplate>
                                        <asp:ImageButton ToolTip="Move Up" ID="btnMoveup"    OnClick="MoveUp"   Width ="16px" Height ="16px"                                     
                                                runat="server" CausesValidation="false" ImageUrl="~/images/Moveup.jpg"/>
                                        </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField>
                                        <ItemTemplate>
                                        <asp:ImageButton ToolTip="Move Down" ID="btnMoveDown"   Width ="16px" Height ="16px"        OnClick="MoveDown"                                   
                                                runat="server" CausesValidation="false" ImageUrl="~/images/MoveDown.jpg" />
                                        </ItemTemplate>
                                        </asp:TemplateField>
                                             <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox  ID="TimeChk" runat="server"  OnCheckedChanged="TimeChk_MakeTimeVisible" AutoPostBack="false" Visible="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField >
                                            <ItemTemplate>
                                            
                                         <asp:HiddenField ID="CustomerID" runat="server" Value='<%# Bind("Customer_ID") %>'/>
                                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("Customer_ID") %>' Visible="False" ></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField >
                                            <ItemTemplate>
                  <asp:HiddenField ID="SiteID1" runat="server" Value='<%# Bind("Site_Use_ID") %>'/>
                                                <asp:Label ID="Site_Use_ID" runat="server" Text='<%# Bind("Site_Use_ID") %>' Visible="False"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                     <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle   />
                                                    <RowStyle CssClass="tdstyle"   />
                                                    <AlternatingRowStyle CssClass="alttdstyle"  />
                                </asp:GridView>   </asp:Panel>
                                 </div> 
                            </ContentTemplate>
                            <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="Btn_Filter" EventName="Click" />
                           
                              
                            </Triggers>
							</asp:UpdatePanel>
							  
                            </div>   
								<asp:ListBox id="SelectedList" runat="server" Width="308px" Visible ="False"></asp:ListBox>
     
  
   
	
</div>

         <asp:UpdateProgress ID="UpdateProgress1"  AssociatedUpdatePanelID="TimeSelPanel" runat="server">
     <ProgressTemplate>
     <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
     
         <img src="../assets/img/ajax-loader.gif" alt="Processing..."  />
                            <span>Processing... </span>
       </asp:Panel>
           
       
         
    </ProgressTemplate>
            </asp:UpdateProgress>  
    </form>
      <script language="javascript" type="text/javascript">
          function myMap() {
              var mapCanvas = document.getElementById("map");
              var mapOptions = {
                  center: new google.maps.LatLng(47.3921611, 8.4957963),
                  zoom: 14
              }

              var contentString = '<div id="content">' +
                '<div id="siteNotice">' +
                '</div>' +
                '<h1 id="firstHeading" class="firstHeading">Uluru</h1>' +
                '<div id="bodyContent">' +
                '<p><b>Uluru</b>, also referred to as <b>Ayers Rock</b>, is a large ' +
                'sandstone rock formation in the southern part of the ' +
                'Northern Territory, central Australia. It lies 335&#160;km (208&#160;mi) ' +
                'south west of the nearest large town, Alice Springs; 450&#160;km ' +
                '(280&#160;mi) by road. Kata Tjuta and Uluru are the two major ' +
                'features of the Uluru - Kata Tjuta National Park. Uluru is ' +
                'sacred to the Pitjantjatjara and Yankunytjatjara, the ' +
                'Aboriginal people of the area. It has many springs, waterholes, ' +
                'rock caves and ancient paintings. Uluru is listed as a World ' +
                'Heritage Site.</p>' +
                '<p>Attribution: Uluru, <a href="https://en.wikipedia.org/w/index.php?title=Uluru&oldid=297882194">' +
                'https://en.wikipedia.org/w/index.php?title=Uluru</a> ' +
                '(last visited June 22, 2009).</p>' +
                '</div>' +
                '</div>';

              var infowindow = new google.maps.InfoWindow({
                  content: contentString
              });

              var map = new google.maps.Map(mapCanvas, mapOptions);
              var image = 'http://maps.google.com/mapfiles/ms/micons/blue.png';
              var beachMarker = new google.maps.Marker({
                  position: map.getCenter(),
                  map: map,
                  icon: image,
              });
              beachMarker.addListener('click', function () {
                  infowindow.open(map, beachMarker);
              });
              return false;
          }
</script>
<%--<script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyAOpIPxhcIJ4PCHkIAl5ed_eP2cfu5HYsg&callback=myMap"></script>--%>
    
</body>
   
</html>

