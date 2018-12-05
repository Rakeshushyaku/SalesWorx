<%@ Page Title="Van Audit Plan" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="Van_Audit_Plan.aspx.vb" Inherits="SalesWorx_BO.Van_Audit_Plan" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>



<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
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

        function alertCallBackFn(arg) {

        }

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
     <h4>Van/FSR Audit Plan</h4> 
     <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
     </telerik:RadWindowManager>
     <asp:UpdatePanel runat="server" ID="TopPanel" UpdateMode ="Conditional" >
        <ContentTemplate>
            <div class="row">
                <div class="col-sm-3">
            
   <div class="form-group">  <label>Organization </label>
                        <telerik:RadComboBox Skin="Simple" ID="ddOraganisation"  Width ="100%"
                    runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="true"></telerik:RadComboBox> 
       </div>
                       </div>     
                <div class="col-sm-3">
                <div class="form-group">   <label>Auditor </label>
                <telerik:RadComboBox Skin="Simple" ID="ddlUser" AutoPostBack="true"   Width ="100%" runat="server"  CssClass="inputSM"></telerik:RadComboBox>
                           </div>
                    </div>
    
          <div class="col-sm-3">
                             <div class="form-group">
                    <label>Audit Year</label><telerik:RadComboBox Skin="Simple" ID="ddlYear" AutoPostBack="true" 
                                Width ="100%" runat="server"  CssClass="inputSM">

                                                                                                       </telerik:RadComboBox> 
                                 </div>
              </div>
                <div class="col-sm-3">
                           <div class="form-group">
                                <label>Audit Month</label>
                                    <telerik:RadComboBox Skin="Simple" ID="ddlMonth" AutoPostBack="true"  Width ="100%" runat="server"  CssClass="inputSM">

                                    </telerik:RadComboBox>
       </div>
              </div>
    
                </div>
     <table width="100%" border="0" cellspacing="0" cellpadding="0" id="tableform">
                            <tr>
                                <td>
                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td>
                                                <p><asp:Label ID="lblFsrAvailed" Font-Bold="true" runat="server" Text=""></asp:Label></p>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <p><asp:Label Font-Bold="true" ID="lblFsrAssign" runat="server" Text=""></asp:Label></p>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="48%">
                                                 <telerik:RadListBox Rows="20" Height="350" SelectionMode="Multiple"
                                                 
                                                     Width="100%" ID="lstDefault" runat="server">
                                                </telerik:RadListBox>
                                            </td>
                                            <td style="padding:10px; vertical-align:middle;" width="50px">
                                                  <table border="0" cellspacing="0" cellpadding="0" width="100%">
                   
                                                            <tr><td style="padding:3px 0;"><asp:ImageButton runat ="server" ID="btnAdd" BorderStyle ="None" 
                                                                 ToolTip ="Move selected item to right"   ValidationGroup="valsum"  OnClick ="btnAdd_Click"   ImageUrl="~/Images/arrowSingleRight.png" /></td></tr>
                   
                                                            <tr><td style="padding:3px 0;">  
                                                              <asp:ImageButton runat ="server" ID="btnRemove"   ValidationGroup="valsum"  BorderStyle ="None"  ToolTip ="Move selected item to left" OnClick ="btnRemove_Click"
                                                                      ImageUrl="~/Images/arrowSingleLeft.png" /></td></tr>
                                                            <tr>
                                                                <td style="padding:3px 0;">
                                                          <asp:ImageButton runat ="server" ID="btnRemoveAll" BorderStyle ="None"    ValidationGroup="valsum"
                                                               ToolTip ="Move all item to left"  ImageUrl="~/Images/doubleRight.png" /></td>
                                                                </tr> 
                                                            <tr>
                                                                <td style="padding:3px 0;">
                                                          <asp:ImageButton runat ="server" ID="btnAddAll"    ValidationGroup="valsum"  BorderStyle ="None"  
                                                                  ToolTip ="Move all Item to right" ImageUrl="~/Images/doubleLeft.png" /></td>
                                                                </tr> 
                                                            </table> 

                                             
                                            </td>
                                            <td width="48%">
                                              <telerik:RadListBox Rows="20"  Height="350" SelectionMode="Multiple" Width="100%" ID="lstSelected" runat="server">
                                                </telerik:RadListBox>
                                            </td>
                                        </tr>
                                    </table>
                           </td>
                                    </tr>
                        </table>
            </ContentTemplate>
     </asp:UpdatePanel> 
     <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="TopPanel"
                    runat="server">
                    <ProgressTemplate>
                        <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                            <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                            <span style="font-size: 12px; color: #3399ff;">Processing... </span>
                        </asp:Panel>
                    </ProgressTemplate>
                </asp:UpdateProgress>
</asp:Content>
