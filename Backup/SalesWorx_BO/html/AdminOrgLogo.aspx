<%@ Page Title="Admin Organization Logo" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master"
    CodeBehind="AdminOrgLogo.aspx.vb" Inherits="SalesWorx_BO.AdminOrgLogo" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<style>
    .imageContainer {
    float: left; 
    margin: 5px; 
    padding: 2px; 
    position: relative; 
    background: #eeeeee;
}
 
    .imageContainer:hover {
        background-color: #a1da29 !important;
    }
 
.buttonsWrapper {
    display: inline-block;
    vertical-align:middle;
}
 
.image {
    cursor: pointer; 
    display: block;
}
 
.txt {
    border: 0px !important;
    background: #eeeeee !important;
    color: Black !important;
    margin-left: 25%;
    margin-right: auto;
    width: 100%;
    filter: alpha(opacity=50); /* IE's opacity*/
    opacity: 0.50;
    text-align: center;
}
 
#list {
    max-width: 900px;
}
 
.clearFix {
    clear: both;
}
 
.demo-container {
    max-width: 856px;
}
 
.sliderWrapper {
    float:left; 
    display:inline-block;
}
</style>
    <script language="javascript" type="text/javascript">


                           function OpenDialog(item) {


                             //  alert(item.href);
                               var img = $get("<%= RadBinaryImage12.ClientID %>");
                               img.src = item.href;
            var $ = $telerik.$;
            var mapWindow = $find("<%=MapWindow.ClientID%>");
            mapWindow.show();
         }


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

    </script>
 <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
                <tr>
                    <td id="contentofpage" width="100%" height="76%" class="topshadow">
                        <div class="pgtitileposition">
                            <span class="pgtitile3">Upload Organization Logo</span></div>
                        <div id="pagenote">
                            This screen may be used to upload the organization logo.</div>
                            <asp:UpdatePanel runat="server" ID="UPModal" UpdateMode ="Conditional" >
                            <Triggers>
                            <asp:PostBackTrigger  ControlID ="btnUpload" />
                            </Triggers>
        <ContentTemplate>
                        <table width="auto" border="0" cellspacing="0" cellpadding="0" id="tableform">
                            <tr>
                                <th>
                                </th>
                            </tr>
                            <tr>
                                <td class="txtSMBold">
                                    <asp:Label ID="lblLable" runat="server" Font-Bold ="true"  Text="Organization :"></asp:Label>&nbsp;&nbsp;
                                    <asp:DropDownList ID="ddOraganisation" Width ="200px" runat="server" CssClass="inputSM">
                                    </asp:DropDownList>
                               
                          <asp:FileUpload  class="file" id="fUpload"        runat="server"/>
                                    <asp:Button ID="btnUpload"   runat="server" CausesValidation="false"  CssClass="btnInputBlue" 
                                        TabIndex="2" Text="Upload Logo" Width="100" OnClick="btnUpload_Click" />
                                </td>
                                </tr>
                                               
                                    </table>
                           
                       
                          <table width="auto" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td>
                                <asp:Button ID="btnHidden" runat="Server" Style="display: none" />
                                <ajaxToolkit:ModalPopupExtender ID="MpInfoError" runat="server" BackgroundCssClass="modalBackground"
                                    TargetControlID="btnHidden" PopupControlID="pnlConfirm" CancelControlID="btnClose"
                                    Drag="true" />
                                <asp:Panel ID="pnlConfirm" Width="400" runat="server" CssClass="modalPopup" Style="cursor: move;
                                    background-color: #3399ff; text-align: center; border: solid 1px #3399ff; color: White;
                                    padding: 3px; display: none;">
                                    <table id="tableinPopupErr" width="400" cellpadding="10" style="background-color: White;">
                                        <tr align="center">
                                            <td align="center" style="cursor: move; background-color: #3399ff; text-align: center;
                                                border: solid 1px #3399ff; color: White; padding: 3px">
                                                <asp:Label ID="lblinfo" runat="server" Font-Size ="13px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" style="text-align: center">
                                                                                                <asp:Label ID="lblMessage"  Font-Size ="13px" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" style="text-align: center;">
                                                <asp:Button ID="btnClose" runat="server" Text="Ok"  CssClass="btnInputBlue"  />
                                            </td>
                                        </tr>
                                    </table>
                                     </asp:Panel>
                   
                   
                            <asp:GridView Width="100%" ID="gvOrgLogo" runat="server" EmptyDataText="No logos to display"
                                                    EmptyDataRowStyle-Font-Bold="true"  AutoGenerateColumns="False"
                                                    AllowPaging="false" AllowSorting="false" PageSize="25" CellPadding="0" CellSpacing="0" CssClass="tablecellalign">
                                                  
                                                    <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
                                                    <Columns>
                                                       
                                                        <asp:BoundField DataField="OrgID" HeaderText="Org ID" SortExpression="OrgID">
                                                            <ItemStyle Wrap="False" VerticalAlign="Top" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="OrgName" HeaderText="Org Name" SortExpression="OrgName">
                                                            <ItemStyle Wrap="False" VerticalAlign="Top" />
                                                        </asp:BoundField>
                                                             
                                                              <asp:TemplateField HeaderText="Logo">
    <ItemTemplate>
        
                 <a href='<%#Eval("LogoPath") %>'
                             style="font-weight:bold;"  onclick="OpenDialog(this);  return false"> 
                <asp:ImageButton ID="imgLogo" runat ="server"  ImageUrl='<%#Eval("LogoPath") %>' ToolTip ="Click and enlarge the logo"  Width ="400px" Height ="90px"
               />
               </a> 
                      
   
        
    
      </ItemTemplate>
    </asp:TemplateField>
                                                    </Columns>
                                                    <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
                                                </asp:GridView>
                                
   
       
     
       <telerik:RadWindow ID="MapWindow" Title ="Organization Logo" runat="server"  Behaviors="Move,Close" 
         width="1000px" height="400px"  ReloadOnShow="false"  VisibleStatusbar="false" Modal ="true"   Overlay="true"  >
               <ContentTemplate>
                     <telerik:RadBinaryImage Style="cursor: pointer; display: block;"  runat="server" ID="RadBinaryImage12" 
                    width="1960px" Height ="277px"
                     resizemode="fit" 
                        />
               </ContentTemplate>
          </telerik:RadWindow>
        </td>
        </tr>                        
                          
                    </table>
                         </ContentTemplate>
     </asp:UpdatePanel> 
                        </td> 
                        </tr> 
                        </table> 
       
        
  
   
    
  
           <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="UpModal"
                    runat="server">
                    <ProgressTemplate>
                        <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                            <img src="../images/Progress.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                            <span style="font-size: 12px; color: #3399ff;">Processing... </span>
                        </asp:Panel>
                    </ProgressTemplate>
                </asp:UpdateProgress>
              
         
</asp:Content>
