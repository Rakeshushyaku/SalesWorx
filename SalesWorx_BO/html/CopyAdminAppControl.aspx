<%@ Page Title="Application Control" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master"
    CodeBehind="CopyAdminAppControl.aspx.vb" Inherits="SalesWorx_BO.CopyAdminAppControl" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="FeaturedContent" runat="server">
    <script type="text/javascript">

        function alertCallBackFn(arg) {

        }
    </script>
    <script language="javascript" src="../js/controlparamsHandler.js"></script>
<style>
    .tablecellalign td .RadComboBox { margin-bottom:3px; }
</style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h4>Application Control</h4>

         <telerik:RadAjaxLoadingPanel  ID="AjaxLoadingPanel1" runat="server" >
             <div ID="ProgressPanel" class ="overlay"  >
     
         <img src="../assets/img/ajax-loader.gif" alt="Processing..." />  
                 <span>Processing... </span>     
       </div>
           </telerik:RadAjaxLoadingPanel>  

        <asp:Panel ID="Panel1" runat="server">      </asp:Panel> 
                         
 
    
                                         
                     <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>
                <p id="pagenote">
                    The Application Control Screen may be used for controlling the additional modules and features of SalesWorx.</p>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <input type="hidden" name="SUB_POINT" id="SUB_POINT" value="">
                       
                                   
                                     <label><asp:Label ID="lblMsg" runat="server" Font-Bold="True" ForeColor="Maroon"></asp:Label></label>
                                     <label><asp:Label ID="ControlParamsTree" Visible="false" runat="server" Text="Label"></asp:Label></label>
                                     <div class="form-group">
                                     <label>Parameter Type</label>
                                     <div class="row">
                                         <div class="col-sm-6 col-md-4">
                                             <div class="form-group">
                                               <telerik:RadComboBox ID="ddlFilterBy" Skin="Simple" AutoPostBack="true" height="200px" TabIndex ="1" runat="server" Width="100%"></telerik:RadComboBox>
                                             </div>
                                         </div>
                                         <div class="col-sm-6 col-md-8">
                                             <div class="form-group">
                                               <telerik:RadButton ID="Button1" Skin="Simple" CssClass="btn btn-success" runat="server" Text="Update" TabIndex ="6"></telerik:RadButton>
                                             </div>
                                         </div>
                                     </div>
                                 
                                        <div class="table-responsive">
                                               <asp:UpdatePanel ID="ClassUpdatePnl" runat="server" UpdateMode="conditional">
                                                    <ContentTemplate>
                                                        <asp:GridView Width="100%" ID="gvParams" runat="server"  AutoGenerateColumns="false"
                                                            PageSize="25" CellPadding="0" CellSpacing="0"  CssClass="tablecellalign" DataKeyNames="Control_Key" OnRowDataBound="RowDataBound">
                                                           
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblParam"  runat="server" Text="Parameter"  />
                                                                    </HeaderTemplate>
                                                                    <HeaderStyle HorizontalAlign="left"  Width="50%" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblParamText"  CssClass ="txtSM" runat="server" Text='<%# Bind("ControlText") %>' />
                                                                        <asp:Label ID="lblControlType" Font-Bold="false" runat="server" Visible="false" Text='<%# Bind("ControlType") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="left" VerticalAlign="middle" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblValue" runat="server" Text="Value" />
                                                                    </HeaderTemplate>
                                                                    <HeaderStyle HorizontalAlign="left" Width="50%" />
                                                                    <ItemTemplate>
                                                                        <telerik:RadComboBox Skin="Simple" ID="drpValue" Width="100%" runat="server" DataTextField="Description"
                                                                            DataValueField="Code">
                                                                        </telerik:RadComboBox>
                                                                        <telerik:RadComboBox Skin="Simple" ID="chkMulti" TabIndex="9" runat="server" Height="200" Width="100%"
                                                                            CheckBoxes="true" CheckedItemsTexts="DisplayAllInInput" EnableCheckAllItemsCheckBox="true"
                                                                            ForeColor="Black" Localization-CheckAllString="All" EmptyMessage="Please Select"
                                                                            DataTextField="Description" DataValueField="Code">
                                                                        </telerik:RadComboBox>
                                                                        <asp:CheckBox ID="ChkValue" runat="server" Text="" TabIndex="1"  />
                                                                     <telerik:RadTimePicker ID="RTP"  runat="server"  MinDate="1900-01-01">
                                                                  <timeview cellspacing="-1"  TimeFormat = "HH:mm"    interval="00:30:00">
                                                </timeview>
                                                <timepopupbutton hoverimageurl="" imageurl="" />
                                                <datepopupbutton hoverimageurl="" visible="false" imageurl="" tabindex="0" />
                                               
                                               
                                                <dateinput readonly="true" dateformat="dd-MM-yyyy HH:mm" displaydateformat="HH:mm"/>
        </telerik:RadTimePicker>
                                                                        
                                                                        <asp:TextBox ID="txtValue" TabIndex="1" 
                                                                            CssClass="txtSM" runat="server"></asp:TextBox>
                                                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server"
                                                                            TargetControlID="txtValue" ValidChars="." FilterType="Numbers,Custom">
                                                                        </ajaxToolkit:FilteredTextBoxExtender>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="left" VerticalAlign="middle" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                           <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                             </div>
                        <asp:Button ID="btnHidden" runat="Server" Style="display: none" />
                        <ajaxToolkit:ModalPopupExtender ID="MpInfoError" runat="server" BackgroundCssClass="modalBackground"
                            TargetControlID="btnHidden" PopupControlID="pnlConfirm" CancelControlID="btnClose"
                            Drag="true" />
                        <asp:Panel ID="pnlConfirm" Width="400" runat="server" CssClass="modalPopup" Style="display: none">
                            <table id="tableinPopupErr" width="400" cellpadding="10" style="background-color: White;">
                                <tr align="center">
                                    <td align="center" style="cursor: move; background-color: #3399ff; text-align: center;
                                        border: solid 1px #3399ff; color: White; padding: 3px">
                                        <asp:Label ID="lblinfo" runat="server" Font-Size="14px"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="text-align: center">
                                        <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="List" />
                                        <asp:Label ID="lblMessage" runat="server" Font-Size="13px" Font-Names="Calibri"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="text-align: center;">
                                        <asp:Button ID="btnClose" runat="server" Text="Ok" CssClass="btnInputBlue" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            
</asp:Content>
