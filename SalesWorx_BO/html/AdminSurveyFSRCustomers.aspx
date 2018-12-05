<%@ Page Title ="Assign Customers to Van for Survey" Language="vb" AutoEventWireup="false" CodeBehind="AdminSurveyFSRCustomers.aspx.vb"
    MasterPageFile="~/html/Site.Master" Inherits="SalesWorx_BO.AdminSurveyFSRCustomers" %>

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
   
     <h4>Assign Customers to Van for Survey</h4>
      <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
     </telerik:RadWindowManager>

     
     

                <asp:UpdatePanel runat="server" ID="TopPanel" UpdateMode="Conditional">
                    <ContentTemplate>
                       <div class="row">
                           <div class="col-sm-4">
                         <div class="form-group">  <label>
                                               Please select a van 
                                            </label>
                            

                               <telerik:RadComboBox ID="ddlSalesRep" Skin="Simple"   AutoPostBack="true" Filter="Contains" 
                                            Width="100%"  TabIndex="2"
                                            runat="server">
                                </telerik:RadComboBox>

                          </div>
                               </div>
             <div class="col-sm-4">
                       <div class="form-group">
                               <label>Filter By Cust.No  </label> 
                          <telerik:RadTextBox runat ="server" ID="txtFilter" EmptyMessage ="Filter By Cust.No" Skin ="Simple"  Width ="100%"   ></telerik:RadTextBox>
                          </div>
                               </div>
             <div class="col-sm-4">
                       <div class="form-group">    
                           <label><br /></label>      
                                      <telerik:RadButton ID="btnFilter" Skin ="Simple"    runat="server" Text="Search" CssClass ="btn btn-primary" />
                                       <telerik:RadButton ID="btnReset"  Skin ="Simple"     runat="server" Text="Reset" CssClass ="btn btn-default" />
                             <asp:Label ID="lblSelectedID" runat ="server" Visible ="false" ></asp:Label>
                                              <asp:Label ID="lblRemovedID" runat ="server" Visible ="false" ></asp:Label>
                        </div> 
                 </div> 
                    </div>
                        <table border="0" cellspacing="0" cellpadding="0" id="tableform" width="100%">
                            <tr>
                                <td>
                                    <table border="0" cellspacing="0" cellpadding="0" width="100%">
                                        <tr>
                                             <td>
                                                <p><asp:Label ID="lblCustAvailed" Font-Bold="true"  runat="server" Text=""></asp:Label></p>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <p><asp:Label Font-Bold="true" ID="lblCustAssign" runat="server" Text=""></asp:Label></p>
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

    <asp:UpdatePanel ID="UPModal" runat="server">
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
    <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="UpModal" runat="server">
        <ProgressTemplate>
            <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                <img src="../assets/img/ajax-loader.gif" alt="Processing..." />
                <span>Processing... </span>
            </asp:Panel>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
