<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sender.aspx.cs" Inherits="EncDec.sender" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table>
                <tr>
                    <td>Enter Text To Encrypt :
                    </td>
                    <td>
                        <asp:TextBox ID="txtplaintext" runat="server"></asp:TextBox></td>
                    <td>
                        <asp:Button ID="btnEncrypt" runat="server" Text="Encrypt Text" OnClick="btnEncrypt_Click" /></td>
                </tr>

                <tr>
                    <td>Encrypted Text :
                    </td>
                    <td>
                        <asp:Label ID="lblCipherText" runat="server" Text=""></asp:Label>

                        <td>
                            <asp:Button ID="btnClearValues" runat="server" Text="Decrypt Text" OnClick="btnClearValues_Click" /></td>
                </tr>

                <tr>
                    <td>Decrypted Text :
                    </td>
                    <td>
                        <asp:Label ID="lblPlainText" runat="server" Text=""></asp:Label>

                        <td></td>
                </tr>

            </table>

        </div>
    </form>
</body>
</html>
