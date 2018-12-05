<%@ Page Title="Assign Bonus Plan To Customer" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master"
    CodeBehind="AssignBonusPlanToCustomer.aspx.vb" Inherits="SalesWorx_BO.AssignBonusPlanToCustomer" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
     <style type="text/css">
        .style1
        {
            font-size: 11px;
            color: #000000;
            text-decoration: none;
            font-weight: bold;
            height: 43px;
        }
    </style>
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
 <asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h4>Assign Bonus Plan To Customer</h4>
      <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>
             
                        
                            <asp:UpdatePanel runat="server" ID="TopPanel" UpdateMode ="Conditional" >
                             <Triggers>
                             <asp:PostBackTrigger  ControlID="btnExport"   />
                            <asp:PostBackTrigger  ControlID="lbLog1"   />
                              
                        <%--  <asp:PostBackTrigger  ControlID="btnImportWindow"  />--%>
                        
                            </Triggers>    
        <ContentTemplate>
            <div class="row">
                <div class="col-sm-6">
                     <div class="text-primary" style="font-weight:700;padding-bottom:15px;">
                        <asp:Label Visible="false" ID="lblnote" runat ="server" Text ="Note: Assigning a bonus plan to the customer removes any existing plan assigned to the same customer."></asp:Label> 
                                      </div>
                </div>
                <div class="col-sm-6 text-right">
                  <asp:Button ID="btnExport" runat="server" CssClass ="btn btn-warning" Text="Export" TabIndex ="11" />
                   <asp:Button ID="btnImportWindow" runat="server"  CssClass="btn btn-warning"  Text="Import" TabIndex ="12" /> 
                                            
                </div> 
            </div>
        
           <%-- <p id="pagenote">This screen may be used to assign bonus plan to customers</p>--%>
            <div class="row">
                <div class="col-sm-4">
                      <div class="form-group"><label>
                             Organization *
                            </label>
                                      <asp:Label runat ="server" ID="lblOrgName" ForeColor ="#248AAF" Font-Bold ="true"  ></asp:Label>
                                    <telerik:RadComboBox Skin="Simple"  Visible ="false"   ID="ddOraganisation"  AutoPostBack="true"  Width ="100%" runat="server" CssClass="inputSM">
                                      </telerik:RadComboBox>
                                </div>
                      </div>
                <div class="col-sm-4">         
                           <div class="form-group"><label>
                              Bonus Plan 
                              </label>
                               <%-- <telerik:RadComboBox Skin="Simple" ID="ddlBonusPlan" AutoPostBack="true" Width ="100%" runat="server" CssClass="inputSM"></telerik:RadComboBox>--%>
                               <asp:Label runat ="server" ID="lblPlanName" ForeColor ="#248AAF" Font-Bold ="true"  ></asp:Label>
                                 <asp:HiddenField ID="hfPlanID" runat ="server" />
                              
                                </div>
                    </div>
                 <div class="col-sm-4">         
                           <div class="form-group"><label>
                              Plan Type
                              </label>
                               <%-- <telerik:RadComboBox Skin="Simple" ID="ddlBonusPlan" AutoPostBack="true" Width ="100%" runat="server" CssClass="inputSM"></telerik:RadComboBox>--%>
                               <asp:Label runat ="server" ID="lblPlanType" ForeColor ="#248AAF" Font-Bold ="true"  ></asp:Label>
                                <asp:HiddenField ID="hfPlanType" runat ="server" />
                                </div>
                    </div>
                </div>
            <div class="row">
                <div class="col-sm-4">
                                   <div class="form-group">
                                   <label>Filter By Customer</label>
                              <telerik:RadTextBox runat ="server" ID="txtFilter" EmptyMessage ="Please type Customer No./Name/Channel " Skin ="Simple"  Width ="100%"   ></telerik:RadTextBox>
                                  
                </div>
                    </div>
                <div class="col-sm-4">
                                   <div class="form-group">  
                                          <label><span style ="visibility:hidden;"> 1</span></label>              
                                    <asp:Button ID="btnFilter" runat="server" Text="Filter" CssClass ="btn btn-primary"  />
                                                                   <asp:Button  ID="btnReset" runat="server" Text="Reset"  CssClass ="btn btn-danger" />
                                       <asp:Button  ID="btnBack" runat="server" Text="Go Back" CssClass ="btn btn-default" />
                               </div> 
                    </div>
                            
                   </div>           
                               <table  border="0" cellspacing="0" cellpadding="0"  width="100%">
                            <tr>
                                <td width="100%">
                                    <table  border="0" cellspacing="0" cellpadding="0"  width="100%">
                                        <tr>
                                            <td>
                                                <p><asp:Label ID="lblProdAvailed" Font-Bold="true" runat="server" Text=""></asp:Label></p>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <p><asp:Label Font-Bold="true" ID="lblProdAssign" runat="server" Text=""></asp:Label>
                                                <asp:LinkButton ID="lbLog1" Font-Bold ="true" Font-Size ="13px"   ForeColor  ="#337AB7"
              ToolTip ="Click here to download the log" runat ="server"
               Text ="View Log" Visible="false" ></asp:LinkButton>
                                                    </p>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="48%">
                                               
                                                 <telerik:RadListBox ID="lstDefault"  AutoPostBack ="false"   runat ="server"   Width="100%" Height ="340px"   SelectionMode ="Multiple"   >
        <%--    
           <ItemTemplate>
             
                    <a href ="#" >
		
                        <asp:ImageButton runat ="server" ID="imgMoveLeft" ImageAlign ="TextTop"  BorderStyle ="None"  ToolTip ="Move item to left"  OnClick ="imgMoveLeft_Click" ImageUrl="~/Images/arrowLeft.png" />
                        </a> &nbsp;&nbsp;
		<asp:Label runat="server" ID="lblProdID"    Visible ="false"    Text='<%#Bind("CustSiteId")%>' ></asp:Label>
		<%# DataBinder.Eval(Container, "Text") %> 
                    
	</ItemTemplate>--%>
            

        </telerik:RadListBox>
                                            </td>
                                              <td style="padding:10px; vertical-align:middle;" width="50px">
                                                  <table  border="0" cellspacing="0" cellpadding="0"  width="100%">
                   
                                                            <tr><td style="padding:3px 0;"><asp:ImageButton runat ="server" ID="btnAdd" BorderStyle ="None" 
                                                                 ToolTip ="Move selected item to right"   ValidationGroup="valsum"  OnClick ="btnAdd_Click"   ImageUrl="~/Images/arrowSingleRight.png" /></td></tr>
                   
                                                            <tr><td style="padding:3px 0;">  
                                                              <asp:ImageButton runat ="server" ID="btnRemove"   ValidationGroup="valsum"  BorderStyle ="None"  ToolTip ="Move selected item to left" OnClick ="btnRemove_Click"
                                                                      ImageUrl="~/Images/arrowSingleLeft.png" /></td></tr>
                                                            <tr>
                                                                <td style="padding:3px 0;">
                                                          <asp:ImageButton runat ="server" ID="btnRemoveAll" BorderStyle ="None"    ValidationGroup="valsum"
                                                               ToolTip ="Move all item to left"  ImageUrl="~/Images/doubleRight.png" /></td>
                                                                </tr> 
                                                            <tr>
                                                               <td style="padding:3px 0;">
                                                          <asp:ImageButton runat ="server" ID="btnAddAll"    ValidationGroup="valsum"  BorderStyle ="None"  
                                                                  ToolTip ="Move all Item to right" ImageUrl="~/Images/doubleLeft.png" /></td>
                                                                </tr> 
                                                            </table> 

                                             
                                            </td>
                                             <td width="48%">
                                                 

                                                <telerik:RadListBox ID="lstSelected"  AutoPostBack ="false" runat ="server" Width="100%"   Height ="340px"  SelectionMode ="Multiple"     >
          
          <%-- <ItemTemplate>
               
            <a href="#">
		      <asp:ImageButton runat ="server" ID="imgMoveRight" ImageAlign ="TextTop"  BorderStyle ="None" OnClick ="imgMoveRight_Click"  ToolTip ="Move item to right" ImageUrl="~/Images/arrowRight.png" />
                </a>  &nbsp;&nbsp;
		<asp:Label runat="server" ID="lblProdID"    Visible ="false"    Text='<%#Bind("CustSiteId")%>' ></asp:Label>
		<%# DataBinder.Eval(Container, "Text") %> 
         
	</ItemTemplate>
            --%>

        </telerik:RadListBox>

                                            </td>
                                        </tr>
                                    </table>
                           </td>
                                </tr>
                        </table>
                         </ContentTemplate>
                             
     </asp:UpdatePanel> 
                        
                        
           <asp:UpdatePanel ID="UPModal" runat="server" >
                                <ContentTemplate>
                          
                    
                                               <div>
                                             

<telerik:RadWindow ID="MPEImport" Title= "Import Customer Bonus Plan" runat="server" Skin="Windows7" Behaviors="Move,Close"
                                                Width="450px" Height="200px" ReloadOnShow="false" VisibleStatusbar="false" Overlay="true" Modal="true">
                                              <ContentTemplate>
<span style ="padding-left:5px;padding-top:10px;"><asp:Label ID="lblMessage"  CssClass ="txtSMBold" runat ="server" ForeColor ="Red" ></asp:Label>
                                                        </span>

                    <asp:HiddenField ID="HFV" runat="server" Value="-1" />
                    <table   border="0" cellspacing="2" cellpadding="2"  width="100%">
                  
                  <tr>
                  <td colspan ="2">
                  <asp:Label runat ="server" ID="Label6" CssClass ="txtSM" Font-Size ="12px" ForeColor ="Blue"  Text =""></asp:Label>
                  
                  </td>
                  </tr>      
                       
		 <tr>
    <td class ="txtSMBold" >Select a File :</td>
    <td style ="color:Black;"> <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="conditional">
                                                        <ContentTemplate><asp:FileUpload ID="ExcelFileUpload" runat="server" /></ContentTemplate>
                                                    </asp:UpdatePanel>    
          
         </td>
       
          </tr>
          <tr><td colspan ="2"><br /></td></tr>
          <tr>
          <td>
           
             
          </td>
          <td>
           <asp:Button ID="btnImport" runat="server" Text="Import" CssClass ="btn btn-primary" />
               <asp:Button ID="btnCancelImport" CssClass ="btn btn-danger" Visible ="false"   TabIndex="5" runat="server"
                                    CausesValidation="false" Text="Close" />
                  <asp:Button ID="DummyImBtn" style="display:none"  runat="server" Text="Reimport" />
           <asp:Button ID="BtnReimport" runat="server" Text="Reimport"  Visible ="false" 
                 CssClass="btnInputBlue" />
           <asp:Button ID="DummyReimBtn" style="display:none"  runat="server" Text="Reimport" />
            <span style ="text-decoration: underline !important;Color:#337AB7;"> <asp:LinkButton ID="lbLog" Font-Bold ="true" Font-Size ="13px"   ForeColor  ="#337AB7"
              ToolTip ="Click here to download the uploaded log" runat ="server"
               Text ="View Log" Visible="false" ></asp:LinkButton></span>
           </td>
          </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel runat="server" ID="UpPanel">
                                    <Triggers>
                                      <asp:AsyncPostBackTrigger ControlID="DummyImBtn" EventName="Click" />
	<asp:AsyncPostBackTrigger ControlID="DummyReimBtn" EventName="Click" />
	   <asp:PostBackTrigger ControlID="btnImport" /> 
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr><td colspan ="2">
                        <asp:Label runat ="server" ID="lblUpMsg" CssClass ="txtSM" Font-Size ="12px" ForeColor ="Green" Font-Bold ="true" > </asp:Label></td></tr>
                        <tr>
                        <td colspan="2">
                       
                         <asp:GridView Width="100%" ID="dgvErros"   runat="server" EmptyDataText="No items to display"
                                                        EmptyDataRowStyle-Font-Bold="true"  AutoGenerateColumns="False" Visible="false" 
                                                        AllowPaging="true" AllowSorting="false"  PageSize="15" CellPadding="0" CellSpacing="0" CssClass="tablecellalign"  >
                                                        
                                                        <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
                                                        <EmptyDataRowStyle Font-Bold="True" />
                                                        <Columns>
                                                     
                                                             <asp:BoundField HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="RowNo"
                                                                HeaderText="Row No">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>
                                                           
                                                       
                                                          
                                                             <asp:BoundField HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="LogInfo"
                                                                HeaderText="Log Info">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>
                                                             
                                                        
                                                          
                                                        </Columns>
                                                        <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
                                                    </asp:GridView>
                        
                         
                        </td>
                        </tr>
                    </table>
                                                </ContentTemplate>
                               
                            
                                                    </telerik:RadWindow> 
                                             </div>
     </ContentTemplate>

     </asp:UpdatePanel> 
        
  
   
    
  
           <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="UpModal"
                    runat="server">
                    <ProgressTemplate>
                        <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                            <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                            <span style="font-size: 12px; color: #3399ff;">Processing... </span>
                        </asp:Panel>
                    </ProgressTemplate>
                </asp:UpdateProgress>
              
         
</asp:Content>
