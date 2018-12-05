<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="StockRequisition_ByVan.aspx.vb" Inherits="SalesWorx_BO.StockRequisition_ByVan" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>


<asp:Content ID="Content1" ContentPlaceHolderID="FeaturedContent" runat="server">
    <style>
        .modalBackground {
            z-index: 6005 !important;
        }
        .modalPopup {
            z-index: 6006 !important;
        }
        .rcbSlide {
            z-index: 6007 !important;
        }
            .rcbSlide .RadComboBoxDropDown {
                max-height:250px !important;
                overflow:auto;
            }
        .t-edit-icon {
            display:block;
            position:relative;
            cursor:pointer;
        }
        .t-edit-icon:before {
            position: absolute;
            width: 11px;
            height: 11px;
            content: "";
            background: url(../images/edit-13.png) no-repeat center center;
            right: -9px;
            top: 5px;
            background-size: 11px;
        }
        .sticky-wrap th {
            white-space: normal !important;
        }
        .sticky-intersect thead tr th {
            display:none;
        }
        .sticky-wrap thead tr th {
            font-weight: bold !important;
            font-size: 12px;
        }
        .sticky-wrap tbody tr td,
        .sticky-wrap tbody tr td.Qty input{
            text-align: right;
        }
    </style>
<script src='../js/jquery.ba-throttle-debounce.min.js'></script>
<script src='../js/jquery.stickyheader.js'></script>
<link rel='stylesheet' href='../styles/component.css' />
<script type="text/javascript">

    function clickExportPDF() {
        $("#MainContent_BtnExportPDF").click()
        return false
    }
    function showDialogue() {
        createtbl()
    }

    function alertCallBackFn(arg) {

    }

    function validate() {
        if (document.getElementById("MainContent_ddl_Van").value == '0') {
            alert("Select Van/FSR")
            document.getElementById("MainContent_ddl_Van").focus()
            return false
        }
        if (document.getElementById("MainContent_ddlSKU").value == '0') {
            alert("Select Product")
            document.getElementById("MainContent_ddlSKU").focus()
            return false
        }
        if (document.getElementById("MainContent_txt_Qty").value == '') {
            alert("Enter the quantity")
            document.getElementById("MainContent_txt_Qty").focus()
            return false
        }
        return true;
    }
     

    function enablebtn() {
        document.getElementById("MainContent_HClicked").value = "0"
        document.getElementById("MainContent_Btn_Confirm").disabled = false
    }

    function showbtn() {

        // document.getElementById("MainContent_Btn_Confirm").disabled = false
    }
    function hidebtn() {


        // document.getElementById("MainContent_Btn_Confirm").disabled = true 
    }

</script>
    <!--script src="../scripts/jquery-1.3.2.min.js" type="text/javascript"></script-->
    <script src="../dojo/dojo.js"  data-dojo-config="async: true"></script> 
    <script>
        /*  window.onbeforeunload = function(e) {

            var i
        i = document.getElementById("MainContent_ReqAlert").value

          
        if (i == '1' ) {

                if (e) {
        e.returnValue = 'Please make sure that you have confirmed the stock requisition for all agencies';
        }




                return 'Please make sure that you have confirmed the stock requisition for all agencies';
        }

        };*/
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server" >

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

            require(["dojo", "dojo/dom-style", "dojo/dom-construct", "dojo/dom", "dojo/on", "dojo/query", "dojo/_base/lang", "dojo/request", "dojo/domReady!"], function (dojo, domstyle, domConstruct, dom, on, query, lang, request) {
                // The piece we had before...


                var SelectCol = {
                    id: "SelectCol",
                    onClick: function (evt) {

                        if (dom.byId("MainContent_HClicked").value == "0") {

                            dom.byId("MainContent_Btn_Confirm").disabled = true
                            dom.byId("MainContent_SearchBtn").disabled = true
                            dom.byId("MainContent_ClearBtn").disabled = true
                            dom.byId("MainContent_Button1").disabled = true
                            dom.byId("MainContent_Btn_Print").disabled = true
                            dom.byId("MainContent_Btn_Print").disabled = true
                            var comboOrgCombo = $find("<%= ddl_org.ClientID%>");
                            comboOrgCombo.disable();

                            var comboVanCombo = $find("<%= ddlVan.ClientID%>");
                            comboVanCombo.disable();

                            var pid = this.id
                            var indx = pid.split("_");
                            for (i = 0; i <= parseInt(dom.byId("MainContent_RowCount").value) ; i = i + 1) {
                                var cid = "Qty_" + i + "_" + indx[1]
                                var val = dom.byId(cid).textContent
                                dom.byId(cid).textContent = ''
                                domConstruct.place("<input  Type='text' id='txt_" + i + "_" + indx[1] + "' value='" + val + "' onKeypress='return NumericOnly(event)'></input>", cid)
                                domConstruct.place("<input  Type='hidden' id='hqty_" + i + "_" + indx[1] + "' value='" + val + "' onKeypress='return NumericOnly()'></input>", cid)
                            }

                            domstyle.set("CBtns", { "display": "block" })
                            dojo.setAttr("AllCBtnsCancel", { id: "AllCBtnsCancel_" + indx[1] })
                            dojo.setAttr("AllCBtnsSave", { id: "AllCBtnsSave_" + indx[1] })

                            dom.byId("MainContent_HClicked").value = "1"

                        }
                    }
                };

                var SelectRow = {
                    id: "SelectRow",
                    onClick: function (evt) {

                        if (dom.byId("MainContent_HClicked").value == "0") {

                            
                            dom.byId("MainContent_Btn_Confirm").disabled = true
                            dom.byId("MainContent_SearchBtn").disabled = true
                            dom.byId("MainContent_ClearBtn").disabled = true
                            dom.byId("MainContent_Button1").disabled = true
                            dom.byId("MainContent_Btn_Print").disabled = true
                            dom.byId("MainContent_Btn_Print").disabled = true
                            var comboOrgCombo = $find("<%= ddl_org.ClientID%>");
                            comboOrgCombo.disable();

                            var comboVanCombo = $find("<%= ddlVan.ClientID%>");
                            comboVanCombo.disable();

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
                            dom.byId("MainContent_HClicked").value = "1"

                        }
                    }
                };

                var SaveRow = {
                    id: "SaveRow",
                    onClick: function (evt) {

                        dom.byId("MainContent_ReqAlert").value = "1"
                        var pid = this.id
                        var idata = ''

                        var orgid
                        orgid = dom.byId("MainContent_H_Org_ID").value

                        //                        var agency
                        //                        agency = dom.byId("MainContent_ddl_Agency").value

                        var OrgID
                        OrgID = dom.byId("MainContent_H_Org_ID").value

                        var indx = pid.split("_");
                        var ids = query(".Qty_" + indx[1] + "> *")
                        var i


                        for (i = colstart; i <= ids.length - colstart; i = i + 1) {


                            var val = dom.byId("txt_" + indx[1] + "_" + i).value


                            idata = idata + '{"SalesMan":"' + dom.byId("SM_" + (i - colstart)).textContent + '","Product":"' + dom.byId("HPR_" + indx[1]).value + '","Qty":"' + val + '","Org_ID":"' + orgid + '"},'

                            dom.byId(ids[i]).textContent = val
                        }



                        idata = idata.substring(0, idata.length - 1)


                        $.ajax({
                            type: "POST",
                            url: "StockRequisition_ByVan.aspx/SaveStock",
                            data: '{"Ostock":[' + idata + ']}',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (response) {
                                //__doPostBack('<%=ReloadThePanel.ClientID %>', null);
                                document.getElementById("MainContent_ReloadThePanel").click()
                            },
                            failure: function (response) {
                                //__doPostBack('<%=ReloadThePanel.ClientID %>', null);
                                document.getElementById("MainContent_ReloadThePanel").click()
                            }
                        });


                        domstyle.set("RBtns", { "display": "none" })
                        dojo.setAttr("AllRBtnsCancel_" + indx[1], { id: "AllRBtnsCancel" })
                        dojo.setAttr("AllRBtnsSave_" + indx[1], { id: "AllRBtnsSave" })
                        dom.byId("MainContent_HClicked").value = "0"
                        dom.byId("MainContent_Btn_Confirm").disabled = false
                        dom.byId("MainContent_SearchBtn").disabled = false
                        dom.byId("MainContent_ClearBtn").disabled = false
                        dom.byId("MainContent_Button1").disabled = false
                        dom.byId("MainContent_Btn_Print").disabled = false
                        dom.byId("MainContent_Btn_Print").disabled = false
                        var comboOrgCombo = $find("<%= ddl_org.ClientID%>");
                        comboOrgCombo.enable();

                            var comboVanCombo = $find("<%= ddlVan.ClientID%>");
                        comboVanCombo.enable();

                    }
                };


                var CancelRow = {
                    id: "CancelRow",
                    onClick: function (evt) {
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
                        dom.byId("MainContent_HClicked").value = "0"
                        dom.byId("MainContent_Btn_Confirm").disabled = false
                        dom.byId("MainContent_SearchBtn").disabled = false
                        dom.byId("MainContent_ClearBtn").disabled = false
                        dom.byId("MainContent_Button1").disabled = false
                        dom.byId("MainContent_Btn_Print").disabled = false
                        dom.byId("MainContent_Btn_Print").disabled = false
                        var comboOrgCombo = $find("<%= ddl_org.ClientID%>");
                        comboOrgCombo.enable();

                        var comboVanCombo = $find("<%= ddlVan.ClientID%>");
                        comboVanCombo.enable();

                    }
                };


                var CancelCol = {
                    id: "CancelCol",
                    onClick: function (evt) {
                        var pid = this.id

                        var indx = pid.split("_");

                        for (i = 0; i <= parseInt(dom.byId("MainContent_RowCount").value) ; i = i + 1) {

                            var val = dom.byId("hqty_" + i + "_" + indx[1]).value

                            dom.byId("Qty_" + i + "_" + indx[1]).textContent = val
                            domConstruct.destroy("txt_" + i + "_" + indx[1])


                        }
                        domstyle.set("CBtns", { "display": "none" })
                        dojo.setAttr("AllCBtnsCancel_" + indx[1], { id: "AllCBtnsCancel" })
                        dojo.setAttr("AllCBtnsSave_" + indx[1], { id: "AllCBtnsSave" })

                        dom.byId("MainContent_HClicked").value = "0"
                        dom.byId("MainContent_Btn_Confirm").disabled = false
                        dom.byId("MainContent_SearchBtn").disabled = false
                        dom.byId("MainContent_ClearBtn").disabled = false
                        dom.byId("MainContent_Button1").disabled = false
                        dom.byId("MainContent_Btn_Print").disabled = false
                        dom.byId("MainContent_Btn_Print").disabled = false
                        var comboOrgCombo = $find("<%= ddl_org.ClientID%>");
                        comboOrgCombo.enable();

                        var comboVanCombo = $find("<%= ddlVan.ClientID%>");
                        comboVanCombo.enable();

                    }
                };


                var SaveCol = {
                    id: "SaveCol",
                    onClick: function (evt) {
                        dom.byId("MainContent_ReqAlert").value = "1"
                        var pid = this.id

                        var idata = ''

                        var orgid
                        orgid = dom.byId("MainContent_H_Org_ID").value


                        //                        var agency
                        //                        agency = dom.byId("MainContent_ddl_Agency").value

                        var OrgID
                        OrgID = dom.byId("MainContent_H_Org_ID").value

                        var indx = pid.split("_");
                        var ids = query(".Qty_" + indx[1] + "> *")
                        var i

                        for (i = 0; i <= parseInt(dom.byId("MainContent_RowCount").value) ; i = i + 1) {

                            var val = dom.byId("txt_" + i + "_" + indx[1]).value

                            idata = idata + '{"SalesMan":"' + dom.byId("SM_" + indx[1]).textContent + '","Product":"' + dom.byId("HPR_" + i).value + '","Qty":"' + val + '","Org_ID":"' + orgid + '"},'

                            dom.byId("Qty_" + i + "_" + indx[1]).textContent = val

                        }

                        idata = idata.substring(0, idata.length - 1)

                        $.ajax({
                            type: "POST",
                            url: "StockRequisition_ByVan.aspx/SaveStock",
                            data: '{"Ostock":[' + idata + ']}',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (response) {
                                //location.href = "StockRequisition.aspx?Agency=" + agency + "&OrgID=" + OrgID
                                //__doPostBack('<%=ReloadThePanel.ClientID %>', null);
                                document.getElementById("MainContent_ReloadThePanel").click()

                            },
                            failure: function (response) {
                                // location.href = "StockRequisition.aspx?Agency=" + agency + "&OrgID=" + OrgID
                                //__doPostBack('<%=ReloadThePanel.ClientID %>', null);
                                document.getElementById("MainContent_ReloadThePanel").click()
                            }
                        });

                        domstyle.set("CBtns", { "display": "none" })
                        dojo.setAttr("AllCBtnsCancel_" + indx[1], { id: "AllCBtnsCancel" })
                        dojo.setAttr("AllCBtnsSave_" + indx[1], { id: "AllCBtnsSave" })

                        dom.byId("MainContent_HClicked").value = "0"
                        dom.byId("MainContent_Btn_Confirm").disabled = false
                        dom.byId("MainContent_SearchBtn").disabled = false
                        dom.byId("MainContent_ClearBtn").disabled = false
                        dom.byId("MainContent_Button1").disabled = false
                        dom.byId("MainContent_Btn_Print").disabled = false
                        dom.byId("MainContent_Btn_Print").disabled = false
                        var comboOrgCombo = $find("<%= ddl_org.ClientID%>");
                        comboOrgCombo.enable();

                        var comboVanCombo = $find("<%= ddlVan.ClientID%>");
                        comboVanCombo.enable();

                    }
                };


                var SelectCell = {
                    id: "SelectCell",
                    onClick: function (evt) {
                       
                        if (dom.byId("MainContent_HClicked").value == "0") {
                            dom.byId("MainContent_Btn_Confirm").disabled = true



                            var pid = this.id
                            dom.byId("MainContent_CellClicked").value = pid
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
                            dom.byId("MainContent_HClicked").value = "1"
                            dom.byId("MainContent_Btn_Confirm").disabled = true 
                            dom.byId("MainContent_SearchBtn").disabled = true
                            dom.byId("MainContent_ClearBtn").disabled = true
                            dom.byId("MainContent_Button1").disabled = true
                            dom.byId("MainContent_Btn_Print").disabled = true
                            dom.byId("MainContent_Btn_Print").disabled = true
                            var comboOrgCombo = $find("<%= ddl_org.ClientID%>");
                            comboOrgCombo.disable();

                        var comboVanCombo = $find("<%= ddlVan.ClientID%>");
                            comboVanCombo.disable();
                        }
                    }
                };


                var CancelCell = {
                    id: "CancelCell",
                    onClick: function (evt) {
                        var pid = dom.byId("MainContent_CellClicked").value

                        var indx = pid.split("_");
                        var ids = query(".Qty_" + indx[1] + "> *")
                        var i


                        i = indx[2]

                        var val = dom.byId("hqty_" + i + "_" + indx[1]).value
                        dom.byId(pid).textContent = val

                        domConstruct.destroy("txt_" + i + "_" + indx[1])

                        domstyle.set("CellIBtns", { "display": "none" })

                        dom.byId("MainContent_HClicked").value = "0"
                        dom.byId("MainContent_Btn_Confirm").disabled = false
                        dom.byId("MainContent_CellClicked").value = ""
                        dom.byId("MainContent_Btn_Confirm").disabled = false
                        dom.byId("MainContent_SearchBtn").disabled = false
                        dom.byId("MainContent_ClearBtn").disabled = false
                        dom.byId("MainContent_Button1").disabled = false
                        dom.byId("MainContent_Btn_Print").disabled = false
                        dom.byId("MainContent_Btn_Print").disabled = false
                        var comboOrgCombo = $find("<%= ddl_org.ClientID%>");
                        comboOrgCombo.enable();

                        var comboVanCombo = $find("<%= ddlVan.ClientID%>");
                        comboVanCombo.enable();
                    }
                };


                var SaveCell = {
                    id: "SaveCell",
                    onClick: function (evt) {
                        dom.byId("MainContent_ReqAlert").value = "1"
                        var pid = dom.byId("MainContent_CellClicked").value

                        var idata = ''
                        var orgid
                       
                        orgid = dom.byId("MainContent_H_Org_ID").value
                        
                        //                        var agency
                        //                        agency = dom.byId("MainContent_ddl_Agency").value

                        var OrgID
                        OrgID = dom.byId("MainContent_H_Org_ID").value
                        
                        var indx = pid.split("_");
                        var ids = query(".Qty_" + indx[1] + "> *")
                        var i

                        i = indx[2]
                        var val = dom.byId("txt_" + i + "_" + indx[1]).value

                        idata = idata + '{"SalesMan":"' + dom.byId("SM_" + i).textContent + '","Product":"' + dom.byId("HPR_" + indx[1]).value + '","Qty":"' + val + '","Org_ID":"' + orgid + '"},'

                        dom.byId("Qty_" + indx[1] + "_" + i).textContent = val



                        idata = idata.substring(0, idata.length - 1)

                        
                        $.ajax({
                            type: "POST",
                            url: "StockRequisition_ByVan.aspx/SaveStock",
                            data: '{"Ostock":[' + idata + ']}',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (response) {
                                //location.href = "StockRequisition.aspx?Agency=" + agency + "&OrgID=" + OrgID
                                //__doPostBack('<%=ReloadThePanel.ClientID %>', null);
                  
                                document.getElementById("MainContent_ReloadThePanel").click()

                            },
                            failure: function (response) {
                                // location.href = "StockRequisition.aspx?Agency=" + agency + "&OrgID=" + OrgID
                                //__doPostBack('<%=ReloadThePanel.ClientID %>', null);
                  
                                document.getElementById("MainContent_ReloadThePanel").click()
                            }
                        });

                        domstyle.set("CellIBtns", { "display": "none" })
                        dom.byId("MainContent_HClicked").value = "0"
                        dom.byId("MainContent_Btn_Confirm").disabled = false
                        dom.byId("MainContent_SearchBtn").disabled = false
                        dom.byId("MainContent_ClearBtn").disabled = false
                        dom.byId("MainContent_Button1").disabled = false
                        dom.byId("MainContent_Btn_Print").disabled = false
                        dom.byId("MainContent_Btn_Print").disabled = false
                        var comboOrgCombo = $find("<%= ddl_org.ClientID%>");
                        comboOrgCombo.enable();

                        var comboVanCombo = $find("<%= ddlVan.ClientID%>");
                        comboVanCombo.enable();

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

    window.onload = function () {
        try {
            TargetBaseControl =
           document.getElementById('<%= Me.UpdatePanel1.ClientID %>');
        }
        catch (err) {
            TargetBaseControl = null;
        }
    }



    </script>
     <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>
    

<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                
        
               <h4>Unconfirmed Stock Requests</h4>
                    
                
                                        
                    <asp:UpdatePanel ID="TopPanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
               
                  <div class="row">                    
                                    <div class="col-sm-6 col-md-4 col-lg-3">
                                            <div class="form-group">
       <label>Organization</label>
       
                                                 <telerik:RadComboBox Skin="Simple" Filter="Contains" ID="ddl_org" Width="100%" runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="True">
                                                </telerik:RadComboBox>
                                                <asp:HiddenField ID="H_Org_ID" runat="server"></asp:HiddenField>
                                                <div style="color:#009933;"><asp:Label ID="lbl_Msg" runat="server" Text=""></asp:Label></div>
            </div>
                                        </div>
     <div class="col-sm-6 col-md-4 col-lg-3">
                                            <div class="form-group">
       <label><asp:Label ID="lbl_Agency" Text="Van/FSR" runat="server"></asp:Label></label>
         <telerik:RadComboBox Skin="Simple"  EmptyMessage="Select Van/FSR" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"  ID="ddlVan" Width ="100%" runat="server" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID" Filter="Contains">
                                        </telerik:RadComboBox >
            </div>
         </div>
                      <div class="col-sm-6 col-md-4 col-lg-2">
                          <div class="form-group">
                              <label class="hidden-xs hidden-sm"><br /></label>
         <asp:Button CssClass="btn btn-sm   btn-primary" ID="SearchBtn" runat="server" Text="Search" />
        <asp:Button CssClass="btn btn-sm  btn-default" ID="ClearBtn" runat="server" Text="Clear" />
    </div>
         </div>
                      <div class="col-sm-6 col-md-8 col-lg-4">
                          <div class="form-group">
                              <label class="hidden-xs hidden-sm hidden-md"><br /></label>
                          <asp:Button ID="Btn_Confirm" runat="server" Text="Confirm" CssClass="btn btn-success" />
                            <asp:Button ID="Button1" runat="server" Text="Add New" CssClass="btn btn-warning"  />
                              <asp:Button ID="Btn_Print" runat="server" Text="Print" CssClass="btn btn-danger" OnClientClick="return clickExportPDF()" />
                               </div>
                      </div>
                                    </div>
                                     
                        
                                      
      
                      
    
     <div class="form-group" style="position:relative;">
     <div style='display:none;' class='CellBtns' id='CellIBtns'><img src='../images/tick.jpg' class='CellBtns_Save' id='CellIBtns_Save'/><img src='../images/Cancel.jpg' class='CellBtns_Cancel' id='CellIBtns_Cancel'/></div>
     <div style='display:none;' class='Btns' id='RBtns'><img src='../images/tick.jpg' class='AllRBtnsSave' id='AllRBtnsSave'/><img src='../images/Cancel.jpg' class='AllRBtnsCancel'  id='AllRBtnsCancel'/></div>
     <div style='display:none;' class='CBtns' id='CBtns'><img src='../images/tick.jpg' class='AllCBtnsSave' id='AllCBtnsSave' /><img src='../images/Cancel.jpg' class='AllCBtnsCancel' id='AllCBtnsCancel' /></div>
     <div>  
        <asp:Panel ID="Pnl_Content" runat="server" Visible="false">
           <asp:Label ID="Label2" runat="server" Text=""></asp:Label>
        </asp:Panel>
     </div>   
     </div>
    <asp:HiddenField ID="btnVisible" runat="server"  value="0"  />
     <asp:HiddenField ID="RowCount" runat="server"  value="-1"  />
     <asp:HiddenField ID="StockCheck" runat="server"  value="0"  />
     <asp:HiddenField ID="CellClicked" runat="server"  value="0"  />
     <asp:HiddenField ID="ReqAlert" runat="server"  value="0"  />
      <asp:Button ID="ReloadThePanel" runat="server" style="display:none;"   OnClick="LoadStockSheet" />
      <asp:Label ID="lblJavaScript" runat="server" Text=""></asp:Label> 
       <asp:HiddenField ID="GENERATESTOCK" runat="server"  value=""  />       
         <asp:HiddenField ID="Clicked" runat="server"  value="0"  />
   
    
       
                                                <asp:Button ID="btn_Addnew" runat="Server" Style="display: none" />
                                                 <ajaxToolkit:ModalPopupExtender ID="MpAddnew" runat="server" BackgroundCssClass="modalBackground"
                                                    TargetControlID="btn_Addnew" PopupControlID="pnl_Addnew" CancelControlID="Btn_cancelAdd"
                                                    Drag="true" />
                                                <asp:Panel ID="pnl_Addnew" Width="480" runat="server" CssClass="modalPopup" Style="display: none">

                                                    <div class="panelouterblk">
                                                        <asp:Panel ID="Panel2" runat="server" CssClass="popupbartitle">
                                            Add New</asp:Panel>
                                                           
                                                     <asp:ImageButton ID="Closebtnimg" runat="server"  ImageUrl="~/assets/img/close.jpg" CssClass="Closebtnimg"></asp:ImageButton> 
                                                        <div class="popupcontentblk" style="width:auto;">
                                                            <p><asp:Label ID="lbl_Result" runat="server" Text="" ForeColor="Red"></asp:Label></p>
                                                            <div class="row">
                                                                <div class="col-sm-3">
                                                                    <label>Organization</label>
                                                                </div>
                                                                <div class="col-sm-9">
                                                                    <div class="form-group">
                                                                      <telerik:RadComboBox Skin="Simple" Filter="Contains" ID="ddlOrgNew" Width="100%" runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="True">
                                                </telerik:RadComboBox>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-sm-3">
                                                                    <label>Van</label>
                                                                </div>
                                                                <div class="col-sm-9">
                                                                    <div class="form-group">
                                                                        <telerik:RadComboBox Skin="Simple" Filter="Contains" ID="ddl_Van" Width="100%" runat="server" AutoPostBack="true">
                                                </telerik:RadComboBox>
                                                                        
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            
                                                            <div class="row">
                                                                <div class="col-sm-3">
                                                                    <label>SKU</label>
                                                                </div>
                                                                <div class="col-sm-9">
                                                                    <div class="form-group">
                                                                       <telerik:RadComboBox Skin="Simple" Filter="Contains" ID="ddlSKU" Width="100%" DataTextField="Description" DataValueField ="Inventory_Item_ID"
                                                    runat="server"  EnableLoadOnDemand="True" AutoPostBack="true" EmptyMessage="Please type item code/name">
                                                </telerik:RadComboBox>
                                                                    </div>
                                                                </div>
                                                            </div> 
                                                            <div class="row">
                                                                <div class="col-sm-3">
                                                                    <label>Qty in Stock UOM</label>
                                                                </div>
                                                                <div class="col-sm-9">
                                                                    <div class="form-group">
                                                                        <asp:TextBox ID="txt_Qty" CssClass ="inputSM" Width ="100%" runat="server" onKeypress='return NumericOnly(event)'></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                            </div> 
                                                            <div class="row">
                                                                <div class="col-sm-3">
                                                                    <label></label>
                                                                </div>
                                                                <div class="col-sm-9">
                                                                    <div class="form-group">
                                                                        <asp:Button ID="btn_Savenew" CssClass="btn btn-success" Text="Save" runat="server" />
                                                                        <asp:Button ID="Btn_cancelAdd" CssClass="btn btn-default" runat="server" Text="Close" />
                                                                    </div>
                                                                </div>
                                                            </div>   
                                                        </div>
                                                    </div>
                                                </asp:Panel>
                                            
    
   <asp:Button ID="btnHidden" runat="Server" Style="display: none" />
                                                 <ajaxToolkit:ModalPopupExtender ID="MpInfoError" runat="server" BackgroundCssClass="modalBackground"
                                                    TargetControlID="btnHidden" PopupControlID="pnlConfirm" 
                                                    Drag="true" />
                                                <asp:Panel ID="pnlConfirm" Width="700" height="500px" runat="server" CssClass="modalPopup" Style="display: none" >
                                                    <div class="panelouterblk">
                                                        <asp:Panel ID="Panel1" runat="server" CssClass="popupbartitle">
                                            Confirmation</asp:Panel>
                                                           
                                                     <asp:ImageButton ID="ImageButton1" runat="server" Visible="false"  ImageUrl="~/assets/img/close.jpg" CssClass="Closebtnimg"></asp:ImageButton>  

                                                    <table id="tableinPopupErr" width="690" cellpadding="10" style="background-color: White;">
                                                        <tr align="center">
                                                            <td align="center" style="cursor: move; background-color: #3399ff; text-align: center;
                                                                border: solid 1px #3399ff; color: White; padding: 3px">
                                                                  <asp:Label ID="lblinfo" Text="Availabe stock is less than the requested Quantity.<br/> Would you like to confirm ?"   runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" style="text-align: center">
                                                                <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="List" />
                                                                <div style="height:370px;overflow-y: auto;" >
                                                              <asp:GridView ID="gv_InvalidStock" runat="server" CssClass="tablecellalign">
                                                                </asp:GridView> 
                                                                    </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" style="text-align: center;">
                                                            <asp:Button ID="btnUpdate"  CssClass="btn btn-success" Text="Confirm" runat="server" />
                                                            <asp:Button ID="btnCancel"  CssClass="btn btn-default" runat="server" Text="Cancel" />
                                                          </td>
                                                        </tr>
                                                    </table>
                                                        </div>
                                                </asp:Panel>
    </ContentTemplate>
                            </asp:UpdatePanel>
                           
                           
                    
                            
                     
                        
                           
                            <asp:HiddenField ID="HClicked" runat="server"  value="0"  />
     
     
         
     
                                                
                                           
     
     </ContentTemplate>
                            </asp:UpdatePanel>
                            
    <div style="display:none">
    
    <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportPDF" runat="server" Text="Export" />
    
   </div>
                            <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="UpdatePanel1"
                    runat="server">
                    <ProgressTemplate>
                        <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                            <img src="../assets/img/ajax-loader.gif" alt="Processing..." />
                            <span>Processing... </span>
                        </asp:Panel>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                
   
 

</asp:Content>
