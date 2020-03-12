using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using LL.EpiserverCyberSourceConnector.Payments;
using LL.EpiserverCyberSourceConnector.Payments.ApplePay;
using Mediachase.Commerce.Orders.Dto;
using Mediachase.Web.Console.Interfaces;

namespace LL.EpiserverCyberSourceConnector.CommerceManager.ApplePay
{
    public partial class ConfigurePayment : UserControl, IGatewayControl
    {
        private PaymentMethodDto _paymentMethodDto;

        /// <summary>
        /// Gets or sets the validation group.
        /// </summary>
        /// <value>The validation group.</value>
        public string ValidationGroup { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurePayment"/> class.
        /// </summary>
        public ConfigurePayment()
        {
            ValidationGroup = string.Empty;
            _paymentMethodDto = null;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindData();
        }

        /// <summary>
        /// Loads the PaymentMethodDto object.
        /// </summary>
        /// <param name="dto">The PaymentMethodDto object.</param>
        public void LoadObject(object dtos)
        {
            _paymentMethodDto = dtos as PaymentMethodDto;
        }

        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// <param name="dto">The dto.</param>
        public void SaveChanges(object dto)
        {
            if (!Visible) return;

            _paymentMethodDto = dto as PaymentMethodDto;
            if (_paymentMethodDto?.PaymentMethodParameter != null)
            {
                var paymentMethodId = Guid.Empty;
                if (_paymentMethodDto.PaymentMethod.Count > 0)
                {
                    paymentMethodId = _paymentMethodDto.PaymentMethod[0].PaymentMethodId;
                }

                UpdateOrCreateParameter(BaseConfiguration.MerchantIdParameter, CybersourceMerchantId, paymentMethodId);
                UpdateOrCreateParameter(BaseConfiguration.TransactionTypeParameter, DropDownListTransactionType, paymentMethodId);
                
                UpdateOrCreateParameter(ApplePayConfiguration.ApplePayMerchantIdParameter, ApplepayMerchantId, paymentMethodId);
                UpdateOrCreateParameter(ApplePayConfiguration.ApplePayMerchantCertificateThumbprintParameter, CertificateThumbprintKey, paymentMethodId);
                UpdateOrCreateParameter(ApplePayConfiguration.ApplePayInitiativeContextParameter, initiativeContext, paymentMethodId);
                UpdateOrCreateParameter(ApplePayConfiguration.ApplePayDisplayNameParameter, displayName, paymentMethodId);
            }
        }

        /// <summary>
        /// Binds the data.
        /// </summary>
        private void BindData()
        {
            if (_paymentMethodDto?.PaymentMethodParameter != null)
            {
                BindParameterData(BaseConfiguration.MerchantIdParameter, CybersourceMerchantId);
                BindParameterData(BaseConfiguration.TransactionTypeParameter, DropDownListTransactionType);

                BindParameterData(ApplePayConfiguration.ApplePayMerchantIdParameter, ApplepayMerchantId);
                BindParameterData(ApplePayConfiguration.ApplePayMerchantCertificateThumbprintParameter, CertificateThumbprintKey);
                BindParameterData(ApplePayConfiguration.ApplePayInitiativeContextParameter, initiativeContext);
                BindParameterData(ApplePayConfiguration.ApplePayDisplayNameParameter, displayName);
            }
            else
            {
                Visible = false;
            }
        }

        private void UpdateOrCreateParameter(string parameterName, TextBox parameterControl, Guid paymentMethodId)
        {
            var parameter = GetParameterByName(parameterName);
            if (parameter != null)
            {
                parameter.Value = parameterControl.Text;
            }
            else
            {
                var row = _paymentMethodDto.PaymentMethodParameter.NewPaymentMethodParameterRow();
                row.PaymentMethodId = paymentMethodId;
                row.Parameter = parameterName;
                row.Value = parameterControl.Text;
                _paymentMethodDto.PaymentMethodParameter.Rows.Add(row);
            }
        }

        private void UpdateOrCreateParameter(string parameterName, CheckBox parameterControl, Guid paymentMethodId)
        {
            var parameter = GetParameterByName(parameterName);
            var value = parameterControl.Checked ? "1" : "0";
            if (parameter != null)
            {
                parameter.Value = value;
            }
            else
            {
                var row = _paymentMethodDto.PaymentMethodParameter.NewPaymentMethodParameterRow();
                row.PaymentMethodId = paymentMethodId;
                row.Parameter = parameterName;
                row.Value = value;
                _paymentMethodDto.PaymentMethodParameter.Rows.Add(row);
            }
        }

        private void UpdateOrCreateParameter(string parameterName, DropDownList parameterControl, Guid paymentMethodId)
        {
            var parameter = GetParameterByName(parameterName);
            var value = parameterControl.SelectedValue;
            if (parameter != null)
            {
                parameter.Value = value;
            }
            else
            {
                var row = _paymentMethodDto.PaymentMethodParameter.NewPaymentMethodParameterRow();
                row.PaymentMethodId = paymentMethodId;
                row.Parameter = parameterName;
                row.Value = value;
                _paymentMethodDto.PaymentMethodParameter.Rows.Add(row);
            }
        }

        private void BindParameterData(string parameterName, TextBox parameterControl)
        {
            var parameterByName = GetParameterByName(parameterName);
            if (parameterByName != null)
            {
                parameterControl.Text = parameterByName.Value;
            }
        }

        private void BindParameterData(string parameterName, CheckBox parameterControl)
        {
            var parameterByName = GetParameterByName(parameterName);
            if (parameterByName != null)
            {
                parameterControl.Checked = parameterByName.Value == "1";
            }
        }

        private void BindParameterData(string parameterName, DropDownList parameterControl)
        {
            var parameterByName = GetParameterByName(parameterName);
            if (parameterByName != null)
            {
                parameterControl.SelectedValue = parameterByName.Value;
            }
        }

        private PaymentMethodDto.PaymentMethodParameterRow GetParameterByName(string name)
        {
            return ConfigurationHelper.GetParameterByName(_paymentMethodDto, name);
        }
    }
}