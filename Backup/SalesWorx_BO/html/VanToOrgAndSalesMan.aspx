<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master" CodeBehind="VanToOrgAndSalesMan.aspx.vb" Inherits="SalesWorx_BO.VanToOrgAndSalesMan" %>
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

            var Filter = /ddFilterBy/
            var AddString = postBackElement.id.search(Filter);
            
            if (AddString == -1) {
                var myRegExp = /_btnUpdate/;

                var AddString = postBackElement.id.search(myRegExp);
                var EditString = postBackElement.id.search(myRegExp);
                
                if (AddString != -1 || EditString != -1) {
                    $get('<%= Me.DetailPnl.FindControl("Panel12").ClientID%>').style.display = 'block';
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

                var AddString = postBackElement.id.search(myRegExp);
                var EditString = postBackElement.id.search(myRegExp);

                if (AddString != -1 || EditString != -1) {
                    $get('<%=Me.DetailPnl.FindControl("Panel12").ClientID%>').style.display = 'none';
                }
                else {
                    $get('<%= Me.UpdateProgress1.ClientID %>').style.display = 'none';
                }
                postBackElement.disabled = false;
            }
        }

       

    </script><script type="text/javascript">
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
       

       
    </script><table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
        <tr>
            <td id="contentofpage" width="100%" height="76%" class="topshadow">
                <div class="pgtitileposition">
                    <span class="pgtitile3">Organization Setup</span></div>
                <table width="auto" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
                    <tr>
                        <td style="padding: 6px 12px">
                            <asp:UpdatePanel ID="TopPanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                        <tr>
                                           
                                            <td class="txtSMBold" width="75">
                                              Organization:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddFilterBy" runat="server" AutoPostBack="true" CssClass="inputSM"
                                                    TabIndex="1">
                                                </asp:DropDownList>
                                           
                                                <asp:Button ID="btnFilter" runat="server" CausesValidation="False" CssClass="btnInputGrey"
                                                    TabIndex="2" Text="Search" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    
                    <tr >
                        <td>
                            <asp:UpdatePanel ID="ClassUpdatePnl" runat="server" UpdateMode="conditional">
                                <ContentTemplate>
                                    <table border="0" cellspacing="0" cellpadding="0" width="600px" style="padding:10px">
                                        <tr>
                                            <td>
                                                <asp:GridView Width="100%" ID="gvDivConfig" runat="server" EmptyDataText="No Organization details found."
                                                    EmptyDataRowStyle-Font-Bold="true"  AutoGenerateColumns="False"
                                                    AllowPaging="True"   PageSize="25" CellPadding="0" CellSpacing="0" CssClass="tablecellalign">
                                                    <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
                                                    <EmptyDataRowStyle Font-Bold="True" />
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:HiddenField ID="HStock_Org_ID" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"Stock_Org_ID") %>'/>
                                                                <asp:HiddenField ID="HOrg_HE_ID" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"Org_HE_ID") %>'/>
                                                                <asp:HiddenField ID="HEmp_Code" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"Emp_Code") %>'/>
                                                                
                                                                <asp:ImageButton ID="btnEdit" ToolTip="Edit Configuration" runat="server" CausesValidation="false"
                                                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Org_ID") %>'
                                                                      ImageUrl="~/images/edit-13.png"   OnClick="btnEdit_Click" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="SalesRep_Name" HeaderText="Van Org" SortExpression="SalesRep_Name">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                         <asp:BoundField DataField="orgHead" HeaderText="Sales Organization" SortExpression="orgHead">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="SourcOrg" HeaderText="Source Warehouse" SortExpression="SourcOrg">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="SalesMan" HeaderText="Sales Man"
                                                            SortExpression="SalesMan">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        
                                                       <%-- <asp:BoundField DataField="Print_Format" HeaderText="Print Format"
                                                            SortExpression="Print_Format">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>--%>
                                                    </Columns>
                                                      <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
                                                </asp:GridView>
                                            </ td >
                                        </tr>
                                    </table>
                                    <asp:Button ID="btnHiddenCurrency" CssClass="btnInput" runat="Server" Style="display: none" />
                                    <ajaxToolkit:ModalPopupExtender BackgroundCssClass="modalBackground" ID="MPEDivConfig"
                                        runat="server" PopupControlID="DetailPnl" TargetControlID="btnHiddenCurrency"
                                        CancelControlID="btnCancel">
                                    </ajaxToolkit:ModalPopupExtender>
                                    <asp:Panel ID="DetailPnl" runat="server" Width="470"  CssClass="modalPopup" Style="display: none" >
                                        <asp:Panel ID="DragPnl" Font-Size ="13px"  runat="server" Width="465px" Style="cursor: move; background-color: #3399ff;
                                            text-align: center; border: solid 1px #3399ff; color: White; padding: 3px">
                                            Organization Setup</asp:Panel>
                                        <asp:HiddenField ID="HidVal" runat="server" Value="-1" />
                                        <asp:Label ID="lblmsgPopUp" runat="server" Text="" ForeColor="Maroon"></asp:Label>
                                        <table width="100%" cellpadding ="2" cellspacing ="2">
                                         <tr>
                                                <td class="txtSMBold">
                                                    Sales Organization :
                                                </td>
                                                <td>
                                                
                                                    <asp:DropDownList ID="drpLocalOrg" TabIndex ="1"  runat="server" CssClass="inputSM"  Enabled="false">
                                                    </asp:DropDownList>
                                                    
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="txtSMBold">
                                                    Van Organization :
                                                </td>
                                                <td>
                                                 
                                                    <asp:DropDownList ID="drpOrganization" TabIndex ="2"  runat="server" CssClass="inputSM" Enabled="false">
                                                    </asp:DropDownList>
                                                    
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="txtSMBold">
                                                   Source Warehouse:
                                                </td>
                                                <td>
                                                 
                                                    <asp:DropDownList ID="drpSourcWH" TabIndex ="3"   runat="server" CssClass="inputSM" AutoPostBack="false">
                                                    </asp:DropDownList>
                                                    
                                                </td>
                                            </tr>
                                           <tr>
                                                <td class="txtSMBold">
                                                   Sales Man:
                                                </td>
                                                <td>
                                                 
                                                    <asp:DropDownList ID="DrpSalesMan" TabIndex ="4"  runat="server" CssClass="inputSM" AutoPostBack="false">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please select the sales man" ControlToValidate="DrpSalesMan" InitialValue="-1"></asp:RequiredFieldValidator>
                                                   
                                                </td>
                                            </tr>
                                          
                                                                                      
                                            <tr>
                                            <td></td>
                                                <td colspan ="2">
                                                    <asp:Panel ID="Panel12" runat="server" Style="display: none" CssClass="overlay">
                                                        <img alt="Processing..." src="../images/Progress.gif" style="z-index: 10010; vertical-align: middle;" />
                                                        <span style="font-size: 12px; color: #3399ff;">Processing... </span>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                            <td></td>
                                                <td >
                                                 
                                                    <asp:Button ID="btnUpdate" TabIndex ="5" CssClass="btnInputGreen" Text="Update" OnClick="btnUpdate_Click"
                                                        runat="server" />
                                                    <asp:Button ID="btnCancel" CssClass="btnInputRed" TabIndex="6" runat="server" CausesValidation="false"
                                                        Text="Cancel" />
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
                                                    padding: 3px; display:none;">
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
                               <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnFilter" EventName="Click" />
                                </Triggers>
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
    </table>
</asp:Content>
