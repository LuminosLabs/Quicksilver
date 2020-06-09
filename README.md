Episerver CyberSource Connector Reference Implementation
========================================================
[![License](http://img.shields.io/:license-apache-blue.svg?style=flat-square)](http://www.apache.org/licenses/LICENSE-2.0.html)

This repository is a fork from official EpiServer Quicksilver repository (https://github.com/episerver/Quicksilver)

This repository serves as an Episerver CyberSource Connector Reference Implementation of the **LL.EpiserverCyberSourceConnector.Core, LL.EpiserverCyberSourceConnector.Site, LL.EpiserverCyberSourceConnector.CommerceManager NuGet packages** with all the front-end and back-end files for the available payment methods.

Available payment methods in the current version: **Credit Card, PayPal, Google Pay, and Apple Pay**.

***Support for Serialized Carts is not supported in the current version of LL.EpiserverCyberSourceConnector.\* NuGet packages***

## Episerver CyberSource Connector Reference Implementation Installation

1. Follow the installation steps from the official EpiServer Quicksilver repository (https://github.com/episerver/Quicksilver)
2. Configure Visual Studio to add this package source: http://nuget.luminoslabs.com/nuget. This allows LL.EpiserverCyberSourceConnector.Core, LL.EpiserverCyberSourceConnector.Site, LL.EpiserverCyberSourceConnector.CommerceManager NuGet packages to be downloaded when the solution is built.
3. Start the site (Debug-Start from Visual studio) and browse to http://localhost:50244 to finish the installation. Login with admin@example.com/store.

#### LL.EpiserverCyberSourceConnector.Site Settings

On the site change the following settings in web.config appSettings:
- `<add key="cybs.keysDirectory" value="CHANGE-THIS" />` to your path on disk for the CyberSource license key file
- `<add key="cybs.merchantId" value="CHANGE-THIS" />` to your CyberSource Merchant ID
- `<add key="cybs.orgId" value="CHANGE-THIS" />` to the Org Id value requested from CyberSource. This value is needed for enabling Device Fingerprinting for Decision Manager fraud check system. Device Fingerprinting data is sent to CyberSource only if this value is populated. 
- `<add key="cybs.sendToProduction" value="false" />` change this value to **true** when going on production

**Note:** The above settings need to be applied to both Web and Commerce Manager projects.

--- 

#### LL.EpiserverCyberSourceConnector.CommerceManager

The following files are added on NuGet install on the Commerce Manager Project:
- Apps\Order\Payments\Plugins\CyberSourceCreditCard:
  - ConfigurePayment.ascx
  - CybersourceCreditCardPaymentMetaClass.xml
- Apps\Order\Payments\Plugins\CyberSourcePayPal:
  - ConfigurePayment.ascx
  - CybersourcePayPalPaymentMetaClass.xml
- Apps\Order\Payments\Plugins\CyberSourceGooglePay:
  - ConfigurePayment.ascx
  - CybersourceGooglePayPaymentMetaClass.xml
- Apps\Order\Payments\Plugins\CyberSourceApplePay:
  - ConfigurePayment.ascx
  - CybersourceApplePayPaymentMetaClass.xml

---
Commerce Manager configuration
------------------------------
Based on the required payment method please refer to the corresponding configuration section.

#### Setting up the Credit Card payment provider in Commerce Manager

Open the Episerver Commerce Manager back-end site. Then, follow these steps.

1. Go to **Administration > Order System > Payments > English**. The last option is the language in which you want to make the payment available.
2. Click **New** to create a new payment method.
	- For **System keyword**, select **CyberSourceCreditCard**, the name of the folder created during deployment.
	- For **Class Name**, select **LL.EpiserverCyberSourceConnector.Core.Payments.CreditCard.CyberSourceCreditCardGateway**
	- For **Payment Class**, select **LL.EpiserverCyberSourceConnector.Core.Payments.CreditCard.CyberSourceCreditCardPayment**
3. Click **OK** to save the CyberSourceCreditCard payment method.
4. Open the **CyberSourceCreditCard** payment method for additional editing.
5. Go to the **Parameters** tab and enter the following:
    - **Transaction Type** - the payment buyers will perform with CyberSource Credit Card. The default value is **Authorization**, whereby a payment is authorized only, not yet captured. If you specify **Sale**, the payment is immediately transferred from a buyer's account to the merchant's account
    - **Decision Manager Enabled** - enables Decision Manager(Advanced Fraud Screen). It will take effect only if the Decision Manager is active on the CyberSource account. If Decision Manager is enabled and **orgId** is enabled (see above) Device Fingerprinting fraud check also occurs for **Authorization** and **Sale** transactions. For more information about Device Fingerprinting fraud check the Decision Manager/Documentation/Guides Section in the CyberSource Dashboard.
    - **Secure Acceptance Secret Key** - obtained from the CyberSource dashboard during the Secure Acceptance setup. Secure Acceptance API is used to obtain the payment token from CyberSource without the sensitive Credit Card data hitting the back-end.
    - **Secure Acceptance Access Key** - obtained from the CyberSource dashboard during the Secure Acceptance setup
    - **Secure Acceptance Profile Id** - obtained from the CyberSource dashboard after a Secure Acceptance profile has been created
    - **Secure Acceptance Unsigned Data Field Names** - sensitive credit card details sent to Secure Acceptance. Default value is **card_type,card_number,card_expiry_date**. 
    - **Add Phone Number To Secure Acceptance Request** - checkbox to add the optional customer phone number field to the Secure Acceptance request
    - **Secure Acceptance Transaction Type** - default value is **create_payment_token** for creating a payment token only. This token then is used for **Authorization** or **Sale** transaction type mentioned above. Secure Acceptance is capable of also Authorization and Sale transaction types, but the back-end checkout flow must be changed to accommodate this change.
6. Open the **Markets** tab and add the expected markets for this payment.
7. In Commerce Manager, go to **Administration > Order System > Meta Classes**.
8. Click **Import/Export**, select **Import MetaData**.
9. To populate the MetaData import screen, drag and drop the meta class file to upload, from Commerce Manager project **Apps\Order\Payments\Plugins\CyberSourceCreditCardCybersourceCreditCardPaymentMetaClass.xml**.
10. Select the CybersourceCreditCardPaymentMetaClass.xml in the MetaData import screen. Click **Start Import**.


### 2. PayPal

#### Setting up the PayPal payment provider in Commerce Manager

Open the Episerver Commerce Manager back-end site. Then, follow these steps.

1. Go to **Administration > Order System > Payments > English**. The last option is the language in which you want to make the payment available.
2. Click **New** to create a new payment method.
	- For **System keyword**, select CyberSourcePayPal, the name of the folder created during deployment.
	- For **Class Name**, select **LL.EpiserverCyberSourceConnector.Core.Payments.PayPal.CyberSourcePayPalGateway**
	- For **Payment Class**, select **LL.EpiserverCyberSourceConnector.Core.Payments.PayPal.CyberSourcePayPalPayment**
3. Click **OK** to save the CyberSourcePayPal payment method.
4. Open the **CyberSourcePayPal** payment method for additional editing.
5. Go to the **Parameters** tab and enter the following:
    - **Transaction Type** - the payment buyers will perform with CyberSource PayPal. The default value is **Authorization**, whereby a payment is authorized only, not yet captured. If you specify **Sale**, the payment is immediately transferred from a buyer's account to the merchant's account
    - **Decision Manager Enabled** - enables Decision Manager(Advanced Fraud Screen). It will take effect only if the Decision Manager is active on the CyberSource account. If Decision Manager is enabled and **orgId** is enabled (see above) Device Fingerprinting fraud check also occurs for **Authorization** and **Sale** transactions. For more information about Device Fingerprinting fraud check the Decision Manager/Documentation/Guides Section in the CyberSource Dashboard.
    - **Success Url** - an endpoint that PayPal will call after the user presses Continue on the PayPal Page. Please see **PayPalPaymentController.Purchase** action for a sample
    - **Cancel Url** - an endpoint that PayPal will call after the user presses Cancel and return to seller's website on the PayPal Page
6. Open the **Markets** tab and add the expected markets for this payment.
7. In Commerce Manager, go to **Administration > Order System > Meta Classes**.
8. Click **Import/Export**, select **Import MetaData**.
9. To populate the MetaData import screen, drag and drop the meta class file to upload, from Commerce Manager project **Apps\Order\Payments\Plugins\CyberSourcePayPal\CybersourcePayPalPaymentMetaClass.xml**.
10. Select the CybersourcePayPalPaymentMetaClass.xml in the MetaData import screen. Click **Start Import**.


### 3. Google Pay

#### Setting up the Google Pay payment provider in Commerce Manager
Open the Episerver Commerce Manager back-end site. Then, follow these steps.

1. Go to **Administration > Order System > Payments > English**. The last option is the language in which you want to make the payment available.
2. Click **New** to create a new payment method.
	- For **System keyword**, select CyberSourceGooglePay, the name of the folder created during deployment.
	- For **Class Name**, select **LL.EpiserverCyberSourceConnector.Core.Payments.GooglePay.CyberSourceGooglePayGateway**
	- For **Payment Class**, select **LL.EpiserverCyberSourceConnector.Core.Payments.GooglePay.CyberSourceGooglePayPayment**
3. Click **OK** to save the CyberSourceGooglePay payment method.
4. Open the **CyberSourceGooglePay** payment method for additional editing.
5. Go to the **Parameters** tab and enter the following:
	- **Transaction Type** - the payment buyers will perform with CyberSource Google Pay. The default value is **Authorization**, whereby a payment is authorized only, not yet captured. If you specify **Sale**, the payment is immediately transferred from a buyer's account to the merchant's account
    - **Decision Manager Enabled** - enables Decision Manager(Advanced Fraud Screen). It will take effect only if the Decision Manager is active on the CyberSource account. If Decision Manager is enabled and **orgId** is enabled (see above) Device Fingerprinting fraud check also occurs for **Authorization** and **Sale** transactions. For more information about Device Fingerprinting fraud check the Decision Manager/Documentation/Guides Section in the CyberSource Dashboard.
6. Open the **Markets** tab and add the expected markets for this payment.
7. In Commerce Manager, go to **Administration > Order System > Meta Classes**.
8. Click **Import/Export**, select **Import MetaData**.
9. To populate the MetaData import screen, drag and drop the meta class file to upload, from the Commerce Manager project **Apps\Order\Payments\Plugins\CyberSourceGooglePay\CyberSourceGooglePayPaymentMetaClass.xml**.
10. Select the CybersourceGooglePayPaymentMetaClass.xml in the MetaData import screen. Click **Start Import**.

### 4. Apple Pay

#### Setting up the Apple Pay payment provider in Commerce Manager
Open the Episerver Commerce Manager back-end site. Then, follow these steps.

1. Go to **Administration > Order System > Payments > English**. The last option is the language in which you want to make the payment available.
2. Click **New** to create a new payment method.
	- For **System keyword**, select CyberSourceApplePay, the name of the folder created during deployment.
	- For **Class Name**, select **LL.EpiserverCyberSourceConnector.Core.Payments.ApplePay.CyberSourceApplePayGateway**
	- For **Payment Class**, select **LL.EpiserverCyberSourceConnector.Core.Payments.ApplePay.CyberSourceApplePayPayment**
    3. Click **OK** to save the **CyberSourceApplePay** payment method.
4. Open the **CyberSourceApplePay** payment method for additional editing.
5. Go to the **Parameters** tab and enter the following:
    - **Transaction Type** - the payment buyers will perform with CyberSource Apple Pay. The default value is **Authorization**, whereby a payment is authorized only, not yet captured. If you specify **Sale**, the payment is immediately transferred from a buyer's account to the merchant's account
    - **Decision Manager Enabled** - enables Decision Manager(Advanced Fraud Screen). It will take effect only if the Decision Manager is active on the CyberSource account. If Decision Manager is enabled and **orgId** is enabled (see above) Device Fingerprinting fraud check also occurs for **Authorization** and **Sale** transactions. For more information about Device Fingerprinting fraud check the Decision Manager/Documentation/Guides Section in the CyberSource Dashboard.
    - **Certificate Thumbprint Key**, the Thumbprint Key of the Merchant certificate installed on the server the application is deployed after it is generated from Apple Developer portal
    - **Initiative Context**, the URL address of the front-end site (Quicksilver)
    - **Display Name in Payment Sheet**, name of the company that will receive the money; this will be displayed in Apple Pay payment sheet pop-up on the Apple device
6. Open the **Markets** tab and add the expected markets for this payment.
7. In Commerce Manager, go to **Administration > Order System > Meta Classes**.
8. Click **Import/Export**, select **Import MetaData**.
9. To populate the MetaData import screen, drag and drop the meta class file to upload, from the Commerce Manager project **Apps\Order\Payments\Plugins\CyberSourceApplePay\CyberSourceApplePayPaymentMetaClass.xml**.
10. Select the CybersourceApplePayPaymentMetaClass.xml in the MetaData import screen. Click **Start Import**.

---

Front-end site reference implementation
---------------------------------------

Note that this is a reference implementation and it can be used as a starting point for your project. This reference implementation is based on the Quicksilver project template

Based on the required payment method please refer to the corresponding section.

#### Files modified from the official EpiServer Quicksilver repository
Below is a list of files that were modified or added from the official EpiServer Quicksilver repository. Please look for comments `//CyberSource Connector Code Changes` that mark the changed areas.

1. Views: 
    - *Modified:*
      - App_Code\Helpers.cshtml
      - Views\Checkout\SingleShipmentCheckout.cshtml, OrderSummary.cshtml
2. Scripts:
    - *Added:* Scripts\cybersSource-utilities.js
3. Controllers:
    - *Modified:*
      -  Features\Checkout\Controllers\CheckoutController.cs
4. Other: 
    - *Modified:*
      - Features\Checkout\Service\ CheckoutService.cs, 
      - Features\Checkout\ViewModelFactories\CheckoutViewModelFactory.cs


### 1. Credit Card 


#### Prerequisites

1. Contact CyberSource Customer Support to set up your CyberSource merchant account
2. Setup in CyberSource Dashboard https://ebctest.cybersource.com/ a Secure Acceptance Profile
3. For more details refer to official CyberSource [Credit Card Simple Order API](https://developer.cybersource.com/library/documentation/dev_guides/CC_Svcs_SO_API/Credit_Cards_SO_API.pdf) and [Secure Acceptance API](https://developer.cybersource.com/library/documentation/dev_guides/Secure_Acceptance_Checkout_API/Secure_Acceptance_Checkout_API.pdf) documentation

#### Setting up the Credit Card payment provider on the front-end site

The following files were added:
1. Views: 
   - Shared\ _CyberSourceCreditCard.cshtml, _CyberSourceCreditCardCheckout.cshtml, _CyberSourceCreditCardConfirmation.cshtml
2. Scripts: 
   - Scripts\ secure-acceptance-sample.js
3. Controllers: 
   - Features\Payment\Controllers\SecureAcceptanceController.cs

#### Credit Card Components

Credit Card implementation features two CyberSource APIs:
1. **Secure Acceptance API** 
   - This API is used to generate a payment token in CyberSource without the sensitive credit card data hitting the back-end.
   - For more details please refer to the official CyberSource Secure Acceptance API documentation
2. **Simple Order API** 
   - This API is used for Authorization / Authorization & Capture (Sale) of the payment in CyberSource. The code for making Simple Order API requests resides in the LL.EpiserverCyberSourceConnector.* NuGet packages. 
   - For more details refer to the official CyberSource Credit Card Simple Order API documentation 


### 2. Pay Pal 

#### Prerequisites

1. Contact CyberSource Customer Support to set up your CyberSource merchant account
2. Follow the prerequisites steps from the official [CyberSource PayPal using Alternative Pay documentation](https://developer.cybersource.com/library/documentation/dev_guides/AltPay_PayPal_Express_SO/AltPay_PayPal_Express_SO_API.pdf)

#### Setting up the PayPal payment provider on the front-end site

The following files were added:
1. Views: 
    - Shared\ _CyberSourcePayPal.cshtml, _CyberSourcePayPalConfirmation.cshtml
3. Controllers:
    - Features\Payment\Controllers\PayPalPaymentController.cs


### 3. Google pay 

#### Prerequisites

1. Contact CyberSource Customer Support to set up your CyberSource merchant account
2. Follow the prerequisites steps from the official [CyberSource Google Pay documentation](https://developer.cybersource.com/library/documentation/dev_guides/Google_Pay_SO_API/Google_Pay_SO_API.pdf)

#### Setting up the Google Pay payment provider on the front-end site

The following files were added:
1. Views:
    - Shared\ _CyberSourceGooglePayCheckout.cshtml, _CyberSourceGooglePayConfirmation.cshtml, _CyberSourceGooglePay.cshtml
2. Scripts:
    - Scripts\ google-pay-sample.js
3. Controllers:
    - Features\Payment\Controllers\GooglePayPaymentController.cs

###  4. Apple Pay

#### Prerequisites

1. Contact CyberSource Customer Support to set up your CyberSource merchant account
2. Follow the prerequisites steps from the official [CyberSource Apple Pay documentation](https://developer.cybersource.com/library/documentation/dev_guides/apple_payments/SO_API/Apple_Pay_SO_API.pdf)

#### Setting up the Apple Pay payment provider on the front-end site

The following files were added:
1. Views:
    - Shared\  _CyberSourceApplePayCheckout.cshtml, _CyberSourceApplePayConfirmation.cshtml, _CyberSourceApplePay.cshtml
2. Scripts:
    - Scripts\ google-pay-sample.js
3. Styles:
    - Styles\ApplePayStyling.css
4. Controllers:
    - Features\Payment\Controllers\ApplePayPaymentController.cs
5. Models:
   - Features\Payment\Models\ MerchantSessionRequest.cs, ValidateMerchantSessionModel.cs
6. Services: 
   - Features\Payment\Services\ CertificateService.cs
