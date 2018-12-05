<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DashboardSection.Master" CodeBehind="DashboardIncomingMsgs.aspx.vb" Inherits="SalesWorx_BO.DashboardIncomingMsgs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
         <asp:GridView EmptyDataText="No messages to display"  width="100%" ID="SearchResultGridView" runat="server" 
                  EmptyDataRowStyle-Font-Bold="true" 
   AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" 
   PageSize="25" CellPadding="0" CellSpacing="0" CssClass="tablecellalign" >
                    
                  <Columns>
                      <asp:BoundField DataField="Message_ID" HeaderText="Message_ID"  
                          Visible="False" />
                      <asp:BoundField DataField="Message_Title" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" SortExpression="Message_Title" HeaderText="Title"  
                          />
                      <asp:BoundField DataField="Message_Content" HeaderStyle-HorizontalAlign="Left" HeaderText="Content"  SortExpression ="Message_Content"/>
                      <asp:BoundField DataField="Message_Date"  HeaderStyle-HorizontalAlign="Left" SortExpression="Message_Date" DataFormatString="{0:dd-MM-yyyy}" 
                          HeaderText="Date" HeaderStyle-Wrap="false" ItemStyle-Wrap="false"  />
                      <asp:BoundField DataField="SalesRep_Name" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" SortExpression="SalesRep_Name" HeaderText="Van" />
                  </Columns>
                  <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
              </asp:GridView>
</asp:Content>
