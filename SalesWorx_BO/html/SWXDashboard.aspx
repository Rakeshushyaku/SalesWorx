<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="SWXDashboard.aspx.vb" Inherits="SalesWorx_BO.SWXDashboard" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Charting" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <script src="../js/kendo.all.min.js"></script>
    <script src="../js/kendo.dataviz.min.js"></script>

    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">


        <link href="../styles/kendo.dataviz.black.min.css" rel="stylesheet" />




        <script type="text/javascript" language="javascript">



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


            function RefreshChart() {
                createChart1();
                createChart2()
                createChart4();
                // createChart6();
                createChart7();
            }

            function OnClientDropDownClosedHandler(sender, eventArgs) {
                $("#MainContent_rpbFilter_i0_dummyOrgBtn").click()
            }

            function OpenDialog(item) {


                //  alert(item.href);

                var $ = $telerik.$;
                var ZeroWindow = $find("<%=ZeroBilledWindow.ClientID%>");
                ZeroWindow.show();
            }

            $(window).resize(function () {
                $("#chart1").data("kendoChart").refresh();
                $("#chart2").data("kendoChart").refresh();
                $("#chart4").data("kendoChart").refresh();
                $("#chart7").data("kendoChart").refresh();
            });
        </script>




    </telerik:RadCodeBlock>

    <style>
        .txtSMBold
        {
            /* font-size: 13px; */
            color: #000000;
            text-decoration: none;
            font-size: 12px !important;
        }

        .RadPanelBar_Default .rpRootGroup
        {
            border-color: lightgrey;
        }
         .RadGrid_Simple .rgCommandRow
        {
            background: whitesmoke;
            color: #000;
            /* height: 15px !important; */
        }

        .RadPanelBar_Default .rpTemplate
        {
            color: #000;
            font-size: 15px !important;
        }

        RadTabStrip .rtsLevel .rtsOut, .RadTabStrip .rtsLevel .rtsIn, .RadTabStrip .rtsLevel .rtsTxt
        {
            text-decoration: none !important;
            font: 15px Segoe UI !important;
            color: #0090d9;
        }

        #contentofpage a:link
        {
            color: #337AB7;
            text-decoration: none !important;
        }

        RadPanelBar .rpLink, .RadPanelBar .rpOut, .RadPanelBar .rpText
        {
            display: block;
            text-decoration: none !important;
            background-color: white;
            font-size: 15px !important;
            color: #0090d9;
        }

        .RadPanelBar_Default a.rpExpanded, .RadPanelBar_Default a.rpSelected, .RadPanelBar_Default div.rpFocused, .RadPanelBar_Default div.rpExpanded, .RadPanelBar_Default div.rpSelected, .RadPanelBar_Default a.rpSelected:hover
        {
            background-color: #fdfdfd;
            border-color: lightgrey;
        }
    </style>
    <script>       if (typeof window.JSON === 'undefined') { document.write('<script src="../js/json2.js"><\/script>'); }</script>

    <style scoped>
        .chart-wrapper, .chart-wrapper .k-chart
        {
            margin: auto;
        }

        .rgExpand, .rgCollapse
        {
            display: none;
        }

        .RadGrid .rgPagerLabel, .RadGrid .rgPager .RadComboBox, .RadGrid .rgPager .RadInput
        {
            margin: 0 4px 0 0;
            vertical-align: middle;
            visibility: hidden;
        }

        #contentofpage a:link
        {
            color: black !important;
            text-decoration: none !important;
            border-style: none !important;
        }
        .visit-statistics td.td-col-7 {
    border-top: #ccc solid 1px !important;
    border-bottom: none  !important;
}

.visit-statistics td.td-col-8 {
    border-top: #ccc solid 1px !important;
     border-bottom: none  !important;
      border-right: none  !important;
}
@media (max-width: 768px){
    .filter-table {
        margin: 0 -15px 30px;
        padding: 10px 15px;
    }
}
    </style>


</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">


    <h4 style="margin-bottom: 0;">Dashboard</h4>

    <telerik:RadAjaxManager ID="RadAjaxManager2" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager2">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="Panel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <telerik:RadAjaxLoadingPanel ID="AjaxLoadingPanel1" runat="server">
        <div id="ProgressPanel" class="overlay">

            <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
        </div>
    </telerik:RadAjaxLoadingPanel>

    <asp:Panel ID="Panel3" runat="server"></asp:Panel>

    <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanel10">




         <telerik:RadWindow ID="ZeroBilledWindow" Title ="Zero Billed Customers" runat="server" modal="true"  Behaviors="Move,Close" 
           ReloadOnShow="true"  VisibleStatusbar="false" Height ="515px" BorderColor ="Black"  Width ="700px"  skin="Windows7"  >
               <ContentTemplate>
                           <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanel1">
                                    <telerik:RadGrid ID="rgZeroBilled" 
                                        AllowSorting="True" AutoGenerateColumns="False" Width="100%" BorderStyle="None"
                                        PageSize="9" AllowPaging="True" runat="server" Skin="Simple" AllowFilteringByColumn="false" ShowFooter="true"
                                        GridLines="None">

                                        <GroupingSettings CaseSensitive="false"></GroupingSettings>
                                        <ClientSettings EnableRowHoverStyle="true">
                                        </ClientSettings>
                                        <MasterTableView AutoGenerateColumns="false"   ShowFooter="true"  AllowFilteringByColumn ="false" 
                                            TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                                            PageSize="9">
                                              
                                            <Columns>

                                                <telerik:GridBoundColumn DataField="Description" HeaderText="Customer" SortExpression="Description"
                                                     ShowFilterIcon="false" AllowFiltering="true"  FilterControlToolTip="Type customer and press enter"
                                                    CurrentFilterFunction="Contains" AutoPostBackOnFilter="true" Aggregate="Count" FooterText="Total Customers : ">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <FooterStyle HorizontalAlign="Left" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                                                </telerik:GridBoundColumn>
                                              
                                                <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" AllowFiltering ="false" ShowFilterIcon ="false" DataField="LastBilledOn" HeaderText="Last Billed On" SortExpression ="LastBilledOn"
                                                               DataFormatString="{0:dd-MMM-yyyy}"  >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>
                                                      
                                            </Columns>
                                        </MasterTableView>


                                    </telerik:RadGrid>
                                    

                                </telerik:RadAjaxPanel>
                   
               </ContentTemplate>
          </telerik:RadWindow>








        <asp:HiddenField ID="hfCurrency" runat="server" />
        <asp:HiddenField ID="hfDecimal" runat="server" />
        <asp:HiddenField ID="hfSE" runat="server" />
        <asp:HiddenField ID="hfAllVan" runat="server" />
        <asp:HiddenField ID="hfSMonth" runat="server" />
        <asp:HiddenField ID="hfRow" runat="server" />
        <asp:HiddenField ID="hfUserID" runat="server" />

        <div class="filter-table form-inline">
            <div class="row" >
            <div class="col-sm-4" id="tdctry2"  runat="server">
            <div class="form-group" >
                <label> Country  </label> 
                
                <div class="form-control">
                    <telerik:RadComboBox Skin="Simple" EmptyMessage="Select Country" ID="ddlCountry" Width="200px" runat="server" DataTextField="Country" DataValueField="MAS_ORG_ID" AutoPostBack="true"></telerik:RadComboBox>
                    <telerik:RadComboBox Skin="Simple" Visible ="false" EmptyMessage="Select Sales Org" EnableCheckAllItemsCheckBox="true" CheckBoxes="true" ID="ddlOrganization" Width="100%" runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" OnClientDropDownClosed="OnClientDropDownClosedHandler" AutoPostBack="false"></telerik:RadComboBox>
                </div>
            </div>
            </div>
            <div class="col-sm-3" style="display:none;">
            <div class="form-group">
                <label id="Label1" runat="server" visible="false">Van</label>
                <div id="Div1" runat ="server" visible ="false" class="form-control">
                    <telerik:RadComboBox runat="server"  ID="ddlFSR" Visible="false"  Filter="Contains" Height="200" Width="300px" Skin="Simple" AutoPostBack="true"></telerik:RadComboBox>
                </div>
            </div>
            </div>
            <div class="col-sm-4">
            <div class="form-group">
                <label>Month</label>
                <div class="form-control">
                   <%-- <telerik:RadDatePicker ID="StartTime" AutoPostBack="true" runat="server" >
                        <DateInput ReadOnly="true" DisplayDateFormat="MMM-yyyy">
                        </DateInput>
                        <Calendar ID="Calendar2" runat="server">
                            <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="OK" />
                        </Calendar>
                    </telerik:RadDatePicker>--%>
                     <telerik:RadMonthYearPicker RenderMode="Lightweight" Skin="Simple" AutoPostBack="true" Width="100%" ID="StartTime" runat="server">
                                                <DateInput ReadOnly="true" DisplayDateFormat="MMM-yyyy">
                                                </DateInput>

                                            </telerik:RadMonthYearPicker>
                </div>
            </div>
            </div>
            <div class="col-sm-2">
            <div class="form-group">
                <label>Currency</label>
                <div class="form-control" style="padding: 6px 11px;">
                    <asp:Label runat="server" ID="lblC" Font-Bold="true" ForeColor="#248aaf"></asp:Label>
                </div>
            </div>
            </div>
                <div class="col-sm-6">
            <div class="form-group text-right" style="display:block;">
                <label> </label>
                <div class="form-control" style="padding: 6px 11px; text-align:right " >
                    <asp:HyperLink ID="hyp_IncomingMsg" runat="server" NavigateUrl="~/html/DashboardIncomingMsgs.aspx" Font-Underline="true" Font-Bold="true"   >Incoming Messages</asp:HyperLink>
                </div>
            </div>
            </div>
            </div>
        </div>

       
                

        <div class="row">
            <div class="col-sm-6 col-md-6 col-lg-4">
                <div class="dashsecblock">
                    <div class="dashsectitle">
                        <asp:LinkButton runat="server" ID="lbVanLink" ToolTip="View all van sales" Text="Top 5 Sales by Van (Value)"   ></asp:LinkButton>
                    </div>
               
                                <telerik:RadScriptBlock runat="server" ID="RadScriptBlock56">
                                    <script type="text/javascript">



                                        function createChart1() {
                                            var param = document.getElementById('<%= hfAllVan.ClientID%>').value;
                                            var param1 = document.getElementById('<%= hfSMonth.ClientID %>').value;
                                            var currency = document.getElementById('<%= lblC.ClientID%>').innerHTML;
                                            $("#chart1").kendoChart({
                                                theme: $(document).data("kendoSkin") || "flat",
                                                dataSource: {
                                                    type: "json",
                                                    transport: {
                                                        read: {
                                                            url: "Chart.asmx/getDashTop10VanSales", //specify the URL which should create new records. This is the Create method of the Products.asmx service.
                                                            contentType: "application/json; charset=utf-8", // tells the web service to serialize JSON
                                                            type: "POST", //use HTTP POST request as the default GET is not allowed for ASMX
                                                            dataType: "json"
                                                        },
                                                        parameterMap: function (data, operation) {

                                                            var output = null;
                                                            switch (operation) {

                                                                case "read":
                                                                    output = '{ param: ' + JSON.stringify(param) + ',param1: ' + JSON.stringify(param1) + '}';
                                                                    break;

                                                            }
                                                            return output;
                                                            return kendo.stringify(data); // stringify the data send back to the server
                                                        }
                                                    },
                                                    schema: {
                                                        data: "d", // ASMX services return JSON in the following format { "d": <result> }. Specify how to get the result.
                                                        model: { // define the model of the data source.
                                                            fields: {

                                                                Description: { type: "string" },
                                                                Unit: { type: "number" }

                                                            }
                                                        }
                                                    },
                                                    sort: {
                                                        field: "Description",
                                                        dir: "asc"
                                                    }
                                                },
                                                title: {
                                                    text: "",

                                                    font: "12px Segoe UI"

                                                },
                                                legend: {

                                                    position: "bottom",
                                                    labels: {
                                                        font: "12px Segoe UI", color: "#333",
                                                        background: "black"
                                                    }

                                                },

                                                chartArea: {
                                                    background: "#fff"
                                                },


                                                seriesColors: ["#fac552", "#248aaf", "#3cbc8d", "#29b7d3", "#6d90c5", "#0099FF", "#666600", "#CCFF00", "#FF6600", "#CCFF99", "#66CCCC", "#CC0066", "#009999", "#330066"],
                                                seriesDefaults: {
                                                    type: "pie",
                                                    stacked: true,

                                                    labels: {

                                                        visible: true,
                                                        padding: {
                                                            top: -8

                                                        },
                                                        format: "{0:N0}",

                                                        font: "12px Segoe UI"
                                                    }

                                                },

                                                categoryAxis:
         {
             labels: { font: "12px Segoe UI" },
             majorGridLines: {
                 visible: true
             }
         },

                                                minorGridLines: {
                                                    visible: false
                                                },
                                                majorGridLines: {
                                                    visible: true
                                                },
                                                valueAxis:
         {

             labels: { font: "12px Segoe UI" },
             minorGridLines: {
                 visible: true
             },
             majorGridLines: {
                 visible: true
             }
         },


                                                series: [{
                                                    field: "Unit",

                                                    categoryField: "Description",
                                                    labels: {
                                                        font: "12px Segoe UI",
                                                        color: "#333"
                                                    }

                                                }],
                                                tooltip: {
                                                    visible: true,
                                                    template: "Van: #= dataItem.Description#  <br> Sales : " + currency + "  #=kendo.format('{0:N0}',dataItem.Unit)#",
                                                    font: "12px Segoe UI",
                                                    color: "white",
                                                    background: "black"
                                                }
                                            });
                                        }







                                    </script>

                                </telerik:RadScriptBlock>
                    <div class="dashseccontent">
                                <div class="chart-wrapper">
                                    <div id="chart1" style="height: 335px;">
                                    </div>
                                </div>
                   </div>
                    </div>                
            </div>
            <div class="col-sm-6 col-md-6 col-lg-4">
                <div class="dashsecblock">
                <div class="dashsectitle">
                                    <asp:LinkButton runat="server" ID="lbColLink" ToolTip="View all van collections" Text="Top 5 Collection by Van (Value)"  ></asp:LinkButton>
                                    
                                </div>
                    <div class="dashseccontent">
                                <div class="chart-wrapper">
                                    <div id="chart7" style="height: 335px;">
                                    </div>
                                  
                                </div>
                    </div>
            </div>
                                <telerik:RadScriptBlock runat="server" ID="RadScriptBlock4">

                                    <script type="text/javascript">



                                        function createChart7() {
                                            var param = document.getElementById('<%= hfAllVan.ClientID%>').value;
                                            var param1 = document.getElementById('<%= hfSMonth.ClientID %>').value;
                                            var currency = document.getElementById('<%= lblC.ClientID%>').innerHTML;
                                            $("#chart7").kendoChart({
                                                theme: $(document).data("kendoSkin") || "flat",
                                                dataSource: {
                                                    type: "json",
                                                    transport: {
                                                        read: {
                                                            url: "Chart.asmx/getDashTop10VanCollections", //specify the URL which should create new records. This is the Create method of the Products.asmx service.
                                                            contentType: "application/json; charset=utf-8", // tells the web service to serialize JSON
                                                            type: "POST", //use HTTP POST request as the default GET is not allowed for ASMX
                                                            dataType: "json"
                                                        },
                                                        parameterMap: function (data, operation) {

                                                            var output = null;
                                                            switch (operation) {

                                                                case "read":
                                                                    output = '{ param: ' + JSON.stringify(param) + ',param1: ' + JSON.stringify(param1) + '}';
                                                                    break;

                                                            }
                                                            return output;
                                                            return kendo.stringify(data); // stringify the data send back to the server
                                                        }
                                                    },
                                                    schema: {
                                                        data: "d", // ASMX services return JSON in the following format { "d": <result> }. Specify how to get the result.
                                                        model: { // define the model of the data source.
                                                            fields: {

                                                                Description: { type: "string" },
                                                                Unit: { type: "number" }

                                                            }
                                                        }
                                                    },
                                                    sort: {
                                                        field: "Description",
                                                        dir: "asc"
                                                    }
                                                },
                                                title: {
                                                    text: "",

                                                    font: "12px Segoe UI"

                                                },
                                                legend: {

                                                    position: "bottom",
                                                    labels: {
                                                        font: "12px Segoe UI", color: "#333",
                                                        background: "black"
                                                    }

                                                },

                                                chartArea: {
                                                    background: "#fff"
                                                },
                                                seriesDefaults: {
                                                    type: "pie", labels: {
                                                        visible: true,
                                                        padding: {
                                                            top: -8

                                                        },
                                                        format: "{0:N0}",

                                                        font: "12px Segoe UI"
                                                    }

                                                },

                                                categoryAxis:
    {
        labels: { font: "12px Segoe UI" }
    },
                                                valueAxis:
    {
        labels: { font: "12px Segoe UI" }
    },
                                                seriesColors: ["#3cbc8d", "#29b7d3", "#6d90c5", "#0099FF", "#fac552", "#248aaf", "#3366FF", "#7FFF00", "#1E90FF", "#8A2BE2", "#FF00FF", "#FF1493", "#B8860B", "#8B4513", "#FF4500", "#00FFFF", "#00FF00", "#0000FF", "#2F4F4F", "#4682B4", "#800080", "#C71585", "#00CED1", "#66CDAA", "#9ACD32", "#CD853F", "#FFA07A", "#A52A2A", "#696969", "#000000", "#708090", "#191970", "#DDA0DD", "#DB7093", "#20B2AA", "#00FA9A", "#BDB76B", "#FFD700", "#FFDEAD", "#FF6347", "#FF0000", "#006400", "#808000", "#FFFF00"],

                                                series: [{
                                                    field: "Unit",
                                                    categoryField: "Description",
                                                    labels: {
                                                        font: "12px Segoe UI",
                                                        color: "#333"
                                                    }

                                                }],
                                                tooltip: {
                                                    visible: true,
                                                    template: "Van: #= dataItem.Description#  <br> Collection:  " + currency + "  #=kendo.format('{0:N0}',dataItem.Unit)#",
                                                    format: "{0:N0}",
                                                    font: "12px Segoe UI",
                                                    color: "white",
                                                    background: "black"
                                                }
                                            });
                                        }



                                    </script>

                                </telerik:RadScriptBlock>
            </div>
            <div class="col-sm-12 col-md-12 col-lg-4">
                <div class="dashsecblock">
                <div class="dashsectitle">
                                    Calls,Coverage & Productivity
                                </div>
                    <div class="dashseccontent">
                                <div class="chart-wrapper">
                                    <table cellpadding="0" cellspacing="0" border="0" width="100%"  height="261px;" class="visit-statistics">
                                        <tr>
                                            <td class="td-col-3">
                                                <span style="font-size: 14px; font: 'Segoe UI'; text-align: center; padding-left: 2px;">Scheduled Customers
                                                                <asp:Label ID="lblScheduled" runat="server" Font-Bold="true" Font-Size="24px" ForeColor="#248aaf"></asp:Label>
                                                </span>
                                            </td>
                                            <td class="td-col-6">
                                                <span style="font-size: 14px; font: 'Segoe UI'; text-align: center; padding-left: 2px;">Total Calls
                                                            <asp:Label runat="server" ID="lblTotCalls" Font-Bold="true" ForeColor="#248aaf" Font-Size="24px">
  
                                                            </asp:Label></span>

                                            </td>

                                        </tr>
                                        <tr>
                                            <td class="td-col-5">
                                                <span style="font-size: 14px; font: 'Segoe UI'; text-align: center; padding-left: 2px;">Customers Visited
                                                                <asp:Label ID="lblOutlets" runat="server" Font-Bold="true" Font-Size="24px" ForeColor="#248aaf"></asp:Label>
                                                </span>

                                            </td>



                                            <td class="td-col-4">
                                                <span style="font-size: 14px; font: 'Segoe UI'; text-align: center; padding-left: 2px;">Productive Calls
                                                            <asp:Label runat="server" ID="lblProdCalls" Font-Bold="true" ForeColor="#248aaf" Font-Size="24px">
  
                                                            </asp:Label></span>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="td-col-5">
                                                <span style="font-size: 14px; font: 'Segoe UI'; text-align: center; padding-left: 2px;">Coverage
                                                            <asp:Label runat="server" ID="lblCoverage" Font-Bold="true" ForeColor="#248aaf" Font-Size="24px">
                                                            </asp:Label></span>
                                            </td>
                                            <td class="td-col-4">
                                                <span style="font-size: 14px; font: 'Segoe UI'; text-align: center; padding-left: 2px;">Productivity
                                                            <asp:Label runat="server" ID="lblProdPercent" Font-Bold="true" Font-Size="24px" ForeColor="#248aaf">
 
                                                            </asp:Label></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="td-col-2" >
                                                <span style="font-size: 14px; font: 'Segoe UI'; text-align: center; padding-left: 2px;">Zero Billed Customers<br />

                                                    <%-- <asp:LinkButton runat="server" ID="lblZeroBilled" Font-Underline ="true" ToolTip ="View Zero Billed Customer" Font-Bold="true"
                                                         OnClientClick ="OpenDialog(this);  return false"
                                                          Font-Size="24px" ForeColor="#248aaf"  ></asp:LinkButton>--%>
                                                    <asp:LinkButton runat="server" ID="lblZeroBilled" Font-Underline ="true" ToolTip ="View Zero Billed Customer" Font-Bold="true"
                                                        
                                                          Font-Size="24px" ForeColor="#248aaf"  ></asp:LinkButton>


                                                         </span>
                                            </td>
                                            <td class="td-col-1">
                                                <span style="font-size: 14px; font: 'Segoe UI'; text-align: center; padding-left: 2px;">Customers Billed
                                                            <asp:Label runat="server" ID="lblOutBilled" Font-Bold="true" Font-Size="24px" ForeColor="#248aaf">
 
                                                            </asp:Label></span>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                        </div>
                    </div>
            </div>
            </div>
        <div class="row">
            <div class="col-sm-6 col-md-6 col-lg-4">
                <div class="dashsecblock">
                <div class="dashsectitle">
                                   
                                     <asp:LinkButton runat="server" ID="lbAgencySaleLink" ToolTip ="View all agency sales" Text="Top 5 Sales by Agency (Value)" ></asp:LinkButton>
                                </div>
                                <telerik:RadScriptBlock runat="server" ID="RadScriptBlock556">

                                    <script type="text/javascript">



                                        function createChart4() {
                                            var param = document.getElementById('<%= hfAllVan.ClientID%>').value;
                                            var param1 = document.getElementById('<%= hfSMonth.ClientID %>').value;
                                            var currency = document.getElementById('<%= lblC.ClientID%>').innerHTML;
                                            $("#chart4").kendoChart({
                                                theme: $(document).data("kendoSkin") || "flat",
                                                dataSource: {
                                                    type: "json",
                                                    transport: {
                                                        read: {
                                                            url: "Chart.asmx/getDashTop10SalesByAgency", //specify the URL which should create new records. This is the Create method of the Products.asmx service.
                                                            contentType: "application/json; charset=utf-8", // tells the web service to serialize JSON
                                                            type: "POST", //use HTTP POST request as the default GET is not allowed for ASMX
                                                            dataType: "json"
                                                        },
                                                        parameterMap: function (data, operation) {

                                                            var output = null;
                                                            switch (operation) {

                                                                case "read":
                                                                    output = '{ param: ' + JSON.stringify(param) + ',param1: ' + JSON.stringify(param1) + '}';
                                                                    break;

                                                            }
                                                            return output;
                                                            return kendo.stringify(data); // stringify the data send back to the server
                                                        }
                                                    },
                                                    schema: {
                                                        data: "d", // ASMX services return JSON in the following format { "d": <result> }. Specify how to get the result.
                                                        model: { // define the model of the data source.
                                                            fields: {

                                                                Description: { type: "string" },
                                                                Unit: { type: "number" }

                                                            }
                                                        }
                                                    },
                                                    sort: {
                                                        field: "Description",
                                                        dir: "asc"
                                                    }
                                                },
                                                title: {
                                                    text: "",

                                                    font: "12px Segoe UI"

                                                },
                                                legend: {

                                                    position: "bottom",
                                                    labels: {
                                                        font: "12px Segoe UI", color: "#333",
                                                        background: "black"
                                                    }

                                                },

                                                chartArea: {
                                                    background: "#fff"
                                                },
                                                seriesDefaults: {
                                                    type: "pie", labels: {
                                                        visible: true,
                                                        padding: {
                                                            top: -8

                                                        },
                                                        format: "{0:N0}",

                                                        font: "12px Segoe UI"
                                                    }

                                                },

                                                categoryAxis:
    {
        labels: { font: "12px Segoe UI" }
    },
                                                valueAxis:
    {
        labels: { font: "12px Segoe UI" }
    },
                                                seriesColors: ["#FFCC00", "#33CCFF", "#688AC9", "#E67A77", "#0B62A4", "#C4C16A", "#CE5F5F", "#688AC9", "#E67A77", "#0B62A4", "#0B62A4", "#7A92A3", "#CE1663"],

                                                series: [{
                                                    field: "Unit",
                                                    categoryField: "Description",
                                                    labels: {
                                                        font: "12px Segoe UI",
                                                        color: "#333"
                                                    }

                                                }],
                                                tooltip: {
                                                    visible: true,
                                                    template: "Agency: #= dataItem.Description#  <br> Sales: " + currency + "  #=kendo.format('{0:N0}',dataItem.Unit)#",
                                                    format: "{0:N0}",
                                                    font: "12px Segoe UI",
                                                    color: "white",
                                                    background: "black"
                                                }
                                            });
                                        }



                                    </script>

                                </telerik:RadScriptBlock>
                    <div class="dashseccontent">
                                <div class="chart-wrapper">
                                    <div id="chart4" style="height: 261px;">
                                    </div>
                                </div>
                        </div>
                    </div>
            </div>
            <div class="col-sm-6 col-md-6 col-lg-4">
                <div class="dashsecblock">
                <div class="dashsectitle">
                                    Productivity (Count)
                                </div>
                                <telerik:RadScriptBlock runat="server" ID="RadScriptBlock2">

                                    <script type="text/javascript">



                                        function createChart2() {
                                            var param = document.getElementById('<%= hfAllVan.ClientID %>').value;
                                            var param1 = document.getElementById('<%= hfSMonth.ClientID %>').value;

                                            $("#chart2").kendoChart({
                                                theme: $(document).data("kendoSkin") || "flat",
                                                dataSource: {
                                                    type: "json",

                                                    transport: {
                                                        read: {
                                                            url: "Chart.asmx/getDashTotalVsProductive", //specify the URL which should create new records. This is the Create method of the Products.asmx service.
                                                            contentType: "application/json; charset=utf-8", // tells the web service to serialize JSON
                                                            type: "POST", //use HTTP POST request as the default GET is not allowed for ASMX
                                                            dataType: "json"
                                                        },
                                                        parameterMap: function (data, operation) {

                                                            var output = null;
                                                            switch (operation) {

                                                                case "read":
                                                                    output = '{ param: ' + JSON.stringify(param) + ',param1: ' + JSON.stringify(param1) + '}';
                                                                    break;

                                                            }
                                                            return output;
                                                            return kendo.stringify(data); // stringify the data send back to the server
                                                        }
                                                    },
                                                    schema: {
                                                        data: "d", // ASMX services return JSON in the following format { "d": <result> }. Specify how to get the result.
                                                        model: { // define the model of the data source.
                                                            fields: {

                                                                Description: { type: "string" },
                                                                Unit: { type: "number" }

                                                            }
                                                        }
                                                    },
                                                    sort: {
                                                        field: "Description",
                                                        dir: "desc"
                                                    }
                                                },
                                                title: {
                                                    text: "",

                                                    font: "12px Segoe UI"

                                                },
                                                legend: {

                                                    position: "bottom",
                                                    labels: {
                                                        font: "12px Segoe UI", color: "#333",
                                                        background: "black"
                                                    }

                                                },

                                                chartArea: {
                                                    background: "#fff"
                                                },
                                                seriesDefaults: {
                                                    type: "pie", labels: {
                                                        visible: true,

                                                        padding: {
                                                            top: -8

                                                        },
                                                        format: "{0:N0}",

                                                        font: "12px Segoe UI"
                                                    }

                                                },

                                                categoryAxis:
    {
        labels: { font: "12px Segoe UI" }
    },
                                                valueAxis:
    {
        labels: { font: "12px Segoe UI" }
    },
                                                seriesColors: ["#6d90c5", "#0099FF"],
                                                series: [{
                                                    field: "Unit",
                                                    categoryField: "Description",
                                                    labels: {
                                                        font: "12px Segoe UI",
                                                        color: "#333"
                                                    }

                                                }],
                                                tooltip: {
                                                    visible: true,
                                                    format: "{0:N0}",
                                                    font: "12px Segoe UI",
                                                    color: "white",
                                                    background: "black"
                                                }
                                            });
                                        }



                                    </script>

                                </telerik:RadScriptBlock>
                    <div class="dashseccontent">
                                <div class="chart-wrapper">
                                    <div id="chart2" style="height: 261px;">
                                    </div>
                                </div>
                        </div>
                    </div>
            </div>
            <div class="col-sm-12 col-md-12 col-lg-4">
                <div class="dashsecblock">
                    <div class="dashsectitle">
                                    Transaction Statistics
                                </div>
                    <div class="dashseccontent">
                        <div class="chart-wrapper">
                        <table cellpadding="0" cellspacing="0" border="0" width="100%" height="261px;">
                                    <%--<tr>
                                                      <th style ="background-color:beige;font-weight:bold;text-align:left;padding-left:7px;font-size:12px;">Document</th>
                                                      <th style ="background-color:beige;font-weight:bold;text-align:center;font-size:12px;">Tot.Count</th>
                                                       <th style ="background-color:beige;font-weight:bold;text-align:center;font-size:12px;">Tot.Value </th>
                                                  </tr>--%>
                                    <tr>
                                        <%--<td  class="td-col-7" width="50px;" >
                                                        <span style="font-size: 14px; font:'Segoe UI'; text-align: left ;
                                                                padding-left: 6px;padding-right:2px;color:crimson;">
                                                               
                                                            </span> 
                                                        </td>--%>
                                        
                                        <td style="color:#26a65b;">
                                            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                <tr>
                                                    <td style="font-weight:bold; font-size:21px;">Orders</td>
                                                    <td>
                                                        
                                                        <div style="float: right;font-weight: 400;">
                                                            <span style=" color:#444;">
                                                                <asp:Label runat="server" ID="lblOrdCuur" Font-Size="12px"></asp:Label>
                                                            </span>
                                                            <asp:Label ID="lblOrdValue" runat="server" Font-Bold="true" Font-Size="24px"></asp:Label>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="border: #e1e1e1 solid 1px; padding: 0 4px; font-weight: 600;" width="50%">
                                                        <div style="float: left; padding-top: 5px; ">
                                                            <span style="font-size: 12px; color:#444;">Count</span>
                                                        </div>
                                                        <div style="float: right;">
                                                            <asp:Label ID="lblOrdCnt" runat="server" Font-Size="17px"></asp:Label>
                                                        </div>

                                                    </td>
                                                    <td style="border: #e1e1e1 solid 1px; padding: 0 4px; font-weight: 600;" width="50%">
                                                        <div style="float: left; padding-top: 5px; ">
                                                            <span style="font-size: 12px; color:#444;">Average</span>
                                                        </div>
                                                        <div style="float: right; ">
                                                            <asp:Label ID="lblAvgOrd" runat="server" Font-Size="17px"></asp:Label>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>

                                        </td>

                                    </tr>
                                    <tr>
                                        



                                        <td style="color:#ef4836;">

                                           <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                <tr>
                                                    <td style="font-weight:bold; font-size:21px;">Returns</td>
                                                    <td>
                                                        <div style="float: left; margin-top: 14px;">
                                                        </div>
                                                        <div style="float: right;font-weight: 400;">
                                                            <span style=" color:#444;">
                                                                <asp:Label runat="server" ID="lblRMACurr" Font-Size="12px"></asp:Label>
                                                            </span>
                                                            <asp:Label runat="server" ID="lblRMAValue" Font-Bold="true" Font-Size="24px"></asp:Label>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="border: #e1e1e1 solid 1px; padding: 0 4px;font-weight: 600;" width="50%">
                                                        <div style="float: left; padding-top: 5px;">
                                                            <span style="font-size: 12px; color:#444;">Count</span>
                                                        </div>
                                                        <div style="float: right;">
                                                            <asp:Label runat="server" ID="lblRMACount" Font-Size="17px"></asp:Label>
                                                        </div>
                                                    </td>
                                                    <td style="border: #e1e1e1 solid 1px; padding: 0 4px;font-weight: 600;" width="50%">
                                                        <div style="float: left; padding-top: 5px;">
                                                            <span style="font-size: 12px; color:#444;">Average</span>
                                                        </div>
                                                        <div style="float: right;">
                                                            <asp:Label ID="lblRMAAvg" runat="server" Font-Size="17px"></asp:Label>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>

                                        
                                        <td style="color:#248aaf;">


                                            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                <tr>
                                                    <td style="font-weight:bold; font-size:21px;">Collections</td>
                                                    <td>
                                                        
                                                        <div style="float: right;font-weight: 400;">
                                                            <span style=" color:#444;">
                                                                <asp:Label runat="server" ID="lblCollCurr" Font-Size="12px"></asp:Label>

                                                            </span>
                                                            <asp:Label runat="server" ID="lblColValue" Font-Bold="true" Font-Size="24px" >
 
                                                            </asp:Label>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="border: #e1e1e1 solid 1px; padding: 0 4px;font-weight: 600;" width="50%">
                                                        <div style="float: left; padding-top: 5px;">
                                                            <span style="font-size: 12px; color:#444;">Count</span>
                                                        </div>
                                                        <div style="float: right;">
                                                            <asp:Label runat="server" ID="lblColCnt" Font-Size="17px"></asp:Label>
                                                        </div>
                                                    </td>
                                                    <td style="border: #e1e1e1 solid 1px; padding: 0 4px;font-weight: 600;" width="50%">
                                                        <div style="float: left; padding-top: 5px;">
                                                            <span style="font-size: 12px; color:#444;">Average</span>
                                                        </div>
                                                        <div style="float: right;">
                                                            <asp:Label ID="lblAvgCol" runat="server"  Font-Size="17px"></asp:Label>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>


                                        </td>

                                    </tr>
                                </table>
                                <%-- <telerik:RadScriptBlock runat="server" ID="RadScriptBlock1">

                                                <script type="text/javascript">



                                                    function createChart6() {
                                                        var param = document.getElementById('<%= hfAllVan.ClientID%>').value;
                                                        var param1 = document.getElementById('<%= hfSMonth.ClientID %>').value;

                                                        $("#chart6").kendoChart({

                                                            dataSource: {
                                                                type: "json",
                                                                transport: {
                                                                    read: {
                                                                        url: "Chart.asmx/getSalesVsReturns", //specify the URL which should create new records. This is the Create method of the Products.asmx service.
                                                                        contentType: "application/json; charset=utf-8", // tells the web service to serialize JSON
                                                                        type: "POST", //use HTTP POST request as the default GET is not allowed for ASMX
                                                                        dataType: "json"
                                                                    }
                        ,
                                                                    parameterMap: function (data, operation) {

                                                                        var output = null;
                                                                        switch (operation) {

                                                                            case "read":
                                                                                output = '{ param: ' + JSON.stringify(param) + ',param1: ' + JSON.stringify(param1) + '}';
                                                                                break;

                                                                        }
                                                                        return output;
                                                                        return kendo.stringify(data); // stringify the data send back to the server
                                                                    }
                                                                },
                                                                schema: {
                                                                    data: "d", // ASMX services return JSON in the following format { "d": <result> }. Specify how to get the result.
                                                                    model: { // define the model of the data source.
                                                                        fields: {

                                                                            Description: { type: "string" },
                                                                            Unit: { type: "number" }

                                                                        }
                                                                    }
                                                                },
                                                                sort: {
                                                                    field: "Description",
                                                                    dir: "asc"
                                                                }
                                                            },
                                                            title: {
                                                                text: "",

                                                                font: "18px Segoe UI"

                                                            },
                                                            legend: {

                                                                position: "bottom",
                                                                labels: {
                                                                    font: "12px Segoe UI", color: "#333",
                                                                    background: "black"
                                                                }

                                                            },

                                                            chartArea: {
                                                                background: "#fff"
                                                            },
                                                            seriesDefaults: {
                                                                type: "pie", labels: {
                                                                    visible: true,
                                                                    padding: {
                                                                        top: -8

                                                                    },
                                                                    format: "{0:N0}",

                                                                    font: "12px Segoe UI"
                                                                }

                                                            },

                                                            categoryAxis:
                {
                    labels: { font: "12px Segoe UI " }
                },
                                                            valueAxis:
                {
                    labels: { font: "12px Segoe UI" }
                },
                                                            seriesColors: [  "#0B62A4",  "#CE1663"],

                                                            series: [{
                                                                field: "Unit",
                                                                categoryField: "Description",
                                                                labels: {
                                                                    font: "12px Segoe UI",
                                                                    color: "#333"
                                                                }

                                                            }],
                                                            tooltip: {
                                                                visible: true,
                                                                format: "{0:N0}",
                                                                font: "12px Segoe UI",
                                                                color: "white",
                                                                background: "black"
                                                            }
                                                        });
                                                    }



                                                </script>

                                            </telerik:RadScriptBlock>


                                            <div class="chart-wrapper">
                                                <div id="chart6" style="height: 260px;">
                                                </div>
                                                <br />
                                            </div>--%>
                            </div>
                    </div>
                </div>
            </div>
        </div>


        
        <script>
            
            $(function () {
                $('.rgMasterTable').find('th').attr("data-container", "body");
                $('body').tooltip({
                    selector: '[scope=col]',
                    container: 'body'
                });
            })
        </script>                   
                                <p style="font-size:15px; color:#333;"><strong>Van Log Summary</strong></p>
        <div class="overflowx">
                                <telerik:RadAjaxPanel runat="server" ID="g">
                                    <telerik:RadGrid ID="gvLog" 
                                        AllowSorting="True" AutoGenerateColumns="False" Width="100%" BorderStyle="None"
                                        PageSize="10" AllowPaging="True" runat="server" Skin="Simple" AllowFilteringByColumn="true" ShowFooter="true"
                                        GridLines="None" OnNeedDataSource="gvLog_NeedDataSource">

                                        <GroupingSettings CaseSensitive="false"></GroupingSettings>
                                        <ClientSettings EnableRowHoverStyle="true">
                                            
                                        </ClientSettings>

                                        <MasterTableView AutoGenerateColumns="false"  ShowFooter="true"  AllowFilteringByColumn ="true" 
                                            TableLayout="Auto" CommandItemDisplay="Top" Width="100%" GridLines="None" BorderColor="LightGray"
                                            PageSize="10">
                                               <CommandItemTemplate>
                                           <div style="float: right;">
                                       <asp:ImageButton ID="Button" runat="server" AlternateText="Clear filter" ToolTip="Clear filter" OnClick="Button_Click" ImageUrl="~/images/Clearfilter.png" />
                                   </div>
                                    </CommandItemTemplate>
                                            <Columns>

                                                <telerik:GridBoundColumn DataField="SalesRep_Name" HeaderText="Van" SortExpression="SalesRep_Name"
                                                     ShowFilterIcon="false" AllowFiltering="true"  FilterControlToolTip="Type van name and press enter"
                                                    CurrentFilterFunction="Contains" AutoPostBackOnFilter="true" Aggregate="Count" FooterText="Total Vans : " HeaderTooltip="Name of the Van">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <FooterStyle HorizontalAlign="Left" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="TSales" SortExpression="TSales" Aggregate="Sum" AllowFiltering="false" ShowFilterIcon="false" HeaderText="Total Sales" HeaderTooltip="Total order amount" >
                                                    <HeaderStyle HorizontalAlign="right" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                    <FooterStyle HorizontalAlign="Right" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="TCreditNote" AllowFiltering="false" ShowFilterIcon="false" Aggregate="Sum" DataFormatString="{0:N2}" HeaderText="Total Returns" SortExpression="TCreditNote">
                                                    <HeaderStyle HorizontalAlign="right" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                    <FooterStyle HorizontalAlign="Right" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Payment" HeaderText="Total Collections" SortExpression="Payment" Aggregate="Sum" AllowFiltering="false" ShowFilterIcon="false">
                                                    <HeaderStyle HorizontalAlign="Right" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                    <FooterStyle HorizontalAlign="Right" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                                                </telerik:GridBoundColumn>
                                                <%--    <telerik:GridBoundColumn DataField="ProdCalls" HeaderText="Productive Calls" Aggregate="Sum" FooterAggregateFormatString="{0:N0}" AllowFiltering="false" ShowFilterIcon="false" SortExpression="ProdCalls">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <FooterStyle HorizontalAlign="Center" Font-Bold="true" BackColor="#eff9ff" ForeColor="DarkMagenta" Font-Size="13px" />
                                                </telerik:GridBoundColumn>--%>

                                                <telerik:GridTemplateColumn UniqueName="ProdColumn" AllowFiltering="false" DataField ="ProdCalls" Aggregate ="Sum" FooterAggregateFormatString="{0:N0}"
                                                    InitializeTemplatesFirst="false" HeaderText="Productive Calls" SortExpression ="ProdCalls">


                                                    <ItemTemplate>

                                                        <asp:HyperLink runat="server" ToolTip ="View Customer Visits Report"  Font-Underline ="true" ForeColor="Navy" ID="hyp" Text='<%# Bind("ProdCalls") %>' NavigateUrl='<%# DataBinder.Eval(Container.DataItem, "MonthYear", "RepCustomerVisits.aspx?type=PC&id={0}")%>' Target="_top"></asp:HyperLink>

                                                    </ItemTemplate>

                                                  
                                                    <HeaderStyle HorizontalAlign="Center" Font-Bold ="true"  Font-Size ="13px"   />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <FooterStyle HorizontalAlign="Center" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                                                </telerik:GridTemplateColumn>


                                               <%-- <telerik:GridBoundColumn DataField="TotCalls" HeaderText="Total Calls" Aggregate="Sum" SortExpression="TotCalls" FooterAggregateFormatString="{0:N0}" AllowFiltering="false" ShowFilterIcon="false">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <FooterStyle HorizontalAlign="Center" Font-Bold="true" BackColor="Beige" ForeColor="DarkMagenta" Font-Size="13px" />
                                                </telerik:GridBoundColumn>--%>
                                                <telerik:GridTemplateColumn UniqueName="TotCallsColumn" AllowFiltering="false" DataField ="TotCalls" Aggregate ="Sum" FooterAggregateFormatString="{0:N0}"
                                                    InitializeTemplatesFirst="false" HeaderText="Total Calls"  SortExpression ="TotCalls">


                                                    <ItemTemplate>

                                                        <asp:HyperLink runat="server" Font-Underline ="true"  ToolTip ="View Customer Visits Report" ForeColor="Navy" ID="hyp1" Text='<%# Bind("TotCalls")%>' NavigateUrl='<%# DataBinder.Eval(Container.DataItem, "MonthYear", "RepCustomerVisits.aspx?type=TC&id={0}")%>' Target="_top"></asp:HyperLink>

                                                    </ItemTemplate>
                                                

                                                    <HeaderStyle HorizontalAlign="Center"  Font-Bold ="true"   Font-Size ="13px" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <FooterStyle HorizontalAlign="Center" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridBoundColumn DataField="ZeroBilled" HeaderText="Zero Billed Customers"  SortExpression="ZeroBilled"
                                                    AllowFiltering="false" ShowFilterIcon="false">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <FooterStyle HorizontalAlign="Center" Font-Bold="true" BackColor="Beige" ForeColor="DarkMagenta" Font-Size="13px" />
                                                </telerik:GridBoundColumn>
                                            </Columns>
                                        </MasterTableView>


                                    </telerik:RadGrid>
                                    

                                </telerik:RadAjaxPanel>

                       </div>    


                



    </telerik:RadAjaxPanel>


    <asp:UpdateProgress ID="UpdateProgress1"
        runat="server">
        <progresstemplate>
                        <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                            <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                            <span style="font-size: 12px; color:  #337AB7  ;">Processing... </span>
                        </asp:Panel>
                    </progresstemplate>
    </asp:UpdateProgress>

    <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1">
        <asp:Label ID="Label2" runat="server" ForeColor="#923137">Loading... </asp:Label>
        <asp:Image ID="Image1" runat="server" ImageUrl="~/images/Progress.gif" ImageAlign="Middle" />
    </telerik:RadAjaxLoadingPanel>
</asp:Content>

