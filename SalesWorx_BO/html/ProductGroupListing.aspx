<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="ProductGroupListing.aspx.vb" Inherits="SalesWorx_BO.ProductGroupListing" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
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
                var myRegExp1 = /btnSave/
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
                var myRegExp1 = /btnSave/
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
    </asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <h4>Product Groups</h4>
                       
                            <asp:UpdatePanel ID="TopPanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                     <div class="row">
	                                    <div class="col-sm-12">
		                                    <div class="form-group">
                                            
                                            <label>Filter By</label>
                                            <asp:DropDownList ID="ddFilterBy" runat="server"   CssClass="inputSM" TabIndex="2">
                                                    <asp:ListItem Selected="True">All</asp:ListItem>
                                                    <asp:ListItem Value="Description">Description</asp:ListItem>
                                                    <asp:ListItem Value="Created By">Created By</asp:ListItem>
                                                  
                                                </asp:DropDownList>
                                             
                                                
                                                 <asp:TextBox ID="txtFilterValue" runat="server" ToolTip ="Enter Filter Value" autocomplete="off" CssClass="inputSM"
                                                    TabIndex="3"></asp:TextBox>
                                                <asp:Button ID="btnFilter" runat="server" CausesValidation="False" CssClass="btn btn-default"
                                                    OnClick="btnFilter_Click" TabIndex="4" Text="Filter" />
                                                    
                                                  <asp:Button ID="btnReset" runat="server" CausesValidation="False" CssClass="btn btn-warning"
                                                    TabIndex="5" Text="Reset" />
                                                    
                                                <asp:Button ID="btnAdd" runat="server" CausesValidation="false" CssClass="btn btn-success"
                                                    OnClick="btnAdd_Click" TabIndex="1" Text="Add New Group" />
                                                </div>
                                              </div>
                                         </div>
                                            
                                            
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        
                   
                            <asp:UpdatePanel ID="ClassUpdatePnl" runat="server" UpdateMode="conditional">
                                <ContentTemplate>
                                  
                                                   <telerik:RadGrid ID="dgv"
                AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                PageSize="12" AllowPaging="True" runat="server"
                GridLines="None">

                <GroupingSettings CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" DataKeyNames="PGID" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                    PageSize="12">


                    <PagerStyle Mode="NextPrevAndNumeric"  AlwaysVisible="true"></PagerStyle>

                    <Columns>
                       


                        <telerik:GridBoundColumn UniqueName="PGID"
                            SortExpression="PGID" HeaderText="Group ID" DataField="PGID">
                            <ItemStyle Wrap="false" />
                        </telerik:GridBoundColumn>


                        <telerik:GridBoundColumn UniqueName="Description"
                            SortExpression="Description" HeaderText="Description" DataField="Description">
                            <ItemStyle Wrap="false" />
                        </telerik:GridBoundColumn>



                        <telerik:GridBoundColumn UniqueName="CreatedBy"
                            SortExpression="CreatedBy" HeaderText="Created By" DataField="CreatedBy">
                            <ItemStyle Wrap="false" />
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn UniqueName="CreatedAt"
                            SortExpression="CreatedAt" HeaderText="Created At" DataField="CreatedAt"
                            DataFormatString="{0:dd-MM-yyyy}"
                         >
                            <ItemStyle Wrap="false" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn UniqueName="TotalItems"
                            SortExpression="TotalItems" HeaderText="Total Items" DataField="TotalItems"
                         >
                            <ItemStyle Wrap="false" /></telerik:GridBoundColumn>



                         <telerik:GridTemplateColumn UniqueName="EditColumn" AllowFiltering="false"
                            InitializeTemplatesFirst="false">


                            <ItemTemplate>

                              

                                 <asp:ImageButton ID="View" runat ="server" CommandName="View"   ImageUrl="~/images/edit-13.png"   ToolTip="Edit Group"  />
                              <asp:Label ID="lblOrgId" runat="server" Visible ="false"  Text='<%# Bind("OrgId") %>'></asp:Label>
                                 <asp:Label ID="lblPGID" runat="server" Visible ="false"  Text='<%# Bind("PGId") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="30px" />
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </telerik:GridTemplateColumn>

                          <telerik:GridTemplateColumn UniqueName="DeleteColumn" AllowFiltering="false"
                            InitializeTemplatesFirst="false">


                            <ItemTemplate>

                                <asp:ImageButton ID="btnDelete" ToolTip="Delet Group" runat="server" CausesValidation="false"  
                                               CommandName="DeleteGroup"  
                               ImageUrl="~/Images/delete-13.png"
                              OnClientClick="return ConfirmDelete('Are you sure to delete this item?',event);"
                             
                            />      
                            </ItemTemplate>
                            <HeaderStyle Width="30px" />
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </telerik:GridTemplateColumn>

                    </Columns>

















                </MasterTableView>


            </telerik:RadGrid>
                                      
                                     
                                </ContentTemplate>
                            </asp:UpdatePanel>
                         
                <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="ClassUpdatePnl"
                    runat="server">
                    <ProgressTemplate>
                        <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                            <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                            <span style="font-size: 12px; color: #3399ff;">Processing... </span>
                        </asp:Panel>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                
           
</asp:Content>

