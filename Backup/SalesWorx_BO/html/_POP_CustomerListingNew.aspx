<%@ Page Language="vb" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="_POP_CustomerListingNew.aspx.vb" Inherits="SalesWorx_BO._POP_CustomerListingNew" %>
<%@ Register Assembly="DropCheck" Namespace="xMilk" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
<%--<script>
    HideWait();
</script>--%>
    <title>Route Planner - Customer List</title>
   
     <script type="text/javascript" src="../js/jquery-1.3.2.min.js"></script>
<link href="../facebox/facebox.css" media="screen" rel="stylesheet" type="text/css"/>
<script src="../facebox/facebox.js" type="text/javascript"></script> 
<link href="../styles/UpdateProgress.css" rel="stylesheet" type="text/css">
<link href="../styles/swxstyle.css" rel="stylesheet" type="text/css">
<link href="../styles/salesworx.css" rel="stylesheet" type="text/css">
<link rel="stylesheet" type="text/css" href="../styles/superfish.css" media="screen">

    <script type="text/javascript">


       var TargetBaseControl = null;

       window.onload = function() {
           try {
               TargetBaseControl =
           document.getElementById('<%= Me.TimeSelPanel.ClientID %>');
           }
           catch (err) {
               TargetBaseControl = null;
           }
       }

       
      
</script>
       
</head>
<body>
    <form id="frmPopupCustList" runat="server" class="outerform" >
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
     <input id="DayText" name="DayText" type="hidden"  runat="server"/>
      <input id="ComVal" name="ComVal" type="hidden"  runat="server"/>
      <input id="DayRef" name="DayRef" type="hidden"  runat="server"/>
        <input id="ComString" name="ComString" type="hidden"  runat="server"/>

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
       
//           if (postBackElement.id == '<%= Me.TimeSelPanel.FindControl("TimeSelection").ClientID %>')
//               $get('UpdateProgress2').style.display = 'none';

           postBackElement.disabled = false;
       } 

</script>
     <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">

	<tr>
	
	<td id="contentofpage" width="100%" height="100%" class="topshadow">
	
	<span class="pgtitile3">Create Route Plan</span>
	

	  <table width="100%" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
	<tr>
					<td align="center">
                        <asp:Label ID="ErrMsg" runat="server" Font-Names="Calibri" ForeColor="#CC3300"></asp:Label> </td>
	</tr>
	
	<tr>
								<td style="padding:6px 6px 2px 12px;" class="txtBold"> Comments:&nbsp;&nbsp;
                                    <asp:TextBox ID="UserComments" CssClass ="inputSM" style="WIDTH:220px" runat="server"></asp:TextBox>
									&nbsp;
                                    <asp:CheckBox ID="chk_Optmize" runat="server" Visible="False" />
                                </td>
								<td></td>
	</tr>
	
		<tr>
								<td class="txtSM" align="center">
									<hr width='95%' size="1" "noshade">
									<asp:Button ID="ResetAllBtn" runat="server" Text="Reset All" 
                                        CssClass="btnInputRed" />
									<asp:Button ID="ResetBtn" runat="server" Text="Reset Day" CssClass="btnInputGreen" />
                                    <asp:Button ID="DayOffBtn" runat="server" Text="Day Off" CssClass="btnInputOrange" />
                                    <asp:Button ID="SetVisitsBtn" runat="server" Text="Set Visits" CssClass="btnInputBlue" OnClientClick="return TestCheckBox()"  />
                                  
                                  	<hr width='95%' size="1" "noshade">
								</td>
		</tr>
		
		 <tr>
						    <td style="padding:6px 6px 2px 12px;" >
						     <asp:Panel ID="MoveCopyPanel" runat="server" Visible="False">
                                <table>
                                <tr>
                                <td align valign="top">
                                    <asp:Panel ID="Panel1" runat="server" Height="100px"  ScrollBars="Auto" BorderStyle="Groove"
                                                BorderWidth="1px" Width="120px">
                                    <asp:CheckBoxList ID="MultiCheck"  DataTextField="DateStr" DataValueField="Date_ID" 
                                                    runat="server" >
                                                </asp:CheckBoxList>
                                                 </asp:Panel>
                               <%--  <cc1:DropCheck ID="MultiCheck" runat="server" 
                                      MaxDropDownHeight="200" TransitionalMode="True" Width="120px" 
                                     Font-Names="Tahoma, Calibri, Arial, sans-serif, Helvetica;" 
                                      DataTextField="DateStr" DataValueField="Date_ID"  ></cc1:DropCheck>--%>
                                      
                                      </td>
                                <td> <asp:Button ID="MoveBtn" runat="server" Text="Move" OnClientClick="return OnHoldValue()" CssClass="btnInputGreen" /></td>
                                <td>  <asp:Button ID="CopyBtn" runat="server" OnClientClick="return OnHoldValue()" Text="Copy" CssClass="btnInputBlue" /> </td>
                           
                                </tr>
                                </table>
                                
                 
                                       <hr width='95%' size="1" "noshade">
                                     
						    
						        </asp:Panel>
						 				                             
						      </td>
		 </tr>
		 	    <tr>
						    	
						    <td style="padding:6px 6px 2px 12px;"  class="txtBold">
						     Filter by:
						     <asp:DropDownList CssClass ="inputSM" ID="FilterType" runat="server" 
                                      DataTextField="Customer_No" 
                                      AutoPostBack="false"> 
                                      <asp:ListItem Value="">Select Filter</asp:ListItem>
                                   <asp:ListItem Value="CustomerNo">Customer No.</asp:ListItem>
                                   <asp:ListItem Value="CustomerName">Customer Name</asp:ListItem>
                                   <asp:ListItem Value="Address">Address</asp:ListItem>
                                   <asp:ListItem Value="City">City</asp:ListItem>
                                   <asp:ListItem Value="Class">Class</asp:ListItem>
                                </asp:DropDownList>
                                <asp:TextBox ID="txt_Filter" runat="server" CssClass ="inputSM"></asp:TextBox> 
                                <asp:Button ID="Btn_Filter" CssClass ="btnInputGrey" runat="server" Text="Search" />
						    &nbsp;<asp:Button ID="Btn_Clear" CssClass ="btnInputRed" runat="server" Text="Clear" />
						    </td>
		   </tr>
				     <tr>
									
							<td style="padding:6px 6px 2px 12px;">
						
								<input id="ValueHolder" runat="server" type="hidden" />
							 <input id="ValueNotHolder" runat="server" type="hidden" />
						 <asp:UpdatePanel ID="TimeSelPanel" runat="server" UpdateMode="Conditional">
							<ContentTemplate>
				
							           <asp:CheckBox ID="TimeSelection" runat="server"     Text="Enable Time Selection" style="display:none"
                                    AutoPostBack="false"    />
                                        &nbsp;&nbsp;
                                                                               
                                <br />
                                <div style ="float:left;margin:0px;width:50%;">
                               <span style="font-weight:bold;font-size:12px;"> Actual Customers List</span><br />
                              	   <asp:Panel runat ="server" ID="pngrid" Height ="210px" ScrollBars ="Vertical">
                         
                                <asp:GridView ID="FilterCustomerGrid" runat="server" AutoGenerateColumns="False" 
                                           EnableViewState="true"   AllowPaging="false" PageSize="10"  
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
                        
                               <div style ="float:right;margin:0px;width:50%;">
                              <span style="font-weight:bold;font-size:12px;"> Planned Customers List</span><br />
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
							  
                                </td>
                                  
							</tr>
								<asp:ListBox id="SelectedList" runat="server" Width="308px" 
                        Visible ="False"></asp:ListBox>
     
  
    </table>
	<br/>
	<br/>
	</td> <!-- "contentofpage" ends in this td -->
	</tr>
	

</table>
          <asp:UpdateProgress ID="UpdateProgress1"  AssociatedUpdatePanelID="TimeSelPanel" runat="server">
     <ProgressTemplate>
     <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
     
         <img src="../images/Progress.gif" alt="Processing..." style="vertical-align:middle;" />            
           <span style="font-size:12px; font-weight:700px; color:#3399ff;">Processing... </span>
       </asp:Panel>
           </div>
       
         
    </ProgressTemplate>
            </asp:UpdateProgress>  

  
        </form>
</body>
</html>
