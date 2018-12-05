<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ShowAssetHistoryImages.aspx.vb" Inherits="SalesWorx_BO.ShowAssetHistoryImages" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <link rel="icon" href="../images/favicon.ico" type="image/x-icon" />


    <link href="../assets/css/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="../assets/css/font-awesome.css" rel="stylesheet" type="text/css" />
    <link href="../assets/css/style.css" rel="stylesheet" type="text/css" />
    <link href="../assets/css/responsive.css" rel="stylesheet" type="text/css" />
        <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">

         <script type="text/javascript">

             function OpenDialog(item) {
                


                 var URL
                 URL = 'ShowAssetHistoryImage_Tab.aspx?SRC1=' + item

                 window.open(URL, '_blank');

                


                 
                            }

                            function containerMouseover(sender) {
                                sender.getElementsByTagName("div")[0].style.display = "";
                            }
                            function containerMouseout(sender) {
                                sender.getElementsByTagName("div")[0].style.display = "none";
                            }

                            function alertCallBackFn(arg) {

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
    </script>

    </telerik:RadScriptBlock>    

</head>
<body>
    <form id="form1" runat="server">
      <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
         
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
               <AjaxSettings>
                   <telerik:AjaxSetting AjaxControlID="RadAjaxManager2">
                       <UpdatedControls>
                           <telerik:AjaxUpdatedControl ControlID="Panel1" LoadingPanelID="AjaxLoadingPanel1" />
                       </UpdatedControls>
                   </telerik:AjaxSetting>
                   
               </AjaxSettings>
           </telerik:RadAjaxManager>

     <asp:HiddenField ID="hfAssetID" runat="server" />
    <asp:HiddenField ID="hfRowID" runat="server" />
                          <asp:HiddenField ID="HImage" runat="server" />
                                             <asp:HiddenField ID="HThumbNail" runat="server" />

           <asp:Panel ID="Panel1" runat="server">      </asp:Panel> 
       <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>

	        <telerik:RadAjaxPanel  runat ="server" ID="raf">
                     <telerik:RadWindow ID="MapWindow" Title ="Product Image" runat="server" modal="true"  Behaviors="Move,Close" 
         width="550px" height="500px" skin="Windows7"  ReloadOnShow="false"  VisibleStatusbar="false"  Overlay="true"  >
               <ContentTemplate>
                     <telerik:RadBinaryImage  runat="server" ID="RadBinaryImage12" resizemode="fit"
                        />
               </ContentTemplate>
          </telerik:RadWindow>
        
                  <div class="" style="padding-top:15px; overflow:hidden;">
                 <div class="col-sm-4">
                    <label>Customer Name</label>
                     <p class="font-weight-600"><asp:Label ID="lbl_CusName" runat="server"></asp:Label></p>
                </div>
                <div class="col-sm-4">
                    <label>Asset Type</label>
                    <p class="font-weight-600"><asp:Label ID="lbl_AssetType" runat="server"></asp:Label></p>
                </div>
                <div class="col-sm-4">
                    <label>Logged At</label>
                    <p class="font-weight-600"><asp:Label ID="lbl_logged_at" runat="server"></asp:Label></p>
                </div>
              </div>                                
                    
  

           <div class="form-group">
               
               <asp:DataList ID="ImgList" runat="server" CellSpacing="3" RepeatLayout="Table" RepeatColumns="3" CssClass="hidebr table">
            
            <ItemTemplate>
                <table cellpadding="0" cellspacing="0" border="0" width="100%" class="table">
                   
                    <%--<tr>
                    
                        <td>
                             <br /><asp:Label ID="Label1" runat="server" Text='<%#Eval("Description")%>'></asp:Label>
                            <asp:HiddenField ID="HID" runat="server" Value='<%#Eval("Row_ID")%>'></asp:HiddenField>
                             <asp:HiddenField ID="HFile" runat="server" Value='<%#Eval("File_Name")%>'></asp:HiddenField>
                            <asp:HiddenField ID="HThumbNail" Value='<%#Eval("File_Name")%>' runat="server" />
                        </td>
                    </tr>--%>
                    <tr><td  style="vertical-align:middle;text-align:center;padding:0 !important;border:0;">
                         
                        <p><a href="javascript:OpenDialog('<%#Eval("SRC1") %>')">
                        <asp:Image ID="Image1" ImageUrl='<%#Eval("SRC") %>' ToolTip='<%#Eval("Description")%>' runat="server" style="max-width:100%; max-height:100%;"></asp:Image>
                            </a></p>
                        <p><asp:Label ID="Label2" runat="server" Text='<%#Eval("Description")%>'></asp:Label></p>
                            </td></tr>
                </table>
                
            </ItemTemplate>
                   <ItemStyle CssClass=""/>
       </asp:DataList> 
                   
            </div>
                                     
                               

             </telerik:RadAjaxPanel> 
    </form>
</body>
</html>
