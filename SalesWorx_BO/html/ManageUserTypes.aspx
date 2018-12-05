<%@ Page Title="Manage User Types" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master"
    CodeBehind="ManageUserTypes.aspx.vb" Inherits="SalesWorx_BO.ManageUserTypes" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

     <script runat="server">
        Protected Sub RadTreeView1_NodeDataBound(ByVal sender As Object, ByVal e As RadTreeNodeEventArgs) Handles RadTreeView1.NodeDataBound
      
            e.Node.Text = Server.HtmlEncode(e.Node.Text)
           
        End Sub
 
      
    </script>

    <style type="text/css">
       ul.treeview-list
{
    list-style:none;
    padding:0;
    margin:0;
}
 
li.treeview-item
{
    float:left;
    width:228px;
    padding-right:4px;
    border-left: solid 1px #b1d8eb;
}
 
div.text
{
    font: 13px 'Segoe UI' , Arial, sans-serif;
    color: #4888a2;
    padding: 6px 18px;
    display: block;
}
    </style>
    </asp:Content>
    <asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
     <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">

  <script type="text/javascript">

      function pageLoad(sender, eventArgs) {

          if (!eventArgs.get_isPartialLoad()) {

              $find("<%= RadAjaxManager2.ClientID%>").ajaxRequest("InitialPageLoad");

         }

     }


     function alertCallBackFn(arg) {

     }

     function RadConfirm(sender, args) {
         var callBackFunction = Function.createDelegate(sender, function (shouldSubmit) {
             if (shouldSubmit) {
                 this.click();
             }
         });

         var text = "Are you sure you want to delete?";
         radconfirm(text, callBackFunction, 350, 150, null, "Confirmation");
         args.set_cancel(true);
     }

</script> 

    
          </telerik:RadScriptBlock> 
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <h4>Manage User Types</h4>
    <telerik:RadAjaxManager ID="RadAjaxManager2" runat="server">
               <AjaxSettings>
                   <telerik:AjaxSetting AjaxControlID="RadAjaxManager2">
                       <UpdatedControls>
                           <telerik:AjaxUpdatedControl ControlID="Panel1" LoadingPanelID="AjaxLoadingPanel1" />
                       </UpdatedControls>
                   </telerik:AjaxSetting>
               </AjaxSettings>
           </telerik:RadAjaxManager>
        
            
           <telerik:RadAjaxLoadingPanel  ID="AjaxLoadingPanel1" runat="server" >
             <div ID="ProgressPanel" class ="overlay"  >
     
         <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index:10010;vertical-align:middle;" />      
       </div>
           </telerik:RadAjaxLoadingPanel>

        <asp:Panel ID="Panel1" runat="server">    </asp:Panel> 
                         
                                      <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server"  LoadingPanelID ="AjaxLoadingPanel1"  >
                     <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>
            <div class="row">

           <div class="col-sm-6 col-md-5 col-lg-4">
                 <div class="form-group">
                                                 <label>User Type  
                                     
                                </label>
                                  <telerik:RadTextBox ID="txtUserType"  Skin="Simple"  runat="server"   Width ="100%"    TabIndex ="1" Visible="False">
                                    </telerik:RadTextBox>
                                    
                                      <telerik:RadComboBox ID="drpUserTypes"  Skin="Simple"   Width ="100%"          Filter="Contains"    runat="server" AutoPostBack="True" >
                                               </telerik:RadComboBox>
          </div> 
               </div>
                </div>
                                          <div class="row">

           <div class="col-sm-6 col-md-5 col-lg-4">
                                           
                                        <div class="form-group">
                                 <label>
                                    Designation
                                </label>
                                   
                                             <telerik:RadComboBox ID="drpDesignation"  Skin="Simple"   Width ="100%"          Filter="Contains"    runat="server" AutoPostBack="True" onclick="uncheckAll();">
                                               </telerik:RadComboBox>
                               </div> 
               </div>
                </div>
                                          <div class="row">

           <div class="col-sm-6 col-md-5 col-lg-4">
                                        
                                                 <div class="form-group">
                                                 <label>Back Office Rights 
                                     
                                </label>
         
                                   <telerik:RadTreeView ID="RadTreeView1" runat="server" Width="100%"
               CheckBoxes="True" skin="Simple"
                TriStateCheckBoxes="true" 
               OnNodeDataBound="RadTreeView1_NodeDataBound">
                <DataBindings>
                    <telerik:RadTreeNodeBinding Expanded="false"></telerik:RadTreeNodeBinding>
                </DataBindings>
                    
            </telerik:RadTreeView>

       
    </div>
               </div>
                </div>
                                          <div class="row">

           <div class="col-sm-6 col-md-5 col-lg-4">
                                                <div class="form-group">
     
               <asp:Panel ID="pnlDefault" runat="server">
                                 
                                                    <telerik:RadButton ID="btnAddNew"  Skin ="Simple" TabIndex="4"  Text="Add" runat ="server" CssClass ="btn btn-success" ></telerik:RadButton>
                                                     <telerik:RadButton ID="btnModify"  Skin ="Simple" TabIndex="5"  runat="server" Text="Modify" CssClass ="btn btn-primary"  />
                                                    <telerik:RadButton ID="btnDelete"  Skin ="Simple" TabIndex="6" runat="server" CssClass ="btn btn-danger" Text="Delete" OnClientClicking="RadConfirm" 
                                                           OnClick="btnDelete_Click" />
                                                </asp:Panel>
                                                <asp:Panel runat="server" ID="pnlAdd"  Visible="false">
                                                     <telerik:RadButton ID="btnSave"   Skin ="Simple" TabIndex="7" runat="server" Text="Save" CssClass ="btn btn-success" />
                                                    <telerik:RadButton ID="btnCancel"  Skin ="Simple" TabIndex="8"  runat="server" Text="Cancel"  CssClass ="btn btn-default"  />
                                                </asp:Panel>
                                                <asp:Panel runat="server" ID="pnlModify" Visible="false">
                                                     <telerik:RadButton ID="btnUpdate"  Skin ="Simple"  TabIndex="9" runat="server" Text="Update" CssClass ="btn btn-success"  />
                                                     <telerik:RadButton ID="btnCancelM"  Skin ="Simple" TabIndex="10"  runat="server" Text="Cancel" CssClass ="btn btn-danger" />
                                                </asp:Panel>
            
       <asp:Label ID="lblMsg" runat="server" Font-Bold="True" ForeColor="Maroon"></asp:Label>
        </div>
               </div>
                </div>
                                          <div class="row">

           <div class="col-sm-6 col-md-5 col-lg-4">
                                          <div class="form-group" id="PDA" runat="server" visible="false">
                                 <label>
                                               PDA Rights
                                            </label>
                                              <telerik:RadComboBox ID="ddlVanRights" Width ="250px" runat="server" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" 
                  EmptyMessage ="Please select"  Localization-CheckAllString ="Select All"  Skin="Simple"   >
               <%-- <Items>
                <telerik:RadComboBoxItem Value ="C" Text ="Collection" />
                <telerik:RadComboBoxItem Value ="D" Text ="Distribution Check" />
                <telerik:RadComboBoxItem Value ="I" Text ="Invoice" />
                <telerik:RadComboBoxItem Value ="O" Text ="Order" />
                <telerik:RadComboBoxItem Value ="R" Text ="Return" />
                
                </Items>--%>
                       </telerik:RadComboBox>
                                        </div>
               </div>
                </div>
                                          
                                         </telerik:RadAjaxPanel> 
                                      
    </asp:Content> 
