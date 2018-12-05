<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master" CodeBehind="ListCustomers.aspx.vb" Inherits="SalesWorx_BO.ListCustomers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 235px;
        }
    </style>
    <script>
    function checkconfirm(status) {
            if (status == "Y")
              return confirm("Are you sure to disable this customer?")
            else
                return confirm("Are you sure to enable this customer?")
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
        <tr>
            <td id="contentofpage" width="100%" height="76%" class="topshadow">
                <div class="pgtitileposition">
                    <span class="pgtitile3">Customer Management</span></div>
                <table width="auto" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
                    <tr>
                        <td style="padding: 6px 12px">
                            <asp:UpdatePanel ID="TopPanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                   <table  border="0" cellspacing="2" cellpadding="2">
          <tr>
            <td width="105" class="txtSMBold">Organization :</td>
            <td class="style1" ><asp:DropDownList CssClass="inputSM" ID="ddlOrganization"  Width ="200px"
                    runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="true"> 
                </asp:DropDownList>
              </td>
                 
          </tr>
               
          <tr> 
            <td width="105" class="txtSMBold">Name :</td>
            <td class="style1">
               <asp:TextBox ID="txtCustomerName" Width ="200px" CssClass="inputSM" runat="server"></asp:TextBox>
            </td>
             <td width="105" class="txtSMBold">Customer No:</td>
            <td ><asp:TextBox ID="txtCustomerNo" CssClass="inputSM" Width ="150px" runat="server"></asp:TextBox>&nbsp;
                     <asp:Button CssClass="btnInputGrey" ID="BtnSearch" runat="server" Text="Search" />
                     <asp:Button CssClass="btnInputBlue" ID="Btn_Clear" runat="server" Text="Clear Filter" />
                     <asp:Button CssClass="btnInputGreen" ID="BtnAdd" runat="server" Text="Add" />
                     </td>
          </tr> 
         <%-- <tr> 
            <td width="105" class="txtSMBold">&nbsp;</td>
            <td>
                &nbsp;</td>
            <td width="105" class="txtSMBold">Type :</td>
            <td colspan="3">
                <asp:DropDownList CssClass="inputSM" ID="ddlType" 
                    runat="server" DataTextField="Customer_Type" DataValueField="Customer_Type">
                </asp:DropDownList>
            </td>       
          </tr> --%>
        </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:100%;padding: 6px 12px" >
                            <asp:UpdatePanel ID="ClassUpdatePnl" runat="server" UpdateMode="conditional">
                                <ContentTemplate>
                                
                                    <table border="0" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td>
                                                <asp:GridView ID="grdCustomer" runat="server" EmptyDataText="No Data to Display"
                                                    EmptyDataRowStyle-Font-Bold="true"  AutoGenerateColumns="False"
                                                    AllowPaging="True" AllowSorting="true"  PageSize="25" CellPadding="0" 
                                                    CellSpacing="0" CssClass="tablecellalign" Width="800">
                                                 
                                                    <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
                                                    <EmptyDataRowStyle Font-Bold="True" />
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Left"  />
                                                            <ItemTemplate>
                                                               
                                                                <asp:ImageButton ID="btnEdit" ToolTip="Edit Customer" runat="server" CausesValidation="false"
                                                                    ImageUrl="~/images/edit-13.png"   OnClick="btnEdit_Click" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Customer_No" HeaderText="Customer No." SortExpression="Customer_No">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                         <asp:BoundField DataField="Customer_Name" HeaderText="Customer Name" SortExpression="Customer_Name">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                         <asp:BoundField DataField="Price_List" HeaderText="Price List" SortExpression="Price_List">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                         <asp:BoundField DataField="Cash_Cust" HeaderText="Cash Customer" SortExpression="Cash_Cust">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                       
                                                        <asp:BoundField DataField="CustStatus" SortExpression="CustStatus"  HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"  HeaderText="Status" >
                       
                     </asp:BoundField>
                   
                                                        <asp:TemplateField HeaderText=""  HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false">
                        <ItemTemplate>
                      
                                                   <asp:LinkButton  runat="server" ID="lbChangeStatus"  Text='<%# Bind("Action") %>'  OnClientClick=<%# "javascript:checkconfirm('" + Eval("Cust_Status") + "');" %> OnClick ="lbChangeStatus_Click"></asp:LinkButton>
                                                     <asp:Label ID="lblStatus" runat ="server" Visible ="false"  Text='<%# Bind("Cust_Status") %>'></asp:Label>
                                                    <asp:Label ID="lblCustomer_ID" runat ="server" Visible ="false"  Text='<%# Bind("Customer_ID") %>'></asp:Label>
                                                    <asp:Label ID="lblSite_Use_ID" runat ="server" Visible ="false"  Text='<%# Bind("Site_Use_ID") %>'></asp:Label><asp:Label ID="lbETime" runat ="server" Visible ="false"  Text='<%# Bind("Customer_No") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                                   
                 
                                                    </Columns>
                                                   <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                            
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
        <tr>
            <td id="contentofpage" width="100%" height="76%" class="topshadow">
                &nbsp;</td>
        </tr>
    </table>
</asp:Content>
