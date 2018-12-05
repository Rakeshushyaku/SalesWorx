<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="BlockCustomers.aspx.vb" Inherits="SalesWorx_BO.BlockCustomers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>



<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script>
        function alertCallBackFn(arg) {
             
        }
    </script>
    <style>        
        select[multiple].height800 {
            height:340px !important;
        }
        .form-inline label {
            display:inline-block;
            margin:0;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <h4>Customer Blocking </h4>

    <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>

     <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
    <div class="row">
                                         <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>
              Organization *</label>

              <telerik:RadComboBox Skin="Simple" Filter="Contains" ID="ddlOrganization" 
                    runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" 
                     TabIndex="1" AutoPostBack="true" Width="100%">
                </telerik:RadComboBox>
                 </div>
                                             </div>
              
                  </div>
    <div class="row">
                                         <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>
                      Customer Name * </label>
                 
                      <asp:TextBox ID="txt_CusName" runat="server" CssClass="inputSM" MaxLength="150" 
                          TabIndex="2"  Width ="100%"></asp:TextBox>
                     
                   </div>
                                             </div>

                   
                                         <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Customer No * </label>
                   
                      <asp:TextBox ID="txt_CusNo" runat="server" CssClass="inputSM" MaxLength="10" 
                          TabIndex="3"  Width ="100%"  ></asp:TextBox>
                   </div>
                                             </div>
         <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>&nbsp;  </label>
                   
                       <asp:Button ID="BtnFilter" runat="server" CssClass="btn btn-primary" Text="Filter" />
                          <asp:Button ID="BtnClearFilter" runat="server" CssClass="btn btn-default" Text="Clear Filter" />
                                                
                   </div>
                                             </div>
        </div>

            
             <div class="row">
                 <div class="col-sm-7">
                                            <div class="form-group">
                        <asp:ListBox ID="lst_Customer" SelectionMode="Multiple" Width="100%" CssClass="inputSM height800"  runat="server"></asp:ListBox>
                  </div>
                     </div>
                 <div class="col-sm-5">
                                            <div class="form-group form-inline">
                        <asp:CheckBoxList ID="chk_BlockPramar" runat="server" Width="100%" RepeatDirection="Vertical" CssClass="inputSM" >
                            <asp:ListItem Text="Available Balance" Value="AB"></asp:ListItem>
                            <asp:ListItem Text="Credit Period" Value="CP"></asp:ListItem>
                            <asp:ListItem Text="No of Bills" Value="NB"></asp:ListItem>
                        </asp:CheckBoxList>
                  </div>
                       <div class="form-group">
                                                <asp:Button ID="Btn_Save" runat="server" Text="Save" CssClass="btn btn-success"></asp:Button>
                                                <asp:Button ID="Btn_Cancel" runat="server" CssClass="btn btn-default" Text="Cancel" />
                  </div>
                     </div>
             </div>

             </ContentTemplate>
         
        </asp:UpdatePanel>  
        <asp:UpdateProgress ID="UpdateProgress2"   AssociatedUpdatePanelID="UpdatePanel1" runat="server" DisplayAfter="10">
     <ProgressTemplate>
     <asp:Panel ID="Panel19" CssClass="overlay" runat="server">
     
         <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="vertical-align:middle;" />            
           <span>Processing... </span>
       </asp:Panel>
           
       
         
    </ProgressTemplate>
            </asp:UpdateProgress>                  
       
</asp:Content>
