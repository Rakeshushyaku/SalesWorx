<%@ Page Title="Survey List Detail" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master" CodeBehind="SurveyListDetail.aspx.vb" Inherits="SalesWorx_BO.SurveyListDetail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
<tr>
<td id="contentofpage" width="100%" height="76%" class="topshadow">
    <div class="pgtitileposition">
        <span class="pgtitile3">Survey List Details&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span></div>
    <table width="100%" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
        <tr>
            <td style="padding: 6px 12px">
                <table width="100%" cellspacing="3" cellpadding="3" >                                                                                             
                    <tr><td class='txtSMBold' ><asp:Label ID="lblcustvan" runat="server" Text="Customer Name:"/> &nbsp;<span class='txtSM'>
                    <asp:Label ID="lblCustName" runat="server" Text=""></asp:Label></span></td> 
                    <td align=right  > <div class="no-print"><a href='javascript:print();' class='txtLinkSM'><img src='../images/iconPrinter.gif' border=0 alt='Print'></a></div> </td></tr>
                      <tr><td colspan="2" class='txtSMBold'>Responses</td></tr>  
                       <tr><td colspan="2" class='txtSMBold'></td></tr>                  
                    <tr><td class='txtSMBold' colspan="2">
                           <asp:Panel ID="PnlDetails" runat="server">
                            <asp:DataList ID="DList_SurveyResult" runat="server" CellPadding="4" ForeColor="#333333" Width="100%" >
                            <ItemTemplate>
                            <b><asp:Label ID="lblQuest" runat="server" Text="Question:"></asp:Label></b>
                            <asp:Label ID="lblQuestion" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Question_Text")%>'></asp:Label>
                            <asp:HiddenField ID="hdnQuestion" runat="server" value='<%#DataBinder.Eval(Container.DataItem, "Question_ID")%>'/>
                            <p>
                            <b>Ans:</b>
                            <asp:Label ID="lblResponseText" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "ResponseText")%>'></asp:Label>     
                            </ItemTemplate>   
                            <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" Width="100%" />
                            <AlternatingItemStyle BackColor="White" />
                            <ItemStyle BackColor="#E3EAEB" Width="100%" HorizontalAlign="Left"  />
                            <SelectedItemStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White"  />
                            </asp:DataList>
                        <asp:HiddenField ID="PrevRowQuestname" runat="server" />  
                      <div style="text-align:right; padding:8px 1px;"> <asp:Button ID="btnSurResCancel" runat="server" Text="Close" Visible="False" Height="23px" Width="89px" BorderColor="#1C5E55" BorderStyle="Solid" BorderWidth="1px" BackColor="#D3D3D3" /></div>  
                    </asp:Panel>            
                    </td></tr>
                    <tr><td colspan="2"></td></tr>
                                       
                </table>
                <div class="no-print">  
                        <asp:HiddenField ID="hdnCustID" runat="server" />
                        <asp:HiddenField ID="hdnsurID" runat="server" />
    <asp:HiddenField ID="hdnSiteID" runat="server" />
    <asp:HiddenField ID="hdnsurveyResDate" runat="server" />
                    <asp:Button ID="btnBack" CssClass="btnInput" runat="server" Text="Back" PostBackUrl="~/html/SurveyList.aspx"/>        
                </div>       
            </td>
        </tr>
    </table>
</td>
</tr>
</table>   
</asp:Content>
