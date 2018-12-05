<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DashboardSection.Master" CodeBehind="DashBoardRouteCoverage.aspx.vb" Inherits="SalesWorx_BO.DashBoardRouteCoverage" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style scoped>
                #gauge-container {
                    background: url("../images/gauge-container-partial-350.png") no-repeat center 5px ;
                    height:250px;
                    margin: 0;
                }

                #gauge {
                    width: 220px;
                    height: 200;
                    margin: 0 auto;
                }

                #gauge-container .k-slider {
                    margin-top: -11px;
                    width: 140px;
                }

            </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
 
           <script src="../jsscript/jquery.min.js" ></script>
            <script src="../jsscript/angular.js" ></script>
            <script src="../jsscript/kendo.all.min.js" ></script>
<script src="../scripts/kendo.all.min.js"></script>
<script src="../scripts/kendo.dataviz.min.js"></script>
<link href="../styles/kendo.dataviz.black.min.css" rel="stylesheet" />
<link href="../styles/kendo.common.min.css" rel="stylesheet" />
<script>
    function createGauge() {
    
        $("#gauge").kendoRadialGauge({
        
        
            pointer: {
            value:document.getElementById('<%= PointVal.ClientID %>').value 
            },

            scale: {
                minorUnit: 5,
                startAngle: -30,
                endAngle: 210,
                max: document.getElementById('<%= Maxval.ClientID %>').value 
            },
            labels: {
                font: "9px Calibri,Helvetica,sans-serif",color:"white"
            }
        });
    }

    $(document).ready(function() {
        createGauge();

        

        
    });
            </script>

<div id="example" class="k-content">
            <div id="gauge-container">
                <div id="gauge"></div>

            </div>
    <asp:HiddenField ID="Maxval" runat="server" />   
    <asp:HiddenField ID="PointVal" runat="server" />   
</asp:Content>
