<%@ Page Title="New Message" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master"
    CodeBehind="AdminMsg.aspx.vb" Inherits="SalesWorx_BO.AdminMsg" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="FeaturedContent" runat="server">

    <script type="text/javascript">

        function alertCallBackFn(arg) {

        }
    </script>
     <script language="javascript">
         function processAction() {
             history.go(-1);
         }


         function OnClientItemChecked(sender, eventArgs) {


             sender.hideDropDown();


         }

    </script>

    

    </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
       <h4>New Message</h4>
       <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
      </telerik:RadWindowManager> 

       <telerik:RadAjaxLoadingPanel  ID="AjaxLoadingPanel1" runat="server" >
             <div ID="ProgressPanel" class ="overlay"  >
     
         <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index:10010;vertical-align:middle;" />      
       </div>
           </telerik:RadAjaxLoadingPanel>
                     <telerik:RadWindowManager ID="RadWindowManager2"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager> 
     

   <asp:HiddenField runat="server" ID="ButtonStatus" />

            <telerik:RadAjaxPanel runat ="server" ID="rap">

                            <asp:Panel ID="TopPanel" runat="server">
                                <div class="row">
                                    <div class="col-sm-4">
                                   <div class="form-group">
                                              <label>Please select a date</label> 
                                          <telerik:RadDatePicker ID="MSG_Date" width="100%"  runat="server" Skin="Simple"
                                              TabIndex="2">
                                                <calendar Skin="Simple" usecolumnheadersasselectors="False" userowheadersasselectors="False"
                                                    viewselectortext="x">
                                            </calendar>
                                                <datepopupbutton hoverimageurl="" imageurl="" tabindex="0" />
                                                <dateinput readonly="true" dateformat="dd-MM-yyyy" displaydateformat="dd-MM-yyyy">
                                            </dateinput>
                                             
                                            </telerik:RadDatePicker>

                                               
                                       </div>
                                        </div>
                                  <div class="col-sm-4">
                                   <div class="form-group">
                                       <label class="hidden-xs"><br /></label>
                                        <telerik:RadButton  ID="GetMsgBtn" Skin="Simple" CssClass ="btn btn-primary"  runat="server" Text="Get Message" TabIndex ="6" >
                                      </telerik:RadButton>
                                                <telerik:RadButton CausesValidation="false" ID="NewMsgBtn" Skin="Simple" CssClass ="btn btn-success"  runat="server" Text="New Message" TabIndex ="6" >
                                      </telerik:RadButton>
                                          
                                         </div>
                                        </div>
                                </div>
                               
                            </asp:Panel>
                            <asp:Panel ID="MsgSelectPanel" runat="server" Visible="false">
                                <div class="row">
                                    <div class="col-sm-4">
                                <div class="form-group">                   

                                        <label>Messages for date selected</label> 


                                       <telerik:RadComboBox ID="MessageSelDD" Skin="Simple"  EnableLoadOnDemand ="true"  
                                            DataTextField="Msg_Title" DataValueField="Msg_ID"  Filter="Contains" height="200px"   TabIndex ="1" 
                                       runat="server" Width="100%" EmptyMessage ="Please type a Message">
                                    </telerik:RadComboBox> 
                                        </div>
                                        </div>
                                  <div class="col-sm-4">
                                   <div class="form-group">  
                                       <label class="hidden-xs"><br /></label>
                                       <telerik:RadButton ID="ViewandRecallBtn" Skin="Simple" CssClass ="btn btn-primary2"    ValidationGroup="G2" runat="server" Text="View/Recall" TabIndex ="6" >
                                      </telerik:RadButton>
                                        </div>
                                        </div>
                                </div>
                                
                                
                                
                            </asp:Panel>
                            <asp:Panel ID="NewMsgPanel" runat="server" Visible="false">
                                <asp:Label ID="MsgLbl" runat="server"></asp:Label>

                                 <div class="row"  >    
                                       <div class="col-sm-3"> 

                                           <div class="form-group">
                                        <label>Start Date  </label> 
                                          <telerik:RadDatePicker ID="Sel_Date" width="100%"  runat="server" Skin="Simple"
                                                  TabIndex="2">
                                                    <calendar Skin="Simple" usecolumnheadersasselectors="False" userowheadersasselectors="False"
                                                        viewselectortext="x">
                                                </calendar>
                                                    <datepopupbutton hoverimageurl="" imageurl="" tabindex="0" />
                                                    <dateinput readonly="true" dateformat="dd-MM-yyyy" displaydateformat="dd-MM-yyyy">
                                                </dateinput>
                                             
                                                </telerik:RadDatePicker>

                                         </div> 
                                       </div>
                                         <div class="col-sm-3"> 
                                            <div class="form-group">
                                              <label>Expiry Date      </label> 
                                                   <telerik:RadDatePicker ID="Expiry_Date" width="100%"  runat="server" Skin="Simple"
                                                      TabIndex="2">
                                                        <calendar Skin="Simple" usecolumnheadersasselectors="False" userowheadersasselectors="False"
                                                            viewselectortext="x">
                                                    </calendar>
                                                        <datepopupbutton hoverimageurl="" imageurl="" tabindex="0" />
                                                        <dateinput readonly="true" dateformat="dd-MM-yyyy" displaydateformat="dd-MM-yyyy">
                                                    </dateinput>
                                             
                                                    </telerik:RadDatePicker>
                                           </div>
                                         </div>
                                    </div>
                                    <div class="row"  >    
                                       <div class="col-sm-6"> 
                                   <div class="form-group">
                   

                                         <label>    Title
                
                                                   </label> 
                                      <telerik:RadTextBox ID="TitleTxt"  Width ="100%"   runat ="server" ></telerik:RadTextBox>
                                  </div> 
                                           </div>
                                        </div>
                                <div class="row"  >   
                                         <div class="col-sm-6"> 

                                  <div class="form-group">
                                         <label>     Message Text </label> 
                                       <telerik:RadTextBox ID="MsgTxt"  TextMode="MultiLine" Width ="100%"  runat ="server" ></telerik:RadTextBox>
                                   </div> 
                                    </div>
                                        </div>
                                <div class="row"  >    
                                       <div class="col-sm-10"> 
                                 <div class="form-group">
                                        <label>     Assign To </label> 
                                     <asp:RadioButtonList ID="Rdo_userType" runat="server" AutoPostBack="true" Visible="false" RepeatDirection="Horizontal"  >
                                         <asp:ListItem Text="Van/FSR" Value="V" Selected="True"></asp:ListItem>
                                         <asp:ListItem Text="Group" Value="G"></asp:ListItem>
                                     </asp:RadioButtonList>
                                <telerik:RadComboBox ID="SalesRepList" Skin="Simple"   AutoPostBack ="false"     AllowCustomText = "true"     TabIndex="4"  runat="server" 
                                       
                                    CheckBoxes="true"  Filter ="Contains"  EnableLoadOnDemand ="false"  
                                              
                                                EnableCheckAllItemsCheckBox="true"  Localization-CheckAllString="All"
                                                EmptyMessage="Please Select"  
                                       
                                             Height="280"  width="70%"  >
                                            </telerik:RadComboBox>
                                 </div>
                                        </div>
                                        </div>
                                 <div class ="form-group">
                                     <telerik:RadButton ID="SendMsgBtn" Skin="Simple"
                                                         ValidationGroup="G3"  CssClass ="btn btn-success"  runat="server" Text="Send Message" TabIndex ="6" >
                                      </telerik:RadButton>
                                      <telerik:RadButton ID="SendMsgBtn_Recall" Skin="Simple"
                                                         ValidationGroup="G3"  CssClass ="btn btn-success"  runat="server" Text="Send Message" TabIndex ="6" Visible="false"  >
                                      </telerik:RadButton>
                                         <telerik:RadButton ID="RecallBtn" Skin="Simple"
                                                        CssClass ="btn btn-primary2"  runat="server" Text="Recall" TabIndex ="6" >
                                        

                                      </telerik:RadButton>
                                         <telerik:RadButton ID="CancelBtn" Skin="Simple"
                                                         CssClass ="btn btn-default"  runat="server" Text="Cancel" TabIndex ="6" >
                                      </telerik:RadButton>
                                       <telerik:RadButton ID="BackBtn" Skin="Simple"
                                                         CssClass ="btn btn-default"  runat="server" Text="Back" TabIndex ="6" >
                                      </telerik:RadButton>
                                       <%-- <input type='button' id="BackBtn" class='btnInputGrey' runat="server" value='Back' onclick="processAction();" />   --%>
                                </div>

                      
                            </asp:Panel>
                    
   
         
        </telerik:RadAjaxPanel>
</asp:Content>
