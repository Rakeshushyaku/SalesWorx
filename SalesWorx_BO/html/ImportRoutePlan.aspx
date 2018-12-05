<%@ Page Title="Import Route Plan" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="ImportRoutePlan.aspx.vb" Inherits="SalesWorx_BO.ImportRoutePlan" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
 
         <script language="javascript" type="text/javascript">
             var prm = Sys.WebForms.PageRequestManager.getInstance();

             prm.add_initializeRequest(InitializeRequest);
             prm.add_endRequest(EndRequest);
             var postBackElement;
             function InitializeRequest(sender, args) {

                 if (prm.get_isInAsyncPostBack())
                     args.set_cancel(true);
                 postBackElement = args.get_postBackElement();
     //            if (postBackElement.id == 'ctl00_ContentPlaceHolder1_Route_FSR_ID') {
                     $get('<%=UpdateProgress1.ClientID %>').style.display = 'block';
                     postBackElement.disabled = true;
      //           }
            
             }

             function EndRequest(sender, args) {
         //        if (postBackElement.id == 'ctl00_ContentPlaceHolder1_Route_FSR_ID') {
                     $get('<%= UpdateProgress1.ClientID %>').style.display = 'none';
                     postBackElement.disabled = false;
          //       }
            } 

</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
     
     <h4>Import Route Plan</h4>
     <telerik:RadAjaxLoadingPanel  ID="AjaxLoadingPanel1" runat="server" >
             <div ID="ProgressPanel" class ="overlay"  >
     
         <img src="../assets/img/ajax-loader.gif" alt="Processing..."  />   
                 <span>Processing... </span>   
       </div>
           </telerik:RadAjaxLoadingPanel>  


      <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
     </telerik:RadWindowManager>
   
    
          <input type="hidden" name="Action_Mode" value="" />

	   <asp:UpdateProgress ID="UpdateProgress1"  AssociatedUpdatePanelID="UpPanel" runat="server">
          <ProgressTemplate>

              

          <div class ="overlay">
         <img src="../images/Progress.gif" alt="Processing..." />            
           <span>Processing... </span></div>
           </ProgressTemplate>
           </asp:UpdateProgress>

    <asp:Button ID="btnHidden" runat="Server" Style="display: none" />
                                                 <ajaxToolkit:ModalPopupExtender ID="MpInfoConfirm" runat="server" BackgroundCssClass="modalBackground"
                                                    TargetControlID="btnHidden" PopupControlID="pnlConfirm" 
                                                    Drag="true" />
                                                <asp:Panel ID="pnlConfirm" Width="550" height="180px" runat="server" CssClass="modalPopup" Style="display: none" >
                                                    <div class="panelouterblk">
                                                        <asp:Panel ID="Panel1" runat="server" CssClass="popupbartitle">
                                            Confirmation</asp:Panel>
                                                           
                                                     <asp:ImageButton ID="ImageButton1" runat="server" Visible="false"  ImageUrl="~/assets/img/close.jpg" CssClass="Closebtnimg"></asp:ImageButton>  

                                                    <table id="tableinPopupErr" width="540" cellpadding="10" style="background-color: White;">
                                                        <tr align="center">
                                                            <td align="center" style="cursor: move; background-color: #3399ff; text-align: center;
                                                                border: solid 1px #3399ff; color: White; padding: 3px">
                                                                  <asp:Label ID="lblinfo" Text="The file contains routeplan for current month.<br/> Would you like to import  ?"   runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                         <tr><td>&nbsp;</td></tr>
                                                        <tr>
                                                            <td align="center" style="text-align: center;">
                                                            <asp:Button ID="btnUpdate"  CssClass="btn btn-success" Text="Import" runat="server" />
                                                            <asp:Button ID="btnCancel"  CssClass="btn btn-default" runat="server" Text="Cancel" />
                                                          </td>
                                                        </tr>
                                                    </table>
                                                        </div>
                                                </asp:Panel>

     <div class="row">
        <div class="col-sm-6">
          <div class="form-group">  
                 <label>Import By </label>
                    <asp:RadioButtonList ID="rdo_Format" runat="server" RepeatDirection="Horizontal" CellPadding="2" CellSpacing="15" RepeatColumns="2" AutoPostBack="true"  >
                        <asp:ListItem Selected="True" Value="D">Date</asp:ListItem>
                        <asp:ListItem Value="Y">Days</asp:ListItem>
                 </asp:RadioButtonList>
              </div>
            </div>
        </div>
     <div class="row" id="divDefaultPlan" runat="server" visible="false"  >
        <div class="col-sm-6">
          <div class="form-group">  
                 <label>Default Plan </label>
                     <telerik:RadComboBox 
                    ID="Default_Plan_DD" runat="server" DataTextField="Details"  Skin="Simple" 
                    DataValueField="Default_Plan_ID" TabIndex ="2" Width ="100%"  Filter="Contains">
                </telerik:RadComboBox>
              </div>
            </div>
        </div>
    <div class="row" id="divformat" runat="server" >
        <div class="col-sm-6">
          <div class="form-group">  
                 <label>Date Format </label>
                    <asp:RadioButtonList ID="Rdo_Type" runat="server" RepeatDirection="Horizontal" CellPadding="2" CellSpacing="15" RepeatColumns="2" >
                        <asp:ListItem Selected="True" Value="1">Single Column</asp:ListItem>
                        <asp:ListItem Value="3">Splitted Colums</asp:ListItem>
                 </asp:RadioButtonList>
              </div>
            </div>
        </div>
        <div class="row">
        <div class="col-sm-6">
            <div class="form-group">
              <label> Select a File </label>
              <asp:FileUpload ID="ExcelFileUpload" runat="server" />
                </div>
            </div>

        </div>
    
        <div class="form-group">  
            <telerik:RadButton ID="BtnImport" runat="server" Text="Import" CssClass="btn btn-warning" Skin ="Simple" />
               <asp:Button ID="DummyImBtn" style="display:none"  runat="server" Text="Reimport" />

              <telerik:RadButton ID="BtnReimport" runat="server" Text="Reimport" 
                 CssClass="btn btn-warning"  Skin ="Simple" Visible="false"   />

                <telerik:RadButton ID="DummyReimBtn" style="display:none"  runat="server" Text="Reimport" Visible="false"/>
            &nbsp;
             <asp:LinkButton id="BtnDownLoad" runat="server" Text="Download Error Log" Visible="false" ></asp:LinkButton>
        </div>

          <div class="form-group">  

              <asp:UpdatePanel runat="server" ID="UpPanel">
	            <ContentTemplate>
                    <div id="divResult" runat="server" class="overflowx">
                        <asp:Label ID="lblResult" runat="server"></asp:Label>       
                    </div>
    	        </ContentTemplate>
	            <Triggers>
	                <asp:AsyncPostBackTrigger ControlID="DummyImBtn" EventName="Click" />
	                <asp:AsyncPostBackTrigger ControlID="DummyReimBtn" EventName="Click" />
	            </Triggers>
	        </asp:UpdatePanel>	
          </div>

  	

</asp:Content>
