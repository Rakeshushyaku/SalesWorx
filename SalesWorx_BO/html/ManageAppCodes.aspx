<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="ManageAppCodes.aspx.vb" Inherits="SalesWorx_BO.ManageAppCodes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <script type="text/javascript">

        var TargetBaseControl = null;

        window.onload = function () {
            try {
                TargetBaseControl =
           document.getElementById('<%= Me.TopPanel.ClientID %>');
        }
        catch (err) {
            TargetBaseControl = null;
        }
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




        function alertCallBackFn(arg) {

        }


        function CheckAll(cbSelectAll) {
            if (TargetBaseControl == null) return false;
            var TargetChildControl = "chkDelete";
           
            var Inputs = TargetBaseControl.getElementsByTagName("input");

            for (var n = 0; n < Inputs.length; ++n)
                if (Inputs[n].type == 'checkbox' && Inputs[n].id.indexOf(TargetChildControl, 0) >= 0) {
                    Inputs[n].checked = cbSelectAll.checked;
                }

        }
    </script>
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

    </script>
     <style>
    div.RadGrid .rgPager .rgAdvPart     
   {     
    display:none;        
   }      
</style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    


    <h4>Manage App Codes</h4>



    <telerik:RadAjaxLoadingPanel ID="AjaxLoadingPanel1" runat="server">
        <div id="ProgressPanel" class="overlay">

            <img src="../assets/img/ajax-loader.gif" alt="Processing..." />
            <span>Processing... </span> 
        </div>
    </telerik:RadAjaxLoadingPanel>

    <asp:Panel ID="Panel1" runat="server"></asp:Panel>



    <telerik:RadWindowManager ID="RadWindowManager1" Skin="Simple" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>



        <asp:UpdatePanel runat="server" ID="TopPanel" UpdateMode ="Conditional" >
        <ContentTemplate>
        
        <div class="row">
            <div class="col-sm-4">
        <div class="form-group">
            <label>Code Type<em>*</em></label>

            <telerik:RadComboBox ID="ddlCodeType" Skin="Simple" AutoPostBack="true"
                Width="100%" Height="250px" Filter="Contains" EnableLoadOnDemand="true" TabIndex="1"
                runat="server" />

        </div>
                </div>
                <div class="col-sm-4">
        <div class="form-group">
            <label>Code<em>*</em></label>

            <telerik:RadTextBox ID="txtAppCode" runat="server" Skin="Simple" MaxLength="50"
                TabIndex="2" Width="100%">
            </telerik:RadTextBox>

        </div>
                    </div>
                <div class="col-sm-4">
        <div class="form-group">
            <label>Description<em>*</em></label>

            <telerik:RadTextBox ID="txtDescription" runat="server" Skin="Simple" Width="100%" MaxLength="50"
                TabIndex="2">
            </telerik:RadTextBox>

        </div>
                    </div>
                <div class="col-sm-4">
        <div class="form-group">
            <telerik:RadButton ID="btnSave" Skin="Simple" TabIndex="37" runat="server" Text="Save" CssClass="btn btn-success" />
            <telerik:RadButton ID="btnCancel" Skin="Simple" TabIndex="4" runat="server" Text="Cancel" CssClass="btn btn-default" />

        </div>
                    </div>
        </div>  
<div class="table-responsive">
        <telerik:RadGrid ID="rgv" DataSourceID="SqlDataSource9"
            AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
             PageSize="10"  AllowPaging="True" runat="server" AllowFilteringByColumn="true"
            GridLines="None">

            <GroupingSettings CaseSensitive="false"></GroupingSettings>
               <ClientSettings EnableRowHoverStyle="true">
            
</ClientSettings>
            <MasterTableView AutoGenerateColumns="false" DataSourceID="SqlDataSource9" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                AllowFilteringByColumn="true"
                 PageSize="10" >


                <PagerStyle Mode="NextPrevAndNumeric"  AlwaysVisible="true"></PagerStyle>

                <Columns>
                    <telerik:GridTemplateColumn UniqueName="EditColumn" AllowFiltering="false"
                        InitializeTemplatesFirst="false">

                        <HeaderTemplate>
                                                                <asp:CheckBox ID="cbSelectAll"   Visible='<%# Bind("DeleteVisible")%>' onclick="CheckAll(this)" ToolTip="Select/Unselect All"
                                                                    runat="server" />
                                                                <asp:ImageButton ToolTip="Delete Selected Items " ID="btnDeleteAll" runat="server"
                                                                    CausesValidation="false" ImageUrl="~/images/delete-13.png" Visible='<%# Bind("DeleteVisible")%>'
                                                                 OnClientClick="return ConfirmDelete('Would you like to delete the code ?',event);"
                                                                    OnClick="btnDeleteAll_Click" CssClass="checkboximgvalign" />
                                                            </HeaderTemplate>

                        <ItemTemplate>
                            <asp:CheckBox ID="chkDelete" runat="server"  Visible='<%# Bind("DeleteVisible")%>'  CssClass="checkboxvalign" />
                                                                <asp:ImageButton ToolTip="Delete app Code" ID="btnDelete"  CommandName="DeleteCode"
                                                                    Visible='<%# Bind("DeleteVisible")%>'
                                                                    runat="server" CausesValidation="false" ImageUrl="~/images/delete-13.png" OnClientClick="return ConfirmDelete('Would you like to delete the selected code?',event);" CssClass="checkboximgvalign" />

                            <asp:ImageButton ID="btnEdit" ToolTip="Edit Code" runat="server" CausesValidation="false"
                                CommandName="EditCode" ImageUrl="~/Images/edit-13.png" />

                        </ItemTemplate>
                        <HeaderStyle Width="75px" />
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </telerik:GridTemplateColumn>

                     <telerik:GridTemplateColumn UniqueName="EditColumn" AllowFiltering="false"  Visible="false"
                        InitializeTemplatesFirst="false">

                        <HeaderTemplate>
                                                                
                                                            </HeaderTemplate>

                        <ItemTemplate>
                             <asp:HiddenField runat="server" ID="hCodeType" Value='<%# Bind("Code_Type")%>' />
                        </ItemTemplate>
                         
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn UniqueName="EditColumn" AllowFiltering="false"  Visible="false"
                        InitializeTemplatesFirst="false">

                        <HeaderTemplate>
                                                                
                                                            </HeaderTemplate>

                        <ItemTemplate>
                             <asp:HiddenField runat="server" ID="hCodevalue" Value='<%# Bind("Code_Value")%>' />
                        </ItemTemplate>
                         
                    </telerik:GridTemplateColumn>
                     
                     <telerik:GridBoundColumn UniqueName="Code_Value" AllowFiltering="true" CurrentFilterFunction="Contains"
                        AutoPostBackOnFilter="true"
                        SortExpression="Code_Value" HeaderText="Code" DataField="Code_Value"
                        ShowFilterIcon="false">
                        <ItemStyle/>
                    </telerik:GridBoundColumn>
                    
                    <telerik:GridBoundColumn UniqueName="Code_Description" AllowFiltering="true" CurrentFilterFunction="Contains"
                        AutoPostBackOnFilter="true"
                        SortExpression="Code_Description" HeaderText="Description" DataField="Code_Description"
                        ShowFilterIcon="false">
                        <ItemStyle/>
                    </telerik:GridBoundColumn>


                    <telerik:GridBoundColumn UniqueName="LastUpdatedAt" AllowFiltering="false"
                        SortExpression="Last_Updated_At" HeaderText="Last Updated At" DataField="Last_Updated_At" DataFormatString="{0:dd-MM-yyyy}"
                        ShowFilterIcon="false">
                        <ItemStyle />
                    </telerik:GridBoundColumn>


                    <telerik:GridBoundColumn UniqueName="LastUpdatedBy" AllowFiltering="false"
                        SortExpression="LastUpdatedBy" HeaderText="Last Updated By" DataField="LastUpdatedBy"
                        ShowFilterIcon="false">
                        <ItemStyle HorizontalAlign="Center" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </telerik:GridBoundColumn>



                </Columns>

















            </MasterTableView>


        </telerik:RadGrid>
    </div>
        <asp:SqlDataSource ID="SqlDataSource9" runat="server" ConnectionString="<%$ ConnectionStrings:SWXConnectionString %>"
            SelectCommand="app_LoadAppCodesByType" SelectCommandType="StoredProcedure">

            <SelectParameters>


                <asp:ControlParameter Name="CodeType" ControlID="ddlCodeType" DefaultValue="0" Type="String" />
            </SelectParameters>


        </asp:SqlDataSource>
          
    
                </ContentTemplate>
     </asp:UpdatePanel> 
                      
        
           <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="TopPanel"
                    runat="server">
                    <ProgressTemplate>
                        <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                           <img src="../assets/img/ajax-loader.gif" alt="Processing..." /> 
                            <span>Processing... </span>   
                        </asp:Panel>
                    </ProgressTemplate>
                </asp:UpdateProgress>






</asp:Content>

