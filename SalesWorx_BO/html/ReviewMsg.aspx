<%@ Page Title="Review Message" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="ReviewMsg.aspx.vb" 
    Inherits="SalesWorx_BO.ReviewMsg" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
      <script type="text/javascript">

          function alertCallBackFn(arg) {

          }
    </script>
    </asp:Content>


 <asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
     <style>
     RadTabStrip .rtsLevel .rtsOut, .RadTabStrip .rtsLevel .rtsIn, .RadTabStrip .rtsLevel .rtsTxt
        {
            text-decoration: none !important;
            font: 12px Segoe UI !important;
            color: #0090d9;
        }
</style>
    <h4>Message Status</h4>
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
                     <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager> 
                                

 	<asp:UpdatePanel ID="Panel" runat="server">
        <ContentTemplate>
                 <div class="form-group">
                   

         <label>  From
                
                   </label> 
               

          
                        <telerik:RadDatePicker ID="From_Message_Date" width="20%"  runat="server" Skin="Simple"
                                              TabIndex="2">
                                                <calendar Skin="Simple" usecolumnheadersasselectors="False" userowheadersasselectors="False"
                                                    viewselectortext="x">
                                            </calendar>
                                                <datepopupbutton hoverimageurl="" imageurl="" tabindex="0" />
                                                <dateinput readonly="true" dateformat="dd-MM-yyyy" displaydateformat="dd-MM-yyyy">
                                            </dateinput>
                                             
                                            </telerik:RadDatePicker>


                
                 </div>
             <div class="form-group">
                   

         <label>  To
                
                   </label> 
               

          
                        <telerik:RadDatePicker ID="To_Message_Date" width="20%"  runat="server" Skin="Simple"
                                              TabIndex="2">
                                                <calendar Skin="Simple" usecolumnheadersasselectors="False" userowheadersasselectors="False"
                                                    viewselectortext="x">
                                            </calendar>
                                                <datepopupbutton hoverimageurl="" imageurl="" tabindex="0" />
                                                <dateinput readonly="true" dateformat="dd-MM-yyyy" displaydateformat="dd-MM-yyyy">
                                            </dateinput>
                                             
                                            </telerik:RadDatePicker>


                
                 </div>

            <div class="form-group">
                   

         <label>  Send To
                
                   </label> 
               

          
                        <telerik:RadComboBox ID="SalesRep_ID" Skin="Simple"  EnableLoadOnDemand ="true"    Filter="Contains" height="200px"   TabIndex ="1" 
                                       runat="server" Width="30%" EmptyMessage ="Please type a user">
                                    </telerik:RadComboBox>

                 <telerik:RadButton ID="SearchBtn" Skin="Simple" CssClass ="btn btn-primary"  runat="server" Text="Search" TabIndex ="6" >
                                      </telerik:RadButton>
                                  
                
                 </div>
        <table width="100%" border="0" cellspacing="2" cellpadding="2"  >
        <tr>
        <td>
              <asp:GridView EmptyDataText="No messages to display."  width="100%" 
                  ID="SearchResultGridView" runat="server" 
                   AutoGenerateColumns="False" AllowPaging="True" 
                   PageSize="8" CellPadding="0" CellSpacing="0" CssClass="tablecellalign" AllowSorting="True" DataKeyNames="Message_ID" >
                    
                  <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
                  <Columns>
                      <asp:BoundField DataField="Message_ID" HeaderText="Message_ID" 
                          Visible="False" />
                      <asp:BoundField DataField="Message_Title" HeaderStyle-Wrap="false" 
                          ItemStyle-Wrap="false" SortExpression="Message_Title" HeaderText="Message Title" 
                          NullDisplayText="N/A" >
                          <HeaderStyle Wrap="False" />
                          <ItemStyle Wrap="False" />
                      </asp:BoundField>
                      <asp:BoundField DataField="Message_Content" HeaderStyle-HorizontalAlign="Left" 
                          HeaderText="Message Content" >
                          <HeaderStyle Wrap="False" />
                          <ItemStyle Wrap="False" />
                      </asp:BoundField>
                      <asp:BoundField DataField="Message_Date"  HeaderStyle-HorizontalAlign="Left" 
                          SortExpression="Message_Date" DataFormatString="{0:dd-MM-yyyy}" 
                          HeaderText="Message Date" HeaderStyle-Wrap="false" 
                          ItemStyle-Wrap="false"  >
                            <HeaderStyle Wrap="False" />
                          <ItemStyle Wrap="False" />
                      </asp:BoundField>
                       <asp:BoundField DataField="Sender_Name" HeaderStyle-HorizontalAlign="Left" 
                          HeaderStyle-Wrap="false" SortExpression="Sender_Name" HeaderText="Send From" >
                            <HeaderStyle Wrap="False" />
                          <ItemStyle Wrap="False" />
                      </asp:BoundField>
                      <asp:BoundField DataField="SalesRep_Name" HeaderStyle-HorizontalAlign="Left" 
                          HeaderStyle-Wrap="false" SortExpression="SalesRep_Name" HeaderText="Send To" >
                            <HeaderStyle Wrap="False" />
                          <ItemStyle Wrap="False" />
                      </asp:BoundField>
                      <asp:BoundField DataField="Message_Read"  ItemStyle-HorizontalAlign="Left"  
                          HeaderText="Message Read" SortExpression ="Message_Read" >
                         <HeaderStyle Wrap="False" HorizontalAlign ="Left" />
                          <ItemStyle Wrap="False" />
                      </asp:BoundField>
                      <asp:BoundField DataField="Message_Reply" ItemStyle-HorizontalAlign="Left"  HeaderText="Message Reply" 
                          NullDisplayText="N/A" >
                            <HeaderStyle Wrap="False"  />
                          <ItemStyle Wrap="False" />
                      </asp:BoundField>
                      <asp:BoundField DataField="Reply_Date"  ItemStyle-HorizontalAlign="Left"  HeaderText="Reply Date" 
                          NullDisplayText="N/A" DataFormatString="{0:dd-MM-yyyy}" ItemStyle-Wrap="false" SortExpression ="Reply_Date"  >
                          <HeaderStyle Wrap="False" />
                          <ItemStyle Wrap="False" />
                      </asp:BoundField>
                  </Columns>
              <PagerStyle  BackColor ="#e4e4e4" BorderStyle ="None" BorderColor ="LightGray"  />
                                                    <HeaderStyle BackColor ="#e4e4e4" BorderStyle ="None" BorderColor ="LightGray"  />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
              </asp:GridView>
          </td>
          </tr>
          
        </table>


            <telerik:RadTabStrip ID="MessageTab" runat="server" Skin="Windows7" MultiPageID="RadMultiPage2"
                                                SelectedIndex="0">
                                                <Tabs>
                                                    <telerik:RadTab Text="Outbox" runat="server">
                                                    </telerik:RadTab>
                                                    <telerik:RadTab Text="Inbox" runat="server">
                                                    </telerik:RadTab>
                                                </Tabs>
                                            </telerik:RadTabStrip>
                                            <telerik:RadMultiPage ID="RadMultiPage2" runat="server" SelectedIndex="0">
                                                <telerik:RadPageView ID="RadPageView4" runat="server" Height="570px">
                                                           <asp:GridView EmptyDataText="No messages to display."  width="100%" 
                  ID="gvOutBox" runat="server" 
                   AutoGenerateColumns="False" AllowPaging="True" 
                   PageSize="8" CellPadding="0" CellSpacing="0" CssClass="tablecellalign" AllowSorting="True" DataKeyNames="Message_ID" >
                    
                  <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
                  <Columns>
                      <asp:BoundField DataField="Message_ID" HeaderText="Message_ID" 
                          Visible="False" />
                      <asp:BoundField DataField="Message_Title" HeaderStyle-Wrap="false" 
                          ItemStyle-Wrap="false" SortExpression="Message_Title" HeaderText="Message Title" 
                          NullDisplayText="N/A" >
                          <HeaderStyle Wrap="False" />
                          <ItemStyle Wrap="False" />
                      </asp:BoundField>
                      <asp:BoundField DataField="Message_Content" HeaderStyle-HorizontalAlign="Left" 
                          HeaderText="Message Content" >
                          <HeaderStyle Wrap="False" />
                          <ItemStyle Wrap="False" />
                      </asp:BoundField>
                      <asp:BoundField DataField="Message_Date"  HeaderStyle-HorizontalAlign="Left" 
                          SortExpression="Message_Date" DataFormatString="{0:dd-MM-yyyy}" 
                          HeaderText="Message Date" HeaderStyle-Wrap="false" 
                          ItemStyle-Wrap="false"  >
                            <HeaderStyle Wrap="False" />
                          <ItemStyle Wrap="False" />
                      </asp:BoundField>
                       <asp:BoundField DataField="Sender_Name" HeaderStyle-HorizontalAlign="Left" 
                          HeaderStyle-Wrap="false" SortExpression="Sender_Name" HeaderText="Send From" >
                            <HeaderStyle Wrap="False" />
                          <ItemStyle Wrap="False" />
                      </asp:BoundField>
                      <asp:BoundField DataField="SalesRep_Name" HeaderStyle-HorizontalAlign="Left" 
                          HeaderStyle-Wrap="false" SortExpression="SalesRep_Name" HeaderText="Send To" >
                            <HeaderStyle Wrap="False" />
                          <ItemStyle Wrap="False" />
                      </asp:BoundField>
                      <asp:BoundField DataField="Message_Read"  ItemStyle-HorizontalAlign="Left"  
                          HeaderText="Message Read" SortExpression ="Message_Read" >
                         <HeaderStyle Wrap="False" HorizontalAlign ="Left" />
                          <ItemStyle Wrap="False" />
                      </asp:BoundField>
                      <asp:BoundField DataField="Message_Reply" ItemStyle-HorizontalAlign="Left"  HeaderText="Message Reply" 
                          NullDisplayText="N/A" >
                            <HeaderStyle Wrap="False"  />
                          <ItemStyle Wrap="False" />
                      </asp:BoundField>
                      <asp:BoundField DataField="Reply_Date"  ItemStyle-HorizontalAlign="Left"  HeaderText="Reply Date" 
                          NullDisplayText="N/A" DataFormatString="{0:dd-MM-yyyy}" ItemStyle-Wrap="false" SortExpression ="Reply_Date"  >
                          <HeaderStyle Wrap="False" />
                          <ItemStyle Wrap="False" />
                      </asp:BoundField>
                  </Columns>
              <PagerStyle  BackColor ="#e4e4e4" BorderStyle ="None" BorderColor ="LightGray"  />
                                                    <HeaderStyle BackColor ="#e4e4e4" BorderStyle ="None" BorderColor ="LightGray"  />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
              </asp:GridView>
                                                  </telerik:RadPageView> 


                                                <telerik:RadPageView ID="RadPageView1" runat="server" Height="570px">
                                                         <asp:GridView EmptyDataText="No messages to display."  width="100%" 
                  ID="gvInbox" runat="server" 
                   AutoGenerateColumns="False" AllowPaging="True" 
                   PageSize="8" CellPadding="0" CellSpacing="0" CssClass="tablecellalign" AllowSorting="True" DataKeyNames="Message_ID" >
                    
                  <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
                  <Columns>
                      <asp:BoundField DataField="Message_ID" HeaderText="Message_ID" 
                          Visible="False" />
                      <asp:BoundField DataField="Message_Title" HeaderStyle-Wrap="false" 
                          ItemStyle-Wrap="false" SortExpression="Message_Title" HeaderText="Message Title" 
                          NullDisplayText="N/A" >
                          <HeaderStyle Wrap="False" />
                          <ItemStyle Wrap="False" />
                      </asp:BoundField>
                      <asp:BoundField DataField="Message_Content" HeaderStyle-HorizontalAlign="Left" 
                          HeaderText="Message Content" >
                          <HeaderStyle Wrap="False" />
                          <ItemStyle Wrap="False" />
                      </asp:BoundField>
                      <asp:BoundField DataField="Message_Date"  HeaderStyle-HorizontalAlign="Left" 
                          SortExpression="Message_Date" DataFormatString="{0:dd-MM-yyyy}" 
                          HeaderText="Message Date" HeaderStyle-Wrap="false" 
                          ItemStyle-Wrap="false"  >
                            <HeaderStyle Wrap="False" />
                          <ItemStyle Wrap="False" />
                      </asp:BoundField>
                       <asp:BoundField DataField="Sender_Name" HeaderStyle-HorizontalAlign="Left" 
                          HeaderStyle-Wrap="false" SortExpression="Sender_Name" HeaderText="Send From" >
                            <HeaderStyle Wrap="False" />
                          <ItemStyle Wrap="False" />
                      </asp:BoundField>
                      <asp:BoundField DataField="SalesRep_Name" HeaderStyle-HorizontalAlign="Left"  Visible ="false" 
                          HeaderStyle-Wrap="false" SortExpression="SalesRep_Name" HeaderText="Send To" >
                            <HeaderStyle Wrap="False" />
                          <ItemStyle Wrap="False" />
                      </asp:BoundField>
                      <asp:BoundField DataField="Message_Read"  ItemStyle-HorizontalAlign="Left"    Visible ="false" 
                          HeaderText="Message Read" SortExpression ="Message_Read" >
                         <HeaderStyle Wrap="False" HorizontalAlign ="Left" />
                          <ItemStyle Wrap="False" />
                      </asp:BoundField>
                      <asp:BoundField DataField="Message_Reply" ItemStyle-HorizontalAlign="Left"  Visible ="false"   HeaderText="Message Reply" 
                          NullDisplayText="N/A" >
                            <HeaderStyle Wrap="False"  />
                          <ItemStyle Wrap="False" />
                      </asp:BoundField>
                      <asp:BoundField DataField="Reply_Date"  ItemStyle-HorizontalAlign="Left"  HeaderText="Reply Date"   Visible ="false" 
                          NullDisplayText="N/A" DataFormatString="{0:dd-MM-yyyy}" ItemStyle-Wrap="false" SortExpression ="Reply_Date"  >
                          <HeaderStyle Wrap="False" />
                          <ItemStyle Wrap="False" />
                      </asp:BoundField>
                  </Columns>
              <PagerStyle  BackColor ="#e4e4e4" BorderStyle ="None" BorderColor ="LightGray"  />
                                                    <HeaderStyle BackColor ="#e4e4e4" BorderStyle ="None" BorderColor ="LightGray"  />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
              </asp:GridView>
                                                  </telerik:RadPageView>
                                                </telerik:RadMultiPage> 
          </ContentTemplate>
        </asp:UpdatePanel>
	
</asp:Content>
