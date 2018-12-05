<%@ Page Title="Van Device Configuartion" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master"
    CodeBehind="VanDeviceConfig.aspx.vb" Inherits="SalesWorx_BO.VanDeviceConfig" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

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

    </script>
 <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
                <tr>
                    <td id="contentofpage" width="100%" height="76%" class="topshadow">
                        <div class="pgtitileposition">
                            <span class="pgtitile3">Van Device Configuration</span></div>
                        <div id="pagenote">
                            This screen may be used to configure the van device</div>
                            <asp:UpdatePanel runat="server" ID="TopPanel" UpdateMode ="Conditional" >
        <ContentTemplate>
                        <table width="auto" border="0" cellspacing="0" cellpadding="0" id="tableform">
                            
                            <tr>
                                <td class="txtSMBold">
                               Organization :
                                  
                                </td>
                                <td>  <asp:DropDownList ID="ddOraganisation"  AutoPostBack="true" runat="server" CssClass="inputSM">
                                    </asp:DropDownList></td>
                                </tr> 
                                <tr>
                                <td  class="txtSMBold" > 
                                  Van :
                               </td>
                               <td>
                                  
                                
                                    <asp:DropDownList ID="ddlSalesRep" AutoPostBack="true" runat="server" CssClass="inputSM">
                                    </asp:DropDownList>
                                      <asp:Button  ID="btnUpdate" runat="server" Text="Update" CssClass="btnInputGreen"
                                                                />
                                                                  <asp:Button  ID="btnReset" runat="server" Text="Reset" CssClass="btnInputGrey"
                                                                />
                                </td>
                               
                            </tr>
                            <tr>
                            <td colspan ="2">
                                             <asp:GridView Width="100%" ID="gvConfig" runat="server" EmptyDataText="No records to display"
                                                    EmptyDataRowStyle-Font-Bold="true"  AutoGenerateColumns="False"
                                                    AllowPaging="false" AllowSorting="false"  PageSize="25" CellPadding="0" CellSpacing="0" CssClass="tablecellalign">
                                                   
                                                    <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
                                                    <Columns>
                                                       
                                                        <asp:BoundField DataField="Row_ID" Visible ="false"  HeaderText="Row ID" SortExpression="Row_ID">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Config_Type" HeaderText="Config Type" SortExpression="Config_Type">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Config_Key" HeaderText="Config Key" SortExpression="Config_Key">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>     
                                                            <asp:TemplateField HeaderText="Config Value">
    <ItemTemplate>
        <asp:TextBox ID="txtConfigValue" runat ="server" CssClass ="inputSM" Text ='<%#Eval("Config_Value") %>'></asp:TextBox>
                    <asp:Label runat ="server" Visible ="false" ID="lblRowID" Text ='<%#Eval("Row_ID") %>' ></asp:Label>
        
    
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
                        </td> 
                        </tr> 
                        </table> 
                 <asp:UpdatePanel ID="UPModal" runat="server" >
                                <ContentTemplate>
                                       
    
             
           
                          <table width="auto" border="0" cellspacing="0" cellpadding="0">
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
                                                                                                <asp:Label ID="lblMessage"  Font-Size ="13px" runat="server"></asp:Label>
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
        
  
   
    
  
           <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="UpModal"
                    runat="server">
                    <ProgressTemplate>
                        <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                            <img src="../images/Progress.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                            <span style="font-size: 12px; color: #3399ff;">Processing... </span>
                        </asp:Panel>
                    </ProgressTemplate>
                </asp:UpdateProgress>
              
         
</asp:Content>
