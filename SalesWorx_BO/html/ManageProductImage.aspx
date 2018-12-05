<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="ManageProductImage.aspx.vb" Inherits="SalesWorx_BO.ManageProductImage" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
           <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
                       
                        <script type="text/javascript">

                            function OpenDialog(item) {


                                //  alert(item.href);
                                var img = $get("<%= RadBinaryImage12.ClientID %>");
                                img.src = item
                                var $ = $telerik.$;
                                var mapWindow = $find("<%=MapWindow.ClientID%>");
                                mapWindow.show();
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

                       
        <script type="text/javascript">

            function OnClientFilesUploaded(sender, args) {
                $find('<%= RadAjaxManager1.ClientID %>').ajaxRequest();
            }
            function OnClientFilesUploaded1(sender, args) {
                $find('<%= RadAjaxManager1.ClientID %>').ajaxRequest();
            }
        </script>
        <script type="text/javascript">
            function Demo(sender, args) {
                var upload = $find("<%= upMedia.ClientID %>");

                if (upload.getUploadedFiles().length != 0)
                    args.IsValid = true;
                else
                    args.IsValid = false;
            }
</script>
   
    </telerik:RadScriptBlock>      
<style type="text/css">
    #ctl00_ContentPlaceHolder1_MapWindow_C
{
	overflow: hidden !important;
}
    .imagecontainer
{
    float:left;
    width:430px;
    margin:6px 15px 6px 0;
    text-align:center;
}
    /*.ruButton .ruRemove, .RadUpload .ruRemove,
    ruButton .ruCancel, .RadUpload .ruCancel
    {
        display:none !important;
    }*/
    .ruBrowse {
  width: 65px;
 margin-left: 0px!important;
  background-position: 0 0;
  
}
    
    .RadUpload_Default .ruFakeInput {
  border-color: #898772;
  color: #333333;
  visibility: hidden !important;
  padding-left:-3px;
  width: 0px !important;
}
   
      .myClass:hover
{
    background-color: #F0A844 !important;
}
.txt
{
    border: 0px !important;
    background: #F0A844 !important;
    color: maroon !important;
   font-weight:bold ;
   
    width: 250px;
     text-align: left;
}
#list
{
    max-width: 900px;
}
        
   .DropZone1
{
    width: 300px;
    height: 90px;
    background-color: #357A2B;
    border-color: #CCCCCC;
    color: #767676;
    float: left;
    text-align: center;
    font-size: 16px;
    color: white;
}
 
 #DropZone2
{
    width: 300px;
    height: 90px;
    background-color: #357A2B;
    border-color: #CCCCCC;
    color: #767676;
    float: left;
    margin-left: 90px;
    text-align: center;
    font-size: 16px;
    color: white;
}

.RadUpload ul li  {
        margin:0;
        padding:0;
    } 

        
.hidebr br{
display:none; 
}    
  
     
        
        
        
        
        ._Telerik_IE9 .RadDock.rieDialogs {
            z-index: 20001;
        }
 
        ._Telerik_IE9 .rcbSlide {
            z-index: 20002 !important;
        }
 
        #dwndWrapper {
            height: 85px;
            background-image: url("../../gfx/grey_noise2.png");
            background-position: left;
            background-repeat: no-repeat;
            padding: 15px 0 0 100px;
        }
        #MainContent_MapWindow_C    { display:table-cell; vertical-align:middle; text-align:center;        }
        #MainContent_MapWindow_C img   { max-width:100%; height:auto; margin:10px auto;         }
 
</style>
 
           

    </asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
  
      <h4>Manage Product Images</h4>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
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

        <asp:Panel ID="Panel1" runat="server">      </asp:Panel> 
       <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>



   
                     <asp:Label ID="lblMediaFile" runat ="server" Visible ="false"></asp:Label>
                     
                    <telerik:RadAjaxPanel  runat ="server" ID="raf">
                     <telerik:RadWindow ID="MapWindow" Title ="Product Image" runat="server" modal="true"  Behaviors="Move,Close" 
         width="550px" height="500px" skin="Windows7"  ReloadOnShow="false"  VisibleStatusbar="false"  Overlay="true"  >
               <ContentTemplate>
                     <telerik:RadBinaryImage  runat="server" ID="RadBinaryImage12" resizemode="fit"
                        />
               </ContentTemplate>
          </telerik:RadWindow>
           <div class="row">
               <div class="col-sm-4">
                            <div class="form-group">
           <label>Organization *   </label> 
                                  <telerik:RadComboBox ID="ddOraganisation" Skin="Simple" AutoPostBack="true" EmptyMessage="Please type a organization"
                                                   Width="100%"  Filter="Contains"    TabIndex="1"
                                                    runat="server" />
                              
                            </div> 
                   </div>
               <div class="col-sm-4">
            <div class="form-group">
                                               <label>     Product Name *       <asp:CompareValidator ID="CompareValidatorRadComboxBoxTables" runat="server" ValidationGroup="valsum"
 ValueToCompare="--Select--" Operator="NotEqual" ControlToValidate="ddlItemCode" errorMessage="" Font-Bold="true" /> </label> 
                                             
                                               <%--    <telerik:RadComboBox ID="ddlItemCode" Skin ="Simple" TabIndex="1" runat="server"   Filter="Contains"
                                                Height="200"      Width="80%" EnableLoadOnDemand ="true"      AutoPostBack="true"   />--%>

                  <telerik:RadComboBox ID="ddlItemCode" Skin="Simple" TabIndex="1" runat="server"
                                                                Filter="Contains"  emptyMessage="Please type product code/ description"
  EnableLoadOnDemand="True"   Height="250px" 
                                                                 Width="100%" AutoPostBack="true" />
                                            
                                            </div> 
                   </div>
               <div class="col-sm-4">
            <div class="form-group">
                                               <label>
                                                    Caption *         
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCaption"
                                ErrorMessage=""  Font-Bold="true" ValidationGroup="valsum"></asp:RequiredFieldValidator></label> 
                                              
                
                                 <telerik:RadTextBox runat ="server" ID="txtCaption"   Skin ="Simple"     Width="100%" ></telerik:RadTextBox>
                                 
 </div> 
                   </div>
               </div>
            <div class="row">
                <div class="col-sm-12">
            <div class="form-group">

                                        <label>
                                                   Product Image * 
                                           </label> 
                                         <telerik:RadAsyncUpload ID="upMedia" runat="server"    
                                           TabIndex ="5"    TemporaryFolder="C:\SalesWorx_CS\ExcelFolder" 
                                       
                                                           Skin ="Simple"  
            Localization-Select="Upload" ToolTip ="Select or drop a file here"   
               MultipleFileSelection ="Disabled"   InitialFileInputsCount="1"  
              MaxFileInputsCount="1"        />  
                  <asp:HiddenField ID="HImage" runat="server" />
                                             <asp:HiddenField ID="HThumbNail" runat="server" />
         </div>                      
            </div>                      
</div>
            
                                            <div class="form-group">

                                           
                                  <telerik:RadButton ID="btnConfirm" Skin ="Simple"  OnClick="btnConfirm_Click"    runat="server" Text="Save" CssClass ="btn btn-primary" />
                                       <telerik:RadButton ID="btnReset"  Skin ="Simple"     runat="server" Text="Clear" CssClass ="btn btn-default" />
                       </div>
                                                  
                    
  

           <div class="form-group">
               
               <asp:DataList ID="ImgList" runat="server" RepeatDirection="Vertical" RepeatLayout="Flow" CssClass="hidebr row">
            
            <ItemTemplate>
                <table cellpadding="0" cellspacing="0" border="0" width="100%" class="table">
                   
                    <tr>
                        <td style="vertical-align:middle;"><asp:ImageButton ToolTip="Delete" ID="btnCan" runat="server" 
                                                                        OnClick="btnDelete_Click" CausesValidation="false"  ImageUrl="~/images/delete-13.png" OnClientClick="javascript:return confirm('Would you like to delete the image?');" />
                         </td>
                        <td>
                            <b>Item Code</b> <br /><asp:Label ID="Label1" runat="server" Text='<%#Eval("Item_Code")%>'></asp:Label>
                            <asp:HiddenField ID="HID" runat="server" Value='<%#Eval("Media_File_ID")%>'></asp:HiddenField>
                             <asp:HiddenField ID="HFile" runat="server" Value='<%#Eval("Filename")%>'></asp:HiddenField>
                            <asp:HiddenField ID="HThumbNail" Value='<%#Eval("Thumbnail")%>' runat="server" />
                        </td>
                    </tr>
                    <tr><td colspan="2" style="vertical-align:middle;text-align:center;">
                        <a href="javascript:OpenDialog('<%#Eval("SRC1") %>')">
                        <asp:Image ImageUrl='<%#Eval("SRC") %>' ToolTip='<%#Eval("Caption")%>' runat="server" style="width:auto; height:80px;"></asp:Image>
                            </a>
                            </td></tr>
                </table>
                
            </ItemTemplate>
                   <ItemStyle CssClass="col-sm-2"/>
       </asp:DataList> 
                   
            </div>
                                     
                               

             </telerik:RadAjaxPanel> 
           
                
          
</asp:Content>
