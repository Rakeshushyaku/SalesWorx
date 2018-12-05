<%@ Page Title="Login" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master" CodeBehind="Login.aspx.vb" Inherits="SalesWorx_BO.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" >
        
        <tr>
            <td id="contentofpage" width="100%" height="76%" class="topshadow" style="vertical-align: middle;
                text-align: center;">
                <asp:Label ID="lblMsg" runat="server" Text="" Font-Bold="True" ForeColor="Maroon"></asp:Label>
                <table width="auto" border="0" cellspacing="0" cellpadding="0" style="text-align: left;
                    margin: auto;" id="tableform">
                   
                    <tr>
                        <td colspan="2" style="background: #3399ff; color: #FFFFFF;">
                            <label style="font-size: 14px;">
                                Login</label>
                        </td>
                    </tr>
                     <tr runat ="server" id="logmoderow">
                    <td>
                                 <label>
                                Authentication Mode</label>
                   </td>
                   <td>
                      <asp:DropDownList ID="ddlLoginMode"  Width ="155px" runat="server">
                  
                    <asp:ListItem Value="Native">Native</asp:ListItem>
                    <asp:ListItem Value="Windows">Windows</asp:ListItem>
             
                </asp:DropDownList>
                  <%--   <asp:RadioButton ID="rbRSALogin" runat="server" Checked="true" GroupName="GpLogin" Text="RSA Login" />
                    <asp:RadioButton ID="rbWinLogin" runat="server" GroupName="GpLogin" 
                        Text="Windows Login" />--%>
                </td>
                    </tr>
                    <tr>
                        <td>
                                                    <label>
                                Login</label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtUserName" runat="server" Width="155px"  ></asp:TextBox>
                            
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                ControlToValidate="txtUserName" ErrorMessage="Enter User Name"></asp:RequiredFieldValidator>
                            
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>
                                Password</label>
                        </td>
                        <td>
                             <asp:TextBox ID="txtPassword" runat="server" Width="155px" TextMode="Password" ></asp:TextBox>
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                 ControlToValidate="txtPassword" ErrorMessage="EnterPassword"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td style="padding-left: 20px">
                           
                            <asp:Button ID="btnLogin" runat="server" Text="Login" class="btnInputBlue" 
                            />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td style="font-size: .76em; padding-left: 20px">
                           <%-- <a href="#" visible ="false">Issues? Contact Admin.</a>--%>
                        </td>
                    </tr>
                </table>
               
            </td>
           
        </tr>
        </table>
</asp:Content>
