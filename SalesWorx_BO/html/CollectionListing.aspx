<%@ Page Title="Collection Listing" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="CollectionListing.aspx.vb" Inherits="SalesWorx_BO.CollectionListing" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="FeaturedContent" runat="server">
<style>
    input.rdfd_[type="text"] { height:0 !important; padding:0 !important; }
</style>
<script type="text/javascript">
        var TargetBaseControl = null;

        window.onload = function() {
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
          var TargetChildControl = "chkRelease";
             var TimeCon = document.getElementById('<%= Me.ClassUpdatePnl.ClientID %>');

          var Inputs = TargetBaseControl.getElementsByTagName("input");

           for (var n = 0; n < Inputs.length; ++n)
               if (Inputs[n].type == 'checkbox' && Inputs[n].id.indexOf(TargetChildControl, 0) >= 0 && Inputs[n].checked) {
                  return confirm('Would you like to release the selected Collection ?');
                  return true;
               }
        alert('Select at least one Collection !');
                return false;

          }

        function CheckAll(cbSelectAll) {
            if (TargetBaseControl == null) return false;
            var TargetChildControl = "chkRelease";
            var TimeCon = document.getElementById('<%= Me.ClassUpdatePnl.ClientID %>');

            var Inputs = TargetBaseControl.getElementsByTagName("input");

            for (var n = 0; n < Inputs.length; ++n)
                if (Inputs[n].type == 'checkbox' && Inputs[n].id.indexOf(TargetChildControl, 0) >= 0) {
                Inputs[n].checked = cbSelectAll.checked;
            }

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
            var win = $find('<%= MPECollection.ClientID %>');
            if (win) {
                if (!win.isClosed()) {
                    win.center();
                }
            }

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   <h4>Held PDC</h4>
 
                                         
                     <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>
          <div class="row">
              <div class="col-sm-2" runat="server" id="dvCountry">
                                            <div class="form-group">
                                                <label>Country</label>
                                                <telerik:RadComboBox Skin="Simple" EmptyMessage="Select Country" ID="ddlCountry" Width ="100%" runat="server" DataTextField="Country" DataValueField="MAS_ORG_ID"  AutoPostBack="true" >
                                            </telerik:RadComboBox>
                                               
                                            </div>
                                        </div>

          <div class="col-sm-3">
                             <div class="form-group">  <label>Organization </label>
            <telerik:RadComboBox Skin="Simple" ID="ddlOrganization"  
                    runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" Width="100%" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" >
                </telerik:RadComboBox>
             </div>
              </div>
               <div class="col-sm-3">
                             <div class="form-group">
                                 
                                 <label>Collection Ref. No</label>

                  <asp:TextBox ID="txtCollectionRefNo"  CssClass="inputSM" runat="server"  Width="100%"></asp:TextBox> 
                     </div>
              </div>
        
 
              <div class="col-sm-3">  
                  <div class="form-group">                  
                   <label>From Date(Cheque. Date) </label>
                                 
              <telerik:RadDatePicker ID="txtFromDate"  Width="100%"  runat="server" >
                                                <DateInput ReadOnly="true" DisplayDateFormat="dd/MM/yyyy">
                                                </DateInput>
                                                <Calendar ID="Calendar2" runat="server">
                                                    <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                </Calendar>
                                            </telerik:RadDatePicker>
                  </div>
                  

                  </div>
            
         
              <div class="col-sm-3">  
                  <div class="form-group">                
                                  <label> To Date(Cheque. Date)</label>
                                  
                <telerik:RadDatePicker ID="txtToDate"   Width="100%" runat="server">
                                                <DateInput ReadOnly="true" DisplayDateFormat="dd/MM/yyyy">
                                                </DateInput>
                                                <Calendar ID="Calendar1" runat="server">
                                                    <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                </Calendar>
                                            </telerik:RadDatePicker>
                      </div>
                  

                  </div>
         </div>
    <div class="row">   
         
              <div class="col-sm-3">  
                  <div class="form-group"> 
                <telerik:RadButton ID="SearchBtn" Skin ="Simple"    runat="server" Text="Search" CssClass ="btn btn-primary" />
         
                                  </div>
            </div>  
                
    </div>
 <div class="table-responsive">
        <asp:UpdatePanel ID="ClassUpdatePnl" runat="server">
        <ContentTemplate>
        
              <asp:GridView  width="100%" ID="GVCollectionList" runat="server" DataKeyNames="Collection_ID"
                  EmptyDataText="No Collections to Display" EmptyDataRowStyle-Font-Bold="true" 
                AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" 
                   PageSize="25" CellPadding="0" CellSpacing="0" CssClass="tablecellalign">
                  
                  <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
                    <EmptyDataRowStyle Font-Bold="True" />
                  <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:CheckBox ID="cbSelectAll" onclick="CheckAll(this)" ToolTip="Select/Unselect All" runat="server" />
                            <asp:ImageButton ToolTip="Release Selected Items " ID="btnReleaseAll" runat="server" CausesValidation="false"   ImageUrl="~/images/edit-13.png"   OnClientClick="return TestCheckBox()"
                                OnClick="btnReleaseAll_Click" />
                        </HeaderTemplate>
                        <HeaderStyle HorizontalAlign="Left" Width="60px" CssClass="display-block"  />
                        <ItemTemplate>
                            <asp:CheckBox ID="chkRelease" runat="server" />
                            <asp:ImageButton ToolTip="Release Collection" ID="btnRelease" OnClick="btnRelease_Click" runat="server" CausesValidation="false"   ImageUrl="~/images/edit-13.png"   OnClientClick="javascript:return confirm('Would you like to release the selected Collection?');" />                            
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" Width="60px" CssClass="display-block"  />
                    </asp:TemplateField>
                      <asp:TemplateField SortExpression="Collection_ID">                         
                          <ItemTemplate>
                              <asp:HiddenField ID="hdnCollection_ID" runat="server" Value='<%# Bind("Collection_ID") %>'/>                          
                          </ItemTemplate>                          
                      </asp:TemplateField>
                      <asp:TemplateField HeaderText="Collection Ref No" SortExpression="Collection_Ref_No">                         
                          <ItemTemplate>
                              <asp:LinkButton ID="lnkbtnCollection_Ref_No" runat="server" Text='<%# Bind("Collection_Ref_No") %>' OnClick="btnDetail_Click" Font-Bold="true"/>
                          </ItemTemplate>
                          <ItemStyle Wrap="False" />
                      </asp:TemplateField>
                    <asp:BoundField DataField="Collected_On" HeaderText="Collected On"  
                          SortExpression="Collected_On" NullDisplayText="N/A" 
                          DataFormatString="{0:dd/MM/yyyy}" >
                      <ItemStyle Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" 
                          DataField="Collected_By" HeaderText="Collected By"  
                          SortExpression="Collected_By">
                        <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                      </asp:BoundField>
                     <asp:BoundField DataField="Customer_Name" ItemStyle-HorizontalAlign="center" 
                          HeaderText="Customer Name" >
                        <ItemStyle HorizontalAlign="Center" />
                     </asp:BoundField>
                    <asp:BoundField DataField="Amount" ItemStyle-HorizontalAlign="center" 
                          HeaderText="Amount" >
                        <ItemStyle HorizontalAlign="Center" />
                     </asp:BoundField>
                     <asp:BoundField DataField="Cheque_No" ItemStyle-HorizontalAlign="center" HeaderText="Cheque No" >
                        <ItemStyle HorizontalAlign="Center" />
                     </asp:BoundField>                      
                     <asp:BoundField DataField="Cheque_Date" DataFormatString = "{0:dd/MM/yyyy}" 
                          HeaderText="Cheque Date" SortExpression="Cheque_Date" >
                         <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundField>
                      <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" 
                          DataField="Bank_Name" HeaderText="Bank_Name" SortExpression="Bank_Name" >
                          <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                      </asp:BoundField>
                      <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" 
                          DataField="Emp_Code" HeaderText="Emp Code">
                          <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                      </asp:BoundField>
                  </Columns>
                   <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
              </asp:GridView>
            <asp:HiddenField ID="hdnCollectionID" runat="server" value=""/>
          
       

                <telerik:RadWindow ID="MPECollection" Title= "Collection Details" runat="server" Skin="Windows7" Behaviors="Move,Close"
                                                AutoSize="true" ReloadOnShow="false" VisibleStatusbar="false" Overlay="true" Modal="true">
                                              <ContentTemplate>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="conditional">
                                <ContentTemplate>
                    <asp:HiddenField ID="HidVal" runat="server" Value="-1" />
                        <div class="popupcontentblk">
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="form-group">
                                        <div class="table-responsive">
                                        <asp:GridView  width="100%" ID="GVCollDtlList" runat="server" EmptyDataText="No details to Display" EmptyDataRowStyle-Font-Bold="true" 
                                              AutoGenerateColumns="False" AllowPaging="True" AllowSorting="true" 
                                              PageSize="25" CellPadding="0" CellSpacing="0" CssClass="tablecellalign">
                                 
                                              <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
                                              <Columns>
                                                <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="Collection_Line_Ref" SortExpression="Collection_Line_Ref" HeaderText="Collection Line Ref"/>                    
                                                 <asp:BoundField DataField="Invoice_No" HeaderText="Invoice No"  SortExpression="Invoice_No" NullDisplayText="N/A" >
                                                  <ItemStyle Wrap="False" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Amount" HeaderText="Amount"  SortExpression="Amount" NullDisplayText="N/A"  >
                                                  <ItemStyle Wrap="False" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="ERP_Status" HeaderText="ERP Status"  SortExpression="ERP_Status"/>                    
                                              </Columns>
                                              <PagerStyle CssClass="pagerstyle" />
                                                                <HeaderStyle />
                                                                <RowStyle CssClass="tdstyle" />
                                                                <AlternatingRowStyle CssClass="alttdstyle" />
                                          </asp:GridView>
                                          </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12 text-center">
                                    <div class="form-group">
                                        <asp:Button ID="btnReleaseDtl"  CssClass ="btn btn-primary"  runat="server" CausesValidation="false" Text="Release" OnClientClick="javascript:return confirm('Would you like to release this Collection?');" />
                                        <asp:Button ID="btnCancel" CssClass ="btn btn-default"  runat="server" CausesValidation="false" Text="Close" />
                                    </div>
                                </div>
                            </div>
                        </div>

                                    </ContentTemplate>
                                    </asp:UpdatePanel>
                                                  </ContentTemplate>
                </telerik:RadWindow>
            
       
          </ContentTemplate>
          <Triggers>
          <asp:AsyncPostBackTrigger ControlID="SearchBtn" EventName="Click" />
          </Triggers>
        </asp:UpdatePanel>                    
     </div>   
	<asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="ClassUpdatePnl" runat="server">
        <ProgressTemplate>
            <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                <img  src="../assets/img/ajax-loader.gif"  alt="Processing..." />
                <span>Processing... </span>
            </asp:Panel>
        </ProgressTemplate>
    </asp:UpdateProgress>
  
	 
</asp:Content>
