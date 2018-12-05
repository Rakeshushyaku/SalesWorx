<%@ Page Title="Admin RoutePlan" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" enableEventValidation="true" EnableSessionState="True"
    CodeBehind="AdminRoutePlan.aspx.vb" Inherits="SalesWorx_BO.AdminRoutePlan" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="FeaturedContent" runat="server">
    <%--<script type="text/javascript" src="../js/RoutePlannerNew.js"></script>--%>

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
             if (postBackElement.id == 'ctl00_ContentPlaceHolder1_SaveBtn') {
                 $get('ctl00_ContentPlaceHolder1_UpdateProgress1').style.display = 'block';
             }

             if (postBackElement.id == 'ctl00_ContentPlaceHolder1_ApprovalBtn')
                 $get('ctl00_ContentPlaceHolder1_UpdateProgress1').style.display = 'block';

             if (postBackElement.id == 'ctl00_ContentPlaceHolder1_UpdateBtn')
                 $get('ctl00_ContentPlaceHolder1_UpdateProgress1').style.display = 'block';

             if (postBackElement.id == 'ctl00_ContentPlaceHolder1_Button1')
                 $get('ctl00_ContentPlaceHolder1_UpdateProgress2').style.display = 'block';

             postBackElement.disabled = true;
         }



         function EndRequest(sender, args) {
             if (postBackElement.id == 'ctl00_ContentPlaceHolder1_SaveBtn') {
                 $get('ctl00_ContentPlaceHolder1_UpdateProgress1').style.display = 'none';
             }
             if (postBackElement.id == 'ctl00_ContentPlaceHolder1_ApprovalBtn')
                 $get('ctl00_ContentPlaceHolder1_UpdateProgress1').style.display = 'none';

             if (postBackElement.id == 'ctl00_ContentPlaceHolder1_UpdateBtn')
                 $get('ctl00_ContentPlaceHolder1_UpdateProgress1').style.display = 'none';

             if (postBackElement.id == 'ctl00_ContentPlaceHolder1_Button1')
                 $get('ctl00_ContentPlaceHolder1_UpdateProgress2').style.display = 'none';
                         postBackElement.disabled = false;
             
         }

         function ValidateMessage(button, args)
         {
            
             if (document.getElementById('MainContent_CommentsTxt').value == '') {
                 radalert('Please add the comment!', 330, 180, 'Validation', alertCallBackFn);
                 args.set_cancel(true);
             }
         }

         function OpenWindow(URL,IsHoliday) {
             // alert(URL);
             var oWnd = radopen(URL, null);
             oWnd.SetSize(1024, 600);
             oWnd.set_behaviors(4); //Close:4
             oWnd.SetModal(true);
             oWnd.Center;
             oWnd.set_visibleStatusbar(false)

             if (IsHoliday == 1) // If selected date is holiday
                radalert('Selected day is holiday however you can still plan the visit(s).', 330, 180, 'Information', alertCallBackFn);
         }

         function OpenViewWindow(URL) {
            
             // alert(URL);
             var oWnd = radopen(URL, null);
             oWnd.SetSize(450, 500);
             oWnd.set_behaviors(4); //Close:4
             oWnd.SetModal(true);
             oWnd.Center;
             oWnd.set_visibleStatusbar(false)

            
         }

         function SetValue(ctrl, val)
         {
             var ctrl1 = document.getElementById(ctrl);
             ctrl1.value = val;
         }

         function ButtonClick()
         {
             document.getElementById('MainContent_Button1').click();
         }

         function alertCallBackFn(arg) {
                         
            
             document.getElementById("ctl00_MainContent_SaveBtn").style.display = "none";

             HideRadWindow()
         }

         function HideRadWindow() {

             var elem = $('a[class=rwCloseButton');

             if (elem != null && elem != undefined) {
                 $('a[class=rwCloseButton')[0].click();
             }

             $("#frm").find("iframe").hide();
         }
</script>
    <style>
     .RadWindow_Simple.rwShadow {
        top: 15px !important;
     }
     div[id^="RadWindowWrapper_alert"].RadWindow_Simple.rwShadow {
         top: 50% !important;
        margin-top: -75px;
     }
   </style>

        
  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
        <input type="hidden" name="RP_ID" id="RP_ID" value="" runat="server" />
         <input type="hidden" name="FSR_ID" id="FSR_ID" value="" runat="server" />
        <input type="hidden" name="Route_ID" value="<%--<%=Route_ID%>--%>" />
        <input type="hidden" name="IsApproved" value="" id="IsApproved" runat="server" />
       <input type="hidden" name="Action_Mode" id="Action_Mode" runat="server" />
        <input type="hidden" name="CheckRefresh" id="CheckRefresh" runat="server" />
           <input type="hidden" name="FromPopUp" id="FromPopUp" runat="server" />
           <input type="hidden" name="DayRef" id="DayRef" runat="server" />
        <input type="hidden" name="HDay1" ID="HDay1" value="" runat="server" />
        <input type="hidden" name="HDay2" ID="HDay2" value="" runat="server"/>
        <input type="hidden" name="HDay3" ID="HDay3" value="" runat="server"/>
        <input type="hidden" name="HDay4" id="HDay4" value="" runat="server"/>
        <input type="hidden" name="HDay5" id="HDay5" value="" runat="server"/>
        <input type="hidden" name="HDay6" id="HDay6" value="" runat="server"/>
        <input type="hidden" name="HDay7" id="HDay7" value="" runat="server"/>
        <input type="hidden" name="HDay8" id="HDay8" value="" runat="server"/>
        <input type="hidden" name="HDay9" id="HDay9" value="" runat="server"/>
        <input type="hidden" name="HDay10" id="HDay10" value="" runat="server"/>
        <input type="hidden" name="HDay11" id="HDay11" value="" runat="server"/>
        <input type="hidden" name="HDay12" id="HDay12" value="" runat="server"/>
        <input type="hidden" name="HDay13" id="HDay13" value="" runat="server"/>
        <input type="hidden" name="HDay14" ID="HDay14" value="" runat="server" />
        <input type="hidden" name="HDay15" ID= "HDay15" value="" runat="server"/>
        <input type="hidden" name="HDay16" ID="HDay16" value="" runat="server"/>
        <input type="hidden" name="HDay17" id="HDay17" value="" runat="server"/>
        <input type="hidden" name="HDay18" id="HDay18" value="" runat="server"/>
        <input type="hidden" name="HDay19" id="HDay19" value="" runat="server"/>
        <input type="hidden" name="HDay20" id="HDay20" value="" runat="server"/>
        <input type="hidden" name="HDay21" id="HDay21" value="" runat="server"/>
        <input type="hidden" name="HDay22" id="HDay22" value="" runat="server"/>
        <input type="hidden" name="HDay23" id="HDay23" value="" runat="server"/>
        <input type="hidden" name="HDay24" id="HDay24" value="" runat="server"/>
        <input type="hidden" name="HDay25" id="HDay25" value="" runat="server"/>
        <input type="hidden" name="HDay26" id="HDay26" value="" runat="server"/>
        <input type="hidden" name="HDay27" ID="HDay27" value="" runat="server" />
        <input type="hidden" name="HDay28" ID= "HDay28" value="" runat="server"/>
        <input type="hidden" name="HDay29" ID="HDay29" value="" runat="server"/>
        <input type="hidden" name="HDay30" id="HDay30" value="" runat="server"/>
        <input type="hidden" name="HDay31" id="HDay31" value="" runat="server"/>
        <input type="hidden" name="Cell1" id="Cell1" value="" runat="server"/>
        <input type="hidden" name="Cell2" id="Cell2" value="" runat="server"/>
        <input type="hidden" name="Cell3" id="Cell3" value="" runat="server"/>
        <input type="hidden" name="Cell4" id="Cell4" value="" runat="server"/>
        <input type="hidden" name="Cell5" id="Cell5" value="" runat="server"/>
        <input type="hidden" name="Cell6" id="Cell6" value="" runat="server"/>
        <input type="hidden" name="Cell7" id="Cell7" value="" runat="server"/>
        <input type="hidden" name="Cell8" id="Cell8" value="" runat="server"/>
        <input type="hidden" name="Cell9" id="Cell9" value="" runat="server"/>
        <input type="hidden" name="Cell10" id="Cell10" value="" runat="server"/>
        <input type="hidden" name="Cell11" id="Cell11" value="" runat="server"/>
        <input type="hidden" name="Cell12" id="Cell12" value="" runat="server"/>
        <input type="hidden" name="Cell13" id="Cell13" value="" runat="server"/>
        <input type="hidden" name="Cell14" id="Cell14" value="" runat="server"/>
        <input type="hidden" name="cell15" id="Cell15" value="" runat="server"/>
        <input type="hidden" name="Cell16" id="Cell16" value="" runat="server"/>
        <input type="hidden" name="Cell17" id="Cell17" value="" runat="server"/>
        <input type="hidden" name="Cell18" id="Cell18" value="" runat="server"/>
        <input type="hidden" name="Cell19" id="Cell19" value="" runat="server"/>
         <input type="hidden" name="Cell20" id="Cell20" value="" runat="server"/>
         <input type="hidden" name="Cell21" id="Cell21" value="" runat="server"/>
         <input type="hidden" name="Cell22" id="Cell22" value="" runat="server"/>
         <input type="hidden" name="Cell23" id="Cell23" value="" runat="server"/>
         <input type="hidden" name="Cell24" id="Cell24" value="" runat="server"/>
         <input type="hidden" name="Cell26" id="Cell26" value="" runat="server"/>
         <input type="hidden" name="Cell27" id="Cell27" value="" runat="server"/>
         <input type="hidden" name="Cell25" id="Cell25" value="" runat="server"/>
          <input type="hidden" name="Cell28" id="Cell28" value="" runat="server"/>
         <input type="hidden" name="Cell29" id="Cell29" value="" runat="server"/>
          <input type="hidden" name="Cell30" id="Cell30" value="" runat="server"/>
         <input type="hidden" name="Cell31" id="Cell31" value="" runat="server"/>
  

        <h4>
        <asp:Label ID="HeaderLbl" runat="server"></asp:Label></h4>
    <telerik:RadAjaxLoadingPanel ID="AjaxLoadingPanel1" runat="server">
        <div id="ProgressPanel" class="overlay">

            <img src="../assets/img/ajax-loader.gif" alt="Processing..." />
              <span>Processing... </span>
        </div>
    </telerik:RadAjaxLoadingPanel>

    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" Skin="Simple" EnableShadow="true"   >
    </telerik:RadWindowManager>
    <telerik:RadWindowManager ID="RadWindowManager2" runat="server" Skin="Simple" EnableShadow="true" Height="180px"   >
    </telerik:RadWindowManager>

       <div class="row">
            <asp:Panel ID="SalesRepPanel" runat ="server">
                <div class="col-sm-3">
                    <div class="form-group">
                         <label>Van    </label>

                        <div class="text-primary"><strong><asp:Literal ID="SalesRep_Name" runat="server" ></asp:Literal></strong></div>
                    </div>
                </div>
            </asp:Panel>
                <div class="col-sm-3">
                    <div class="form-group">
                       <label>Description    </label>



                       <div class="text-primary"><strong><asp:Literal ID="Description" runat="server" ></asp:Literal></strong></div>
                    </div>
                </div>
                 <div class="col-sm-3">
                    <div class="form-group">
                       <label>Period   </label>
                        <div class="text-primary"><strong><asp:Literal ID="Start_Date" runat="server"></asp:Literal>&nbsp; to &nbsp;<asp:Literal ID="End_Date" runat="server"></asp:Literal></strong></div>
                        </div>
                     </div>
                     <div class="col-sm-3">

                    <div class="form-group">
                         <asp:Panel ID="AddandSavePanel" runat="server" Visible="false">
        <div class="form-group">
                  
           <telerik:RadButton ID="SaveBtn" Skin="Simple" runat="server" Text="Save" CssClass="btn btn-success"   />
            
            <input name="button0" type="button" class="btn btn-default" onclick="window.location.href = 'CreateRoutePlan.aspx'" value="Cancel" />
        </div>
    </asp:Panel>

    <asp:Panel ID="ModifyAndUpdatePanel" runat="server" Visible="false">
        <div class="form-group">
            <telerik:RadButton ID="UpdateBtn" Skin="Simple" runat="server" Text="Update" CssClass="btn btn-success" />
            <input name="button0" type="button" class="btn btn-default" onclick="window.location.href = 'ModDelRoutePlan.aspx'" value="Cancel" />
        </div>

    </asp:Panel>

    <asp:Panel ID="ApprovalPanel" runat="server" Visible="false">
        <div class="form-group">
            <telerik:RadButton ID="ApprovalBtn" Skin="Simple" runat="server" Text="Approve" CssClass="btn btn-success" />
            <input name="button0" type="button" class="btn btn-default" onclick="window.location.href = 'PlansForApproval.aspx'" value="Cancel">
        </div>

    </asp:Panel>

    <asp:Panel ID="ReviewPanel" runat="server" Visible="false">
        <div class="form-group">
            <telerik:RadButton ID="ReviewButton" Skin="Simple" runat="server" Text="Go Back" CssClass="btn btn-default" />

        </div>

    </asp:Panel>
</div>
                         </div>
                       

                        
                   

                    </div>
    <div class="form-group">         
    <div class="table-responsive">

        <asp:UpdatePanel ID="UpdateCal" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
             <asp:Panel ID="CalPanel" runat="server" Visible="false" 
                    CssClass="TblCalendarCntrl" >
                    <asp:Calendar DayHeaderStyle-CssClass="hdrCalendarCntrl" CssClass="calendar" 
                        ID="DefPlanCalendar" runat="server" ShowGridLines="True" 
                    ShowNextPrevMonth="False" Width="100%" CellPadding="0" 
                    BorderColor="#666666" BorderStyle="Solid" BorderWidth="1px" ShowTitle="False" 
                        BackColor="White" EnableViewState="False">
                    <DayStyle CssClass="txtCalDate" Height="90px" BorderColor="#666666" BorderStyle="Solid" BorderWidth="1px" 
                        VerticalAlign="Top" Width="130px" />
                    <DayHeaderStyle BackColor="#ffffff" BorderColor="#666666" BorderStyle="Solid" BorderWidth="1px" Font-Bold="True" HorizontalAlign="Center" 
                        VerticalAlign="Middle" />
                 </asp:Calendar>
                          <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" style="display:none" /> 
                   </asp:Panel>
             </ContentTemplate>
           
          </asp:UpdatePanel>
</div>
        </div>
   <div class="form-group">
        <p id="note"><strong>Note:</strong> Hold on mouse pointer over a cell to view complete details for the day.</p>
        <p id="sp" runat="server" visible="false">Marked as assign/modification of visits should be greater than this date.</p>
    </div>
   
     
    <div id="commenttbl">

        <asp:UpdatePanel UpdateMode="Conditional" ID="CommentsUpdatePanel" runat="server">
            <ContentTemplate>
                <div class="row">
                <div class="col-sm-6">
                <div class="form-group">
                    <label>Add Comments </label>

                    <asp:TextBox
                        ID="CommentsTxt" runat="server" Height="90px" TextMode="MultiLine"
                        class="input" cols="50" Rows="5" Width="100%"></asp:TextBox>

                </div>

                <div class="form-group">
                    <telerik:RadButton ID="SendCommentsBtn" Skin="Simple" runat="server" Text="Send Comments" CssClass="btn btn-primary"  OnClientClicking  ="ValidateMessage" />
                </div>
                </div>
                </div>


            </ContentTemplate>
        </asp:UpdatePanel>


    </div>


           
      
	
  	<div id="commenttbl1">
	      
           
           <div class="row">
                <div class="col-sm-6">  
                   
           
               
          
          <asp:Panel runat="server" ID="LnkPanel" Visible="false">
              <div class="form-group"><asp:LinkButton ID="ShowCommLnk" runat="server" CssClass="showcomment" >Show Comments</asp:LinkButton>&nbsp;/&nbsp;<asp:LinkButton ID="HideCommLink" 
                  runat="server" CssClass="hidecomment">Hide Comments</asp:LinkButton>
                    </asp:Panel></div>
    <asp:UpdatePanel ID="ShowCommLnkPanel" runat="server">
        <ContentTemplate>
            <asp:GridView Width="100%" ID="CommentsGridView"
                runat="server" Visible="False"
                AutoGenerateColumns="False"  CellPadding="0" CellSpacing="0" CssClass="tablecellalign" EnableViewState="False">
                <Columns>
                    <asp:BoundField DataField="Message_Date" HeaderText="Date"
                        DataFormatString="{0:dd-MM-yyyy HH:mm}">
                        <ItemStyle />
                    </asp:BoundField>
                    <asp:BoundField DataField="Message_Content" HeaderText="Message" />
                </Columns>
                <HeaderStyle  HorizontalAlign="Left" />
            </asp:GridView>
            <asp:Label Visible="false" CssClass='txtSM' ID="NoCommentsLbl" runat="server" Text="There is no comments for this Plan"></asp:Label>
           
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ShowCommLnk" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="HideCommLink" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="SaveBtn" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="UpdateBtn" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="ApprovalBtn" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>        
           
             
          <asp:UpdateProgress ID="UpdateProgress1"  AssociatedUpdatePanelID="ShowCommLnkPanel" runat="server">
     <ProgressTemplate>

          
 <asp:Panel ID="Panel1" CssClass="overlay" runat="server">
  <img src="../assets/img/ajax-loader.gif" alt="Processing..."  />            
           <span>Processing... </span>
</asp:Panel>
    
           </ProgressTemplate>
            </asp:UpdateProgress>  
                     <asp:UpdateProgress ID="UpdateProgress2"  AssociatedUpdatePanelID="UpdateCal" runat="server">
     <ProgressTemplate>
     <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
     
         <img src="../assets/img/ajax-loader.gif" alt="Processing..."  />            
           <span>Processing... </span>
       </asp:Panel>
          
       
         
    </ProgressTemplate>
            </asp:UpdateProgress>      
          
	
	</div>
               </div>
          </div>
	
	  
       
     
	
</asp:Content>
