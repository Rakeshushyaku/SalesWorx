<%@ Page Title="Plans For Approval" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="PlansForApproval.aspx.vb" Inherits="SalesWorx_BO.PlansForApproval" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
    <script>
        function alertCallBackFn(arg) {
           
        }
    </script>
 </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  <h4>Plans for Approval</h4>
  <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
  </telerik:RadWindowManager>
  <asp:UpdatePanel ID="ClassUpdatePnl" runat="server" UpdateMode="conditional">
     <ContentTemplate>

        <div class="form-group">  
            <asp:Label runat="server" ID="ConfirmationMsg"></asp:Label>
        </div>
	    <div class="form-group">
              <asp:GridView width="100%" ID="ApprovalPlans" DataKeyNames="FSR_Plan_ID" 
                        runat="server" AutoGenerateColumns="False"  RowStyle-Wrap="false" 
                        PageSize="25" CellPadding="0" CellSpacing="0" CssClass="tablecellalign" >
                   
                        <Columns>
                            <asp:BoundField  DataField="FSR_Plan_ID" 
                                ShowHeader="False" Visible="False" >
                        <ItemStyle Width="100px"></ItemStyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Van/FSR" HeaderStyle-HorizontalAlign ="Left">
                                <ItemTemplate>
                                    <%--<asp:Label ID="Label1" runat="server" Text='<%# Bind("SalesRep_Name") %>'></asp:Label>--%>
                                    <asp:LinkButton CssClass="txtLinkSM" CommandName="Approve" ID="SalesLink" runat="server" Text='<%# Bind("SalesRep_Name") %>' ToolTip="View/Approve Plan"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Route_Plan"  HeaderText="Route Plan" >
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                             <asp:BoundField DataField="Site"  HeaderText="Site" >
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="No_Of_Working_Days" HeaderText="Working Days" />
                            <asp:BoundField DataField="No_Of_Visits" HeaderText="No Of Visits" />
                            <asp:TemplateField HeaderText="Approval">
                                <ItemTemplate>
                                    <%--<asp:Label ID="Label1" runat="server" Text='<%# Bind("SalesRep_Name") %>'></asp:Label>--%>
                                    <asp:LinkButton CssClass="txtLinkSM" CommandName="Approval" ID="SalesApprovalLink" runat="server" Text="Approve" ToolTip="Approve Plan"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                       
                        </Columns>
                       <PagerStyle CssClass="pagerstyle" />
                                                        <HeaderStyle />
                                                        <RowStyle CssClass="tdstyle" />
                                                        <AlternatingRowStyle CssClass="alttdstyle" />
                    </asp:GridView>
        </div>  	
	    <div class="form-group"> 
            <asp:Label ID="MsgLbl" runat="server" Cssclass='txtSM'></asp:Label>
        </div>
	</ContentTemplate>
 </asp:UpdatePanel>
   <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="ClassUpdatePnl"
                    runat="server">
                    <ProgressTemplate>
                        <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                            <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                            <span style="font-size: 12px; color: #e10000;">Processing... </span>
                        </asp:Panel>
                    </ProgressTemplate>
       </asp:UpdateProgress>
</asp:Content>
