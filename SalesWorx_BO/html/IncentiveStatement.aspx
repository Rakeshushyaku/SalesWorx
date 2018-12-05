<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="IncentiveStatement.aspx.vb" Inherits="SalesWorx_BO.IncentiveStatement" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <style>
        .btn-m-w-150{
            min-width:150px;
        }
    </style>
     <script>
         function clickSearch() {
             $("#MainContent_SearchBtn").click()
         }
         function clickExportExcel() {
             $("#MainContent_BtnExportExcel").click()
             return false

         }
         function clickExportPDF() {
             $("#MainContent_BtnExportPDF").click()
             return false
         }
         function alertCallBackFn(arg) {

         }

         function OpenViewWindow(cid) {
             var URL
            // URL = 'RepDetails.aspx?Type=Col&ReportName=CollectionDetails&ID=' + cid;
             URL = 'Rep_CollectionDetails.aspx?ID=' + cid;
             var oWnd = radopen(URL, null);
             oWnd.SetSize(900, 600);
             oWnd.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move + Telerik.Web.UI.WindowBehaviors.Close); //Close:4
             oWnd.SetModal(true);
             oWnd.Center;
             oWnd.set_visibleStatusbar(false)

             return false

         }

         document.onkeydown = function (evt) {

             evt = evt || window.event;
             if (evt.keyCode == 27) {

                 HideRadWindow();
             }
         };

         function HideRadWindow() {

             var elem = $('a[class=rwCloseButton');

             if (elem != null && elem != undefined) {
                 $('a[class=rwCloseButton')[0].click();
             }


         }

         function NumericOnly(e) {

             var keycode;

             if (window.event) {
                 keycode = window.event.keyCode;
             } else if (e) {
                 keycode = e.which;
             }
             if ((parseInt(keycode) >= 48 && parseInt(keycode) <= 57) || parseInt(keycode) == 8 || parseInt(keycode) == 46 || parseInt(keycode) == 0)
                 return true;

             return false;
         }

    </script>
    <style>
    div.RadGrid .rgPager .rgAdvPart     
   {     
    display:none;        
   }      
</style>
     </asp:Content>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
   <h4>Incentive Statement</h4>
	<telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>
	 
   <telerik:RadAjaxManager ID="RadAjaxManager2" runat="server">

        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager2">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxManager2" />
                </UpdatedControls>
            </telerik:AjaxSetting>

        </AjaxSettings>
    </telerik:RadAjaxManager>

     <telerik:RadAjaxPanel ID="l" runat="server">
  
                                        <div class="row">
                                            
                                         <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Organization<em><span>&nbsp;</span>*</em></label>
             <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddlOrganization" Width ="100%" runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="True" >
                                        </telerik:RadComboBox>
                  </div>
                                          </div>
                                                    <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Employee</label>
                                                 
                                             
                                                 <telerik:RadComboBox Skin="Simple"  EmptyMessage="Select Employee" EnableCheckAllItemsCheckBox="true" ID="ddlEmployee" Width ="100%" runat="server" Filter="Contains">
                                        </telerik:RadComboBox >
                                            </div>
                                          </div>       
                                           
                                               
                                            <div class="col-sm-2">
                                                <div class="form-group">
                                                    <label>&nbsp;</label>
                                                <asp:Button  CssClass ="btn btn-sm  btn-primary"  ID="SearchBtn" runat="server" Text="Search"  />
                                                    <asp:Button  CssClass ="btn btn-sm  btn-default"  ID="ClearBtn" runat="server" Text="Clear"  />
                                                    </div>
                                                
                                            </div>


                                       
                                            </div>
   <div class="row">
        <div class="col-sm-6">
            <div class="form-group">
           <asp:Button  CssClass ="btn btn-sm btn-m-w-150 btn-success"  ID="Btn_pay" runat="server" Text="Pay" Visible="false"   />
                </div>
       </div>
       <div class="col-sm-6">
           <div id="divCurrency" runat="server" visible="false"  >
                <h5 class='text-right'>Currency <span class='text-blue'><strong><asp:Label ID="lbl_Currency" runat="server" Text=""></asp:Label></strong></span></h5>
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
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Emp_Name" HeaderText="Emp Name" SortExpression ="Emp_Name"
                                                               >
                                                                <ItemStyle HorizontalAlign ="Left" Wrap="False" />
                                                                  <HeaderStyle HorizontalAlign ="Left" Wrap="False" />
                                                            </telerik:GridBoundColumn>
                                                      
                                                          
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Emp_Code" HeaderText="Emp Code"
                                                                  SortExpression ="Emp_Code" >
                                                              <ItemStyle HorizontalAlign ="Left" Wrap="False" />
                                                                  <HeaderStyle HorizontalAlign ="Left" Wrap="true" />
                                                            </telerik:GridBoundColumn>

                                                                <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Outstanding" HeaderText="Outstanding Amount"
                                                                  SortExpression ="Outstanding" DataFormatString="{0:N2}" >
                                                                    <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                                                                  <HeaderStyle HorizontalAlign ="Center" Wrap="true" />
                                                            </telerik:GridBoundColumn>
                                                           <telerik:GridTemplateColumn UniqueName="Amount" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" HeaderText="Amount">
                                                            <ItemTemplate>
                                                                    <asp:TextBox ID="Txt_Amount" runat="server" Enabled="true"  onKeypress='return NumericOnly(event)' Text='' MaxLength="10" ForeColor="SteelBlue" Font-Underline="true"  ></asp:TextBox>
                                                                 <asp:HiddenField ID="HEmpCode" runat="server"  Value='<%# Bind("Emp_Code")%>'/>
                                                                <asp:HiddenField ID="HAmt" runat="server"  Value='<%# Bind("Outstanding")%>'/>
                                                            </ItemTemplate>

                                                            </telerik:GridTemplateColumn>
                                                               
                                                        </Columns>
                                                        </MasterTableView>
                                                    </telerik:RadGrid>   
         
         <asp:HiddenField ID="HCurrency" runat="server" />          
 <div style="display:none">
                                   <asp:Label ID="lblDecimal" runat="server" Text="N2"></asp:Label>
                                   </div>
                              
                        

                         </telerik:RadAjaxPanel>
    
          
 <asp:UpdateProgress ID="UpdateProgress2" DisplayAfter="10"
        runat="server">
        <progresstemplate>
                                    <asp:Panel ID="Panel14" CssClass="overlay" runat="server">
                                        <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                                        <span style="font-size: 12px; color: #3399ff;">Processing... </span>
                                    </asp:Panel>
                                </progresstemplate>
    </asp:UpdateProgress>
</asp:Content>

