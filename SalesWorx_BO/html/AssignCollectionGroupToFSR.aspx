<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="AssignCollectionGroupToFSR.aspx.vb" Inherits="SalesWorx_BO.AssignCollectionGroupToFSR" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


  

 <asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">

.rcbSlide
{
	z-index: 100002 !important;
}
</style>

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

            var Filter = /ddFilterBy/
            var AddString = postBackElement.id.search(Filter);
            if (AddString == -1) {
                var myRegExp = /_btnUpdate/;
                var myRegExp1 = /btnSaveAcc/
                var AddString = postBackElement.id.search(myRegExp);
                var EditString = postBackElement.id.search(myRegExp1);
                if (AddString != -1 || EditString != -1) {
                    
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
                var myRegExp1 = /btnSaveAcc/
                var AddString = postBackElement.id.search(myRegExp);
                var EditString = postBackElement.id.search(myRegExp1);

                if (AddString != -1 || EditString != -1) {
                    
                }
                else {
                    $get('<%= Me.UpdateProgress1.ClientID %>').style.display = 'none';
                }
                postBackElement.disabled = false;
            }
        }

       

    </script>

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
                return confirm('Would you like to confirm the selected Stock Requisitions?');
                return true;
            }
            alert('Select at least one Stock Requisition!');
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


        function alertCallBackFn(arg) {

        }
    </script>
</asp:Content>
   <asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
   
     <h4>Bill Collector Assignment</h4>
                     <telerik:RadAjaxLoadingPanel  ID="AjaxLoadingPanel1" runat="server" >
             <div ID="ProgressPanel" class ="overlay"  >
     
         <img src="../assets/img/ajax-loader.gif" alt="Processing..." />      
       </div>
           </telerik:RadAjaxLoadingPanel>  

        <asp:Panel ID="Panel1" runat="server">      </asp:Panel> 
                         
 
    
                                         
                     <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>    
                      <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                           
             
     
                            <asp:UpdatePanel ID="TopPanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                <asp:Label ID="lblSelectedID" runat ="server" Visible ="false" ></asp:Label>
                                              <asp:Label ID="lblRemovedID" runat ="server" Visible ="false" ></asp:Label>
                                     
                                           
                                            <div class="row">
                                           
                                            <div class="col-sm-4">
                                               <label>
                                               Organization</label>
                                             
                                                        <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddl_org" Width ="100%" runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="True" EmptyMessage="Select Organisation" >
                                        </telerik:RadComboBox>
                                                
                                                </div>
                                                 <div class="col-sm-4">
                                            <label>Van/FSR </label>
                                           <asp:DropDownList ID="ddlSalesRep" runat="server" width="250" AutoPostBack="true">
                                                    
                                                    </asp:DropDownList>
                                           </div>
                                                </div>

                                                 
                                                     
                                    <br />
                                    <br />

                                                 <table width="100%">
                                        <tr>
                                            <td width="49%">
                                                <asp:Label ID="lblProdAvailed" Font-Bold="true" ForeColor ="#337AB7" runat="server" Text=""></asp:Label>
                                            </td>
                                            <td width="2%">
                                            </td>
                                            <td width="49%">
                                                <asp:Label Font-Bold="true" ID="lblProdAssign" ForeColor ="#337AB7" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="49%">
                                              
                                              

                                                  <telerik:RadListBox ID="lstDefault"  ToolTip ="Press CTRl key for multiple selection"   runat ="server" 
                                                        Width="100%" Height ="290px"   SelectionMode ="Multiple"   >
          
            

        </telerik:RadListBox>

                                            </td>


                                             <td  width="2%">
                <table border ="0" width="2%">
                   
                    <tr><td><asp:ImageButton runat ="server" ID="imgRemoveSelected" BorderStyle ="None" 
                         ToolTip ="Move selected item to right"   ValidationGroup="valsum"  OnClick ="imgAddSlected_Click"   ImageUrl="~/Images/arrowSingleRight.png" /></td></tr>
                   
                    <tr><td>  
                      <asp:ImageButton runat ="server" ID="imgAddSlected"   ValidationGroup="valsum"  BorderStyle ="None"  ToolTip ="Move selected item to left" OnClick ="imgRemoveSlected_Click"
                              ImageUrl="~/Images/arrowSingleLeft.png" /></td></tr>
                    <tr>
                        <td >
                  <asp:ImageButton runat ="server" ID="imgMoveAllLeft" BorderStyle ="None"    ValidationGroup="valsum"
                       ToolTip ="Move all item to left" OnClick ="imgMoveAllRight_Click" ImageUrl="~/Images/doubleRight.png" /></td>
                        </tr> 
                    <tr>
                        <td >
                  <asp:ImageButton runat ="server" ID="imgMoveAllRight"    ValidationGroup="valsum"  BorderStyle ="None"  OnClick ="imgMoveAllLeft_Click"  ToolTip ="Move all Item to right" ImageUrl="~/Images/doubleLeft.png" /></td>
                        </tr> 
                    </table> 
                  </td> 

                                       
                                            <td width="49%">
                                                

                                                  <telerik:RadListBox ID="lstSelected"    ToolTip ="Press CTRl key for multiple selection"      runat ="server"  
                                                       Width="100%" Height ="290px"   SelectionMode ="Multiple"   >
          
            

        </telerik:RadListBox>
                                            </td>
                                        </tr>
                                    </table>
                                   
                                </ContentTemplate>
                                <Triggers>
                                
                                 
                                </Triggers>
                            </asp:UpdatePanel>
                      
                
                 </ContentTemplate>
                 </asp:UpdatePanel>
                
                <asp:UpdatePanel ID="ClassUpdatePnl" runat="server" UpdateMode="conditional">
                                <ContentTemplate>
                 <table>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnHidden" runat="Server" Style="display: none" />
                                                <ajaxToolkit:ModalPopupExtender ID="MpInfoError" runat="server" BackgroundCssClass="modalBackground"
                                                    TargetControlID="btnHidden" PopupControlID="pnlConfirm" CancelControlID="btnClose"
                                                    Drag="true" />
                                                <asp:Panel ID="pnlConfirm" Width="400" runat="server" CssClass="modalPopup" Style="display: none">
                                                    <table id="tableinPopupErr" cellpadding="10" style="background-color: White;width:100%">
                                                        <tr align="center">
                                                            <td align="center" style="cursor: move; background-color: #3399ff; text-align: center;
                                                                border: solid 1px #3399ff; color: White; padding: 3px">
                                                                <asp:Label ID="lblinfo" runat="server" Font-Size ="13px"></asp:Label></td></tr><tr>
                                                            <td align="center" style="text-align: center">
                                                                <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="List" />
                                                                <asp:Label ID="lblMessage" runat="server" Font-Size ="13px"></asp:Label></td></tr><tr>
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