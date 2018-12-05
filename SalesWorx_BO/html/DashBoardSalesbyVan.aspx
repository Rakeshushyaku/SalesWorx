<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DashboardSection.Master" CodeBehind="DashBoardSalesbyVan.aspx.vb" Inherits="SalesWorx_BO.DashBoardSalesbyVan" %>

    
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style scoped>
        .chart-wrapper, .chart-wrapper .k-chart {
       

        }
             .rgExpand, .rgCollapse
  {
    display: none;
  }
  .RadGrid .rgPagerLabel, .RadGrid .rgPager .RadComboBox, .RadGrid .rgPager .RadInput {
margin: 0 4px 0 0;
vertical-align: middle;

visibility: hidden;
}


    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
 
 <script src="../jsscript/jquery.min.js" ></script>
<script src="../scripts/kendo.all.min.js"></script>
<script src="../scripts/kendo.dataviz.min.js"></script>
<link href="../styles/kendo.dataviz.black.min.css" rel="stylesheet" />

  <script type="text/javascript">

function createChart3() {
          var param = document.getElementById('<%= UId.ClientID %>').value;
          
          $("#chart3").kendoChart({

              dataSource: {
                  type: "json",
                  transport: {
                      read: {
                      url: "Chart.asmx/GetSalesbyVan", //specify the URL which should create new records. This is the Create method of the Products.asmx service.
                          contentType: "application/json; charset=utf-8", // tells the web service to serialize JSON
                          type: "POST", //use HTTP POST request as the default GET is not allowed for ASMX
                          dataType: "json"
                      }
                        ,
                      parameterMap: function(data, operation) {

                          var output = null;
                          switch (operation) {

                              case "read":
                                  output = '{ param: ' + JSON.stringify(param) + ' }';
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
                              Amount: { type: "number" }

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
                  color: "white",
                  font: "14px Calibri"

              },
              
              
              seriesDefaults: {
                  type: "column", labels: {
                      visible: false ,
                      format: "{0:N0}",

                      font: "12px Calibri"
                  }

              },
chartArea: {
                    background: "White"
                    
                },
              categoryAxis:
                {
                    labels: { font: "12px Calibri",rotation: -55,color:"black"},
                    
                    minorGridLines: {visible: false},
                            majorGridLines: {
            visible: false        }
                },
                
                
              valueAxis:
                {
                   
                    labels: { font: "12px Calibri",color:"black" },majorGridLines: {            visible: false        }
                },
             seriesColors: ["#6699FF","#CCCC66",  "#33CCFF", "#FF99CC", "#FF9933", "#FFFF99", "#FFCC99", "#9999FF"],
              series: [{
              field: "Amount",
                  categoryField: "Description",
                  

}],
                  tooltip: {
                      visible: true,
                      format: "{0:N0}",
                      font: "12px Calibri",
                      color: "white",
                      background: "black"
                  }
              });
          }
          
          $(document).ready(function() {

              createChart3();

          });

    </script>   

    <asp:HiddenField ID="UId" runat="server" />
    <div class="chart-wrapper">
        <div id="chart3"   style ="width:100%;height:215px;">
        </div>
        </div> 
</asp:Content>
