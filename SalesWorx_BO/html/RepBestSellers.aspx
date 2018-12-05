<%@ Page Title="Best Sellers" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="RepBestSellers.aspx.vb" Inherits="SalesWorx_BO.RepBestSellers" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>




<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">

    <script src="../scripts/kendo.all.min.js"></script>
    <script src="../scripts/kendo.dataviz.min.js"></script>
    <link href="../styles/kendo.dataviz.black.min.css" rel="stylesheet" />


    <telerik:RadScriptBlock runat="server" ID="RadScriptBlock19">

        <script type="text/javascript">

            function RefreshChart() {
                createChart1();
              


            }
            function RefreshChart1() {
                createChart2();



            }
            function onDataBound(e) {
                $("#MainContent_Panel1").hide()

            }

            function createChart1() {
                var param = document.getElementById('<%= hfOrg.ClientID%>').value;
                var param1 = document.getElementById('<%= hfSE.ClientID%>').value;
                var param2 = document.getElementById('<%= hfSMonth.ClientID%>').value;
                var param3 = document.getElementById('<%= hfEMonth.ClientID%>').value;
                var currency = document.getElementById('<%= lblC.ClientID%>').innerHTML;

                $("#MainContent_Panel1").show()

                $("#chart1").kendoChart({
                    theme: $(document).data("kendoSkin") || "flat",
                    dataBound: onDataBound,
                    dataSource: {
                        type: "json",
                        transport: {
                            read: {
                                url: "Chart.asmx/getBestSellers", //specify the URL which should create new records. This is the Create method of the Products.asmx service.
                                contentType: "application/json; charset=utf-8", // tells the web service to serialize JSON
                                type: "POST", //use HTTP POST request as the default GET is not allowed for ASMX
                                dataType: "json"
                            }
,
                            parameterMap: function (data, operation) {

                                var output = null;
                                switch (operation) {

                                    case "read":
                                        output = '{ param: ' + JSON.stringify(param) + ',param1: ' + JSON.stringify(param1) + ',param2: ' + JSON.stringify(param2) + ',param3: ' + JSON.stringify(param3) + '}';
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


                                    Unit: { type: "number" },
                                    DispOrder: { type: "number" },
                                    AgencyName: { type: "number" },
                                    Count: { type: "number" },
                                    Description: { type: "string" }




                                }
                            }
                        },




                    },






                    chartArea: {
                        background: "#fff",
                      
                    },
                    title: {
                        text: "",
                        font: "18px Segoe UI"

                    },
                    legend: {
                        position: "top",
                        visible:true,
                        labels: {
                            font: "12px Segoe UI", color: "#333",
                            background: "black"

                        }

                    },


                   
                    valueAxis: [
                        
                        {
                            name: "DispOrder",
                           
                            color: "#97c95d",
                            title: { text: "Volume", font: "bold 12px Segoe UI", color: "#97c95d" },
                            labels: {
                                font: "bold 12px Segoe UI", color: "#97c95d"
                            }
                        },
                        {
                            name: "Unit",
                            color: "#ef9933",


                            title: { text: "Value (" + currency + ")", font: "bold 12px Segoe UI", color: "#ef9933" },
                            labels: {
                                font: "bold 12px Segoe UI", color: "#ef9933"
                            }
                        },

                    {
                        name: "AgencyName",
                           
                        color: "dodgerblue",
                           
                        title: { text: "No.of Invoices", font: "bold 12px Segoe UI", color: "dodgerblue" },
                labels: {
                    font: "bold 12px Segoe UI", color: "dodgerblue"
                }
                    }
                    //,
                    // {
                    //     name: "Count",

                    //     color: "DarkMagenta",

                    //     title: { text: "No.of Products", font: "bold 12px Segoe UI", color: "DarkMagenta" },
                    //     labels: {
                    //         font: "bold 12px Segoe UI", color: "DarkMagenta"
                    //     }
                    // }
                    ]

                    ,
                    categoryAxis:
{
    field: "Description",
    title: { text: "Vans", font: "bold 12px  Segoe UI", color: "#4d5ec1" },
    color: "#4d5ec1",
    axisCrossingValues: [0, 0, 100, 100],
    labels: {
        font: "bold 12px  Segoe UI", color: "#4d5ec1",
        rotation: 90
    },

    majorGridLines: {
        visible: true
    }
},

                    seriesColors: ["#97c95d", "#ef9933", "Crimson"],
                    series: [
                        {
                            field: "DispOrder",
                            type: "column",

                            axis: "DispOrder",
                            tooltip: {
                                visible: true,
                                template: "Van : #= dataItem.Description#  <br> Volume : #=dataItem.DispOrder# ",
                                font: "12px Segoe UI",
                                color: "white",
                                background: "black"
                            },
                            labels: {
                                font: "bold 12px  Segoe UI", color: "#ff7663"
                            }

                        },
                        {
                        field: "Unit",
                        type: "column",
                       
                        axis: "Unit",
                        tooltip: {
                            visible: true,
                           
                            template: "Van : #= dataItem.Description#  <br> Value : " + currency + "  #=kendo.format('{0:N0}',dataItem.Unit)#",
                            font: "12px Segoe UI",
                            color: "white",
                            background: "black"
                        },
                        labels: {

                            font: "bold 12px  Segoe UI", color: "#1c9ec4"
                        }

                    },

                    
                     {
                         field: "AgencyName",
                         type: "line",
                         style: "smooth",
                         width: "1px",
                         axis: "AgencyName",
                         color: "dodgerblue",
                         tooltip: {
                             visible: true,
                             template: "Van : #= dataItem.Description#  <br> Tot.Invoices : #=dataItem.AgencyName# ",
                             font: "12px Segoe UI",
                             color: "white",
                             background: "black"
                         },
                         labels: {
                             font: "bold 12px  Segoe UI", color: "dodgerblue"
                         }

                     }
                     //,
                     //{
                     //    field: "Count",
                     //    type: "line",
                     //    style: "smooth",
                     //    width: "1px",
                     //    axis: "Count",
                     //    color: "Darkmagenta",
                     //    tooltip: {
                     //        visible: true,
                     //        template: "Van : #= dataItem.Description#  <br> Tot.Products : #=dataItem.Count# ",
                     //        font: "12px Segoe UI",
                     //        color: "white",
                     //        background: "black"
                     //    },
                     //    labels: {
                     //        font: "bold 12px  Segoe UI", color: "Darkmagenta"
                     //    }

                     //}
                    ],


                    tooltip: {
                        visible: true,
                        format: "{0:N0}",
                        font: "12px Segoe UI",
                        color: "white",
                        background: "black"
                    }
                });
            }


            function createChart2() {
                var param = document.getElementById('<%= hfOrg.ClientID%>').value;
                   var param1 = document.getElementById('<%= hfSE.ClientID%>').value;
                   var param2 = document.getElementById('<%= hfSMonth.ClientID%>').value;
                   var param3 = document.getElementById('<%= hfEMonth.ClientID%>').value;
                var currency = document.getElementById('<%= lblC.ClientID%>').innerHTML;

                $("#MainContent_Panel1").show()

                   $("#chart2").kendoChart({
                       theme: $(document).data("kendoSkin") || "flat",
                       dataBound: onDataBound,
                       dataSource: {
                           type: "json",
                           transport: {
                               read: {
                                   url: "Chart.asmx/getBestSellersBySKU", //specify the URL which should create new records. This is the Create method of the Products.asmx service.
                                   contentType: "application/json; charset=utf-8", // tells the web service to serialize JSON
                                   type: "POST", //use HTTP POST request as the default GET is not allowed for ASMX
                                   dataType: "json"
                               }
   ,
                               parameterMap: function (data, operation) {

                                   var output = null;
                                   switch (operation) {

                                       case "read":
                                           output = '{ param: ' + JSON.stringify(param) + ',param1: ' + JSON.stringify(param1) + ',param2: ' + JSON.stringify(param2) + ',param3: ' + JSON.stringify(param3) + '}';
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


                                       Unit: { type: "number" },
                                       DispOrder: { type: "number" },
                                       AgencyName: { type: "number" },
                                       Count: { type: "string" },
                                       Description: { type: "string" }




                                   }
                               }
                           },




                       },






                       chartArea: {
                           background: "#fff",

                       },
                       title: {
                           text: "",
                           font: "18px Segoe UI"

                       },
                       legend: {
                           visible:true,
                           position: "top"

                       },
                       
                       valueAxis: [



                           {
                               name: "DispOrder",
                               color: "#97c95d",
                               title: { text: "Volume", font: "bold 12px Segoe UI", color: "#97c95d" },
                               labels: {
                                   font: "bold 12px Segoe UI", color: "#97c95d"
                               }
                           },
                           {
                               name: "Unit",
                               color: "#ef9933",


                               title: { text: "Value (" + currency + ")", font: "bold 12px Segoe UI", color: "#ef9933" },
                               labels: {
                                   font: "bold 12px Segoe UI", color: "#ef9933"
                               }
                           },
                       {
                           name: "AgencyName",

                           color: "Crimson",

                           title: { text: "No.of Invoices", font: "bold 12px Segoe UI", color: "Crimson" },
                           labels: {
                               font: "bold 12px Segoe UI", color: "Crimson"
                           }
                       }
                       ]

                       ,
                       categoryAxis:
   {
       field: "Description",
       title: { text: "SKU", font: "bold 12px  Segoe UI", color: "#4d5ec1" },
       color: "#4d5ec1",
       axisCrossingValues: [0, 0, 100, 100],
       labels: {
           font: "bold 12px  Segoe UI", color: "#4d5ec1",
           rotation: 90

       },

       majorGridLines: {
           visible: true
       }
   },
                       seriesColors: ["#97c95d", "#ef9933", "Crimson"],

                       series: [

                       {
                           field: "DispOrder",
                           type: "column",
                           width: 30,
                           gap: 1,
                           axis: "DispOrder",
                           tooltip: {
                               visible: true,
                               template: "SKU : #= dataItem.Count#  <br> Volume : #=dataItem.DispOrder# ",
                               font: "12px Segoe UI",
                               color: "white",
                               background: "black"
                           },
                           labels: {
                               font: "bold 12px  Segoe UI", color: "#ef9933"
                           }

                       },
                       {
                           field: "Unit",
                           type: "column",
                           width: 30,
                           gap: 1,
                           axis: "Unit",
                           tooltip: {
                               visible: true,

                               template: "SKU : #= dataItem.Count#  <br> Value : " + currency + "  #=kendo.format('{0:N0}',dataItem.Unit)#",
                               font: "12px Segoe UI",
                               color: "white",
                               background: "black"
                           },
                           labels: {

                               font: "bold 12px  Segoe UI", color: "#14b4fc"
                           }

                       },
                        {
                            field: "AgencyName",
                            type: "line",
                            style: "smooth",
                            width: "1px",
                            axis: "AgencyName",
                            color: "Crimson",
                            tooltip: {
                                visible: true,
                                template: "SKU : #= dataItem.Count#  <br> Tot.Invoices : #=dataItem.AgencyName# ",
                                font: "12px Segoe UI",
                                color: "white",
                                background: "black"
                            },
                            labels: {
                                font: "bold 12px  Segoe UI", color: "Crimson"
                            }

                        }
                       ],


                       tooltip: {
                           visible: true,
                           format: "{0:N0}",
                           font: "12px Segoe UI",
                           color: "white",
                           background: "black"
                       }
                   });
               }

            function pageLoad(sender, args) {
                $('.rgMasterTable').find('th > a').attr("data-container", "body");
                $('.rgMasterTable').find('th > a').attr("data-toggle", "tooltip");
                $('[data-toggle="tooltip"]').tooltip();
            }

        </script>

    </telerik:RadScriptBlock>
    <style>
        .widgetblk {
    position: relative;
    min-height: 75px;
}
        .widgetblk .text-primary {
    position: absolute;
    right: 10px;
    bottom: 0;
    font-size:24px;
}
        input[type="text"].rdfd_
        {
            margin: 0 !important;
            padding: 0 !important;
            height: 0 !important;
            width: 0 !important;
        }

        .RadTabStrip .rtsLevel .rtsTxt {
    text-decoration: inherit;
    font-size: 13px;
    font-weight: bold;
    width:100%;
}
        div[id*="ReportDiv"]
        {
            overflow: hidden !important;
            WIDTH: 100%;
            direction: ltr;
        }
        .k-chart svg{
	margin:0 -14px;
}
    </style>
    <script>
        function clickSearch() {
            $("#MainContent_SearchBtn").click()
        }
        function clickExportExcel() {
            $("#MainContent_BtnExportExcel").click()
            return false

        }
        function clickExportPDF() {
            $("#MainContent_BtnExportPDF").click()
            return false
        }
        function alertCallBackFn(arg) {

        }


    </script>
    <style type="text/css">
        div.RadGrid .rgPager .rgAdvPart
        {
            display: none;
        }

        .rgGroupHeader td p
        {
            display: inline;
            margin: 0;
            padding: 0 10px;
            color: #000 !Important;
            font-weight: bold !Important;
        }

        .rgGroupHeader td
        {
            padding-left: 8px;
            padding-bottom: 2px;
            background-color: #eff9ff !Important;
            color: #000 !Important;
        }
         .RadGrid_Simple .rgCommandRow
        {
            background: whitesmoke;
            color: #000;
            /* height: 15px !important; */
        }

        .rgFooter td
        {
            border-top: 1px solid;
            border-color: #999 #c3c3c3;
            color: #000 !Important;
            background-color: #ebe8e8 !Important;
            font-weight: bold !Important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <h4>Best Sellers</h4>
    <telerik:RadWindowManager ID="RadWindowManager1" Skin="Simple" runat="server" EnableShadow="true">
    </telerik:RadWindowManager>


    <telerik:RadAjaxManager ID="RadAjaxManager2" runat="server">

        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager2">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxManager2" />
                </UpdatedControls>
            </telerik:AjaxSetting>

        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxPanel runat="server" ID="g">
        <asp:HiddenField ID="hfCurrency" runat="server" />
        <asp:HiddenField ID="hfDecimal" runat="server" />
        <asp:HiddenField ID="hfSE" runat="server" />
        <asp:HiddenField ID="hfSMonth" runat="server" />
        <asp:HiddenField ID="hfEMonth" runat="server" />
        <asp:HiddenField ID="hfOrg" runat="server" />

        <telerik:RadPanelBar runat="server" ID="rpbFilter" Skin="Simple" Width="100%"
            ExpandMode="MultipleExpandedItems">
            <Items>
                <telerik:RadPanelItem Expanded="True" Text=" ">

                    <ContentTemplate>
                        <div class="row">
                            <div class="col-sm-10 col-md-10 col-lg-10">
                                <div class="row">

                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <label>Organization<em><span>&nbsp;</span>*</em> </label>
                                            <telerik:RadComboBox Skin="Simple" Filter="Contains" ID="ddlOrganization" Width="100%" runat="server"
                                                 DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="True">
                                            </telerik:RadComboBox>
                                        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <label>Van/FSR<em><span>&nbsp;</span>*</em> </label>
                                            <telerik:RadComboBox Filter="Contains" Skin="Simple" EnableCheckAllItemsCheckBox="true"
                                                CheckBoxes="true" EmptyMessage="Select a Van/FSR" ID="ddlVan" Width="100%" runat="server">
                                            </telerik:RadComboBox>
                                        </div>
                                    </div>



                                    <div class="col-sm-3">
                                        <div class="form-group">
                                            <label>From Date</label>

                                            <telerik:RadDatePicker ID="txtFromDate" Width="100%" runat="server" Skin="Simple" >
                                                <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy">
                                                </DateInput>
                                                <Calendar ID="Calendar2" runat="server">
                                                    <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                </Calendar>
                                            </telerik:RadDatePicker>
                                        </div>
                                    </div>
                                    <div class="col-sm-3">
                                        <div class="form-group">
                                            <label>To Date</label>
                                            <telerik:RadDatePicker ID="txtToDate" Width="100%" runat="server" Skin="Simple" >
                                                <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy">
                                                </DateInput>
                                                <Calendar ID="Calendar1" runat="server">
                                                    <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                </Calendar>
                                            </telerik:RadDatePicker>
                                        </div>
                                    </div>


                                    <div class="col-sm-4">
                                        <div class="form-group">
                                            <label></label>

                                        </div>
                                    </div>


                                </div>
                            </div>
                            <div class="col-sm-2 col-md-2 col-lg-2">
                                <div class="form-group">
                                    <label>&nbsp;</label>
                                    <asp:Button CssClass="btn btn-sm btn-block btn-primary" ID="SearchBtn" runat="server" Text="Search" />
                                     <asp:Button  CssClass ="btn btn-sm btn-block btn-default"  ID="ClearBtn" runat="server" Text="Clear"  />
                                </div>
                                <div class="form-group fontbig text-center">
                                    <label>&nbsp;</label>
                                    <asp:HyperLink href="" CssClass="" ID="BtnExportDummyExcel" runat="server" OnClick="return clickExportExcel()"><i class="fa fa-file-excel-o text-success"></i></asp:HyperLink>
                                    <asp:HyperLink href="" CssClass="" ID="BtnExportDummyPDF" runat="server" OnClick="return clickExportPDF()"><i class="fa fa-file-pdf-o text-danger"></i></asp:HyperLink>

                                </div>
                            </div>
                        </div>

                    </ContentTemplate>
                </telerik:RadPanelItem>
            </Items>
        </telerik:RadPanelBar>

        <div id="Args" runat="server" visible="false">
            <div id="popoverblkouter">
                Hover on icon to view search criteria <i class="fa fa-info-circle">
                    <span class="popoverblk">
                        <p>
                            <strong>Organisation: </strong>
                            <asp:Label ID="lbl_org" runat="server" Text=""></asp:Label>
                        </p>
                        <p>
                            <strong>Van: </strong>
                            <asp:Label ID="lbl_van" runat="server" Text=""></asp:Label>
                        </p>
                        <p>
                            <strong>From Date: </strong>
                            <asp:Label ID="lbl_from" runat="server" Text=""></asp:Label>
                        </p>
                        <p>
                            <strong>To Date: </strong>
                            <asp:Label ID="lbl_To" runat="server" Text=""></asp:Label>
                        </p>
                    </span>
                </i>
            </div>
        </div>
 <div id="rpt" runat="server" visible="false" style="padding-top:5px">


         <div class="row" id="MainContent_summary">
        
           
            <div class="col-sm-4 col-md-4 col-lg-2 colblk">
                <div class="widgetblk widgetblkinsmall">Unique Outlets <!--<asp:Label ID="lblVCnt" runat="server"></asp:Label>--><div class="text-primary"><asp:Label ID="lblTeamSize" runat="server"></asp:Label></div>
                   
                </div>
            </div>
              <div class="col-sm-4 col-md-4 col-lg-2 colblk">
                <div class="widgetblk widgetblkinsmall">Unique Products <div class="text-primary"><asp:Label ID="lblTotProducts" runat="server"></asp:Label></div>
                  
                </div>
            </div>
            <div class="col-sm-4 col-md-4 col-lg-2 colblk">
                <div class="widgetblk widgetblkinsmall">Total Invoices <!--<asp:Label ID="lblTCount" runat="server"></asp:Label>--><div class="text-primary"><asp:Label ID="lblTotalInvoices" runat="server"></asp:Label></div>
                   
                </div>
            </div>
              <div class="col-sm-6 col-md-6 col-lg-2 colblk">
                <div class="widgetblk widgetblkinsmall">Volume <div class="text-primary"><asp:Label ID="lblVolume" runat="server"></asp:Label></div>
                  
                </div>
            </div>
                 <div class="col-sm-6 col-md-6 col-lg-4 colblk">
                <div class="widgetblk widgetblkinsmall">Value (<asp:Label ID="lblOrdCurr" runat="server"></asp:Label>)<div class="text-primary"><asp:Label ID="lblSales" runat="server"></asp:Label></div>
                    
                </div>
            </div>
           
        </div>

     <p><br /><br /></p>
        <div class="text-right">
            <p><strong>Currency <span class="text-blue"><asp:Label runat="server" ID="lblC"></asp:Label></span></strong></p>
        </div>
        <asp:HiddenField ID="hfActCnt" runat ="server" />
         <telerik:RadTabStrip ID="AgencyTab" runat="server" Skin="Windows7" MultiPageID="RadMultiPage21"
                                                SelectedIndex="0">
                                                <Tabs>
                                                  <telerik:RadTab Text="Top 10 Vans/FSR Value vs Volume vs No.Of Invoices" runat="server">
                                                    </telerik:RadTab>
                                                     
                                                     <telerik:RadTab Text="Value vs Volume By Van / Product" runat="server">
                                                    </telerik:RadTab>
                                                   
                                                   
                                                </Tabs>
                                            </telerik:RadTabStrip>
                                            <telerik:RadMultiPage ID="RadMultiPage21" runat="server" SelectedIndex="0">

                                                   <telerik:RadPageView ID="RadPageView1" runat="server" >
                               
                                  <div style ="text-align:center;padding-top:10px;">
                       
                                            <span style ="font-weight:bold;">Show by </span>
                                                <telerik:RadComboBox Skin="Simple" AutoPostBack ="true"   ID="ddlChartMode" Width="150px"
                                                    runat="server">
                                                    <Items>
                                                        <telerik:RadComboBoxItem Text="SKU" Value="SKU"></telerik:RadComboBoxItem>
                                                        <telerik:RadComboBoxItem Text="Van/FSR" Value="Van"></telerik:RadComboBoxItem>
                                                        
                                                       
                                                    </Items>
                                                </telerik:RadComboBox>  
                                                 
                                        
                    </div>
                                               <div class="chart-wrapper padding0" style="width:100%;"  id="divChart1" runat ="server"  >
<div id="chart1"> </div>



<p><br /><br /></p>


                                                                 <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanel3">
            <telerik:RadGrid ID="gvvans" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                PageSize="8" AllowPaging="false" runat="server" DataSourceID="SqlDataSource2"  AllowFilteringByColumn ="true" ShowFooter ="true" 
                GroupingSettings-RetainGroupFootersVisibility="true" GroupingSettings-GroupContinuesFormatString="" GroupingSettings-GroupContinuedFormatString=""
                GridLines="None">

                <GroupingSettings CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto"  ShowFooter ="true"  AllowFilteringByColumn ="true" 
                     Width="100%" GridLines="None" BorderColor="LightGray" DataSourceID="SqlDataSource2" CommandItemDisplay="Top"
                    PageSize="8">
                     <CommandItemTemplate>
                                           <div style="float: right;">
                                       <asp:ImageButton ID="Button1" runat="server" AlternateText="Clear filter" ToolTip="Clear filter" OnClick="Button1_Click" ImageUrl="~/images/Clearfilter.png" />
                                   </div>
                                    </CommandItemTemplate>
                    <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                    
                    <Columns>



                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Description" HeaderText="Van/FSR" FooterText ="Total : "
                            SortExpression="Description"  AllowFiltering ="true" ShowFilterIcon ="false" CurrentFilterFunction ="Contains" AutoPostBackOnFilter ="true" FilterControlToolTip ="Type the van name and press enter"  >
                            <ItemStyle Wrap="False" HorizontalAlign ="Left"  />
                            <HeaderStyle Wrap="False" HorizontalAlign ="Left"  />


                       
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TotOutlets" HeaderText="Unique Outlets<i class='fa fa-info-circle'></i>"
                            SortExpression="TotOutlets" AllowFiltering ="false"  HeaderTooltip="Number of unique outlets that have billed the item" >
                             <ItemStyle Wrap="False" HorizontalAlign ="Right"  />
                            <HeaderStyle Wrap="True" HorizontalAlign ="Center"  />
                             <FooterStyle HorizontalAlign="Center" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                        </telerik:GridBoundColumn>
                         <telerik:GridBoundColumn  DataField="ProdCount" HeaderText="Unique Products<i class='fa fa-info-circle'></i>"
                            SortExpression="ProdCount" AllowFiltering ="false"   HeaderTooltip="Unique Products Sold" >
                             <ItemStyle Wrap="False" HorizontalAlign ="Right"  />
                            <HeaderStyle Wrap="True" HorizontalAlign ="Center"  />
                             <FooterStyle HorizontalAlign="Center" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                        </telerik:GridBoundColumn>
                            
                       <telerik:GridBoundColumn  DataField="MnOrder" HeaderText="Tot.Invoices"
                            SortExpression="MnOrder" AllowFiltering ="false" Aggregate ="Sum"   >
                          <ItemStyle Wrap="False" HorizontalAlign ="Right"  />
                            <HeaderStyle Wrap="True" HorizontalAlign ="Center"  />
                             <FooterStyle HorizontalAlign="Center" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn  DataField="TotValue" HeaderText="Value<i class='fa fa-info-circle'></i>"
                            SortExpression="TotValue"  UniqueName="Value" AllowFiltering ="false" Aggregate ="Sum" HeaderTooltip="Total Sales value" >
                          <ItemStyle Wrap="False" HorizontalAlign ="Right"  />
                            <HeaderStyle Wrap="True" HorizontalAlign ="Center"  />
                             <FooterStyle HorizontalAlign="right" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn  DataField="DispOrder" HeaderText="Volume<i class='fa fa-info-circle'></i>"
                            SortExpression="DispOrder"  AllowFiltering ="false"  Aggregate ="Sum" HeaderTooltip="Total Sales volume" DataFormatString="{0:0,000.0000}">
                             <ItemStyle Wrap="False" HorizontalAlign ="Right"  />
                            <HeaderStyle Wrap="True" HorizontalAlign ="Center"  />
                             <FooterStyle HorizontalAlign="right" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                        </telerik:GridBoundColumn>

                    
                      
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:SWXConnectionString %>"
            SelectCommand="Rep_BestSellers" SelectCommandType="StoredProcedure">

            <SelectParameters>
                <asp:ControlParameter ControlID="hfOrg" Name="OID" DefaultValue="0" />
                <asp:ControlParameter ControlID="hfSE" Name="VanList" DefaultValue="0" />
                <asp:ControlParameter ControlID="hfSMonth" Name="FMonth" DefaultValue="01-01-1900" />
                <asp:ControlParameter ControlID="hfEMonth" Name="TMonth" DefaultValue="01-01-1900" />
                <asp:Parameter Name="Mode" DefaultValue="ChartGrid" />
                   <asp:Parameter Name="ChartMode" DefaultValue="Van" />
            </SelectParameters>

        </asp:SqlDataSource>


</div>
                                                          <div class="chart-wrapper padding0" style="width: 100%;" id="divChart2" runat ="server" >
                        <div id="chart2" >
                        </div>

                                                              <p><br /><br /></p>


                                                                 <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanel2">
            <telerik:RadGrid ID="gvSKU" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                PageSize="8" AllowPaging="false" runat="server" DataSourceID="SqlDataSource1"  AllowFilteringByColumn ="true"  ShowFooter ="true" 
                GroupingSettings-RetainGroupFootersVisibility="true" GroupingSettings-GroupContinuesFormatString="" GroupingSettings-GroupContinuedFormatString=""
                GridLines="None">

                <GroupingSettings CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto"  AllowFilteringByColumn ="true"  ShowFooter ="true" 
                     Width="100%" GridLines="None" BorderColor="LightGray" DataSourceID="SqlDataSource1"  CommandItemDisplay="Top"
                    PageSize="8">
                       <CommandItemTemplate>
                                           <div style="float: right;">
                                       <asp:ImageButton ID="Button" runat="server" AlternateText="Clear all filter" ToolTip="Clear all filter" OnClick="Button_Click" ImageUrl="~/images/Clearfilter.png" />
                                   </div>
                                    </CommandItemTemplate>
                    <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                    
                    <Columns>



                        <telerik:GridBoundColumn  DataField="Description" HeaderText="SKU" FooterText ="Total : "
                            SortExpression="Description"   AllowFiltering ="true" ShowFilterIcon ="false" CurrentFilterFunction ="Contains" AutoPostBackOnFilter ="true" FilterControlToolTip ="Type the item code and press enter" >
                             <ItemStyle Wrap="False" HorizontalAlign ="Left"  />
                            <HeaderStyle Wrap="false" HorizontalAlign ="Left"  />

                       
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn  DataField="Item" HeaderText="Name"
                            SortExpression="Item" AllowFiltering ="true" ShowFilterIcon ="false" CurrentFilterFunction ="Contains" AutoPostBackOnFilter ="true" FilterControlToolTip ="Type the description and press enter">
                               <ItemStyle Wrap="False" HorizontalAlign ="Left"  />
                            <HeaderStyle Wrap="false" HorizontalAlign ="Left"  />
                            
                            
                        </telerik:GridBoundColumn>
                          <telerik:GridBoundColumn  DataField="UOM" HeaderText="UOM"
                            SortExpression="UOM" ShowFilterIcon ="false" AllowFiltering ="false"  >
                                <ItemStyle Wrap="False" HorizontalAlign ="Left"  />
                            <HeaderStyle Wrap="false" HorizontalAlign ="Left"  />
                             <FooterStyle HorizontalAlign="Left" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn DataField="Agency" HeaderText="Agency"
                            SortExpression="Agency" AllowFiltering ="true" ShowFilterIcon ="false" CurrentFilterFunction ="Contains" AutoPostBackOnFilter ="true" FilterControlToolTip ="Type the agency name and press enter">
                              <ItemStyle Wrap="False" HorizontalAlign ="Left"  />
                            <HeaderStyle Wrap="false" HorizontalAlign ="Left"  />
                          
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Brand_Code" HeaderText="Brand"
                            SortExpression="Brand_Code" AllowFiltering ="true" ShowFilterIcon ="false" CurrentFilterFunction ="Contains" AutoPostBackOnFilter ="true" FilterControlToolTip ="Type the brand name and press enter">
                            <ItemStyle Wrap="False" HorizontalAlign ="Right"  />
                            <HeaderStyle Wrap="true" HorizontalAlign ="Center"  />
                             <FooterStyle HorizontalAlign="Left" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn  Aggregate ="Sum"  DataField="TotValue" HeaderText="Value<i class='fa fa-info-circle'></i>"
                            SortExpression="TotValue"  UniqueName="Value" AllowFiltering ="false" HeaderTooltip="Total Sales value">
                             <ItemStyle Wrap="False" HorizontalAlign ="Right"  />
                            <HeaderStyle Wrap="true" HorizontalAlign ="Center"  />
                             <FooterStyle HorizontalAlign="right" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn  Aggregate ="Sum"  DataField="DispOrder" HeaderText="Volume<i class='fa fa-info-circle'></i>"
                            SortExpression="DispOrder" AllowFiltering ="false"  HeaderTooltip="Total Sales volume" DataFormatString="{0:0,000.0000}">
                              <ItemStyle Wrap="False" HorizontalAlign ="Right"  />
                            <HeaderStyle Wrap="true" HorizontalAlign ="Center"  />
                             <FooterStyle HorizontalAlign="right" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                        </telerik:GridBoundColumn>

                       <telerik:GridBoundColumn   DataField="MnOrder" HeaderText="Tot.Invoices<i class='fa fa-info-circle'></i>"
                            SortExpression="MnOrder" AllowFiltering ="false"  HeaderTooltip="Number of invoices in which the item was billed">
                              <ItemStyle Wrap="False" HorizontalAlign ="Right"  />
                            <HeaderStyle Wrap="true" HorizontalAlign ="Center"  />
                             <FooterStyle HorizontalAlign="Center" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                        </telerik:GridBoundColumn>
                           <telerik:GridBoundColumn  DataField="TotOutlets" HeaderText="Unique Outlets<i class='fa fa-info-circle'></i>"
                            SortExpression="TotOutlets" AllowFiltering ="false"   HeaderTooltip="Number of unique outlets that have billed the item">
                             <ItemStyle Wrap="False" HorizontalAlign ="Right"  />
                            <HeaderStyle Wrap="true" HorizontalAlign ="Center"  />
                             <FooterStyle HorizontalAlign="Center" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                        </telerik:GridBoundColumn>
                      
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SWXConnectionString %>"
            SelectCommand="Rep_BestSellers" SelectCommandType="StoredProcedure">

            <SelectParameters>
                <asp:ControlParameter ControlID="hfOrg" Name="OID" DefaultValue="0" />
                <asp:ControlParameter ControlID="hfSE" Name="VanList" DefaultValue="0" />
                <asp:ControlParameter ControlID="hfSMonth" Name="FMonth" DefaultValue="01-01-1900" />
                <asp:ControlParameter ControlID="hfEMonth" Name="TMonth" DefaultValue="01-01-1900" />
                <asp:Parameter Name="Mode" DefaultValue="ChartGrid" />
                   <asp:Parameter Name="ChartMode" DefaultValue="SKU" />
            </SelectParameters>

        </asp:SqlDataSource>


                    </div>
                                        
                                        
                                                       </telerik:RadPageView> 
  <telerik:RadPageView ID="RadPageView2" runat="server"  >
      
        <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanel1">
            <telerik:RadGrid ID="gvRep" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                PageSize="8" AllowPaging="false" runat="server" DataSourceID="sqlRep" ShowFooter ="true"  
                GroupingSettings-RetainGroupFootersVisibility="true" GroupingSettings-GroupContinuesFormatString="" GroupingSettings-GroupContinuedFormatString=""
                GridLines="None">

                <GroupingSettings CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" ShowGroupFooter="true" ShowFooter ="true"  Width="100%" GridLines="None" BorderColor="LightGray" DataSourceID="sqlRep"
                    PageSize="8">
                    
                    <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                    <GroupByExpressions>
                        
                        <telerik:GridGroupByExpression>
                            <SelectFields>
                                <telerik:GridGroupByField FieldName="Van" SortOrder="Ascending"></telerik:GridGroupByField>

                                

                            </SelectFields>
                            <GroupByFields>
                                <telerik:GridGroupByField FieldName="Van" SortOrder="Ascending"></telerik:GridGroupByField>
                               
                            </GroupByFields>
                        </telerik:GridGroupByExpression>

                    </GroupByExpressions>
                    <Columns>



                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Item_Code" HeaderText="SKU"
                            SortExpression="Item_Code"   FooterText="Total : " >
                            <ItemStyle Wrap="False" />

                            <FooterStyle HorizontalAlign="Left" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Description" HeaderText="Name"
                            SortExpression="Description">
                            <ItemStyle Wrap="False" />
                             <FooterStyle HorizontalAlign="Left" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                            
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Agency" HeaderText="Agency"
                            SortExpression="Agency">
                            <ItemStyle Wrap="False" />
                             <FooterStyle HorizontalAlign="Left" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Brand_Code" HeaderText="Brand"
                            SortExpression="Brand_Code">
                            <ItemStyle Wrap="False" />
                             <FooterStyle HorizontalAlign="Left" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Right" DataField="Value" HeaderText="Value<i class='fa fa-info-circle'></i>"
                            SortExpression="Value" Aggregate="Sum" UniqueName="Value" HeaderTooltip="Total Sales value">
                            <ItemStyle Wrap="False" HorizontalAlign="Right" />
                             <FooterStyle HorizontalAlign="right" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Right" DataField="Volume" HeaderText="Volume<i class='fa fa-info-circle'></i>"
                            SortExpression="Volume" Aggregate="Sum" HeaderTooltip="Total Sales volume" DataFormatString="{0:0,000.0000}">
                            <ItemStyle Wrap="False" HorizontalAlign="Right" />
                             <FooterStyle HorizontalAlign="right" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Center" DataField="UOM" HeaderText="UOM"
                            SortExpression="UOM">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" />
                             <FooterStyle HorizontalAlign="Left" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                        </telerik:GridBoundColumn>
                          <telerik:GridTemplateColumn uniqueName="VanCnt"  HeaderStyle-HorizontalAlign="Left"   Visible="false" 
                                                                         >
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTotVans" runat="server" Text='<%# Bind("TotVans")%>' ></asp:Label>
                                                               
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
        <asp:SqlDataSource ID="sqlRep" runat="server" ConnectionString="<%$ ConnectionStrings:SWXConnectionString %>"
            SelectCommand="Rep_BestSellers" SelectCommandType="StoredProcedure">

            <SelectParameters>
                <asp:ControlParameter ControlID="hfOrg" Name="OID" DefaultValue="0" />
                <asp:ControlParameter ControlID="hfSE" Name="VanList" DefaultValue="0" />
                <asp:ControlParameter ControlID="hfSMonth" Name="FMonth" DefaultValue="01-01-1900" />
                <asp:ControlParameter ControlID="hfEMonth" Name="TMonth" DefaultValue="01-01-1900" />
                <asp:Parameter Name="Mode" DefaultValue="Grid" />
                   <asp:Parameter Name="ChartMode" DefaultValue="Van" />
            </SelectParameters>

        </asp:SqlDataSource>
    
      </telerik:RadPageView> 
                                                </telerik:RadMultiPage> 
              </div> 
    </telerik:RadAjaxPanel>

    <div style="display: none">
        <asp:Button CssClass="btn btn-primary" ID="BtnExportExcel" runat="server" Text="Export" />
        <asp:Button CssClass="btn btn-primary" ID="BtnExportPDF" runat="server" Text="Export" />
    </div>

    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="10">
        <progresstemplate>
     <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
   
         <img  src="../assets/img/ajax-loader.gif" alt="Processing..." style="vertical-align:middle;" />            
           <span style="font-size:12px; font-weight:bold; color:#3399ff;">Processing... </span>
       </asp:Panel>
         
         
    </progresstemplate>
    </asp:UpdateProgress>

     <asp:Panel ID="Panel1" CssClass="overlay" runat="server" style="display:none">
                                        <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                                        <span style="font-size: 12px; color: #3399ff;">Processing... </span>
     </asp:Panel>

</asp:Content>
