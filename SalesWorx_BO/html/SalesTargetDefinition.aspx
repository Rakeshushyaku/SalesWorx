<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="SalesTargetDefinition.aspx.vb" Inherits="SalesWorx_BO.SalesTargetDefinition" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">

    <script type="text/javascript">
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
    function DisableValidation() {
        //            Page_ValidationActive = false;
        //            return true;

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
    function IntegerOnly(e) {

        var keycode;

        if (window.event) {
            keycode = window.event.keyCode;
        } else if (e) {
            keycode = e.which;
        }

        if ((parseInt(keycode) >= 48 && parseInt(keycode) <= 57) || parseInt(keycode) == 8 || parseInt(keycode) == 0)
            return true;

        return false;
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
    <script type="text/javascript">
        $(window).resize(function () {
            var win = $find('<%= MPEImport.ClientID %>');
           if (win) {
               if (!win.isClosed()) {
                   win.center();
               }
           }

       });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <h4>SKU Wise Target Definition</h4>
    <telerik:RadWindowManager ID="RadWindowManager1" Skin="Simple" runat="server" EnableShadow="true">
    </telerik:RadWindowManager>
    <asp:UpdatePanel ID="ClassUpdatePnl1" runat="server" UpdateMode="conditional">
        <contenttemplate>
                                <telerik:RadTabStrip ID="RadTabStrip1" runat="server" MultiPageID="RadMultiPage1" SelectedIndex="0">
            <Tabs>
                <telerik:RadTab runat="server" Text="Target Definition" PageViewID="PageView2" Selected="true" SelectedIndex="0">
                </telerik:RadTab>
                <telerik:RadTab runat="server" Text="Import Target Definition" PageViewID="PageView1" >
                </telerik:RadTab>
                <telerik:RadTab runat="server" Text="Export Target Definition" PageViewID="PageView3" >
                </telerik:RadTab>
            </Tabs>
        </telerik:RadTabStrip>
        <telerik:RadMultiPage ID="RadMultiPage1" runat="server"    Width="100%" SelectedIndex="0" >
            <telerik:RadPageView ID="PageView2" runat="server" BackColor="#ffffff" BorderStyle="solid" BorderColor="#cccccc" BorderWidth="1"  Selected="true" style="padding:15px;">
                 
                    <asp:UpdatePanel ID="TopPanel" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                        <div class="row">
                                            
                         <div class="col-sm-3">
                                            <div class="form-group">  <label>Organization</label>
                        <telerik:RadComboBox Skin="Simple" ID="ddlOrganization"  Width ="100%"
                    runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="true"></telerik:RadComboBox></div></div>

                           <div class="col-sm-3">
                                            <div class="form-group">  <label>Van/FSR </label>

                <telerik:RadComboBox Skin="Simple" ID="ddlVan" AutoPostBack="true" Width ="100%" runat="server"  CssClass="inputSM"></telerik:RadComboBox>
                    </div>
                               </div>
                             <div class="col-sm-3">
                                            <div class="form-group">  <label>Target Year </label>
                                                <telerik:RadComboBox Skin="Simple" ID="ddlYear" AutoPostBack="true" 
                                 Width ="100%" runat="server"  CssClass="inputSM"> </telerik:RadComboBox> 
                                                </div>
                                 </div>

                               <div class="col-sm-3">
                                            <div class="form-group">  <label>Target Month </label>
                                    <telerik:RadComboBox Skin="Simple" ID="ddlMonth" AutoPostBack="true"  Width ="100%" runat="server"  CssClass="inputSM">

                                    </telerik:RadComboBox> 
                                                </div>
                                   </div>

                             
                            
                             <div class="col-sm-12">
                                                <div class="form-group">

                                                     <asp:Button ID="Btn_Save" runat="server" CssClass ="btn btn-success" Text="Save" />
                                <asp:DropDownList ID="ddl_ValueType" AutoPostBack="True" Width ="200px"  CssClass="inputSM" Visible="false"
                    runat="server" ><asp:ListItem Value="Q">By Quantity</asp:ListItem><asp:ListItem Value="V">By Value</asp:ListItem>
                                    <asp:ListItem Value="B" Selected="True">Both</asp:ListItem></asp:DropDownList> 
               <asp:HiddenField ID="HSaveClick" runat="server" /> 
                                                    </div>
                                 </div>
                            </div>
                                                                                           </ContentTemplate></asp:UpdatePanel>

                                                                                                       
      <asp:UpdatePanel ID="ClassUpdatePnl" runat="server" UpdateMode="conditional"><ContentTemplate>
          <div class="text-right">
              <asp:Button CssClass="btn btn-success"  ID="Btn_SetValues" Visible="false" runat="server" Text="Load Target Value from Price List based on Qty" style="margin-bottom:10px;" /></div>
          <div class="table-responsive">
                    <telerik:RadGrid ID="Grd_Products" runat="server" Skin="Vista"  allowmultirowselection="True" 
                                            AutoGenerateColumns="False"  AllowPaging="true" AllowFilteringByColumn="True" PageSize="25" DataKeyNames="Item_Code"  ClientDataKeyNames="Item_Code">
                        <GroupingSettings CaseSensitive="false" />
                        <mastertableview width="100%" summary="RadGrid table" EditMode="InPlace" AllowFilteringByColumn="true" DataKeyNames="Item_Code"  ClientDataKeyNames="Item_Code">
                            <NoRecordsTemplate><div>There are no records to display</div></NoRecordsTemplate><Columns>
                                                <telerik:GridBoundColumn  UniqueName="Item_Code" DataField="Item_Code" HeaderText ="Item Code"   AllowFiltering="true" ShowFilterIcon="false" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" >
                                                    <HeaderStyle Width="150px" /></telerik:GridBoundColumn><telerik:GridBoundColumn UniqueName="Description"  DataField="Description" HeaderText ="Item Name"  AllowFiltering="true" ShowFilterIcon="false" AutoPostBackOnFilter="true"><HeaderStyle Width="200px" /></telerik:GridBoundColumn><telerik:GridBoundColumn UniqueName="Agency"  DataField="Agency" HeaderText ="Agency"  AllowFiltering="true" ShowFilterIcon="false" AutoPostBackOnFilter="true"><HeaderStyle Width="150px" /></telerik:GridBoundColumn><telerik:GridTemplateColumn UniqueName="TgtQty" AllowFiltering="false" 
                    InitializeTemplatesFirst="false" HeaderText="Qty"><HeaderStyle Width="40px"></HeaderStyle><ItemTemplate><asp:TextBox ID="txt_qty" runat="server" Text='<%# Bind("Target_Value_1") %>' onKeypress='return IntegerOnly(event)'></asp:TextBox></ItemTemplate><ItemStyle HorizontalAlign="Center"></ItemStyle></telerik:GridTemplateColumn><telerik:GridTemplateColumn UniqueName="TgtValue"  AllowFiltering="false" 
                    InitializeTemplatesFirst="false" HeaderText="Value"><HeaderStyle Width="40px"></HeaderStyle><ItemTemplate><asp:TextBox ID="txt_value" runat="server" Text='<%# Bind("Target_Value_2") %>' onKeypress='return NumericOnly(event)'></asp:TextBox></ItemTemplate><ItemStyle HorizontalAlign="Center"></ItemStyle></telerik:GridTemplateColumn></Columns></mastertableview><ClientSettings><Selecting AllowRowSelect="True"></Selecting></ClientSettings><pagerstyle mode="NextPrevAndNumeric"></pagerstyle></telerik:RadGrid></div></ContentTemplate></asp:UpdatePanel> </telerik:RadPageView>
                                  <telerik:RadPageView ID="PageView1" runat="server" BackColor="#ffffff" BorderStyle="solid" BorderColor="#cccccc" BorderWidth="1" style="padding:15px;">

                                      <div class="row">
                         <div class="col-sm-3">
                                            <div class="form-group">  <label>Organization </label>
                                                      <telerik:RadComboBox CssClass="inputSM" ID="ddl_importOrg"  Width ="100%"
                    runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" Skin="Simple"></telerik:RadComboBox> 
                                                </div>
                             </div>
                                              <div class="col-sm-3">
                                            <div class="form-group">  <label>Target Year</label>
                                                  <telerik:RadComboBox Skin="Simple" ID="ddl_ImportYear" AutoPostBack="true" Width ="100%" runat="server" CssClass="inputSM">

                                                  </telerik:RadComboBox>
                                                </div>
                                                  </div>
                                                <div class="col-sm-3">
                                            <div class="form-group">  <label>Target Month</label>

                                                      <telerik:RadComboBox ID="ddl_ImportMonth" runat="server" CssClass="inputSM" Width ="100%" 
                      Skin="Simple"  ></telerik:RadComboBox> 
                                                </div>
                                                    </div>
                                          <div class="col-sm-3">
                                            <div class="form-group">  <label>&nbsp;</label>
                                                <asp:Button ID="Btn_Import" runat="server" CssClass="btn btn-warning"
                       Text="Import" /> </div>
                                              </div>

                                          </div>                                  
                                          </telerik:RadPageView>
                            <telerik:RadPageView ID="RadPageView3" runat="server" BackColor="#ffffff" BorderStyle="solid" BorderColor="#cccccc" BorderWidth="1" style="padding:15px;">
                               
                                 <div class="row">
                         <div class="col-sm-3">
                                            <div class="form-group">  <label>Organization</label>
                                            <telerik:RadComboBox CssClass="inputSM" ID="ddl_ExportOrg"  Width ="100%"
                    runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="true" Skin="Simple"></telerik:RadComboBox>

                                                                                                 </div>
                             </div>
                                      <div class="col-sm-3">
                                            <div class="form-group">
                                        <label>Van/FSR </label>
                                            <telerik:RadComboBox ID="ddl_ExportVan"  CssClass="inputSM" runat="server" 
                    Width="100%" Skin="Simple" ></telerik:RadComboBox>
                                     </div>
                                </div>
                                      
                         <div class="col-sm-3">
                                            <div class="form-group"><label>Target Year</label>

                         <telerik:RadComboBox Skin="Simple" ID="ddl_ExportYear" AutoPostBack="true" Width ="100%" runat="server" CssClass="inputSM">

                                           </telerik:RadComboBox>
                                                </div>
                             </div>

                                                 <div class="col-sm-3">
                                            <div class="form-group"><label> Month</label>
                                               <telerik:RadComboBox ID="ddl_ExportMonth" runat="server" CssClass="inputSM" Width="100%"
                        Skin="Simple"></telerik:RadComboBox>
                                               

                                                </div>
                                                     </div>

                                     <div class="col-sm-12">
                                            <div class="">
                                         <asp:Button ID="Btn_Export" runat="server" CssClass="btn btn-warning" 
                                                               Text="Export" />
                                                </div>
                                         </div>

                                     </div>
                                                 </telerik:RadPageView>
                            </telerik:RadMultiPage>
         
           
             
        
                <telerik:RadWindow ID="MpPricelist" Title= "Load Target Value" runat="server" Skin="Windows7" Behaviors="Move,Close"
                                                Width="430px" Height="310px" ReloadOnShow="false" VisibleStatusbar="false" Overlay="true" Modal="false">
                                              <ContentTemplate>
                                                 
                                                    <span style ="padding-left:5px;padding-top:10px;"><asp:Label ID="lblPop"  CssClass ="txtSMBold" runat ="server" ForeColor ="Red" ></asp:Label>
                                                        </span>

                                                    <table id="table1"   cellpadding="5" style="background-color: White;" width="100%">
                                                     <tr align="center">
                                                            <td colspan="2">
                                                                <asp:Label ID="lbl_Title" runat="server" Font-Size ="13px" Visible="false"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr align="center">
                                                            <td colspan="2" align="center" style="color:Green;padding: 3px" >
                                                                <asp:Label ID="LabelMsg" runat="server" Font-Size ="13px" Text="The Target Value will be loaded based on the Target quantity you entred and the price list selection."></asp:Label>
                                                                
                                                            </td>
                                                        </tr>
                                                        
                                                        <tr>
                                                            <td align="center" style="color:Black;padding: 3px">
                                                                <asp:Label ID="Label2" runat="server" Font-Size ="13px" Text="Price List"></asp:Label>
                                                            </td>
                                                            <td align="left" style="text-align: center">
                                                                <telerik:RadComboBox ID="ddl_priceList" runat="server"  Skin ="Simple" Width="250" >
                                                                </telerik:RadComboBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" style="text-align: center;" colspan="2"> 
                                                                            <asp:Button ID="Btn_applyValue" runat="server" Text="OK"  CssClass="btn btn-success" />
                                                                            <asp:Button ID="btnClosethis" runat="server" Text="Cancel" CssClass ="btn btn-danger"  />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                 </ContentTemplate>
                               
                            
                                                    </telerik:RadWindow> 
                                                             
                                           
               
              
                                        
                                         
                                                   
                                         
<telerik:RadWindow ID="MPEImport" Title= "Import target definition" runat="server" Skin="Windows7" Behaviors="Move,Close"
                                                AutoSize="true" ReloadOnShow="false" VisibleStatusbar="false" Overlay="true" Modal="true">
                                              <ContentTemplate>
                                                  <asp:HiddenField ID="HFV" runat="server" Value="-1" />
                                                  <div class="popupcontentblk">
                                                      <p><asp:Label runat ="server" ID="lblUpMsg"  ForeColor ="Red" > </asp:Label></p>
                                                     <div><asp:Label runat ="server" ID="Label6" CssClass="text-danger"  Text ="Note: Uploading a FSR target data removes any existing target data for the month specified in excel file."></asp:Label><br /><br /></div>
                                                      <div class="row">
		                                                <div class="col-sm-5">
			                                                <label>Select a File</label>
		                                                </div>
		                                                <div class="col-sm-7">
			                                                <div class="form-group">
                                                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="conditional">
                                                                        <ContentTemplate><asp:FileUpload ID="ExcelFileUpload" runat="server" /></ContentTemplate>
                                                                    </asp:UpdatePanel>  
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
		                                                <div class="col-sm-5"></div>
		                                                <div class="col-sm-7">
			                                                <div class="form-group">
                                                                <asp:Button ID="btnImport" runat="server" Text="Import"  CssClass="btn btn-warning" /> <asp:Button ID="btnCancelImport" CssClass ="btn btn-default" TabIndex="5" runat="server"
                                                                                        CausesValidation="false" Text="Cancel" />
                                                                      <asp:Button ID="DummyImBtn" style="display:none"  runat="server" Text="Reimport" />
                                                               <asp:Button ID="BtnReimport" runat="server" Text="Reimport"  Visible ="false" 
                                                                     CssClass="btnInputBlue" />
                                                               <asp:Button ID="DummyReimBtn" style="display:none"  runat="server" Text="Reimport" />
                                                                <span style ="text-decoration: underline !important;Color:#337AB7;"> <asp:LinkButton ID="lbLog" ToolTip ="Click here to download the uploaded log" runat ="server"
                                                                   Text ="View Log" Visible="false"></asp:LinkButton></span>
                                                            </div>
                                                        </div>
                                                    </div>

                   
                                <asp:UpdatePanel runat="server" ID="UpPanel">
                                    <Triggers>
                                      <asp:AsyncPostBackTrigger ControlID="DummyImBtn" EventName="Click" />
	<asp:AsyncPostBackTrigger ControlID="DummyReimBtn" EventName="Click" />
	
                                    </Triggers>
                                </asp:UpdatePanel>
                            
                        
                                                      </div>
                                              
                                            </ContentTemplate>
                               
                            
                                                    </telerik:RadWindow> 
                                </contenttemplate>
        <triggers>
            <asp:PostBackTrigger ControlID="Btn_Export" /> 
           
	
        </triggers>
    </asp:UpdatePanel>





    <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="ClassUpdatePnl1"
        runat="server">
        <progresstemplate>
            <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                <img  src="../assets/img/ajax-loader.gif" alt="Processing..." />
                <span>Processing... </span>
            </asp:Panel>
        </progresstemplate>
    </asp:UpdateProgress>


</asp:Content>
