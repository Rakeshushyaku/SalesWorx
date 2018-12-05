<%@ Page Language="vb" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="_POP_CustomerListingNewTest.aspx.vb" Inherits="SalesWorx_BO._POP_CustomerListingNewTest" %>
<%@ Register Assembly="DropCheck" Namespace="xMilk" TagPrefix="cc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">

    <title>Route Planner - Customer List Test</title>
   
<script type="text/javascript" src="../js/jquery-1.3.2.min.js"></script>
<link href="../facebox/facebox.css" media="screen" rel="stylesheet" type="text/css"/>
<script src="../facebox/facebox.js" type="text/javascript"></script> 
<link href="../styles/UpdateProgress.css" rel="stylesheet" type="text/css">
<link href="../assets/css/bootstrap.css" rel="stylesheet" type="text/css">
<link href="../assets/css/style.css" rel="stylesheet" type="text/css">
<link href="../styles/salesworx.css" rel="stylesheet" type="text/css">
<link rel="stylesheet" type="text/css" href="../styles/superfish.css" media="screen">

       <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?sensor=false&key=AIzaSyAOpIPxhcIJ4PCHkIAl5ed_eP2cfu5HYsg"></script>
       
</head>
<body>
    <form id="frmPopupCustList" runat="server" class="outerform" >
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
     <input id="DayText" name="DayText" type="hidden"  runat="server"/>
      <input id="ComVal" name="ComVal" type="hidden"  runat="server"/>
      <input id="DayRef" name="DayRef" type="hidden"  runat="server"/>
        <input id="ComString" name="ComString" type="hidden"  runat="server"/>



    
	
	<div id="contentofpage" class="popupcontentblk" style="width:auto;">
	    <p class="popupcontenterror"><asp:Label ID="ErrMsg" runat="server" ForeColor="Red"></asp:Label></p>
	   
        
	    <div class="row">
           
            <div class="col-sm-6">
                <label>Mapp</label>
       


<%--<telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="Button1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="Label1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
</telerik:RadAjaxManager>--%>


                 <telerik:RadAjaxPanel runat ="server" ID="rap">


                   <asp:HiddenField ID="hfDefLat" runat="server" />
                   <asp:HiddenField ID="hfDefLng" runat="server" />
                
                      
                      <div id="map_canvas" ></div>
              
                  
              
                </telerik:RadAjaxPanel>
            </div>
	    

		    <asp:Panel ID="MoveCopyPanel" runat="server" Visible="False">
                <div class="col-sm-6">
                    <div class ="row">
                     <div class="col-sm-6">
                          
                          <div class="form-group">
                              <label>CommentsTest</label>
                    <asp:TextBox ID="UserComments" CssClass ="inputSM" Height="98px" TextMode="MultiLine" Width="250" runat="server"></asp:TextBox>
                    <asp:CheckBox ID="chk_Optmize" runat="server" Visible="False" />
                </div>

                   
                     </div>

                         <div class="col-sm-6">
                              <div class="form-group form-inline-blk">
                                   <label>&nbsp;</label>
                              <asp:Panel ID="Panel1" runat="server" Height="88px"  ScrollBars="Auto" BorderColor="#cccccc" BorderStyle="Solid" BorderWidth="1px" Width="200">
                            <asp:CheckBoxList ID="MultiCheck" Width="100" DataTextField="DateStr" DataValueField="Date_ID" runat="server" ></asp:CheckBoxList>
                        </asp:Panel>
                    </div>                                     
                       <div class="form-group form-inline-blk">
                        <asp:Button ID="MoveBtn" runat="server" Text="Move" CssClass="btn btn-primary2" />
                        <asp:Button ID="CopyBtn" runat="server"  Text="Copy" CssClass="btn btn-warning" />
                    </div> 
                             </div>
                    </div>
                   
                </div> 
            </asp:Panel>
        </div>
									                             
		
        		   <hr />	    
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
                        <asp:Button ID="ResetAllBtn" runat="server" Text="Reset All" CssClass="btn btn-default" />
					<asp:Button ID="ResetBtn" runat="server" Text="Reset Day" CssClass="btn btn-default" />
                    <asp:Button ID="DayOffBtn" runat="server" Text="Day Off" CssClass="btn btn-primary2" />
                    <asp:Button ID="SetVisitsBtn" runat="server" Text="Set Visits" CssClass="btn btn-success"  OnClientClick="return ValidateVisit()"  />   
                   <asp:Button ID="Button1" runat="server" Text="Test Map" CssClass="btn btn-success"   />     
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
                              	   <asp:Panel runat ="server" ID="pngrid" Height ="210px" ScrollBars ="Vertical">
                         
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
                              	   <asp:Panel runat ="server" ID="Panel2" Height ="210px" ScrollBars ="Vertical">
                                <asp:GridView ID="CustomerGrid"  runat="server" AutoGenerateColumns="False" 
                                           EnableViewState="true"   CellPadding="0" CellSpacing="0" CssClass="tablecellalign"  EmptyDataText="No visits planned">
                                    <Columns>
                                   
                                        <asp:TemplateField>
                                           <ItemTemplate>
                                  <asp:HiddenField ID="CustomerNo" runat="server" Value='<%# Bind("Customer_No") %>'/>
                                                <asp:Label  ID="Customer_No" runat="server" Text='<%# Bind("Customer_No") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderTemplate>
                                             Cust.No
                                            </HeaderTemplate>
                                        </asp:TemplateField>
                                       
                                       <asp:TemplateField>
                                           <ItemTemplate>
                                  <asp:HiddenField ID="CustomerName" runat="server" Value='<%# Bind("Customer_Name") %>'/>
                                   <asp:HiddenField ID="Sequence" runat="server" Value='<%# Bind("Sequence") %>'/>
                                                <asp:Label  ID="Customer_Name" runat="server" Text='<%# Bind("Customer_Name") %>'></asp:Label>
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
                                        <asp:TemplateField>
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
                                                <asp:Label ID="Customer_ID" runat="server" Text='<%# Bind("Customer_ID") %>' Visible="False" ></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField >
                                            <ItemTemplate>
                  <asp:HiddenField ID="SiteID" runat="server" Value='<%# Bind("Site_Use_ID") %>'/>
                                                <asp:Label ID="Site_Use_ID" runat="server" Text='<%# Bind("Site_Use_ID") %>' Visible="False"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                     <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle   />
                                                    <RowStyle CssClass="tdstyle"   />
                                                    <AlternatingRowStyle CssClass="alttdstyle"  />
                                </asp:GridView>  </asp:Panel>
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
     
         <img src="../assets/img/ajax-loader.gif" alt="Processing..."style="vertical-align: middle;" />
                            <span style="font-size: 12px; font-weight: bold; color: #3399ff;">Processing... </span>
       </asp:Panel>
           
       
         
    </ProgressTemplate>
            </asp:UpdateProgress>  

          <script language="javascript" type="text/javascript">

              var prm = Sys.WebForms.PageRequestManager.getInstance();

              prm.add_initializeRequest(InitializeRequest);
              prm.add_endRequest(EndRequest);
              var postBackElement;


              function InitializeRequest(sender, args) {

                  if (prm.get_isInAsyncPostBack())
                      args.set_cancel(true);
                  postBackElement = args.get_postBackElement();
                  if (postBackElement.id == 'Btn_Filter') {
                      $get('UpdateProgress1').style.display = 'block';
                  }



                  if (postBackElement.id == 'Panel2Trigger')
                      $get('UpdateProgress2').style.display = 'block';
                  if (postBackElement.id == 'MoveBtn')
                      $get('UpdateProgress2').style.display = 'block';
                  if (postBackElement.id == 'CopyBtn')
                      $get('UpdateProgress2').style.display = 'block';

                  postBackElement.disabled = true;

                 
              }

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

                  var gridViewRowCount = null;
           if (gridViewRowCount <= 1) {
               $("#ErrMsg").text('Please add at least one customer!');
               //radalert('Please add at least one customer!', 330, 180, 'Validation', alertCallBackFn);
               return false
           }

       }




</script>

     
    <script type="text/javascript">


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

        function PlotMapp() {
            alert("PlotMapp");
            //Showing Map Area
            $("#map_canvas").show()

            var Deflat = 0;
            var Deflng = 0;

            Deflat = 25.000000;
            Deflng = 55.000000;
            alert(locationList)


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
                alert(myLatlng)
            }
            else {
                var myLatlng = new google.maps.LatLng(25.000000, 55.000000);



            }
            alert(myLatlng)
            alert("Deflat Deflng")
            alert(Deflat.value)
            alert(Deflng.value)
            var myLatlng = new google.maps.LatLng(25.000000, 55.000000);
            alert(myLatlng)
            var myOptions = {
                zoom: 8,
                center: myLatlng,
                mapTypeId: google.maps.MapTypeId.ROADMAP

            }

            alert("map")
            map = new google.maps.Map(document.getElementById('map_canvas'), myOptions);

           

            // var markerBounds = new google.maps.LatLngBounds();

            if (locationList.length > 0) {
                alert(" locationList loop");

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
                    alert(args)
                    if (args[0] != '0.0000000000' & args[0] != 25.000000) {

                        //  var latlng = new google.maps.LatLng(args[0], args[1]);
                        // markerBounds.extend(latlng);

                        var location = new google.maps.LatLng(args[0], args[1])
                        var marker = new google.maps.Marker({
                            position: location,
                            map: map,
                            icon: image
                        });
                        markers.push(marker);
                        alert("location");
                        alert(location);
                    }




                    var j = i + 1;
                    if (markers.length > 0) {
                        marker.setAnimation(google.maps.Animation.DROP); attachMessage(marker, i);
                        alert("setAnimation");
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
                if (markers.length > 0) {

                    for (var i = 0; i < markers.length; i++) {
                        bounds.extend(markers[i].getPosition());
                    }
                    map.fitBounds(bounds);
                    alert("fitBounds");
                }

            }
        }
    }

        function clearOverlays() {
            alert("clearOverlays");
        for (var i = 0; i < markers.length; i++) {
            markers[i].setMap(null);
        }
        markers = [];
            
           PlotMapp();
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
    //function HideMap() {
     //   $("#map_canvas").hide()
   // }

</script>
        </form>
</body>
</html>
