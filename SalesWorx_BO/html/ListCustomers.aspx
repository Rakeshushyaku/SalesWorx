<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="ListCustomers.aspx.vb" Inherits="SalesWorx_BO.ListCustomers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="FeaturedContent" runat="server">
    <style type="text/css">
        .style1        {
            width: 235px;
        }
        #ctl00_MainContent_ExcelFileUpload {
            width: auto !important;
            display: inline-block;
            vertical-align: middle;
        }
        #ctl00_MainContent_ExcelFileUpload  ul li{
            padding-left:0 !important;
            font-size: 14px;
        }
        .RadUpload .ruBrowse {
            width: auto;
            padding: 0 10px;
        }
        .ruFileInput {
            cursor:pointer;
        }
    </style>
    <script>
        var TargetBaseControl = null;

        window.onload = function () {
            try {
                TargetBaseControl =
           document.getElementById('<%= Me.ClassUpdatePnl.ClientID %>');
            }
            catch (err) {
                TargetBaseControl = null;
            }
        }
        function confirmLinkButton(button) {
            function linkButtonCallbackFn(arg) {
                if (arg) {
                    //obtains a __doPostBack() with the correct UniqueID as rendered by the framework
                    eval(button.href);

                    //can be used in a simpler environment so that event validation is not triggered.
                    //__doPostBack(button.id, "");
                }
            }


            radconfirm("Are you sure you want to enable the customer", linkButtonCallbackFn, 330, 180, null, "Confirm");
        }

        function confirmLinkButtondis(button) {
            function linkButtonCallbackFn(arg) {
                if (arg) {
                    //obtains a __doPostBack() with the correct UniqueID as rendered by the framework
                    eval(button.href);

                    //can be used in a simpler environment so that event validation is not triggered.
                    //__doPostBack(button.id, "");
                }
            }


            radconfirm("Are you sure you want to disable the customer?", linkButtonCallbackFn, 330, 180, null, "Confirm");
        }
    function checkconfirm(status) {
            if (status == "Y")
              return confirm("Are you sure to disable this customer?")
            else
                return confirm("Are you sure to enable this customer?")
    }
    function alertCallBackFn(arg) {
        HideRadWindow()
    }

    function openPopup(cID) {
        alert(cid);
       
        window.open("AdminCustomersCreditLimit.aspx?Customer_ID=" + cID.get_value() + "&Site_Use_ID=" + sID.get_value() + "", "_blank", "WIDTH=1080,HEIGHT=500,scrollbars=no, menubar=no,resizable=yes,directories=no,location=no");

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
        <telerik:RadScriptBlock ID="RadScriptBlock2" runat="server">
        <script type="text/javascript">
            function OnClientFilesUploaded(sender, args) {
                $find('<%= RadAjaxManager2.ClientID%>').ajaxRequest();
            }
        </script>
    </telerik:RadScriptBlock>
   <h4>Customer Management</h4>

        <telerik:RadAjaxManager ID="RadAjaxManager2" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager2">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="Panel1" LoadingPanelID="AjaxLoadingPanel1" />

                </UpdatedControls>

            </telerik:AjaxSetting>
            
        </AjaxSettings>
    </telerik:RadAjaxManager>

      <telerik:RadAjaxLoadingPanel  ID="AjaxLoadingPanel1" runat="server" >
             <div ID="ProgressPanel" class ="overlay"  >
     
         <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index:10010;vertical-align:middle;" />      
       </div>
           </telerik:RadAjaxLoadingPanel>  

        <asp:Panel ID="Panel1" runat="server">      </asp:Panel> 

   

     <telerik:RadWindowManager EnableViewState = "false" ID="RadWindowManager1" Skin="Simple" runat="server" EnableShadow="true" CssClass="RadWindow-Confirm">
    </telerik:RadWindowManager>
                 
                            <asp:UpdatePanel ID="TopPanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                   
                                            
                                       <div class="row">
                                         <div class="col-sm-3">
                                            <div class="form-group">
                                                <label>Organization</label>
            
                <telerik:RadComboBox Skin="Simple" Filter="Contains"  ID="ddlOrganization"  Width ="100%" 
                    runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="true"> 
                 </telerik:RadComboBox>
              </div>
                                             </div>
               
                                         <div class="col-sm-3">
                                            <div class="form-group">
                                                <label>Name</label>
               <asp:TextBox ID="txtCustomerName" Width ="100%" CssClass="inputSM" runat="server"></asp:TextBox>
             </div>
              </div>
                                                    
                                         <div class="col-sm-3">
                                            <div class="form-group">
                                                <label>Customer No</label>
                                                 <asp:TextBox ID="txtCustomerNo" CssClass="inputSM" Width ="100%" runat="server"></asp:TextBox>
                                                </div>
    </div>

                                           <div class="col-sm-3">
                                            <div class="form-group">
                                                <label>&nbsp;</label>
                                                  <asp:Button CssClass ="btn btn-primary" ID="BtnSearch" runat="server" Text="Search" />
                     <asp:Button CssClass ="btn btn-default"  ID="Btn_Clear" runat="server" Text="Clear Filter" />
                                                </div>
                                               </div>

                                       
                                               
                                      
                                      </div>
                                    
                                </ContentTemplate>
                            </asp:UpdatePanel>
      




        <div class="mrgbtm">
                                            <div class="form-inline">
                                            <div class="form-group">
                                                    
                   
                     <asp:Button CssClass="btn btn-success" ID="BtnAdd" runat="server" Text="Add New Customers" />
                     <telerik:RadButton ID="btnExport" Skin="Simple" Text="Export Customers" runat="server" CssClass="btn btn-warning"></telerik:RadButton>
                     <telerik:RadButton ID="btndownloadTemplate" Skin="Simple" Text="Download Template" runat="server" CssClass="btn btn-danger"></telerik:RadButton>
                    <%-- <telerik:RadAsyncUpload ID="ExcelFileUpload" runat="server" TemporaryFolder="C:\inetpub\wwwroot\SWX_VS_V2\SalesWorx_BO\Temp" Skin="Simple" OnFileUploaded="ExcelFileUpload_FileUploaded"
                      Localization-Select="Upload"  OnClientFilesUploaded="OnClientFilesUploaded" MultipleFileSelection="Disabled" InitialFileInputsCount="1"MaxFileInputsCount="1" /> --%>
                    
                    <%-- <asp:Button ID="BtnUpload" runat="server" CssClass ="btn btn-primary" Text="Upload"  />--%>
                                              
    
                <telerik:RadAsyncUpload ID="ExcelFileUpload" runat="server" 
                    Skin="Simple" OnFileUploaded="ExcelFileUpload_FileUploaded" 
                    Localization-Select="Import Customer"  OnClientFilesUploaded="OnClientFilesUploaded"
                    MultipleFileSelection="Disabled" InitialFileInputsCount="1" 
                    MaxFileInputsCount="1"  />
          
                       
                                                    </div>

                                                <div class="pull-right">
                <label>
                    <a id="link1" href="#">
                        <asp:Image alt="Upload Info" ToolTip="Upload Info" ImageUrl="~/images/info.png" ID="upl" runat="server" Width="18px" Height="18px" /></a>
                    <telerik:RadToolTip RenderMode="Lightweight" runat="server" ID="RadToolTip1" RelativeTo="Element" Width="300px" AutoCloseDelay="30000" BackColor="WhiteSmoke"
                        Height="400px" TargetControlID="link1" IsClientID="true" Animation="None" Position="TopCenter">
                        <p style="color: darkolivegreen; padding-left: 10px; font-size: 12px;">
                            <b style="color: orchid;"><u>Upload Information</u></b><br />
                            New Customer  will be uploaded. Existing Customer and invalid rows are ignored.

           <br />
                            <br />
                            <b style="color: green;"><u>Validations</u></b><br />
                            <ul>
                               <li type="square">Customer No,Name and Sales_Org_ID columns are mandatory in the Customer Sheet .</li>
                                <li type="square">If Customer is a Credit Customer Credit Limit,Avail Balance  and Bill Credit Period columns are mandatory in the Customer Sheet .</li>
                                <li type="square">Customer No,Name and Sales_Org_ID columns are mandatory in the Customer Ship Address Sheet</li>
                                <li type="square">Existing Customers are updated.New Customers are inserted.</li>
                                <li type="square">Customers detail's Sheet name should be Customer.</li>
                                <li type="square">Customer Ship Address Sheet name should be Customer_Ship_Address.</li>
                            </ul>

                        </p>
                    </telerik:RadToolTip>

                    <asp:LinkButton ID="lbLog" runat="server" Text="View Uploaded Log" Font-Underline="true" CssClass="btn btn-link" ToolTip="Click here to view the uploaded log" ForeColor="Blue"
                         OnClick="lbLog_Click" ></asp:LinkButton>
                    <telerik:RadButton ID="btnClear" Skin="Simple" Visible="false" runat="server" CssClass="btn btn-default" Text="Reset">
                    </telerik:RadButton>
                </label>
            </div>
                                           </div>
   </div> 

   <div class="table-responsive">
   <asp:UpdatePanel ID="ClassUpdatePnl" runat="server" UpdateMode="conditional">
                                <ContentTemplate>
                                   
                  
                         
                                
                                         
                                                <asp:GridView ID="grdCustomer" runat="server" EmptyDataText="No Data to Display"
                                                    EmptyDataRowStyle-Font-Bold="true"  AutoGenerateColumns="False"
                                                    AllowPaging="True" AllowSorting="true"  PageSize="25" CellPadding="0" 
                                                    CellSpacing="0" CssClass="tablecellalign" Width="800">
                                                 
                                                    <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
                                                    <EmptyDataRowStyle Font-Bold="True" />
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" Width="100px" CssClass="display-block"  />
                                                            <ItemTemplate>
                                                               
                                                                <asp:ImageButton ID="btnEdit" ToolTip="Edit Customer" runat="server" CausesValidation="false"
                                                                    ImageUrl="~/images/edit-13.png"   OnClick="btnEdit_Click" />

                                                                <asp:ImageButton ID="btnView" ToolTip="View Customer" runat="server" CausesValidation="false"
                                                                    ImageUrl="~/images/view-13.png"   OnClick="btnView_Click" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" CssClass="display-block"  />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Customer_No" HeaderText="Customer No." SortExpression="Customer_No">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                         <asp:BoundField DataField="Customer_Name" HeaderText="Customer Name" SortExpression="Customer_Name">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                         <asp:BoundField DataField="Price_List" HeaderText="Price List" SortExpression="Price_List">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                         <asp:BoundField DataField="Cash_Cust" HeaderText="Cash Customer" SortExpression="Cash_Cust">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                       
                                                        <asp:BoundField DataField="CustStatus" SortExpression="CustStatus"  HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"  HeaderText="Status" >
                       
                     </asp:BoundField>
                   
                                                        <asp:TemplateField HeaderText=""  HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false">
                        <ItemTemplate>
                      
                                                   <asp:LinkButton  runat="server" ID="lblDisable"   text="Disable"  visible='<%# Bind("ShowDisable")%>'  OnClientClick="confirmLinkButtondis(this); return false;" OnClick ="lbChangeStatus_Click"></asp:LinkButton>
                             <asp:LinkButton  runat="server" ID="lblEnable"   text="Enable"  visible='<%# Bind("ShowEnable")%>'  OnClientClick="confirmLinkButton(this); return false;" OnClick ="lbChangeStatus_Click"></asp:LinkButton>
                                                     <asp:Label ID="lblStatus" runat ="server" Visible ="false"  Text='<%# Bind("Cust_Status") %>'></asp:Label>
                                                    <asp:Label ID="lblCustomer_ID" runat ="server" Visible ="false"  Text='<%# Bind("Customer_ID") %>'></asp:Label>
                                                    <asp:Label ID="lblSite_Use_ID" runat ="server" Visible ="false"  Text='<%# Bind("Site_Use_ID") %>'></asp:Label><asp:Label ID="lbETime" runat ="server" Visible ="false"  Text='<%# Bind("Customer_No") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                                   
                 
                                                          <asp:TemplateField HeaderText=""  HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false">
                        <ItemTemplate>
                      
                                                   <asp:LinkButton  runat="server" ID="lbCreditLimit"  Text='<%# Bind("CreditLimit")%>'     OnClick ="lbCreditLimit_Click" ></asp:LinkButton>
                                                     <asp:Label ID="Label1" runat ="server" Visible ="false"  Text='<%# Bind("Cust_Status") %>'></asp:Label>
                                                    <asp:Label ID="Label2" runat ="server" Visible ="false"  Text='<%# Bind("Customer_ID") %>'></asp:Label>
                                                    <asp:Label ID="Label3" runat ="server" Visible ="false"  Text='<%# Bind("Site_Use_ID") %>'></asp:Label>
                                                    <asp:Label ID="Label4" runat ="server" Visible ="false"  Text='<%# Bind("Customer_No") %>'></asp:Label>

                        </ItemTemplate></asp:TemplateField>


                                                    </Columns>
                                                   <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
                                                </asp:GridView>
                                          
                                       
                            
                                      
                             
                   
       </ContentTemplate>
   </asp:UpdatePanel>
</div> 

         
                <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="TopPanel"
                    runat="server">
                    <ProgressTemplate>
                        <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                            <img  src="../assets/img/ajax-loader.gif"  alt="Processing..."  />
                            <span>Processing... </span>
                        </asp:Panel>
                    </ProgressTemplate>
                </asp:UpdateProgress>


    <asp:UpdateProgress ID="UpdateProgress2" AssociatedUpdatePanelID="ClassUpdatePnl"
                    runat="server">
                    <ProgressTemplate>
                        <asp:Panel ID="Panel2" CssClass="overlay" runat="server">
                            <img  src="../assets/img/ajax-loader.gif"  alt="Processing..."  />
                            <span>Processing... </span>
                        </asp:Panel>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                
</asp:Content>
