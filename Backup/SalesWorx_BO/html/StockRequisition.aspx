<%@ Page Language="vb" AutoEventWireup="false"  CodeBehind="StockRequisition.aspx.vb" Inherits="SalesWorx_BO.StockReq" MasterPageFile="~/html/DefaultLayout.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script src='../js/jquery.ba-throttle-debounce.min.js'></script>
     <script src='../js/jquery.stickyheader.js'></script>
     <!--[if IE]>
  		<script src='../js/html5.js'></script>
	 <![endif]-->
	 <link rel='stylesheet' href='../styles/component.css' />
<script type="text/javascript">
    function showDialogue() {
        createtbl()
    }

   
    function validate() {
        if (document.getElementById("ctl00_ContentPlaceHolder1_ddl_Van").value == '0') {
            alert("Select Van")
            document.getElementById("ctl00_ContentPlaceHolder1_ddl_Van").focus()
            return false
        }
        if (document.getElementById("ctl00_ContentPlaceHolder1_ddlSKU").value == '0') {
            alert("Select Product")
            document.getElementById("ctl00_ContentPlaceHolder1_ddlSKU").focus()
            return false
        }
        if (document.getElementById("ctl00_ContentPlaceHolder1_txt_Qty").value == '') {
            alert("Enter the quantity")
            document.getElementById("ctl00_ContentPlaceHolder1_txt_Qty").focus()
            return false
        }
        return true;
    }
    function validatefrm() {
  
        if (document.getElementById("ctl00_ContentPlaceHolder1_ddl_org").value == '0') {
            alert("Select Organization")
            document.getElementById("ctl00_ContentPlaceHolder1_ddl_org").focus()
            return false
        }
        if (document.getElementById("ctl00_ContentPlaceHolder1_GENERATESTOCK").value == "Y") {
            if (document.getElementById("ctl00_ContentPlaceHolder1_ddl_Agency").value == '0') {
                alert("Select Agency")
                document.getElementById("ctl00_ContentPlaceHolder1_ddl_Agency").focus()
                return false
            }
        }
        if (parseInt(document.getElementById("ctl00_ContentPlaceHolder1_RowCount").value) < 0)
            return false
        
        return true;
    }

    function enablebtn() {
       document.getElementById("ctl00_ContentPlaceHolder1_HClicked").value = "0"
       document.getElementById("ctl00_ContentPlaceHolder1_Btn_Confirm").disabled = false
       }
    
    function showbtn() {
       
       // document.getElementById("ctl00_ContentPlaceHolder1_Btn_Confirm").disabled = false
    }
    function hidebtn() {

        
       // document.getElementById("ctl00_ContentPlaceHolder1_Btn_Confirm").disabled = true 
    }
   
</script>
    <!--script src="../scripts/jquery-1.3.2.min.js" type="text/javascript"></script-->
    <script src="../dojo/dojo.js"  data-dojo-config="async: true"></script> 
    <script>
        /*  window.onbeforeunload = function(e) {

            var i
        i = document.getElementById("ctl00_ContentPlaceHolder1_ReqAlert").value

          
        if (i == '1' ) {

                if (e) {
        e.returnValue = 'Please make sure that you have confirmed the stock requisition for all agencies';
        }




                return 'Please make sure that you have confirmed the stock requisition for all agencies';
        }

        };*/
</script>


<style>
   .CellBtns img, .Btns img, .CBtns img{margin-left:5px; cursor:pointer;}
   .divbtnblk{position:absolute; top:5px; right:0;}
   #ctl00_ContentPlaceHolder1_TopPanel{width:1200px;}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >

    <script language="javascript" type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();

        prm.add_initializeRequest(InitializeRequest);
        prm.add_pageLoaded(pageLoaded);
        prm.add_endRequest(EndRequest);
        var postBackElement;

        function pageLoaded(sender, args) {


            var colstart
            colstart = 2

            var dojoConfig = {
                async: true,
                baseUrl: '.',
                packages: [
                    'dojo',
                    'dijit',
                    'dojox'
                ]
            };

            require(["dojo", "dojo/dom-style", "dojo/dom-construct", "dojo/dom", "dojo/on", "dojo/query", "dojo/_base/lang", "dojo/request", "dojo/domReady!"], function(dojo,domstyle, domConstruct, dom, on, query, lang, request) {
                // The piece we had before...


                var SelectCol = {
                    id: "SelectCol",
                    onClick: function(evt) {

                    if (dom.byId("ctl00_ContentPlaceHolder1_HClicked").value == "0") {
                       
                            dom.byId("ctl00_ContentPlaceHolder1_Btn_Confirm").disabled = true
                            
                            var pid = this.id
                            var indx = pid.split("_");
                            for (i = 0; i <= parseInt(dom.byId("ctl00_ContentPlaceHolder1_RowCount").value); i = i + 1) {
                                var cid = "Qty_" + i + "_" + indx[1]
                                var val = dom.byId(cid).textContent
                                dom.byId(cid).textContent = ''
                                domConstruct.place("<input  Type='text' id='txt_" + i + "_" + indx[1] + "' value='" + val + "' onKeypress='return NumericOnly(event)'></input>", cid)
                                domConstruct.place("<input  Type='hidden' id='hqty_" + i + "_" + indx[1] + "' value='" + val + "' onKeypress='return NumericOnly()'></input>", cid)
                            }

                            domstyle.set("CBtns", { "display": "block" })
                            dojo.setAttr("AllCBtnsCancel", { id: "AllCBtnsCancel_" + indx[1] })
                            dojo.setAttr("AllCBtnsSave", { id: "AllCBtnsSave_" + indx[1] })
                            
                            dom.byId("ctl00_ContentPlaceHolder1_HClicked").value = "1"

                        }
                    }
                };

                var SelectRow = {
                    id: "SelectRow",
                    onClick: function(evt) {

                    if (dom.byId("ctl00_ContentPlaceHolder1_HClicked").value == "0") {
                        
                            dom.byId("ctl00_ContentPlaceHolder1_Btn_Confirm").disabled = true
                            var pid = this.id
                            var indx = pid.split("_");

                            var ids = query(".Qty_" + indx[1] + "> *")
                            var i

                            for (i = colstart; i < ids.length - 1; i = i + 1) {
                            var val
                            val = dom.byId(ids[i]).textContent;
                            dom.byId(ids[i]).textContent = ''

                            domConstruct.place("<input  Type='text' id='txt_" + indx[1] + "_" + i + "' value='" + val + "' onKeypress='return NumericOnly(event)'></input>", ids[i])
                                domConstruct.place("<input  Type='hidden' id='hqty_" + indx[1] + "_" + i + "' value='" + val + "' onKeypress='return NumericOnly()'></input>", ids[i])

                            }
                            
                            domstyle.set("RBtns", { "display": "block" })
                            dojo.setAttr("AllRBtnsCancel", { id: "AllRBtnsCancel_" + indx[1] })
                            dojo.setAttr("AllRBtnsSave", { id: "AllRBtnsSave_" + indx[1] })
                            dom.byId("ctl00_ContentPlaceHolder1_HClicked").value = "1"

                        }
                    }
                };

                var SaveRow = {
                    id: "SaveRow",
                    onClick: function(evt) {
                    
                        dom.byId("ctl00_ContentPlaceHolder1_ReqAlert").value = "1"
                        var pid = this.id
                        var idata = ''
                        
                        var orgid
                        orgid = dom.byId("ctl00_ContentPlaceHolder1_ddl_org").value
                        
//                        var agency
//                        agency = dom.byId("ctl00_ContentPlaceHolder1_ddl_Agency").value

                        var OrgID
                        OrgID = dom.byId("ctl00_ContentPlaceHolder1_ddl_org").value

                        var indx = pid.split("_");
                        var ids = query(".Qty_" + indx[1] + "> *")
                        var i


                        for (i = colstart; i <= ids.length - colstart; i = i + 1) {


                            var val = dom.byId("txt_" + indx[1] + "_" + i).value


                            idata = idata + '{"SalesMan":"' + dom.byId("SM_" + (i - colstart)).textContent + '","Product":"' + dom.byId("HPR_" + indx[1]).value + '","Qty":"' + val + '","Org_ID":"'+orgid +'"},'

                            dom.byId(ids[i]).textContent = val
                        }



                        idata = idata.substring(0, idata.length - 1)


                        $.ajax({
                            type: "POST",
                            url: "StockRequisition.aspx/SaveStock",
                            data: '{"Ostock":[' + idata + ']}',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function(response) {
                                //__doPostBack('<%=ReloadThePanel.ClientID %>', null);
                                document.getElementById("ctl00_ContentPlaceHolder1_ReloadThePanel").click()
                            },
                            failure: function(response) {
                                //__doPostBack('<%=ReloadThePanel.ClientID %>', null);
                                document.getElementById("ctl00_ContentPlaceHolder1_ReloadThePanel").click()
                            }
                        });

                        
                        domstyle.set("RBtns", { "display": "none" })
                        dojo.setAttr("AllRBtnsCancel_" + indx[1], { id: "AllRBtnsCancel" })
                        dojo.setAttr("AllRBtnsSave_" + indx[1], { id:  "AllRBtnsSave"})
                        dom.byId("ctl00_ContentPlaceHolder1_HClicked").value = "0"
                        dom.byId("ctl00_ContentPlaceHolder1_Btn_Confirm").disabled = false
                       
                    }
                };


                var CancelRow = {
                    id: "CancelRow",
                    onClick: function(evt) {
                        var pid = this.id

                        var indx = pid.split("_");
                        var ids = query(".Qty_" + indx[1] + "> *")
                        var i

                        for (i = colstart; i <= ids.length - colstart; i = i + 1) {

                            var val = dom.byId("hqty_" + indx[1] + "_" + i).value

                            dom.byId(ids[i]).textContent = val
                           
                                                        
                            domConstruct.destroy("txt_" + indx[1] + "_" + i)


                        }

                        domstyle.set("RBtns", { "display": "none" })
                        dojo.setAttr("AllRBtnsCancel_" + indx[1], { id: "AllRBtnsCancel" })
                        dojo.setAttr("AllRBtnsSave_" + indx[1], { id: "AllRBtnsSave" })
                        dom.byId("ctl00_ContentPlaceHolder1_HClicked").value = "0"
                        dom.byId("ctl00_ContentPlaceHolder1_Btn_Confirm").disabled = false
                        
                    }
                };


                var CancelCol = {
                    id: "CancelCol",
                    onClick: function(evt) {
                        var pid = this.id

                        var indx = pid.split("_");

                        for (i = 0; i <= parseInt(dom.byId("ctl00_ContentPlaceHolder1_RowCount").value); i = i + 1) {

                            var val = dom.byId("hqty_" + i + "_" + indx[1]).value

                            dom.byId("Qty_" + i + "_" + indx[1]).textContent = val
                            domConstruct.destroy("txt_" + i + "_" + indx[1])


                        }
                        domstyle.set("CBtns", { "display": "none" })
                        dojo.setAttr("AllCBtnsCancel_" + indx[1], { id: "AllCBtnsCancel" })
                        dojo.setAttr("AllCBtnsSave_" + indx[1], { id: "AllCBtnsSave" })
                       
                        dom.byId("ctl00_ContentPlaceHolder1_HClicked").value = "0"
                        dom.byId("ctl00_ContentPlaceHolder1_Btn_Confirm").disabled = false
                        
                    }
                };


                var SaveCol = {
                    id: "SaveCol",
                    onClick: function(evt) {
                        dom.byId("ctl00_ContentPlaceHolder1_ReqAlert").value = "1"
                        var pid = this.id

                        var idata = ''

                        var orgid
                        orgid = dom.byId("ctl00_ContentPlaceHolder1_ddl_org").value
                        
                        
//                        var agency
//                        agency = dom.byId("ctl00_ContentPlaceHolder1_ddl_Agency").value

                        var OrgID
                        OrgID = dom.byId("ctl00_ContentPlaceHolder1_ddl_org").value

                        var indx = pid.split("_");
                        var ids = query(".Qty_" + indx[1] + "> *")
                        var i

                        for (i = 0; i <= parseInt(dom.byId("ctl00_ContentPlaceHolder1_RowCount").value); i = i + 1) {

                            var val = dom.byId("txt_" + i + "_" + indx[1]).value

                            idata = idata + '{"SalesMan":"' + dom.byId("SM_" + indx[1]).textContent + '","Product":"' + dom.byId("HPR_" + i).value + '","Qty":"' + val + '","Org_ID":"'+orgid +'"},'

                            dom.byId("Qty_" + i + "_" + indx[1]).textContent = val

                        }

                        idata = idata.substring(0, idata.length - 1)

                        $.ajax({
                            type: "POST",
                            url: "StockRequisition.aspx/SaveStock",
                            data: '{"Ostock":[' + idata + ']}',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function(response) {
                                //location.href = "StockRequisition.aspx?Agency=" + agency + "&OrgID=" + OrgID
                                //__doPostBack('<%=ReloadThePanel.ClientID %>', null);
                                document.getElementById("ctl00_ContentPlaceHolder1_ReloadThePanel").click()

                            },
                            failure: function(response) {
                                // location.href = "StockRequisition.aspx?Agency=" + agency + "&OrgID=" + OrgID
                                //__doPostBack('<%=ReloadThePanel.ClientID %>', null);
                                document.getElementById("ctl00_ContentPlaceHolder1_ReloadThePanel").click()
                            }
                        });
                        
                        domstyle.set("CBtns", { "display": "none" })
                        dojo.setAttr("AllCBtnsCancel_" + indx[1], { id: "AllCBtnsCancel" })
                        dojo.setAttr("AllCBtnsSave_" + indx[1], { id: "AllCBtnsSave" })
                      
                        dom.byId("ctl00_ContentPlaceHolder1_HClicked").value = "0"
                        dom.byId("ctl00_ContentPlaceHolder1_Btn_Confirm").disabled = false
                        
                    }
                };


                var SelectCell = {
                    id: "SelectCell",
                    onClick: function(evt) {
                       
                        if (dom.byId("ctl00_ContentPlaceHolder1_HClicked").value == "0") {
                            dom.byId("ctl00_ContentPlaceHolder1_Btn_Confirm").disabled = true
                            


                            var pid = this.id
                            dom.byId("ctl00_ContentPlaceHolder1_CellClicked").value = pid
                            var indx = pid.split("_");
                            var i
                            i = indx[2]


                            var cid = "Qty_" + indx[1] + "_" + i
                            var val = dom.byId(cid).textContent
                            dom.byId(cid).textContent = ''
                            
                            domConstruct.place("<input  Type='text' id='txt_" + i + "_" + indx[1] + "' value='" + val + "' onKeypress='return NumericOnly(event)'></input>", cid)
                            domConstruct.place("<input  Type='hidden' id='hqty_" + i + "_" + indx[1] + "' value='" + val + "' onKeypress='return NumericOnly()'></input>", cid)


                           

                            domstyle.set("CellIBtns", { "display": "block" })
                            /*domstyle.set("CellIBtns", { "position": "absolute" })
                            domstyle.set("CellIBtns", { "left": parseInt(posx) + 40 + "px", "top": parseInt(posy) - 12 + "px" });*/
                            dom.byId("ctl00_ContentPlaceHolder1_HClicked").value = "1"

                        }
                    }
                };


                var CancelCell = {
                    id: "CancelCell",
                    onClick: function(evt) {
                        var pid = dom.byId("ctl00_ContentPlaceHolder1_CellClicked").value

                        var indx = pid.split("_");
                        var ids = query(".Qty_" + indx[1] + "> *")
                        var i


                        i = indx[2]

                        var val = dom.byId("hqty_" + i + "_" + indx[1]).value
                        dom.byId(pid).textContent = val

                        domConstruct.destroy("txt_" + i + "_" + indx[1])

                        domstyle.set("CellIBtns", { "display": "none" })

                        dom.byId("ctl00_ContentPlaceHolder1_HClicked").value = "0"
                        dom.byId("ctl00_ContentPlaceHolder1_Btn_Confirm").disabled = false
                        dom.byId("ctl00_ContentPlaceHolder1_CellClicked").value = ""
                        
                    }
                };


                var SaveCell = {
                    id: "SaveCell",
                    onClick: function(evt) {
                        dom.byId("ctl00_ContentPlaceHolder1_ReqAlert").value = "1"
                        var pid = dom.byId("ctl00_ContentPlaceHolder1_CellClicked").value

                        var idata = ''
                        var orgid
                        orgid = dom.byId("ctl00_ContentPlaceHolder1_ddl_org").value
                        
//                        var agency
//                        agency = dom.byId("ctl00_ContentPlaceHolder1_ddl_Agency").value

                        var OrgID
                        OrgID = dom.byId("ctl00_ContentPlaceHolder1_ddl_org").value

                        var indx = pid.split("_");
                        var ids = query(".Qty_" + indx[1] + "> *")
                        var i

                        i = indx[2]
                        var val = dom.byId("txt_" + i + "_" + indx[1]).value

                        idata = idata + '{"SalesMan":"' + dom.byId("SM_" + i).textContent + '","Product":"' + dom.byId("HPR_" + indx[1]).value + '","Qty":"' + val + '","Org_ID":"'+orgid +'"},'

                        dom.byId("Qty_" + indx[1] + "_" + i).textContent = val



                        idata = idata.substring(0, idata.length - 1)


                        $.ajax({
                            type: "POST",
                            url: "StockRequisition.aspx/SaveStock",
                            data: '{"Ostock":[' + idata + ']}',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function(response) {
                                //location.href = "StockRequisition.aspx?Agency=" + agency + "&OrgID=" + OrgID
                                //__doPostBack('<%=ReloadThePanel.ClientID %>', null);
                                document.getElementById("ctl00_ContentPlaceHolder1_ReloadThePanel").click()

                            },
                            failure: function(response) {
                                // location.href = "StockRequisition.aspx?Agency=" + agency + "&OrgID=" + OrgID
                                //__doPostBack('<%=ReloadThePanel.ClientID %>', null);
                                document.getElementById("ctl00_ContentPlaceHolder1_ReloadThePanel").click()
                            }
                        });

                        domstyle.set("CellIBtns", { "display": "none" })
                        dom.byId("ctl00_ContentPlaceHolder1_HClicked").value = "0"
                        dom.byId("ctl00_ContentPlaceHolder1_Btn_Confirm").disabled = false
                       
                    }
                };


                query(".Qty").on("click", SelectCell.onClick);
                query(".SM").on("click", SelectCol.onClick);
                query(".ProdHdr").on("click", SelectRow.onClick);
                query(".AllRBtnsSave").on("click", SaveRow.onClick);
                query(".AllRBtnsCancel").on("click", CancelRow.onClick);
                query(".AllCBtnsCancel").on("click", CancelCol.onClick);
                query(".AllCBtnsSave").on("click", SaveCol.onClick);

                query(".CellBtns_Cancel").on("click", CancelCell.onClick);
                query(".CellBtns_Save").on("click", SaveCell.onClick);

                query(".CellBtns_Cancel").on("click", CancelCell.onClick);
                query(".CellBtns_Save").on("click", SaveCell.onClick);

            });

            
            
        }

        function InitializeRequest(sender, args) {
            $get('<%= Me.UpdateProgress1.ClientID %>').style.display = 'block';
            if (prm.get_isInAsyncPostBack())

                args.set_cancel(true);
            postBackElement = args.get_postBackElement();
            postBackElement.disabled = true;

        }
        function EndRequest(sender, args) {
            postBackElement.disabled = false;
            $get('<%= Me.UpdateProgress1.ClientID %>').style.display = 'none';
        }

        function NumericOnly(e) {
          
           var keycode;
          
           if (window.event) {
               keycode = window.event.keyCode;
           } else if (e) {
               keycode = e.which;
           }
        if ((parseInt(keycode) >= 48 && parseInt(keycode) <= 57) || parseInt(keycode) == 8 || parseInt(keycode) == 0)
               return true;
                
            return false;
        }
</script>
<script type="text/javascript">
    var TargetBaseControl = null;

    window.onload = function() {
        try {
            TargetBaseControl =
           document.getElementById('<%= Me.UpdatePanel1.ClientID %>');
        }
        catch (err) {
            TargetBaseControl = null;
        }
    }
       

       
    </script>
    
    
<table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                
        <tr>
            <td id="contentofpage" width="100%" height="76%" class="topshadow">
                <div class="pgtitileposition">
                    <span class="pgtitile3">Stock Requisition</span></div>
                    
                    
                    <table id="tableformNrml"  width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                    <td >
                    <div style="position:relative;">
                    <div style ="float:right;margin-right:100px;"> <asp:Button ID="Btn_Confirm" runat="server" Text="Confirm" CssClass="btnInputBlue" />
                   
                    
                    <asp:Button ID="Button1" runat="server" Text="Add New" CssClass="btnInputGreen"  />
                                        </div>
                                        
                    <asp:UpdatePanel ID="TopPanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
              <table >
         <tr>
                        <td style="padding: 6px 12px">
                        
                         <table  >
                         <tr><td colspan="3">
                        <asp:Label ID="lbl_Msg" runat="server" Text="" ForeColor="Red"></asp:Label> </td></tr>
                
    <tr><td class="txtSMBold" >Organization:</td><td>
        <asp:DropDownList ID="ddl_org" CssClass ="inputSM" TabIndex ="1" Width ="200px" runat="server" AutoPostBack="true">
        </asp:DropDownList></td>
    <td>
        </td>
    </tr>
    <tr><td class="txtSMBold" ><asp:Label ID="lbl_Agency" Text="Agency:" runat="server"></asp:Label></td><td>
        <asp:DropDownList ID="ddl_Agency" CssClass ="inputSM" TabIndex ="2" Width ="200px" runat="server" AutoPostBack="true">
        </asp:DropDownList>
         
    </td>
       
    </tr>
  
    </table>
      
                      </td>
    </tr>
    </table>
    
     <div style="padding: 5px 0 0 0; background:#fff;">
     <div style='display:none; z-index:9; text-align:right; padding-bottom:5px;' class='CellBtns' id='CellIBtns'><img src='../images/tick.jpg' class='CellBtns_Save' id='CellIBtns_Save'/><img src='../images/Cancel.jpg' class='CellBtns_Cancel' id='CellIBtns_Cancel'/></div>
     <div style='display:none; z-index:15; text-align:right; padding-bottom:5px;' class='Btns' id='RBtns'><img src='../images/tick.jpg' class='AllRBtnsSave' id='AllRBtnsSave'/><img src='../images/Cancel.jpg' class='AllRBtnsCancel'  id='AllRBtnsCancel'/></div>
     <div style='display:none; z-index:20; text-align:right; padding-bottom:5px;' class='CBtns' id='CBtns'><img src='../images/tick.jpg' class='AllCBtnsSave' id='AllCBtnsSave' /><img src='../images/Cancel.jpg' class='AllCBtnsCancel' id='AllCBtnsCancel' /></div>
     <asp:Panel ID="Pnl_Content" runat="server" Visible="false">
           <asp:Label ID="Label2" runat="server" Text=""></asp:Label>
        </asp:Panel>
        
     </div>
    <asp:HiddenField ID="btnVisible" runat="server"  value="0"  />
     <asp:HiddenField ID="RowCount" runat="server"  value="-1"  />
     <asp:HiddenField ID="StockCheck" runat="server"  value="0"  />
     <asp:HiddenField ID="CellClicked" runat="server"  value="0"  />
     <asp:HiddenField ID="ReqAlert" runat="server"  value="0"  />
      <asp:Button ID="ReloadThePanel" runat="server" style="display:none;"   OnClick="LoadStockSheets" />
      <asp:Label ID="lblJavaScript" runat="server" Text=""></asp:Label> 
       <asp:HiddenField ID="GENERATESTOCK" runat="server"  value=""  />       
        
   
    
       <table>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btn_Addnew" runat="Server" Style="display: none" />
                                                 <ajaxToolkit:ModalPopupExtender ID="MpAddnew" runat="server" BackgroundCssClass="modalBackground"
                                                    TargetControlID="btn_Addnew" PopupControlID="pnl_Addnew" CancelControlID="Btn_cancelAdd"
                                                    Drag="true" />
                                                <asp:Panel ID="pnl_Addnew" Width="800" runat="server" CssClass="modalPopup" Style="display: none">
                                                    <table id="table1" width="100%" cellpadding="10" style="background-color: White;" border="1">
                                                       
                                                 <tr><td colspan="2">
                                                <asp:Label ID="lbl_Result" runat="server" Text="" ForeColor="Red"></asp:Label> </td></tr>
                                            <tr>   
                                     <tr><td class="txtSMBold" >Organization:</td><td>
                                <asp:DropDownList ID="ddlOrgNew" CssClass ="inputSM" TabIndex ="1" Width ="200px" runat="server" AutoPostBack="true">
                                </asp:DropDownList></td><td class="txtSMBold" >Sales Man:</td><td>
                                <asp:DropDownList ID="ddl_Van" CssClass ="inputSM" TabIndex ="2" Width ="200px" runat="server" AutoPostBack="true">
                                </asp:DropDownList>
                                
                            </td>
                            
                            </tr>
                           <asp:Panel ID="Pnl_Agency" runat="server">
                            <tr>
                            <td class="txtSMBold" >Agency:</td><td colspan="3" >
                                <asp:DropDownList ID="ddlAgencyNew" CssClass ="inputSM" TabIndex ="2" Width ="200px" runat="server" AutoPostBack="true">
                                </asp:DropDownList>
                                
                            </td>
                            </tr>
                            </asp:Panel> 
                            <tr>
                               <td class="txtSMBold" >SKU:</td><td colspan="3" >
                                <asp:DropDownList ID="ddlSKU" CssClass ="inputSM" TabIndex ="2" Width ="200px" runat="server" AutoPostBack="true">
                                </asp:DropDownList>
                                 
                            </td>
                            </tr>
                            
                           
                            <tr><td class="txtSMBold" >Qty:</td><td>
                                <asp:TextBox ID="txt_Qty" CssClass ="inputSM" runat="server" onKeypress='return NumericOnly(event)'></asp:TextBox>
                                
                            </td>
                               <td></td>
                            </tr>
                            <tr>
                                                            <td align="center" style="text-align: center;"> 
                                                            </td>
                                                          <td><div class ="inputSM"><asp:Button ID="btn_Savenew" CssClass="btnInputGreen" Text="Save" runat="server" />
                                                           <asp:Button ID="Btn_cancelAdd" CssClass="btnInputRed" runat="server" Text="Close" /></div></td> 
                                           <td></td>              
                                                        </tr>
  
    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
    
   
    </ContentTemplate>
                            </asp:UpdatePanel>
                           
                           
                           </div>
                            </td> 
                             
                    </tr>
                    </table>
                     
                        
                           
                            <asp:HiddenField ID="HClicked" runat="server"  value="0"  />
     </td>
     
     </tr>
     
         
     <td ><table>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnHidden" runat="Server" Style="display: none" />
                                                 <ajaxToolkit:ModalPopupExtender ID="MpInfoError" runat="server" BackgroundCssClass="modalBackground"
                                                    TargetControlID="btnHidden" PopupControlID="pnlConfirm" 
                                                    Drag="true" />
                                                <asp:Panel ID="pnlConfirm" Width="400" runat="server" CssClass="modalPopup" Style="display: none">
                                                    <table id="tableinPopupErr" width="400" cellpadding="10" style="background-color: White;">
                                                        <tr align="center">
                                                            <td align="center" style="cursor: move; background-color: #3399ff; text-align: center;
                                                                border: solid 1px #3399ff; color: White; padding: 3px">
                                                                  <asp:Label ID="lblinfo" Text="Availabe stock is less than the requested Quantity.<br/> Would you like to confirm ?"   runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" style="text-align: center">
                                                                <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="List" />
                                                              <asp:GridView ID="gv_InvalidStock" runat="server">
                                                                </asp:GridView> 
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" style="text-align: center;">
                                                            <asp:Button ID="btnUpdate" CssClass="btnInput" Text="Confirm" runat="server" />
                                                            <asp:Button ID="btnCancel" CssClass="btnInput" runat="server" Text="Cancel" />
                                                          </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table></td>
     </tr>
     
     </ContentTemplate>
                            </asp:UpdatePanel>
                            
                            <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="UpdatePanel1"
                    runat="server">
                    <ProgressTemplate>
                        <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                            <img src="../images/Progress.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                            <span style="font-size: 12px; color: #3399ff;">Processing... </span>
                        </asp:Panel>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                
     </table>
     <link rel='stylesheet' href='../styles/component.css' />

</asp:Content>
