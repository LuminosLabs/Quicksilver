<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConfigurePayment.ascx.cs" Inherits="LL.EpiserverCyberSourceConnector.CommerceManager.ApplePay.ConfigurePayment" %>

<div id="DataForm">
    <table cellpadding="0" cellspacing="2">
        <tr>
            <td class="FormLabelCell" colspan="2"><b>
                <asp:Literal ID="TitleLiteral" runat="server" Text="Configure CyberSource Account" /></b></td>
        </tr>
    </table>
    <br />
    <table class="DataForm">
        <%--Transaction Type--%>
        <tr>
            <td class="FormLabelCell">
                <asp:Literal ID="DropDownListTransactionTypeLiteral" runat="server" Text="Transaction Type" />:</td>
            <td class="FormFieldCell">
                <asp:DropDownList ID="DropDownListTransactionType" runat="server">
                    <asp:ListItem Text="Authorization" Value="Authorization" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="Sale" Value="Sale"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
        <tr>
            <td class="FormLabelCell">
                <asp:Literal ID="CheckBoxDecisionManagerEnabledLiteral" runat="server" Text="Decision Manager Enabled" />:
            </td>
            <td class="FormFieldCell">
                <asp:CheckBox ID="CheckBoxDecisionManagerEnabled" runat="server" Checked="false" />
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
        <%--Cetificate Thubprint--%>
        <tr>
            <td class="FormLabelCell">
                <asp:Literal ID="CertificateThumbprintLiteral" runat="server" Text="Certificate Thumbprint Key" />:</td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" ID="CertificateThumbprintKey" Width="300px" MaxLength="250"></asp:TextBox><br />
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
        <%--Initiative Context--%>
        <tr>
            <td class="FormLabelCell">
                <asp:Literal ID="initiativeContextLiteral" runat="server" Text="Initiative Context" />:</td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" ID="initiativeContext" Width="300px" MaxLength="250"></asp:TextBox><br />
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
        <%--Display Name--%>
        <tr>
            <td class="FormLabelCell">
                <asp:Literal ID="displayNameLiteral" runat="server" Text="Display Name in Payment Sheet" />:</td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" ID="displayName" Width="300px" MaxLength="250"></asp:TextBox><br />
            </td>
        </tr>
    </table>
</div>
