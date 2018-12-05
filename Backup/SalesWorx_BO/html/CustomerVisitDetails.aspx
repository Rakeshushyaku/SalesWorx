<%@ Page Title="Customer Visit Details" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master" CodeBehind="CustomerVisitDetails.aspx.vb" Inherits="SalesWorx_BO.CustomerVisitDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
<tr>
<td id="contentofpage" width="100%" height="76%" class="topshadow">
    <div class="pgtitileposition">
        <span class="pgtitile3">Customer Visit Details&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span></div>
    <table width="100%" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
        <tr>
            <td style="padding: 6px 12px">
                <table width="100%" cellspacing="3" cellpadding="3" >                                                                                             
                    <tr><td class='txtSMBold' >Customer Name: &nbsp;<span class='txtSM'>
                    <asp:Label ID="lblCustName" runat="server" Text=""></asp:Label></span></td> <td align=right  > <div class="no-print"><a href='javascript:print();' class='txtLinkSM'><img src='../images/iconPrinter.gif' border=0 alt='Print'></a></div> </td></tr>
                    <tr><td class='txtSMBold' colspan="2">Customer No:&nbsp; <span class='txtSM'>
                    <asp:Label ID="lblCustID" runat="server" Text=""></asp:Label></span></td></tr>
                   <asp:Literal ID="litDynamicPart" runat="server"></asp:Literal>
                    <tr><td  class='txtSMBold' colspan="2">Visit Date:&nbsp; <span class='txtSM'>
                    <asp:Label ID="lblVisitDate" runat="server" Text=""></asp:Label></span></td>
                    </tr>
                    <tr><td class='txtSMBold' colspan="2">Started At:&nbsp; <span class='txtSM'>
                    <asp:Label ID="lblStartedAt" runat="server" Text=""></asp:Label></span></td></tr>
                    <tr><td class='txtSMBold' colspan="2">Ended At:&nbsp; <span class='txtSM'>
                    <asp:Label ID="lblEndedAt" runat="server" Text=""></asp:Label></span></td></tr>
                    <tr><td class='txtSMBold' colspan="2">Visited By:&nbsp; <span class='txtSM'>
                    <asp:Label ID="lblVisitedBy" runat="server" Text=""></asp:Label></span></td></tr>
                    <tr><td class='txtSMBold' colspan="2">Scanned:&nbsp; <span class='txtSM'>
                    <asp:Label ID="lblScanned" runat="server" Text=""></asp:Label></span></td></tr>
                     <tr><td class='txtSMBold' colspan="2">Odometer Reading:&nbsp; <span class='txtSM'>
                    <asp:Label ID="lblOdoReading" runat="server" Text=""></asp:Label></span></td></tr>
                       <tr><td class='txtSMBold' colspan="2">Employee Name:&nbsp; <span class='txtSM'>
                    <asp:Label ID="lblEmpName" runat="server" Text=""></asp:Label></span></td></tr>
                    
                  <%--  <tr><td class='txtSMBold' colspan="2">Notes :&nbsp; <span class='txtSM'>
                    <asp:Label ID="lblNotes" runat="server" Text=""></asp:Label></span></td></tr>--%>
                    <tr><td  colspan=2>&nbsp;</td></tr>  
                    <tr><td class='txtSMBold' colspan="2" align="center"  >
                    <div class="no-print">        
                    <asp:Button ID="btnBack" CssClass="btnInput" runat="server" Text="Back"/>           
                    <asp:Button ID="btnOrders" CssClass="btnInput" runat="server" Text="Orders"/>
                    <asp:Button ID="btnReturns" CssClass="btnInput" runat="server" Text="Returns"/>
                    <asp:Button ID="btnDistriChk" CssClass="btnInput" runat="server" Text="Distribution Check"/>
                    </div>
                    </td></tr>
                </table>       
            </td>
        </tr>
    </table>
</td>
</tr>
</table>
</asp:Content>
