<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ReviewRoutePlan.aspx.vb" Inherits="SalesWorx_BO.ReviewRoutePlan"  MasterPageFile="~/html/Site.Master"%>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        #MainContent_TopPanel .RadInput_Simple a {
            background:none !important;
        }
    </style>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="FeaturedContent" runat="server">
    <script>
        function alertCallBackFn(arg) {

        }

        var prm = Sys.WebForms.PageRequestManager.getInstance();

        prm.add_initializeRequest(InitializeRequest);
        prm.add_endRequest(EndRequest);
        var postBackElement;
        function InitializeRequest(sender, args) {

            if (prm.get_isInAsyncPostBack())
                args.set_cancel(true);

            $get('<%=UpdateProgress1.ClientID %>').style.display = 'block';

        }

        function EndRequest(sender, args) {
            $get('<%= UpdateProgress1.ClientID %>').style.display = 'none';
        }

        function clickExportBiffExcel(id) {
            $get('<%= H_FSRPLANID.ClientID%>').value=id
            $("#MainContent_BtnExportBiffExcel").click()
            return false

        }

        function OpenLocWindow(FSRPlanID) {


            var URL
            URL = 'ReviewRoutePlan_View.aspx?FSRPlanID=' + FSRPlanID;
            var oWnd = radopen(URL, null);
            oWnd.SetSize(1275, 750);
            oWnd.set_behaviors(Telerik.Web.UI.WindowBehaviors.Close); //Close:4
            oWnd.SetModal(true);
            oWnd.Center;
            return false
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h4>Review Route Plan</h4>



    <telerik:RadAjaxLoadingPanel ID="AjaxLoadingPanel1" runat="server">
        <div id="ProgressPanel" class="overlay">

            <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
        </div>
    </telerik:RadAjaxLoadingPanel>

    <telerik:RadWindowManager ID="RadWindowManager1" Skin="Simple" runat="server" EnableShadow="true">
    </telerik:RadWindowManager>

    <div class="form-group">
        <asp:Label runat="server" ID="ConfirmationMsg"></asp:Label>
    </div>


    <asp:UpdatePanel ID="TopPanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="row">
                <div class="col-sm-6 col-md-3">
                    <div class="form-group">
                <label>Filter By </label>
                <telerik:RadComboBox ID="ddFilterBy" Skin="Simple"
                    Width="100%" Height="250px" TabIndex="1"
                    runat="server">
                    <Items>

                        <telerik:RadComboBoxItem Selected="True" Text="All"></telerik:RadComboBoxItem>
                        <telerik:RadComboBoxItem Value="Description" Text="Description"></telerik:RadComboBoxItem>
                        <telerik:RadComboBoxItem Value="Sales Rep" Text="Sales Rep"></telerik:RadComboBoxItem>
                        <telerik:RadComboBoxItem Value="Status" Text="Status"></telerik:RadComboBoxItem>
                    </Items>
                </telerik:RadComboBox>
                </div>
                </div>
                <div class="col-sm-6 col-md-3">
                    <div class="form-group">
                <label class="hidden-xs">&nbsp;</label>
                <telerik:RadTextBox TabIndex="2" runat="server" ID="txtFilterValue" EmptyMessage="Enter Filter Value" Skin="Simple" Width="100%"></telerik:RadTextBox>
                </div>
                    </div>
                <div class="col-sm-6 col-md-3">
                    <div class="form-group">
                <label>Select Month</label>
                                                            <telerik:RadMonthYearPicker RenderMode="Lightweight" Skin="Simple" Width="100%" ID="MonthPicker" runat="server">
                                                    <DateInput ReadOnly="true" DisplayDateFormat="MMM-yyyy">
                                                    </DateInput>

                                                </telerik:RadMonthYearPicker>
                </div>
                    </div>
                <div class="col-sm-6 col-md-3">
                    <div class="form-group">
                        <label class="hidden-xs">&nbsp;</label>
                <telerik:RadButton TabIndex="3" CausesValidation="false" ID="btnFilter" Skin="Simple" runat="server" Text="Filter" CssClass="btn btn-primary" />
                         <telerik:RadButton TabIndex="3" CausesValidation="false" ID="btnReset" Skin="Simple" runat="server" Text="Reset" CssClass="btn btn-default" />
                        </div>
                    </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <asp:HiddenField id="hfPlanID" runat="server"></asp:HiddenField>
              <telerik:RadWindow ID="MapWindow" Title="Visit Status" runat="server" Behaviors="Move,Close"
            Width="990px" Height="570px" Skin="Windows7" ReloadOnShow="false" VisibleStatusbar="false" Modal="true" Overlay="true">
            <ContentTemplate>
            
             <telerik:RadAjaxPanel ID="raf" runat="server">
                          <telerik:RadGrid ID="rgVisits" DataSourceID="sqlVisits" 
                            AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                             PageSize="10"  AllowPaging="false" runat="server" AllowFilteringByColumn="false"
                            GridLines="None">

                   
                           <ClientSettings EnableRowHoverStyle="true">
           
</ClientSettings>
                            <MasterTableView AutoGenerateColumns="false" TableLayout="Fixed" Width="100%" GridLines="None" BorderColor="LightGray"
                                DataSourceID="sqlVisits"
                                 PageSize="10" >
                                
                                        <GroupByExpressions>
                                                <telerik:GridGroupByExpression>
                                                    <SelectFields>
                                                        <telerik:GridGroupByField  FieldAlias="Date"  HeaderValueSeparator=" : "    FieldName="Visit_Date"></telerik:GridGroupByField>
                                                    </SelectFields>
                                                    <GroupByFields>
                                                        <telerik:GridGroupByField FieldName="Visit_Date"      SortOrder="Descending"></telerik:GridGroupByField>
                                                        
                                                    </GroupByFields>
                                                    
                                                </telerik:GridGroupByExpression>
                                            
                                            </GroupByExpressions>

                                <PagerStyle Mode="NextPrevAndNumeric"  AlwaysVisible="true"></PagerStyle>

                                <Columns>
                                     <telerik:GridBoundColumn UniqueName="Customer_No" 
                                        
                                        SortExpression="Customer_No" HeaderText="Customer No" DataField="Customer_No"
                                        ShowFilterIcon="false">
                                        <HeaderStyle Width="110px" Wrap="false" />
                                        <ItemStyle Wrap="false" />
                                    </telerik:GridBoundColumn>
                                       <telerik:GridBoundColumn UniqueName="CustomerName" 
                                        SortExpression="Customer_Name" HeaderText="Customer Name" DataField="Customer_Name"
                                        ShowFilterIcon="false" 
                                          >
                                        <HeaderStyle Width="270px" Wrap="false" />
                                        <ItemStyle Wrap="false" />
                                    </telerik:GridBoundColumn>

                                  <telerik:GridTemplateColumn UniqueName="StatusColumn" HeaderText="Visit Status" AllowFiltering="false" SortExpression ="VisitStatus"
                        InitializeTemplatesFirst="false">


                        <ItemTemplate>
                            <asp:Image ID="imgGreen" runat ="server" ToolTip ="Visited"  ImageUrl ="~/assets/img/Green.png" Visible='<%# Bind("GreenVisible")%>' />

                            <asp:Image ID="imgred" runat ="server" ToolTip ="Not Visited"  ImageUrl ="~/assets/img/Red.png" Visible='<%# Bind("RedVisible")%>' />

                        </ItemTemplate>
                        <HeaderStyle Width="100px" />
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                      <HeaderStyle HorizontalAlign ="Center" />
                    </telerik:GridTemplateColumn>


                                     <telerik:GridBoundColumn UniqueName="VisitStatus"  Visible ="false" 
                                        SortExpression="VisitStatus" HeaderText="Visit Status" DataField="VisitStatus"
                                        ShowFilterIcon="false">
                                        <HeaderStyle Width="110px" Wrap="false" />
                                        <ItemStyle Wrap="false" />
                                    </telerik:GridBoundColumn>
                                   
                                   
                                
                                </Columns>

















                            </MasterTableView>


                        </telerik:RadGrid>
                        <asp:SqlDataSource ID="sqlVisits" runat="server" ConnectionString="<%$ ConnectionStrings:SWXConnectionString %>"
                            SelectCommand="app_GetFSRVisitStatus" SelectCommandType="StoredProcedure">

                            <SelectParameters>


                                <asp:ControlParameter Name="FSRPlanID" ControlID="hfPlanID" Type="Int64" DefaultValue ="0"  />
                               
                            </SelectParameters>
                            </asp:SqlDataSource> 
                 </telerik:RadAjaxPanel> 
            </ContentTemplate>
        </telerik:RadWindow>

            <asp:GridView Width="100%" ID="ApprovalPlans" DataKeyNames="FSR_Plan_ID" runat="server"
                AutoGenerateColumns="False" RowStyle-Wrap="false"
                PageSize="12" CellPadding="0" CellSpacing="0" CssClass="tablecellalign" AllowSorting="true" AllowPaging="true">

                <Columns>
                    <asp:BoundField DataField="FSR_Plan_ID" ShowHeader="False" Visible="False">
                        <ItemStyle Width="100px"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Van/FSR" HeaderStyle-HorizontalAlign="Left" SortExpression="SalesRep_Name">
                        <ItemTemplate>
                            <%--<asp:Label ID="Label1" runat="server" Text='<%# Bind("SalesRep_Name") %>'></asp:Label>--%>
                            <asp:LinkButton CssClass="txtLinkSM" CommandName="Review" ID="SalesLink" runat="server"
                                Text='<%# Bind("SalesRep_Name") %>' ToolTip="Review/ Plan"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Route_Plan" HeaderText="Route Plan" SortExpression="Route_Plan">
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Site" HeaderText="Site" SortExpression="Site">
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="No_Of_Working_Days" HeaderText="Working Days" />
                    <asp:BoundField DataField="No_Of_Visits" HeaderText="No Of Visits" />
                    <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status">
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>

                     <asp:TemplateField HeaderText="View" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                          
                            <asp:LinkButton CssClass="txtLinkSM" CommandName="ViewStatus" ID="LinkButton1" runat="server"
                                Text="View" ToolTip="View Visit Status"></asp:LinkButton>
                                                    </ItemTemplate>
                         
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Export" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                          
                            <asp:LinkButton CssClass="txtLinkSM"  ID="lnk_Export" runat="server" OnClientClick='<%# String.Format("clickExportBiffExcel(""{0}"");", Eval("FSR_Plan_ID"))%>'  
                                Text="Export" ToolTip="Export" ></asp:LinkButton>
                                                    </ItemTemplate>
                         
                    </asp:TemplateField>
                </Columns>
                <PagerStyle CssClass="pagerstyle" />
                <HeaderStyle />
                <RowStyle CssClass="tdstyle" />
                <AlternatingRowStyle CssClass="alttdstyle" />
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="form-group">
        <asp:Label ID="MsgLbl" runat="server" CssClass='txtSM'></asp:Label>
        <asp:HiddenField ID="H_FSRPLANID" runat="server" />
    </div>
    <br />
    <br />
    <div style="display: none">
         
          <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportBiffExcel" runat="server" Text="Export" />
    </div>
    <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="TopPanel"
        runat="server">
        <ProgressTemplate>
            <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
            </asp:Panel>
        </ProgressTemplate>
    </asp:UpdateProgress>

</asp:Content>
