<%@ Page Title="Admin RoutePlan" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master" CodeBehind="AdminRoutePlan.aspx.vb" Inherits="SalesWorx_BO.AdminRoutePlan" %>
<%@ Register
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<script type="text/javascript" src="../js/RoutePlanner.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
  
            <script language="javascript" type="text/javascript">


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

</script>
        <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">


	<tr>
	
	<td id="contentofpage" width="100%" height="76%" class="topshadow">
	<div class="pgtitileposition">
	<span class="pgtitile3"><asp:Label ID="HeaderLbl" runat="server"></asp:Label></span></div>
	<div id="pagenote" > This screen may be used to modify/delete route plans for each of the vans. </div>
		
	
	<table width="auto" border="0" cellspacing="0" cellpadding="0" id="tableform">
	<tr><td colspan="3" style="height:10px";> </td></tr>
	 <asp:Panel ID="SalesRepPanel" runat ="server">
          <tr> 
            <td colspan="2" ><label>Van:</label>&nbsp;<asp:Literal ID="SalesRep_Name" runat="server" ></asp:Literal></td>
          </tr>
    </asp:Panel>
	
	   <tr> 
            <td colspan="2"  ><label>Description:</label>&nbsp;<asp:Literal ID="Description" runat="server" ></asp:Literal></td>
          </tr>
          <tr> 
            <td ><label>Period:</label>&nbsp;<asp:Literal ID="Start_Date" runat="server"></asp:Literal>&nbsp;<span class="txtBold">to</span>&nbsp;<asp:Literal ID="End_Date" runat="server" ></asp:Literal></td>
             <asp:Panel ID="AddandSavePanel" runat="server" Visible="false">
            <td style="padding:3px 0px; text-align:right;"><asp:Button
                    ID="SaveBtn" runat="server" Text="Save"  CssClass="btnInputGreen"  />
            <input name="button0" type="button" class="btnInputRed" onclick="window.location.href='CreateRoutePlan.aspx'" value="Cancel"/>
            </td>
          </asp:Panel>
            <asp:Panel ID="ModifyAndUpdatePanel" runat="server" Visible="false">
            <td style="padding:3px 0px; text-align:right;">
            <asp:Button ID="UpdateBtn" runat="server" Text="Update"  CssClass="btnInputGreen"  />
            <input name="button0" type="button" class="btnInputRed" onclick="window.location.href='ModDelRoutePlan.aspx'" value="Cancel" />
            </td>
          </asp:Panel>
            <asp:Panel ID="ApprovalPanel" runat="server" Visible="false">
            <td style="padding:3px 0px;text-align:right;" ><asp:Button ID="ApprovalBtn" runat="server" Text="Approve" CssClass="btnInputGreen" />
            <input name="button0" type="button" class="btnInputRed" onclick="window.location.href='PlansForApproval.aspx'" value="Cancel">
            </td>
          </asp:Panel>
          </tr>
        
           <tr><td colspan="2" style="height:10px"></td></tr>
            <br />
          <tr> 
       
        
            <td colspan="2" >
            <asp:UpdatePanel ID="UpdateCal" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
             <asp:Panel ID="CalPanel" runat="server" Visible="false" 
                    CssClass="TblCalendarCntrl" >
                    <asp:Calendar DayHeaderStyle-CssClass="hdrCalendarCntrl" CssClass="calendar" 
                        ID="DefPlanCalendar" runat="server" ShowGridLines="True" 
                    ShowNextPrevMonth="False" Width="100%" CellPadding="0" 
                    BorderColor="#EDEDED" BorderStyle="Solid" BorderWidth="1px" ShowTitle="False" 
                        BackColor="White" EnableViewState="False">
                    <DayStyle CssClass="txtCalDate" Height="90px" 
                        VerticalAlign="Top" Width="130px" />
                    <DayHeaderStyle BackColor="#ddb500" Font-Bold="False" HorizontalAlign="Center" 
                        VerticalAlign="Middle" />
                 </asp:Calendar>
                          <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" style="display:none" /> 
                   </asp:Panel>
             </ContentTemplate>
           
          </asp:UpdatePanel>
            </td>
          </tr>
  
            </tr>
             <tr> 
            <td colspan="3" style='padding:10px 0px 2px 0px;' >
            <span id="note"><strong>Note</strong> : Hold on mouse pointer over a cell to view complete details for the day.</span>
            </td>
          </tr>
             <tr> 
            <td colspan="3"  style='padding:10px 0px 2px 0px;' >
            
                <span id ="sp" runat ="server" visible ="false"   style="background-color:Honeydew;font-weight:bold;"> Marked as assign/modification of visits should be greater than this date.</span>
            </td>
          </tr>
          <tr><td class='txtSM' colspan="3"></td></tr>
         
    </table>
  	<table id="commenttbl">
	      
           
            
             <asp:UpdatePanel UpdateMode="Conditional" ID="CommentsUpdatePanel" runat="server" >
              <ContentTemplate >
             
                  <tr>
            <td style="vertical-align:top;">

              <span class="txtBold" style="vertical-align:middle;">Add Comments:</span>&nbsp;
                         
                            <asp:TextBox 
                                    ID="CommentsTxt" runat="server" Height="64px" TextMode="MultiLine" 
                                  class="input" cols=50 rows=5 Width="382px"></asp:TextBox></td>
               </tr>
                  <tr>
                             
                            <td valign="middle" align=right>
                                <asp:Button ID="SendCommentsBtn" runat="server" Text="Send Comments" CssClass="btnInputGreen" /></td>
               </tr>
     
              </ContentTemplate>
              </asp:UpdatePanel>
                   
           
               <tr>
          <td>
          
          <asp:Panel runat="server" ID="LnkPanel" Visible="false">
              <asp:LinkButton ID="ShowCommLnk" runat="server" CssClass="showcomment" >Show Comments</asp:LinkButton>&nbsp;/&nbsp;<asp:LinkButton ID="HideCommLink" runat="server" CssClass="hidecomment"
                  >Hide Comments</asp:LinkButton>
           </asp:Panel>
           
           
             <asp:UpdatePanel ID="ShowCommLnkPanel" runat="server" >
              <ContentTemplate>              
                   <asp:GridView Width="100%"  ID="CommentsGridView" 
                       runat="server" Visible="False"   
                       AutoGenerateColumns="False" CellPadding="0" CellSpacing="0" CssClass="tablecellalign" EnableViewState="False">
                      <Columns>
                          <asp:BoundField DataField="Message_Date" HeaderText="Date" 
                              DataFormatString="{0:dd-MM-yyyy HH:mm}" >
                          <ItemStyle/>
                          </asp:BoundField>
                          <asp:BoundField DataField="Message_Content" HeaderText="Message" />
                      </Columns>
                       <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
                  </asp:GridView>
                    <asp:Label Visible="false" Cssclass='txtSM' ID="NoCommentsLbl" runat="server" Text="There is no comments for this FSR Plan"></asp:Label>
              </ContentTemplate>
              <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ShowCommLnk" EventName ="Click" />
                <asp:AsyncPostBackTrigger ControlID="HideCommLink" EventName ="Click" />
                 <asp:AsyncPostBackTrigger ControlID="SaveBtn" EventName ="Click" />
                   <asp:AsyncPostBackTrigger ControlID="UpdateBtn" EventName ="Click" />
                      <asp:AsyncPostBackTrigger ControlID="ApprovalBtn" EventName ="Click" />
              </Triggers>
              </asp:UpdatePanel>
                   <asp:UpdateProgress ID="UpdateProgress1"  AssociatedUpdatePanelID="ShowCommLnkPanel" runat="server">
     <ProgressTemplate>
        <%-- <div style="z-index:9999; position:absolute; top:80%; left:48%;">
         <img src="../images/Progress.gif" alt="Processing..." style="vertical-align:middle;" />            
           <span style="font-size:12px; font-weight:700px; color:#3399ff;">Processing... </span></div>--%>
          
 <asp:Panel ID="Panel1" CssClass="overlay" runat="server">
  <img src="../images/Progress.gif" alt="Processing..." style="vertical-align:middle;" />            
           <span style="font-size:12px; font-weight:700px; color:#3399ff;">Processing... </span>
</asp:Panel>
    
           </ProgressTemplate>
            </asp:UpdateProgress>  
                     <asp:UpdateProgress ID="UpdateProgress2"  AssociatedUpdatePanelID="UpdateCal" runat="server">
     <ProgressTemplate>
     <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
     
         <img src="../images/Progress.gif" alt="Processing..." style="vertical-align:middle;" />            
           <span style="font-size:12px; font-weight:700px; color:#3399ff;">Processing... </span>
       </asp:Panel>
           </div>
       
         
    </ProgressTemplate>
            </asp:UpdateProgress>      
          </td>
        
          </tr>
	
	</table>
	
	  </td>
    </tr>
       
     
		</table>
</asp:Content>
