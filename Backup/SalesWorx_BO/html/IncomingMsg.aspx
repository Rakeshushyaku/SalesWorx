<%@ Page Title="Incoming Message" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master" CodeBehind="IncomingMsg.aspx.vb" Inherits="SalesWorx_BO.IncomingMsg" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

 <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">


	<tr>
	
	<td id="contentofpage" width="100%" height="76%" class="topshadow">
	
		<div class="pgtitileposition">
	<span class="pgtitile3">Incoming Messages</span></div>
	<div id="pagenote" >Incoming Messages shows a list of messages for a selected set of dates.</div>	
	    	<table width="auto" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
	<tr>
	<td style="padding-left:40px;">
	<asp:UpdatePanel ID="Panel" runat="server">
        <ContentTemplate>
              
        <table  border="0" cellspacing="0" cellpadding="0">
         <tr><th></th></tr>
          <tr> 
            <td align="left"  class="txtSMBold">From:</td>
            <td><asp:TextBox ID="From_Message_Date" CssClass="inputSM" runat="server"></asp:TextBox>
              &nbsp;<ajaxToolkit:CalendarExtender CssClass="cal_Theme1" ID="FromDateExtender" Format="dd/MM/yyyy" runat="server" TargetControlID="From_Message_Date" 
              /></td>
            <td align="left"  class="txtSMBold">To:</td>
            <td><asp:TextBox ID="To_Message_Date" CssClass="inputSM" runat="server"></asp:TextBox> 
              &nbsp;<ajaxToolkit:CalendarExtender CssClass="cal_Theme1" ID="ToDateExtender" Format="dd/MM/yyyy" runat="server" TargetControlID="To_Message_Date" 
             /></td>
          </tr>
          <tr> 
            <td  class="txtSMBold">Van:</td>
            <td colspan="3"><asp:DropDownList CssClass="inputSM" ID="SalesRep_ID" 
                    runat="server" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID">
                </asp:DropDownList> &nbsp;
                <asp:Button CssClass="btnInputGrey" ID="SearchBtn" runat="server" Text="Search" />
                <asp:Button CssClass="btnInputBlue" ID="NewMsgBtn" runat="server" Text="New Message" />
                 </td>
          </tr>
           <tr><th></th></tr>
        </table>
        <table >
        <tr>
        <td>
              <asp:GridView EmptyDataText="There is no message"  width="100%" ID="SearchResultGridView" runat="server" 
                   AutoGenerateColumns="False" AllowPaging="True" 
                   PageSize="25" CellPadding="0" CellSpacing="0" CssClass="tablecellalign" AllowSorting="True" DataKeyNames="Message_ID" >
                      
                  <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
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
          </td>
          </tr>
          
        </table>
          </ContentTemplate>
        </asp:UpdatePanel>
	</td>
	</tr>

    </table>
	<br/>
	<br/>
	</td> 
	</tr>
	
</table>
</asp:Content>
