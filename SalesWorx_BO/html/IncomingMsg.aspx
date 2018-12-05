<%@ Page Title="Incoming Message" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="IncomingMsg.aspx.vb"
     Inherits="SalesWorx_BO.IncomingMsg" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
 
  <h4>Incoming Messages</h4>
  
  <telerik:RadWindowManager ID="RadWindowManager1" runat="server" Skin="Simple" EnableShadow="true">
  </telerik:RadWindowManager>  
    <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="Panel" runat="server">
        <ProgressTemplate>
            <asp:Panel ID="Panel11" CssClass="overlay" runat="server">

                <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                   <span style="font-size: 12px; font-weight: 700px; color: #e10000;">Processing...
                            </span>
            </asp:Panel>

        </ProgressTemplate>
    </asp:UpdateProgress>

<asp:UpdatePanel ID="Panel" runat="server">
        <ContentTemplate>
          <div class="row"  >    
             <div class="col-sm-3"> 
                <div class="form-group">
                     <label>From </label> 
                      <telerik:RadDatePicker ID="From_Message_Date" width="100%"  runat="server" Skin="Simple"
                                                      TabIndex="2">
                                                        <calendar Skin="Simple" usecolumnheadersasselectors="False" userowheadersasselectors="False"
                                                            viewselectortext="x">
                                                    </calendar>
                                                        <datepopupbutton hoverimageurl="" imageurl="" tabindex="0" />
                                                        <dateinput readonly="true" dateformat="dd-MM-yyyy" displaydateformat="dd-MM-yyyy">
                                                    </dateinput>
                                             
                     </telerik:RadDatePicker>

                </div>
             </div>
              <div class="col-sm-3">

                <div class="form-group">
                    <label> To  </label> 
                          <telerik:RadDatePicker ID="To_Message_Date" width="100%"  runat="server" Skin="Simple"
                                                      TabIndex="2">
                                                        <calendar Skin="Simple" usecolumnheadersasselectors="False" userowheadersasselectors="False"
                                                            viewselectortext="x">
                                                    </calendar>
                                                        <datepopupbutton hoverimageurl="" imageurl="" tabindex="0" />
                                                        <dateinput readonly="true" dateformat="dd-MM-yyyy" displaydateformat="dd-MM-yyyy">
                                                    </dateinput>
                                             
                                                    </telerik:RadDatePicker>


                
               </div>
             </div>
          </div>
<div class="row"  >    
             <div class="col-sm-6"> 
      <div class="form-group">
            <label>  Send To </label> 
          <telerik:RadComboBox  ID="SalesRep_ID"  Width="100%" EmptyMessage ="Please Type" EnableLoadOnDemand ="true"    Filter="Contains" Skin="Simple"
                    runat="server">
                </telerik:RadComboBox>
      </div>
                 </div>
                 <div class="col-sm-6"> 
      <div class="form-group">
          <label class="hidden-xs"><br /></label>
                         <telerik:RadButton ID="SearchBtn" Skin="Simple" CssClass ="btn btn-primary"  runat="server" Text="Search" TabIndex ="6" >
                                      </telerik:RadButton>
                                   <telerik:RadButton ID="NewMsgBtn" Skin="Simple"   runat="server" Text="New Message" CssClass ="btn btn-success" TabIndex ="7">
                                   </telerik:RadButton>
                     </div>
                 </div>
    </div>
 <div class="table-responsive">
   <asp:GridView EmptyDataText="There is no message"  width="100%" ID="SearchResultGridView" runat="server" 
                   AutoGenerateColumns="False" AllowPaging="True" 
                   PageSize="25" CellPadding="0" CellSpacing="0" CssClass="tablecellalign" AllowSorting="True" DataKeyNames="Message_ID" >
                      
                  <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
                  <Columns>
                      <asp:BoundField DataField="Message_ID" HeaderText="Message_ID"  
                          Visible="False" />
                      <asp:BoundField DataField="Message_Title" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" SortExpression="Message_Title" HeaderText="Title"  
                          />
                      <asp:BoundField DataField="Message_Content" HeaderStyle-HorizontalAlign="Left" HeaderText="Content"  SortExpression ="Message_Content"/>
                      <asp:BoundField DataField="Message_Date"  HeaderStyle-HorizontalAlign="Left" SortExpression="Message_Date" DataFormatString="{0:dd-MM-yyyy}" 
                          HeaderText="Date" HeaderStyle-Wrap="false" ItemStyle-Wrap="false"  />
                      <asp:BoundField DataField="SalesRep_Name" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" SortExpression="SalesRep_Name" HeaderText="User" />
                  </Columns>
                  <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
              </asp:GridView>
    </div>
          </ContentTemplate>
 </asp:UpdatePanel>

</asp:Content>
