<%@ Page Title="Customer-Level Discount Limits" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master" CodeBehind="CustomerOrderlvlDiscount.aspx.vb" Inherits="SalesWorx_BO.CustomerOrderlvlDiscount" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script type="text/javascript" >
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
    function DisableValidation() {
        //            Page_ValidationActive = false;
        //            return true;

    }
    function NumericOnly(e) {

        var keycode;

        if (window.event) {
            keycode = window.event.keyCode;
        } else if (e) {
            keycode = e.which;
        }
        if ((parseInt(keycode) >= 48 && parseInt(keycode) <= 57) || parseInt(keycode) == 8 || parseInt(keycode) == 46 || parseInt(keycode) == 0)
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
    </script>

   <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
                <tr>
                    <td id="contentofpage" width="100%" height="76%" class="topshadow">
                        <div class="pgtitileposition">
                            <span class="pgtitile3">Customer-Level Discount Limits</span></div>  
                   
                            <asp:UpdatePanel ID="ClassUpdatePnl1" runat="server" UpdateMode="conditional">
                                <ContentTemplate>
                                <table  width="auto" border="0" cellspacing="6" cellpadding="6" id="tableformNrml">
                                <tr>
                                <td width="105" class="txtSMBold">Organization :</td><td ><asp:DropDownList CssClass="inputSM" ID="ddl_org"  Width ="200px"
                    runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="true"></asp:DropDownList>
                    
                     <asp:Button ID="btnExport" 
                                                                                                              runat="server" CssClass="btnInputOrange" Text="Export" TabIndex ="11" />
                                                        <asp:Button ID="btnImportWindow" runat="server" CssClass="btnInputGreen" Text="Import" TabIndex ="12" />
                                                   
                    </td></tr>
                    
                    
                                <tr>
                                <td colspan="2">
                              <asp:Panel ID="pnl1" runat ="server" BorderStyle =Solid BorderColor ="LightGray" GroupingText ="" >
            <table width="auto" border="0" cellspacing="0" cellpadding="0" Width="100%">
                    <tr>
                        <td style="padding: 6px 12px">
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode ="Conditional" >
                            <Triggers>
                             <asp:PostBackTrigger  ControlID="btnExport"   />
                             <asp:PostBackTrigger  ControlID="lbLog"   />
                              
                          <asp:PostBackTrigger  ControlID="btnImportWindow"  />
                        
                            </Triggers>
                                <ContentTemplate>
                                
                                        <table border="0" width ="100%" cellspacing="5" cellpadding="5">
                                         <tr>
                                <td class="txtSMBold"  >
                                  
                                  
                                  </td>
                                    <td class="txtSMBold" >
                                       </td>
                            
                               <td colspan ="2">
                                
                               
                               </td>
                                </tr>   
                            
                                            <tr>
                                                <td   class="txtSMBold" >
                                                   Customer:
                                                </td>
                                                <td  >
                                             
    <telerik:RadComboBox ID="ddlCustomer" Filter="Contains"  EmptyMessage ="Please enter Customer code/Name"
                                                   EnableLoadOnDemand="True" TabIndex="2"  Sort ="Ascending"   AutoPostBack ="true" 
                                                    MinimumFilterLength="1"  runat="server"
                                                    Height="200px" Width="300px">
                                                </telerik:RadComboBox>
                                                <asp:Label runat ="server" ID="lblLineID" Visible ="false" ></asp:Label>
                                                </td>
                                                    <td   class="txtSMBold" >
                                                    Discount Min Value(%) :
                                                </td>
                                                <td  >
                                                      <asp:TextBox ID="txtMinValue" runat="server" TabIndex ="7"
                                                            CssClass="inputSM" Width="70px"></asp:TextBox>
                                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" 
                                                            FilterType="Numbers,Custom"  ValidChars ="." TargetControlID="txtMinValue">
                                                        </ajaxToolkit:FilteredTextBoxExtender>
                                                </td>
                                             
                                            </tr>
                                                       <tr>
                 <td  class="txtSMBold">
                  Minimum Order Value :</td>
                 <td  >
                 <asp:TextBox ID="txtvalue" runat="server"  TabIndex ="5" 
                                                            CssClass="inputSM" Width="70px"></asp:TextBox>
                                                        <ajaxToolkit:FilteredTextBoxExtender ID="FTEOrdQty" runat="server"   ValidChars ="."
                                                            FilterType="Numbers,Custom" TargetControlID="txtvalue">
                                                        </ajaxToolkit:FilteredTextBoxExtender></td>
                        
                        <td class="txtSMBold"   >
                                                    Discount Max Value(%) :
                                                </td>
                                                <td >
                                                    <asp:TextBox ID="txtMaxValue" runat="server" TabIndex ="7"
                                                            CssClass="inputSM" Width="70px"></asp:TextBox>
                                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" 
                                                            FilterType="Numbers,Custom"  ValidChars ="." TargetControlID="txtMaxValue">
                                                        </ajaxToolkit:FilteredTextBoxExtender>
                                                </td>
               
                        
                        </tr>
                                           
                                       
                                       
                                             
                                               
                                   
                                            
                                                  
                                                <tr>
                                               <td></td>
                                                 <td colspan="3"  >&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                                                              <asp:Button ID="btnAddItems" 
                                                                                                              runat="server" CssClass="btnInputBlue" Text="Add" TabIndex ="8" />
                                                        <asp:Button ID="btnClear" runat="server" CssClass="btnInputRed" Text="Reset" TabIndex ="9" />
                                                   
                                                
                                                
                                                    </td>
                                                   
                                                </tr>
                                             
                                        
                                          
                                    
                                        </table>
                                    
                    
                                
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    
                </table>
                         <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
                         <ContentTemplate>
                        
                                   
                                        <table border="0"  cellspacing="0" cellpadding="0" width>
                                            <tr>
                                                <td>
                                                    
                                                    <asp:GridView Width="100%" ID="dgvItems"   runat="server" EmptyDataText="No items to display"
                                                        EmptyDataRowStyle-Font-Bold="true"  AutoGenerateColumns="False"
                                                        AllowPaging="true" AllowSorting="true"  PageSize="25" CellPadding="0" CellSpacing="0" CssClass="tablecellalign"  >
                                                        
                                                        <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
                                                        <EmptyDataRowStyle Font-Bold="True" />
                                                        <Columns>
                                                     
                                                            <asp:BoundField HeaderStyle-HorizontalAlign="Left" SortExpression ="Customer_no" DataField="Customer_no" HeaderText="Customer No"
                                                                NullDisplayText="N/A">
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                      
                                                           
                                                            <asp:BoundField HeaderStyle-HorizontalAlign="Left" SortExpression ="Customer_Name" DataField="Customer_Name" HeaderText="Customer Name"
                                                                NullDisplayText="N/A">
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            
                                                      
                                                            
                                                         
                                                            <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="MinOrderValue"
                                                                HeaderText="Minimum Order Value" SortExpression ="minordervalue"  DataFormatString="{0:F2}">
                                                                   <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                                                <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                                            </asp:BoundField>
                                                              <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="MinDisc"
                                                                HeaderText="Min.Discount(%)"   SortExpression ="MinDisc" DataFormatString="{0:F2}">
                                                                 <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                                                <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                                            </asp:BoundField>
                                                             <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="MaxDisc"
                                                                HeaderText="Max.Discount (%)" SortExpression ="MaxDisc" DataFormatString="{0:F2}">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>
                                                         
                                                            <asp:TemplateField HeaderText="">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ToolTip="Edit" ID="btnEdit"   runat="server" CommandName="EditRecord"
                                                                        CausesValidation="false"   ImageUrl="~/images/edit-13.png"   />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                  <asp:Label ID="lblCustomer_ID" Visible ="false"  runat="server" Text='<%# Bind("Customer_ID") %>'></asp:Label>
                                                                  <asp:Label ID="lblSite_Use_ID" Visible ="false"  runat="server" Text='<%# Bind("Site_Use_ID") %>'></asp:Label>
                                                                    <asp:ImageButton ToolTip="Delete" ID="btnCan" runat="server" CommandName="DeleteRecord"
                                                                        CausesValidation="false"  ImageUrl="~/images/delete-13.png" OnClientClick="javascript:return confirm('Would you like to delete the selected item?');" />
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
                         </ContentTemplate>
                         </asp:UpdatePanel>   
                             
         </asp:Panel>  
                                </td>
                                </tr>
                                </table>
                               
     
         
           <div>
                                                <asp:Button ID="btnHidden" runat="Server" Style="display: none" />
                                                <ajaxToolkit:ModalPopupExtender ID="MpInfoError" runat="server" BackgroundCssClass="modalBackground"
                                                    TargetControlID="btnHidden" PopupControlID="pnlConfirm" CancelControlID="btnClose"
                                                    Drag="true" />
                                                <asp:Panel ID="pnlConfirm"   Width="400" runat="server" CssClass="modalPopup" Style="cursor: move;
                                                    background-color: #3399ff; text-align: center; border: solid 1px #3399ff; color: White;
                                                    padding: 3px; display: none;">
                                                    <table id="tableinPopupErr" cellpadding="10" style="background-color: White;"  width="100%">
                                                        <tr align="center">
                                                            <td align="center" style="cursor: move; background-color: #3399ff; text-align: center;
                                                                border: solid 1px #3399ff; color: White; padding: 3px">
                                                                <asp:Label ID="lblinfo" runat="server" Font-Size ="13px"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" style="text-align: center">
                                                                <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="List" />
                                                                <asp:Label ID="lblMessage" runat="server" Font-Size ="13px"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" style="text-align: center;">
                                                                            <asp:Button ID="btnClose" runat="server" Text="Ok" CssClass="btnInputBlue" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                                             
                                           </div>
             
           
               
              
                                        
                                         
                                                   <div>
                                                <asp:Button ID="BtnImportHidden" runat="Server" Style="display: none" />
                                                <ajaxToolkit:ModalPopupExtender ID="MPEImport" runat="server" BackgroundCssClass="modalBackground"
                                                    TargetControlID="BtnImportHidden" PopupControlID="Pnl_import" CancelControlID="btnCancelImport"
                                                    Drag="true" />
                                                <asp:Panel ID="Pnl_import" Width="400" runat="server" CssClass="modalPopup" Style="cursor: move;
                                                    background-color: #3399ff; text-align: center; border: solid 1px #3399ff; color: White;
                                                    padding: 3px; display: none;">
                                                    
                                                   <asp:Panel ID="Dragpnl2" runat="server" Style="cursor: move;font-family:Verdana,Tahoma; font-weight:bold; font-size:13px;
                          background-color:  #337AB7  ;
                        text-align: center; border: solid 1px  #337AB7  ; color: White; padding: 3px"  >
                        Import Order level Customer Discount</asp:Panel>
                    <asp:HiddenField ID="HFV" runat="server" Value="-1" />
                    <table   border="0" cellspacing="2" cellpadding="2"  width="100%">
                  
                  <tr>
                  <td colspan ="2">
                  <asp:Label runat ="server" ID="Label6" CssClass ="txtSM" Font-Size ="12px" ForeColor ="Blue"  Text =""></asp:Label>
                  
                  </td>
                  </tr>      
                       
		 <tr>
    <td class ="txtSMBold">Select a File :</td>
    <td  style ="color:Black;"> <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="conditional">
                                                        <ContentTemplate><asp:FileUpload ID="ExcelFileUpload" runat="server" /></ContentTemplate>
                                                    </asp:UpdatePanel>    
          
         </td>
       
          </tr>
          <tr><td colspan ="2"><br /></td></tr>
          <tr>
          <td>
           
             
          </td>
          <td>
           <asp:Button ID="btnImport" runat="server" Text="Import" CssClass="btnInputGrey" /> <asp:Button ID="btnCancelImport" CssClass="btnInputRed" TabIndex="5" runat="server"
                                    CausesValidation="false" Text="Close" />
                  <asp:Button ID="DummyImBtn" style="display:none"  runat="server" Text="Reimport" />
           <asp:Button ID="BtnReimport" runat="server" Text="Reimport"  Visible ="false" 
                 CssClass="btnInputBlue" />
           <asp:Button ID="DummyReimBtn" style="display:none"  runat="server" Text="Reimport" />
            <span style ="text-decoration: underline !important;Color:#337AB7;"> <asp:LinkButton ID="lbLog" Font-Bold ="true" Font-Size ="13px"   ForeColor  ="#337AB7"
              ToolTip ="Click here to download the uploaded log" runat ="server"
               Text ="View Log" Visible="false" ></asp:LinkButton></span>
           </td>
          </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel runat="server" ID="UpPanel">
                                    <Triggers>
                                      <asp:AsyncPostBackTrigger ControlID="DummyImBtn" EventName="Click" />
	<asp:AsyncPostBackTrigger ControlID="DummyReimBtn" EventName="Click" />
	
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr><td colspan ="2">
                        <asp:Label runat ="server" ID="lblUpMsg" CssClass ="txtSM" Font-Size ="12px" ForeColor ="Green" Font-Bold ="true" > </asp:Label></td></tr>
                        <tr>
                        <td colspan="2">
                       
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
                        
                         
                        </td>
                        </tr>
                    </table>
                                                </asp:Panel>
                                             </div>
                                             
                                             
                                           
                                </ContentTemplate>
                                <Triggers>
           
      
            <asp:PostBackTrigger ControlID="btnImport" /> 
	
        </Triggers>
                            </asp:UpdatePanel>
                            
                    
             
                       </td>
                       </tr>
                       </table>
    
    <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="ClassUpdatePnl1"
        runat="server">
        <ProgressTemplate>
            <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                <img src="../images/Progress.gif" alt="Processing..." style="padding-left: 400px" />
                <span style="font-size: 12px; color: #666;">Processing... </span>
            </asp:Panel>
        </ProgressTemplate>
    </asp:UpdateProgress>
    
 
</asp:Content>
