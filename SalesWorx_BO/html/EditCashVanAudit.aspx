<%@ Page Title="Modify Cash Van Audit Survey" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="EditCashVanAudit.aspx.vb" Inherits="SalesWorx_BO.EditCashVanAudit" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
    <script>
        function alertCallBackFn(arg) {

        }

        function NumericOnly(e) {

            var keycode;

            if (window.event) {
                keycode = window.event.keyCode;
            } else if (e) {
                keycode = e.which;
            }
            if ((parseInt(keycode) >= 48 && parseInt(keycode) <= 57) || parseInt(keycode) == 8 || parseInt(keycode) == 46 || parseInt(keycode) == 0 || parseInt(keycode) == 44)
                return true;

            return false;
        }

        function IntegerOnly(e) {

            var keycode;

            if (window.event) {
                keycode = window.event.keyCode;
            } else if (e) {
                keycode = e.which;
            }
            if ((parseInt(keycode) >= 48 && parseInt(keycode) <= 57) || parseInt(keycode) == 8)
                return true;

            return false;
        }



    </script>
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <h4>Modify Cash Van/FSR Audit Survey</h4>
	<telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>
    <asp:UpdatePanel ID="Panel" runat="server">
        <ContentTemplate>
	<div class="row">
                 <div class="col-sm-4">
                                                     
                                            <div class="form-group">
                                                <label><asp:Label ID="lblCustVan" runat="server" Text="Van/FSR :"></asp:Label> </label>

                  
                <telerik:RadComboBox Skin="Simple"  Filter="Contains"   ID="ddlVan" Width ="100%" 
                    runat="server" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID" EmptyMessage="Select Van/FSR"></telerik:RadComboBox>
              </div>
                     </div>
           <div class="col-sm-4">
                                         <label>&nbsp;</label>            
                                            <div class="form-group">
                     <asp:Button  CssClass ="btn btn-success" ID="SearchBtn" runat="server" Text="Search" />
                   
                     <asp:Button  CssClass ="btn btn-default"  ID="btnUpdate" runat="server" Text="Update" />
                     <asp:Button CssClass ="btn btn-primary" ID="btnConfirm" runat="server" Text="Confirm" />
                                                </div>
               </div>
        </div>
            <div id="NoSurvey" runat="server" visible="false" >
                  <label>No Audit Survey Initialised.</label>
                </div>
    <div id="vandetails" runat="server" visible="false" >
                <div class="row" >
                     <div class="col-sm-4">
                                                     
                                            <div class="form-group">
                                                <label>Name of sales man </label>
                                                <asp:Label ID="lbl_van"    runat="server" Text=''></asp:Label>
                                            </div>
                                            </div>
                    <div class="col-sm-4">
                                                     
                                            <div class="form-group">
                                                <label>Site</label>
                                                <asp:Label ID="lbl_Site"     runat="server" Text=''></asp:Label>
                                            </div>
                        </div>
                         <div class="col-sm-4">
                                                     
                                            <div class="form-group">
                                                <label>Surveyed at</label>
                                                <asp:Label ID="lbl_surveyedat"    runat="server" Text=''></asp:Label>
                                            </div>
                                            </div>
                        

                </div>
                <div class="row">
                     <div class="col-sm-4">
                                                     
                                            <div class="form-group">
                                                <label>Surveyed By  </label>
                                                <asp:Label ID="lbl_surveyedBy"     runat="server" Text=''></asp:Label>
                                            </div>
                                            </div>
                    </div>
        </div>
                       <asp:Label ID="txtSurveyId"  Visible ="false"   runat="server" Text=''></asp:Label>
                       <asp:Label ID="txtAVanName"  Visible ="false"   runat="server" Text=''></asp:Label>
                       <asp:Label ID="txtSalesRepid"  Visible ="false"   runat="server" Text=''></asp:Label>
                       <asp:Label ID="txtEmpCode"  Visible ="false"   runat="server" Text=''></asp:Label>
                          <asp:Label ID="txtSurveyTime"  Visible ="false"   runat="server" Text=''></asp:Label>


        
      
        
     <asp:GridView Width="100%" ID="gvResponse" runat="server"  AutoGenerateColumns="False"   
                                                                    PageSize="25" CellPadding="0" CellSpacing="0" CssClass="tablecellalign">
                                                                   
                                                                    <Columns>
                                                                         
                                                                        <asp:TemplateField HeaderText="Question">
                                                                          
                                                                            <HeaderStyle HorizontalAlign="left" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="txtQuest"    runat="server" Text='<%# Bind("Question") %>'></asp:Label><br />
                                                                                <asp:Label ID="txtQuest1"  visible="false" runat="server"   ></asp:Label><br />
                                                                                <asp:Label ID="txtQuest2"  visible="false" runat="server"   ></asp:Label><br />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="left" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Response">
                                                                          
                                                                            <HeaderStyle HorizontalAlign="left" />
                                                                            <ItemTemplate>
                                                                              <asp:TextBox ID="txtResponse"   runat="server"  Width ="450px"  visible="false" ></asp:TextBox>
                                                                              <asp:RadioButtonList runat="server" ID="Rdo_resp"  visible="false" ></asp:RadioButtonList>
                                                                              <asp:CheckBoxList runat="server" ID="Chk_resp"  visible="false"></asp:CheckBoxList>
                                                                              <asp:TextBox ID="txtResponse1"   runat="server"   Width ="250px"  visible="false"  ></asp:TextBox>
                                                                              <asp:TextBox ID="txtResponse2"   runat="server"   Width ="250px"  visible="false"  ></asp:TextBox>
                                                                              <asp:TextBox ID="txtResponse3"   runat="server"   Width ="250px"  visible="false" onkeypress="return NumericOnly(event);"  ></asp:TextBox>

                                                                                <telerik:RadDatePicker ID="txtFromDate"   Width ="30%" runat="server" Visible="false" >
                                                        <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy">
                                                        </DateInput>
                                                        <Calendar ID="Calendar2" runat="server">
                                                            <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                        </Calendar>
                                                    </telerik:RadDatePicker>

                                                                                <telerik:RadDatePicker ID="txtToDate"  Width ="30%"  runat="server" Visible="false" >
                                                        <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy">
                                                        </DateInput>
                                                        <Calendar ID="Calendar1" runat="server">
                                                            <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                        </Calendar>
                                                    </telerik:RadDatePicker>

                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="left" />
                                                                        </asp:TemplateField>
                                                                        
                                                                         <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbRespType" runat="server" Text='<%# Bind("RespTypeID") %>'></asp:Label>
                                                                <asp:Label ID="lblReps" runat="server" Text='<%# Bind("Restxt")%>'></asp:Label>
                                                                 <asp:Label ID="lblEnabled" runat="server" Text='<%# Bind("Editable")%>'></asp:Label>
                                                                <asp:Label ID="lblMand_Confirm" runat="server" Text='<%# Bind("Mandatory_On_Confirmation")%>'></asp:Label>
                                                                
                                                                  <asp:Label ID="lblremark_requ" runat="server" Text='<%# Bind("Remarks_Required")%>'></asp:Label>
                                                                 <asp:Label ID="lblremark" runat="server" Text='<%# Bind("Remarks")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        
                                                                    <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbQuestId" runat="server" Text='<%# Bind("QuestId") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                    <PagerStyle CssClass="pagerstyle" />
                                                    
                                                                </asp:GridView>
     
          </ContentTemplate>
          <Triggers>
          <asp:AsyncPostBackTrigger ControlID="SearchBtn" EventName="Click" />
          </Triggers>
        </asp:UpdatePanel>                    
       
                             
                        
      
	 
</asp:Content>
