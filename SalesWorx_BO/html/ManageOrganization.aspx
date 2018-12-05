<%@ Page Title="Sales Org Configuration" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master"
    CodeBehind="ManageOrganization.aspx.vb" Inherits="SalesWorx_BO.ManageOrganization" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="FeaturedContent" runat="server">
   <script language="javascript" type="text/javascript">


     

      
   

        



  

       

        function alertCallBackFn(arg) {
            HideRadWindow()
        }
        function HideRadWindow() {

            var elem = $('a[class=rwCloseButton');

            if (elem != null && elem != undefined) {
                $('a[class=rwCloseButton')[0].click();
            }

            $("#frm").find("iframe").hide();
        }

        function CloseWindow(sender, args) {
            var window;
            window = $find('<%= MPEDetails.ClientID%>');


                window.close();
            }
    </script>
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    
    <h4>Manage Organization</h4>
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
                                    <label>Filter By</label>
                                    <div class="row">
                                             <div class="col-sm-4">
                                    <div class="form-group">  
                                              <telerik:RadComboBox ID="ddFilterBy" Skin="Simple"  
                    Width="100%" Height="250px" TabIndex="2"
                    runat="server">
                    <Items>

                        <telerik:RadComboBoxItem Selected="True" Text="All"></telerik:RadComboBoxItem>
                        <telerik:RadComboBoxItem Value="Organization Code" Text="Organization Code"></telerik:RadComboBoxItem>
                        <telerik:RadComboBoxItem Value="Van Code" Text="Van Code"></telerik:RadComboBoxItem>
                       
                    </Items>
                </telerik:RadComboBox>
                </div>
                                            </div>
                                            <div class="col-sm-4">
                                                <div class="form-group"> 
                                                 <telerik:RadTextBox runat="server" ID="txtFilterVal" EmptyMessage="Enter Currency" Skin="Simple" Width="100%"></telerik:RadTextBox>
                                                </div>
                                             </div>
                                            <div class="col-sm-4">
                                                 <div class="form-group">   
                                              
                                                      <telerik:RadButton ID="btnFilter" Skin ="Simple"    runat="server" Text="Search" CssClass ="btn btn-primary" />
                                            
                                                      <telerik:RadButton ID="btnReset" Skin="Simple" OnClick ="btnReset_Click" runat="server" Text="Reset" CssClass="btn btn-default" />
                                                          <telerik:RadButton ID="btnAdd" Skin="Simple" OnClick ="btnAdd_Click" runat="server" Text="Add" CssClass="btn btn-success" />
                                            <asp:Button ID="btnImport" runat="server" Visible ="false"  CausesValidation="false" CssClass="btnInputGreen"
                                                    TabIndex="2" Text="Import"  />
                                                     </div>
                                                </div>
                                        </div> 
                                </ContentTemplate>
                            </asp:UpdatePanel>
                   <div class="table-responsive">
                            <asp:UpdatePanel ID="ClassUpdatePnl" runat="server" UpdateMode="conditional">
                                <ContentTemplate>
                                    
                                                  <asp:GridView Width="100%" ID="gvOrgCtl" runat="server" EmptyDataText="No Configuration details found."
                                                    EmptyDataRowStyle-Font-Bold="true"  AutoGenerateColumns="False"
                                                    AllowPaging="True" AllowSorting="true"   PageSize="10" CellPadding="0" CellSpacing="0" CssClass="tablecellalign">
                                                    <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
                                                    <EmptyDataRowStyle Font-Bold="True" />
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <HeaderStyle width="75" />
                                                              
                                                                
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" Width="100px" CssClass="display-block"  />
                                                            <ItemTemplate>
                                                              
                                                                
                                                                <asp:ImageButton ToolTip="Delete Configuration " ID="btnDelete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ORG_HE_ID")%>'
                                                                    OnClick="btnDelete_Click" runat="server" CausesValidation="false" 
                                                                     ImageUrl="~/images/delete-13.png" 
                                                                    OnClientClick="javascript:return confirm('Would you like to delete the selected Organization?');"/>

                                                               <%-- <asp:ImageButton ID="btnEdit" ToolTip="Edit Configuration" runat="server" CausesValidation="false"
                                                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ORG_HE_ID")%>'
                                                                      ImageUrl="~/images/edit-13.png"   OnClick="btnEdit_Click" />--%>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" CssClass="display-block"  />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Description" HeaderText="Organization Name" SortExpression="Description">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                      
                                                        <asp:BoundField DataField="Currency_Code" HeaderText="Currency " SortExpression="Description">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                                                                         
                                                        
                                                        
                                                    </Columns>
                                                    <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
                                                </asp:GridView> 

                                             <%--<script type="text/javascript">
                                                 $(window).resize(function () {
                                                     var win = $find('<%= MPEDetails.ClientID %>');
            if (win) {
                if (!win.isClosed()) {
                    win.center();
                }
            }

        });
    </script>--%>
                                    <telerik:RadWindow ID="MPEDetails" Title= "Organization Details" runat="server" Skin="Windows7" Behaviors="Move,Close"
                                                AutoSize="true" ReloadOnShow="false" VisibleStatusbar="false" Overlay="true" Modal="true">
                                              <ContentTemplate>
                                                   
                                                   <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="conditional">
                                                  <ContentTemplate>
                                        <asp:HiddenField ID="HidVal" runat="server" Value="-1" />
                                        
                                                  <div class="popupcontentblk">
                                                      <p><asp:Label ID="lblPop" runat="server" Text="" ForeColor="Red"></asp:Label></p>
                                                    
                                                   
                                                    
                                                  
                                                    <div class="row">
                                                        <div class="col-sm-5">
                                                            <label>Organization Name</label>
                                                        </div>
                                                        <div class="col-sm-7">
                                                            <div class="form-group">
                                                            <asp:TextBox ID="txtDescription" TabIndex ="5" Width="100%" CssClass="inputSM" MaxLength="50" runat="server"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-sm-5">
                                                            <label>Currency</label>
                                                        </div>
                                                        <div class="col-sm-7">
                                                            <div class="form-group">
                                                             <telerik:RadComboBox ID="ddl_Currency" Width ="100%" runat="server" TabIndex="4" Skin="Simple" Filter="Contains">
                                                      
                                                    </telerik:RadComboBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    
                                                   
                                                  
                                                    <div class="row">
                                                        <div class="col-sm-5">
                                                            <label></label>
                                                        </div>
                                                        <div class="col-sm-7">
                                                            <asp:Button ID="btnSave" CssClass ="btn btn-success" TabIndex="7" OnClick="btnSave_Click" runat="server" Text="Save" />
                                                            <asp:Button ID="btnUpdate" CssClass ="btn btn-success" Text="Update" OnClick="btnUpdate_Click" runat="server" />
                                                           <telerik:RadButton ID="btnCancel" Skin="Simple" runat="server" Text="Cancel" TabIndex="18" OnClientClicked="CloseWindow" AutoPostBack="false" CssClass="btn btn-default" />        
                                        
                                        
                                                                                       </div>
                                                        
                                                    </div>
                                                      <asp:Panel ID="Panel12" runat="server" Style="display: none" CssClass="overlay">
                                                            <img alt="Processing..." src="../assets/img/ajax-loader.gif" />
                                                            <span>Processing... </span>
                                                        </asp:Panel>
                                       
                                    </div>


                                </ContentTemplate>

                        </asp:UpdatePanel>
                                 </ContentTemplate>
                                                    </telerik:RadWindow> 
                                    
                                </ContentTemplate>
                                
                            </asp:UpdatePanel>
                       </div>
                 <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="ClassUpdatePnl"
                    runat="server">
                    <ProgressTemplate>
                        <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                            <img src="../assets/img/ajax-loader.gif" alt="Processing..." />
                            <span>Processing... </span>
                        </asp:Panel>
                    </ProgressTemplate>
                </asp:UpdateProgress>
               
</asp:Content>
