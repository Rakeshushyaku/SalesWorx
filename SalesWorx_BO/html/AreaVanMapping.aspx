<%@ Page Title="Area Van Mapping" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="AreaVanMapping.aspx.vb" Inherits="SalesWorx_BO.AreaVanMapping" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="FeaturedContent" runat="server">
    <style type="text/css">
        .style1
        {
            font-size: 11px;
            color: #000000;
            text-decoration: none;
            font-weight: bold;
            width: 120px;
        }
    </style>
    <script type="text/javascript" language="javascript">
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
                  return confirm('Would you like to delete the selected mapping?');
                  return true;
              }
          alert('Select at least one row!');
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
                var AddString = postBackElement.id.search(myRegExp);
                var EditString = postBackElement.id.search(myRegExp1);
                var myRegExp2 = /btnCancel/
                var cancelString = postBackElement.id.search(myRegExp2);
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
               // $('a[class=rwCloseButton')[0].click();
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
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h4>Van/FSR Territory Mapping</h4>
	
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
                                    <label>Filter By</label>
                                    <div class="row">
	                                    <div class="col-sm-4">
		                                    <div class="form-group">
                                                <telerik:RadComboBox  Skin="Simple"   ID="ddFilterByVan" Width="100%" Height="250px" TabIndex="2" runat="server" Filter="Contains"></telerik:RadComboBox>
		                                    </div>
	                                    </div>
                                        <div class="col-sm-4">
		                                    <div class="form-group">
                                                <telerik:RadComboBox ID="ddFilterByDistrict" Skin="Simple"   Width="100%" Height="250px" TabIndex="2" runat="server" Filter="Contains"></telerik:RadComboBox>
		                                    </div>
	                                    </div>
                                        <div class="col-sm-4">
		                                    <div class="form-group">
                                                <telerik:RadComboBox ID="ddFilterBySegment" Skin="Simple"  Width="100%" Height="250px" TabIndex="2"  runat="server" Filter="Contains"></telerik:RadComboBox>  
		                                    </div>
	                                    </div>
                                    </div>
                                    <div class="row">
	                                    <div class="col-sm-4">
		                                    <div class="form-group">
                                             <telerik:RadButton ID="btnFilter" Skin ="Simple"    runat="server" Text="Search" CssClass ="btn btn-primary" />
                                             <telerik:RadButton ID="btnAdd" Skin="Simple" runat="server" Text="Add" CssClass="btn btn-success" />
                                            </div>
                                        </div>
                                    </div>
                                     
                                </ContentTemplate>
                            </asp:UpdatePanel>
       <div class="table-responsive">
        <asp:UpdatePanel ID="ClassUpdatePnl" runat="server">
        <ContentTemplate>
        
              <asp:GridView  width="100%" ID="grdAreaVanList" runat="server" 
                  EmptyDataText="No listing found" EmptyDataRowStyle-Font-Bold="true" 
                  AutoGenerateColumns="False" AllowPaging="True"
                   AllowSorting="True" 
                  PageSize="25" CellPadding="0" CellSpacing="0" CssClass="tablecellalign">
                   
                  <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
                    <EmptyDataRowStyle Font-Bold="True" />
                  <Columns>
                   <asp:TemplateField>
                                                         
                                                            <HeaderStyle HorizontalAlign="Left" Width="100px" CssClass="display-block"  />
                       <HeaderTemplate>
                                                                <asp:CheckBox ID="cbSelectAll" onclick="CheckAll(this)" ToolTip="Select/Unselect All"
                                                                    runat="server" />
                                                                <asp:ImageButton ToolTip="Delete Selected Items " ID="btnDeleteAll" runat="server"
                                                                    CausesValidation="false" ImageUrl="~/images/delete-13.png"
                                                                 OnClientClick="return ConfirmDelete('Would you like to delete the selected Mapping ?',event);"
                                                                    OnClick="btnDeleteAll_Click" CssClass="checkboximgvalign" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkDelete" runat="server"  CssClass="checkboxvalign" />
                                                                <asp:ImageButton ToolTip="Delete Currency Code" ID="ImageButton1" OnClick="btnDelete_Click"
                                                                    runat="server" CausesValidation="false" ImageUrl="~/images/delete-13.png" OnClientClick="return ConfirmDelete('Would you like to delete the selected mapping?',event);" CssClass="checkboximgvalign" />
                                                                
                                                                 <asp:ImageButton ID="btnEdit" ToolTip="Edit Mapping" runat="server" CausesValidation="false"
                                                                      ImageUrl="~/images/edit-13.png"   OnClick="btnEdit_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"AF_Map_ID") %>' />
                                                            </ItemTemplate>
                       <ItemStyle HorizontalAlign="Left" Width="100px" CssClass="display-block"  />
                                                        </asp:TemplateField>
                   <asp:BoundField DataField="SalesRep_Name" HeaderText="Van/FSR"  SortExpression="SalesRep_Name" NullDisplayText="N/A" >
                      <ItemStyle Wrap="False" />
                   </asp:BoundField>
                   <asp:BoundField ItemStyle-HorizontalAlign="center" DataField="CustomerSegment" SortExpression="CustomerSegment" HeaderText="Customer Segment">
                       <ItemStyle HorizontalAlign="Center" />
                   </asp:BoundField> 
                   <asp:BoundField DataField="SalesDistrict" HeaderText="Sales District"  SortExpression="SalesDistrict" NullDisplayText="N/A" >
                      <ItemStyle Wrap="False" />
                   </asp:BoundField>
                      <asp:TemplateField>
                          <ItemTemplate>
                              
                              <asp:HiddenField ID="hfMapID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"AF_Map_ID") %>' />
                               <asp:HiddenField ID="hfSalesrep_ID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"Salesrep_ID") %>' />
                          </ItemTemplate>
                      </asp:TemplateField>
                  </Columns>
                 <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
              </asp:GridView>
        
          
          <telerik:RadWindow ID="MPEDetails" Title= "Van Territory Mapping" runat="server" Skin="Windows7" Behaviors="Move,Close"
                                                AutoSize="true" ReloadOnShow="false" VisibleStatusbar="false" Overlay="true" Modal="true">
                                              <ContentTemplate>
                                                   <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
                                <ContentTemplate>
                                    <div class="popupcontentblk">
                                        <p><asp:Label ID="lblmsg" runat="server" ForeColor="Red"></asp:Label><asp:HiddenField ID="hfID" runat="server" Value="0" /></p>
	                                    <div class="row">
		                                    <div class="col-sm-5">
			                                    <label>Van</label>
		                                    </div>
		                                    <div class="col-sm-7">
			                                    <div class="form-group">
                                                    <telerik:RadComboBox  Skin="Simple" ID="drpVan"  Width ="100%"  runat="server" DataTextField="Description" DataValueField="ORG_HE_ID" Filter="Contains"></telerik:RadComboBox>
			                                    </div>
		                                    </div>
	                                    </div>
                                        <div class="row">
		                                    <div class="col-sm-5">
			                                    <label>Customer Segment</label>
		                                    </div>
		                                    <div class="col-sm-7">
			                                    <div class="form-group">
                                                    <telerik:RadComboBox  Skin="Simple"  Width ="100%" ID="drpCustomerSegment" runat="server" Filter="Contains"></telerik:RadComboBox>
			                                    </div>
		                                    </div>
	                                    </div>
                                        <div class="row">
		                                    <div class="col-sm-5">
			                                    <label>Sales District</label>
		                                    </div>
		                                    <div class="col-sm-7">
			                                    <div class="form-group">
                                                    <telerik:RadComboBox  Skin="Simple"  Width ="100%" ID="drpSalesDistrict" runat="server"  Filter="Contains" ></telerik:RadComboBox>
			                                    </div>
		                                    </div>
	                                    </div>
                                        <div class="row">
		                                    <div class="col-sm-5">
			                                    <label></label>
		                                    </div>
		                                    <div class="col-sm-7">
			                                    <div class="form-group">
                                                    <telerik:RadButton ID="btnSave" Skin="Simple" CssClass ="btn btn-success" 
                                                           OnClick="btnSave_Click"  ValidationGroup ="valsum"
                                                            runat="server" Text="Save" TabIndex ="6" > </telerik:RadButton>

                                                     <telerik:RadButton ID="btnUpdate" Skin="Simple" CssClass ="btn btn-success" 
                                                           OnClick="btnUpdate_Click"  ValidationGroup ="valsum"
                                                            runat="server" Text="Update" TabIndex ="6" > </telerik:RadButton>

                                                     <telerik:RadButton ID="btnCancel" Skin="Simple" Visible ="true"  CssClass ="btn btn-default" 
                                                         OnClientClick="return DisableValidation()"
                                                            runat="server" Text="Cancel" TabIndex ="6" > </telerik:RadButton>
			                                    </div>
		                                    </div>
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
                            <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                            <span style="font-size: 12px; color: #3399ff;">Processing... </span>
                        </asp:Panel>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                
</asp:Content>
