<%@ Page Title="Back Ground Report" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="AdminBGReports.aspx.vb" Inherits="SalesWorx_BO.AdminBGReports" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>


<asp:Content ID="Content1" ContentPlaceHolderID="FeaturedContent" runat="server">
    <style type="text/css">
        input[type="text"].rdfd_
        {
            margin:0 !important;
            padding:0 !important;
            height:0 !important;
            width:0 !important;
        }
.rcbSlide
{
	z-index: 100002 !important;
}
</style>
     <script type="text/javascript">
         function onLoadLoc(sender) {
             if ($("#ctl00_ContentPlaceHolder1_ddl_Location").find(".rcbCheckAllItemsCheckBox:checkbox").length > 0) {
                 $("#ctl00_ContentPlaceHolder1_ddl_Location").find(".rcbCheckAllItemsCheckBox:checkbox").change(function () {

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
                 $("#ctl00_ContentPlaceHolder1_ddl_Agency").find(".rcbCheckAllItemsCheckBox:checkbox").change(function () {

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

         function alertCallBackFn(arg) {
             
         }
   </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  
    <h4>Background Report</h4>

         <telerik:RadAjaxLoadingPanel  ID="AjaxLoadingPanel1" runat="server" >
             <div ID="ProgressPanel" class ="overlay"  >
     
         <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index:10010;vertical-align:middle;" />      
       </div>
           </telerik:RadAjaxLoadingPanel>  

        <asp:Panel ID="Panel1" runat="server">      </asp:Panel> 
                         
 
    
                                         
                     <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>
    
                        
       
  <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
         <div class="row">
             <div class="col-sm-4">
                <div class="form-group">
                    <label>Year</label>
                    <telerik:RadComboBox ID="ddl_year"  Skin="Simple"  runat="server" AllowCustomText="false" MarkFirstMatch="true"  Width="100%">
                       
                     </telerik:RadComboBox> 
                </div>
             </div>
             <div class="col-sm-4">
                <div class="form-group">
                    <label>Month</label>
                    <telerik:RadComboBox ID="ddlMonth"  Skin="Simple"  CheckBoxes="true" EnableCheckAllItemsCheckBox="true"  runat="server" Width="100%">
                          
                     </telerik:RadComboBox> 
                </div>
             </div>
             <div class="col-sm-4">
                <div class="form-group">
                     <label>Division</label>
                     <telerik:RadComboBox ID="ddl_Org"  Skin="Simple"  runat="server" AllowCustomText="false" MarkFirstMatch="true" AutoPostBack="true" Width="100%">
                           
                      </telerik:RadComboBox> 
                </div>
             </div>  
         </div>     
         <div class="row">

             <div class="col-sm-4">
                <div class="form-group">
                    <label>Route</label>
                     <telerik:RadComboBox ID="ddl_Van"  Skin="Simple"  runat="server" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"  Width="100%">
                                                
                                                </telerik:RadComboBox>
                </div>
             </div>
             <div class="col-sm-4">
                <div class="form-group">
                    <label>Doc Type</label>
                    <telerik:RadComboBox ID="ddl_DocType"  Skin="Simple"  runat="server" AllowCustomText="false" MarkFirstMatch="true"  Width="100%" >
                                                <Items>
                                                <telerik:RadComboBoxItem Value="'C','I'" Text="Both" /> 
                                                <telerik:RadComboBoxItem Value="'I'" Text="Order" /> 
                                                <telerik:RadComboBoxItem Value="'C'" Text="Retruns" /> 
                                                </Items>
                                                </telerik:RadComboBox>
                </div>
             </div>
             <div class="col-sm-4">
                <div class="form-group">
                     <label>Location</label>
                      <telerik:RadComboBox ID="ddl_Location"  Skin="Simple" runat="server"  AutoPostBack="true" CheckBoxes="true"  OnClientLoad="onLoadLoc" EnableCheckAllItemsCheckBox="true" Width="100%">
                                                
                                                </telerik:RadComboBox>
                    <asp:Button ID="btnLoadCustomer" CssClass="btnInputGreen" TabIndex="5"  style="display:none;" 
                                                        runat="server" />
                </div>
             </div>  
        </div>  

            <div class="row">

             <div class="col-sm-4">
                <div class="form-group">
                    <label>Area</label>
                     <telerik:RadComboBox ID="ddl_Area"  Skin="Simple" runat="server"  CheckBoxes="true" EnableCheckAllItemsCheckBox="true"  Width="100%">
                                               
                                                </telerik:RadComboBox> 
                </div>
             </div>
             <div class="col-sm-4">
                <div class="form-group">
                    <label> Manufacture</label>
                    <telerik:RadComboBox ID="ddl_Agency"  Skin="Simple"  runat="server"  AutoPostBack="true" CheckBoxes="true"  OnClientLoad="onLoadAgen" EnableCheckAllItemsCheckBox="true"  Width="100%">
                                               
                                                </telerik:RadComboBox>
                                                <asp:Button ID="BtnLoadItem" CssClass="btnInputGreen" TabIndex="5"  style="display:none;" 
                                                        runat="server" />
                </div>
             </div>
               <div class="col-sm-4">
                <div class="form-group">
                    <label> Customer</label>
                     <telerik:RadComboBox ID="ddl_Customer"  Skin="Simple" runat="server" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"  Width="100%">
                                                
                                                </telerik:RadComboBox>  
                </div>
             </div>
        </div>

            <div class="row">

             <div class="col-sm-4">
                <div class="form-group">
                    <label>Item</label>
                      <telerik:RadComboBox ID="ddl_Product"  Skin="Simple" runat="server" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"  Width="100%">
                                             
                                                </telerik:RadComboBox> 
                </div>
             </div>
             <div class="col-sm-4">
                <div class="form-group">
                    <label>  </label>
                    
                </div>
             </div>
               <div class="col-sm-4">
                <div class="form-group">
                    <label>&nbsp;</label>
                      <asp:Button ID="btnSave"  CssClass ="btn btn-success"  TabIndex="5" OnClick="btnSave_Click"
                                                        runat="server" Text="Save" />
                                                 
                                                    <asp:Button ID="btnCancel" CssClass="btn btn-default" TabIndex="6" runat="server" CausesValidation="false"
                                                        Text="Clear" />
                </div>
             </div>
        </div>
         
	
 
  </ContentTemplate>
                 
                  </asp:UpdatePanel>
        
                  <asp:UpdateProgress ID="UpdateProgress1"   AssociatedUpdatePanelID="UpdatePanel1" runat="server" DisplayAfter="10">
     <ProgressTemplate>
     <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
     
         <img src="../assets/img/ajax-loader.gif" alt="Processing..." />            
           <span>Processing... </span>
       </asp:Panel>
           
       
         
    </ProgressTemplate>
            </asp:UpdateProgress>  
                    

        <asp:UpdatePanel ID="Panel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="searchblk">
                   <div class="row">
                      
                    <div class="col-sm-4">
                        <div class="row">
                            <div class="col-sm-6">
                             <div class="form-group">   <label>
                                                                From Date </label>
                                            
                                                                     <telerik:RadDatePicker ID="txt_fromDate" Width="100%" runat="server">
                                                                <DateInput   DisplayDateFormat="dd-MMM-yyyy">
                                                                </DateInput>
                                                                <Calendar ID="Calendar2" runat="server">
                                                                    <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                                </Calendar>
                                                            </telerik:RadDatePicker>
                                                                    </div>
                                </div>
                                       <div class="col-sm-6">
                                 <div class="form-group">
                                                                <label>To Date</label>
                                               

                                                                           <telerik:RadDatePicker ID="txt_ToDate"  Width="100%" runat="server">
                                                                <DateInput  DisplayDateFormat="dd-MMM-yyyy">
                                                                </DateInput>
                                                                <Calendar ID="Calendar1" runat="server">
                                                                    <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                                </Calendar>
                                                            </telerik:RadDatePicker>
                                                                    </div>
                                           </div>

                        </div>
                    </div>
                       <div class="col-sm-4">
                 <div class="form-group">
                                                <label>Status</label>
                                                 <telerik:RadComboBox  Skin="Simple"  runat="server" ID="ddl_Status" Width="100%">
                                                      <Items>
                                                    <telerik:RadComboBoxItem Value="-1" Text="All"></telerik:RadComboBoxItem>
                                                    <telerik:RadComboBoxItem Value="N" Text="New"></telerik:RadComboBoxItem>
                                                   <telerik:RadComboBoxItem Value="Y" Text="Processed"></telerik:RadComboBoxItem>
                                                    <telerik:RadComboBoxItem Value="X" Text="Cancelled"></telerik:RadComboBoxItem>
                                                           </Items>
                                                    </telerik:RadComboBox>  
                    </div>
                           </div>
                       <div class="col-sm-4">
                 <div class="form-group">
                     <label><br /></label>
                                                <asp:Button ID="btnFilter" runat="server" CausesValidation="False" CssClass ="btn btn-primary"
                                                    TabIndex="4" Text="Search" />
                                                    <asp:Button ID="btnClearFilter" runat="server" CausesValidation="False" CssClass ="btn btn-default" 
                                                    TabIndex="4" Text="Clear Filter" />
                          </div>
                           </div>                         
                        </div>  
                     </div> 
            <div class="table-responsive">
                      <table border="0" cellspacing="0" cellpadding="0" Width="100%"  >
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
                                                                    OnClick="btnDeleteAll_Click" CausesValidation="false" ImageUrl="~/images/delete-13.png"
                                                                    OnClientClick="return TestCheckBox()" />
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" Width="100px" CssClass="display-block"  />
                                                            <ItemTemplate>
                                                                <asp:HiddenField ID="HReport_ID" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"Report_ID") %>'/>
                                                                                                                                                                                              
                                                                <asp:CheckBox ID="chkDelete" runat="server"  Visible ='<%# Bind("DelIsVisible") %>'  />
                                                                <asp:ImageButton ToolTip="Delete" ID="btnDelete" Visible ='<%# Bind("DelIsVisible") %>' 
                                                                    OnClick="btnDelete_Click" runat="server" CausesValidation="false" ImageUrl="~/images/delete-13.png"
                                                                    OnClientClick="javascript:return confirm('Would you like to delete?');" />
                                                                
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" CssClass="display-block"  />
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
                  
              
            </div>

         
          
                                                
                                                
                                                 <asp:Button ID="BtnDetails" CssClass="btnInput" runat="Server" Style="display: none" />
                                    <ajaxToolkit:ModalPopupExtender BackgroundCssClass="modalBackground" ID="MPInfoRep"
                                        runat="server" PopupControlID="DetailInfoPnl" TargetControlID="BtnDetails"
                                        CancelControlID="btnCancelDetails">
                                    </ajaxToolkit:ModalPopupExtender>
                                    <asp:Panel ID="DetailInfoPnl" runat="server" Width="486px" CssClass="modalPopup" Style="display: none" >
                                   
                                     <div class="panelouterblk">
                                        <asp:Panel ID="DragPnl" runat="server" class="paneltitleblk">
                                            Report Criteria</asp:Panel>
                                                           
                                                     <asp:ImageButton ID="btn_close" runat="server"  ImageUrl="~/assets/img/close.jpg" CssClass="Closebtnimg"></asp:ImageButton>    

                                        <asp:HiddenField ID="HiddenField1" runat="server" Value="-1" />
                                        <asp:Label ID="Label1" runat="server" Text="" ForeColor="Maroon"></asp:Label>
                                        <div class="popupcontentblk">
	                                        <div class="row">
		                                        <div class="col-sm-5">
			                                        Year
		                                        </div>
		                                        <div class="col-sm-7">
			                                        <div class="form-group">
                                                        <asp:Label ID="lbl_year" runat="server" Text=""></asp:Label>
			                                        </div>
		                                        </div>
	                                        </div>
                                            <div class="row">
		                                        <div class="col-sm-5">
			                                        Division
		                                        </div>
		                                        <div class="col-sm-7">
			                                        <div class="form-group">
                                                        <asp:Label ID="lbl_Division" runat="server" Text=""></asp:Label> 
			                                        </div>
		                                        </div>
	                                        </div>
                                            <div class="row">
		                                        <div class="col-sm-5">
			                                        Month
		                                        </div>
		                                        <div class="col-sm-7">
			                                        <div class="form-group">
                                                        <asp:Label ID="lbl_months" runat="server" Text=""></asp:Label> 
			                                        </div>
		                                        </div>
	                                        </div>
                                            <div class="row">
		                                        <div class="col-sm-5">
			                                        Route
		                                        </div>
		                                        <div class="col-sm-7">
			                                        <div class="form-group">
                                                        <div style="max-height: 20px; overflow-y: auto;">
                                                            <asp:Label ID="lbl_Vans" runat="server" Text=""></asp:Label>
                                                        </div>
			                                        </div>
		                                        </div>
	                                        </div>
                                            <div class="row">
		                                        <div class="col-sm-5">
			                                        Doc Type
		                                        </div>
		                                        <div class="col-sm-7">
			                                        <div class="form-group">
                                                        <asp:Label ID="lbl_DocType" runat="server" Text=""></asp:Label> 
			                                        </div>
		                                        </div>
	                                        </div>
                                            <div class="row">
		                                        <div class="col-sm-5">
			                                        Area
		                                        </div>
		                                        <div class="col-sm-7">
			                                        <div class="form-group">
                                                        <div style="max-height: 20px; overflow-y: auto;">
                                                            <asp:Label ID="lbl_Area" runat="server" Text=""></asp:Label> 
                                                        </div>
			                                        </div>
		                                        </div>
	                                        </div>
                                            <div class="row">
		                                        <div class="col-sm-5">
			                                        Location
		                                        </div>
		                                        <div class="col-sm-7">
			                                        <div class="form-group">
                                                        <div style="max-height: 20px; overflow-y: auto;">
                                                            <asp:Label ID="lbl_Location" runat="server" Text=""></asp:Label>  
                                                        </div>
			                                        </div>
		                                        </div>
	                                        </div>
                                            <div class="row">
		                                        <div class="col-sm-5">
			                                        Customer
		                                        </div>
		                                        <div class="col-sm-7">
			                                        <div class="form-group">
                                                        <div style="max-height: 100px; overflow-y: auto;">
                                                            <asp:Label ID="lbl_Customer" runat="server" Text=""></asp:Label>
                                                        </div>
			                                        </div>
		                                        </div>
	                                        </div>
                                            <div class="row">
		                                        <div class="col-sm-5">
			                                        Manufacture
		                                        </div>
		                                        <div class="col-sm-7">
			                                        <div class="form-group">
                                                        <div style="max-height: 70px; overflow-y: auto;">
                                                            <asp:Label ID="lbl_Agency" runat="server" Text=""></asp:Label>
                                                        </div>
			                                        </div>
		                                        </div>
	                                        </div>
                                            <div class="row">
		                                        <div class="col-sm-5">
			                                        Item
		                                        </div>
		                                        <div class="col-sm-7">
			                                        <div class="form-group">
                                                       <div style="max-height: 100px; overflow-y: auto;">
                                                        <asp:Label ID="lbl_Items" runat="server" Text=""></asp:Label>
                                                       </div>
			                                        </div>
		                                        </div>
	                                        </div>
                                            <div class="row">
		                                        <div class="col-sm-12">
			                                        <div class="form-group text-center">
                                                        <asp:Button ID="btnCancelDetails" CssClass ="btn btn-default"  TabIndex="6" runat="server" CausesValidation="false" Text="Close" />
			                                        </div>
		                                        </div>
	                                        </div>
                                            <asp:Panel ID="Panel3" runat="server" Style="display: none" CssClass="overlay">
                                                <img alt="Processing..." src="../assets/img/ajax-loader.gif" />
                                                <span>Processing... </span>
                                            </asp:Panel>
                                        </div>
                                        
                                      </div>
                                    </asp:Panel>
                                     
                      
                                     
          </ContentTemplate>
         
        </asp:UpdatePanel> 
        
     <asp:UpdateProgress ID="UpdateProgress2"   AssociatedUpdatePanelID="Panel" runat="server" DisplayAfter="10">
    
    <ProgressTemplate>
     <asp:Panel ID="Panel19" CssClass="overlay" runat="server">
     
         <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="vertical-align:middle;" />            
           <span style="font-size:12px; font-weight:700px; color:#3399ff;">Processing... </span>
       </asp:Panel>
           
       
         
    </ProgressTemplate>
            </asp:UpdateProgress>                  
       
	<asp:Panel ID="Panel12" runat="server" Style="display: none" CssClass="overlay">
                                                        <img alt="Processing..." src="../assets/img/ajax-loader.gif" style="z-index: 10010; vertical-align: middle;" />
                                                        <span style="font-size: 12px; color: #3399ff;">Processing... </span>
                                                    </asp:Panel>
</asp:Content>