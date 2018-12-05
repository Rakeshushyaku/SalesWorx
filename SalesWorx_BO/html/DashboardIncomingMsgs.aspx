<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="DashboardIncomingMsgs.aspx.vb" Inherits="SalesWorx_BO.DashboardIncomingMsgs" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Charting" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="FeaturedContent" runat="server">
 </asp:Content>   
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server" >
    <h4>Incoming Messages</h4> 
    <div style="text-align:right">
        <asp:Button ID="Btn_back" runat="server" Text="Back"  CssClass="btn btn-success" />
    </div>
    <br />
       <telerik:RadAjaxPanel ID="RadAjaxPanel3" runat ="server" >
         <asp:GridView EmptyDataText="No messages to display"  width="100%" ID="SearchResultGridView" runat="server" 
                  EmptyDataRowStyle-Font-Bold="true" 
   AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" 
   PageSize="10" CellPadding="0" CellSpacing="0" CssClass="tablecellalign" >
                    
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
            </telerik:RadAjaxPanel>
</asp:Content>
