<%@ Page Title="Edit/Delete Route Plan" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" EnableSessionState="True"
    CodeBehind="ModDelRoutePlan.aspx.vb" Inherits="SalesWorx_BO.ModDelRoutePlan" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

   

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
     <script type="text/javascript" src="../js/RoutePlanner.js"></script>
    <script language="javascript" type="text/javascript">
        function alertCallBackFn(arg) {

        }


        var prm = Sys.WebForms.PageRequestManager.getInstance();

        prm.add_initializeRequest(InitializeRequest);
        prm.add_endRequest(EndRequest);
        var postBackElement;
        function InitializeRequest(sender, args) {

            if (prm.get_isInAsyncPostBack())
                args.set_cancel(true);
            postBackElement = args.get_postBackElement();
            if (postBackElement.id == 'ctl00_MainContent_Route_FSR_ID') {
                $get('MainContent_UpdateProgress1').style.display = 'block';
                postBackElement.disabled = true;
            }

        }

        function EndRequest(sender, args) {
            if (postBackElement.id == 'ctl00_MainContent_Route_FSR_ID') {
                $get('MainContent_UpdateProgress1').style.display = 'none';
                postBackElement.disabled = false;
            }
        } 

    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
   
   
        <h4>Edit/Delete Route Plan</h4>
    <telerik:RadAjaxLoadingPanel ID="AjaxLoadingPanel1" runat="server">
        <div id="ProgressPanel" class="overlay">
            <img src="../assets/img/ajax-loader.gif" alt="Processing..."  />
            <span>Processing... </span> 
        </div>
    </telerik:RadAjaxLoadingPanel>

    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" Skin="Simple" EnableShadow="true">
    </telerik:RadWindowManager>
    
     <input type="hidden" name="Action_Mode" value="" />

                <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="UpPanel" runat="server">
                    <ProgressTemplate>
                        <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                            <img src="../assets/img/ajax-loader.gif" alt="Processing..."  />
                            <span>Processing...
                            </span>
                        </asp:Panel>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <asp:UpdatePanel runat="server" ID="UpPanel">
                    <ContentTemplate>

                            <asp:Panel ID="VanPanel" runat="server">
                                <div class="row">
                                    <div class="col-sm-4">
                                 <div class="form-group">
                                     <label>Van/FSR</label>
                                
                                      <telerik:RadComboBox ID="Route_FSR_ID" runat="server" AutoPostBack="true" DataTextField="SalesRep_Name"
                                            Skin="Simple" Filter="Contains" EmptyMessage="Please type a Van/FSR"
                                            TabIndex ="1" Width ="100%" DataValueField="SalesRep_ID" >
                                           
                                        </telerik:RadComboBox>
                                     </div>
                                 </div>
                                    </div>
                            </asp:Panel>
                            <asp:Panel runat="server" ID="BtnPanel" Visible="false">
                                <div class="row">
                                    <div class="col-sm-4">
                                        <div class="form-group">
                                     <label>Route Plans</label>
                                     <telerik:RadComboBox ID="Route_ID"   TabIndex ="2" Width ="100%" runat="server"  DataTextField="Details"  Skin="Simple"
                                            DataValueField="FSR_Plan_ID">
                                        </telerik:RadComboBox>
                                            </div>
                                 </div>
                                <div class="col-sm-4">
                                    <div class="form-group">
                                        <label class="hidden-xs"><br /></label>
                                        <telerik:RadButton ID="ModBtn"  TabIndex ="3" CssClass="btn btn-primary" Skin="Simple" runat="server" Text="View/Modify" />
                                        <telerik:RadButton ID="DelBtn"  TabIndex ="4" CssClass="btn btn-danger" Skin="Simple" runat="server" Text="Delete" />
                                    </div>
                                </div>
                                </div>
                            </asp:Panel>
                       
                    </ContentTemplate>
                </asp:UpdatePanel>

</asp:Content>
