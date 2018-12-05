<%@ Page Title="Sales Org Configuration" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master"
    CodeBehind="ManageDeliveryCalenderold.aspx.vb" Inherits="SalesWorx_BO.ManageDeliveryCalenderold" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="FeaturedContent" runat="server">
   <script language="javascript" type="text/javascript">


     

      
   
      
        



  

       

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

        function CloseWindow(sender, args) {
    
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
    <h4>Manage DeliveryCalender</h4>

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

        <asp:Panel ID="Panel2" runat="server">      </asp:Panel> 
                         
 

        <div class="mrgbtm">
        <div class="form-inline">
            <div class="form-group">
            <%--    <telerik:RadButton ID="RadButton1" AutoPostBack="true" Skin="Simple" Text="Add New Product" runat="server" CssClass="btn btn-success"></telerik:RadButton>--%>
                <telerik:RadButton ID="btnExport" Skin="Simple" Text="Export Exception Dates" runat="server" CssClass="btn btn-danger"></telerik:RadButton>
                <telerik:RadButton ID="btndownloadTemplate" Skin="Simple" Text="Download Template" runat="server" CssClass="btn btn-primary"></telerik:RadButton>

            </div>
            <div class="form-group">
                <telerik:RadAsyncUpload ID="ExcelFileUpload" runat="server"
                    Skin="Simple" OnFileUploaded="ExcelFileUpload_FileUploaded"
                    Localization-Select="Import Exception Dates" OnClientFilesUploaded="OnClientFilesUploaded"
                    MultipleFileSelection="Disabled" InitialFileInputsCount="1"
                    MaxFileInputsCount="1" />
            </div>


            <div class="pull-right">
                <label>
                    <a id="link1" href="#">
                        <asp:Image alt="Upload Info" ToolTip="Upload Info" ImageUrl="~/images/info.png" ID="upl" runat="server" Width="18px" Height="18px" /></a>
                    <telerik:RadToolTip RenderMode="Lightweight" runat="server" ID="RadToolTip1" RelativeTo="Element" Width="300px" AutoCloseDelay="30000" BackColor="WhiteSmoke"
                        Height="360px" TargetControlID="link1" IsClientID="true" Animation="None" Position="TopCenter">
                        <h5>Upload Information</h5>
                        <p>Exception dates will be uploaded. </p>
                        <hr />
                        <h5>Validations</h5>
                        <ul style="padding: 0 0 0 15px; margin: 0; list-style-type: disc;">
                            <li>Organization,date and Is_working columns are mandatory in Exception dates.</li>
                            <li>Is_working value should be either Y or N.</li>
                            <li>Existing dates are updated.New dates are inserted.</li>
                            <li>Sheet name should be ExceptionDates.</li>
                        </ul>

                    </telerik:RadToolTip>

                    <asp:LinkButton ID="lbLog" runat="server" Text="View Uploaded Log" Font-Underline="true" CssClass="btn btn-link" ToolTip="Click here to view the uploaded log" ForeColor="Blue"
                        OnClick="lbLog_Click"></asp:LinkButton>
                    <telerik:RadButton ID="btnClear" Skin="Simple" Visible="false" runat="server" CssClass="btn btn-default" Text="Reset">
                    </telerik:RadButton>
                </label>
            </div>
        </div>


    </div>


    
                    

        <telerik:RadWindowManager EnableViewState = "false" ID="RadWindowManager1" Skin="Simple" runat="server" EnableShadow="true" CssClass="RadWindow-Confirm">
    </telerik:RadWindowManager>



                            <asp:UpdatePanel ID="TopPanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                  
                                    <div class="row">
                                             <div class="col-sm-6 col-md-4 col-lg-6">
                                    <div class="form-group">
                                                <label>Organization<em><span>&nbsp;</span>*</em></label>
                                                <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddlOrganization"  Width ="100%"
                    runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="true">
                </telerik:RadComboBox>
                                            </div>
                                            </div>
                                            <div class="col-sm-6 col-md-4 col-lg-3">
                                                <div class="form-group"> 
                                           

                <label>Normal Holidays<em><span>&nbsp;</span></em></label>                                     
<%-- <asp:CheckBoxList id="chklst_day" RepeatDirection ="Horizontal" runat="server">
 
         <asp:ListItem Text="Sun" Value="1"></asp:ListItem>
         <asp:ListItem Text="Mon" Value="2"></asp:ListItem>
         <asp:ListItem Text="Tues" Value="3"></asp:ListItem>
         <asp:ListItem Text="Wednes" Value="4"></asp:ListItem>
         <asp:ListItem Text="Thurs" Value="5"></asp:ListItem>
         <asp:ListItem Text="Fri" Value="6"></asp:ListItem>
         <asp:ListItem Text="Saturday" Value="7"></asp:ListItem>
      </asp:CheckBoxList>--%>
 <telerik:RadComboBox Skin="Simple" ID="ddlday" EmptyMessage="Select holidays"  CheckBoxes="true" EnableCheckAllItemsCheckBox="true"   Width ="100%" runat="server" Filter="Contains">
                                       
      <Items>   
        <telerik:RadComboBoxItem runat="server" Text="Sunday" Value="1" />   
        <telerik:RadComboBoxItem runat="server" Text="Monday" Value="2" />   
        <telerik:RadComboBoxItem runat="server" Text="Tuesday"  Value="3" /> 
        <telerik:RadComboBoxItem runat="server"  Text="Wednesday" Value="4"/>
        <telerik:RadComboBoxItem runat="server"  Text="Thursday" Value="5"/>
        <telerik:RadComboBoxItem runat="server"  Text="Friday" Value="6"/>
        <telerik:RadComboBoxItem runat="server" Text="Satur" Value="7"/>

    </Items>
      </telerik:RadComboBox >




                                                </div>
                                             </div>
                                        <div class="col-sm-6 col-md-4 col-lg-3">
                                                 <div class="form-group">   
                                                <label class="hidden-xs hidden-sm">&nbsp;</label>
                                        <telerik:RadButton ID="btnSave_holiday" Skin ="Simple"    runat="server" Text="Save" CssClass ="btn btn-success" OnClick ="btnSave_holiday_Click"  />
                                                          <telerik:RadButton ID="btnAdd" Skin="Simple" OnClick ="btnAdd_Click" runat="server" Text="Add Exception" CssClass="btn btn-success" />
                                            <asp:Button ID="btnImport" runat="server" Visible ="false"  CausesValidation="false" CssClass="btnInputGreen"
                                                    TabIndex="2" Text="Import"  />
                                        </div>
                                    </div>
                                </div>
                                    <div class="row">


                                                 <div class="col-sm-6 col-md-4 col-lg-3">
                                            <div class="form-group">
                                                <label>From Date</label>
                                                <telerik:RadDatePicker ID="txtFromDate"   Width ="100%" runat="server">
                                                        <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy">
                                                        </DateInput>
                                                        <Calendar ID="Calendar1" runat="server">
                                                            <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                        </Calendar>
                                                    </telerik:RadDatePicker>
              </div>
                                          </div>
              <div class="col-sm-6 col-md-4 col-lg-3">
                                                    <div class="form-group">
                                                        <label>To Date</label>
                                                             <telerik:RadDatePicker ID="txtToDate"  Width ="100%"  runat="server">
                                                        <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy">
                                                        </DateInput>
                                                        <Calendar ID="Calendar3" runat="server">
                                                            <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                        </Calendar>
                                                    </telerik:RadDatePicker>

              </div>
                                          </div>
                                        <div class="col-sm-6 col-md-4 col-lg-3">
                                    <div class="form-group">
                                                <label>Working<em><span>&nbsp;</span>*</em></label>
                                                <telerik:RadComboBox ID="ddFilterBy" Skin="Simple"  
                    Width="100%" Height="250px" TabIndex="2"
                    runat="server">
                    <Items>

                        <telerik:RadComboBoxItem Selected="True" Text="All"></telerik:RadComboBoxItem>
                        <telerik:RadComboBoxItem Value="Y" Text="YES"></telerik:RadComboBoxItem>
                        <telerik:RadComboBoxItem Value="N" Text="NO"></telerik:RadComboBoxItem>
                       
                    </Items>
                </telerik:RadComboBox>
                                            </div>
                                            </div>

                                            <div class="col-sm-6 col-md-4 col-lg-3">
                                                 <div class="form-group">   
                                                <label class="hidden-xs hidden-sm">&nbsp;</label>
                                                      
                                                     <telerik:RadButton ID="btnFilter" Skin ="Simple"    runat="server" Text="Search" CssClass ="btn btn-primary" />
                                            
                                                      <telerik:RadButton ID="btnReset" Skin="Simple" OnClick ="btnReset_Click" runat="server" Text="Reset" CssClass="btn btn-default" />
                                                      
                                                     </div>
                                                </div>
                                        </div> 
                                    
                                </ContentTemplate>
                            </asp:UpdatePanel>
                   <div class="table-responsive">
                            <asp:UpdatePanel ID="ClassUpdatePnl" runat="server" UpdateMode="conditional">
                                <ContentTemplate>
                                    
                                                  <asp:GridView Width="100%" ID="gvDlvClndr" runat="server" EmptyDataText="No Delivery Claender details found."
                                                    EmptyDataRowStyle-Font-Bold="true"  AutoGenerateColumns="False"
                                                    AllowPaging="True" AllowSorting="true"   PageSize="10" CellPadding="0" CellSpacing="0" CssClass="tablecellalign">
                                                    <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
                                                    <EmptyDataRowStyle Font-Bold="True" />
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <HeaderStyle width="75" />
                                                              
                                                                
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" Width="100px" CssClass="display-block"  />
                                                            <ItemTemplate>
                                                              
                                                                
                                                                <asp:ImageButton ToolTip="Delete Configuration " ID="btnDelete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Row_ID")%>'
                                                                    OnClick="btnDelete_Click" runat="server" CausesValidation="false" 
                                                                     ImageUrl="~/images/delete-13.png" 
                                                                    OnClientClick="javascript:return confirm('Would you like to delete the selected expection date?');"/>

                                                                <asp:ImageButton ID="btnEdit" ToolTip="Edit Configuration" runat="server" CausesValidation="false"
                                                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Row_ID")%>'
                                                                      ImageUrl="~/images/edit-13.png"   OnClick="btnEdit_Click" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" CssClass="display-block"  />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Delivery_Date" HeaderText="Date" SortExpression="Delivery_Date" DataFormatString="{0:dd/MMM/yyyy}">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                      
                                                        <asp:BoundField DataField="Working" HeaderText="Working " SortExpression="Working">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                                                                         
                                                        
                                                        
                                                    </Columns>
                                                    <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
                                                </asp:GridView> 

                                            


                                      <script type="text/javascript">
                                          $(window).resize(function () {
                                              var win = $find('<%= MPEDetails.ClientID%>');
                        if (win) {
                            if (!win.isClosed()) {
                                win.center();
                            }
                        }

                    });
                </script>

                                    <telerik:RadWindow ID="MPEDetails" Title= "Delivery Clander Exception Details" runat="server" Skin="Windows7" Behaviors="Move,Close"
                                                AutoSize="true" ReloadOnShow="false" VisibleStatusbar="false" Overlay="true" Modal="true">
                                              <ContentTemplate>
                                                   
                                                   <asp:UpdatePanel ID="UpdatePanel1" runat="server"  UpdateMode="conditional">
                                                  <ContentTemplate>
                                        <asp:HiddenField ID="HidVal" runat="server" Value="-1" />
                                        
                                                  <div class="popupcontentblk">
                                                      <p><asp:Label ID="lblPop" runat="server" Text="" ForeColor="Red"></asp:Label></p>
                                                    
                                                   


                                                       <div class="row">
                                                        <div class="col-sm-5">
                                                            <label>Organization</label>
                                                        </div>
                                                        <div class="col-sm-7">
                                                            <div class="form-group">
                                                     <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddlOrganization_add"  Width ="100%"
                    runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="true">
                </telerik:RadComboBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    
                                                  
                                                    <div class="row">
                                                        <div class="col-sm-5">
                                                            <label>Date</label>
                                                        </div>
                                                        <div class="col-sm-7">
                                                            <div class="form-group">
                                                        <telerik:RadDatePicker ID="txtExDate"   Width ="100%" runat="server">
                                                        <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy">
                                                        </DateInput>
                                                        <Calendar ID="Calendar2" runat="server">
                                                            <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                        </Calendar>
                                                    </telerik:RadDatePicker>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-sm-5">
                                                            <label>Working</label>
                                                        </div>
                                                        <div class="col-sm-7">
                                                            <div class="form-group">
                                                            <asp:RadioButtonList ID="rdo_Working" runat="server" TabIndex="18" RepeatDirection="Horizontal"  >
                                                                  <asp:ListItem Value="Y">Yes</asp:ListItem>
                                                                  <asp:ListItem Value="N" Selected="True" >No</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    
                                                   
                                                  
                                                    <div class="row">
                                                        <div class="col-sm-5">
                                                            <label></label>
                                                        </div>
                                                        <div class="col-sm-7">
                                                            <asp:Button ID="btnSave" CssClass ="btn btn-success" TabIndex="7" OnClick="btnSave_Click" runat="server" Text="Save" />
                                                            <asp:Button ID="btnUpdate" CssClass ="btn btn-success" Text="Update" OnClick="btnUpdate_Click" runat="server" />
                                                           <telerik:RadButton ID="btnCancel" Skin="Simple" runat="server" Text="Cancel" TabIndex="18" OnClientClicked="CloseWindow" AutoPostBack="false" CssClass="btn btn-default" />        
                                        
                                        
                                                                                       </div>
                                                        
                                                    </div>
                                                      <asp:Panel ID="Panel12" runat="server" Style="display: none" CssClass="overlay">
                                                            <img alt="Processing..." src="../assets/img/ajax-loader.gif" />
                                                            <span>Processing... </span>
                                                        </asp:Panel>
                                       
                                    </div>


                                </ContentTemplate>

                        </asp:UpdatePanel>
                                 </ContentTemplate>
                                                    </telerik:RadWindow> 
                                    
                                </ContentTemplate>
                                
                            </asp:UpdatePanel>
                       </div>
                 <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="ClassUpdatePnl"
                    runat="server">
                    <ProgressTemplate>
                        <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                            <img src="../assets/img/ajax-loader.gif" alt="Processing..." />
                            <span>Processing... </span>
                        </asp:Panel>
                    </ProgressTemplate>
                </asp:UpdateProgress>
               
</asp:Content>
