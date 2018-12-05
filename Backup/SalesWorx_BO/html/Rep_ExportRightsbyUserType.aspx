<%@ Page Title="Export Rights" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master" CodeBehind="Rep_ExportRightsbyUserType.aspx.vb" Inherits="SalesWorx_BO.Rep_ExportRightsbyUserType" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

   
       

    <style type="text/css">
        .overlay
        {
            position: fixed;
            z-index: 1000;
            top: 49%;
            left: 49%;
            width: 100%;
            height: 100%;
        }
        * html .overlay
        {
            position: absolute;
            height: expression(document.body.scrollHeight > document.body.offsetHeight ? document.body.scrollHeight : document.body.offsetHeight + 'px');
            width: expression(document.body.scrollWidth > document.body.offsetWidth ? document.body.scrollWidth : document.body.offsetWidth + 'px');
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
        <tr>
            <td id="contentofpage" width="100%" height="76%" class="topshadow">
                <div class="pgtitileposition">
                    <span class="pgtitile3">Export User Type and Rights</span></div>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table width="auto" border="0" cellpadding="3" cellspacing="10" id="tableformNrml">
                            <tr>
                                <td>
                                    <asp:Label ID="lblmsg" runat="server" Font-Bold="True" ForeColor="Maroon"></asp:Label>
                                    <table cellpadding="4" cellspacing="4">
                                  
                                        <tr>
                                            <td class="txtSMBold">
                                                User Type:
                                            </td>
                                            <td class="txtSMBold">
                                                <asp:DropDownList ID="drpUserTypes"  CssClass="inputSM"   Width ="250px" runat="server" AutoPostBack="True" >
                                                </asp:DropDownList>
                                                
                                                <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="btnInputGreen" />
                                            </td>
                                           
                                        </tr>  
                                        
                                                  <tr>
                                            <td class="txtSMBold" valign="top" >
                                               
                                                    
                                                    
                                                
                                                Back Office Rights
                                               
                                                    
                                                    
                                                
                                            </td>
                                          
                                            <td> 
                                                <telerik:RadTreeList ID="TreeViewRights" runat="server" 
                                                    AllowLoadOnDemand="false" AllowPaging="true" AutoGenerateColumns="false" 
                                                    ClientDataKeyNames="ID" DataKeyNames="ID" Height="650" PageSize="35" 
                                                    ParentDataKeyNames="PID" Skin="Vista" width="850">
                                                    <Columns>
                                                        <telerik:TreeListBoundColumn DataField="Name" HeaderText="User Type" 
                                                            UniqueName="UserType">
                                                        </telerik:TreeListBoundColumn>
                                                        <telerik:TreeListBoundColumn DataField="value" HeaderText="User Rights" 
                                                            UniqueName="Title">
                                                        </telerik:TreeListBoundColumn>
                                                    </Columns>
                                                    <ClientSettings AllowItemsDragDrop="true">
                                                        <scrolling allowscroll="true" savescrollposition="true" scrollHeight="550" 
                                                            usestaticheaders="true" />
                                                    </ClientSettings>
                                                    <PagerStyle Mode="NextPrevAndNumeric" />
                                                </telerik:RadTreeList>
                                                <asp:XmlDataSource ID="XmlDataSource1" runat="server" 
                                                    DataFile="~/xml/UserRights.xml" TransformFile="~/xml/XSLTFile1.xsl" 
                                                    XPath="/*/*"></asp:XmlDataSource>
                                                      </td>
                                          
                                        </tr>
                              
                                    </table>
                                </td>
                              
                            </tr>
                           
                            <tr>
                               
                                <td>
                                    <asp:Button ID="btnHidden" runat="Server" Style="display: none" />
                                    <ajaxToolkit:ModalPopupExtender ID="MpInfoError" runat="server" BackgroundCssClass="modalBackground"
                                        TargetControlID="btnHidden" PopupControlID="pnlConfirm" CancelControlID="btnClose"
                                        Drag="true" />
                                    <asp:Panel ID="pnlConfirm" Width="400" runat="server" CssClass="modalPopup" Style="display: none">
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
                        <asp:PostBackTrigger ControlID="btnExport" /> 
                      </Triggers>
                </asp:UpdatePanel>
                <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="UpdatePanel1" runat="server">
                    <ProgressTemplate>
                        <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                            <img src="../images/Progress.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                            <span style="font-size: 12px; font-weight: 700; color: #3399ff;">Processing... </span>
                        </asp:Panel>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </td>
        </tr>
    </table>
</asp:Content>
