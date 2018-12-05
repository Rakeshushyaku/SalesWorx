<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="VanToOrgAndSalesMan.aspx.vb" Inherits="SalesWorx_BO.VanToOrgAndSalesMan" %>
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

            var Filter = /ddFilterBy/
            var AddString = postBackElement.id.search(Filter);
            
            if (AddString == -1) {
                var myRegExp = /_btnUpdate/;

                var AddString = postBackElement.id.search(myRegExp);
                var EditString = postBackElement.id.search(myRegExp);
                
                if (AddString != -1 || EditString != -1) {
                    
                }
                else {
                    $get('<%= Me.UpdateProgress1.ClientID %>').style.display = 'block';
                }
               
                postBackElement.disabled = true;
            }
        }


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

        function EndRequest(sender, args) {
            var Filter = /ddFilterBy/
            var AddString = postBackElement.id.search(Filter);
            if (AddString == -1) {
                var myRegExp = /_btnUpdate/;

                var AddString = postBackElement.id.search(myRegExp);
                var EditString = postBackElement.id.search(myRegExp);
                var myRegExp2 = /btnCancel/
                var cancelString = postBackElement.id.search(myRegExp2);
                
                if (AddString != -1 || EditString != -1) {
                  
                }
                else {
                    $get('<%= Me.UpdateProgress1.ClientID %>').style.display = 'none';
                }
                postBackElement.disabled = false;

                if (cancelString != -1) {
                    HideRadWindow();
                }
            }
        }

       

    </script><script type="text/javascript">
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
       

       
    </script>
    <script type="text/javascript">
        $(window).resize(function () {
            var win = $find('<%= MPEDetails.ClientID %>');
            if (win) {
                if (!win.isClosed()) {
                    win.center();
                }
            }

        });
    </script>
 </asp:Content>
     <asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

          <telerik:RadAjaxLoadingPanel  ID="AjaxLoadingPanel1" runat="server" >
             <div ID="ProgressPanel" class ="overlay"  >
     
         <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index:10010;vertical-align:middle;" />      
       </div>
           </telerik:RadAjaxLoadingPanel>  

        <asp:Panel ID="Panel1" runat="server">      </asp:Panel> 
                         
 
    
                                         
                     <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>

    <h4>Organization Setup</h4>
               
                            <asp:UpdatePanel ID="TopPanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <label>Organization</label>
                                    <div class="row">
                                        <div class="col-sm-4">
                                            <div class="form-group"> 
                                                <telerik:RadComboBox Skin="Simple"  ID="ddFilterBy" runat="server" AutoPostBack="true"  
                                                    TabIndex="1"  Width="100%">
                                                </telerik:RadComboBox>
                                            </div>
                                        </div>
                                    
                                    <div class="col-sm-4">
                                     <div class="form-group">  
                                                
                                           
                                                <asp:Button ID="btnFilter" runat="server" CausesValidation="False"  CssClass ="btn btn-primary"
                                                    TabIndex="2" Text="Search" />
                                           </div>
                                    </div>
                                    </div>
                                    
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        <div class="table-responsive">
                            <asp:UpdatePanel ID="ClassUpdatePnl" runat="server" UpdateMode="conditional">
                                <ContentTemplate>
                                  
                                                <asp:GridView Width="100%" ID="gvDivConfig" runat="server" EmptyDataText="No Organization details found."
                                                    EmptyDataRowStyle-Font-Bold="true"  AutoGenerateColumns="False"
                                                    AllowPaging="True"   PageSize="25" CellPadding="0" CellSpacing="0" CssClass="tablecellalign">
                                                    <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
                                                    <EmptyDataRowStyle Font-Bold="True" />
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:HiddenField ID="HStock_Org_ID" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"Stock_Org_ID") %>'/>
                                                                <asp:HiddenField ID="HOrg_HE_ID" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"Org_HE_ID") %>'/>
                                                                <asp:HiddenField ID="HEmp_Code" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"Emp_Code") %>'/>
                                                                <asp:HiddenField ID="hfPrefix" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"DocPrefix") %>'/>
                                                                <asp:ImageButton ID="btnEdit" ToolTip="Edit Configuration" runat="server" CausesValidation="false"
                                                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Org_ID") %>'
                                                                      ImageUrl="~/images/edit-13.png"   OnClick="btnEdit_Click" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" CssClass="display-block"  />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="SalesRep_Name" HeaderText="Van/FSR Org" SortExpression="SalesRep_Name">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                         <asp:BoundField DataField="orgHead" HeaderText="Sales Organization" SortExpression="orgHead">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="SourcOrg" HeaderText="Source Warehouse" SortExpression="SourcOrg">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="SalesMan" HeaderText="Sales Man"
                                                            SortExpression="SalesMan">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                         <asp:BoundField DataField="DocPrefix" HeaderText="Prefix No"
                                                            SortExpression="DocPrefix">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                       <%-- <asp:BoundField DataField="Print_Format" HeaderText="Print Format"
                                                            SortExpression="Print_Format">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>--%>
                                                    </Columns>
                                                      <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
                                                </asp:GridView>
                                            
                                       <telerik:RadWindow ID="MPEDetails" Title= "Organization Setup" runat="server" Skin="Windows7" Behaviors="Move,Close"
                                                AutoSize="true" ReloadOnShow="false" VisibleStatusbar="false" Overlay="true" Modal="true">
                                              <ContentTemplate>
                                                   <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="conditional">
                                <ContentTemplate>
                                    
                                        <asp:HiddenField ID="HidVal" runat="server" Value="-1" />
                                    <div class="popupcontentblk">
                                        <p><asp:Label ID="lblmsgPopUp" runat="server" Text="" ForeColor="Red"></asp:Label></p>
                                        <div class="row">
                                            <div class="col-sm-5">
                                                <label>Sales Organization</label>
                                            </div>
                                            <div class="col-sm-7">
                                                <div class="form-group">
                                                    <telerik:RadComboBox Skin="Simple"  ID="drpLocalOrg" TabIndex ="1"  runat="server" Width="100%" CssClass="inputSM"  Enabled="false"></telerik:RadComboBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-5">
                                                <label>Van/FSR Organization</label>
                                            </div>
                                            <div class="col-sm-7">
                                                <div class="form-group">
                                                    <telerik:RadComboBox Skin="Simple"  ID="drpOrganization" TabIndex ="2" Width="100%"  runat="server" CssClass="inputSM" Enabled="false"></telerik:RadComboBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-5">
                                                <label>Source Warehouse</label>
                                            </div>
                                            <div class="col-sm-7">
                                                <div class="form-group">
                                                    <telerik:RadComboBox Skin="Simple"  ID="drpSourcWH" TabIndex ="3"  Width="100%"  runat="server" CssClass="inputSM" AutoPostBack="false"></telerik:RadComboBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-5">
                                                <label>Sales Man</label>
                                            </div>
                                            <div class="col-sm-7">
                                                <div class="form-group">
                                                    <telerik:RadComboBox Skin="Simple"  ID="DrpSalesMan" TabIndex ="4"  Width="100%" runat="server" CssClass="inputSM" AutoPostBack="false"></telerik:RadComboBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please select the sales man" ControlToValidate="DrpSalesMan" InitialValue="-1"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                         <div class="row" id="divDocPrefix" runat ="server"  >
                                            <div class="col-sm-5">
                                                <label>Document Prefix</label>
                                            </div>
                                            <div class="col-sm-7">
                                                <div class="form-group">
                                                    <telerik:RadTextBox Skin="Simple"  ID="txtPrefix" TabIndex ="4"  Width="100%"  runat="server" CssClass="inputSM"></telerik:RadTextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-5">
                                                <label></label>
                                            </div>
                                            <div class="col-sm-7">
                                                <div class="form-group">
                                                    <asp:Button ID="btnUpdate" TabIndex ="5" CssClass ="btn btn-success"  Text="Update" OnClick="btnUpdate_Click" runat="server" />
                                                    <asp:Button ID="btnCancel" CssClass ="btn btn-default"  TabIndex="6" runat="server" OnClick="btnCancel_Click" Text="Cancel" />
                                                </div>
                                            </div>
                                        </div>

                                        <asp:Panel ID="Panel12" runat="server" Style="display: none" CssClass="overlay">
                                                        <img alt="Processing..." src="../images/Progress.gif" />
                                                        <span>Processing... </span>
                                                    </asp:Panel>
                                        </div>
                                    </ContentTemplate>
                            </asp:UpdatePanel>
                                    </ContentTemplate>
                                   </telerik:RadWindow>
                                </ContentTemplate>
                               <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnFilter" EventName="Click" />
                                </Triggers>
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
