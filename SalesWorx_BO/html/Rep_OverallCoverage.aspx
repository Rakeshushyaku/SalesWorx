<%@ Page Title="Overall Coverage Report" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="Rep_OverallCoverage.aspx.vb" Inherits="SalesWorx_BO.Rep_OverallCoverage" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    
    <style type="text/css">
        div.RadGrid .rgPager .rgAdvPart {
            display: none;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <script src="../scripts/kendo.all.min.js"></script><script src="../scripts/kendo.dataviz.min.js"></script><link href="../styles/kendo.dataviz.black.min.css" rel="stylesheet" /><style type="text/css">
        div[id*="ReportDiv"] {
            overflow: hidden !important;
            WIDTH: 100%;
            direction: ltr;
        }
        #ctl00_MainContent_gvRep_OT > tbody > tr:first-child {
            height:70px !important;
        }
    </style><script>

                function clickExportBiffExcel() {

                    $("#MainContent_BtnExportBiffExcel").click()
                    return false

                }

                function onDataBound(e) {
                    $("#MainContent_Panel1").hide()

                }

        function createChart3() {
            var res
            var param1 = document.getElementById('<%= hfOrg.ClientID%>').value;
            var param2 = document.getElementById('<%= HSID.ClientID%>').value;
            var param3 = document.getElementById('<%= hfSMonth.ClientID%>').value;
            var param4 = document.getElementById('<%= hfEMonth.ClientID%>').value;

            $("#MainContent_Panel1").show()
            var dataSource = {
                type: "json",
                transport: {
                    read: {
                        url: "Chart.asmx/OverallCoverageCustomer", //specify the URL which should create new records. This is the Create method of the Products.asmx service.
                        contentType: "application/json; charset=utf-8", // tells the web service to serialize JSON
                        type: "POST", //use HTTP POST request as the default GET is not allowed for ASMX
                        dataType: "json"
                    }
                      ,
                    parameterMap: function (data, operation) {

                        var output = null;
                        switch (operation) {

                            case "read":
                                output = '{ param1: ' + JSON.stringify(param1) + ',param2: ' + JSON.stringify(param2) + ',param3: ' + JSON.stringify(param3) + ',param4: ' + JSON.stringify(param4) + ' }';
                                break;

                        }

                        return output;
                        return kendo.stringify(data);
                        // stringify the data send back to the server
                    }
                },

                schema: {
                    data: "d", // ASMX services return JSON in the following format { "d": <result> }. Specify how to get the result.
                    model: { // define the model of the data source.
                        fields: {

                            Description: { type: "string" },
                            Amount: { type: "number" },
                            Description1: { type: "string" },
                            Count: { type: "number" },
                        }
                    }
                },

            }

            $("#chart3").kendoChart({
                theme: "flat",
                dataSource: dataSource,
                dataBound: onDataBound,
                title: {
                    text: "",
                    color: "white",
                    font: "12px Segoe UI"

                },

                legend: {
                    position: "top"
                },


                chartArea: {
                    background: "White"
                },
                categoryAxis:
                {
                    labels: { font: "bold 12px Segoe UI", color: "#10C4B2", rotation: -90 },

                    minorGridLines: { visible: false },
                    majorGridLines: {
                        visible: false
                    }
                },

                valueAxis: [{
                    name: "Sales",
                    color: "#14b4fc",


                    title: { text: "No of Customers", font: "bold 12px Segoe UI", color: "#14b4fc" },
                    labels: {
                        font: "bold 12px Segoe UI", color: "#14b4fc"
                    }
                },


                                       {
                                           name: "Description1",
                                           min: 0,
                                           max: 100,
                                           color: "Crimson",

                                           title: { text: "Productivity %", font: "bold 12px Segoe UI", color: "Crimson" },
                                           labels: {
                                               font: "bold 12px Segoe UI", color: "Crimson"
                                           }
                                       },

                                       {
                                           name: "Description2",
                                           min: 0,
                                           max: 100,
                                           color: "#8e5e0c",

                                           title: { text: "Adherence %", font: "bold 12px Segoe UI", color: "#8e5e0c" },
                                           labels: {
                                               font: "bold 12px Segoe UI", color: "#8e5e0c"
                                           }
                                       }
                ],

                seriesColors: ["#52D726", "#1BAA2F", "#2DCB75", "#FF9933", "#FFFF99", "#FFCC99", "#9999FF"],
                series: [{
                    type: "column",
                    name: "Planned Customers",
                    field: "Amount",
                    categoryField: "Description",
                    tooltip: {
                        visible: true,
                        template: "Van: <b>${category}</b></br>Planned Customers: ${value}",
                        font: "12px Segoe UI",
                        color: "white",
                        background: "black"
                    },

                },

                 {
                     type: "column",
                     name: "Coverage",
                     field: "ReturnAmount",
                     categoryField: "Description",
                     tooltip: {
                         visible: true,
                         template: "Van: <b>${category}</b></br>Coverage : ${value}",
                         font: "12px Segoe UI",
                         color: "white",
                         background: "black"
                     },

                 },

                 {
                     type: "column",
                     name: "Billed Customers",
                     field: "Amount1",
                     categoryField: "Description",
                     tooltip: {
                         visible: true,
                         template: "Van: <b>${category}</b></br>Billed Customers : ${value}",
                         font: "12px Segoe UI",
                         color: "white",
                         background: "black"
                     },

                 },

                 {
                     field: "Count",
                     type: "line",
                     style: "smooth",
                     width: "1px",
                     axis: "Description1",
                     color: "Crimson",
                     tooltip: {
                         visible: true,
                         template: "Van : #= dataItem.Description1#  <br> Productivity %  : #=dataItem.Count# ",
                         font: "12px Segoe UI",
                         color: "white",
                         background: "black"
                     },
                     labels: {
                         font: "bold 12px  Segoe UI", color: "Crimson"
                     }

                 },
                 {
                     field: "Count1",
                     type: "line",
                     style: "smooth",
                     width: "1px",
                     axis: "Description2",
                     color: "#8e5e0c",
                     tooltip: {
                         visible: true,
                         template: "Van : #= dataItem.Description2#  <br> Adherence %  : #=dataItem.Count1# ",
                         font: "12px Segoe UI",
                         color: "white",
                         background: "black"
                     },
                     labels: {
                         font: "bold 12px  Segoe UI", color: "#8e5e0c"
                     }

                 }
                ],



            });






        }





        function createChart4() {
            var res
            var param1 = document.getElementById('<%= hfOrg.ClientID%>').value;
            var param2 = document.getElementById('<%= HSID.ClientID%>').value;
            var param3 = document.getElementById('<%= hfSMonth.ClientID%>').value;
            var param4 = document.getElementById('<%= hfEMonth.ClientID%>').value;

            $("#MainContent_Panel1").show()
            var dataSource = {
                type: "json",
                transport: {
                    read: {
                        url: "Chart.asmx/OverallCoverageVisits", //specify the URL which should create new records. This is the Create method of the Products.asmx service.
                        contentType: "application/json; charset=utf-8", // tells the web service to serialize JSON
                        type: "POST", //use HTTP POST request as the default GET is not allowed for ASMX
                        dataType: "json"
                    }
                      ,
                    parameterMap: function (data, operation) {

                        var output = null;
                        switch (operation) {

                            case "read":
                                output = '{ param1: ' + JSON.stringify(param1) + ',param2: ' + JSON.stringify(param2) + ',param3: ' + JSON.stringify(param3) + ',param4: ' + JSON.stringify(param4) + ' }';
                                break;

                        }

                        return output;
                        return kendo.stringify(data);
                        // stringify the data send back to the server
                    }
                },

                schema: {
                    data: "d", // ASMX services return JSON in the following format { "d": <result> }. Specify how to get the result.
                    model: { // define the model of the data source.
                        fields: {

                            Description: { type: "string" },
                            Amount: { type: "number" },
                            Description1: { type: "string" },
                            Count: { type: "number" },
                        }
                    }
                },

            }

            $("#chart4").kendoChart({
                theme: "flat",
                dataSource: dataSource,
                dataBound: onDataBound,
                title: {
                    text: "",
                    color: "white",
                    font: "12px Segoe UI"

                },

                legend: {
                    position: "top"
                },


                chartArea: {
                    background: "White"
                },
                categoryAxis:
                {
                    labels: { font: "bold 12px Segoe UI", color: "#10C4B2", rotation: -90 },

                    minorGridLines: { visible: false },
                    majorGridLines: {
                        visible: false
                    }
                },

                valueAxis: [{
                    name: "Sales",
                    color: "#14b4fc",


                    title: { text: "No of Visits", font: "bold 12px Segoe UI", color: "#14b4fc" },
                    labels: {
                        font: "bold 12px Segoe UI", color: "#14b4fc"
                    }
                },


                                       {
                                           name: "Description1",
                                           color: "Crimson",
                                           min: 0,
                                           max: 100,
                                           title: { text: "Productivity %", font: "bold 12px Segoe UI", color: "Crimson" },
                                           labels: {
                                               font: "bold 12px Segoe UI", color: "Crimson"
                                           }
                                       }
                ],

                seriesColors: ["#00CCFF", "#267AB8", "#FF99CC", "#FF9933", "#FFFF99", "#FFCC99", "#9999FF"],
                series: [
                 {
                     type: "column",
                     name: "Planned Calls",
                     field: "Amount",
                     categoryField: "Description",
                     tooltip: {
                         visible: true,
                         template: "Van: <b>${category}</b></br>Planned Calls: ${value}",
                         font: "12px Segoe UI",
                         color: "white",
                         background: "black"
                     },

                 }
                 ,

                 {
                     type: "column",
                     name: "Actual Calls",
                     field: "ReturnAmount",
                     categoryField: "Description",
                     tooltip: {
                         visible: true,
                         template: "Van: <b>${category}</b></br>Actual Calls : ${value}",
                         font: "12px Segoe UI",
                         color: "white",
                         background: "black"
                     },

                 }
                 ,

                 {
                     field: "Count",
                     type: "line",
                     style: "smooth",
                     width: "1px",
                     axis: "Description1",
                     color: "Crimson",
                     tooltip: {
                         visible: true,
                         template: "Van : #= dataItem.Description1#  <br> Productivity %  : #=dataItem.Count# ",
                         font: "12px Segoe UI",
                         color: "white",
                         background: "black"
                     },
                     labels: {
                         font: "bold 12px  Segoe UI", color: "Crimson"
                     }

                 }
                ],



            });






        }


        function alertCallBackFn(arg) {

        }

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

        function pageLoad(sender, args) {

            $('[data-toggle="tooltip"]').tooltip();
        }

        function UpdateHeader() {
            var dist
            dist = document.getElementById('<%= HUseDistributionIncall.ClientID%>').value
            createChart3();
            createChart4()
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_10").html("Assigned <i class='fa fa-info-circle' data-container='body' data-toggle='tooltip' title='Customers assigned to the van'></i>")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_10").css("background-color", "#97c95d")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_10").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_10").css("background-image", "none")

            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_11").html("Planned <i class='fa fa-info-circle' data-container='body' data-toggle='tooltip' title='Unique Customers included in route plan'></i>")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_11").css("background-color", "#97c95d")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_11").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_11").css("background-image", "none")

            if(dist=='Y')
                $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_12").html("Coverage <i class='fa fa-info-circle' data-container='body' data-toggle='tooltip' title='Unique customers visited and performed Distribution Check'></i>")
            else
                $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_12").html("Coverage <i class='fa fa-info-circle' data-container='body' data-toggle='tooltip' title='Unique customers visited'></i>")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_12").css("background-color", "#97c95d")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_12").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_12").css("background-image", "none")

            //$("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_12").html("Missed")
            //$("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_12").css("background-color", "#97c95d")
            //$("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_12").css("color", "#FFFFFF")
            //$("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_12").css("background-image", "none")

            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_13").html("Out of Route <i class='fa fa-info-circle' data-container='body' data-toggle='tooltip' title='Unique customers visited as Out of the route'></i>")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_13").css("background-color", "#97c95d")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_13").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_13").css("background-image", "none")

            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_14").html("Billed <i class='fa fa-info-circle' data-container='body' data-toggle='tooltip' title='Unique customers who had billed'></i>")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_14").css("background-color", "#97c95d")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_14").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_14").css("background-image", "none")

            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_15").html("0 Billed <i class='fa fa-info-circle' data-container='body' data-toggle='tooltip' title='Customers who have not billed'></i>")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_15").css("background-color", "#97c95d")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_15").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_15").css("background-image", "none")

            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_16").html("Productivity % <i class='fa fa-info-circle' data-container='body' data-toggle='tooltip' title='Customers billed Vs Covered'></i>")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_16").css("background-color", "#97c95d")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_16").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_16").css("background-image", "none")

            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_17").html("Adherence % <i class='fa fa-info-circle' data-container='body' data-toggle='tooltip' title='Adherence to Route Plan'></i>")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_17").css("background-color", "#97c95d")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_17").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_17").css("background-image", "none")
            // $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_14").hide()
            //$("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_15").html("Out Of Route")
            //$("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_16").html("Success Visits")
           
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_18").html("Planned <i class='fa fa-info-circle' data-container='body' data-toggle='tooltip' title='Visits planned'></i>")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_18").css("background-color", "#14b4fc")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_18").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_18").css("background-image", "none")

            if (dist == 'Y')
             $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_19").html("Actual <i class='fa fa-info-circle' data-container='body' data-toggle='tooltip' title='Visits in which Distribution check was performed.'></i>")
            else
             $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_19").html("Actual <i class='fa fa-info-circle' data-container='body' data-toggle='tooltip' title='Visits done.'></i>")

            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_19").css("background-color", "#14b4fc")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_19").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_19").css("background-image", "none")

            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_110").html("Out of Route <i class='fa fa-info-circle' data-container='body' data-toggle='tooltip' title='Visits that were made out of the route'></i>")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_110").css("background-color", "#14b4fc")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_110").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_110").css("background-image", "none")

          

            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_111").html("Productive Calls <i class='fa fa-info-circle' data-container='body' data-toggle='tooltip' title='Visits in which an invoice was generated.'></i>")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_111").css("background-color", "#14b4fc")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_111").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_111").css("background-image", "none")

            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_112").html("Missed")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_112").css("background-color", "#14b4fc")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_112").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_112").css("background-image", "none")

            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_112").hide()
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_113").hide()
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_114").hide()
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_115").hide()

        }

        function openpopup(id) {

            var param1 = document.getElementById('<%= hfOrg.ClientID%>').value;
            var param3 = document.getElementById('<%= hfSMonth.ClientID%>').value;
            var param4 = document.getElementById('<%= hfEMonth.ClientID%>').value;
            var cid
            cid=id.split("__")
            var URL
            var name = $("#" + id).html()
            
            URL = 'Rep_OverallCoverageDetailed.aspx?OrgID=' + param1 + '&FromDt=' + param3 + '&ToDt=' + param4 + '&SID=' + cid[1] + "&SPName=" + name;
            var oWnd = radopen(URL, null);
            oWnd.SetSize(930, 630);
            oWnd.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move + Telerik.Web.UI.WindowBehaviors.Close); //Close:4
            oWnd.SetModal(true);
            oWnd.Center;
            oWnd.set_visibleStatusbar(false)

            return false
             
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <h4>Overall Coverage Report </h4>
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
           
        <asp:HiddenField ID="HSID" runat="server" />
        <asp:HiddenField ID="hfSMonth" runat="server" />
        <asp:HiddenField ID="hfEMonth" runat="server" />
        <asp:HiddenField ID="hfOrg" runat="server" />
       <asp:HiddenField ID="HUID" runat="server" />

       

                 <telerik:RadPanelBar runat="server" ID="rpbFilter" Skin="Simple" Width="100%"
                            ExpandMode="MultipleExpandedItems">
                            <Items>
                                <telerik:RadPanelItem Expanded="True" Text=" ">

                                    <ContentTemplate>

                                        <div class="row">
                                            <div class="col-sm-10">
                                                <div class="row">
                                                   
                                                     <div class="col-sm-2" runat="server" id="dvCountry">
                                            <div class="form-group">
                                                <label>Country</label>
                                                <telerik:RadComboBox Skin="Simple" EmptyMessage="Select Country" ID="ddlCountry" Width ="100%" runat="server" DataTextField="Country" DataValueField="MAS_ORG_ID"  AutoPostBack="true" >
                                            </telerik:RadComboBox>
                                               
                                            </div>
                                        </div>
                                                     <div class="col-sm-5">
                                            <div class="form-group">
                                                <label>Organization<em><span>&nbsp;</span>*</em></label>
                                                <telerik:RadComboBox Skin="Simple"  CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Filter="Contains" ID="ddlOrganization" Width ="100%" runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="true">
                                        </telerik:RadComboBox>
                                            </div>
                                          </div>
          
                                                     <div class="col-sm-5">
                                            <div class="form-group">
                                                <label>Van/FSR</label>
                                                   <telerik:RadComboBox Skin="Simple" CheckBoxes="true" EmptyMessage="Select Van/FSR" EnableCheckAllItemsCheckBox="true" ID="ddl_Van" Width ="100%" runat="server" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID" Filter="Contains">
                                        </telerik:RadComboBox >
                                                 </div>
                                             </div>
                                                    </div>
                                              
                                                  <div class="row">
                                                     <div class="col-sm-3">
                                                       <div class="form-group">
                                                           <label>From Date  </label>
                                                            <telerik:RadDatePicker ID="txtFromDate"  Width="100%"  runat="server">
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
                                                           <label>To Date  </label>
                                                             <telerik:RadDatePicker ID="txtToDate"  Width="100%"  runat="server">
                                                        <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy">
                                                        </DateInput>
                                                        <Calendar ID="Calendar1" runat="server">
                                                            <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                        </Calendar>
                                                    </telerik:RadDatePicker>

                                                        </div>
                                                      </div>

                                                      

                                                  </div>


                                            </div>
                                                    
                                                    <div class="col-sm-2">
                                                 <div class="form-group">
                                                    <label>&nbsp;</label>
                                                <asp:Button  CssClass ="btn btn-sm btn-block btn-primary"  ID="SearchBtn" runat="server" Text="Search" />
                                                     <asp:Button  CssClass ="btn btn-sm btn-block btn-default"  ID="ClearBtn" runat="server" Text="Clear"  />
                                                    </div>
                                                <div class="form-group fontbig text-center">
                                                    <label>&nbsp;</label>
                                                <asp:HyperLink href="" CssClass=""  ID="BtnExportDummyExcel" runat="server" OnClick="return clickExportExcel()"><i class="fa fa-file-excel-o text-success"></i></asp:HyperLink>
                                                <asp:HyperLink href=""  CssClass =""  ID="BtnExportDummyPDF" runat="server" OnClick="return clickExportPDF()"><i class="fa fa-file-pdf-o text-danger"></i></asp:HyperLink>
                                                
                                            </div>
                                            </div>
                                        </div>

                                         </ContentTemplate>
                                        </telerik:RadPanelItem> 
                                     </Items> 
                    </telerik:RadPanelBar> 
       <div id="RepDiv" runat="server" visible="false" >
                        <div id="Args" runat="server" visible="false">
        <div id="popoverblkouter">
            Hover on icon to view search criteria <i class="fa fa-info-circle">
            <span class="popoverblk">
              <p><strong>Organisation: </strong><asp:Label ID="lbl_org" runat="server" Text=""></asp:Label></p>
              <p><strong>Van: </strong> <asp:Label ID="lbl_van" runat="server" Text=""></asp:Label></p>
              <p><strong>From Date: </strong><asp:Label ID="lbl_from" runat="server" Text=""></asp:Label></p>
              <p><strong>To Date: </strong><asp:Label ID="lbl_To" runat="server" Text=""></asp:Label></p>  
               
            </span>
            </i>      
        </div>
    </div>

                            
            <div id="summary" runat="server" class="row"></div>
           <div class="table-responsive" style="position:relative;">
             <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanel1">
                              
                                  <div style="position:absolute;padding:5px;top:0;left:0;">
                                     <asp:ImageButton ID="img_export" runat="server" ImageUrl ="../assets/img/export-excel.png"  Visible="false"  OnClientClick="clickExportBiffExcel()"></asp:ImageButton>
                            </div>
                                 </telerik:RadAjaxPanel>
                                 <telerik:RadPivotGrid  RenderMode="Lightweight" AllowPaging="true"  PageSize="10" EnableViewState ="true" 
                                                    ID="gvRep" runat="server"  
                                                    ShowFilterHeaderZone="false" AllowFiltering="false" ShowColumnHeaderZone="false" ShowRowHeaderZone="false" ShowDataHeaderZone="false" cssClass="no-wrap" 
                                                     >
                                                    <TotalsSettings GrandTotalsVisibility="None"  />
                                                                                                        
                                                    <Fields>
                                                       
                                                          <telerik:PivotGridRowField DataField="Description" ZoneIndex="0"  UniqueName="Description" >
                                                                <CellTemplate>
                                                                     <asp:LinkButton ForeColor="SteelBlue" Font-Underline="true"  ID="Van" runat="server" Text='<%# Container.DataItem%>' OnClientClick="return openpopup(this.id)"></asp:LinkButton>
                                                                </CellTemplate>  
                                                                </telerik:PivotGridRowField>
                                                        <telerik:PivotGridColumnField DataField="Type" SortOrder="None">
                 
                                                        </telerik:PivotGridColumnField>
                                                        
                                                         <telerik:PivotGridAggregateField DataField="C1" SortOrder="None" >  
                                                                     
                                                        </telerik:PivotGridAggregateField>
                                                        <telerik:PivotGridAggregateField DataField="C2" SortOrder="None">                 
                                                        </telerik:PivotGridAggregateField>
                                                        <telerik:PivotGridAggregateField  DataField="C3" SortOrder="None">  
                                                                    
                                                        </telerik:PivotGridAggregateField>
                                                         <telerik:PivotGridAggregateField  DataField="C4" SortOrder="None">  
                                                                    
                                                        </telerik:PivotGridAggregateField>
                                                        <telerik:PivotGridAggregateField  DataField="C5" SortOrder="None">  
                                                                    
                                                        </telerik:PivotGridAggregateField>
                                                        <telerik:PivotGridAggregateField  DataField="C6" SortOrder="None">  
                                                                    
                                                        </telerik:PivotGridAggregateField>
                                                        <telerik:PivotGridAggregateField  DataField="C7" SortOrder="None">  
                                                                    
                                                        </telerik:PivotGridAggregateField>
                                                        <telerik:PivotGridAggregateField  DataField="C8" SortOrder="None">  
                                                                    
                                                        </telerik:PivotGridAggregateField>

                                                        <%-- <telerik:PivotGridAggregateField  DataField="C9" SortOrder="None">  
                                                                    
                                                        </telerik:PivotGridAggregateField>--%>
                                                       
                                                    </Fields>
                                                    <PagerStyle PageSizeControlType="None" Mode="NumericPages" />
                                                </telerik:RadPivotGrid>

                              </div>
           <p><br /></p>
            <h3>Customers</h3>
            <div class="overflowx">
            <div class="chart-wrapper padding0" style="height:400px" id="Chartwrapper" runat="server" >
                                                <div id="chart3" style="height:100%" class="margin-14"> </div>
                                                </div>
                 </div>
           <hr />
           <h3>Calls</h3>
            <div class="overflowx">
            <div class="chart-wrapper padding0" style="height:400px" id="Chartwrapper1" runat="server" >
                                                <div id="chart4" style="height:100%" class="margin-14"> </div>
                                                </div>
                 </div>
        </div>
              

    </telerik:RadAjaxPanel>
    <asp:HiddenField ID="HUseDistributionIncall" runat="server" Value="N" />
    <div style="display: none">
        <asp:Button CssClass="btn btn-primary" ID="BtnExportExcel" runat="server" Text="Export" />
        <asp:Button CssClass="btn btn-primary" ID="BtnExportPDF" runat="server" Text="Export" />
         <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportBiffExcel" runat="server" Text="Export" />
    </div>
   <asp:UpdateProgress ID="UpdateProgress2" DisplayAfter="10"
        runat="server">
        <progresstemplate>
                                    <asp:Panel ID="Panel14" CssClass="overlay" runat="server">
                                        <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                                        <span style="font-size: 12px; color: #3399ff;">Processing... </span>
                                    </asp:Panel>
                                </progresstemplate>
    </asp:UpdateProgress>

     <asp:Panel ID="Panel1" CssClass="overlay" runat="server" style="display:none">
                                        <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                                        <span style="font-size: 12px; color: #3399ff;">Processing... </span>
     </asp:Panel>
</asp:Content>



