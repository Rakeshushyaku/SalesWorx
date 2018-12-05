<%@ Page Title="Default Plan" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master"
    CodeBehind="MngDefaultPlan.aspx.vb" Inherits="SalesWorx_BO.MngDefaultPlan" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
     <%--<script type="text/javascript" language="javascript">
         Sys.Application.add_load(setCalendarTable);
         function setCalendarTable(sender, eventArgs) {
             var picker = $find("<%= StartTime.ClientID %>");
            var calendar = picker.get_calendar();
            var fastNavigation = calendar._getFastNavigation();
            //Changing the Month Display Order
            fastNavigation.MonthNames[0] = "Jan";
            fastNavigation.MonthNames[1] = "Jul";
            fastNavigation.MonthNames[2] = "Feb";
            fastNavigation.MonthNames[3] = "Aug";
            fastNavigation.MonthNames[4] = "Mar";
            fastNavigation.MonthNames[5] = "Sep";
            fastNavigation.MonthNames[6] = "Apr";
            fastNavigation.MonthNames[7] = "Oct";
            fastNavigation.MonthNames[8] = "May";
            fastNavigation.MonthNames[9] = "Nov";
            fastNavigation.MonthNames[10] = "Jun";
            fastNavigation.MonthNames[11] = "Dec";

            $clearHandlers(picker.get_popupButton());
            picker.get_popupButton().href = "javascript:void(0);";
            $addHandler(picker.get_popupButton(), "click", function () {
                var textbox = picker.get_textBox();
                //adjust where to show the popup table
                var x, y;
                var adjustElement = textbox;
                if (textbox.style.display == "none")
                    adjustElement = picker.get_popupImage();

                var pos = picker.getElementPosition(adjustElement);
                x = pos.x;
                y = pos.y + adjustElement.offsetHeight;

                var e = {
                    clientX: x,
                    clientY: y - document.documentElement.scrollTop
                };
                var date = picker.get_selectedDate();
                if (date) {
                    var changedMonthNo = date.getChangedMonthNo();
                    var changeddate = new Date(date.getFullYear(), changedMonthNo, 1);
                    calendar.get_focusedDate()[0] = changeddate.getFullYear();
                    calendar.get_focusedDate()[1] = changeddate.getMonth() + 1;
                }
                $get(calendar._titleID).onclick(e);
                return false;
            });

            fastNavigation.OnOK =
                       function () {
                           debugger;

                           var m = fastNavigation.SelectedMonthCell.textContent;
                           var number = getSelectedMonthNumber(m);

                           var date = new Date(fastNavigation.Year, number, 1);

                           picker.get_dateInput().set_selectedDate(date);
                           fastNavigation.Popup.Hide();
                       }


            fastNavigation.OnToday =
                       function () {
                           var date = new Date();
                           picker.get_dateInput().set_selectedDate(date);
                           fastNavigation.Popup.Hide();
                       }
        }


        //Adding new function Prototypes
        Date.prototype.getChangedMonthNo = function () {
            return this.changedmonthNo[this.getMonth()];
        };
        Date.prototype.changedmonthNo = [
                           0, 2, 4,
                           6, 8, 10,
                           1, 3, 5,
                           7, 9, 11
        ];


        function getSelectedMonthNumber(selectedMonthName) {
            if (selectedMonthName.replace(/^\s+|\s+$/g, '') == "Jan") {
                MonthValue = 0;
            }
            else if (selectedMonthName.replace(/^\s+|\s+$/g, '') == "Feb") {
                MonthValue = 1;
            }
            else if (selectedMonthName.replace(/^\s+|\s+$/g, '') == "Mar") {
                MonthValue = 2;
            }
            else if (selectedMonthName.replace(/^\s+|\s+$/g, '') == "Apr") {
                MonthValue = 3;
            }
            else if (selectedMonthName.replace(/^\s+|\s+$/g, '') == "May") {
                MonthValue = 4;
            }
            else if (selectedMonthName.replace(/^\s+|\s+$/g, '') == "Jun") {
                MonthValue = 5;
            }
            else if (selectedMonthName.replace(/^\s+|\s+$/g, '') == "Jul") {
                MonthValue = 6;
            }
            else if (selectedMonthName.replace(/^\s+|\s+$/g, '') == "Aug") {
                MonthValue = 7;
            }
            else if (selectedMonthName.replace(/^\s+|\s+$/g, '') == "Sep") {
                MonthValue = 8;
            }
            else if (selectedMonthName.replace(/^\s+|\s+$/g, '') == "Oct") {
                MonthValue = 9;
            }
            else if (selectedMonthName.replace(/^\s+|\s+$/g, '') == "Nov") {
                MonthValue = 10;
            }
            else if (selectedMonthName.replace(/^\s+|\s+$/g, '') == "Dec") {
                MonthValue = 11;
            }
            return MonthValue;
        }
        </script>  --%>
    
    
    <script language="javascript" type="text/javascript">
          function alertCallBackFn(arg) {

          }



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

        function OpenWindow(URL)
        {
           // alert(URL);
            var oWnd = radopen(URL, null);
            oWnd.SetSize(450, 300);
            oWnd.set_behaviors(4); //Close:4
            oWnd.SetModal(true);
            oWnd.Center;
            oWnd.set_visibleStatusbar(false)
        }

    </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
        <h4>Default Plan</h4>
  

    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" Skin="Simple" EnableShadow="true">
    </telerik:RadWindowManager>

     <asp:UpdatePanel ID="UpdateCal" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="row">
                <div class="col-sm-3">
                  <div class="form-group">
                        <label>Description</label>
                        <telerik:RadTextBox MaxLength="100" TabIndex="5" runat="server" ID="Description" EmptyMessage="Description" Skin="Simple" Width="100%"></telerik:RadTextBox>
                  </div>
                </div>
                <div class="col-sm-3">
                    <div class="form-group">
                        <label>Month</label>
                        <telerik:RadMonthYearPicker RenderMode="Lightweight" ID="StartTime" runat="server"   >
                                                            <DateInput ReadOnly="true" DisplayDateFormat="MMM-yyyy">
                                                        </DateInput>
                                                         
                                                        </telerik:RadMonthYearPicker>
                  
                    </div>
                </div>

                <div class="col-sm-3" style ="display:none;">
                 <div class="form-group">
                         <label>Start Date    </label>

                        <telerik:RadDatePicker ID="Start_Date" Width="30%" runat="server" Skin="Simple">
                            <Calendar Skin="Simple" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                                ViewSelectorText="x">
                            </Calendar>
                            <DatePopupButton HoverImageUrl="" ImageUrl="" TabIndex="3" />
                            <DateInput ReadOnly="true" DateFormat="dd/MM/yyyy" DisplayDateFormat="dd/MM/yyyy">
                            </DateInput>

                        </telerik:RadDatePicker>
                </div>
                </div>

                <div class="col-sm-3" style ="display:none;">
                <div class="form-group">
                <label>End Date    </label>

                    <telerik:RadDatePicker ID="End_Date" Width="30%" runat="server" Skin="Simple">
                        <Calendar Skin="Simple" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                            ViewSelectorText="x">
                        </Calendar>
                        <DatePopupButton HoverImageUrl="" ImageUrl="" TabIndex="3" />
                        <DateInput ReadOnly="true" DateFormat="dd/MM/yyyy" DisplayDateFormat="dd/MM/yyyy">
                        </DateInput>

                    </telerik:RadDatePicker>
                </div>
                </div>

                <div class="col-sm-3">
                    <div class="form-group">
                        <label class="hidden-xs empty-label"><br /></label>
                    <telerik:RadButton ID="ShowAndRefreshBtn" Skin="Simple" runat="server" Text="Show/Refresh Calendar" CssClass="btn btn-primary2" />
                    </div>
                </div>

                <div class="col-sm-3">
                <div class="form-group">
                    <label class="hidden-xs empty-label"><br /></label>
                    <asp:Panel ID="AddAndSavePanel" runat="server" Visible="false">

                    
                        <telerik:RadButton ID="SaveBtn" Skin="Simple" runat="server" Text="Save" CssClass="btn btn-success" />
                        <telerik:RadButton ID="CancelBtn1" CausesValidation="false" Skin="Simple" runat="server" Text="Cancel" CssClass="btn btn-default" />

                    

                    </asp:Panel>
                    
                     <asp:Panel ID="ModAndUpdatePanel" runat="server" Visible="false">

                    
                        <telerik:RadButton ID="UpdateBtn" CausesValidation="false" Skin="Simple" runat="server" Text="Update" CssClass="btn btn-success" />
                        <telerik:RadButton ID="CancelBtn" CausesValidation="false" Skin="Simple" runat="server" Text="Cancel" CssClass="btn btn-default" />

                    

                </asp:Panel>
                    </div>
            </div>
        </div>
                
            <div class="table-responsive">
                <asp:Panel ID="CalPanel" runat="server" Visible="false" CssClass="TblCalendarCntrl">
                    <asp:Calendar DayHeaderStyle-CssClass="hdrCalendarCntrl" CssClass="calendar" ID="DefPlanCalendar"
                        runat="server" ShowGridLines="True" ShowNextPrevMonth="False" Width="100%" CellPadding="0"
                        BorderColor="#666666" BorderStyle="Solid" BorderWidth="1px" ShowTitle="False"
                        BackColor="White">
                        <DayStyle CssClass="txtCalDate" Height="90px" BorderColor="#666666" BorderStyle="Solid" BorderWidth="1px" VerticalAlign="Top" Width="130px" />
                        <DayHeaderStyle BackColor="#ffffff" BorderColor="#666666" BorderStyle="Solid" BorderWidth="1px" Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:Calendar>
                    <asp:Button ID="Button1" CausesValidation="false" runat="server" OnClick="Button1_Click" Text="Button" Style="display: none" />
                </asp:Panel>
            </div>

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

                <img src="../assets/img/ajax-loader.gif" alt="Processing..." />
            </asp:Panel>

        </ProgressTemplate>
    </asp:UpdateProgress>


               
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
                <%--<asp:RequiredFieldValidator runat="server" ID="StartDateReq" ControlToValidate="Start_Date"
                    Display="None" ErrorMessage="<b>Required Field Missing</b><br />Start Date is required." />
                <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="StartDateReqE" Width="30%"
                    TargetControlID="StartDateReq" />
                <asp:RequiredFieldValidator runat="server" ID="EndDateReq" ControlToValidate="End_Date"
                    Display="None" ErrorMessage="<b>Required Field Missing</b><br />End Date is required." />
                <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="EndDateReqE" Width="30%"
                    TargetControlID="EndDateReq" />--%>
                    <%-- <asp:RegularExpressionValidator ID="StartDateRegEx" runat="server" Display="None"
                    ErrorMessage="<b>Valid Start Date is required.</b>" ControlToValidate="Start_date"
                    ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$">
                </asp:RegularExpressionValidator>
                <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidStartDateReq" Width="30%"
                    TargetControlID="StartDateRegEx" />
                <asp:RegularExpressionValidator ID="EndDateRegEx" runat="server" Display="None" ErrorMessage="<b>Valid End Date is required.</b>"
                    ControlToValidate="End_Date" ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$">
                </asp:RegularExpressionValidator>
                <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender1"
                    Width="30%" TargetControlID="EndDateRegEx" />--%>

</asp:Content>
