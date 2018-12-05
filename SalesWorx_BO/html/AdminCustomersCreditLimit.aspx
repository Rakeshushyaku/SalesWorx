
<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="AdminCustomersCreditLimit.aspx.vb" Inherits="SalesWorx_BO.AdminCustomersCreditLimit" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="FeaturedContent" runat="server">
        <script >
            function RecalAvail()
            {
                var crLimit
                crLimit = document.getElementById('<%= Me.txt_CreditLimit.ClientID%>').value;

                var crCLimit
                crCLimit = document.getElementById('<%= Me.txt_CCreditLimit.ClientID%>').value;

                var AvailBal
                AvailBal = document.getElementById('<%= Me.txt_CAvailBal.ClientID%>') 

                var NewAvailBal
                NewAvailBal = document.getElementById('<%= Me.txt_AvailBal.ClientID%>') 

                var AvailBalValue
                AvailBalValue = AvailBal.value
                if (AvailBalValue.trim() == '')
                    AvailBalValue = 0
                 
                if (parseFloat(crLimit) > parseFloat(crCLimit)) {
                    var dif
                    dif = parseFloat(crLimit) - parseFloat(crCLimit)

                    NewAvailBal.value = parseFloat(AvailBalValue) + dif
                }
                else {
                    NewAvailBal.value=''
                }

                }
            function NumericOnly(e) {

            var keycode;

            if (window.event) {
                keycode = window.event.keyCode;
            } else if (e) {
                keycode = e.which;
            }
            if ((parseInt(keycode) >= 48 && parseInt(keycode) <= 57) || parseInt(keycode) == 8 ||   parseInt(keycode) == 46 || parseInt(keycode) == 0)
                return true;

            return false;
            }
            function alertCallBackFn(arg) {
                HideRadWindow();
            }
            function HideRadWindow() {

                var elem = $('a[class=rwCloseButton');

                if (elem != null && elem != undefined) {
                    $('a[class=rwCloseButton')[0].click();
                }


            }
            </script>
    </asp:Content>
 <asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   
     <h4>Customer Credit Limit  </h4>
	  <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>

 
                        
       
    
         <div class="popupcontentblk">
             <div id="details_div">
         <asp:Panel runat="server" id="details">
              <telerik:RadAjaxPanel runat ="server" ID="rap">
        <ContentTemplate >
                 <div class="row">
                                         <div class="col-sm-6">
                                            <div class="form-group">
                                                <label>Customer No</label>  
               
                   <asp:TextBox ID="Txt_CustNo" runat="server" CssClass="inputSM" 
                       MaxLength="100" ReadOnly="true" TabIndex="2" Width="100%"></asp:TextBox>
                   </div>
                                             </div>
                     
                <div class="col-sm-6">
                                            <div class="form-group">
                                                <label>Customer Name</label>
                
                   <asp:TextBox ID="Txt_CustName" runat="server" CssClass="inputSM" 
                       MaxLength="100" TabIndex="2" Width="100%"></asp:TextBox>
                     </div>
                    </div>
                     <div class="col-sm-6">
                                            <div class="form-group">
                                                <label>
               
                 Current Credit Limit</label>
               
                 
                
                    <asp:TextBox ID="txt_CCreditLimit" runat="server" CssClass="inputSM" 
                        MaxLength="100" TabIndex="2" Width="100%"  ></asp:TextBox>
                          
           </div>
                         </div>
              
               
                    <div class="col-sm-6">
                                            <div class="form-group">
                                                <label>
                       Current Available Balance</label>
                    
                        <asp:TextBox ID="txt_CAvailBal" runat="server" CssClass="inputSM" 
                            MaxLength="100" TabIndex="2" Width="100%"></asp:TextBox>
                    </div>
                        </div>

               
               
                  
               
               
                 <div class="col-sm-6">
                                            <div class="form-group">
                                                <label> Credit Limit *</label>
                               
                               
                                   <asp:TextBox ID="txt_CreditLimit" runat="server" CssClass="inputSM"  onKeypress='return NumericOnly(event)'
                                       MaxLength="10" TabIndex="1" Width="100%" ></asp:TextBox>
                                                  <asp:RequiredFieldValidator runat="server" id="reqCreditLimit" controltovalidate="txt_CreditLimit"  ValidationGroup="Save"  errormessage="Credit limit cannot be left blank!" ForeColor="Red" />

                               </div>
                     </div>
                 <div class="col-sm-6">
                                            <div class="form-group">
                                                <label>
                                   Available Balance  *</label>
                               
                                   <asp:TextBox ID="txt_AvailBal" runat="server" CssClass="inputSM"  onKeypress='return NumericOnly(event)' MaxLength="10" 
                                       TabIndex="2" Width="100%"></asp:TextBox>
                                 <asp:RequiredFieldValidator runat="server" id="reqavailbalance" controltovalidate="txt_AvailBal"  ValidationGroup="Save"  errormessage="Avail Balance cannot be left blank!" ForeColor="Red" />
                               </div>
                        </div>
             
             <div class="col-sm-12">
                                            <div class="form-group">
                                                <label>
                                   Comment  *</label>
                               
                                   <asp:TextBox ID="txt_comment" runat="server" CssClass="inputSM" MaxLength="100" TextMode="MultiLine" hight="72"
                                       TabIndex="3" Width="100%"></asp:TextBox>
                                  <asp:RequiredFieldValidator runat="server" id="reqcomment" controltovalidate="txt_comment"  ValidationGroup="Save"  errormessage="Please enter your Comment!" ForeColor="Red" />
                               </div>
                        </div>

               
                </div>
                <div class="row" id="VanlistTokenBox"  runat="server" visible="false">
                  
                     <div class="col-sm-12">
                                            <div class="form-group">  <label><asp:Label ID="lable1" runat="server" Visible="false">Vans</asp:Label></label>

                  
                   
                
                       
                       
                  </div>
                         </div>
              
              
                  </div>

              </ContentTemplate>
   </telerik:RadAjaxPanel>
                 <div class="row" >
                     <div class="col-sm-12">
                        <div class="form-group">
                            <asp:HiddenField ID="HVanList" runat="server" />
                     <asp:Button ID="btnSave" runat="server"  CssClass="btn btn-success"
                        OnClick="btnSave_Click"   Text="Save" ValidationGroup="Save" />
                     <asp:Button ID="btnCancel" runat="server" CssClass ="btn btn-default"
                         OnClick="btnCancel_Click"  Text="Go Back" />
                             
                            </div>
                         </div>
                 </div>
                    
             
          </asp:Panel>    
                 </div>    
      
               </div>
 
      </asp:Content>