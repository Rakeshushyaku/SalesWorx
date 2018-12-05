<%@ Page Title="Manage Sync Cut Off Time" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="ManageSyncCutOffDate.aspx.vb" Inherits="SalesWorx_BO.ManageSyncCutOffDate" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
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
                    return confirm('Would you like to delete the selected holiday?');
                    return true;
                }
            alert('Select at least one holiday!');
            return false;

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



      var prm = Sys.WebForms.PageRequestManager.getInstance();

      prm.add_initializeRequest(InitializeRequest);
      prm.add_endRequest(EndRequest);
      var postBackElement;
      function InitializeRequest(sender, args) {

          if (prm.get_isInAsyncPostBack())
              args.set_cancel(true);
          postBackElement = args.get_postBackElement();

          var Filter = /ddFilterBy/
          var AddString = postBackElement.id.search(Filter);
          if (AddString == -1) {
              var myRegExp = /_btnUpdate/;
              var myRegExp1 = /btnSave/
              var AddString = postBackElement.id.search(myRegExp);
              var EditString = postBackElement.id.search(myRegExp1);
              if (AddString != -1 || EditString != -1) {

              }
              else {
                  $get('<%= Me.UpdateProgress1.ClientID%>').style.display = 'block';
                }
                postBackElement.disabled = true;
            }
        }


        function EndRequest(sender, args) {
            var Filter = /ddFilterBy/
            var AddString = postBackElement.id.search(Filter);
            if (AddString == -1) {
                var myRegExp = /_btnUpdate/;
                var myRegExp1 = /btnSave/
                var myRegExp2 = /btnCancel/
                var cancelString = postBackElement.id.search(myRegExp2);
                var AddString = postBackElement.id.search(myRegExp);
                var EditString = postBackElement.id.search(myRegExp1);

                if (AddString != -1 || EditString != -1) {

                }
                else {
                    $get('<%= Me.UpdateProgress1.ClientID %>').style.display = 'none';
                }
                postBackElement.disabled = false;
                if (cancelString != -1) {
                    HideRadWindow();
                }
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
    <script type="text/javascript">
        $(window).resize(function () {
            var win = $find('<%= MPEDetails.ClientID %>');
            if (win) {
                if (!win.isClosed()) {
                    win.center();
                }
            }

        });
    </script>
   </asp:Content>
     <asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

          <h4>Sync Cut Off Time Management</h4>

         <telerik:RadAjaxLoadingPanel  ID="AjaxLoadingPanel1" runat="server" >
             <div ID="ProgressPanel" class ="overlay"  >
     
         <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index:10010;vertical-align:middle;" />      
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
                                                <label>Country</label>

                       <telerik:RadComboBox Skin="Simple" ID="ddlFilterCountry" 
                                Width ="100%" runat="server"  CssClass="inputSM" AutoPostBack="true">

                                                                                                       </telerik:RadComboBox>   </div>
                                             </div>
                                         <div class="col-sm-3">
                                            <div class="form-group">
                                                <label>Year</label>

                       <telerik:RadComboBox Skin="Simple" ID="ddlFilterYear" AutoPostBack="true" 
                                Width ="100%" runat="server"  CssClass="inputSM">

                                                                                                       </telerik:RadComboBox>   </div>
                                             </div>

                      
                      <div class="col-sm-3">
                                            <div class="form-group">
                                                <label>Month </label> <telerik:RadComboBox Skin="Simple" ID="ddlFilterMonth" AutoPostBack="true"  Width ="100%" runat="server"  CssClass="inputSM">

                                    </telerik:RadComboBox> </div>
                          </div>

                     
                    <div class="col-sm-3">

                                            <div class="form-group">
                                                 <label>&nbsp;</label>
                         <telerik:RadButton ID="btnAdd" Skin="Simple" OnClick ="btnAdd_Click" runat="server" Text="Add" CssClass="btn btn-success" /> 
                       

                    </div>
                        </div>

                

                  </div>
                                            
                               

                                </ContentTemplate>
                                
                            </asp:UpdatePanel>
                  <div class="table-responsive">
                            <asp:UpdatePanel ID="ClassUpdatePnl" runat="server" UpdateMode="conditional">
                                <ContentTemplate>
                                 
                                                <asp:GridView Width="100%" ID="gvCutoff" runat="server" EmptyDataText="No Data to Display"
                                                    EmptyDataRowStyle-Font-Bold="true"  AutoGenerateColumns="False"
                                                    AllowPaging="True" AllowSorting="True"  PageSize="25" CellPadding="0" CssClass="tablecellalign">
                                                 
                                                    <EmptyDataRowStyle Font-Bold="True" />
                                                 
                                                    <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            
                                                            <HeaderStyle HorizontalAlign="Left" Width="100px" CssClass="display-block"  />
                                                            <ItemTemplate>
                                                               
                                                                <asp:ImageButton ToolTip="Delete Cut Off Date" ID="btnDelete" OnClick="btnDelete_Click"  Visible ='<%# Bind("deletable")%>'   
                                                                    runat="server" CausesValidation="false" ImageUrl="~/images/delete-13.png" OnClientClick="return ConfirmDelete('Would you like to delete the selected Cut Off Date?',event);" CssClass="checkboximgvalign" />
                                                                <asp:ImageButton ID="btnEdit" ToolTip="Edit Cut Off Date" runat="server" CausesValidation="false" Visible ='<%# Bind("editable")%>' 
                                                                    ImageUrl="~/images/edit-13.png" OnClick="btnEdit_Click" CssClass="checkboximgvalign" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" CssClass="display-block"  />
                                                        </asp:TemplateField>
                                                         <asp:BoundField DataField="Country" HeaderText="Country" SortExpression="Country">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Period_Year" HeaderText="Year" SortExpression="Period_Year">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Period_Month" HeaderText="Month" SortExpression="Period_Month">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Sync_CutOff_Time" HeaderText="Cut Off Date" SortExpression="Sync_CutOff_Time" DataFormatString="{0:dd-MMM-yyyy hh:mm:ss tt}"  >
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        
                                                        
                                                    </Columns>
                                                   <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
                                                </asp:GridView>
                                            
                                       <telerik:RadWindow ID="MPEDetails" Title= "Sync Cut Off Date" runat="server" Skin="Windows7" Behaviors="Move,Close"
                                                AutoSize="true" ReloadOnShow="false" VisibleStatusbar="false" Overlay="true" Modal="true">
                                              <ContentTemplate>

                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="conditional">
                                <ContentTemplate>

                               <asp:HiddenField ID="HidVal" runat="server" Value="-1" />
                                    <div class="popupcontentblk">
                                                    <p><asp:Label ID="lblPop" runat ="server" ForeColor ="Red" ></asp:Label></p>
                                        <div class="row">
                                            <div class="col-sm-5">
                                                <label>Country</label>
                                            </div>
                                            <div class="col-sm-7">
                                                <div class="form-group">
                                                    <telerik:RadComboBox Skin="Simple" ID="ddlCountry" Width="100%"  runat="server"  CssClass="inputSM"></telerik:RadComboBox> 
                                                </div>
                                            </div>

                                            <div class="col-sm-5">
                                                <label>Year</label>
                                            </div>
                                            <div class="col-sm-7">
                                                <div class="form-group">
                                                    <telerik:RadComboBox Skin="Simple" ID="ddlYear" Width="100%" AutoPostBack="true" runat="server"  CssClass="inputSM"></telerik:RadComboBox> 
                                                </div>
                                            </div>
                                            <div class="col-sm-5">
                                                <label>Month</label>
                                            </div>
                                            <div class="col-sm-7">
                                                <div class="form-group">
                                                    <telerik:RadComboBox Skin="Simple" ID="ddlMonth" Width="100%" runat="server"  CssClass="inputSM"></telerik:RadComboBox>
                                                </div>
                                            </div>
                                            <div class="col-sm-5">
                                                <label>Date</label>
                                            </div>
                                            <div class="col-sm-7">
                                                <div class="form-group">
                                                    <telerik:RadDateTimePicker  ID="DatePickerCutoffDate" Width="100%" runat="server"></telerik:RadDateTimePicker >
                                                </div>
                                            </div>
                                            <div class="col-sm-5">
                                                <label></label>
                                            </div>
                                            <div class="col-sm-7">
                                                <div class="form-group">
                                                    <telerik:RadButton ID="btnSave" Skin="Simple" CssClass ="btn btn-success" 
                                                           OnClick="btnSave_Click"   
                                                            runat="server" Text="Save" TabIndex ="6" >
                                                      </telerik:RadButton>

                                                                     <telerik:RadButton ID="btnUpdate" Skin="Simple" CssClass ="btn btn-success" 
                                                                           OnClick="btnUpdate_Click"   
                                                                            runat="server" Text="Update" TabIndex ="6" >
                                                      </telerik:RadButton>

                                                                     <telerik:RadButton ID="btnCancel" Skin="Simple"  CssClass ="btn btn-default" 
                                                                         OnClick="btnCancel_Click" 
                                                                            runat="server" Text="Cancel" TabIndex ="7" >
                                                      </telerik:RadButton>
                                                </div>
                                            </div>
                                            <asp:Panel ID="Panel12" runat="server" Style="display: none" CssClass="overlay">
                                                        <img alt="Processing..." src="../assets/img/ajax-loader.gif" />
                                                        <span>Processing... </span>
                                                    </asp:Panel>
                                        </div>
                                        
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
