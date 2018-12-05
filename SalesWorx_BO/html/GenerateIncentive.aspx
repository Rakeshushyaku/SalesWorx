<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="GenerateIncentive.aspx.vb" Inherits="SalesWorx_BO.GenerateIncentive" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
     <style>
       input.rdfd_[type="text"] { height:0 !important; padding:0 !important; }
       .rgSelectedRow:hover td {
           background: #828282 !important;
       }
       .rgSelectedRow td {
            background: #828282 !important;
        }
       #ctl00_MainContent_MPSettle_C {
           height: auto !important;
           padding: 20px 0;
       }
       .rwWindowContent {
           vertical-align: middle !important;
       }
       .rwTable {
           height: auto !important;
       }

   </style>
     <style>
    div.RadGrid .rgPager .rgAdvPart     
   {     
    display:none;        
   }      
</style>
    <script>
function alertCallBackFn(arg) {
         
    }
    </script>

    </asp:Content>
 <asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
     <h4>Incentive Generation</h4>
                                             
                     <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>
                <asp:UpdatePanel ID="ClassUpdatePnl1" runat="server"  UpdateMode="conditional">
                    <ContentTemplate>
                        
                        
                                    <asp:Label ID="lblMsg" runat="server" Font-Bold="True" ForeColor="Maroon"></asp:Label>
                        <div class="row">
          <div class="col-sm-6 col-md-4">
                             <div class="form-group">
                                 <label> <asp:Label ID="lbl_Month" runat="server" Text="Label"></asp:Label></label>
                                 </div>
              </div></div>
                                  <div class="row">
          <div class="col-sm-6 col-md-4">
                             <div class="form-group">  <label>Organization</label>
                                                        
                                                        
                                                            <telerik:RadComboBox Skin="Simple" ID="ddlOrganization"  Width ="100%" 
                    runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="true" Filter="Contains" ></telerik:RadComboBox>

                                 
                               </div>
              </div>
                                      <div class="col-sm-6 col-md-1">
                             <div class="form-group">  <label>&nbsp;</label>
                                                     <asp:Button ID="Btn_Generate" runat="server" Text="Generate Incentive" CssClass="btn btn-success" ></asp:Button>   
                                                        
                                                            
                               </div>
              </div>
                                       <div class="col-sm-6 col-md-7">
                                           <div class="form-group"> &nbsp;
                                               </div>
                                           </div>
                                      </div>
                        
                        <telerik:RadGrid id="gvRep" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="transparent"
                                PageSize="10" AllowPaging="True" runat="server" 
                                GridLines="None" >
                                                       
                                                        <GroupingSettings CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="transparent"
                    PageSize="10">


                  <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                        <Columns>
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Emp_Code" HeaderText="Employee Code" SortExpression ="Emp_Code"
                                                               >
                                                                <ItemStyle HorizontalAlign ="Left" Wrap="False" />
                                                                  <HeaderStyle HorizontalAlign ="Left" Wrap="False" />
                                                            </telerik:GridBoundColumn>
                                                      
                                                          
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Emp_Name" HeaderText="Employee Name"
                                                                  SortExpression ="Emp_Name" >
                                                              <ItemStyle HorizontalAlign ="Left" Wrap="False" />
                                                                  <HeaderStyle HorizontalAlign ="Left" Wrap="true" />
                                                            </telerik:GridBoundColumn>

                                                                <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Total_Incentive" HeaderText="Total Incentive"
                                                                  SortExpression ="Total_Incentive" DataType="System.Double" DataFormatString="{0:N2}">
                                                                    <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                                                                  <HeaderStyle HorizontalAlign ="Center" Wrap="true" />
                                                            </telerik:GridBoundColumn>
                                                           
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Incentive_Eligibility_EOM" HeaderText="Eligibile Incentive(EOM) "
                                                                  SortExpression ="Incentive_Eligibility_EOM" DataType="System.Double" DataFormatString="{0:N2}">
                                                                    <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                                                                  <HeaderStyle HorizontalAlign ="Center" Wrap="true" />
                                                           </telerik:GridBoundColumn>

                                                              
                                                        </Columns>
                                                        </MasterTableView>
                                                    </telerik:RadGrid>  

                        <telerik:RadWindow ID="MPSettle" Title= "Confirmation" runat="server" Skin="Windows7" Behaviors="Move,Close"
                                                Width="450px" Height="210px" ReloadOnShow="false" VisibleStatusbar="false" Overlay="true" Modal="true">
                                              <ContentTemplate>
                            <table id="table1" width="400" cellpadding="10" style="background-color: White;">
                                <tr>
                                    <td align="center" style="text-align: center">
                                       <asp:Label ID="Label1" runat="server"  Font-Size ="13px"  Text="User who have not synced in this month" ></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadGrid id="rg_users" AllowSorting="false" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="transparent"
                                PageSize="5" AllowPaging="True" runat="server" 
                                GridLines="None" >
                                                       
                                                        <GroupingSettings CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="transparent"
                    PageSize="5">


                  <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                        <Columns>
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Emp_Code" HeaderText="Employee Code" SortExpression ="Emp_Code"
                                                               >
                                                                <ItemStyle HorizontalAlign ="Left" Wrap="False" />
                                                                  <HeaderStyle HorizontalAlign ="Left" Wrap="False" />
                                                            </telerik:GridBoundColumn>
                                                      
                                                          
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Emp_Name" HeaderText="Employee Name"
                                                                  SortExpression ="Emp_Name" >
                                                              <ItemStyle HorizontalAlign ="right" Wrap="False" />
                                                                  <HeaderStyle HorizontalAlign ="Left" Wrap="true" />
                                                            </telerik:GridBoundColumn>

                                                                <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="lastSynctime" HeaderText="Last Sync Time "
                                                                  SortExpression ="lastSynctime"  >
                                                                    <ItemStyle Wrap="False"  />
                                                                  <HeaderStyle HorizontalAlign ="Center" Wrap="true" />
                                                            </telerik:GridBoundColumn>
                                                           
                                                               
                                                        </Columns>
                                                        </MasterTableView>
                                                    </telerik:RadGrid> 
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="text-align: center">
                                       <asp:Label ID="Label3" runat="server"  Font-Size ="13px"  Text="Are you sure to generate the incentive?" ></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="text-align: center;">
                                        <asp:Button ID="btn_Yes" runat="server" Text="Yes"  CssClass="btn btn-success"  />
                                        <asp:Button ID="btnCloseconfirm" runat="server" Text="No"  CssClass ="btn btn-danger"  />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                               
                            
                                                    </telerik:RadWindow> 

                                       </ContentTemplate>
                </asp:UpdatePanel>

     <asp:UpdateProgress ID="UpdateProgress2" AssociatedUpdatePanelID="ClassUpdatePnl1" DisplayAfter="10"
                                runat="server">
                                <ProgressTemplate>
                                    <asp:Panel ID="Panel14" CssClass="overlay" runat="server">
                                        <img src="../assets/img/ajax-loader.gif" alt="Processing..." />
                                        <span>Processing... </span>
                                    </asp:Panel>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
    

</asp:Content>
