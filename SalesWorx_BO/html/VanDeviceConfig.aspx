<%@ Page Title="Van Device Configuartion" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master"
    CodeBehind="VanDeviceConfig.aspx.vb" Inherits="SalesWorx_BO.VanDeviceConfig" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
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

        }
    </script>
 </asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
<h4>Van/FSR Device Configuration</h4>
                        <div id="pagenote">
                            <p>This screen may be used to configure the van/FSR device</p></div>

    <telerik:RadAjaxLoadingPanel ID="AjaxLoadingPanel1" runat="server">
        <div id="ProgressPanel" class="overlay">

            <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
        </div>
    </telerik:RadAjaxLoadingPanel>

    <asp:Panel ID="Panel1" runat="server"></asp:Panel>

    <telerik:RadWindowManager ID="RadWindowManager1" Skin="Simple" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>

                            <asp:UpdatePanel runat="server" ID="TopPanel" UpdateMode ="Conditional" >
        <ContentTemplate>

            <div class="row">
	            <div class="col-sm-4">
		            <div class="form-group">
                        <label>Organization</label>
                        <telerik:RadComboBox ID="ddOraganisation"  Width="100%" Skin="Simple"  AutoPostBack="true" runat="server" ></telerik:RadComboBox>
		            </div>
	            </div>
                <div class="col-sm-4">
		            <div class="form-group">
                        <label>Van/FSR</label>
                         <telerik:RadComboBox Skin="Simple" CheckBoxes="true" EmptyMessage="Select Van/FSR" EnableCheckAllItemsCheckBox="true" ID="ddlSalesRep" Width ="100%" runat="server" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID" Filter="Contains">
                                                            </telerik:RadComboBox >
		            </div>
	            </div>
                <div class="col-sm-4">
		            <div class="form-group">
                        <label><br /></label>
                        <asp:Button  ID="BtnSearch" runat="server" Text="Search" CssClass="btn btn-primary" />
                        <asp:Button  ID="btnUpdate" runat="server" Text="Update" CssClass="btn btn-success" />
                        <asp:Button  ID="btnReset" runat="server" Text="Reset" CssClass="btn btn-default" />
		            </div>
	            </div>
            </div>

                       <div class="table-responsive">
                           
                                             <asp:GridView Width="100%" ID="gvConfig" runat="server" EmptyDataText="No records to display"
                                                    EmptyDataRowStyle-Font-Bold="true"  AutoGenerateColumns="False"
                                                    AllowPaging="false" AllowSorting="false"  PageSize="25" CellPadding="0" CellSpacing="0" CssClass="tablecellalign">
                                                   
                                                    <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
                                                    <Columns>
                                                       <asp:BoundField DataField="Row_ID" Visible ="false"  HeaderText="Row ID" SortExpression="Row_ID">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="SalesRep_name" Visible ="true"  HeaderText="Van/FSR" SortExpression="SalesRep_name">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Config_Type" HeaderText="Config Type" SortExpression="Config_Type">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Config_Key" HeaderText="Config Key" SortExpression="Config_Key">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>     
                                                            <asp:TemplateField HeaderText="Config Value">
    <ItemTemplate>
        <asp:TextBox ID="txtConfigValue" runat ="server" CssClass ="inputSM" Text ='<%#Eval("Config_Value") %>'></asp:TextBox>
                    <asp:Label runat ="server" Visible ="false" ID="lblRowID" Text ='<%#Eval("Row_ID") %>' ></asp:Label>
        
    
      </ItemTemplate>
    </asp:TemplateField> 
                                                    </Columns>
                                                   <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
                                                </asp:GridView>
                          </div>
           
                         </ContentTemplate>
     </asp:UpdatePanel> 
                        
                                       
    
             
           
                       
    
  
   
    
  
           <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="TopPanel"
                    runat="server">
                    <ProgressTemplate>
                        <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                            <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                            <span style="font-size: 12px; color: #3399ff;">Processing... </span>
                        </asp:Panel>
                    </ProgressTemplate>
                </asp:UpdateProgress>
              
         
</asp:Content>
