<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ShowAssetHistoryImage_Tab.aspx.vb" Inherits="SalesWorx_BO.ShowAssetHistoryImage_Tab" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     

</head>
<body>
    <form id="form1" runat="server">
      <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
          <h4 runat ="server" >AssetHistory Image</h4>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
               <AjaxSettings>
                   <telerik:AjaxSetting AjaxControlID="RadAjaxManager2">
                       <UpdatedControls>
                           <telerik:AjaxUpdatedControl ControlID="Panel1" LoadingPanelID="AjaxLoadingPanel1" />
                       </UpdatedControls>
                   </telerik:AjaxSetting>
                   
               </AjaxSettings>
           </telerik:RadAjaxManager>

    
                     

           <asp:Panel ID="Panel1" runat="server">      </asp:Panel> 
       <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>

	        <telerik:RadAjaxPanel  runat ="server" ID="raf">
                    
                                                  
                    
  

           <div class="form-group">
               
             <telerik:RadBinaryImage  runat="server" ID="RadBinaryImage12" resizemode="fit"
                        />
                   
            </div>
                                     
                               

             </telerik:RadAjaxPanel> 
    </form>
</body>
</html>
