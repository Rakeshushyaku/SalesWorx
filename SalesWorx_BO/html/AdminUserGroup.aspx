<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="AdminUserGroup.aspx.vb" Inherits="SalesWorx_BO.AdminUserGroup" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">

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
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <h4>Manage User Groups</h4>
    
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

    <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>

    <asp:Panel ID="Panel2" runat="server"></asp:Panel> 

 
    <asp:UpdatePanel ID="UpdatePanel1" runat="server"  >
    
        <ContentTemplate>
            

              
             <div class="row">

                 <div class="col-sm-4">
                     <div class="form-group">
                         <label>
                             <asp:Label runat="server" ID="Label1" Text="Organization" Visible="true"></asp:Label></label>
                              
                  <telerik:RadComboBox ID="ddlOrganization"  Skin="Simple"  Width="100%"    
                                            Filter="Contains"    runat="server" Visible="true"  AutoPostBack ="true" DataTextField="Description" DataValueField="MAS_Org_ID">
                                               </telerik:RadComboBox>
                     </div>
                     </div>

                  <div class="col-sm-4">
               <div class="form-group">
                   <label>
                                     <asp:Label ID="lbl_Van1" runat="server" Text="Van/FSR"></asp:Label> </label>
                               
                       <telerik:RadComboBox Filter="Contains" Skin="Simple" EnableCheckAllItemsCheckBox="true"
                                                    CheckBoxes="true" EmptyMessage="Select a user" ID="ddlVan" Width="100%" runat="server">
                                                </telerik:RadComboBox>
                                    
                              </div>
                    </div>
                 </div>
            <div class="row">
            <div class="col-sm-4">
            <div class="form-group">
                   
<label>User Group</label>
                                    <telerik:RadTextBox ID="txtUserGroups"  Skin="Simple"  runat="server"   Width ="100%"   TabIndex ="1"  MaxLength="50">
                                    </telerik:RadTextBox>


                
                 </div>
                </div>
                 <div class="col-sm-4">
            <div class="form-group">
                   
<label>&nbsp;</label>
                          <asp:Button ID="btnSave" runat="server" CssClass ="btn btn-success" Text="Save" />
                          <asp:Button ID="btnUpdate" runat="server" CssClass ="btn btn-success" Text="Save" Visible="false"  />
                                        <asp:Button ID="btnClear" runat="server" CssClass ="btn btn-default"  Text="Clear" />
                 </div>
                     </div>
                </div>
            <br />
            <div class="row">
            <div class="col-sm-4">
            <div class="form-group">
                <asp:HiddenField ID="HID" runat="server"></asp:HiddenField>   
            <label>Group Name</label>
                 <asp:TextBox ID="txt_filter" runat="server" MaxLength="50"></asp:TextBox> <asp:Button ID="btn_search" runat="server" Text="Search" CssClass ="btn btn-success"></asp:Button>
                  <asp:Button ID="btn_reset" runat="server" CssClass ="btn btn-default"  Text="Clear Filter" />
                </div></div>
                
            <div class="table-responsive">
                         <asp:GridView  width="100%" ID="grduserGrp" runat="server" 
                  EmptyDataText="No listing found" EmptyDataRowStyle-Font-Bold="true" 
                  AutoGenerateColumns="False" AllowPaging="True"
                   AllowSorting="false" 
                  PageSize="20" CellPadding="0" CellSpacing="0" CssClass="tablecellalign">
                   
                  <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
                    <EmptyDataRowStyle Font-Bold="True" />
                  <Columns>
                   <asp:TemplateField>
                                                         
                                                            <HeaderStyle HorizontalAlign="Left" Width="100px" CssClass="display-block"  />
                       <HeaderTemplate>
                                                                
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                               
                                                                <asp:ImageButton ToolTip="Delete Currency Code" ID="ImageButton1" OnClick="btnDelete_Click"
                                                                    runat="server" CausesValidation="false" ImageUrl="~/images/delete-13.png" OnClientClick="return ConfirmDelete('Would you like to delete the selected group?',event);" CssClass="checkboximgvalign" />
                                                                
                                                                 <asp:ImageButton ID="btnEdit" ToolTip="Edit Mapping" runat="server" CausesValidation="false"
                                                                      ImageUrl="~/images/edit-13.png"   OnClick="btnEdit_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"UG_ID") %>' />
                                                            </ItemTemplate>
                       <ItemStyle HorizontalAlign="Left" Width="100px" CssClass="display-block"  />
                                                        </asp:TemplateField>
                   <asp:BoundField DataField="Group_Name" HeaderText="Group Name"  SortExpression="Group_Name" NullDisplayText="N/A" >
                      <ItemStyle Wrap="False" />
                   </asp:BoundField>
                    
                      <asp:TemplateField>
                          <ItemTemplate>
                              
                              <asp:HiddenField ID="hUG_ID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"UG_ID") %>' />
                               
                          </ItemTemplate>
                      </asp:TemplateField>
                  </Columns>
                 <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
              </asp:GridView>
        
                </div>
                                                                 
            </div>
            
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
