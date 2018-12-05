<%@ Page Title="Manage Assets" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master"
    CodeBehind="ManageAssets.aspx.vb" Inherits="SalesWorx_BO.ManageAssets" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<style>
    #ctl00_ContentPlaceHolder1_MapWindow_C
    {
    	overflow: hidden !important;
    }
    .RadWindow_Default a.rwIcon {
   background-image: none !important;
}
</style>
    <script type="text/javascript">

        var TargetBaseControl = null;

        window.onload = function() {
            try {
                TargetBaseControl =
           document.getElementById('<%= Me.ClassUpdatePnl.ClientID %>');
            }
            catch (err) {
                TargetBaseControl = null;
            }
        }
        function TestCheckBox() {
            if (TargetBaseControl == null) return false;
            var TargetChildControl = "chkDelete";
            var TimeCon = document.getElementById('<%= Me.ClassUpdatePnl.ClientID %>');

            var Inputs = TargetBaseControl.getElementsByTagName("input");

            for (var n = 0; n < Inputs.length; ++n)
                if (Inputs[n].type == 'checkbox' && Inputs[n].id.indexOf(TargetChildControl, 0) >= 0 && Inputs[n].checked) {
                return confirm('Would you like to delete the selected assets and history?');
                return true;
            }
            alert('Select at least one assets!');
            return false;

        }

        function CheckAll(cbSelectAll) {
            if (TargetBaseControl == null) return false;
            var TargetChildControl = "chkDelete";
            var TimeCon = document.getElementById('<%= Me.ClassUpdatePnl.ClientID %>');

            var Inputs = TargetBaseControl.getElementsByTagName("input");

            for (var n = 0; n < Inputs.length; ++n)
                if (Inputs[n].type == 'checkbox' && Inputs[n].id.indexOf(TargetChildControl, 0) >= 0) {
                Inputs[n].checked = cbSelectAll.checked;
            }

        }


        function Validate() {
            //            Page_ClientValidate();
            //            if (!Page_IsValid) {
            //                $find('<%=MpInfoError.ClientID%>').show();
            //                var Info = document.getElementById('<%=lblinfo.ClientID%>');
            //                Info.innerHTML = "Validation";
            //                document.getElementById('<%=lblMessage.ClientID%>').innerHTML = '';
            //                return false;
        }
        ////        }

        function DisableValidation() {
            //            Page_ValidationActive = false;
            //            return true;

        }

    

   
    </script>

    <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
        <tr>
            <td id="contentofpage" width="100%" height="76%" class="topshadow">
                <div class="pgtitileposition">
                    <span class="pgtitile3">Manage Assets</span></div>
                <table width="auto" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
                    <tr>
                        <td style="padding: 6px 12px">
                            <asp:UpdatePanel ID="TopPanel" runat="server" UpdateMode="Conditional">
                             <Triggers>
                           <asp:PostBackTrigger  ControlID="btnExport"   />
                          <asp:PostBackTrigger  ControlID="btnImportWindow"  />
                        
                            </Triggers>
                                <ContentTemplate>
                                    <table width="100%" border="0" cellspacing="2" cellpadding="2">
                                    
                                           <tr>
                                <td class="txtSMBold">
                               Organization :
                                  
                                </td>
                                <td> 
                                <asp:DropDownList CssClass="inputSM" ID="ddOraganisation"  Width ="250px"
                    runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" 
                    AutoPostBack="True">
                </asp:DropDownList>
                <asp:Button ID="btnAdd" runat="server" CausesValidation="false" CssClass="btnInputBlue"
                                                    OnClick="btnAdd_Click" TabIndex="1" Text="Add" />
                                                 <asp:Button ID="btnExport" runat="server" CssClass="btnInputOrange" Text="Export"   
                                                    />
                                                      <asp:Button ID="btnImportWindow" runat="server" CssClass="btnInputGreen" Text="Import" 
                                                     />
                               </td>
                                </tr> 
                                    
                                   
                                         <tr>
                                           
                                            
                                            <td  class="txtSMBold">
                                                Filter By :
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlFilterBy" runat="server"  CssClass="inputSM" TabIndex="3">
                                                    <asp:ListItem Selected="True">All</asp:ListItem>
                                                    <asp:ListItem Value="Customer No">Customer No</asp:ListItem>
                                                     <asp:ListItem Value="Customer Name">Customer Name</asp:ListItem>
                                                       <asp:ListItem Value="Asset Type">Asset Type</asp:ListItem>
                                                    <asp:ListItem Value="AssetType ID">Asset Code</asp:ListItem>
                                                    <asp:ListItem Value="Description">Description</asp:ListItem>
                                                     <asp:ListItem Value="Is Active">Is Active</asp:ListItem>
                                                </asp:DropDownList>  <asp:TextBox ID="txtFilterVal" runat="server" ToolTip ="Enter Filter Value" autocomplete="off" CssClass="inputSM"
                                                    TabIndex="4"></asp:TextBox>
                                                                                        
                                                <asp:Button ID="btnFilter" runat="server" CausesValidation="False" CssClass="btnInputGrey"
                                                    OnClick="btnFilter_Click" TabIndex="5" Text="Filter" />
                                                          <asp:Button ID="Clear" runat="server" CausesValidation="False" CssClass="btnInputRed"
                                                     TabIndex="6" Text="Reset" />
                                            </td>
                                        </tr>
                                    </table>
                                           
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                  
                    <tr>
                        <td>     
                            <asp:UpdatePanel ID="ClassUpdatePnl" runat="server" UpdateMode="conditional">
                           
                                <ContentTemplate>
                                 <asp:GridView Width="100%" ID="dgvErros" Visible ="false"  runat="server" EmptyDataText="No items to display"
                                                        EmptyDataRowStyle-Font-Bold="true"  AutoGenerateColumns="False" 
                                                        AllowPaging="false" AllowSorting="false"  PageSize="25" CellPadding="0" CellSpacing="0" CssClass="tablecellalign" >
                   
                                                        <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
                                                        <EmptyDataRowStyle Font-Bold="True" />
                                                        <Columns>
                                                     
                                                            <asp:BoundField HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="RowNo"
                                                                HeaderText="Row No">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>
                                                           
                                                         <%-- <asp:BoundField HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="ColNo"
                                                                HeaderText="Col No">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>
                                                           
                                                         <asp:BoundField HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="ColName"
                                                                HeaderText="Colume Name">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>--%>
                                                          
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
                                   <telerik:RadWindow ID="MapWindow" Title ="Asset Details" runat="server"  Behaviors="Move,Close" 
         width="600px" height="400px"  ReloadOnShow="false"  VisibleStatusbar="false" Modal ="true"   Overlay="true"  >
               <ContentTemplate>
            
                    <table cellpadding ="2" cellspacing ="2" width ="100%" >
                    <tr><td colspan ="2"><asp:Label runat ="server" Font-Bold ="true" ID="lblValMsg"></asp:Label></td></tr>
                                            <tr>
                                               
                                                <td colspan ="2">
                                                    <asp:TextBox ID="txtAssetTypeId" Visible ="false" Enabled ="false" runat="server" TabIndex="1" CssClass="inputSM"
                                                        ></asp:TextBox>
                                                </td>
                                            
                                            <tr>
                                                <td   class="txtSMBold">
                                                  Customer *:
                                                </td>
                                                <td>
                                                     <telerik:RadComboBox ID="ddlCustomer" Filter="Contains"  EmptyMessage ="Please select"
                                                   EnableLoadOnDemand="True" TabIndex="1" Skin ="Default"   
                                                    MinimumFilterLength="1"  runat="server" 
                                                    Height="200px" Width="300px">
                                                </telerik:RadComboBox>
                                                </td>
                                               </tr>
                                               
                                               
                                                <tr>
                                                <td   class="txtSMBold">
                                                   Asset Type *:
                                                </td>
                                                <td>
                                                       <asp:DropDownList CssClass="inputSM" ID="ddlAssetType"  Width ="300px"
                    runat="server" DataTextField="Description" DataValueField="AssetTypeID"  
                    >
                </asp:DropDownList>
                                                </td>
                                               </tr>
                                               
                                               <tr>
                                                <td   class="txtSMBold">
                                                   Asset Code *:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAssetCode"     TabIndex="4" CssClass="inputSM" runat="server"></asp:TextBox>
                                                </td>
                                               </tr>
                                               
                                               
                                               <tr>
                                                <td   class="txtSMBold"> 
                                                      Description *:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDescription" TextMode ="MultiLine"   Width="300px"  TabIndex="5" CssClass="inputSM" runat="server"></asp:TextBox>
                                                </td>
                                               </tr>
                                               
                                               
                                               <tr>
                                                <td   class="txtSMBold">
                                                  
                                                </td>
                                                <td>
                                                   <asp:CheckBox runat ="server" ID="ChkActive"  Text ="Is Active" />
                                                </td>
                                               </tr>
                                               
                                               
                                            
                                               
                                                
                                            <tr>
                                                <td colspan ="2">
                                                    <asp:Panel ID="Panel12" runat="server" Style="display: none" CssClass="overlay">
                                                        <img alt="Processing..." src="../images/Progress.gif" style="z-index: 10010; vertical-align: middle;" />
                                                        <span style="font-size: 12px; color: #e10000;">Processing... </span>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                          <td></td>
                                                <td>
                                                     <asp:Button ID="btnSave" CssClass="btnInputGreen" TabIndex="8" 
                                                        runat="server" Text="Save" OnClick="btnSave_Click" />
                                                    <asp:Button ID="btnUpdate" CssClass="btnInputGreen" OnClick="btnUpdate_Click"
                                                        runat="server" Text="Update"  />
                                                    <asp:Button ID="btnCancel" CssClass="btnInputRed" TabIndex="9" OnClientClick="return DisableValidation()"
                                                        runat="server" CausesValidation="false" Text="Cancel" />
                                                </td>
                                            </tr>
                                        </table>
                 
               </ContentTemplate>
          </telerik:RadWindow>
                              
                                       
                                       
                                    
                                   <asp:GridView Width="100%" ID="gvAssets" runat="server" EmptyDataText="No assets to Display"
                                                    EmptyDataRowStyle-Font-Bold="true" AutoGenerateColumns="False" 
                                                    AllowPaging="True" AllowSorting="true"  PageSize="25" CellPadding="0" CellSpacing="0" CssClass="tablecellalign" DataKeyNames ="Asset_ID" >
                                                  
                                                    <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
                                                  
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="cbSelectAll" onclick="CheckAll(this)" ToolTip="Select/Unselect All"
                                                                    runat="server" />
                                                                <asp:ImageButton ToolTip="Delete Selected Items " ID="btnDeleteAll" runat="server"
                                                                    CausesValidation="false"  ImageUrl="~/images/delete-13.png" 
                                                                    OnClientClick="return TestCheckBox()"
                                                                    OnClick="btnDeleteAll_Click" />
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Left"  />
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkDelete" runat="server" />
                                                                <asp:ImageButton ToolTip="Delete assets" ID="btnDelete" OnClick="btnDelete_Click"
                                                                    runat="server" CausesValidation="false"  ImageUrl="~/images/delete-13.png" OnClientClick="javascript:return confirm('Would you like to delete the selected assets and history?');" />
                                                                <asp:ImageButton ID="btnEdit" ToolTip="Edit assets" runat="server" CausesValidation="false"
                                                                      ImageUrl="~/images/edit-13.png"   OnClick="btnEdit_Click" />
                                                                                                                                  </ItemTemplate>
                                                                                                                                    <ItemStyle Wrap="false" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="AssetType" HeaderText="Asset Type" SortExpression="AssetType">
                                                            <ItemStyle Wrap="false" />
                                                               <HeaderStyle HorizontalAlign="Left" Wrap ="false"  />
                                                        </asp:BoundField>
                                                         <asp:BoundField DataField="Asset_Code" HeaderText="Asset Code" SortExpression="Asset_Code">
                                                            <ItemStyle Wrap="false" />
                                                               <HeaderStyle HorizontalAlign="Left" Wrap ="false"  />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description">
                                                            <ItemStyle Wrap="true" />
                                                               <HeaderStyle HorizontalAlign="Left" Wrap ="false"  />
                                                        </asp:BoundField>
                                                           <asp:BoundField DataField="Customer_No" HeaderText="Cust.No" SortExpression="Customer_No">
                                                            <ItemStyle Wrap="false" />
                                                               <HeaderStyle HorizontalAlign="Left" Wrap ="false"  />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="CustomerName" HeaderText="Cust.Name" SortExpression="CustomerName">
                                                          <ItemStyle Wrap="false" />
                                                               <HeaderStyle HorizontalAlign="Left" Wrap ="false"  />
                                                        </asp:BoundField>
                                                         <asp:BoundField DataField="Is_Active" HeaderText="Is Active" SortExpression="Is_Active">
                                                            <ItemStyle Wrap="false" />
                                                               <HeaderStyle HorizontalAlign="Left" Wrap ="false"  />
                                                        </asp:BoundField>
                                                         <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCurrency" runat="server" Text='<%# Bind("Asset_id") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        
                                                    </Columns>
                                                      <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
                                                </asp:GridView>
                                    
                               
                                
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
                                                </td>
                                            </tr>
                                        </table>
                                </ContentTemplate>
                            
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
                
                   <table>
        <tr>
            <td>
                <asp:Button ID="btnImportHidden" CssClass="btnInput" runat="Server" Style="display: none" />
                <ajaxToolkit:ModalPopupExtender BackgroundCssClass="modalBackground" ID="MPEImport"
                    runat="server" PopupControlID="DetailPnl" TargetControlID="btnImportHidden" CancelControlID="btnCancelImport">
                </ajaxToolkit:ModalPopupExtender>
               <asp:Panel ID="DetailPnl" runat="server" Style="display: none" Width="350" CssClass="modalPopup2">
                                      <asp:Panel ID="DragPnl" Font-Size ="13px"  runat="server" Style="cursor: move; background-color: #3399ff;
                                            text-align: center; border: solid 1px #3399ff; color: White; padding: 3px" Width="345">
                                           Import Assets</asp:Panel>
                    <asp:HiddenField ID="HFV" runat="server" Value="-1" />
                    <table  width ="350px">

<tr><td colspan ="2"><br /></td></tr>
                        <tr>
                            <td  class="txtSMBold">
                                Select File :
                            </td>
                            <td>
                                <asp:FileUpload ID="ExcelFileUpload" runat="server" />
                            </td>
                         
                        </tr>
                       

                   <tr><td colspan ="2"><br /></td></tr>

                        <tr>
                            <td class="txtSMBold" >
                            
                            </td>
                            <td colspan ="2" >
                            <asp:CheckBox ID="chkUpdate" Font-Bold ="true"  ForeColor ="blue" CssClass ="inputSM" 
                            runat ="server" Text ="Update Existing Assets" />
                               
                            </td>
                        </tr>
                        <tr><td colspan ="2"><br /></td></tr>
                        <tr>
                        <td class="txtSMBold"></td><td >
                         <asp:Button ID="btnImportSave" CssClass="btnInput" TabIndex="1" CausesValidation="false"
                                    OnClientClick="return DisableValidation()" runat="server" Text="Import" />
                                <asp:Button ID="DummyImBtn" Style="display: none" runat="server" Text="Import" CausesValidation="false"
                                    OnClientClick="return DisableValidation()" />
                                <asp:Button ID="btnCancelImport" CssClass="btnInput" TabIndex="2" OnClientClick="return DisableValidation()"
                                    runat="server" CausesValidation="false" Text="Cancel" />
                                     <asp:LinkButton ID="lbLog" Font-Bold ="true" Font-Size ="13px" ForeColor  ="#337AB7" ToolTip ="Click here to see the uploaded log" runat ="server" Text ="View Log" OnClick ="lbLog_Click"></asp:LinkButton>
                        </td></tr>
                        
                        <tr><td colspan ="2"><asp:Label runat ="server" ID="lblUpMsg" CssClass ="txtSM" Font-Size ="12px" ForeColor ="Green" Font-Bold ="true" ></asp:Label></td></tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel runat="server" ID="UpPanel">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="DummyImBtn" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
                <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="ClassUpdatePnl"
                    runat="server">
                    <ProgressTemplate>
                        <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
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
