<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConfigurePayment.ascx.cs" Inherits="LL.EpiserverCyberSourceConnector.CommerceManager.CreditCard.ConfigurePayment" %>

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
                <asp:Literal ID="MerchantIdLiteral" runat="server" Text="Merchant Id" />:</td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" ID="MerchantId" Width="300px" MaxLength="250"></asp:TextBox><br />
                <asp:RequiredFieldValidator ControlToValidate="MerchantId" Display="dynamic" Font-Name="verdana" Font-Size="9pt"
                    ErrorMessage="Merchant Id required" runat="server" ID="Requiredfieldvalidator2">
                </asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
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
        <tr>
            <td class="FormLabelCell">
                <asp:Literal ID="SecretKeyLiteral" runat="server" Text="Secure Acceptance Secret Key" />:
            </td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" ID="SecretKey" Width="300px" MaxLength="500"></asp:TextBox><br />
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
        <tr>
            <td class="FormLabelCell">
                <asp:Literal ID="AccessKeyLiteral" runat="server" Text="Secure Acceptance Access Key" />:</td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" ID="AccessKey" Width="300px" MaxLength="250"></asp:TextBox><br />
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
        <tr>
            <td class="FormLabelCell">
                <asp:Literal ID="ProfileIdLiteral" runat="server" Text="Secure Acceptance Profile Id" />:</td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" ID="ProfileId" Width="300px" MaxLength="250"></asp:TextBox><br />
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
        <tr>
            <td class="FormLabelCell">
                <asp:Literal ID="UnsignedFieldsLiteral" runat="server" Text="Secure Acceptance Unsigned Data Field Names" />:</td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" ID="UnsignedFields" Width="300px" MaxLength="250"></asp:TextBox><br />
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
        <tr>
            <td class="FormLabelCell">
                <asp:Literal ID="IsPhoneNumberAddedLiteral" runat="server" Text="Add Phone Number To Secure Acceptance Request" />
            </td>
            <td class="FormFieldCell">
                <asp:CheckBox ID="IsPhoneNumberAdded" runat="server" Checked="false" />
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
        <tr>
            <td class="FormLabelCell">
                <asp:Literal ID="DropDownListSecureAcceptanceTransactionTypeLiteral" runat="server" Text="Secure Acceptance Transaction Type" />:</td>
            <td class="FormFieldCell">
                <asp:DropDownList ID="DropDownListSecureAcceptanceTransactionType" runat="server">
                    <asp:ListItem Text="create_payment_token" Value="create_payment_token" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="authorization" Value="authorization"></asp:ListItem>
                    <asp:ListItem Text="authorization,create_payment_token" Value="authorization,create_payment_token"></asp:ListItem>
                    <asp:ListItem Text="sale" Value="sale"></asp:ListItem>
                    <asp:ListItem Text="sale,create_payment_token" Value="sale,create_payment_token"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
    </table>
</div>
