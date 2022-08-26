<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="consumer.aspx.cs" Inherits="EncDec.consumer" %>

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
                    <td>Encrypted Text :
                    </td>
                    <td>
                        <asp:Label ID="lblCiphertext" runat="server" Text=""></asp:Label>
                    </td>

                </tr>
                <tr>
                    <td>Plain Text :
                    </td>
                    <td>
                        <asp:Label ID="lblPlainText" runat="server" Text=""></asp:Label>

                    </td>
                    <td>
                        <asp:Button ID="btnbacktosender" runat="server" Text="Back To Sender" OnClick="btnbacktosender_Click" /></td>
                </tr>



            </table>
        </div>
    </form>
</body>
</html>
