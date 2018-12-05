<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WebForm1.aspx.vb" Inherits="SalesWorx_BO.WebForm1" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"  AsyncPostBackTimeOut="36000">
             </asp:ScriptManager>
    <div>
      <asp:TextBox ID="txt" runat="server"></asp:TextBox>
        <asp:Button ID="test" Text="test"  runat="server"/>
    </div>

         <telerik:RadHtmlChart runat="server" ID="RadHtmlChart1" Width="800px" Height="500px">
            <PlotArea>
                <Series>
                    <telerik:LineSeries Name="Call Productivity %"  DataFieldY="PercentCP">
                        <TooltipsAppearance Color="White" ></TooltipsAppearance>
                        
                    </telerik:LineSeries>
                    <telerik:LineSeries Name="Growth Over Last Year %" DataFieldY="PercentGR">
                        <TooltipsAppearance Color="White" ></TooltipsAppearance>
                         
                    </telerik:LineSeries>
                    <telerik:LineSeries Name="JP adherence %"  DataFieldY="PercentJP">
                        <TooltipsAppearance Color="White"></TooltipsAppearance>
                        
                    </telerik:LineSeries>
                    <telerik:LineSeries Name="Outlet Productivity %"  DataFieldY="PercentOP">
                        <TooltipsAppearance Color="White" ></TooltipsAppearance>
                       
                    </telerik:LineSeries>
                </Series>
                <XAxis  DataLabelsField="Month">
                    <TitleAppearance Text="Month" />
                </XAxis>
                <YAxis MaxValue="100" MinValue="0">
                    <LabelsAppearance DataFormatString="{0}%" />
                    <TitleAppearance Text="%" />
                </YAxis>
            </PlotArea>
            
        </telerik:RadHtmlChart>

        

    </form>
</body>
</html>
