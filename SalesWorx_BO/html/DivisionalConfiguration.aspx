<%@ Page Title="Sales Org Configuration" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master"
    CodeBehind="DivisionalConfiguration.aspx.vb" Inherits="SalesWorx_BO.DivisionalConfiguration" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="FeaturedContent" runat="server">
   <script language="javascript" type="text/javascript">


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
                    return confirm('Would you like to delete the selected reason code?');
                    return true;
                }
            alert('Select at least one Reason Code!');
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




        function DisableValidation() {
            //            Page_ValidationActive = false;
            //            return true;

        }
        </script>
<script language="javascript" type="text/javascript">


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
            var myRegExp1 = /btnSaveAcc/
            var AddString = postBackElement.id.search(myRegExp);
            var EditString = postBackElement.id.search(myRegExp1);
            if (AddString != -1 || EditString != -1) {

            }
            else {
                $get('<%= Me.UpdateProgress1.ClientID %>').style.display = 'block';
            }
            postBackElement.disabled = true;
        }
    }


    function EndRequest(sender, args) {
        var Filter = /ddFilterBy/
        var AddString = postBackElement.id.search(Filter);
        if (AddString == -1) {
            var myRegExp = /_btnUpdate/;
            var myRegExp1 = /btnSaveAcc/
            var AddString = postBackElement.id.search(myRegExp);
            var EditString = postBackElement.id.search(myRegExp1);

            if (AddString != -1 || EditString != -1) {

            }
            else {
                $get('<%= Me.UpdateProgress1.ClientID %>').style.display = 'none';
                }
                postBackElement.disabled = false;
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
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    
    <h4>Sales Org Configuration</h4>
                <telerik:RadAjaxLoadingPanel  ID="AjaxLoadingPanel1" runat="server" >
             <div ID="ProgressPanel" class ="overlay"  >
     
         <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index:10010;vertical-align:middle;" />      
       </div>
           </telerik:RadAjaxLoadingPanel>  

        <asp:Panel ID="Panel2" runat="server">      </asp:Panel> 
                         
 
    
                                         
                     <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>
                            <asp:UpdatePanel ID="TopPanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <label>Organization</label>
                                    <div class="row">
                                            <div class="col-sm-4">
                                         <div class="form-group"> 
                                                <telerik:RadComboBox ID="ddFilterBy" Skin="Simple"  
                    Width="100%" Height="250px" TabIndex="2"
                    runat="server">
                                                </telerik:RadComboBox>
                                          </div>
                                                </div>
                                        <div class="col-sm-4">
                                         <div class="form-group"> 

                                                         <telerik:RadButton ID="btnFilter" Skin ="Simple"    runat="server" Text="Search" CssClass ="btn btn-primary" />
                                                 <telerik:RadButton ID="btnAdd" Skin="Simple" OnClick ="btnAdd_Click" runat="server" Text="Add" CssClass="btn btn-success" />
                                          </div> 
                                            </div> 
                                        </div> 
                                </ContentTemplate>
                            </asp:UpdatePanel>
                   <div class="table-responsive">
                            <asp:UpdatePanel ID="ClassUpdatePnl" runat="server" UpdateMode="conditional">
                                <ContentTemplate>
                                    
                                                  <asp:GridView Width="100%" ID="gvDivConfig" runat="server" EmptyDataText="No Configuration details found."
                                                    EmptyDataRowStyle-Font-Bold="true"  AutoGenerateColumns="False"
                                                    AllowPaging="True" AllowSorting="true"   PageSize="25" CellPadding="0" CellSpacing="0" CssClass="tablecellalign">
                                                    <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
                                                    <EmptyDataRowStyle Font-Bold="True" />
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <HeaderStyle width="75" />
                                                                <asp:CheckBox ID="cbSelectAll" onclick="CheckAll(this)" ToolTip="Select/Unselect All"
                                                                    runat="server" />
                                                                <asp:ImageButton ToolTip="Delete Selected Items " ID="btnDeleteAll" runat="server"
                                                                    OnClick="btnDeleteAll_Click" CausesValidation="false" ImageUrl="~/images/delete-13.png"
                                                                    OnClientClick="return TestCheckBox()" />
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" Width="100px" CssClass="display-block"  />
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkDelete" runat="server" />
                                                                
                                                                <asp:ImageButton ToolTip="Delete Configuration " ID="btnDelete" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Org_ID") %>'
                                                                    OnClick="btnDelete_Click" runat="server" CausesValidation="false" 
                                                                     ImageUrl="~/images/delete-13.png" 
                                                                    OnClientClick="javascript:return confirm('Would you like to delete the selected configuration?');"/>

                                                                <asp:ImageButton ID="btnEdit" ToolTip="Edit Configuration" runat="server" CausesValidation="false"
                                                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Org_ID") %>'
                                                                      ImageUrl="~/images/edit-13.png"   OnClick="btnEdit_Click" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" CssClass="display-block"  />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Description" HeaderText="Organization Name" SortExpression="Description">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Allow_Manual_FOC" HeaderText="Allow Manual FOC" SortExpression="Allow_Manual_FOC">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Odo_Reading_At_Visit" HeaderText="Odometer Reading At Visit"
                                                            SortExpression="Odo_Reading_At_Visit">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Advance_PDC_Posting" HeaderText="Advance PDC Posting "
                                                            SortExpression="Advance_PDC_Posting">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Collection_OutputFolder" HeaderText="Collection Output Folder"
                                                            SortExpression="Collection_Output_Folder">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        
                                                         <asp:BoundField DataField="CN_LIMIT" HeaderText="Credit Note Limit"
                                                            SortExpression="CN_LIMIT">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                         <asp:BoundField DataField="CustomerNoSeq" HeaderText="Customer No. Sequence"
                                                            SortExpression="CustomerNoSeq">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                       <%-- <asp:BoundField DataField="Print_Format" HeaderText="Print Format"
                                                            SortExpression="Print_Format">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>--%>
                                                        <asp:BoundField DataField="VAT_REGISTRATION" HeaderText="TRN"
                                                            SortExpression="VAT_REGISTRATION">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>

                                                         <asp:BoundField DataField="DELIVERY_CUT_OFF_TIME" HeaderText="Delivery Cut Off Time"
                                                            SortExpression="DELIVERY_CUT_OFF_TIME">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                    <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
                                                </asp:GridView> 
                                            
                                    <telerik:RadWindow ID="MPEDetails" Title= "Sales Org Details" runat="server" Skin="Windows7" Behaviors="Move,Close"
                                                AutoSize="true" ReloadOnShow="false" VisibleStatusbar="false" Overlay="true" Modal="true">
                                              <ContentTemplate>
                                                   

                                        <asp:HiddenField ID="HidVal" runat="server" Value="-1" />
                                        
                                                  <div class="popupcontentblk">
                                                      <p><asp:Label ID="lblPop" runat="server" Text="" ForeColor="Red"></asp:Label></p>
                                                    
                                                      
                                                       <div class="row">

                                                         <div class="col-sm-6">
                                                            <label>Organization</label>
                                                            <div class="form-group">
                                                              <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="conditional">
                                                            <ContentTemplate>
                                                            <telerik:RadComboBox ID="drpOrganization" Skin="Simple" AutoPostBack="True" Width="100%" runat="server"></telerik:RadComboBox>    
                                                            </ContentTemplate>
                                                            </asp:UpdatePanel> 
                                                            </div>

                                                        </div>

                                                        <div class="col-sm-6">
                                                            <div class="form-group">
                                                               



                                                                <label>
                                                                   Advance PDC Posting      
                                                                </label>
                                                                  <asp:TextBox ID="txtPDCPosting" Width="100%" TabIndex ="4" CssClass="inputSM" runat="server"></asp:TextBox>
                                                            <asp:Label ID="Label1" runat="server" Text="(days)"></asp:Label>
                                                            <ajaxToolkit:FilteredTextBoxExtender
                                                                ID="FT" runat="server" TargetControlID="txtPDCPosting" FilterType="Numbers">
                                                            </ajaxToolkit:FilteredTextBoxExtender>
                                                            </div>
                                                        </div>

                                                                                         

                                                    </div>
                                                     <%-- <div class="row">
                                                        <div class="col-sm-5">
                                                            <label>Organization</label>
                                                        </div>
                                                        <div class="col-sm-7">
                                                            <div class="form-group">
                                                           
                                                            </div>
                                                        </div>
                                                    </div>--%>




                                                    <%--<div class="row">
                                                        <div class="col-sm-5">
                                                            <label>Allow Manual FOC</label>
                                                        </div>
                                                        <div class="col-sm-7">
                                                            <div class="form-group">
                                                         
                                                            </div>
                                                        </div>
                                                    </div>--%>
                                                       
                                                       <div class="row">

                                                         <div class="col-sm-6">
                                                            <label>Odometer Reading At Visit</label>
                                                            <div class="form-group">
                                                                <asp:CheckBox ID="chkOdo" TabIndex ="3" runat="server" CssClass="inputSM" TextAlign="Left" />
                                                            </div>

                                                        </div>

                                                        <div class="col-sm-6">
                                                            <div class="form-group">
                                                                 <label>
                                                                   Allow Manual FOC       
                                                                </label>
                                                                  <asp:CheckBox ID="chkFOC" TabIndex ="2" runat="server" CssClass="inputSM" TextAlign="Left"  />
                                                            </div>
                                                        </div>

                                                                                         

                                                    </div>



                                                <%--    <div class="row">
                                                        <div class="col-sm-5">
                                                            <label>Odometer Reading At Visit</label>
                                                        </div>
                                                        <div class="col-sm-7">
                                                            <div class="form-group">
                                                         
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-sm-5">
                                                            <label>Advance PDC Posting</label>
                                                        </div>
                                                        <div class="col-sm-7">
                                                            <div class="form-group">
                                                           
                                                            </div>
                                                        </div>
                                                    </div>--%>


                                                        <div class="row">

                                                         <div class="col-sm-6">
                                                            <label>Collection Output Folder</label>
                                                            <div class="form-group">
                                                                 <asp:TextBox ID="txtCollectionOutputFolder" TabIndex ="5" Width="100%" CssClass="inputSM" runat="server"></asp:TextBox>
                                                            </div>

                                                        </div>

                                                        <div class="col-sm-6">
                                                            <div class="form-group">
                                                                <label>
                                                                  Credit Note Limit   
                                                                </label>
                                                                  <asp:TextBox ID="txtCRLimit" Width="100%" TabIndex ="6" CssClass="inputSM" runat="server"></asp:TextBox>
                                                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtCRLimit" FilterType="Numbers">
                                                            </ajaxToolkit:FilteredTextBoxExtender>
                                                            </div>
                                                        </div>

                                                                                         

                                                    </div>



                                                      <div class="row">

                                                         <div class="col-sm-6">
                                                            <label>TRN</label>
                                                            <div class="form-group">
                                                                 <asp:TextBox ID="txtTRN" TabIndex ="5" Width="100%" CssClass="inputSM" MaxLength="40"  runat="server"></asp:TextBox>
                                                            </div>

                                                        </div>

                                                        <div class="col-sm-6">
                                                            <div class="form-group">
                                                                <label>
                                                                  Delivery Cut Off Time 
                                                                </label>
                                                                    
                                                                 <telerik:RadTimePicker ID="DatePickerDeliveryCutoffTime" runat="server" EnableTyping="true" DateInput-EmptyMessage="Time" TimeView-Culture="en-gb" TimeView-Interval="00:30:00" DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm"></telerik:RadTimePicker>
                                                            </div>
                                                        </div>

                                                                                         

                                                    </div>



                                                    <%--<div class="row">
                                                        <div class="col-sm-5">
                                                            <label>Collection Output Folder</label>
                                                        </div>
                                                        <div class="col-sm-7">
                                                            <div class="form-group">
                                                          
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-sm-5">
                                                            <label>Credit Note Limit</label>
                                                        </div>
                                                        <div class="col-sm-7">
                                                            <div class="form-group">
                                                            
                                                            </div>
                                                        </div>
                                                    </div>--%>



                                                       <div class="row">

                                                         <div class="col-sm-6">
                                                            <label>Customer No. Sequence</label>
                                                            <div class="form-group">
                                                               <asp:TextBox ID="txtCustNoSeq" Width="100%" TabIndex ="6" CssClass="inputSM" runat="server"></asp:TextBox>
                                                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtCustNoSeq" FilterType="Numbers">
                                                            </ajaxToolkit:FilteredTextBoxExtender>
                                                            </div>

                                                        </div>

                                                        <div class="col-sm-6">
                                                            <div class="form-group">
                                                                <label>
                                                                 Is DC Optional?
                                                                </label>
                                                                  <small><asp:Label ID="Label3" runat="server" Font-Bold ="false"  Text="(select to make distribution check optional for a van)"></asp:Label></small></label>

                                                                   <asp:UpdatePanel ID="upVan" runat="server" UpdateMode="conditional">
                                                                <ContentTemplate>
                                                                    <asp:Panel ID="Panel1" runat="server" Height="143px" ScrollBars="Auto" BorderStyle="solid" BorderColor="#cccccc" BorderWidth="1px" Width="100%" >
                                                                        <telerik:RadListBox ID="chkSalesRep"  CssClass="multiColumn"  Visible="true"   TabIndex="4"  runat="server" CheckBoxes="true"  Skin ="Default" BackColor ="White"  BorderStyle ="None" BorderColor ="LightGray" BorderWidth ="1px"
                                                                            DataTextField="UserName"  ForeColor ="Black" DataValueField="User_ID" style="top: 0px; left: 0px" Width="100%" >
                                                                        </telerik:RadListBox>
                                                                    </asp:Panel>
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="drpOrganization" EventName="SelectedIndexChanged" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                            </div>
                                                        </div>

                                                                                         

                                                    </div>




<%--                                                    <div class="row">
                                                        <div class="col-sm-5">
                                                            <label>Customer No. Sequence</label>
                                                        </div>
                                                        <div class="col-sm-7">
                                                            <div class="form-group">
                                                            
                                                            </div>
                                                        </div>
                                                    </div>--%>
                                                    <%--<div class="row">
                                                         <div class="col-sm-5">
                                                            <label>Print Format</label>
                                                        </div>
                                                        <div class="col-sm-7">
                                                             <div class="form-group">
                                                            <asp:RadioButton ID="rbStandard" runat="server"  Text ="Standard" GroupName ="rb" CssClass="inputSM" />
                                                            <asp:RadioButton ID="rbContinue" runat="server" Text ="Continuous" GroupName ="rb" CssClass="inputSM" />
                                                             </div>
                                                        </div>
                                                    </div>--%>
                                                    <%--<div class="row">
                                                        <div class="col-sm-5">
                                                            <label>Is DC Optional?<br />
                                                            <small><asp:Label ID="Label2" runat="server" Font-Bold ="false"  Text="(select to make distribution check optional for a van)"></asp:Label></small></label>
                                                        </div>
                                                        <div class="col-sm-7">
                                                            <div class="form-group">
                                                            
                                                            </div>
                                                        </div>
                                                    </div>--%>


                                                    <div class="row">
                                                        <div class="col-sm-5">
                                                            <label></label>
                                                        </div>
                                                        <div class="col-sm-7">
                                                            <asp:Button ID="btnSave" CssClass ="btn btn-success" TabIndex="7" OnClick="btnSave_Click" runat="server" Text="Save" />
                                                            <asp:Button ID="btnUpdate" CssClass ="btn btn-success" Text="Update" OnClick="btnUpdate_Click" runat="server" />
                                                            <asp:Button ID="btnCancel" CssClass ="btn btn-default" TabIndex="8" runat="server"  Text="Cancel" />
                                                        </div>
                                                        
                                                    </div>
                                                      <asp:Panel ID="Panel12" runat="server" Style="display: none" CssClass="overlay">
                                                            <img alt="Processing..." src="../assets/img/ajax-loader.gif" />
                                                            <span>Processing... </span>
                                                        </asp:Panel>
                                       
                                    </div>
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
