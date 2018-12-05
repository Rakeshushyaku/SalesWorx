<%@ Page Title="Modify Cash Van Audit Survey" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master" CodeBehind="EditCashVanAudit.aspx.vb" Inherits="SalesWorx_BO.EditCashVanAudit" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
	<tr>
	
	<td id="contentofpage" width="100%" height="76%" class="topshadow">
		<div class="pgtitileposition">
	        <span class="pgtitile3">Modify Cash Van Audit Survey</span></div>
	
	<table width="80%" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
	<tr>
<td>
<table width="100%" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td >&nbsp;</td>
            <td class="txtSMBold">&nbsp;</td>
                 <td class="txtSMBold" ><asp:Label ID="lblCustVan" runat="server" Text="Van :"></asp:Label></td>
                 <td width="105" class="txtSMBold">
                <asp:DropDownList CssClass="inputSM" ID="ddlVan" runat="server" Width ="250px" 
                         DataTextField="SalesRep_Name" DataValueField="SalesRep_ID"/>                        
              </td>
            <td>&nbsp;
                     <asp:Button CssClass="btnInputGrey" ID="SearchBtn" runat="server" Text="Search" />
                   
                     <asp:Button CssClass="btnInput" ID="Button1" runat="server" Text="Update" />
                     <asp:Button CssClass="btnInput" ID="Button2" runat="server" Text="Confirm" />
                       <asp:Label ID="txtSurveyId"  Visible ="false"   runat="server" Text=''></asp:Label>
                       <asp:Label ID="txtAVanName"  Visible ="false"   runat="server" Text=''></asp:Label>
                       <asp:Label ID="txtSalesRepid"  Visible ="false"   runat="server" Text=''></asp:Label>
                       <asp:Label ID="txtEmpCode"  Visible ="false"   runat="server" Text=''></asp:Label>
                          <asp:Label ID="txtSurveyTime"  Visible ="false"   runat="server" Text=''></asp:Label>
                     </td>
          </tr>
            </table>
</td>
</tr>
<tr><td>
  
    </td></tr>
	<tr>
       <td style='padding:5px'>
       <br />
        <asp:UpdatePanel ID="Panel" runat="server">
        <ContentTemplate>
        <table  border="0" cellspacing="0" cellpadding="0" width='100%' >
        <tr>
        <td  class="tdstyle">
           <%=DivHTML%>
     <asp:GridView Width="100%" ID="gvResponse" runat="server"  AutoGenerateColumns="False"
                                                                    PageSize="25" CellPadding="0" CellSpacing="0" CssClass="tablecellalign">
                                                                   
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="SNo" Visible ="false" >
                                                                            
                                                                            <HeaderStyle HorizontalAlign="left"  />
                                                                            <ItemTemplate>
                                                                               <asp:Label ID="txtSNo"   runat="server" Text='<%# Bind("LineId") %>' 
                                                                                    TabIndex="5" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="left" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Question">
                                                                          
                                                                            <HeaderStyle HorizontalAlign="left" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="txtQuest"   runat="server" Text='<%# Bind("Question") %>' 
                                                                                    TabIndex="5" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="left" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Response">
                                                                          
                                                                            <HeaderStyle HorizontalAlign="left" />
                                                                            <ItemTemplate>
                                                                              <asp:TextBox ID="txtResponse"  AutoPostBack ="true" OnTextChanged ="txtResponse_TextChanged"  runat="server" Text='<%# Bind("Response") %>' Width ="450px" ></asp:TextBox>
                                                                                  
                                                                                <asp:CheckBox ID="ChkDefault" AutoPostBack ="true"  OnCheckedChanged ="Chk_CheckedChanged"  runat="server" Text='<%# Bind("Response") %>' TabIndex="6"
                                                                                    Checked='<%# Bind("IsDefault") %>' />
                                                                                     <asp:RadioButton ID="Rb"  runat="server"  AutoPostBack ="true"  OnCheckedChanged ="rb_CheckedChanged"  Text='<%# Bind("Response") %>' TabIndex="6"
                                                                                    Checked='<%# Bind("IsDefault") %>'  />
                                                                        
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="left" />
                                                                        </asp:TemplateField>
                                                                         <%-- <asp:TemplateField>
                                                                            <HeaderTemplate>
                                                                                <asp:Label ID="lblDefault" runat="server" Text="Is Default" />
                                                                            </HeaderTemplate>
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="ChkDefault"  runat="server" Text="" TabIndex="6"
                                                                                    Checked='<%# Bind("IsDefault") %>' />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                        </asp:TemplateField>--%>
                                                                         <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbRespType" runat="server" Text='<%# Bind("RespTypeID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        
                                                                    <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbQuestId" runat="server" Text='<%# Bind("QuestId") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                    <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
                                                                </asp:GridView>
          </td>
          </tr>
          <tr>
          <td>
             
          </td>
          </tr>
          
        </table>
          </ContentTemplate>
          <Triggers>
          <asp:AsyncPostBackTrigger ControlID="SearchBtn" EventName="Click" />
          </Triggers>
        </asp:UpdatePanel>                    
       </td>              
	</tr>
  <tr>
                        <td>
                            <asp:UpdatePanel ID="UPModal" runat="server">
                                <ContentTemplate>
                                    <asp:Button ID="btnHidden" runat="Server" Style="display: none" />
                                    <ajaxToolkit:ModalPopupExtender ID="MpInfoError" runat="server" BackgroundCssClass="modalBackground"
                                        TargetControlID="btnHidden" PopupControlID="pnlConfirm" CancelControlID="btnClose"
                                        Drag="true" />
                                    <asp:Panel ID="pnlConfirm" Width="400" runat="server" CssClass="modalPopup" Style="cursor: move;
                                        background-color: #3399ff; text-align: center; border: solid 1px #3399ff; color: White;
                                        padding: 3px; display: none;">
                                        <table id="tableinPopupErr" width="400" cellpadding="10" style="background-color: White;">
                                            <tr align="center">
                                                <td align="center" style="cursor: move; background-color: #3399ff; text-align: center;
                                                    border: solid 1px #3399ff; color: White; padding: 3px">
                                                    <asp:Label ID="lblinfo" runat="server" Font-Size ="13px"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" style="text-align: center">
                                                    <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="List" />
                                                    <asp:Label ID="lblMessage" runat="server" Font-Size ="13px"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" style="text-align: center;">
                                                                <asp:Button ID="btnClose" runat="server" Text="Ok" CssClass="btnInputBlue" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    </td> </tr> </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
    </table>
      <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="UPModal" runat="server">
                    <ProgressTemplate>
                        <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                            <img src="../images/Progress.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                            <span style="font-size: 12px; color: #3399ff;">Processing... </span>
                        </asp:Panel>
                    </ProgressTemplate>
                </asp:UpdateProgress>
	<br/>
	<br/>
	</td> 
	</tr>
	</table>
</asp:Content>
