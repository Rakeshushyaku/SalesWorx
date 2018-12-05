<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="ManagePushNotification.aspx.vb" Inherits="SalesWorx_BO.ManagePushNotification" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Namespace="FilterCustomEditors" TagPrefix="custom" %>

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
            $get('<%=UpdateProgress1.ClientID %>').style.display = 'block';
            postBackElement.disabled = true;
        }

        function EndRequest(sender, args) {
            $get('<%=UpdateProgress1.ClientID %>').style.display = 'none';
            postBackElement.disabled = false;
        }

    </script>
   

    <script type="text/javascript">


        function OnClientItemChecked(sender, eventArgs) {

            sender.hideDropDown();


        }



        function alertCallBackFn(arg) {

        }



    </script>

      

       <script type="text/javascript">



           function ConfirmDelete(msg, event) {

               var ev = event ? event : window.event;
               var callerObj = ev.srcElement ? ev.srcElement : ev.target;
               var callbackFunctionConfirmDelete = function (arg, ev) {
                   if (arg) {
                       callerObj["onclick"] = "";
                       if (callerObj.click) callerObj.click();
                       else if (callerObj.tagName == "A") {
                           try {
                               eval(callerObj.href)
                           }
                           catch (e) { }
                       }
                   }
               }
               radconfirm(msg, callbackFunctionConfirmDelete, 330, 100, null, 'Confirmation');
               return false;
           }







    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function lengthInUtf8Bytes() {
            // Matches only the 10.. bytes that are non-initial characters in a multi-byte sequence.

            var str;
            var str1;
            // str = document.getElementById("ctl00_ContentPlaceHolder1_txt_Msg_text").value
            var txtMsg = $find('<%= txt_Msg.ClientID %>');
            str = txtMsg.get_textBoxValue()

            var txtTitle = $find('<%= txtTitle.ClientID%>');
            str1 = txtTitle.get_textBoxValue();

            var m = encodeURIComponent(str).match(/%[89ABab]/g);
            var n = encodeURIComponent(str1).match(/%[89ABab]/g);
            m = m + n;
            var l

            l = parseInt(document.getElementById('<%= htotal.ClientID%>').value) - parseInt((str.length + str1.length + (m ? m.length : 0)))
            //document.getElementById("ctl00_ContentPlaceHolder1_txt_Left").value = 'Characters Left: ' + l.toString()
        var txtLeft = $find('<%= txt_Left.ClientID %>');
            txtLeft.set_textBoxValue('Characters Left: ' + l.toString());
        }
 </script>

    <h4>Send Push Notification</h4>



    <telerik:RadAjaxLoadingPanel ID="AjaxLoadingPanel1" runat="server">
        <div id="ProgressPanel" class="overlay">

            <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
        </div>
    </telerik:RadAjaxLoadingPanel>

    <asp:Panel ID="Panel1" runat="server"></asp:Panel>




    <telerik:RadWindowManager ID="RadWindowManager1" Skin="Simple" runat="server" EnableShadow="true">
    </telerik:RadWindowManager>

     
                                                  
                                              
    <asp:UpdatePanel runat="server" ID="TopPanel" UpdateMode="Conditional">
        <contenttemplate>
              
                                        

        <asp:Label runat ="server"  ID="lbl_length"  visible="false"   font-size="12px"   
                            ForeColor="Darkmagenta" Text="Note : the combined length of Message and OtherInfo should not exceed 195 characters."></asp:Label>
               
               
                     <div class="form-group">
 <label>Title *   <asp:RequiredFieldValidator ID="rfvHoldType" runat="server" ControlToValidate="txtTitle"
                                   errorMessage=""  ForeColor ="Red"     Font-Bold="true"  ValidationGroup="valsum"></asp:RequiredFieldValidator>  </label> 
                   
                                          <telerik:RadTextBox ID="txtTitle" Skin="Simple"  
                                       
                                          Width ="30%"   MaxLength ="50"        TabIndex ="2"  
                                       runat="server"     />
                         
                              
                </div>
         

          
                
                    <div class="form-group">
 <label>Description *   <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txt_Msg"  
                                   errorMessage=""  ForeColor ="Red"     Font-Bold="true"  ValidationGroup="valsum"></asp:RequiredFieldValidator> </label> 
                   
                                          <telerik:RadTextBox ID="txt_Msg" Skin="Simple" MaxLength ="145"  
                                       
                                       Width ="40%"  TextMode ="MultiLine"       TabIndex ="2" 
                                       runat="server"     />
                          
                              <%--   <telerik:RadTextBox ID="lbl_length"   runat="server" Readonly="True"   Width="150px" 
                            ForeColor="Maroon" Text="* Max length (195)"></telerik:RadTextBox>--%>
                            <telerik:RadTextBox ID="txt_Left" runat="server" Visible ="false"    Readonly="True"
                             Width="150px" > </telerik:RadTextBox>
                                            <asp:HiddenField ID="htotal" runat="server" Value="195" /> 
                </div>
           
            

                         <div class="form-group">
           <label>Recipient Type *    <asp:CompareValidator ID="CompareValidatorRadComboxBoxTables" runat="server" ValidationGroup="valsum"
 ValueToCompare="--Select--" Operator="NotEqual" ControlToValidate="ddlReceType" ForeColor ="Red"   errorMessage=""  Font-Bold="true" /> </label> 
                   
                                  <telerik:RadComboBox ID="ddlReceType" Skin="Simple"   AutoPostBack="true" 
                                       
                                          Width ="30%"   Height ="250px"       TabIndex ="1" 
                                       runat="server"     />
                               
                            </div> 
                      
                <div class="form-group" id="sendto" runat ="server" >
           <label>Send To *     </label> 
                   
                               <telerik:RadComboBox ID="ddlFSR" Skin="Simple"     
                                      TabIndex="4"  runat="server" 
                                       
                                    CheckBoxes="true"  Filter ="Contains" 
                                              
                                                EnableCheckAllItemsCheckBox="true"  Localization-CheckAllString ="Select All"
                                                EmptyMessage="Please Select"  
                                       
                                             Height="280"  width="40%"  >
                                            </telerik:RadComboBox>
                               
                            </div> 
        
              <div class="form-group">
           
                  </div> 
          
                    <div class="form-group">
                        <label style="visibility: hidden;">1 </label>
                              
                          <telerik:RadButton ID="btnSave" Skin ="Simple" ValidationGroup="valsum"   runat="server" Text="Save" CssClass ="btn btn-primary" />
                                                           <telerik:RadButton ID="btnCancel"  Skin ="Simple"   runat="server" Text="Cancel" CssClass ="btn btn-warning" />
                    </div>
               
           

                
             
              
          
            

                         </contenttemplate>
    </asp:UpdatePanel>







    <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="TopPanel"
        runat="server">
        <progresstemplate>
                        <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                           <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index:10010;vertical-align:middle;" />   
                        </asp:Panel>
                    </progresstemplate>
    </asp:UpdateProgress>


</asp:Content>
