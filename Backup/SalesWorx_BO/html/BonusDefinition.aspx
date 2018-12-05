<%@ Page Title="Bonus Definition" Language="vb" AutoEventWireup="false" EnableEventValidation="false"    
 MasterPageFile="~/html/DefaultLayout.Master" CodeBehind="BonusDefinition.aspx.vb"  Inherits="SalesWorx_BO.BonusDefinition" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <style>
     .rcTimePopup
 {
   display:none ! important;
 }
 </style> 

  <script language="javascript" type="text/javascript">

     

    </script>



    <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
        <tr>
            <td id="contentofpage" width="100%" height="76%" class="topshadow">
                <div class="pgtitileposition">
                    <span class="pgtitile3">Bonus Definition</span></div>
                    <div style="float:right;">
          <asp:Button  ID="btnBack" runat="server" Text="Go Back" CssClass="btnInputGrey" />
                       
                        <asp:Button ID="btnExport" 
                                                                                                              runat="server" CssClass="btnInputOrange" Text="Export" TabIndex ="11" />
                                                        <asp:Button ID="btnImportWindow" runat="server" CssClass="btnInputGreen" Text="Import" TabIndex ="12" />                                   
                                                             
        </div> 
                <table width="auto" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
                    <tr>
                        <td style="padding: 6px 12px">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode ="Conditional" >
                             <Triggers>
                             <asp:PostBackTrigger  ControlID="btnExport"   />
                             <asp:PostBackTrigger  ControlID="lbLog"   />
                              
                          <asp:PostBackTrigger  ControlID="btnImportWindow"  />
                        
                            </Triggers>          
                                  
                                <ContentTemplate>
                                
                                      
                                
                                
                                
                                
                        
                                   
                                    <asp:Panel ID="PnlOrderDetails"  Font-Bold ="true"  ForeColor="#0090d9"   GroupingText="" runat="server">
                                        <table border="0" cellspacing="6" cellpadding="0">
                                         <tr>
                                <td class="txtSMBold" >
                                    Organization :
                                  
                                  </td>
                                    <td class="txtSMBold" >
                                        <asp:DropDownList ID="ddl_org" Visible ="false" runat="server" Width ="200px"  CssClass="inputSM"  AutoPostBack="true">
        </asp:DropDownList>
                                  <asp:Label runat ="server" Font-Bold ="true" ForeColor ="Green" ID="lblOrg"></asp:Label>
                                    
                                </td>
                                <td><span style ="padding-left:120px;"></span> </td>
                            
                                <td class="txtSMBold" >
                                  
                                  Plan Name :
                                  </td>
                                    <td class="txtSMBold" >
                                    <asp:Label ID="lblPlanName"  runat ="server" Font-Bold ="true"  ForeColor="#0090d9"  ></asp:Label>
                                    
                                    
                                </td>
                                </tr>  
                                </table>
                                 <table border="0" cellspacing="6" cellpadding="0">
                                            <tr>
                                                <td   class="txtSMBold">
                                                    Order Item :
                                                </td>
                                                <td  >
                                                 <%--   <asp:TextBox ID="txtItemCode" runat="server"  Font-Bold ="false"  TabIndex ="1"
                                                        CssClass ="inputSM" AutoPostBack="false"></asp:TextBox>
                                                         <asp:hiddenfield id="hdnValue1" onvaluechanged="hdnValue1_ValueChanged" runat="server"/>--%>
    <telerik:RadComboBox ID="ddlOrdCode" Filter="Contains"  EmptyMessage ="Please select"
                                                   EnableLoadOnDemand="True" TabIndex="1"  Sort ="Ascending"   AutoPostBack ="true" 
                                                    MinimumFilterLength="1"  runat="server"
                                                    Height="200px" Width="300px">
                                                </telerik:RadComboBox>
                                                </td>
                                                    <td   class="txtSMBold">
                                                    Description :
                                                </td>
                                                <td  >
                                                <%--    <asp:TextBox ID="txtDescription" runat="server"   Font-Bold ="false"  TabIndex ="2"
                                                        CssClass ="inputSM" AutoPostBack="false" Width="240px"></asp:TextBox>
                                                       
                                                <asp:hiddenfield id="hdnValue2" onvaluechanged="hdnValue2_ValueChanged" runat="server"/>--%>
                                                 <telerik:RadComboBox ID="ddlOrdDesc" Filter="Contains" EmptyMessage ="Please select"
                                                   EnableLoadOnDemand="True" TabIndex="2"  Sort ="Ascending" 
                                                    MinimumFilterLength="1"  runat="server"  AutoPostBack ="true" 
                                                    Height="200px" Width="300px">
                                                </telerik:RadComboBox>
                                                </td>
                                             
                                            </tr>
                                             
                                            <tr>
                                                <td   class="txtSMBold" >
                                                   Bonus Item:
                                                </td>
                                                <td>
                                                  <%--  <asp:TextBox ID="txtBItemCode" runat="server"  Font-Bold ="false"  TabIndex ="3"
                                                        CssClass ="inputSM" AutoPostBack="false"></asp:TextBox>
                                                         <asp:hiddenfield id="hdnValue3" onvaluechanged="hdnValue3_ValueChanged" runat="server"/>--%>
  <telerik:RadComboBox ID="ddlGetCode" Filter="Contains" EmptyMessage ="Please select"
                                                   EnableLoadOnDemand="True" TabIndex="3"  Sort ="Ascending"   AutoPostBack ="true" 
                                                    MinimumFilterLength="1"  runat="server" 
                                                    Height="200px" Width="300px">
                                                </telerik:RadComboBox>
                                                     
                                                </td>
                                                    <td   class="txtSMBold">
                                                    Description :
                                                </td>
                                                <td >
                                                   <%-- <asp:TextBox ID="txtBDescription" runat="server"   Font-Bold ="false"  TabIndex ="4"
                                                        CssClass ="inputSM" AutoPostBack="false" Width="240px"></asp:TextBox>
                                                       
                                                <asp:hiddenfield id="hdnValue4" onvaluechanged="hdnValue4_ValueChanged" runat="server"/>--%>
                                                   <telerik:RadComboBox ID="ddlgetDesc" Filter="Contains" EmptyMessage ="Please select"
                                                   EnableLoadOnDemand="True" TabIndex="4" Sort ="Ascending"   AutoPostBack ="true" 
                                                    MinimumFilterLength="1"  runat="server" 
                                                    Height="200px" Width="300px">
                                                </telerik:RadComboBox>
                                                </td>
                                            
                                             
                                            </tr>
                                      
                                            <tr>
                                               
                                                <td class="txtSMBold"   >
                                                   From Qty :
                                                </td>
                                                <td >
                                                    <asp:TextBox ID="txtFromQty" runat="server"  TabIndex ="6" 
                                                            CssClass="inputSM" Width="70px"></asp:TextBox>
                                                        <ajaxToolkit:FilteredTextBoxExtender ID="FTEOrdQty" runat="server" 
                                                            FilterType="Numbers,Custom" TargetControlID="txtFromQty">
                                                        </ajaxToolkit:FilteredTextBoxExtender>
                                                </td>
                                                 <td class="txtSMBold" >
                                                    Bonus Type :
                                                </td>
                                                <td >
                                                    <asp:DropDownList CssClass ="inputSM" ID="ddlType"  runat="server" TabIndex ="5" AutoPostBack ="false">
                                                  <%--  <asp:ListItem Value ="POINT">POINT</asp:ListItem>
                                                    <asp:ListItem Value ="RECURRING">RECURRING</asp:ListItem>
                                                     <asp:ListItem Value ="PERCENT">PERCENT</asp:ListItem>--%>
                                                    </asp:DropDownList>
                                                       <asp:HiddenField  ID="hfOrgID" runat="server" />
                                    <asp:HiddenField  ID="hfPlanId" runat="server" />
                                                </td>
                                             
                                               
                                                </tr>
                                                
                                             
                                                  <tr>
                                                <td class="txtSMBold"   >
                                                   To Qty :
                                                </td>
                                                <td >
                                                    <asp:TextBox ID="txtToQty" runat="server"  TabIndex ="7"
                                                            CssClass="inputSM" Width="70px"></asp:TextBox>
                                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" 
                                                            FilterType="Numbers,Custom" TargetControlID="txtToQty">
                                                        </ajaxToolkit:FilteredTextBoxExtender>
                                                </td>
                                                <td class="txtSMBold"   >
                                                  Bonus Qty :
                                                </td>
                                                <td >
                                                    <asp:TextBox ID="txtGetQty" runat="server" TabIndex ="8"
                                                            CssClass="inputSM" Width="70px"></asp:TextBox>
                                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" 
                                                            FilterType="Numbers,Custom" TargetControlID="txtGetQty">
                                                        </ajaxToolkit:FilteredTextBoxExtender>
                                                
                                                
                                                <%-- <label style="font-weight:bold;color:Black;font-size:11px;width:5%;">Addl.%:</label> --%>
                                               
                                               
                                                    <asp:TextBox ID="txtAddPercent" Visible ="false"  runat="server" TabIndex ="8" MaxLength ="3" 
                                                            CssClass="inputSM" Width="15%"></asp:TextBox>
                                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" 
                                                            FilterType="Numbers,Custom" TargetControlID="txtAddPercent" >
                                                        </ajaxToolkit:FilteredTextBoxExtender>
                                                </td>
                                                </tr>
                                               
                                             <tr>
                 <td  class="txtSMBold" >
                  Valid From</td>
                 <td  >
                  <telerik:RadDateTimePicker ID="StartTime"  MinDate ="1900-01-01 00:00:00.000"  MaxDate ="9999-12-31 00:00:00.000"    Width="100px" TabIndex ="9"    runat="server" 
                                    >
                                    <DateInput DateFormat ="dd-MM-yyyy" readonly="true" ></DateInput>
                                   
                                </telerik:RadDateTimePicker>
                                  <asp:RequiredFieldValidator runat="server" Visible ="false" Width ="3px" ID="RequiredFieldValidator1" ControlToValidate="StartTime"
                        ErrorMessage="*"></asp:RequiredFieldValidator></td>
                <td  class="txtSMBold" >         
                     Valid To</td><td >
                 <telerik:RadDateTimePicker ID="EndTime" MinDate ="1900-01-01 00:00:00.000"  MaxDate ="9999-12-31 00:00:00.000"   Width="100px" TabIndex ="10"   runat="server" 
                                    >
                                    <DateInput DateFormat ="dd-MM-yyyy" readonly="true" ></DateInput>
                                   
                                </telerik:RadDateTimePicker>
                                 <asp:RequiredFieldValidator runat="server" Visible ="false" ID="Requiredfieldvalidator2" Width ="3px"  ControlToValidate="EndTime"
                        ErrorMessage="*"></asp:RequiredFieldValidator>
                       
                     
                        </td> 
                        
                        </tr>
                                            
                                                   <tr>
                                                    <td colspan="3">
                                                          <asp:CompareValidator ID="dateCompareValidator" runat="server" ControlToValidate="EndTime"
                        ControlToCompare="StartTime" Operator="GreaterThan"    Type="String"
                        ErrorMessage="To date > From date">
                    </asp:CompareValidator>
                                                    </td>
                                                   
                                                </tr>
                                                <tr>
                                               
                                                 <td colspan="4" style ="width:100%;" >
                                                                                                              <asp:Button ID="btnAddItems" 
                                                                                                              runat="server" CssClass="btnInputBlue" Text="Add" TabIndex ="9" />
                                                        <asp:Button ID="btnClear" runat="server" CssClass="btnInputRed" Text="Clear" TabIndex ="10" />
                                                   
                                                   <asp:Button ID="btnGo" Visible ="false"  runat="server" CssClass="btnInput" Text="Go Back" TabIndex ="10" />
                                                   <asp:CheckBox ID="chShow" runat ="server" Text ="Show deactivated items" AutoPostBack ="true"  />
                                                    </td>
                                                   
                                                </tr>
                                             
                                        
                                          
                                            <tr>
                                                    <td>
                                                        <asp:Label ID="lblDItemCode" Visible="false" Width="45" CssClass="inputSM" runat="server"></asp:Label>
                                                    
                                                        <asp:Label ID="LblDItemId" Visible="false" Width="45" CssClass="inputSM" runat="server"></asp:Label>
                                                            <asp:Label ID="lblEditRow" Visible="false" Width="45" CssClass="inputSM" runat="server"></asp:Label>
                                                   
                                                        <asp:Label ID="lblDDescription" Visible="false" Width="45" CssClass="inputSM" runat="server"></asp:Label>
                                                 
                                                        <asp:Label ID="lblBItemCode" Visible="false" Width="45" CssClass="inputSM" runat="server"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="LblBItemId" Visible="false" Width="45" CssClass="inputSM" runat="server"></asp:Label>
                                                             <asp:Label ID="lblLineId" Visible="false" Width="45" CssClass="inputSM" runat="server"></asp:Label>
                                                    </td>
                                                    <td colspan ="2">
                                                        <asp:Label ID="lblBDescription" Visible="false" Width="45" CssClass="inputSM" runat="server"></asp:Label>
                                                           <asp:Label ID="lblOrgId" Visible="false" Width="45" CssClass="inputSM" runat="server"></asp:Label>
                                                             <asp:Label ID="lblF" Visible="false" Width="45" CssClass="inputSM" runat="server"></asp:Label>
                                                             <asp:Label ID="lblT" Visible="false" Width="45" CssClass="inputSM" runat="server"></asp:Label>
                                                               <asp:Label ID="lblVF" Visible="false" Width="45" CssClass="inputSM" runat="server"></asp:Label>
                                                             <asp:Label ID="lblVT" Visible="false" Width="45" CssClass="inputSM" runat="server"></asp:Label>
                                                               <asp:Label ID="lblDUOM" Visible ="false"  runat="server" CssClass="inputSM" Font-Bold="True" 
                                                        Width="70px"  ForeColor="#0090d9" ></asp:Label>
                                                         <asp:Label ID="lblBUOM" Visible ="false"  runat="server" CssClass="inputSM" Font-Bold="True" 
                                                        Width="70px"  ForeColor="#0090d9" ></asp:Label>
                                                    </td>
                                                </tr>
                                        </table>
                                    </asp:Panel>
                                    
                                
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    
                </table>
                            <asp:UpdatePanel ID="Panel" runat="server" >
                             <Triggers>
           
      
            <asp:PostBackTrigger ControlID="btnImport" /> 
	
        </Triggers>
                                <ContentTemplate>
                                
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
                        Import Bonus Rules</asp:Panel>
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
                                
                                
                                    <asp:Panel ID="PnlGridData" runat="server" Visible ="false" >
                                        <table border="0"  cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td>
                                                    <asp:GridView Width="100%" ID="dgvItems" runat="server" EmptyDataText="No items to display"
                                                        EmptyDataRowStyle-Font-Bold="true"  AutoGenerateColumns="False" DataKeyNames ="LineId"
                                                        AllowPaging="true" AllowSorting="true"  PageSize="5" CellPadding="0" CellSpacing="0" CssClass="tablecellalign"  >
                                                       
                                                        <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
                                                        <EmptyDataRowStyle Font-Bold="True" />
                                                        <Columns>
                                                        <asp:TemplateField>
                                                           <HeaderStyle HorizontalAlign="Left" />
                                                           
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lblStatus"  CssClass="txtLinkSM" ForeColor='<%# System.Drawing.Color.FromName(DataBinder.Eval(Container.DataItem,"IsColor").ToString()) %>'  Font-Bold ="true"  CommandName="DeActivate" runat="server" Text='<%# Bind("IsActive") %>'></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                            <asp:BoundField HeaderStyle-HorizontalAlign="Left"  Visible ="false"  HeaderStyle-Wrap="false" DataField="LineId"
                                                                HeaderText="Line No">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>
                                                           
                                                            <asp:BoundField HeaderStyle-HorizontalAlign="Left" DataField="ItemCode" HeaderText="Order Item" SortExpression ="ItemCode"
                                                                NullDisplayText="N/A">
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                      
                                                          
                                                            <asp:BoundField HeaderStyle-HorizontalAlign="Left" DataField="BItemCode" HeaderText="Bonus Item"
                                                                NullDisplayText="N/A"  SortExpression ="BItemCode" >
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                        
                                                         
                                                            <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="FromQty"
                                                                HeaderText="From Qty"  DataFormatString="{0:F0}">
                                                                   <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                                                <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                                            </asp:BoundField>
                                                              <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="ToQty"
                                                                HeaderText="To Qty"  DataFormatString="{0:F0}">
                                                                 <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                                                <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                                            </asp:BoundField>
                                                               <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="TypeCode"
                                                                HeaderText="Type">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>
                                                              <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="GetQty"
                                                                HeaderText="Bonus Qty"  DataFormatString="{0:F0}">
                                                                <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                                                <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="Get_Add_Per"
                                                                HeaderText="Addl.%"  Visible ="false"  DataFormatString="{0:F2}">
                                                                <HeaderStyle HorizontalAlign="Right" Wrap="False" />
                                                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                            </asp:BoundField>
                                                                <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="Valid_From"
                                                                HeaderText="Valid From"  DataFormatString="{0:dd-MM-yyyy}"   SortExpression ="Valid_From">
                                                               <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false"  SortExpression ="Valid_To" DataField="Valid_To"
                                                                HeaderText="Valid To"  DataFormatString="{0:dd-MM-yyyy}" >
                                                               <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>
                                                             
                                                            <asp:TemplateField HeaderText="">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ToolTip="Edit" ID="btnEdit"   runat="server" Visible ='<%# Bind("IsVisible") %>' CommandName="EditRecord"
                                                                        CausesValidation="false"   ImageUrl="~/images/edit-13.png"   />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ToolTip="Delete" ID="btnCan" runat="server" CommandName="DeleteRecord"
                                                                        CausesValidation="false"  ImageUrl="~/images/delete-13.png" OnClientClick="javascript:return confirm('Would you like to delete the selected item?');" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                          
                                                              <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDItem" runat="server" Text='<%# Bind("DItemId") %>'></asp:Label>
                                                                 <asp:Label ID="lblACode" runat="server" Text='<%# Bind("ACode") %>'></asp:Label>
                                                                 <asp:Label ID="lblADesc" runat="server" Text='<%# Bind("ADesc") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         
                                                          <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblBItem" runat="server" Text='<%# Bind("BItemId") %>'></asp:Label>
                                                                 <asp:Label ID="lblBCode" runat="server" Text='<%# Bind("BCode") %>'></asp:Label>
                                                                 <asp:Label ID="lblBDesc" runat="server" Text='<%# Bind("BDesc") %>'></asp:Label>
                                                                    <asp:Label ID="lblValidFrom" runat="server" Text='<%# Bind("Valid_From") %>'></asp:Label>
                                                                       <asp:Label ID="lblValidTo" runat="server" Text='<%# Bind("Valid_To") %>'></asp:Label>
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
                                    </asp:Panel>
                                   
                                        <table>
                                      
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
                                                       <asp:Label ID="lblinfo" runat="server" Font-Size ="13px" Font-Bold ="true"   ></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" style="text-align: center">
                                                        <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="List" />
                                                      <asp:Label ID="lblMessage" runat="server"  Font-Size ="13px"    ></asp:Label>
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
                         
                                      <table>
                                        <tr>
                                            <td>
                                                <asp:Button ID="Button2" runat="Server" Style="display: none" />
                                                <ajaxToolkit:ModalPopupExtender ID="MPEAlloc" runat="server" BackgroundCssClass="modalBackground"
                                                    TargetControlID="Button2" PopupControlID="pnlAlloc" Drag="true" />
                                                <asp:Panel ID="pnlAlloc" Width="600" runat="server" CssClass="modalPopup" Style="cursor: move;
                                            background-color: #3399ff; text-align: center; border: solid 1px #3399ff; color: White;
                                            padding: 3px; display: none;">
                                                    <table id="table1" width="600" cellpadding="10" style="background-color: White;">
                                                        <tr align="center">
                                                            <td align="center" style="cursor: move; background-color: #3399ff; text-align: center;
                                                                border: solid 1px #3399ff;  padding: 3px">
                                                                
                                                                <asp:Label ID="lblinfo1" runat="server" Font-Size ="12px"></asp:Label>
                                                                <asp:Label ID="lblAction" Visible="false" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                         <tr>
                                                            <td align="center" style="text-align: center">
                                                            <asp:Label ID="lblMsg2" runat="server"  ForeColor ="Green" Font-Bold ="true"  ></asp:Label><br /><br />
                                                              <asp:GridView Width="400px" ID="dgvActive" runat="server" EmptyDataText="No items to display"
                                                        EmptyDataRowStyle-Font-Bold="true"  AutoGenerateColumns="False" DataKeyNames ="ActiveLineId"
                                                        AllowPaging="false" AllowSorting="false"  PageSize="25" CellPadding="0" CellSpacing="0" CssClass="tablecellalign"  >
                                                       
                                                        <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
                                                        <EmptyDataRowStyle Font-Bold="True" />
                                                        <Columns>
                                                      
                                                          
                                                           
                                                            <asp:BoundField HeaderStyle-HorizontalAlign="Left" DataField="ItemName" HeaderText="Item Name"
                                                                NullDisplayText="N/A">
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                      
                                                          
                                                                                                                   
                                                            <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="Prom_Qty_From"
                                                                HeaderText="From Qty"  DataFormatString="{0:F0}">
                                                                    <HeaderStyle HorizontalAlign="Right" Wrap="False" />
                                                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                            </asp:BoundField>
                                                              <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="Prom_Qty_To"
                                                                HeaderText="To Qty"  DataFormatString="{0:F0}">
                                                                   <HeaderStyle HorizontalAlign="Right" Wrap="False" />
                                                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                            </asp:BoundField>
                                                            
                                                              <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="Get_Qty"
                                                                HeaderText="Bonus Qty"  DataFormatString="{0:F0}">
                                                                <HeaderStyle HorizontalAlign="Right" Wrap="False" />
                                                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                            </asp:BoundField>
                                                           
                                                                <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="Valid_From"
                                                                HeaderText="Valid From"  DataFormatString="{0:dd-MM-yyyy}" >
                                                                <HeaderStyle HorizontalAlign="Right" Wrap="False" />
                                                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="Valid_To"
                                                                HeaderText="Valid To"  DataFormatString="{0:dd-MM-yyyy}" >
                                                                <HeaderStyle HorizontalAlign="Right" Wrap="False" />
                                                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                            </asp:BoundField>
                                                          
                                                        
                                                          
                                                              <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblActiveLineID" runat="server" Text='<%# Bind("ActiveLineID") %>'></asp:Label>
                                                              
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
                                                       
                                                        <tr>
                                                            <td align="center" style="text-align: center">
                                                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" />
                                                                <asp:Label ID="lblmessage1" runat="server" Font-Size ="12px"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" align="center" style="text-align: center;">
                                                                <asp:Button ID="btnDeactivate" runat="server" Text="Deactivate & Apply" CssClass="btnInput" />
                                                                <asp:Button ID="btnhide" Visible="false" CssClass="btnInput" TabIndex="7" runat="server"
                                                                    Text="Cancel" CausesValidation="false" OnClientClick="$find('MPEAlloc').Hide(); return false;" />
                                                                <asp:Button ID="btnClose1" runat="server" Text="Cancel" CssClass="btnInput" />
                                                             
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        
                            <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="Panel" runat="server" 
                                DisplayAfter="10">
                                <ProgressTemplate>
                                    <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                                        <img src="../images/Progress.gif" alt="Processing..." style="vertical-align: middle;" />
                                        <span style="font-size: 12px; font-weight:700px;color: #3399ff;">Processing...
                                        
                                                        
                                        
                                        
                                        
                                        
                                        
                                        
                                        
                                        
                                        
                                        
                                        
                                        </span>
                                    </asp:Panel>
                                    </div>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                            <asp:UpdateProgress ID="UpdateProgress2" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="10"
                                runat="server">
                                <ProgressTemplate>
                                    <asp:Panel ID="Panel14" CssClass="overlay" runat="server">
                                        <img src="../images/Progress.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                                        <span style="font-size: 12px; color: #3399ff;">Processing... </span>
                                    </asp:Panel>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                    
                <br />
                <br />
            </td>
        </tr>
    </table>
</asp:Content>
