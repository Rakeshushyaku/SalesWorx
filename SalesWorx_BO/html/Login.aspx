<%@ Page Language="vb" AutoEventWireup="false"   CodeBehind="Login.aspx.vb" Inherits="SalesWorx_BO.Login" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" />
    <link rel="icon" href="../images/favicon.ico" type="image/x-icon" />

    <title>Login</title>
    
    <link href="../assets/css/bootstrap.css" rel="stylesheet" type="text/css"/>
    <link href="../assets/css/style.css" rel="stylesheet" type="text/css"/>
   
    <!--[if lt IE 9]>
      <script src="../assets/js/html5shiv.min.js"></script>
      <script src="../assets/js/respond.min.js"></script>
    <![endif]-->


    <script type="text/javascript">
        function ClearMsg_onkeypress() {
            document.getElementById("lblMsg").innerHTML = "";
            
            
        }
        </script>
</head>

<body class="login-page">
   
    <div class="login-content">
        <div class="login-content-inner">
            <div class="text-center login-logo">
                <img  alt="Salesworx" title="Salesworx" id="imgLogin" runat="server"  />
            </div>
            <form id="frmLogin" class="login-form" runat ="server">
                <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager> 
    

                <div class="row">
                    <div class="col-xs-12">
                        <div class="login-error"><asp:Label runat ="server" ID="lblMsg" ForeColor ="#D32F2F"  ></asp:Label></div>
                    </div>
                 
                </div>

                <div ID="logmoderow"  runat="server">
                    <div class="form-group" >
                        <label>Authentication Mode <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat ="server" ControlToValidate ="txtusername"  ForeColor ="Red" ErrorMessage ="*" ValidationGroup ="valreq"></asp:RequiredFieldValidator> </label>    
                        <asp:DropDownList Width ="100%" id="ddlLoginMode" runat="server"  class="form-control" >
                            <asp:ListItem Value="Native">Native</asp:ListItem>
                            <asp:ListItem Value="Windows">Windows</asp:ListItem>
                        </asp:DropDownList>                     
                    </div>
                </div>
                    
                <div class="form-group">
                    <label>Username <asp:RequiredFieldValidator ID="rfvUserName" runat ="server" ControlToValidate ="txtusername"  ForeColor ="Red" ErrorMessage ="*" ValidationGroup ="valreq"></asp:RequiredFieldValidator> </label>
                    <asp:TextBox type="text" name="txtUserName" id="txtUserName" class="form-control" runat ="server" onkeypress="javascript:ClearMsg_onkeypress();" />                        
                </div>
          
                    
                <div class="form-group">
                    <label>Password<asp:RequiredFieldValidator ID="rfvPwd" runat ="server" ControlToValidate ="txtpassword" ErrorMessage ="*"  ForeColor ="Red"  ValidationGroup ="valreq"></asp:RequiredFieldValidator> </label>
                    <asp:TextBox type="password" name="txtpassword" id="txtpassword" class="form-control" runat ="server" onkeypress="javascript:ClearMsg_onkeypress();" /> 
                </div>
          
                <!--<div class="form-group  col-md-12">
                    <div class="checkbox checkbox check-success">
                        <input type="checkbox" id="checkbox1" value="1">
                    </div>
                </div>-->

                <div class="row">
                    
                    <div class="col-xs-12">
                        <asp:Button   CssClass="btn btn-primary btn-block" Text="Submit" ValidationGroup ="valreq" runat ="server" ID="btnLogin" />
                    </div>
                </div>

            </form>
        </div>
    </div>
 
</body>
</html>