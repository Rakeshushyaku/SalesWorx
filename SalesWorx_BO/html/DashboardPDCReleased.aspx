<%@ Page Title="PDC Released" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DashboardSection.Master" CodeBehind="DashboardPDCReleased.aspx.vb" Inherits="SalesWorx_BO.DashboardPDCReleased" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:GridView  width="100%" ID="GVCollectionList" runat="server" DataKeyNames="Collection_ID"
  EmptyDataText="No collections to display" EmptyDataRowStyle-Font-Bold="true" 
  AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" 
  PageSize="25" CellPadding="0" CellSpacing="0" CssClass="tablecellalign">
  
  <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
    <EmptyDataRowStyle Font-Bold="True" />
  <Columns>    
      <asp:TemplateField HeaderText="PDC #">         
          <ItemTemplate>
              <asp:Label ID="lblNo" runat="server" Text="<%# Container.DataItemIndex + 1 %>" />
          </ItemTemplate>
      </asp:TemplateField>      
     <asp:BoundField DataField="Customer_Name" ItemStyle-HorizontalAlign="center" 
          HeaderText="Customer Name" SortExpression="Customer_Name">
        <ItemStyle HorizontalAlign="Center" />
     </asp:BoundField>
      <asp:BoundField DataField="Collected_On" HeaderText="Collected On"  
          SortExpression="Collected_On" NullDisplayText="N/A" 
          DataFormatString="{0:dd/MM/yyyy}" >
      <ItemStyle Wrap="False" />
    </asp:BoundField> 
    <asp:BoundField DataField="Amount" ItemStyle-HorizontalAlign="center" 
          HeaderText="Amount" >
        <ItemStyle HorizontalAlign="Center" />
     </asp:BoundField>
     <asp:BoundField DataField="Cheque_No" ItemStyle-HorizontalAlign="center" HeaderText="Cheque No" >
        <ItemStyle HorizontalAlign="Center" />
     </asp:BoundField>                      
     <asp:BoundField DataField="Cheque_Date" DataFormatString = "{0:dd/MM/yyyy}" 
          HeaderText="Cheque Date" SortExpression="Cheque_Date" >
         <HeaderStyle HorizontalAlign="Left" Wrap="False" />
    </asp:BoundField>
      <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" 
          DataField="Bank_Name" HeaderText="Bank_Name" SortExpression="Bank_Name" >
          <HeaderStyle HorizontalAlign="Left" Wrap="False" />
      </asp:BoundField>                     
  </Columns>
    <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
</asp:GridView>
</asp:Content>
