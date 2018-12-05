<%@ Page Title="Incentive Target Management " Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master"
    CodeBehind="ManageIncentiveTarget.aspx.vb" Inherits="SalesWorx_BO.ManageIncentiveTarget" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <style>
        .RadCalendarFastNavPopup {
            z-index: 6006 !important;
        }
        html body .RadInput_Default .riTextBox {
            height:auto;
        }
        .RadPicker .rcSelect {
            height: 31px;
        }
        .RadPicker.RadMonthYearPicker .rcCalPopup {
            background-image: none;
        }
         .RadMonthYearPicker {
            position:relative;
        }
        .RadMonthYearPicker[disabled="disabled"]:after {
            position:absolute;
            top:0;
            left:0;
            width:100%;
            height:32px;
            content:"";
            z-index: 2;
        }
    </style>
    <script type="text/javascript">


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
        function TestCheckBox() {
            if (TargetBaseControl == null) return false;
            var TargetChildControl = "chkDelete";
            var TimeCon = document.getElementById('<%= Me.ClassUpdatePnl.ClientID %>');

          var Inputs = TargetBaseControl.getElementsByTagName("input");

          for (var n = 0; n < Inputs.length; ++n)
              if (Inputs[n].type == 'checkbox' && Inputs[n].id.indexOf(TargetChildControl, 0) >= 0 && Inputs[n].checked) {
                  return confirm('Would you like to delete the selected currency code?');
                  return true;
              }
          alert('Select at least one Currency Code!');
          return false;

      }

      function checkconfirm(status) {
          if (status == "Y")
              return confirm("Are you sure to disable this parameter?")
          else
              return confirm("Are you sure to enable this parameter?")
      }

      function CheckAll(cbSelectAll) {
          if (TargetBaseControl == null) return false;
          var TargetChildControl = "chkDelete";
          var TimeCon = document.getElementById('<%= Me.ClassUpdatePnl.ClientID %>');

            var Inputs = TargetBaseControl.getElementsByTagName("input");

            for (var n = 0; n < Inputs.length; ++n)
                if (Inputs[n].type == 'checkbox' && Inputs[n].id.indexOf(TargetChildControl, 0) >= 0) {
                    Inputs[n].checked = cbSelectAll.checked;
                }

        }






        function ConfirmDelete(msg, event) {

            var ev = event ? event : window.event;
            var callerObj = ev.srcElement ? ev.srcElement : ev.target;
            var callbackFunctionConfirmDelete = function (arg, ev) {
                if (arg) {
                    callerObj["onclick"] = "";
                    if (callerObj.click) callerObj.click();
                    else if (callerObj.tagName == "A") {
                        try {
                            eval(callerObj.href)
                        }
                        catch (e) { }
                    }
                }
            }
            radconfirm(msg, callbackFunctionConfirmDelete, 330, 100, null, 'Confirmation');
            return false;
        }

        function RadConfirm(sender, args) {
            var callBackFunction = Function.createDelegate(sender, function (shouldSubmit) {
                if (shouldSubmit) {
                    this.click();
                }
            });

            var text = "Would you like to release this Collection?";
            radconfirm(text, callBackFunction, 350, 150, null, "Confirmation");
            args.set_cancel(true);
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
     <asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
         

          <h4>Incentive Target  Management</h4>


        


         <telerik:RadAjaxLoadingPanel  ID="AjaxLoadingPanel1" runat="server" >
             
             

             
             


 <div ID="ProgressPanel" class ="overlay"  >
                 <img src="../assets/img/ajax-loader.gif" alt="Processing..." />
               <span>Processing... </span>
</div>
     </telerik:RadAjaxLoadingPanel>  

        <asp:Panel ID="Panel1" runat="server">      </asp:Panel> 
                         
 
    
                                         
                     <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
                         
                         
                         
                         
                         
 </telerik:RadWindowManager>
      
        
                            <asp:UpdatePanel ID="TopPanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                       <div class="row">

                                            <div class="col-sm-3">
                                            <div class="form-group">
                                                <label>
                                               Select Organization
                                            </label>
                                                <telerik:RadComboBox Skin="Simple"  ID="ddlorg_F" Width="100%" Height="250px" TabIndex="2" runat="server">
                                                  
                                                </telerik:RadComboBox>
                                            </div>
                                        </div>

                                         <div class="col-sm-2">
                                            <div class="form-group">
                                                <label>
                                               Incentive Month
                                            </label>
                                                <telerik:RadMonthYearPicker RenderMode="Lightweight" ID="monthpicker_F" runat="server"  Width="100%">
                                                     </telerik:RadMonthYearPicker>
                                                 
                                            </div>
                                        </div>
                                        <div class="col-sm-2">
                                            <div class="form-group">
                                                <label>
                                                Filter By
                                            </label>
                                                <telerik:RadComboBox Skin="Simple"  ID="ddFilterBy" Width="100%" Height="250px" TabIndex="2" runat="server">
                                                    <Items>
                                                        <telerik:RadComboBoxItem Selected="True" Text="All"></telerik:RadComboBoxItem>
                                                        <telerik:RadComboBoxItem Value="Parameter Code" Text="Parameter Code"></telerik:RadComboBoxItem>
                                                        <telerik:RadComboBoxItem Value="Emp Code" Text="Emp Name"></telerik:RadComboBoxItem>
                                                   </Items>
                                                </telerik:RadComboBox>
                                            </div>
                                        </div>
                                        <div class="col-sm-2">   
                                            <div class="form-group">  
                                                   <label>
                                                &nbsp;
                                            </label> 
                                             <telerik:RadTextBox  runat="server" ID="txtFilterVal" EmptyMessage="Enter Filter Value"  Width="100%"></telerik:RadTextBox>
                                          </div>
                                        </div>
                                           <div class="col-sm-3">   
                                            <div class="form-group">  
                                                   <label>
                                                &nbsp;
                                            </label> 
                                                <telerik:RadButton ID="btnFilter" Skin ="Simple"    runat="server" Text="Search" CssClass ="btn btn-primary" OnClick="btnFilter_Click" />
                                                <telerik:RadButton ID="btnReset" Skin ="Simple"    runat="server" Text="Reset" OnClick="btnReset_Click" CssClass ="btn btn-default" />
                                                </div>
                                               </div>
                                     
                                 </div> 
                                    <div  class="row">

                                           <div class="col-sm-12">   
                                            <div class="form-group">   
                                                <label>
                                             
                                            </label>    
                                                 
                                                 <telerik:RadButton ID="btnAdd" Skin="Simple" OnClick ="btnAdd_Click" runat="server" Text="Add" CssClass="btn btn-success" />
                                                  
                                                   <telerik:RadButton ID="btn_Import" Skin="Simple" OnClick ="btn_Import_Click" runat="server" Text="Import" CssClass="btn btn-warning" />
                                                  <telerik:RadButton ID="btnExport" Skin="Simple" Text="Export Target" runat="server" CssClass="btn btn-info" OnClick="btnExport_Click"></telerik:RadButton>
                                            <asp:LinkButton ID="lbLog" runat="server" Text="View Uploaded Log" Font-Underline="true" CssClass="btn btn-link" ToolTip="Click here to view the uploaded log" ForeColor="Blue"
                            OnClick ="lbLog_Click" ></asp:LinkButton>   
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>


                                <Triggers>
                                            <asp:PostBackTrigger ControlID="btnExport" />
                                             <asp:PostBackTrigger ControlID="lbLog" />
                                            </Triggers>
                            </asp:UpdatePanel>
                  <div class="table-responsive">
                            <asp:UpdatePanel ID="ClassUpdatePnl" runat="server" UpdateMode="conditional">
                                <ContentTemplate>
                                 
                                                <asp:GridView Width="100%" ID="gvIncentiveTarget" runat="server" EmptyDataText="No Incentive Target to Display"
                                                    EmptyDataRowStyle-Font-Bold="true"  AutoGenerateColumns="False" 
                                                    AllowPaging="True" AllowSorting="true"  PageSize="15" CellPadding="0" CellSpacing="0" CssClass="tablecellalign">
                                                 
                                                    <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
                                                    <Columns>

                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                              
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" Width="100px" CssClass="display-block"  />
                                                            <ItemTemplate>
                                                                 <asp:LinkButton  runat="server" CommandName="Status" ID="lbChangeStatus_Target"  Text='<%# Bind("Status")%>'  OnClientClick=<%# "javascript:return checkconfirm('" + Eval("Is_Active") + "');"%> OnClick ="lbChangeStatus_Target_Click"  ></asp:LinkButton>
                                                                <asp:Label ID="lblROW_ID" runat ="server" Visible ="false"  Text='<%# Bind("ROW_ID")%>'></asp:Label>
                                                               <asp:Label ID="lblActive" runat ="server" Visible ="false"  Text='<%# Bind("Is_Active")%>'></asp:Label> 
                                                                  <asp:Label ID="lblOrg" runat ="server" Visible ="false"  Text='<%# Bind("Organization_ID")%>'></asp:Label> 
                                                                  <asp:Label ID="lblMonth" runat ="server" Visible ="false"  Text='<%# Bind("Incentive_Month")%>'></asp:Label> 
                                                                  <asp:Label ID="lblPcode" runat ="server" Visible ="false"  Text='<%# Bind("Parameter_Code")%>'></asp:Label> 
                                                                  <asp:Label ID="lblempcode" runat ="server" Visible ="false"  Text='<%# Bind("Emp_Code")%>'></asp:Label> 
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" CssClass="display-block"  />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" Width="100px" CssClass="display-block"  />
                                                            <ItemTemplate>
                                                               <asp:ImageButton ID="btnEdit" ToolTip="Edit Incentive Target" runat="server" CausesValidation="false"
                                                                    ImageUrl="~/images/edit-13.png" OnClick="btnEdit_Click" CssClass="checkboximgvalign" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" CssClass="display-block"  />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Code_Description" HeaderText="Parameter" SortExpression="Code_Description">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                         <asp:BoundField DataField="Emp_Name" HeaderText="Emp Name" SortExpression="Emp_Name">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Target" DataFormatString="{0:#,###.00}"  HeaderText="Target"
                                                            SortExpression="Target">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Tmonth" HeaderText="Incentive Month" SortExpression="Tmonth">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Incentive_Year" HeaderText="Incentive Year" SortExpression="Incentive_Year">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblRowID" runat="server" Text='<%# Bind("Row_ID")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                   <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
                                                </asp:GridView>

                                    <script type="text/javascript">
                                        $(window).resize(function () {
                                            var win = $find('<%= MPETarget.ClientID%>');
                                            if (win) {
                                                if (!win.isClosed()) {
                                                    win.center();
                                                }
                                            }

                                        });
                                    </script> 
                                           
                                    <telerik:RadWindow ID="MPETarget" Title= "Incentive Target  Details" runat="server" Skin="Windows7" Behaviors="Move,Close"
                                                AutoSize="true" ReloadOnShow="false" VisibleStatusbar="false" Overlay="true" Modal="true">
                                              <ContentTemplate>

                           <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="conditional">
                                <ContentTemplate>

                               <asp:HiddenField ID="HidVal" runat="server" Value="-1" />
                                    <div class="popupcontentblk">
                                        <p><asp:Label ID="lblPop"  runat ="server" ForeColor ="Red" ></asp:Label></p>
                                        <div class="row">


                                               <div class="col-sm-5">
                                               <label> Organization</label>
                                            </div>
                                            <div class="col-sm-7">
                                                <div class="form-group">
                                                    <telerik:RadComboBox ID="ddl_org" Width ="100%" runat="server" AutoPostBack="true"   TabIndex="4" Skin="Simple" Filter="Contains" OnSelectedIndexChanged="ddl_org_SelectedIndexChanged" >
                                                      
                                                    </telerik:RadComboBox>
                                                                                                                                            
                                                </div>
                                            </div>

                                             <div class="col-sm-5">
                                                <label>Incentive Month</label>
                                            </div>
                                            <div class="col-sm-7">
                                                <div class="form-group">
                                                    <telerik:RadMonthYearPicker RenderMode="Lightweight" ID="month_picker" AutoPostBack ="true"  runat="server" Width="238px" OnSelectedDateChanged ="month_picker_SelectedDateChanged">
                                                     </telerik:RadMonthYearPicker>
                                                </div>
                                            </div>

                                            <div class="col-sm-5">
                                                <label>Parameter </label>
                                            </div>
                                            <div class="col-sm-7">
                                                <div class="form-group">
                                                      <telerik:RadComboBox ID="ddl_ParameterCode" Width ="100%" runat="server" TabIndex="4" Skin="Simple" Filter="Contains">
                                                      
                                                    </telerik:RadComboBox>
                                                </div>
                                            </div>
                                            
                                                                                                                              
                                         
                                            
                                            
                                               <div class="col-sm-5">
                                                <label>Emp name </label>
                                            </div>
                                            <div class="col-sm-7">
                                                <div class="form-group">
                                                  <telerik:RadComboBox ID="ddl_empcode" Width ="100%" runat="server" TabIndex="4" Skin="Simple" Filter="Contains">
                                                      
                                                    </telerik:RadComboBox>
                                                </div>
                                            </div>
                                            <div class="col-sm-5">
                                                <label>Target</label>
                                            </div>
                                            <div class="col-sm-7">
                                                <div class="form-group">
                                                     <telerik:RadNumericTextBox RenderMode="Lightweight" runat="server" ID="txttarget"   Width="190px" MinValue="0"  >

                                                          <NumberFormat AllowRounding="False" DecimalDigits="4"/>  
                                                     </telerik:RadNumericTextBox>
                                                </div>
                                            </div>
                                           

                                                <div class="col-sm-1">
                                               </div>
                                            <div class="col-sm-11">
                                                <div class="form-group">
                                                     <p><asp:Label ID="lbl_workingdays"  runat ="server" ForeColor ="Red" ></asp:Label></p>                                                </div>
                                            </div>

                                            <div class="col-sm-5">
                                                
                                            </div>
                                            <div class="col-sm-7">
                                                <div class="">
                                                    <telerik:RadButton ID="btnSave" Skin="Simple" CssClass ="btn btn-success" 
                                                           OnClick="btnSave_Click"  ValidationGroup ="valsum"
                                                            runat="server" Text="Save" TabIndex ="6" >
                                                      </telerik:RadButton>

                                                                     <telerik:RadButton ID="btnUpdate" Skin="Simple" CssClass ="btn btn-success" 
                                                                           OnClick="btnUpdate_Click"  ValidationGroup ="valsum"
                                                                            runat="server" Text="Update" TabIndex ="6" >
                                                      </telerik:RadButton>

                                                                     <telerik:RadButton ID="btnCancel" Skin="Simple"  CssClass ="btn btn-default" 
                                                                         OnClick="btnCancel_Click" ValidationGroup ="valsum"
                                                                            runat="server" Text="Cancel" TabIndex ="7" >
                                                      </telerik:RadButton>
                                                </div>
                                            </div>
                                        </div>
                                        <asp:Panel ID="Panel12" runat="server" Style="display: none" CssClass="overlay">
                                                        <img alt="Processing..." src="../assets/img/ajax-loader.gif"  />
                                                        <span>Processing... </span>
                                                    </asp:Panel>
                                    </div>
                                      
                                      </ContentTemplate>
                                
                            </asp:UpdatePanel>
                                       </ContentTemplate>
                               
                            
                                                    </telerik:RadWindow> 

                                    <script type="text/javascript">
                                        $(window).resize(function () {
                                            var win = $find('<%= MPETarget_import.ClientID%>');
                                            if (win) {
                                                if (!win.isClosed()) {
                                                    win.center();
                                                }
                                            }

                                        });
                                    </script> 
                                     <telerik:RadWindow ID="MPETarget_import" Title= "Import Incentive Target  Details" runat="server" Skin="Windows7" Behaviors="Close"
                                                AutoSize="true" ReloadOnShow="false" VisibleStatusbar="false" Overlay="false" Modal="true">
                                              <ContentTemplate>
                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="conditional">
                                <ContentTemplate>

                               <asp:HiddenField ID="HiddenField1" runat="server" Value="-1" />
                                    <div class="popupcontentblk">
                                        <p><asp:Label ID="lblmsgPopUp"  runat ="server" ForeColor ="Red" ></asp:Label></p>
                                        <div class="row">

                                <%--               <div class="col-sm-5">
                                               <label> Organization</label>
                                            </div>
                                            <div class="col-sm-7">
                                                <div class="form-group">
                                                    <telerik:RadComboBox ID="ddl_orgimport" Width ="100%" runat="server"  TabIndex="4" Skin="Simple" Filter="Contains"  >
                                                      
                                                    </telerik:RadComboBox>
                                                                                                                                            
                                                </div>
                                            </div>
                                            
                                            <div class="col-sm-5">
                                                <label>Incentive Month</label>
                                            </div>
                                            <div class="col-sm-7">
                                                <div class="form-group">
                                                    <telerik:RadMonthYearPicker RenderMode="Lightweight" ID="RadMonthYearPicker1" runat="server" Width="238px">
                                                     </telerik:RadMonthYearPicker>

                                                                     

                                                </div>
                                            </div>--%>

                                             <div class="col-sm-2">
                                                <label>File</label>
                                            </div>
                                             <div class="col-sm-10">
                                                <div class="">
                                            <asp:UpdatePanel ID="UpdatePanelF" runat="server" UpdateMode="conditional">
                                                        <ContentTemplate>
                                                    <asp:FileUpload ID="file_import" runat="server"  /><asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                            ControlToValidate="file_import" Display="Dynamic"  ErrorMessage="*Upload only .xls and .xlsx Files" ValidationExpression="^(.)+(.xls|.XLS|.xlsx|.XLSX)$"></asp:RegularExpressionValidator>
                                                  <br />
                                                    <asp:Button ID="btn_importFile" CssClass ="btn btn-primary" TabIndex="5"  
                                                        runat="server" Text="Import" OnClick ="btn_importFile_Click" />
                                                    
                                                    <asp:Button ID="btn_CancelImport"  CssClass ="btn btn-danger" OnClick ="btn_CancelImport_Click"  TabIndex="6" runat="server" CausesValidation="false"
                                                        Text="Cancel" />
                                                            
                                                     </ContentTemplate>
                                                    </asp:UpdatePanel>  
                                                    </div>
                                                 </div>

                                           
                                        </div>
                                      
                                    </div>
                                      
                                      </ContentTemplate>
                                 <Triggers>
                                            <asp:PostBackTrigger ControlID="btn_importFile" />
                                            
                                            </Triggers>
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
