<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="ListingBlockCustomers.aspx.vb" Inherits="SalesWorx_BO.ListingBlockCustomers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>



<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script>
        function alertCallBackFn(arg) {

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <h4>Customer Blocking</h4>
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
                   
                       <asp:Button ID="BtnFilter" runat="server" CssClass="btn btn-warning" Text="Filter" />
                          <asp:Button ID="BtnClearFilter" runat="server" CssClass="btn btn-success" Text="Clear Filter" />
                                                 <asp:Button ID="Brn_Add" runat="server" CssClass="btn btn-warning" Text="Add Blocking Conditions" />
                   </div>
                                             </div>
        </div>


            <asp:GridView Width="100%" ID="gvCustomer" runat="server" EmptyDataText="No records to Display"
                                                    EmptyDataRowStyle-Font-Bold="true"  AutoGenerateColumns="False"
                                                    AllowPaging="True" AllowSorting="True"  PageSize="25" CellPadding="0" CssClass="tablecellalign">
                                                   
                                                   
                                                    <EmptyDataRowStyle Font-Bold="True" />
                                                   
                                                   
                                                    <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
                                                    <Columns>
                                                         
                                                        <asp:BoundField DataField="Customer_No" HeaderText="Customer No" SortExpression="Customer_No">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Customer_name" HeaderText="Customer Name" SortExpression="Customer_name">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                       
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:HiddenField runat="server" id="HcustID" value='<%# Bind("CID")%>' ></asp:HiddenField>
                                                                 <asp:HiddenField runat="server" id="HsiteID" value='<%# Bind("SID")%>' ></asp:HiddenField>
                                                                <asp:CheckBox ID="chk_AB" runat="server" checked='<%# Bind("AB")%>' />Available Balance &nbsp;
                                                                <asp:CheckBox ID="chk_CP" runat="server" checked='<%# Bind("CP")%>'/>Credit Period &nbsp;
                                                                <asp:CheckBox ID="chk_NB" runat="server" checked='<%# Bind("NB")%>'/>No. of Bills &nbsp;

                                                                <asp:Button ID="btnUpdate" runat="server" CssClass="btn btn-success" Text="Update" OnClick="btnUpdate_Click" />
                                                            </ItemTemplate>
                                                            
                                                        </asp:TemplateField>
                                                        
                                                    </Columns>
                                                   <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
                                                </asp:GridView>

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
