<%@ Page Title="Dash Board Open Van Requisition" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DashboardSection.Master" CodeBehind="DashboardOpenVanRequisition.aspx.vb" Inherits="SalesWorx_BO.DashboardOpenVanRequisition" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:GridView  width="100%" ID="GVVanRequistion" runat="server" EmptyDataText="No requisition to display" EmptyDataRowStyle-Font-Bold="true" 
  CssClass="txtSM" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" PageSize="25" CellPadding="6">
  <RowStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" CssClass="tdstyle" Height="12px" Wrap="True" />
  <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
  <EmptyDataRowStyle Font-Bold="True" />
  <Columns>    
     <asp:BoundField DataField="Dest_Org_ID" ItemStyle-HorizontalAlign="center" 
          HeaderText="Van" SortExpression="Dest_Org_ID">
        <ItemStyle HorizontalAlign="Center" />
     </asp:BoundField>
      <asp:BoundField DataField="Request_Date" HeaderText="Request Date"  
          SortExpression="Request_Date" NullDisplayText="N/A" 
          DataFormatString="{0:dd/MM/yyyy}" >
      <ItemStyle Wrap="False" />
    </asp:BoundField>                     
     <asp:TemplateField >
     <ItemTemplate>
        <asp:HyperLink  runat="server" ID="hyp" Text="Details" NavigateUrl='<%# DataBinder.Eval(Container.DataItem, "StockRequisition_ID","VanRequisitionDetails.aspx?id={0}")%>' Target="_top"></asp:HyperLink>
        </ItemTemplate>
    </asp:TemplateField>
  </Columns>
   <PagerStyle CssClass="pagernumberlink" />
       <HeaderStyle BackColor="Silver" BorderColor="Silver" BorderStyle="Solid" 
           BorderWidth="1px" CssClass="headerstyle" />
</asp:GridView>
</asp:Content>
