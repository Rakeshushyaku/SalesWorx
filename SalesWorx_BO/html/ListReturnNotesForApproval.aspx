<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="ListReturnNotesForApproval.aspx.vb" Inherits="SalesWorx_BO.ListReturnNotesForApproval" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="FeaturedContent" runat="server">
    <style>
    input.rdfd_[type="text"] { height:0 !important; padding:0 !important; }
</style>
<script type="text/javascript">


    function alertCallBackFn(arg) {
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
     
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h4>Return Notes</h4>
 
                                         
                     <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>
      <asp:UpdatePanel ID="ClassUpdatePnl" runat="server"  >
        <ContentTemplate>
          <div class="row">
               

          <div class="col-sm-3">
                             <div class="form-group">  <label>Organization </label>
            <telerik:RadComboBox Skin="Simple" ID="ddlOrganization"  
                    runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" Width="100%" AutoPostBack="true"  >
                </telerik:RadComboBox>
             </div>
              </div>
               
              <div class="col-sm-3">
                                            <div class="form-group">
                                                <label>Van/FSR</label>
                                                  <telerik:RadComboBox Skin="Simple"  EmptyMessage="Select Van/FSR" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"  ID="ddlVan" Width ="100%" runat="server" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID" Filter="Contains">
                                        </telerik:RadComboBox >
                                            </div>
                                          </div>
                                           
 
              <div class="col-sm-3">  
                  <div class="form-group">                  
                   <label>From Date </label>
                                 
              <telerik:RadDatePicker ID="txtFromDate"  Width="100%"  runat="server" >
                                                <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy">
                                                </DateInput>
                                                <Calendar ID="Calendar2" runat="server">
                                                    <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                </Calendar>
                                            </telerik:RadDatePicker>
                  </div>
                  

                  </div>
            
         
              <div class="col-sm-3">  
                  <div class="form-group">                
                                  <label> To Date </label>
                                  
                <telerik:RadDatePicker ID="txtToDate"   Width="100%" runat="server">
                                                <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy">
                                                </DateInput>
                                                <Calendar ID="Calendar1" runat="server">
                                                    <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                </Calendar>
                                            </telerik:RadDatePicker>
                      </div>
                  

                  </div>
               <div class="col-sm-3">
                                            <div class="form-group">
                                                <label>Ref. No</label>
                                                <asp:TextBox ID="txtRefNo" Width="100%" CssClass="inputSM" runat="server"></asp:TextBox>
                                            </div>
                                            
                                        </div>
              <div class="col-sm-3">
                                            <div class="form-group">

                                                <label>Customer</label>
                                                <telerik:RadComboBox Skin="Simple" Filter="Contains" ID="ddlCustomer" Width="100%"
                                                    runat="server" DataTextField="Customer" DataValueField="CustomerID" EnableLoadOnDemand="True" AutoPostBack="true" EmptyMessage="Please type Customer No./Name">
                                                </telerik:RadComboBox>
                                            </div>
                                        </div>
              <div class="col-sm-3">  
                  <div class="form-group"> 
                       <label>&nbsp;</label>
                <telerik:RadButton ID="SearchBtn" Skin ="Simple"    runat="server" Text="Search" CssClass ="btn btn-primary" />
         <asp:Button CssClass="btn btn-sm btn-default" ID="ClearBtn" runat="server" Text="Clear" />
                                  </div>
            </div> 
         </div>
  

    <telerik:RadGrid ID="gvRep" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                PageSize="10" AllowPaging="True" runat="server"
                GridLines="None">

                <GroupingSettings CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                    PageSize="10">


                    <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                    <Columns>
                        <telerik:GridTemplateColumn UniqueName="Orig_Sys_Document_Ref" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="Orig_Sys_Document_Ref" SortExpression="Orig_Sys_Document_Ref"
                            HeaderText="Return Ref. No.">
                            <ItemTemplate>
                                <asp:HiddenField ID="HID" runat="server" Value='<%# Bind("Row_ID")%>'></asp:HiddenField>
                                <asp:LinkButton ID="Lnk_RefID" runat="server" Text='<%# Bind("Orig_Sys_Document_Ref")%>' ForeColor="SteelBlue" Font-Underline="true" CommandName="ConfirmReturnNote"></asp:LinkButton>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>

                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Creation_Date" HeaderText="Return Date" SortExpression="Creation_Date"
                            DataFormatString="{0:dd-MMM-yyyy}">
                            <ItemStyle Wrap="False" />
                        </telerik:GridBoundColumn>


                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="SalesRep_Name" HeaderText="Van/FSR"
                            SortExpression="SalesRep_Name">
                            <ItemStyle Wrap="True" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Customer" HeaderText="Customer No. & Customer Name"
                            SortExpression="Customer">
                            <ItemStyle Wrap="True" />
                        </telerik:GridBoundColumn>
 
                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Transaction_Amt" HeaderText="Net Amount"
                            SortExpression="Transaction_Amt" DataType="System.Double" DataFormatString="{0:f2}">
                            <ItemStyle Wrap="False" HorizontalAlign="Right" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="VAT_Amount" HeaderText="VAT Amount"
                            SortExpression="VAT_Amount" DataType="System.Double" DataFormatString="{0:f2}">
                            <ItemStyle Wrap="False" HorizontalAlign="Right" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
            </ContentTemplate>
           

 

                                     </asp:UpdatePanel> 

	<asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="ClassUpdatePnl" runat="server">
        <ProgressTemplate>
            <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                <img  src="../assets/img/ajax-loader.gif"  alt="Processing..." />
                <span>Processing... </span>
            </asp:Panel>
        </ProgressTemplate>
    </asp:UpdateProgress>
  
	 
</asp:Content>
