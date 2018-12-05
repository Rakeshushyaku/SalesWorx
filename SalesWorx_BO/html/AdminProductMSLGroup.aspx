<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="AdminProductMSLGroup.aspx.vb" Inherits="SalesWorx_BO.AdminProductMSLGroup" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


  <asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

 <asp:Content ID="Content1" ContentPlaceHolderID="FeaturedContent" runat="server">
     <style type="text/css">

.rcbSlide
{
	z-index: 100002 !important;
}
</style>

     <script language="javascript" type="text/javascript">
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
                 var myRegExp1 = /btnSaveAcc/
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
                var myRegExp1 = /btnSaveAcc/
                var AddString = postBackElement.id.search(myRegExp);
                var EditString = postBackElement.id.search(myRegExp1);

                if (AddString != -1 || EditString != -1) {

                }
                else {
                    $get('<%= Me.UpdateProgress1.ClientID %>').style.display = 'none';
                }
                postBackElement.disabled = false;
            }
        }



    </script>

    <script type="text/javascript">
        var TargetBaseControl = null;

        window.onload = function () {
            try {
                TargetBaseControl =
           document.getElementById('<%= Me.ClassUpdatePnl.ClientID %>');
            }
            catch (err) {
                TargetBaseControl = null;
            }
        }


        function alertCallBackFn(arg) {
          
        }

       


       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

      
  <h4>Manage Product MSL Group</h4>
                  <telerik:RadAjaxLoadingPanel  ID="AjaxLoadingPanel1" runat="server" >
             <div ID="ProgressPanel" class ="overlay"  >
     
         <img src="../assets/img/ajax-loader.gif" alt="Processing..." />      
       </div>
           </telerik:RadAjaxLoadingPanel>  

        <asp:Panel ID="Panel1" runat="server">      </asp:Panel> 
                         
 
    
                                         
                     <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>  
                      <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                           
 
                            <asp:UpdatePanel ID="TopPanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                <asp:Label ID="lblSelectedID" runat ="server" Visible ="false" ></asp:Label>
                                              <asp:Label ID="lblRemovedID" runat ="server" Visible ="false" ></asp:Label>
                                     <div class="row">
                                         <div class="col-sm-10 col-md-10 col-lg-10">
                                              <div class="row">
	                                       <div class="col-sm-4">
                                               <label> Organization</label>
                                          
                                               <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddl_org" Width ="100%" runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="True" EmptyMessage="Select Organisation" >
                                        </telerik:RadComboBox>
                                         </div>
                                         <div class="col-sm-4">
                                              <label> Group Name</label>
                                             <asp:Label ID="lblGroupId" runat="server" Visible ="false" Font-Bold ="true"  ></asp:Label>
                                             <asp:Label ID="lblGroupName" runat="server" Visible ="false" Font-Bold ="true"  ></asp:Label>
                                              <asp:TextBox ID="txtGroupName" runat="server" CssClass="inputSM" MaxLength="50"></asp:TextBox>
                                           
                                             </div>
                                             </div>
                                        
                                         </div>
                                          <div class="col-sm-2 col-md-2 col-lg-2">
                                              <label> &nbsp;</label>
                                             <asp:Button ID="Btn_back" runat="server" CausesValidation="False" CssClass ="btn btn-sm btn-block btn-default"
                                                    TabIndex="4" Text="Back" />
                                                
                                             </div>
                                        </div>
                                     <br />
                                              <br />
                                               <div class="row">
                                                      <div class="col-sm-4">
                                                    <label> Filter</label>
                                                    
                                                       <telerik:RadTextBox runat ="server" ID="txtFilter" EmptyMessage ="By Item Code/Description" Skin ="Simple"  Width ="100%"   ></telerik:RadTextBox>
                                                       
                                                     </div>
                                                     <div class="col-sm-8">
                                                         <label>&nbsp;</label>
                                                       <asp:Button ID="btnFilter" runat="server" CausesValidation="False" CssClass ="btn btn-success"
                                                    TabIndex="4" Text="Search" /> &nbsp;
                                                           <asp:Button ID="btnReset" runat="server"   CssClass="btn btn-warning"
                                                    TabIndex="5" Text="Reset"/>
                                                            <asp:Button ID="btnImportWindow" runat="server" CssClass="btn btn-primary" Text="Import" TabIndex ="12" />
                                                    <asp:Button ID="btnExport"  runat="server" CssClass ="btn btn-warning" Text="Export" TabIndex ="11" />
                                                    <asp:LinkButton id="btndownloadTemplate" Text="Download Template" OnClick="btndownloadTemplate_Click" runat="server"  Font-Size="Medium"  Font-Underline="true"/>
                                                    </div>
                                                   </div>
                                             
                                              
                                                 <table width="100%">
                                        <tr>
                                            <td width="49%">
                                                <asp:Label ID="lblProdAvailed" Font-Bold="true" ForeColor ="#337AB7" runat="server" Text=""></asp:Label>
                                            </td>
                                            <td width="2%">
                                            </td>
                                            <td width="49%">
                                                <asp:Label Font-Bold="true" ID="lblProdAssign" ForeColor ="#337AB7" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="49%">
                                              
                                              

                                                  <telerik:RadListBox ID="lstDefault"  ToolTip ="Press CTRl key for multiple selection"   runat ="server" 
                                                        Width="100%" Height ="290px"   SelectionMode ="Multiple"   >
          
            

        </telerik:RadListBox>

                                            </td>


                                             <td  width="2%">
                <table border ="0" width="2%">
                   
                    <tr><td><asp:ImageButton runat ="server" ID="imgRemoveSelected" BorderStyle ="None" 
                         ToolTip ="Move selected item to right"   ValidationGroup="valsum"  OnClick ="imgAddSlected_Click"   ImageUrl="~/Images/arrowSingleRight.png" /></td></tr>
                   
                    <tr><td>  
                      <asp:ImageButton runat ="server" ID="imgAddSlected"   ValidationGroup="valsum"  BorderStyle ="None"  ToolTip ="Move selected item to left" OnClick ="imgRemoveSlected_Click"
                              ImageUrl="~/Images/arrowSingleLeft.png" /></td></tr>
                    <tr>
                        <td >
                  <asp:ImageButton runat ="server" ID="imgMoveAllLeft" BorderStyle ="None"    ValidationGroup="valsum"
                       ToolTip ="Move all item to left" OnClick ="imgMoveAllRight_Click" ImageUrl="~/Images/doubleRight.png" /></td>
                        </tr> 
                    <tr>
                        <td >
                  <asp:ImageButton runat ="server" ID="imgMoveAllRight"    ValidationGroup="valsum"  BorderStyle ="None"  OnClick ="imgMoveAllLeft_Click"  ToolTip ="Move all Item to right" ImageUrl="~/Images/doubleLeft.png" /></td>
                        </tr> 
                    </table> 
                  </td> 

                                       
                                            <td width="49%">
                                                

                                                  <telerik:RadListBox ID="lstSelected"    ToolTip ="Press CTRl key for multiple selection"      runat ="server"  
                                                       Width="100%" Height ="290px"   SelectionMode ="Multiple"   >
          
            

        </telerik:RadListBox>
                                            </td>
                                        </tr>
                                    </table>
                                      <telerik:RadWindow ID="MPEImport" Title= "Import MSL Group Items" runat="server" Skin="Windows7" Behaviors="Move,Close"
                                                 Height="170px" Width ="490px"  ReloadOnShow="false" VisibleStatusbar="false" Overlay="true" Modal="true">
                                              <ContentTemplate>
                                                        <asp:HiddenField ID="HFV" runat="server" Value="-1" />
                                                  <div class="popupcontentblk">
                    
                                                <p><asp:Label runat ="server" ID="Label6" ForeColor ="Red"  Text =""></asp:Label></p>
                                                <p><asp:Label runat ="server" ID="lblUpMsg" ForeColor ="Red"></asp:Label></p>
                  
                                                <div class="row">
		                                        <div class="col-sm-5">
			                                        <label>Select a File</label>
		                                        </div>
		                                        <div class="col-sm-7">
			                                        <div class="form-group">
                                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="conditional">
                                                            <ContentTemplate><asp:FileUpload ID="ExcelFileUpload" runat="server" /></ContentTemplate>
                                                        </asp:UpdatePanel> 
                                                    </div>
                                                </div>
                                                </div>  
                                                <div class="row">
		                                        <div class="col-sm-5"></div>
		                                        <div class="col-sm-7">
			                                        <div class="form-group">
                                                        <asp:Button ID="btnImport" runat="server" Text="Import" CssClass ="btn btn-warning" /> 
                                                              <asp:Button ID="btnCancelImport"  Visible ="false"  CssClass ="btn btn-default"  TabIndex="5" runat="server"
                                                                                    CausesValidation="false" Text="Cancel" />
                                                                  <asp:Button ID="DummyImBtn" style="display:none"  runat="server" Text="Reimport" />
                                                           <asp:Button ID="BtnReimport" runat="server" Text="Reimport"  Visible ="false" 
                                                                 CssClass ="btn btn-primary" />
                                                           <asp:Button ID="DummyReimBtn" style="display:none"  runat="server" Text="Reimport" />
                                                            <span> <asp:LinkButton ID="lbLog" 
                                                              ToolTip ="Click here to download the uploaded log" runat ="server"
                                                               Text ="View Log" Visible="false" ></asp:LinkButton></span>
                                                    </div>
                                                </div>
                                                </div>   
                      
		
         
                                                <div>
                                                        <asp:UpdatePanel runat="server" ID="UpPanel">
                                                            <Triggers>
                                                              <asp:AsyncPostBackTrigger ControlID="DummyImBtn" EventName="Click" />
	                        <asp:AsyncPostBackTrigger ControlID="DummyReimBtn" EventName="Click" />
	
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                            
                                                </div>

                        
                        <div class="table-responsive">
                       
                       
                         <asp:GridView Width="100%" ID="dgvErros"   runat="server" EmptyDataText="No items to display"
                                                        EmptyDataRowStyle-Font-Bold="true"  AutoGenerateColumns="False" Visible="false" 
                                                        AllowPaging="true" AllowSorting="false"  PageSize="15" CellPadding="0" CellSpacing="0" CssClass="tablecellalign"  >
                                                        
                                                        <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
                                                        <EmptyDataRowStyle Font-Bold="True" />
                                                        <Columns>
                                                     
                                                             <asp:BoundField HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="RowNo"
                                                                HeaderText="Row No">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>
                                                           
                                                       
                                                          
                                                             <asp:BoundField HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="LogInfo"
                                                                HeaderText="Log Info">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>
                                                             
                                                        
                                                          
                                                        </Columns>
                                                        <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
                                                    </asp:GridView>
                        
                         
                    
                        </div>
                    
                                                      </div>
                                                </ContentTemplate>
                               
                            
                                                    </telerik:RadWindow> 
                                </ContentTemplate>
                                
                            </asp:UpdatePanel>
                        
                            <asp:UpdatePanel ID="ClassUpdatePnl" runat="server" UpdateMode="conditional">
                                <ContentTemplate>
                                    
                                                                     
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnFilter" EventName="Click" />
                                       <asp:PostBackTrigger  ControlID="btnExport"   />
                                       <asp:PostBackTrigger  ControlID="btndownloadTemplate"   />
                             
                        
                                </Triggers>
                            </asp:UpdatePanel>
                         
                 </ContentTemplate>
                 </asp:UpdatePanel>
                <asp:UpdatePanel ID="Panel" runat="server" UpdateMode="Conditional" >
                             <Triggers>
           
        <asp:AsyncPostBackTrigger ControlID="btnImportWindow" EventName="Click" />
            
	
        </Triggers>
                                <ContentTemplate>
  
                                    </ContentTemplate>
         </asp:UpdatePanel>
         
                
                <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="ClassUpdatePnl"
                    runat="server">
                    <ProgressTemplate>
                          <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                            <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                            <span style="font-size: 12px; color: #3399ff;">Processing... </span>
                        </asp:Panel>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <br />
                <br />
             
</asp:Content>
