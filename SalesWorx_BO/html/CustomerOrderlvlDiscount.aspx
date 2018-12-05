<%@ Page Title="Customer-Level Discount Limits" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="CustomerOrderlvlDiscount.aspx.vb" Inherits="SalesWorx_BO.CustomerOrderlvlDiscount" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">

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
    </script>
    <script type="text/javascript">
        $(window).resize(function () {
            var win = $find('<%= MPEImport.ClientID %>');
            if (win) {
                if (!win.isClosed()) {
                    win.center();
                }
            }

        });
    </script>
    </asp:Content>
     <asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
         <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>
   <h4>Customer-Level Discount Limits</h4> 
                   
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>

                                    <div class="row">
                                    
                                         <div class="col-sm-6">
                                            <div class="form-group">
                                                <label>Organization</label>
                                     <telerik:RadComboBox Skin="Simple"  ID="ddl_org"  Width ="100%"
                    runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="true"></telerik:RadComboBox>
                    
                                                </div>
                                             </div>
                                              <div class="col-sm-6">
                                                <div class="form-group">
                                                    <label>&nbsp;</label>
                     <asp:Button ID="btnExport" 
                                                                                                              runat="server" CssClass ="btn btn-warning"  Text="Export" TabIndex ="11" />
                                                    &nbsp;    <asp:Button ID="btnImportWindow" runat="server" CssClass="btn btn-warning"  Text="Import" TabIndex ="12" />
                                                     <asp:Button ID="btndownloadTemp" runat="server" CssClass ="btn btn-primary" Text="Download Template" TabIndex ="11" />
                                                   
                   
                    
                         </div>
                                                    </div>
                                         
                                        </div>
                            
                              <asp:Panel ID="pnl1" runat ="server"  GroupingText ="" >
                                  <asp:UpdatePanel ID="ClassUpdatePnl1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
             <div class="row">
                 <div class="col-sm-3">
                                            <div class="form-group">
                                                <label>
                                                   Customer</label>
                                                 
                                             
    <telerik:RadComboBox ID="ddlCustomer" Skin="Simple" Filter="Contains"  EmptyMessage ="Please enter Customer code/Name"
                                                   EnableLoadOnDemand="True" TabIndex="2"  Sort ="Ascending"   AutoPostBack ="true" 
                                                    MinimumFilterLength="1"  runat="server"
                                                     Width="100%">
                                                </telerik:RadComboBox>
                                                <asp:Label runat ="server" ID="lblLineID" Visible ="false" ></asp:Label>
                                                </div>
                 </div>
                  <div class="col-sm-2">
                                            <div class="form-group">
                                                <label>
                 
                  Transaction Type </label>
                  
                <telerik:RadComboBox CssClass ="inputSM" ID="ddl_transactiontype"  runat="server" TabIndex ="3" Skin="Simple">
                    <Items>
                        <telerik:RadComboBoxItem Value ="0" Selected="True" Text="All" />
                        <telerik:RadComboBoxItem Value ="CASH" Text="CASH" />
                        <telerik:RadComboBoxItem Value ="CREDIT" Text="CREDIT" />
                    </Items></telerik:RadComboBox>
                                               </div>
                 </div>
                
                 <div class="col-sm-2">
                                            <div class="form-group">
                                                <label>
                 
                  Minimum Order Value </label>
                  
                 <asp:TextBox ID="txtvalue" runat="server"  TabIndex ="5" 
                                                            CssClass="inputSM" Width="100%"></asp:TextBox>
                                                        <ajaxToolkit:FilteredTextBoxExtender ID="FTEOrdQty" runat="server"   ValidChars ="."
                                                            FilterType="Numbers,Custom" TargetControlID="txtvalue">
                                                        </ajaxToolkit:FilteredTextBoxExtender> </div>
                 </div>
                                                <div class="col-sm-2">
                                            <div class="form-group">
                                                <label>
                                                    Discount Min Value(%) 
                                                </label>
                                                      <asp:TextBox ID="txtMinValue" runat="server" TabIndex ="7"
                                                            CssClass="inputSM" Width="100%"></asp:TextBox>
                                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" 
                                                            FilterType="Numbers,Custom"  ValidChars ="." TargetControlID="txtMinValue">
                                                        </ajaxToolkit:FilteredTextBoxExtender>
                                                 </div>
                 </div>
                                                
                                                <div class="col-sm-2">
                                            <div class="form-group">
                                                <label>Discount Max Value(%)</label>
                                                
                                                    <asp:TextBox ID="txtMaxValue" runat="server" TabIndex ="7"
                                                            CssClass="inputSM" Width="100%"></asp:TextBox>
                                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" 
                                                            FilterType="Numbers,Custom"  ValidChars ="." TargetControlID="txtMaxValue">
                                                        </ajaxToolkit:FilteredTextBoxExtender>
                                                 </div>
                 </div>
                                                 
                          </div>
                                                     
                                       
                                       
                                             
                                               
                                   
                                            
                                                  
                                              <div class="row">
                    <div class="col-sm-12">
                        <div class="form-group">
                                                                                                              <asp:Button ID="btnAddItems" 
                                                                                                              runat="server" CssClass="btn btn-success" Text="Add" TabIndex ="8" />
                                                        <asp:Button ID="btnClear" runat="server" CssClass ="btn btn-default"  Text="Reset" TabIndex ="9" />
                                                   
                                                
                                                
                                                   </div>
                    </div>
                </div>
                                  
                        <div class="table-responsive">    
                         <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
                         <ContentTemplate>
                        
                                   
                                        <table border="0"  cellspacing="0" cellpadding="0" width="100%">
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
                                                          <asp:BoundField HeaderStyle-HorizontalAlign="Left" SortExpression ="TransType" DataField="TransType" HeaderText="Transaction Type"
                                                                NullDisplayText="N/A">
                                                                <ItemStyle Wrap="False" />
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
                                                                       <asp:Label ID="lbl_Custom_Attribute_3" Visible ="false"  runat="server" Text='<%# Bind("Transaction_type")%>'></asp:Label>
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
                    </div>
                                    <telerik:RadWindow ID="MPEImport" Title= "Import Order level Customer Discount" runat="server" Skin="Windows7" Behaviors="Move,Close"
                                                AutoSize="true" ReloadOnShow="false" MinHeight="170px" VisibleStatusbar="false" Overlay="true" Modal="true">
                                              <ContentTemplate>
                    <asp:HiddenField ID="HFV" runat="server" Value="-1" />
                                <div class="popupcontentblk">
                                    <p><asp:Label runat ="server" ID="Label6" CssClass ="txtSM" ForeColor ="Blue"  Text =""></asp:Label></p>
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
		                                <div class="col-sm-5">
			                                <label></label>
		                                </div>
		                                <div class="col-sm-7">
			                                <div class="form-group">
                                                <asp:Button ID="btnImport" runat="server" Text="Import" CssClass ="btn btn-warning" /> <asp:Button ID="btnCancelImport" CssClass ="btn btn-default"  TabIndex="5" runat="server"
                                                                        CausesValidation="false" Text="Cancel" />
                                                      <asp:Button ID="DummyImBtn" style="display:none"  runat="server" Text="Reimport" />
                                               <asp:Button ID="BtnReimport" runat="server" Text="Reimport"  Visible ="false" 
                                                     CssClass="btnInputBlue" />
                                               <asp:Button ID="DummyReimBtn" style="display:none"  runat="server" Text="Reimport" />
                                                <span style ="text-decoration: underline !important;"> <asp:LinkButton ID="lbLog" 
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
                                    <p><asp:Label runat ="server" ID="lblUpMsg" CssClass ="txtSM" ForeColor ="Red"></asp:Label></p>
                                    <div class="">
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
                             
         </asp:Panel> 

                                    
                                    
                                    </ContentTemplate>
                                 <Triggers>
                             <asp:PostBackTrigger  ControlID="btnExport"   />
                         
                            <asp:AsyncPostBackTrigger ControlID="btnImportWindow" />
                                <asp:PostBackTrigger  ControlID="btndownloadTemp"  />        
                            </Triggers>
                            </asp:UpdatePanel> 
                                
                               
                                                   
                                                 
                                             
                                             
                                           
                                
                    
             
                  
    <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="ClassUpdatePnl1"
        runat="server">
        <ProgressTemplate>
            <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                <img src="../assets/img/ajax-loader.gif" alt="Processing..."  />
                <span>Processing... </span>
            </asp:Panel>
        </ProgressTemplate>
    </asp:UpdateProgress>
    
 
</asp:Content>
