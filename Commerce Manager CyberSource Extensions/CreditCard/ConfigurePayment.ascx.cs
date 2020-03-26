using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using LL.EpiserverCyberSourceConnector.Payments;
using LL.EpiserverCyberSourceConnector.Payments.CreditCard;
using Mediachase.Commerce.Orders.Dto;
using Mediachase.Web.Console.Interfaces;

namespace LL.EpiserverCyberSourceConnector.CommerceManager.CreditCard
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
        public void LoadObject(object dto)
        {
            _paymentMethodDto = dto as PaymentMethodDto;
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

                UpdateOrCreateParameter(BaseConfiguration.TransactionTypeParameter, DropDownListTransactionType, paymentMethodId);
                UpdateOrCreateParameter(BaseConfiguration.DecisionManagerIsEnabledParameter, CheckBoxDecisionManagerEnabled, paymentMethodId);
                UpdateOrCreateParameter(CreditCardConfiguration.SecretKeyParameter, SecretKey, paymentMethodId);
                UpdateOrCreateParameter(CreditCardConfiguration.AccessKeyParameter, AccessKey, paymentMethodId);
                UpdateOrCreateParameter(CreditCardConfiguration.ProfileIdParameter, ProfileId, paymentMethodId);
                UpdateOrCreateParameter(CreditCardConfiguration.UnsignedFieldsParameter, UnsignedFields, paymentMethodId);
                UpdateOrCreateParameter(CreditCardConfiguration.IsPhoneNumberAddedParameter, IsPhoneNumberAdded, paymentMethodId);
                UpdateOrCreateParameter(CreditCardConfiguration.SecureAcceptanceTransactionTypeParameter, DropDownListSecureAcceptanceTransactionType, paymentMethodId);
            }
        }

        /// <summary>
        /// Binds the data.
        /// </summary>
        private void BindData()
        {
            if (_paymentMethodDto?.PaymentMethodParameter != null)
            {
                BindParameterData(BaseConfiguration.TransactionTypeParameter, DropDownListTransactionType);
                BindParameterData(BaseConfiguration.DecisionManagerIsEnabledParameter, CheckBoxDecisionManagerEnabled);
                BindParameterData(CreditCardConfiguration.SecretKeyParameter, SecretKey);
                BindParameterData(CreditCardConfiguration.AccessKeyParameter, AccessKey);
                BindParameterData(CreditCardConfiguration.ProfileIdParameter, ProfileId);
                BindParameterData(CreditCardConfiguration.UnsignedFieldsParameter, UnsignedFields);
                BindParameterData(CreditCardConfiguration.IsPhoneNumberAddedParameter, IsPhoneNumberAdded);
                BindParameterData(CreditCardConfiguration.SecureAcceptanceTransactionTypeParameter,
                    DropDownListSecureAcceptanceTransactionType);
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