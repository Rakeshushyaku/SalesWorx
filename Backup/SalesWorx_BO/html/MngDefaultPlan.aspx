<%@ Page Title="Default Plan" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master"
    CodeBehind="MngDefaultPlan.aspx.vb" Inherits="SalesWorx_BO.MngDefaultPlan" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="javascript" type="text/javascript">

        var prm = Sys.WebForms.PageRequestManager.getInstance();

        prm.add_initializeRequest(InitializeRequest);
        prm.add_endRequest(EndRequest);
        var postBackElement;
        function InitializeRequest(sender, args) {

            if (prm.get_isInAsyncPostBack())
                args.set_cancel(true);
            postBackElement = args.get_postBackElement();
            if (postBackElement.id == 'ctl00_ContentPlaceHolder1_ShowAndRefreshBtn') {
                $get('<%= Me.UpdateProgress1.ClientID %>').style.display = 'block';
            }

            if (postBackElement.id == 'ctl00_ContentPlaceHolder1_SaveBtn')
                $get('<%= Me.UpdateProgress1.ClientID %>').style.display = 'block';

            if (postBackElement.id == 'ctl00_ContentPlaceHolder1_UpdateBtn')
                $get('<%= Me.UpdateProgress1.ClientID %>').style.display = 'block';

            postBackElement.disabled = true;
        }



        function EndRequest(sender, args) {
            if (postBackElement.id == 'ctl00_ContentPlaceHolder1_ShowAndRefreshBtn') {
                $get('<%= Me.UpdateProgress1.ClientID %>').style.display = 'none';
            }
            if (postBackElement.id == 'ctl00_ContentPlaceHolder1_SaveBtn')
                $get('<%= Me.UpdateProgress1.ClientID %>').style.display = 'none';

            if (postBackElement.id == 'ctl00_ContentPlaceHolder1_UpdateBtn')
                $get('<%= Me.UpdateProgress1.ClientID %>').style.display = 'none';

            postBackElement.disabled = false;
        } 

    </script>

    <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
        <tr>
            <td id="contentofpage" width="100%" height="76%" class="topshadow">
                <div class="pgtitileposition">
                    <span class="pgtitile3">Default Plan</span></div>
                                <div id="pagenote">
                    This screen may be used to create a default route plans for the org.</div>
                <table width="100%" border="0" cellspacing="0" cellpadding="0" id="tableform">
                
                    <tr>
                        <td>
                            <label id="labelstyle">
                                Description:</label>
                        </td>
                        <td>
                            <asp:TextBox ID="Description" runat="server" Width ="200px" TabIndex="1" ToolTip="Description" AutoCompleteType="Disabled"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>
                                Start Date:</label>
                        </td>
                        <td>
                      
                        
                                    <asp:TextBox ID="Start_Date"  Width ="150px" TabIndex="2" runat="server" EnableViewState="true" ToolTip="Click here to set Start Date"
                                        AutoCompleteType="Disabled" AutoPostBack="True" ></asp:TextBox>
                                    <ajaxToolkit:CalendarExtender CssClass="cal_Theme1" ID="calendarButtonExtender" Format="dd/MM/yyyy"
                                        runat="server" TargetControlID="Start_Date"  PopupButtonID="Start_Date" />
                                      
                                        
                           
                        </td>
                        <td>
                            <asp:Button ID="ShowAndRefreshBtn" runat="server" CssClass="btnInputBlue" Text="Show/Refresh Calendar" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>
                                End Date:</label>
                        </td>
                        <td>
                            <asp:TextBox ID="End_Date" runat="server"  Width ="150px" TabIndex="3" ReadOnly="true" ToolTip="Click here to set End Date"
                                AutoCompleteType="Disabled" Enabled ="false"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender CssClass="cal_Theme1" ID="CalendarExtender1" Format="dd/MM/yyyy"
                                runat="server" TargetControlID="End_Date" PopupButtonID="End_Date"  Enabled ="false"/>
                        </td>
                        <asp:Panel ID="AddAndSavePanel" runat="server" Visible="false">
                            <td>
                                <asp:Button ID="SaveBtn" CssClass="btnInputGreen" runat="server" Text="Save" />
                                <asp:Button ID="CancelBtn1" CssClass="btnInputRed" runat="server" Text="Cancel" CausesValidation="false" />
                            </td>
                        </asp:Panel>
                        <asp:Panel ID="ModAndUpdatePanel" runat="server" Visible="false">
                            <td>
                                <asp:Button ID="UpdateBtn" CssClass="btnInputGreen" runat="server" Text="Update" CausesValidation="false" />
                                <asp:Button ID="CancelBtn" CssClass="btnInputRed" runat="server" Text="Cancel" CausesValidation="false" />
                            </td>
                        </asp:Panel>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:UpdatePanel ID="UpdateCal" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Panel ID="CalPanel" runat="server" Visible="false" CssClass="TblCalendarCntrl">
                                        <asp:Calendar DayHeaderStyle-CssClass="hdrCalendarCntrl" CssClass="calendar" ID="DefPlanCalendar"
                                            runat="server" ShowGridLines="True" ShowNextPrevMonth="False" Width="100%" CellPadding="0"
                                            BorderColor="#EDEDED" BorderStyle="Solid" BorderWidth="1px" ShowTitle="False"
                                            BackColor="White">
                                            <DayStyle CssClass="txtCalDate" Height="90px" VerticalAlign="Top" Width="130px" />
                                            <DayHeaderStyle BackColor="#ddb500" Font-Bold="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:Calendar>
                                        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" Style="display: none" />
                                    </asp:Panel>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ShowAndRefreshBtn" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="SaveBtn" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="UpdateBtn" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                            <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="UpdateCal" runat="server">
                                <ProgressTemplate>
                                    <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                                        <img src="../images/Progress.gif" alt="Processing..." style="vertical-align: middle;" />
                                        <span style="font-size: 12px; font-weight: 700px; color: #3399ff;">Processing...
                                        
                                        </span>
                                    </asp:Panel>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                    </tr>
                </table>
                <input type="hidden" name="RP_ID" id="RP_ID" runat="server" />
                <input type="hidden" name="ISRefreskClick" id="ISRefreskClick" runat="server" />
                <input type="hidden" name="Action_Mode" id="Action_Mode" runat="server" />
                <input type="hidden" name="CheckRefresh" id="CheckRefresh" runat="server" />
                <input type="hidden" name="FromPopUp" id="FromPopUp" runat="server" />
                <input type="hidden" name="DayRef" id="DayRef" runat="server" />
                <input type="hidden" name="SetType" id="SetType" runat="server" />
                <input type="hidden" name="UserComments" id="UserComments" runat="server" />
                <%-- ' <asp:Literal ID="Action_Mode" runat="server" Visible="false"></asp:Literal>--%>
                <input type="hidden" name="RP_ID" id="Hidden1" runat="server" />
                <input type="hidden" name="Action_Mode" id="Hidden2" runat="server" />
                <input type="hidden" name="DayRef" id="Hidden3" value="" runat="server" />
                <input type="hidden" name="IsPopUp" id="IsPopUp" value="" runat="server" />
                <input type="hidden" name="HWorkingDays" id="HWorkingDays" value="0" />
                <input type="hidden" name="HDay1" id="HDay1" value="" runat="server" />
                <input type="hidden" name="HDay2" id="HDay2" value="" runat="server" />
                <input type="hidden" name="HDay3" id="HDay3" value="" runat="server" />
                <input type="hidden" name="HDay4" id="HDay4" value="" runat="server" />
                <input type="hidden" name="HDay5" id="HDay5" value="" runat="server" />
                <input type="hidden" name="HDay6" id="HDay6" value="" runat="server" />
                <input type="hidden" name="HDay7" id="HDay7" value="" runat="server" />
                <input type="hidden" name="HDay8" id="HDay8" value="" runat="server" />
                <input type="hidden" name="HDay9" id="HDay9" value="" runat="server" />
                <input type="hidden" name="HDay10" id="HDay10" value="" runat="server" />
                <input type="hidden" name="HDay11" id="HDay11" value="" runat="server" />
                <input type="hidden" name="HDay12" id="HDay12" value="" runat="server" />
                <input type="hidden" name="HDay13" id="HDay13" value="" runat="server" />
                <input type="hidden" name="HDay14" id="HDay14" value="" runat="server" />
                <input type="hidden" name="HDay15" id="HDay15" value="" runat="server" />
                <input type="hidden" name="HDay16" id="HDay16" value="" runat="server" />
                <input type="hidden" name="HDay17" id="HDay17" value="" runat="server" />
                <input type="hidden" name="HDay18" id="HDay18" value="" runat="server" />
                <input type="hidden" name="HDay19" id="HDay19" value="" runat="server" />
                <input type="hidden" name="HDay20" id="HDay20" value="" runat="server" />
                <input type="hidden" name="HDay21" id="HDay21" value="" runat="server" />
                <input type="hidden" name="HDay22" id="HDay22" value="" runat="server">
                <input type="hidden" name="HDay23" id="HDay23" value="" runat="server" />
                <input type="hidden" name="HDay24" id="HDay24" value="" runat="server" />
                <input type="hidden" name="HDay25" id="HDay25" value="" runat="server" />
                <input type="hidden" name="HDay26" id="HDay26" value="" runat="server" />
                <input type="hidden" name="HDay27" id="HDay27" value="" runat="server" />
                <input type="hidden" name="HDay28" id="HDay28" value="" runat="server" />
                <input type="hidden" name="HDay29" id="HDay29" value="" runat="server" />
                <input type="hidden" name="HDay30" id="HDay30" value="" runat="server" />
                <input type="hidden" name="HDay31" id="HDay31" value="" runat="server" />
                <input type="hidden" name="Cell1" id="Cell1" value="" runat="server" />
                <input type="hidden" name="Cell2" id="Cell2" value="" runat="server" />
                <input type="hidden" name="Cell3" id="Cell3" value="" runat="server" />
                <input type="hidden" name="Cell4" id="Cell4" value="" runat="server" />
                <input type="hidden" name="Cell5" id="Cell5" value="" runat="server" />
                <input type="hidden" name="Cell6" id="Cell6" value="" runat="server" />
                <input type="hidden" name="Cell7" id="Cell7" value="" runat="server" />
                <input type="hidden" name="Cell8" id="Cell8" value="" runat="server" />
                <input type="hidden" name="Cell9" id="Cell9" value="" runat="server" />
                <input type="hidden" name="Cell10" id="Cell10" value="" runat="server" />
                <input type="hidden" name="Cell11" id="Cell11" value="" runat="server" />
                <input type="hidden" name="Cell12" id="Cell12" value="" runat="server" />
                <input type="hidden" name="Cell13" id="Cell13" value="" runat="server" />
                <input type="hidden" name="Cell14" id="Cell14" value="" runat="server" />
                <input type="hidden" name="cell15" id="Cell15" value="" runat="server" />
                <input type="hidden" name="Cell16" id="Cell16" value="" runat="server" />
                <input type="hidden" name="Cell17" id="Cell17" value="" runat="server" />
                <input type="hidden" name="Cell18" id="Cell18" value="" runat="server" />
                <input type="hidden" name="Cell19" id="Cell19" value="" runat="server" />
                <input type="hidden" name="Cell20" id="Cell20" value="" runat="server" />
                <input type="hidden" name="Cell21" id="Cell21" value="" runat="server" />
                <input type="hidden" name="Cell22" id="Cell22" value="" runat="server" />
                <input type="hidden" name="Cell23" id="Cell23" value="" runat="server" />
                <input type="hidden" name="Cell24" id="Cell24" value="" runat="server" />
                <input type="hidden" name="Cell26" id="Cell26" value="" runat="server" />
                <input type="hidden" name="Cell27" id="Cell27" value="" runat="server" />
                <input type="hidden" name="Cell25" id="Cell25" value="" runat="server" />
                <input type="hidden" name="Cell28" id="Cell28" value="" runat="server" />
                <input type="hidden" name="Cell29" id="Cell29" value="" runat="server" />
                <input type="hidden" name="Cell30" id="Cell30" value="" runat="server" />
                <input type="hidden" name="Cell31" id="Cell31" value="" runat="server" />
                <asp:RequiredFieldValidator runat="server" ID="DescriptionReq" ControlToValidate="Description"
                    Display="None" ErrorMessage="<b>Required Field Missing</b><br />Description is required." />
                <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="DescriptionReqE" Width="30%"
                    TargetControlID="DescriptionReq" />
                <asp:RequiredFieldValidator runat="server" ID="StartDateReq" ControlToValidate="Start_Date"
                    Display="None" ErrorMessage="<b>Required Field Missing</b><br />Start Date is required." />
                <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="StartDateReqE" Width="30%"
                    TargetControlID="StartDateReq" />
                <asp:RequiredFieldValidator runat="server" ID="EndDateReq" ControlToValidate="End_Date"
                    Display="None" ErrorMessage="<b>Required Field Missing</b><br />End Date is required." />
                <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="EndDateReqE" Width="30%"
                    TargetControlID="EndDateReq" />
                <asp:RegularExpressionValidator ID="StartDateRegEx" runat="server" Display="None"
                    ErrorMessage="<b>Valid Start Date is required.</b>" ControlToValidate="Start_date"
                    ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$">
                </asp:RegularExpressionValidator>
                <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidStartDateReq" Width="30%"
                    TargetControlID="StartDateRegEx" />
                <asp:RegularExpressionValidator ID="EndDateRegEx" runat="server" Display="None" ErrorMessage="<b>Valid End Date is required.</b>"
                    ControlToValidate="End_Date" ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$">
                </asp:RegularExpressionValidator>
                <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender1"
                    Width="30%" TargetControlID="EndDateRegEx" />
                <br />
                <br />
            </td>
        </tr>
    </table>
</asp:Content>
