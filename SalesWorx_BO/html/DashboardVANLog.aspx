<%@ Page Title="Van Log" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DashboardSection.Master" CodeBehind="DashboardVANLog.aspx.vb" Inherits="SalesWorx_BO.DashboardVANLog" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:GridView  width="100%" ID="GVVanLog" runat="server" EmptyDataText="No log to display" EmptyDataRowStyle-Font-Bold="true" 
   AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" 
    PageSize="9" CellPadding="0" CellSpacing="0" CssClass="tablecellalign">

  <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
  <EmptyDataRowStyle Font-Bold="True" />
 <Columns>    
     <asp:BoundField DataField="SalesRep_Name" ItemStyle-HorizontalAlign="center" 
          HeaderText="Van" SortExpression="SalesRep_Name">
        <ItemStyle HorizontalAlign="Center" />
     </asp:BoundField>
     <asp:BoundField DataField="TSales" ItemStyle-HorizontalAlign="center" 
          HeaderText="Total Sales" SortExpression="TSales">
        <ItemStyle HorizontalAlign="Center" />
     </asp:BoundField>
     <asp:BoundField DataField="TCreditNote" ItemStyle-HorizontalAlign="center" 
          HeaderText="Total Credit Note" SortExpression="TCreditNote">
        <ItemStyle HorizontalAlign="Center" />
     </asp:BoundField>
     <asp:BoundField DataField="Payment" ItemStyle-HorizontalAlign="center" 
          HeaderText="Payment" SortExpression="Payment">
        <ItemStyle HorizontalAlign="Center" />
     </asp:BoundField>     
      <asp:TemplateField HeaderText="Productive Calls" SortExpression="ProdCalls">
     <ItemTemplate>
        <asp:HyperLink  runat="server" ID="hyp" Text='<%# Bind("ProdCalls") %>' NavigateUrl='<%# DataBinder.Eval(Container.DataItem, "SalesRep_ID","RepCustomerVisits.aspx?type=PC&id={0}")%>' Target="_top"></asp:HyperLink>
        </ItemTemplate>
        <ItemStyle HorizontalAlign="Center" />
    </asp:TemplateField>
    <asp:TemplateField HeaderText="Total Calls" SortExpression="TotCalls">
     <ItemTemplate>
        <asp:HyperLink  runat="server" ID="hyp" Text='<%# Bind("TotCalls") %>' NavigateUrl='<%# DataBinder.Eval(Container.DataItem, "SalesRep_ID","RepCustomerVisits.aspx?type=TC&id={0}")%>' Target="_top"></asp:HyperLink>
    </ItemTemplate>
    <ItemStyle HorizontalAlign="Center" />
    </asp:TemplateField>
  </Columns>
   <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
</asp:GridView></asp:Content>
