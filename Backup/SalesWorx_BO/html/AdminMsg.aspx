<%@ Page Title="New Message" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master"
    CodeBehind="AdminMsg.aspx.vb" Inherits="SalesWorx_BO.AdminMsg" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script language="javascript">
        function processAction() {
            history.go(-1);
        }
    </script>

    <script runat="server">
        Sub Myvalidation(ByVal source As Object, ByVal args As ServerValidateEventArgs)
            Try
                If Me.SalesRepList.SelectedIndex = -1 Then
                    args.IsValid = False
                Else
                    args.IsValid = True
                End If
            Catch ex As Exception
            End Try
        End Sub
        
        Sub MyvalidationDate(ByVal source As Object, ByVal args As ServerValidateEventArgs)
            Try
                Dim MsgDateArr As Array = Sel_Date.Text.Split("/")
                Dim MsgDate As Date = CDate(MsgDateArr(1) & "-" & MsgDateArr(0) & "-" & MsgDateArr(2))
                Dim ExDateArr As Array = Expiry_Date.Text.Split("/")
                Dim ExDate As Date = CDate(ExDateArr(1) & "-" & ExDateArr(0) & "-" & ExDateArr(2))
                
                If MsgDate >= ExDate Then
                    args.IsValid = False
                Else
                    args.IsValid = True
                End If
            Catch ex As Exception
            End Try
        End Sub
    </script>

    <link href="../styles/salesworx.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField runat="server" ID="ButtonStatus" />
    <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
        <tr>
            <td id="contentofpage" width="100%" height="76%" class="topshadow">
                <div class="pgtitileposition">
                    <span class="pgtitile3">New Messages</span></div>
                <div id="pagenote">
                    Use the Messages screen to create new messages for one or more vans.
                    You can also locate and recall messages previously created for vans.</div>
                <table width="auto" border="0" cellspacing="0" cellpadding="0" id="tableform">
                    <tr>
                        <td align="left" valign="top" class="tdclear">
                            <asp:CustomValidator Display="Dynamic" CssClass="txtBold" ID="CustomValidator1" runat="server"
                                OnServerValidate="Myvalidation" ErrorMessage="Select atleast one SalesRep" ValidationGroup="G3"></asp:CustomValidator>
                            <asp:CustomValidator Display="Dynamic" CssClass="txtBold" ID="CustomValidator2" runat="server"
                                OnServerValidate="MyvalidationDate" ErrorMessage="Expiry date should be greater than selected date. "
                                ValidationGroup="G3"></asp:CustomValidator>
                            <asp:Panel ID="TopPanel" runat="server">
                                <table border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td class="txtBold" style="width: 200px">
                                            Please select a date:
                                        </td>
                                        <td class="txt" valign="top">
                                            <asp:TextBox ID="MSG_Date" runat="server"></asp:TextBox>
                                            &nbsp;<ajaxToolkit:CalendarExtender CssClass="cal_Theme1" ID="calendarButtonExtender"
                                                Format="dd/MM/yyyy" runat="server" TargetControlID="MSG_Date" PopupButtonID="MSG_Date" />
                                        </td>
                                        <td>
                                            <asp:Button ID="GetMsgBtn" class="btnInputBlue" runat="server" Text="Get Message" ValidationGroup="G1" />
                                        </td>
                                        <td>
                                            <asp:Button ID="NewMsgBtn" class="btnInputGreen" runat="server" Text="New Message" CausesValidation="false" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="MsgSelectPanel" runat="server" Visible="false">
                                <table border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td class="txtBold" style="width: 200px;">
                                            Messages for date selected:
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="MessageSelDD" DataTextField="Msg_Title" DataValueField="Msg_ID"
                                                runat="server">
                                            </asp:DropDownList>
                                        </td>
                                        <td valign="top" class="txt">
                                            &nbsp;&nbsp;&nbsp;
                                            <asp:Button ID="ViewandRecallBtn"  CssClass="btnInputBlue"  runat="server" Text="View/Recall"
                                                ValidationGroup="G2" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="NewMsgPanel" runat="server" Visible="false">
                                <table border="0" cellspacing="0" cellpadding="4">
                                    <tr>
                                        <td colspan="3">
                                            <asp:Label ID="MsgLbl" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" align="left" width="175" class="txtBold">
                                            Please select a date:
                                        </td>
                                        <td width="150" class="txt" valign="top">
                                            <asp:TextBox CssClass="input" ID="Sel_Date" runat="server"></asp:TextBox>
                                            &nbsp;<ajaxToolkit:CalendarExtender CssClass="cal_Theme1" ID="SelDateCalendar" Format="dd/MM/yyyy"
                                                runat="server" TargetControlID="Sel_Date" PopupButtonID="Sel_Date" />
                                        </td>
                                        <td valign="top" class="txt">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" width="175" class="txtBold">
                                            Expiry Date:
                                        </td>
                                        <td width="150" class="txt" valign="top">
                                            <asp:TextBox CssClass="input" ID="Expiry_Date" runat="server"></asp:TextBox>
                                            &nbsp;<ajaxToolkit:CalendarExtender CssClass="cal_Theme1" ID="ExpiryDateCalendar"
                                                Format="dd/MM/yyyy" runat="server" TargetControlID="Expiry_Date" PopupButtonID="Expiry_Date" />
                                        </td>
                                        <td valign="top" class="txt">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" width="175" class="txtBold">
                                            Title:
                                        </td>
                                        <td width="150" class="txt" valign="top">
                                            <asp:TextBox CssClass="input" ID="TitleTxt" runat="server"></asp:TextBox>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" width="175" class="txtBold">
                                            Message Text:
                                        </td>
                                        <td width="150" class="txt" valign="top">
                                            <asp:TextBox CssClass="input" ID="MsgTxt" runat="server" Height="101px" TextMode="MultiLine"
                                               Width="400px"></asp:TextBox>
                                        </td>
                                        <td valign="top" class="txt">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" width="175" class="txtBold">
                                            Assign To:
                                        </td>
                                        <td width="150" class="txt" valign="top">
                                            <%--<asp:ListBox ID="SalesRepList" runat="server" DataTextField="SalesRep_Name" 
                          DataValueField="SalesRep_ID" SelectionMode="Multiple" ></asp:ListBox>--%>
                                            <asp:Panel ID="Panel1" runat="server" Height="200px" ScrollBars="Auto" BorderStyle="Groove"
                                                BorderWidth="1px" Width="400px">
                                                <asp:CheckBoxList ID="SalesRepList" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID"
                                                    runat="server" RepeatColumns="2">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                        </td>
                                        <td valign="bottom">
                                      
                                        </td>
                                    </tr>
                                    <tr>
                                    <td></td>
                                       <td valign="bottom" colspan ="2">
                                            <asp:Button ID="SendMsgBtn" CssClass="btnInputGreen" runat="server" Text="Send Message"
                                                ValidationGroup="G3" />
                                            <asp:Button ID="RecallBtn" runat="server" CssClass="btnInputGreen" Text="Recall" />
                                            <asp:Button ID="CancelBtn" CssClass="btnInputRed" runat="server" Text="Cancel" />
                                            <input type='button' id="BackBtn" class='btnInputGrey' runat="server" value='Back' onclick="processAction();" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
                <br />
                <br />
            </td>
        </tr>
    </table>
    <asp:RequiredFieldValidator runat="server" ID="DescriptionReq" ControlToValidate="MSG_Date"
        ValidationGroup="G1" Display="None" ErrorMessage="<b>Required Field Missing</b><br />Please select a Date." />
    <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="DescriptionReqE" Width="25%"
        TargetControlID="DescriptionReq" />
    <asp:RegularExpressionValidator ID="StartDateRegEx" runat="server" Display="None"
        ValidationGroup="G1" ErrorMessage="<b>Valid Date is required.</b>" ControlToValidate="MSG_Date"
        ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$">
    </asp:RegularExpressionValidator>
    <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidStartDateReq" Width="25%"
        TargetControlID="StartDateRegEx" />
    <asp:RequiredFieldValidator runat="server" ID="ReqMsg" ControlToValidate="MessageSelDD"
        ValidationGroup="G2" Display="None" InitialValue="-- Select a Message --" ErrorMessage="<b>Required Field Missing</b><br />Please select a Message." />
    <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender1"
        Width="25%" TargetControlID="ReqMsg" />
    <asp:RequiredFieldValidator runat="server" ID="SelDateReq" ControlToValidate="Sel_Date"
        ValidationGroup="G3" Display="None" ErrorMessage="<b>Required Field Missing</b><br />Please select a date." />
    <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender2"
        Width="25%" TargetControlID="SelDateReq" />
    <asp:RegularExpressionValidator ID="SelDateRegularEx" runat="server" Display="None"
        ValidationGroup="G3" ErrorMessage="<b>Valid Date is required.</b>" ControlToValidate="Sel_Date"
        ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$">
    </asp:RegularExpressionValidator>
    <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender3"
        Width="25%" TargetControlID="SelDateRegularEx" />
    <asp:RequiredFieldValidator runat="server" ID="ExpiryDateReq" ControlToValidate="Expiry_Date"
        ValidationGroup="G3" Display="None" ErrorMessage="<b>Required Field Missing</b><br />Please select a expiry date." />
    <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender4"
        Width="25%" TargetControlID="ExpiryDateReq" />
    <asp:RegularExpressionValidator ID="ExDateRegularEx" runat="server" Display="None"
        ValidationGroup="G3" ErrorMessage="<b>Valid Date is required.</b>" ControlToValidate="Sel_Date"
        ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$">
    </asp:RegularExpressionValidator>
    <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender5"
        Width="25%" TargetControlID="ExDateRegularEx" />
    <asp:RequiredFieldValidator runat="server" ID="TitleReq" ControlToValidate="TitleTxt"
        ValidationGroup="G3" Display="None" ErrorMessage="<b>Required Field Missing</b><br />Please enter a valid message title." />
    <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender6"
        Width="25%" TargetControlID="TitleReq" />
    <asp:RequiredFieldValidator runat="server" ID="MsgReq" ControlToValidate="MsgTxt"
        ValidationGroup="G3" Display="None" ErrorMessage="<b>Required Field Missing</b><br />Please enter valid message text." />
    <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender7"
        Width="25%" TargetControlID="MsgReq" />
</asp:Content>
