<%@ Page Title="Manage Users" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master"
    CodeBehind="ManageUsers.aspx.vb" Inherits="SalesWorx_BO.ManageUsers" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="FeaturedContent" runat="server">
    <script src="../Scripts/jquery.tokeninput.js" type="text/javascript"></script>
    <script src="../Scripts/json2.js" type="text/javascript"></script>
  <link href="../styles/token-input.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/token-input-mac.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/token-input-facebook.css" rel="stylesheet" type="text/css" />
    <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
 
<script type="text/javascript">

    function pageLoad(sender, eventArgs) {

        if (!eventArgs.get_isPartialLoad()) {

            $find("<%= RadAjaxManager2.ClientID%>").ajaxRequest("InitialPageLoad");

        }

    }

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

     function alertCallBackFn(arg) {

     }

     function ConfirmDelete(msg, event) {

         var ev = event ? event : window.event;
         var callerObj = ev.srcElement ? ev.srcElement : ev.target;
         var callbackFunctionConfirmDelete = function (arg, ev) {
             if (arg) {
                 callerObj["onclick"] = "";
                 if (callerObj.click) callerObj.click();
                 else if (callerObj.tagName == "A") {
                     try {
                         eval(callerObj.href)
                     }
                     catch (e) { }
                 }
             }
         }
         radconfirm(msg, callbackFunctionConfirmDelete, 330, 100, null, 'Confirmation');
         return false;
     }

</script> 
         </telerik:RadScriptBlock> 
       <style type="text/css">
        div.RadGrid .rgPager .rgAdvPart
        {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h4>Manage Users</h4>
    
      <telerik:RadAjaxManager ID="RadAjaxManager2" runat="server">
               <AjaxSettings>
                   <telerik:AjaxSetting AjaxControlID="RadAjaxManager2">
                       <UpdatedControls>
                        
                       </UpdatedControls>
                   </telerik:AjaxSetting>
               </AjaxSettings>
           </telerik:RadAjaxManager>
        
            
           <telerik:RadAjaxLoadingPanel  ID="AjaxLoadingPanel1" runat="server" >
             <div ID="ProgressPanel" class ="overlay"  >
     
         <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index:10010;vertical-align:middle;" />      
       </div>
           </telerik:RadAjaxLoadingPanel>

    <asp:Panel ID="Panel2" runat="server"></asp:Panel> 

 
    <asp:UpdatePanel ID="UpdatePanel1" runat="server"  >
    
        <ContentTemplate>
            <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>

             <asp:Label ID="lblMsg" runat="server" Font-Bold="True" ForeColor="Maroon"></asp:Label>
             <div class="row">
            <div class="col-sm-4">
            <div class="form-group">
                   
<label>
                                    User 
                                 </label>
                                  
                              
                                    
                                         <telerik:RadComboBox ID="drpUser"  Skin="Simple"  Width="100%"      
                                                Filter="Contains"    runat="server" AutoPostBack="True" >
                                               </telerik:RadComboBox>

                                    <telerik:RadTextBox ID="txtUser"  Skin="Simple"  runat="server"   Width ="100%"   TabIndex ="1" Visible="False">
                                    </telerik:RadTextBox>


                
                 </div>
                </div>
 <div class="col-sm-4">
                  <div class="form-group">
                        <label> 
                                    Password
                                 </label>
                                    <asp:TextBox ID="txtPwd" Width="100%" runat="server" TextMode="Password"></asp:TextBox>
                                </div>
     </div>
                 <div class="col-sm-4">
                     <div class="form-group">
                        <label> 
                                   Designation
                                </label>
                                  
                                   <telerik:RadComboBox ID="drpDesignation"  Skin="Simple"  Width="100%"     
                                            Filter="Contains"    runat="server" AutoPostBack ="true" >
                                               </telerik:RadComboBox>

                
                 </div>
          
                     </div>
                 </div>
           <div class="row">
                <div class="col-sm-4">
            <div class="form-group">
                        <label>  
                                   User Type  
                                </label>
                                    
                                     <telerik:RadComboBox ID="drpUserType"  Skin="Simple"   Width="100%"      
                                            Filter="Contains"    runat="server">
                                               </telerik:RadComboBox>
                                 </div>
                    </div>
                <div class="col-sm-4">
               <div class="form-group">
                   <label>
                                     <asp:Label ID="lbl_Van1" runat="server" Text="Van/FSR"></asp:Label> </label>
                               
                      <telerik:RadComboBox ID="drpSalesRep"  Skin="Simple"  Width="100%"    
                                            Filter="Contains"    runat="server">
                                               </telerik:RadComboBox>
                                    
                              </div>
                    </div>
                <div class="col-sm-4">
                 
               </div>
               </div>
            <div class="row" id="Orgdiv"  visible="false" runat="server" >
                  <div class="col-sm-4">
                     <div class="form-group">
                         <label>
                             <asp:Label runat="server" ID="lbl_Org" Text="Organization" Visible="false"></asp:Label></label>
                              
                  <telerik:RadComboBox ID="ddl_org"  Skin="Simple"  Width="100%"    
                                            Filter="Contains"    runat="server" Visible="false"  AutoPostBack ="true">
                                               </telerik:RadComboBox>
                     </div>
                     </div>
                </div>
                
            <div class="row" visible="false"  id="SalesRepdiv" runat="server" >
                  <div class="col-sm-4">
                 <div class="form-group"><label>
                                <asp:Label ID="lbl_van" runat="server" Text="Van" Visible="false"></asp:Label></label>
                               <asp:Panel ID="Panel1" runat="server" Height="143px" ScrollBars="Auto" BorderStyle="Groove"
                                        BorderWidth="1px" Visible="False" Width="514px">
                                       
                                    <telerik:RadListBox ID="chkSalesRep"  CssClass="multiColumn"  Visible="true"   TabIndex="4"  runat="server"  
                                       
                                      CheckBoxes="true"  Skin ="Default" BackColor ="White"  BorderStyle ="None" BorderColor ="LightGray" BorderWidth ="1px"
                                           DataTextField="UserName"  ForeColor ="Black"
                            DataValueField="User_ID" 
                                      
                                            style="top: 0px; left: 0px" Width="100%" >
                                             
                                             
                                            </telerik:RadListBox>
                                    </asp:Panel></div>
          
              
                
                </div>
                </div>
             <div class="row">
                          <div class="col-sm-12">
                 <div class="form-group">
                                    <asp:Panel ID="pnlDefault" runat="server">
                                        <asp:Button ID="BtnAdd" runat="server" CssClass ="btn btn-success" Text="Add" />
                                        <asp:Button ID="btnModify" runat="server"  CssClass ="btn btn-primary"  Text="Modify" />
                                        <asp:Button ID="btnDelete" runat="server"  CssClass ="btn btn-danger" Text="Delete"  OnClientClick="return ConfirmDelete('Would you like to delete ?',event);"/>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlAdd" runat="server" Visible="false">
                                        <asp:Button ID="btnSave" runat="server" CssClass ="btn btn-success" Text="Save" />
                                        <asp:Button ID="Button2" runat="server" CssClass ="btn btn-default"  Text="Cancel" />
                                    </asp:Panel>
                                    <asp:Panel ID="pnlModify" runat="server" Visible="false">
                                        <asp:Button ID="btnUpdate" runat="server" CssClass ="btn btn-success" Text="Update" />
                                        <asp:Button ID="Button4" runat="server"  CssClass ="btn btn-default"  Text="Cancel" />
                                    </asp:Panel>
                                </div>
                              </div>
                </div>

            <div class="table-responsive">
                         <telerik:RadGrid ID="rgUsers" DataSourceID="SqlDataSource9" Skin="Simple"
                                                                    AllowSorting="true" autogeneratedcolumns="false" AutoGenerateColumns="False"
                                                                    PageSize="10" AllowPaging="true" runat="server" AllowFilteringByColumn="true"
                                                                    GridLines="None">
                                                                    <GroupingSettings CaseSensitive="false" />    <ClientSettings> </ClientSettings>
                                                                    <MasterTableView Summary="RadGrid table" AllowFilteringByColumn="true" PageSize="8" TableLayout ="fixed" >

                                                                        <NoRecordsTemplate>
                                                                            <div>
                                                                                There are no records to display
                                                                            </div>
                                                                        </NoRecordsTemplate>

                                                                        <Columns>

                                                                              <telerik:GridBoundColumn UniqueName="UserID"  AllowFiltering ="false" ShowFilterIcon ="false" 
                                                                                HeaderText="User ID" DataField="User_ID">
                                                                                <ItemStyle Wrap ="false" /><HeaderStyle Wrap ="false" Width ="40px" />
                                                                            </telerik:GridBoundColumn>
                                                                             <telerik:GridBoundColumn UniqueName="Username"  AllowFiltering ="true" ShowFilterIcon ="false" 
                                                                                 AutoPostBackOnFilter="true" CurrentFilterFunction ="Contains"
                                                                                HeaderText="User Name" DataField="Username">
                                                                                <ItemStyle Wrap ="false" /><HeaderStyle Wrap ="false" Width ="80px" />
                                                                            </telerik:GridBoundColumn>
                                                                            <telerik:GridBoundColumn UniqueName="UserType"
                                                                                AllowFiltering ="true" ShowFilterIcon ="false" 
                                                                                 AutoPostBackOnFilter="true" CurrentFilterFunction ="Contains"
                                                                                HeaderText="User Type" DataField="User_Type">
                                                                                <ItemStyle Wrap ="false" /><HeaderStyle Wrap ="false" Width ="90px" />
                                                                            </telerik:GridBoundColumn>
                                                                            <telerik:GridBoundColumn UniqueName="Designation"
                                                                                AllowFiltering ="true" ShowFilterIcon ="false" 
                                                                                 AutoPostBackOnFilter="true" CurrentFilterFunction ="Contains"
                                                                                HeaderText="Designation" DataField="Designation">
                                                                                <ItemStyle Wrap ="false" /><HeaderStyle Wrap ="false" Width ="70px" />
                                                                            </telerik:GridBoundColumn>
                                                                             
                                                                          
                                                                          
                                                                        </Columns>



















                                                                    </MasterTableView>
                                                                      <PagerStyle Mode="NextPrevAndNumeric"  AlwaysVisible="true"></PagerStyle>
                                                                    <HeaderStyle HorizontalAlign="Left" Wrap="true" />
                                                                </telerik:RadGrid>
                </div>
                                                                <asp:SqlDataSource ID="SqlDataSource9" runat="server" ConnectionString="<%$ ConnectionStrings:SWXConnectionString %>"
                                                                    SelectCommand="Rep_UsersList" SelectCommandType="StoredProcedure">

                                                                  

                                                                </asp:SqlDataSource>

            <asp:Button ID="btnHidden" runat="Server" Style="display: none" />
              <ajaxToolkit:ModalPopupExtender ID="MpInfoError" runat="server" BackgroundCssClass="modalBackground"
                                                        TargetControlID="btnHidden" PopupControlID="pnlConfirm" CancelControlID="btnClose"
                                                        Drag="true" />
                                                    <asp:Panel ID="pnlConfirm" Width="400" runat="server" CssClass="modalPopup"   style="display:none">
                                                    
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
        </ContentTemplate>
    </asp:UpdatePanel>
  

      <asp:UpdateProgress ID="UpdateProgress1" 
                    runat="server">
                    <ProgressTemplate>
                        <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                            <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                            <span style="font-size: 12px; color:  #337AB7  ;">Processing... </span>
                        </asp:Panel>
                    </ProgressTemplate>
                </asp:UpdateProgress>
</asp:Content>
