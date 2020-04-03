<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConfigurePayment.ascx.cs" Inherits="LL.EpiserverCyberSourceConnector.CommerceManager.GooglePay.ConfigurePayment" %>

<div id="DataForm">
    <table cellpadding="0" cellspacing="2">
        <tr>
            <td class="FormLabelCell" colspan="2"><b>
                <asp:Literal ID="TitleLiteral" runat="server" Text="Configure CyberSource Account" /></b></td>
        </tr>
    </table>
    <br />
    <table class="DataForm">
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
    </table>
</div>
