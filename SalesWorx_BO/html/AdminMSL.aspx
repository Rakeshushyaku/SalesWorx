<%@ Page Title="MSL Management" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master"
    CodeBehind="AdminMSL.aspx.vb" Inherits="SalesWorx_BO.AdminMSL" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">

    <script language="javascript" type="text/javascript">


        var prm = Sys.WebForms.PageRequestManager.getInstance();

        prm.add_initializeRequest(InitializeRequest);
        prm.add_endRequest(EndRequest);
        var postBackElement;
        function InitializeRequest(sender, args) {

            if (prm.get_isInAsyncPostBack())
                args.set_cancel(true);
            postBackElement = args.get_postBackElement();
            $get('<%=UpdateProgress1.ClientID %>').style.display = 'block';
            postBackElement.disabled = true;
        }

        function EndRequest(sender, args) {
            $get('<%=UpdateProgress1.ClientID %>').style.display = 'none';
            postBackElement.disabled = false;
        }

        function alertCallBackFn(arg) {
           
        }

    </script>
    </asp:Content>
 <asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

     <h4>MSL Management</h4>
      <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
     </telerik:RadWindowManager>
     
                            
 
                        
                            <asp:UpdatePanel runat="server" ID="TopPanel" UpdateMode ="Conditional" >
        <ContentTemplate>

            <div class="row">
                <div class="col-sm-4">
                        <div class="form-group">  
                            <label><asp:Label ID="lblLable" runat="server"   Text="Organization"></asp:Label></label>
                                    <telerik:RadComboBox ID="ddOraganisation" Skin="Simple"   Width="100%" AutoPostBack="true" runat="server" CssClass="inputSM"></telerik:RadComboBox>
                               </div>
                    </div>
                <div class="col-sm-4">
            <div class="form-group">  <label>
                                    <asp:Label ID="Label1" runat="server"   Text="Van/FSR"></asp:Label></label>
                                    <telerik:RadComboBox ID="ddlSalesRep" Skin="Simple"   Width="100%" AutoPostBack="true" runat="server" CssClass="inputSM">
                                    </telerik:RadComboBox>
                               </div>
                    </div>
                <div class="col-sm-4">
                                <div class="form-group">
                               <label>Filter By item Code  </label> 
                          <telerik:RadTextBox runat ="server" ID="txtFilter" EmptyMessage ="Filter By Item Code" Skin ="Simple"  Width ="100%"   ></telerik:RadTextBox>
                                   
                                      
                        </div> 
                    </div>
                </div> 
                <div class="row">
                <div class="col-sm-12">
                    <div class="form-group">
                <telerik:RadButton ID="btnFilter" Skin ="Simple"    runat="server" Text="Search" CssClass ="btn btn-primary" />
                                     <telerik:RadButton ID="btnClearFilter" Skin ="Simple"    runat="server" Text="Clear Filter" CssClass ="btn btn-warning"  />
                        <asp:Button ID="btnImportWindow" runat="server" CssClass="btn btn-primary" Text="Import" TabIndex ="12" />
                                                    <asp:Button ID="btnExport"  runat="server" CssClass ="btn btn-warning" Text="Export" TabIndex ="11" />
                                       <telerik:RadButton ID="btnReset"  Skin ="Simple"     runat="server" Text="Reset" CssClass ="btn btn-default" />
                        
                             <asp:Label ID="lblSelectedID" runat ="server" Visible ="false" ></asp:Label>
                                              <asp:Label ID="lblRemovedID" runat ="server" Visible ="false" ></asp:Label>
                         </div>
                    </div> 
                    </div>

                              
                         <table border="0" cellspacing="0" cellpadding="0" id="tableform" width="100%">
                            <tr>
                                <td>
                                    <table border="0" cellspacing="0" cellpadding="0" width="100%">
                                        <tr>
                                            <td>
                                                <p><asp:Label ID="lblProdAvailed" Font-Bold="true" runat="server" Text=""></asp:Label></p>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <p><asp:Label Font-Bold="true" ID="lblProdAssign" runat="server" Text=""></asp:Label></p>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="48%">
                                                 <telerik:RadListBox Rows="20" Height="350" SelectionMode="Multiple"
                                                 
                                                     Width="100%" ID="lstDefault" runat="server">
                                                </telerik:RadListBox>
                                            </td>
                                            <td style="padding:10px; vertical-align:middle;" width="50px">
                                                  <table border="0" cellspacing="0" cellpadding="0" width="100%">
                   
                                                            <tr><td style="padding:3px 0;"><asp:ImageButton runat ="server" ID="btnAdd" BorderStyle ="None" 
                                                                 ToolTip ="Move selected item to right"   ValidationGroup="valsum"  OnClick ="btnAdd_Click"   ImageUrl="~/Images/arrowSingleRight.png" /></td></tr>
                   
                                                            <tr><td style="padding:3px 0;">  
                                                              <asp:ImageButton runat ="server" ID="btnRemove"   ValidationGroup="valsum"  BorderStyle ="None"  ToolTip ="Move selected item to left" OnClick ="btnRemove_Click"
                                                                      ImageUrl="~/Images/arrowSingleLeft.png" /></td></tr>
                                                            <tr>
                                                                <td style="padding:3px 0;">
                                                          <asp:ImageButton runat ="server" ID="btnRemoveAll" BorderStyle ="None"    ValidationGroup="valsum"
                                                               ToolTip ="Move all item to left"  ImageUrl="~/Images/doubleRight.png" /></td>
                                                                </tr> 
                                                            <tr>
                                                                <td style="padding:3px 0;">
                                                          <asp:ImageButton runat ="server" ID="btnAddAll"    ValidationGroup="valsum"  BorderStyle ="None"  
                                                                  ToolTip ="Move all Item to right" ImageUrl="~/Images/doubleLeft.png" /></td>
                                                                </tr> 
                                                            </table> 

                                             
                                            </td>
                                            <td width="48%">
                                              <telerik:RadListBox Rows="20"  Height="350" SelectionMode="Multiple" Width="100%" ID="lstSelected" runat="server">
                                                </telerik:RadListBox>
                                            </td>
                                        </tr>
                                    </table>
                           </td>
                                    </tr>
                        </table>
                         </ContentTemplate>
     </asp:UpdatePanel> 
                      
              <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode ="Conditional" >
                             <Triggers>
                             <asp:PostBackTrigger  ControlID="btnExport"   />
                            
                              
                          <asp:PostBackTrigger  ControlID="btnImportWindow"  />
                        
                            </Triggers>          
                                  
                                <ContentTemplate>
                                
                                      
                                
                                
                                
                                
                        
                        
                                
                                </ContentTemplate>
                            </asp:UpdatePanel>
               
     <asp:UpdatePanel ID="Panel" runat="server" UpdateMode="Conditional" >
                             <Triggers>
           
      
            
	
        </Triggers>
                                <ContentTemplate>
    <telerik:RadWindow ID="MPEImport" Title= "Import MSL" runat="server" Skin="Windows7" Behaviors="Move,Close"
                                                AutoSize="true" MinHeight="160px" ReloadOnShow="false" VisibleStatusbar="false" Overlay="true" Modal="true">
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
                                                              <asp:Button ID="btnCancelImport"  CssClass ="btn btn-default"  TabIndex="5" runat="server"
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
         
    
  
           <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="TopPanel"
                    runat="server">
                    <ProgressTemplate>
                        <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                            <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                            <span style="font-size: 12px; color: #3399ff;">Processing... </span>
                        </asp:Panel>
                    </ProgressTemplate>
                </asp:UpdateProgress>
              
         
</asp:Content>
