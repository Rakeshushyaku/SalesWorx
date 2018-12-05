<%@ Page Title="Back Ground Report" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master" CodeBehind="AdminBGReports.aspx.vb" Inherits="SalesWorx_BO.AdminBGReports" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">

.rcbSlide
{
	z-index: 100002 !important;
}
</style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <script type="text/javascript">
       function onLoadLoc(sender) {
           if ($("#ctl00_ContentPlaceHolder1_ddl_Location").find(".rcbCheckAllItemsCheckBox:checkbox").length > 0) {
               $("#ctl00_ContentPlaceHolder1_ddl_Location").find(".rcbCheckAllItemsCheckBox:checkbox").change(function() {
             
                   var $this = $(this);
                   // $this will contain a reference to the checkbox
                   if ($this.is(':checked')) {
                   
                       document.getElementById("ctl00_ContentPlaceHolder1_btnLoadCustomer").click()
                   } else {
                       document.getElementById("ctl00_ContentPlaceHolder1_btnLoadCustomer").click()
                   }
               });

           }
       }

       function onLoadAgen(sender) {
           if ($("#ctl00_ContentPlaceHolder1_ddl_Agency").find(".rcbCheckAllItemsCheckBox:checkbox").length > 0) {
               $("#ctl00_ContentPlaceHolder1_ddl_Agency").find(".rcbCheckAllItemsCheckBox:checkbox").change(function() {
               
                   var $this = $(this);
                   // $this will contain a reference to the checkbox   
                   if ($this.is(':checked')) {
                       document.getElementById("ctl00_ContentPlaceHolder1_BtnLoadItem").click()
                   } else {
                       document.getElementById("ctl00_ContentPlaceHolder1_BtnLoadItem").click()
                   }
               });

           }
       }

   </script>
    <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
	<tr>
	
	<td id="contentofpage" width="100%" height="76%" class="topshadow">
		<div class="pgtitileposition">
	        <span class="pgtitile3">Background Report</span>
	</div> 
	
	<table width="auto" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
	<tr>
<td style="padding:6px 12px">
 
                        
       
  <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
              
        
<table width="100%" border="0" cellspacing="2" cellpadding="2">
          
          <tr>
          <td>
         <table width="100%" cellpadding ="6" cellspacing ="6">
                                             <tr>
                                                <td class="txtSMBold">
                                                    Year :
                                                </td>
                                                <td>
                                                 
                                                    <telerik:RadComboBox ID="ddl_year"  runat="server" AllowCustomText="false" MarkFirstMatch="true"   >
                                                <Items> 
                                                </Items>
                                                </telerik:RadComboBox> 
                                                    
                                                </td>
                                                <td class="txtSMBold">
                                                    Month :
                                                </td>
                                                <td>
                                                 
                                                    <telerik:RadComboBox ID="ddlMonth"  CheckBoxes="true" EnableCheckAllItemsCheckBox="true"  runat="server"  >
                                                <Items> 
                                                </Items>
                                                </telerik:RadComboBox> 
                                                   
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="txtSMBold">
                                                    Division :
                                                
                                                </td>
                                                <td>
                                               
                                                 <telerik:RadComboBox ID="ddl_Org"  runat="server" AllowCustomText="false" MarkFirstMatch="true" AutoPostBack="true" >
                                                <Items> 
                                                </Items>
                                                </telerik:RadComboBox> 
                                               
                                                </td>
                                                <td class="txtSMBold">
                                                    Route:
                                                </td>
                                                <td>
                                                 <telerik:RadComboBox ID="ddl_Van"  runat="server" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"  >
                                                <Items> 
                                                </Items>
                                                </telerik:RadComboBox> 
                                                </td>
                                            </tr>
                                             <tr>
                                                 <td class="txtSMBold">
                                                     Doc Type:</td>
                                                 <td>
                                                      <telerik:RadComboBox ID="ddl_DocType"  runat="server" AllowCustomText="false" MarkFirstMatch="true"   >
                                                <Items>
                                                <telerik:RadComboBoxItem Value="'C','I'" Text="Both" /> 
                                                <telerik:RadComboBoxItem Value="'I'" Text="Order" /> 
                                                <telerik:RadComboBoxItem Value="'C'" Text="Retruns" /> 
                                                </Items>
                                                </telerik:RadComboBox> </td>
                                                 <td class="txtSMBold">
                                                     &nbsp;</td>
                                                 <td>
                                                     &nbsp;</td>
                                             </tr>
                                            <tr>
                                                <td class="txtSMBold">
                                                    Area :
                                                
                                                </td>
                                                <td>
                                                 <telerik:RadComboBox ID="ddl_Area"  runat="server"  CheckBoxes="true" EnableCheckAllItemsCheckBox="true" >
                                                <Items> 
                                                </Items>
                                                </telerik:RadComboBox> 
                                                </td>
                                                <td class="txtSMBold">
                                                    Location:
                                                </td>
                                                <td>
                                                
                                                 <telerik:RadComboBox ID="ddl_Location"  runat="server"  AutoPostBack="true" CheckBoxes="true"  OnClientLoad="onLoadLoc" EnableCheckAllItemsCheckBox="true">
                                                <Items> 
                                                </Items>
                                                </telerik:RadComboBox>
                                                 <asp:Button ID="btnLoadCustomer" CssClass="btnInputGreen" TabIndex="5"  style="display:none;" 
                                                        runat="server" />
                                                   
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="txtSMBold">
                                                    Customer :
                                                </td>
                                                <td colspan="3">
                                                 
                                                    <telerik:RadComboBox ID="ddl_Customer" width="400" runat="server" CheckBoxes="true" EnableCheckAllItemsCheckBox="true">
                                                <Items></Items>
                                                </telerik:RadComboBox> 
                                                  
                                                </td>
                                                
                                            </tr>
                                            <tr>
                                             <td class="txtSMBold">
                                                    Manufacture:
                                                </td>
                                                <td colspan="3">
                                               
                                                 <telerik:RadComboBox ID="ddl_Agency"  runat="server"  AutoPostBack="true" CheckBoxes="true"  OnClientLoad="onLoadAgen" EnableCheckAllItemsCheckBox="true">
                                                <Items> 
                                                </Items>
                                                </telerik:RadComboBox>
                                                <asp:Button ID="BtnLoadItem" CssClass="btnInputGreen" TabIndex="5"  style="display:none;" 
                                                        runat="server" />
                                                  
                                                </td>
                                                </tr>
                                               <tr>
                                                <td class="txtSMBold" >
                                                    Item:
                                                </td>
                                                <td colspan="3">
                                                   <telerik:RadComboBox ID="ddl_Product"  runat="server" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" width="400">
                                                <Items></Items>
                                                </telerik:RadComboBox> 
                                                </td>
                                            </tr>
                                           
                                            <tr>
                                                <td colspan="4">
                                                    <asp:Panel ID="Panel12" runat="server" Style="display: none" CssClass="overlay">
                                                        <img alt="Processing..." src="../images/Progress.gif" style="z-index: 10010; vertical-align: middle;" />
                                                        <span style="font-size: 12px; color: #3399ff;">Processing... </span>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                            <td  colspan="2" ></td>
                                                <td colspan="2" align="left" >
                                                
                                                    <asp:Button ID="btnSave" CssClass="btnInputGreen" TabIndex="5" OnClick="btnSave_Click"
                                                        runat="server" Text="Save" />
                                                 
                                                    <asp:Button ID="btnCancel" CssClass="btnInputRed" TabIndex="6" runat="server" CausesValidation="false"
                                                        Text="Clear" />
                                                </td>
                                            </tr>
                                        </table>
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
             <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                        <tr>
                                           
                                            <td  class="txtSMBold">
                                                &nbsp;From Date: &nbsp;</td>
                                            <td class="txtSMBold" width="135">
                                                 <asp:TextBox ID="txt_fromDate" with="150px"  runat="server" CssClass="inputSM"></asp:TextBox>
                <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" 
                    CssClass="cal_Theme1" Format="dd-MMM-yyyy" PopupButtonID="txt_fromDate" 
                    TargetControlID="txt_fromDate" />
                                            </td><td class="txtSMBold" width="75">
                                                &nbsp;Todate:</td>
                                                <td>
                                                    <asp:TextBox ID="txt_ToDate" with="150px" runat="server" CssClass="inputSM"></asp:TextBox>
                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" 
                    CssClass="cal_Theme1" Format="dd-MMM-yyyy" PopupButtonID="txt_ToDate" 
                    TargetControlID="txt_ToDate" />
                                                </td>
                                                <td class="txtSMBold">Status:</td>
                                                <td><asp:DropDownList runat="server" ID="ddl_Status">
                                                    <asp:ListItem Value="-1">All</asp:ListItem>
                                                    <asp:ListItem Value="N">New</asp:ListItem>
                                                    <asp:ListItem Value="Y">Processed</asp:ListItem>
                                                    <asp:ListItem Value="X">Cancelled</asp:ListItem>
                                                    </asp:DropDownList> </td>
                                                <td><asp:Button ID="btnFilter" runat="server" CausesValidation="False" CssClass="btnInputGrey"
                                                    TabIndex="4" Text="Search" />
                                                    <asp:Button ID="btnClearFilter" runat="server" CausesValidation="False" CssClass="btnInputGrey"
                                                    TabIndex="4" Text="Clear Filter" />
                                                    </td>
                                                </table>
                             
                      
                      <table border="0" cellspacing="0" cellpadding="0" Width="90%"  style="padding:10px">
                                        <tr>
                                            <td>
                                                <asp:GridView Width="100%" ID="gvDivConfig" runat="server" EmptyDataText="No Data Found."
                                                    EmptyDataRowStyle-Font-Bold="true" AutoGenerateColumns="False"
                                                    AllowPaging="True" AllowSorting="True"  PageSize="25" CellPadding="0" 
                                                    CssClass="tablecellalign">
                                                   
                                                    <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
                                                    <EmptyDataRowStyle Font-Bold="True" />
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="cbSelectAll" onclick="CheckAll(this)" ToolTip="Select/Unselect All"
                                                                    runat="server" />
                                                                <asp:ImageButton ToolTip="Delete Selected Items " ID="btnDeleteAll" runat="server"
                                                                    OnClick="btnDeleteAll_Click" CausesValidation="false" ImageUrl="~/images/_del.gif"
                                                                    OnClientClick="return TestCheckBox()" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:HiddenField ID="HReport_ID" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"Report_ID") %>'/>
                                                                                                                                                                                              
                                                                <asp:CheckBox ID="chkDelete" runat="server"  Visible ='<%# Bind("DelIsVisible") %>'  />
                                                                <asp:ImageButton ToolTip="Delete" ID="btnDelete" Visible ='<%# Bind("DelIsVisible") %>' 
                                                                    OnClick="btnDelete_Click" runat="server" CausesValidation="false" ImageUrl="~/images/_del.gif"
                                                                    OnClientClick="javascript:return confirm('Would you like to delete?');" />
                                                                
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Created_At" HeaderText="Created At" SortExpression="Created_At">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="UserName" HeaderText="Created By" SortExpression="UserName">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Status" HeaderText="Status"
                                                            SortExpression="Status" >
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Out put File">
                                                         <ItemTemplate>
                                                           <asp:HyperLink Visible ='<%# Bind("DownloadlIsVisible") %>' NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"OutputFile") %>' Text="Download" runat="server" ID="DwnloadLnk"></asp:HyperLink>
                                                          
                                                         </ItemTemplate>
                                                         </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="View">
                                                         <ItemTemplate>
                                                          <asp:LinkButton ToolTip="View" ID="btnView" 
                                                                    OnClick="btnView_Click" runat="server" CausesValidation="false"  Text="View"
                                                                     />
                                                        
                                                         </ItemTemplate>
                                                         </asp:TemplateField>
                                                    </Columns>
                                                   <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
                                                </asp:GridView>
                                            </td>
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
                                                
                                                
                                                 <asp:Button ID="BtnDetails" CssClass="btnInput" runat="Server" Style="display: none" />
                                    <ajaxToolkit:ModalPopupExtender BackgroundCssClass="modalBackground" ID="MPInfoRep"
                                        runat="server" PopupControlID="DetailInfoPnl" TargetControlID="BtnDetails"
                                        CancelControlID="btnCancelDetails">
                                    </ajaxToolkit:ModalPopupExtender>
                                    <asp:Panel ID="DetailInfoPnl" runat="server" Width="750" CssClass="modalPopup" Style="display: none" >
                                   
                                        <asp:Panel ID="Panel2" runat="server" Width="741px" Font-Size ="13px" Style="cursor: move; background-color: #3399ff;
                                            text-align: center; border: solid 1px #3399ff; color: White; padding: 3px">
                                            Report Criteria</asp:Panel><asp:HiddenField ID="HiddenField1" runat="server" Value="-1" />
                                        <asp:Label ID="Label1" runat="server" Text="" ForeColor="Maroon"></asp:Label>
                                        
                                        <table width="100%" cellpadding ="6" cellspacing ="6">
                                             <tr>
                                                <td class="txtSMBold">
                                                    Year :
                                                </td>
                                                <td>
                                                 
                                                      <asp:Label ID="lbl_year" runat="server" Text="" ForeColor="Maroon"></asp:Label> 
                                                    
                                                </td>
                                                <td class="txtSMBold">
                                                    Division :
                                                
                                                </td>
                                                <td>
                                                  <asp:Label ID="lbl_Division" runat="server" Text="" ForeColor="Maroon"></asp:Label> 
                                                </td>
                                                
                                            </tr>
                                            <tr>
                                                <td class="txtSMBold">
                                                    Month :
                                                </td>
                                                <td colspan="3">
                                                 
                                                      <asp:Label ID="lbl_months" runat="server" Text="" ForeColor="Maroon"></asp:Label> 
                                                   
                                                </td>
                                                
                                            </tr>
                                            <tr>
                                            <td class="txtSMBold">
                                                    Route:
                                                </td>
                                                <td colspan="3">
                                                <div style="height: 20px; overflow-y: scroll;">
                                                 <asp:Label ID="lbl_Vans" runat="server" Text="" ForeColor="Maroon"></asp:Label>
                                                  </div>
                                                </td>
                                            </tr>
                                             <tr>
                                            <td class="txtSMBold">
                                                    Doc Type:
                                                </td>
                                                <td colspan="3">
                                                 <asp:Label ID="lbl_DocType" runat="server" Text="" ForeColor="Maroon"></asp:Label> 
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="txtSMBold">
                                                    Area :
                                                
                                                </td>
                                                <td colspan="3">
                                                 <div style="height: 20px; overflow-y: scroll;">
                                                 <asp:Label ID="lbl_Area" runat="server" Text="" ForeColor="Maroon"></asp:Label> 
                                                  </div>
                                                </td>
                                             </tr>
                                             <tr>
                                                <td class="txtSMBold">
                                                    Location:
                                                </td>
                                                <td colspan="3">
                                                  <div style="height: 20px; overflow-y: scroll;">
                                                <asp:Label ID="lbl_Location" runat="server" Text="" ForeColor="Maroon"></asp:Label>  
                                                 </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="txtSMBold">
                                                    Customer :
                                                </td>
                                                <td colspan="3">
                                                 <div style="height: 100px; overflow-y: scroll;">
                                                    <asp:Label ID="lbl_Customer" runat="server" Text="" ForeColor="Maroon"></asp:Label>
                                                  </div>
                                                </td>
                                                
                                            </tr>
                                            <tr>
                                             <td class="txtSMBold">
                                                    Manufacture:
                                                </td>
                                                <td colspan="3">
                                                <div style="height: 70px; overflow-y: scroll;">
                                                <asp:Label ID="lbl_Agency" runat="server" Text="" ForeColor="Maroon"></asp:Label>
                                                  </div>
                                                </td>
                                                </tr>
                                               <tr>
                                                <td class="txtSMBold" >
                                                    Item:
                                                </td>
                                                <td colspan="3">
                                                 <div style="height: 100px; overflow-y: scroll;">
                                                   <asp:Label ID="lbl_Items" runat="server" Text="" ForeColor="Maroon"></asp:Label>
                                                   </div>
                                                </td>
                                            </tr>
                                           
                                            <tr>
                                                <td colspan="4">
                                                    <asp:Panel ID="Panel3" runat="server" Style="display: none" CssClass="overlay">
                                                        <img alt="Processing..." src="../images/Progress.gif" style="z-index: 10010; vertical-align: middle;" />
                                                        <span style="font-size: 12px; color: #3399ff;">Processing... </span>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                            <td  colspan="2" ></td>
                                                <td colspan="2" align="left" >
                                                    
                                                 
                                                    <asp:Button ID="btnCancelDetails" CssClass="btnInputRed" TabIndex="6" runat="server" CausesValidation="false"
                                                        Text="Close" />
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
	
</asp:Content>