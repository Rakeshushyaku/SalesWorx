<%@ Page Title="FSR Target Definition" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="FSRSalesTarget.aspx.vb" Inherits="SalesWorx_BO.FSRSalesTarget" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <style>
        .AlignRight
        {
            text-align: right;
        }
    </style>
    <style>
        ult .rpRootGroup
        {
            border-color: lightgrey !important;
        }

        .RadPanelBar_Default .rpTemplate
        {
            color: #000;
            font: normal 12px/24px "Segoe UI", Arial, sans-serif;
        }

        #contentofpage a:link
        {
            color: #337AB7;
            text-decoration: none !important;
        }

        RadPanelBar .rpLink, .RadPanelBar .rpOut, .RadPanelBar .rpText
        {
            display: block;
            text-decoration: none !important;
            background-color: whitesmoke;
            font: normal 12px/24px "Segoe UI", Arial, sans-serif;
            color: #0090d9;
        }

        .RadPanelBar_Default a.rpExpanded, .RadPanelBar_Default a.rpSelected, .RadPanelBar_Default div.rpFocused, .RadPanelBar_Default div.rpExpanded, .RadPanelBar_Default div.rpSelected, .RadPanelBar_Default a.rpSelected:hover
        {
            background-color: #fdfdfd;
            border-color: lightgrey !important;
        }
    </style>

    <script type="text/javascript" language="javascript">
        Sys.Application.add_load(setCalendarTable);
        function setCalendarTable(sender, eventArgs) {
            var picker = $find("<%= StartTime.ClientID %>");
            var calendar = picker.get_calendar();
            var fastNavigation = calendar._getFastNavigation();
            //Changing the Month Display Order
            fastNavigation.MonthNames[0] = "Jan";
            fastNavigation.MonthNames[1] = "Feb";
            fastNavigation.MonthNames[2] = "Mar";
            fastNavigation.MonthNames[3] = "Apr";
            fastNavigation.MonthNames[4] = "May";
            fastNavigation.MonthNames[5] = "Jun";
            fastNavigation.MonthNames[6] = "Jul";

            fastNavigation.MonthNames[7] = "Aug";

            fastNavigation.MonthNames[8] = "Sep";

            fastNavigation.MonthNames[9] = "Oct";

            fastNavigation.MonthNames[10] = "Nov";

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

                //var pos = picker.getElementPosition(adjustElement);
                x = 275;
                y = 325;

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
                           0, 1, 2,
                                  3, 4, 5,
                                  6, 7, 8,
                                  9, 10, 11
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
    </script>

    <script type="text/javascript">


      

        function alertCallBackFn(arg) {

        }
    </script>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">


    <h4>FSR Target Definition</h4>



    <telerik:RadAjaxLoadingPanel ID="AjaxLoadingPanel1" runat="server">
        <div id="ProgressPanel" class="overlay">

            <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
        </div>
    </telerik:RadAjaxLoadingPanel>

    <asp:Panel ID="Panel1" runat="server"></asp:Panel>




    <telerik:RadWindowManager ID="RadWindowManager1" Skin="Simple" runat="server" EnableShadow="true">
    </telerik:RadWindowManager>



    <asp:UpdatePanel ID="ClassUpdatePnl" runat="server" UpdateMode="conditional">
        <%--  <Triggers>
                           <asp:PostBackTrigger  ControlID="btnExport"   />
                          <asp:PostBackTrigger  ControlID="btnImportWindow"  />
                            </Triggers>--%>
        <contenttemplate>


          
                            <asp:UpdatePanel ID="ClassUpdatePnl1" runat="server">
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnExport" />
                                 

                                </Triggers>
                                <ContentTemplate>


                                        <div class="form-group" >
           <label>Organization *   </label> 
                   <div style ="float:right;padding-right:5px;">
                              <telerik:RadButton ID="btnExport" Skin="Simple" OnClick="Export_Click" runat="server" Text="Export" CssClass="btn btn-info" />

                                                    <telerik:RadButton ID="btnImportWindow" Skin="Simple" OnClick="btnImportWindow_Click" runat="server" Text="Import" CssClass="btn btn-success" />
                       </div> 
  <telerik:RadComboBox ID="ddOraganisation" Skin="Simple" AutoPostBack="true" EmptyMessage="Please type a organization"
                                                   Width="30%"  Height="250px" Filter="Contains"    TabIndex="1"
                                                    runat="server" />

                                               </div>       <div class="form-group" >
    <label>FSR *   </label> 
                                              <telerik:RadComboBox ID="ddlSalesRep" Skin="Simple" AutoPostBack="false" EmptyMessage="Please type a FSR"
                                                    Width="30%" Height="250px" Filter="Contains"    TabIndex="2"
                                                    runat="server" />
                                                </div>       <div class="form-group">
                                                    <label>Target Year & Month *</label> 
                                           
                                              <telerik:RadDatePicker ID="StartTime" Skin ="Simple" Autopostback="false"  Width="30%"          runat="server" 
                                    >
                                 <DateInput  readonly="true"  DisplayDateFormat="MMM-yyyy"></DateInput>
                                 <Calendar ID="Calendar2" runat="server">
                        <FastNavigationSettings TodayButtonCaption="Current Month"   OkButtonCaption="     OK    " />
                        
                    </Calendar>
                                </telerik:RadDatePicker>

                                              </div>      <div class="form-group" >
                                             <label>
                                       
                                           Primary Classification
                                          
                                            </label> 
                                       <asp:Label ID="lblPrimaryValue" runat="server" Font-Bold ="true" ForeColor ="DarkMagenta"></asp:Label>
                                           
                                         
                                           
                                        </div> 

                                    <div class="form-group" >
                                             <label>
                                       
                                           Secondary Classification
                                          
                                            </label> 
                                       <asp:Label ID="lblSecondary" runat="server" Font-Bold ="true" ForeColor ="DarkMagenta"></asp:Label>
                                           
                                         
                                           
                                        </div> 

                                         <telerik:RadWindow ID="DocWindow" Title ="Import FSR Target"   runat="server" skin="Windows7" Behaviors="Move,Close" 
         width="450px" height="215px"   ReloadOnShow="false"  VisibleStatusbar="false"  Overlay="true" Modal ="true"  >
               <ContentTemplate>
                        <asp:HiddenField ID="HiddenField1" runat="server" Value="-1" />
                    <table width="100%" border="0" cellspacing="2" cellpadding="2">
                  
                  <tr>
                  <td colspan ="2">
                  <asp:Label runat ="server" ID="Label7"  Font-Size ="12px" ForeColor ="Blue" 
                       Text ="Note: Uploading a FSR target data removes any existing target data for the month specified in excel file."></asp:Label>
                  <asp:Label runat ="server" ID="lblImportMode" Visible ="false" ></asp:Label>
                  </td>
                  </tr>      
                       <tr>
                           <td colspan ="2">
                               <br />
                           </td>
                       </tr>
		 <tr>
    <td class ="txtSMBold">Select a File :</td>
    <td><asp:FileUpload ID="ExcelFileUpload" runat="server" />
          
         </td>
       
          </tr>
       <tr>
                           <td colspan ="2">
                               <br />
                           </td>
                       </tr>
          <tr>
          <td>
           
             
          </td>
          <td >
         
                <telerik:RadButton ID="btnImport"  Skin ="Simple"   runat="server" Text="Import" CssClass ="btn btn-success" />
              
               <telerik:RadButton ID="btnCancelImport"  Skin ="Simple" Visible ="false"    runat="server" Text="Close" CssClass ="btn btn-danger" />
                  <asp:Button ID="DummyImBtn" style="display:none"  runat="server" Text="Reimport" />

           <asp:Button ID="BtnReimport" runat="server" Text="Reimport"  Visible ="false" 
                 CssClass="btnInputBlue" />
           <asp:Button ID="DummyReimBtn" style="display:none"  runat="server" Text="Reimport" />
             <asp:LinkButton ID="lbLog" Font-Bold ="true" Font-Size ="13px" ForeColor  ="#337AB7" ToolTip ="Click here to see the uploaded log" runat ="server" Text ="View Uploaded Log" OnClick ="lbLog_Click"></asp:LinkButton>
           </td>
          </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel runat="server" ID="UpPanel">
                                    <Triggers>
                                      <asp:AsyncPostBackTrigger ControlID="DummyImBtn" EventName="Click" />
	<asp:AsyncPostBackTrigger ControlID="DummyReimBtn" EventName="Click" />
	
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr><td colspan ="2"><asp:Label runat ="server" ID="lblUpMsg" CssClass ="txtSM" Font-Size ="12px" ForeColor ="Green" Font-Bold ="true" ></asp:Label></td></tr>
                        
                    </table>
             
            </td>
        </tr>
   </table>
                      </ContentTemplate> 
                   </telerik:RadWindow> 
                                </ContentTemplate>
                            </asp:UpdatePanel>
                     

                 
           
     

          
            <asp:GridView Width="100%" ID="dgvErros" Visible="false" runat="server" EmptyDataText="No items to display"
                EmptyDataRowStyle-Font-Bold="true" Font-Size="12px" CssClass="txtSM" AutoGenerateColumns="False"
                AllowPaging="false" AllowSorting="false" PageSize="25" CellPadding="6">
                <rowstyle bordercolor="Silver" borderstyle="Solid" borderwidth="1px" cssclass="tdstyle"
                    height="12px" wrap="True" />
                <pagersettings mode="NumericFirstLast" position="TopAndBottom" />
                <emptydatarowstyle font-bold="True" />
                <columns>

                    <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="RowNo"
                        HeaderText="Row No">
                        <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundField>

                    <%-- <asp:BoundField HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="ColNo"
                                                                HeaderText="Col No">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>
                                                           
                                                         <asp:BoundField HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="ColName"
                                                                HeaderText="Colume Name">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>--%>

                    <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="LogInfo"
                        HeaderText="Log Info">
                        <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundField>




                </columns>
                <pagerstyle cssclass="pagernumberlink" />
                <headerstyle backcolor="Silver" bordercolor="Silver" borderstyle="Solid" borderwidth="1px"
                    cssclass="headerstyle" />
            </asp:GridView>
          
        </ContentTemplate>
    </asp:UpdatePanel>






    <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="ClassUpdatePnl"
        runat="server">
        <progresstemplate>
                        <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                            <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                        </asp:Panel>
                    </progresstemplate>
    </asp:UpdateProgress>

</asp:Content>
