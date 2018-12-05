<%@ Page Title="Geolocation Management" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master" CodeBehind="MngLatiLongitude.aspx.vb" Inherits="SalesWorx_BO.MngLatiLongitude" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                  return confirm('Would you like to delete the selected record?');
                  return true;
               }
        alert('Select at least one record!');
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
                var myRegExp1 = /btnSave/
                var AddString = postBackElement.id.search(myRegExp);
                var EditString = postBackElement.id.search(myRegExp1);
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
                var myRegExp1 = /btnSave/
                var AddString = postBackElement.id.search(myRegExp);
                var EditString = postBackElement.id.search(myRegExp1);

                if (AddString != -1 || EditString != -1) {
                    $get('<%=Me.DetailPnl.FindControl("Panel12").ClientID%>').style.display = 'none';
                }
                else {
                    $get('<%= Me.UpdateProgress1.ClientID %>').style.display = 'none';
                }
                postBackElement.disabled = false;
            }
        }
   
    </script>

    <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
        <tr>
            <td id="contentofpage" width="100%" height="76%" class="topshadow">
                <div class="pgtitileposition">
                    <span class="pgtitile3">Geolocation Management</span></div>
            <table width="auto" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
                    <tr>
                        <td style="padding: 6px 12px">
                            <asp:UpdatePanel ID="TopPanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table border="0" cellspacing="0" cellpadding="0">
                                        <tr>
                                            
                                            
                                            
                                            <td width="75" class="txtSMBold">
                                                Filter By :
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddFilterBy" runat="server" AutoPostBack ="true" OnSelectedIndexChanged ="ddFilterBy_SelectedIndexChanged" CssClass="inputSM" TabIndex="2">
                                                    <asp:ListItem Selected="True">-- Select Filter --</asp:ListItem>
                                                    <asp:ListItem Value="Customer_No">Customer No</asp:ListItem>
                                                    <asp:ListItem Value="Customer_Name">Customer Name</asp:ListItem>
                                                    <asp:ListItem Value="Address">Address</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtFilterVal" runat="server" ToolTip ="Enter Filter Value" autocomplete="off" CssClass="inputSM"
                                                    TabIndex="3"></asp:TextBox>
                                                <asp:Button ID="btnFilter" runat="server" CausesValidation="False" CssClass="btnInputGrey"
                                                    OnClick="btnFilter_Click" TabIndex="4" Text="Filter" />
                                                    
                                                  <asp:Button ID="btnReset" runat="server" CausesValidation="False" CssClass="btnInputRed"
                                                    TabIndex="5" Text="Reset" />
                                                    
                                                <asp:Button ID="btnAdd" runat="server" CausesValidation="false" CssClass="btnInputBlue"
                                                    OnClick="btnAdd_Click" TabIndex="1" Text="Import" />
                                            
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
                                  
                                                <asp:GridView Width="100%" ID="gvLatitude" runat="server" EmptyDataText="No records to Display"
                                                    EmptyDataRowStyle-Font-Bold="true"  AutoGenerateColumns="False"
                                                    AllowPaging="True" AllowSorting="true"  PageSize="25" CellPadding="0" CellSpacing="0" CssClass="tablecellalign">
                                                   
                                                   
                                                    <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
                                                    <Columns>
                                                        <asp:TemplateField>
                                                           <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                                                                           
                                                                <asp:ImageButton ID="btnEdit" ToolTip="Edit Geolocation Data" runat="server" CausesValidation="false"
                                                                       ImageUrl="~/images/edit-13.png"    OnClick="btnEdit_Click" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Customer_No" HeaderText="Customer No" SortExpression="Customer_No">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="CustName" HeaderText="Customer Name" SortExpression="CustName">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        
                                                         <asp:BoundField DataField="Address" HeaderText="Address" SortExpression="Address">
                                                          
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="CustLat" DataFormatString="{0:N6}" HeaderText="Latitude"
                                                            SortExpression="CustLat">
                                                            <ItemStyle Wrap="False" HorizontalAlign ="Left" />
                                                               <HeaderStyle HorizontalAlign ="Left" />
                                                        </asp:BoundField>
                                                         <asp:BoundField DataField="CustLong" DataFormatString="{0:N6}" HeaderText="Longitude"
                                                            SortExpression="CustLong" >
                                                            <ItemStyle Wrap="False" HorizontalAlign ="Left" />
                                                            <HeaderStyle HorizontalAlign ="Left" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCusId" runat="server" Text='<%# Bind("Customer_ID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSiteId" runat="server" Text='<%# Bind("Site_Use_ID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                   <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
                                                </asp:GridView>
                                      
                                    <asp:Button ID="btnHiddenCurrency" CssClass="btnInput" runat="Server" Style="display: none" />
                                    <ajaxToolkit:ModalPopupExtender BackgroundCssClass="modalBackground" ID="MPECurrency"
                                        runat="server" PopupControlID="DetailPnl" TargetControlID="btnHiddenCurrency"
                                        CancelControlID="btnCancel">
                                    </ajaxToolkit:ModalPopupExtender>
                                    <asp:Panel ID="DetailPnl" runat="server" Style="display: none" Width="300" CssClass="modalPopup">
                                        <asp:Panel ID="DragPnl" runat="server" Style="cursor: move; background-color:  #337AB7  ;
                                            text-align: center; border: solid 1px  #337AB7  ;font-family:Calibri,Tahoma; font-weight:bold; font-size:13px;  color: White; padding: 3px" Width="293">
                                            Edit Geolocation Data </asp:Panel>
                                                                        
                                        <asp:HiddenField ID="HidVal" runat="server" Value="-1" />
                                        <asp:HiddenField ID="hidUseId" runat="server" Value="-1" />
                                        <table  width ="100%" cellpadding ="2" cellspacing ="2">
                                            <tr>
                                                <td  class="txtSMBold" >
                                                    Latitude 
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtLatitude" runat="server" TabIndex="1" CssClass="inputSM"
                                                       ></asp:TextBox>
                                                </td>
                                                </tr>
                                            <tr>
                                                <td   class="txtSMBold">
                                                    Longitude 
                                                </td>
                                                <td>
                                                    <asp:TextBox  ID="txtLongitude" TabIndex="2" CssClass="inputSM" runat="server"></asp:TextBox>
                                                </td>
                                                <%--<asp:RequiredFieldValidator Display="None" Text=" " ControlToValidate="txtDescription"
                                                    ID="ReqDescription" runat="server" ErrorMessage="Description Required"></asp:RequiredFieldValidator>--%></tr>
                                      
                                            <tr>
                                                <td colspan ="2">
                                                    <asp:Panel ID="Panel12" runat="server" Style="display: none" CssClass="overlay">
                                                        <img alt="Processing..." src="../images/Progress.gif" style="z-index: 10010; vertical-align: middle;" />
                                                        <span style="font-size: 12px; color:  #337AB7  ;">Processing... </span>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                           <td></td>
                                                <td >
                                                     
                                                    <asp:Button ID="btnUpdate" CssClass="btnInputGreen" OnClick="btnUpdate_Click"
                                                        runat="server" Text="Update"  />
                                                    <asp:Button ID="btnCancel" CssClass="btnInputRed" TabIndex="6" OnClientClick="return DisableValidation()"
                                                        runat="server" CausesValidation="false" Text="Cancel" />
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
                                                    background-color:  #337AB7  ; text-align: center; border: solid 1px  #337AB7  ; color: White;
                                                    padding: 3px; display:none;">
                                                    <table id="tableinPopupErr" width="400" cellpadding="10" style="background-color: White;">
                                                        <tr align="center">
                                                            <td align="center" style="cursor: move; background-color:  #337AB7  ; text-align: center;
                                                                border: solid 1px  #337AB7  ; color: White; padding: 3px">
                                                                 <asp:Label ID="lblinfo" runat="server" Font-Bold ="true" Font-Size ="13px"   ></asp:Label>
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
                            <span style="font-size: 12px; color:  #337AB7  ;">Processing... </span>
                        </asp:Panel>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <br />
                <br />
            </td>
        </tr>
    </table>
</asp:Content>
