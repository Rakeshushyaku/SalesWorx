<%@ Page Title="Survey Question" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master"
    CodeBehind="AdminSurveyQues.aspx.vb" Inherits="SalesWorx_BO.AdminSurveyQues" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">

    <script type="text/javascript">
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

        <h4>Survey Questions</h4>

 <telerik:RadAjaxLoadingPanel  ID="AjaxLoadingPanel1" runat="server" >
             <div ID="ProgressPanel" class ="overlay"  >
     
         <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index:10010;vertical-align:middle;" />      
       </div>
           </telerik:RadAjaxLoadingPanel>  

        <asp:Panel ID="Panel2" runat="server">      </asp:Panel> 
      <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
     </telerik:RadWindowManager>


      
                            <asp:UpdatePanel ID="TopPanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-sm-4">
                                            <div class="form-group">
                                                <label id="lblSurvey" runat="server" >Survey</label>
                                             
                                                 <telerik:RadComboBox ID="ddSurvey" Skin="Simple"    Filter="Contains"  AutoPostBack="true"   Width="100%" Height="250px" TabIndex="2"
                                                        runat="server"> </telerik:RadComboBox>
                                                
                                            </div>
                                        </div>
                                        <div class="col-sm-4">
                                            <div class="form-group">
                                                <label id="lblQuestion1" runat="server" >Question</label>
                                             
                                                <telerik:RadComboBox ID="ddlQuestions" Skin="Simple" AppendDataBoundItems="true"   AutoPostBack="true"   Width="100%" Height="250px" TabIndex="2"
                                                        runat="server"> 
                                                    <Items>

                                                            <telerik:RadComboBoxItem Selected="True" Text="--Select a Question--" Value=""></telerik:RadComboBoxItem>                                                           
                                                     </Items> 

                                                </telerik:RadComboBox>                     
                                                
                                            </div>
                                        </div>
                                        <div class="col-sm-4">
                                            <div class="form-group">
                                                <label id="lblResponse" runat="server" >Default Response</label>
                                             
                                                <telerik:RadComboBox ID="ddlResponses"   Filter="Contains" Skin="Simple" AppendDataBoundItems="true"   AutoPostBack="true"   Width="100%" Height="250px" TabIndex="2"
                                                        runat="server"> 
                                                    <Items>

                                                            <telerik:RadComboBoxItem Selected="True" Text="--Select a Response--" Value=""></telerik:RadComboBoxItem>                                                           
                                                     </Items> 

                                                </telerik:RadComboBox>                     
                                                
                                            </div>
                                        </div>
                                    </div>

                                  <div class="row" id="divQues" runat="server" visible ="false">
                                        <div class="col-sm-4">
                                            <div class="form-group">
                                                <label id="lblQuestion" runat="server" >Question</label>
                                             
                                                 <asp:TextBox ID="txtQuestion" runat="server"   Filter="Contains" Width ="100%" CssClass="inputSM" TabIndex="2" Rows="3"
                                                    TextMode="MultiLine"></asp:TextBox>
                                                
                                            </div>
                                        </div>

                                    </div>

                                    <div class="row">
                                        <div class="col-sm-12">
                                            <div class="form-group">
                                               <telerik:RadButton ID="btnAdd"  OnClick="btnAdd_Click"  Skin ="Simple" TabIndex="5"   runat="server" Text="Add" 
                                                   CssClass ="btn btn-success" />
                                              <telerik:RadButton ID="btnSave"  OnClick="btnSave_Click"  Skin ="Simple" TabIndex="6"   runat="server" Text="Save" 
                                                   CssClass ="btn btn-success" />
                                             <telerik:RadButton ID="btnModify"  OnClick="btnModify_Click"  Skin ="Simple" TabIndex="7"   runat="server" Text="Modify" 
                                                   CssClass ="btn btn-warning" />
                                               <telerik:RadButton ID="btnDelete"  OnClick="btnDelete_Click"  Skin ="Simple" TabIndex="8"   runat="server" Text="Delete" 
                                                   CssClass ="btn btn-danger" />
                                            <telerik:RadButton ID="btnCancel"  OnClick="btnCancel_Click"  Skin ="Simple" TabIndex="9"   runat="server" Text="Cancel" 
                                                   CssClass ="btn btn-default" />
                                                </div>
                                        </div>
                                    </div>

                                </ContentTemplate>
                            </asp:UpdatePanel>
         
                            <asp:UpdatePanel ID="UPModal" runat="server" >
                                <ContentTemplate>
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
                                    </td> </tr> </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
              
           
     
           <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="UPModal" runat="server">
                    <ProgressTemplate>
                        <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                            <img src="../assets/img/ajax-loader.gif" alt="Processing..." />
                            <span>Processing... </span>
                        </asp:Panel>
                    </ProgressTemplate>
                </asp:UpdateProgress>

</asp:Content>
