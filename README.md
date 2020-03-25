Quicksilver 
===========
[![GitHub version](https://badge.fury.io/gh/episerver%2FQuicksilver.svg)](https://github.com/episerver/Quicksilver)
[![License](http://img.shields.io/:license-apache-blue.svg?style=flat-square)](http://www.apache.org/licenses/LICENSE-2.0.html)

This repository is fork from official EpiServer Quicksilver repository (https://github.com/episerver/Quicksilver)

This repository serves as a sample instalation of the **LL.EpiserverCyberSourceConnector NuGet** with all the front-end and back-end files for the available payment methods in the package.

The available payment methods in CyberSource are the following: **Credit Card, PayPal, Apple Pay, Google Pay**.

**!!! IMPORTANT !!!** In the current version LL.EpiserverCyberSourceConnector NuGet applies only to Episerver solutions **WITHOUT *Serialized Carts***.


Quicksilver Installation
------------

1.  Configure Visual Studio to add this package sources: http://nuget.episerver.com/feed/packages.svc/ and http://nuget.luminoslabs.com/nuget. This allows missing packages to be downloaded, when the solution is built.
2.  Open solution and build to download nuget package dependencies.
3.  Search the solution for "ChangeThis" and review/update as described.
4.  Run Setup\SetupDatabases.cmd to create the databases *. In the unlucky event of errors please check the logs.  
5.  Start the site (Debug-Start from Visual studio) and browse to http://localhost:50244 to finish installation. Login with admin@example.com/store.

*By default SetupDatabases.cmd use the default SQL Server instance. Change this line `set sql=sqlcmd -S . -E` by replacing `.` with the instance name to use different instance.

Note: SQL scripts are executed using Windows authentication so make sure your user has sufficient permissions.

Quicksilver Styling
-------------------

The styling of the site is done in [less](http://lesscss.org/). In order to be able to recompile the less files to css you will need to
install [nodejs](https://nodejs.org/). If you have nodejs the less files will be recompiled into css on every build. From the command line
you can also execute the following command in folder "Sources\EPiServer.Reference.Commerce.Site\":

```
msbuild -t:BuildLessFiles
```

Quicksilver Compiling the razor views
-------------------------------------

If you want to build the views to validate their correctness you can set the MvcBuildViews parameter to true.

```
msbuild -p:MvcBuildViews=true
```


Quicksilver SQL Server authentication
-------------------------------------

If you don't have mixed mode authentication enabled you can edit this line in SetupDatabases.cmd and provide username and password.

```
set sql=sqlcmd -S . -U username -P password
```


LL.EpiserverCyberSourceConnector Settings
-----------------------------------------

On the site change the following settings in web.config appSettings:
- `<add key="cybs.keysDirectory" value="CHANGE-THIS" />` to your path on disk for the Cybs license key file
- `<add key="cybs.merchantId" value="CHANGE-THIS" />` to your Merchant ID
- `<add key="cybs.orgId" value="CHANGE-THIS" />` to the Org Id value requested from CyberSource. This value is needed for enabling Device Fingerprinting for Decision Manager fraud check system. Device Fingerprinting data is sent to CyberSource only if this value is populated, 
- `<add key="cybs.sendToProduction" value="false" />` change this value to **true** when going live on production

**Note:** **LL.EpiserverCyberSourceConnector** must be installed on the front-end site and on the Commerce Manager site. The above settings will be added to the Commerce Manager web.config as well. We removed them from Commerce Manager site in sample. We advise you to remove them from the web.config in the Commerce Manager website.


Credit Card
======================

Commerce Manager Setup
-----------
1. Find the Commerce Manager Apps folder, located under the Episerver Commerce Manager project folder.
2. Deploy the **ConfigurePayment.ascx** file from the root of this repository in Commerce Manager\CreditCard to the Apps\Order\Payments\Plugins\CyberSourceCreditCard folder. If the CyberSourceCreditCard folder does not exist, create it.

Setting up the Credit Card payment provider in Commerce Manager
---------------------------------------------------------------
Open the Episerver Commerce Manager back-end site. Then, follow these steps.

1. Go to **Administration > Order System > Payments > English**. The last option is the language in which you want to make the payment available.
2. Click **New** to create new payment method.
	- For **System keyword**, select **CyberSourceCreditCard**, the name of the folder created during deployment.
	- For **Class Name**, select **LL.EpiserverCyberSourceConnector.Payments.CreditCard.CyberSourceCreditCardGateway**
	- For **Payment Class**, select **LL.EpiserverCyberSourceConnector.Payments.CreditCard.CyberSourceCreditCardPayment**
3. Click **OK** to save the CyberSourceCreditCard payment method.
4. Open the **CyberSourceCreditCard** payment method for additional editing.
5. Go to the **Parameters** tab and enter the following:
    - **Transaction Type** - the payment buyers will perform with CyberSource Credit Card. The default value is **Authorization**, whereby a payment is authorized only, not yet captured. If you specify **Sale**, the payment is immediately transferred from a buyer's account to the merchant's account
    - **Decision Manager Enabled** - enables Decision Manager(Advanced Fraud Screen). It will have effect only if the Decision Manager is active on the CyberSource account. If Decision Manager is enabled and **orgId** is enabled (see above) Device Fingerprinting fraud check also occurs for **Authorization** and **Sale** transactions. For more information about Device Fingerprinting fraud check the Decision Manager/Documentation/Guides Section in the CyberSource Dashboard.
    - **Secure Acceptance Secret Key** - obtained from the CyberSource dashboard during the Secure Acceptance setup. Secure Acceptance API is used to obtain the payment token from CyberSource without the sensitive Credit Card data hitting the back-end.
    - **Secure Acceptance Access Key** - obtained from the CyberSource dashboard during the Secure Acceptance setup
    - **Secure Acceptance Profile Id** - obtained from the CyberSource dashboard after a Secure Acceptance profile has been created
    - **Secure Acceptance Unsigned Data Field Names** - sensitive credit card details sent to Secure Acceptance. Default value is **card_type,card_number,card_expiry_date**. 
    - **Add Phone Number To Secure Acceptance Request** - checkbox to add the otional customer phone number field to the Secure Acceptance request
    - **Secure Acceptance Transaction Type** - default value is **create_payment_token** for creating a payment token only. This token then is used for **Authorization** or **Sale** transaction type mentioned above. Secure Acceptance is capable of also Authorization and Sale transaction types, but the back-end checkout flow must be changed to accomodate this change.
6. Open the **Markets** tab and add the expected markets for this payment.
7. In Commerce Manager, go to **Administration > Order System > Meta Classes**.
8. Click **Import/Export**, select **Import MetaData**.
9. To populate the MetaData import screen, drag and drop the meta class file to upload, from the root of this repository in Commerce Manager\CreditCard\CybersourceCreditCardPaymentMetaClass.xml.
10. Select the CybersourceCreditCardPaymentMetaClass.xml in the MetaData import screen. Click **Start Import**.

Setting up the Credit Card payment provider on the front-end site
-----------------------------------------------------------------
Below is a list of files that were added or modified from the default Quicksilver solution. Please look out for comments `//CyberSource Connector Code Changes` that highlighted the changed areas.

1. Views:
    - Added: 
      - Shared\ _CyberSourceCreditCard.cshtml, _CyberSourceCreditCardCheckout.cshtml, _CyberSourceCreditCardConfirmation.cshtml
      - App_Code\Helpers.cshtml
    - Modified: 
      - Views\Checkout\SingleShipmentCheckout.cshtml, OrderSummary.cshtml
2. Scripts:
    - Added: Scripts\cybersSource-utilities.js, secure-acceptance-sample.js
3. Controllers:
    - Added: Features\Payment\Controllers\SecureAcceptanceController.cs
    - Modified: Features\Checkout\Controllers\CheckoutController.cs
4. Other back-end files that were modified: CheckoutService.cs, CheckoutViewModelFactory.cs

Credit Card Components
----------------------

Credit Card implementation features two CyberSource APIs:
1. **Secure Acceptance API** 
   - This API is used to generate a payment token in CyberSource without the sensitive credit card data hitting the back-end.
2. **Simple Order API** 
   - This API is used for Authorization / Authorization + Capture (Sale) of the payment in CyberSource. The code for making Simple Order API requests resides in the LL.EpiserverCyberSourceConnector NuGet. 

PayPal
======
1. Find the Commerce Manager Apps folder, located under the Episerver Commerce Manager project folder.
2. Deploy the **ConfigurePayment.ascx** file from the root of this repository in Commerce Manager\PayPal to the Apps\Order\Payments\Plugins\CyberSourcePayPal folder. If the CyberSourcePayPal folder does not exist, create it.

Setting up the PayPal payment provider in Commerce Manager
----------------------------------------------------------
Open the Episerver Commerce Manager back-end site. Then, follow these steps.

1. Go to **Administration > Order System > Payments > English**. The last option is the language in which you want to make the payment available.
2. Click **New** to create new payment method.
	- For **System keyword**, select CyberSourcePayPal, the name of the folder created during deployment.
	- For **Class Name**, select **LL.EpiserverCyberSourceConnector.Payments.PayPal.CyberSourcePayPalGateway**
	- For **Payment Class**, select **LL.EpiserverCyberSourceConnector.Payments.PayPal.CyberSourcePayPalPayment**
3. Click **OK** to save the CyberSourcePayPal payment method.
4. Open the **CyberSourcePayPal** payment method for additional editing.
5. Go to the **Parameters** tab and enter the following:
    - **Transaction Type** - the payment buyers will perform with CyberSource PayPal. The default value is **Authorization**, whereby a payment is authorized only, not yet captured. If you specify **Sale**, the payment is immediately transferred from a buyer's account to the merchant's account
    - **Decision Manager Enabled** - enables Decision Manager(Advanced Fraud Screen). It will have effect only if the Decision Manager is active on the CyberSource account. If Decision Manager is enabled and **orgId** is enabled (see above) Device Fingerprinting fraud check also occurs for **Authorization** and **Sale** transactions. For more information about Device Fingerprinting fraud check the Decision Manager/Documentation/Guides Section in the CyberSource Dashboard.
    - **Success Url** - endpoint that PayPal will call after the user presses Continue on the PayPal Page. Please see **PayPalPaymentController.Purchase** action as a sample
    - **Cancel Url** - endpoint that PayPal will call after the user presses Cancel and return to seller's website on the PayPal Page
6. Open the **Markets** tab and add the expected markets for this payment.
7. In Commerce Manager, go to **Administration > Order System > Meta Classes**.
8. Click **Import/Export**, select **Import MetaData**.
9. To populate the MetaData import screen, drag and drop the meta class file to upload, from the root of this repository in Commerce Manager\PayPal\CybersourcePayPalPaymentMetaClass.xml.
10. Select the CybersourcePayPalPaymentMetaClass.xml in the MetaData import screen. Click **Start Import**.

Setting up the PayPal payment provider on the front-end site
-----------------------------------------------------------------
Below is a list of files that were added or modified from the default Quicksilver solution. Please look out for comments `//CyberSource Connector Code Changes` that highlighted the changed areas.

1. Views:
    - Added: 
      - Shared\ _CyberSourcePayPal.cshtml, _CyberSourcePayPalConfirmation.cshtml
      - App_Code\Helpers.cshtml
    - Modified: 
      - Views\Checkout\SingleShipmentCheckout.cshtml, OrderSummary.cshtml
2. Scripts:
    - Added: Scripts\cybersSource-utilities.js
3. Controllers:
    - Added: Features\Payment\Controllers\PayPalPaymentController.cs
    - Modified: Features\Checkout\Controllers\CheckoutController.cs
4. Other back-end files that were modified: CheckoutService.cs, CheckoutViewModelFactory.cs



Google Pay
==========
1. Find the Commerce Manager Apps folder, located under the Episerver Commerce Manager project folder.
2. Deploy the **ConfigurePayment.ascx** file from the root of this repository in in Commerce Manager\GooglePay to the Apps\Order\Payments\Plugins\CyberSourceGooglePay folder. If the CyberSourceGooglePay folder does not exist, create it.

Setting up the Google Pay payment provider in Commerce Manager
----------------------------------------------------------
Open the Episerver Commerce Manager back-end site. Then, follow these steps.

1. Go to **Administration > Order System > Payments > English**. The last option is the language in which you want to make the payment available.
2. Click **New** to create new payment method.
	- For **System keyword**, select CyberSourceGooglePay, the name of the folder created during deployment.
	- For **Class Name**, select **LL.EpiserverCyberSourceConnector.Payments.GooglePay.CyberSourceGooglePayGateway**
	- For **Payment Class**, select **LL.EpiserverCyberSourceConnector.Payments.GooglePay.CyberSourceGooglePayPayment**
3. Click **OK** to save the CyberSourceGooglePay payment method.
4. Open the **CyberSourceGooglePay** payment method for additional editing.
5. Go to the **Parameters** tab and enter the following:
	- **Transaction Type** - the payment buyers will perform with CyberSource Google Pay. The default value is **Authorization**, whereby a payment is authorized only, not yet captured. If you specify **Sale**, the payment is immediately transferred from a buyer's account to the merchant's account
    - **Decision Manager Enabled** - enables Decision Manager(Advanced Fraud Screen). It will have effect only if the Decision Manager is active on the CyberSource account. If Decision Manager is enabled and **orgId** is enabled (see above) Device Fingerprinting fraud check also occurs for **Authorization** and **Sale** transactions. For more information about Device Fingerprinting fraud check the Decision Manager/Documentation/Guides Section in the CyberSource Dashboard.
6. Open the **Markets** tab and add the expected markets for this payment.
7. In Commerce Manager, go to **Administration > Order System > Meta Classes**.
8. Click **Import/Export**, select **Import MetaData**.
9. To populate the MetaData import screen, drag and drop the meta class file to upload, from the root of this repository in Commerce Manager\GooglePay\CyberSourceGooglePayPaymentMetaClass.xml.
10. Select the CybersourceGooglePayPaymentMetaClass.xml in the MetaData import screen. Click **Start Import**.

Setting up the Google Pay payment provider on the front-end site
-----------------------------------------------------------------
Below is a list of files that were added or modified from the default Quicksilver solution. Please look out for comments `//CyberSource Connector Code Changes` that highlighted the changed areas.

1. Views:
    - Added: 
      - Shared\ _CyberSourceGooglePayCheckout.cshtml, _CyberSourceGooglePayConfirmation.cshtml, _CyberSourceGooglePay.cshtml
      - App_Code\Helpers.cshtml
    - Modified: 
      - Views\Checkout\SingleShipmentCheckout.cshtml, OrderSummary.cshtml
2. Scripts:
    - Added: Scripts\cybersSource-utilities.js, google-pay-sample.js
3. Controllers:
    - Added: Features\Payment\Controllers\GooglePayPaymentController.cs
    - Modified: Features\Checkout\Controllers\CheckoutController.cs
4. Other back-end files that were modified: CheckoutService.cs, CheckoutViewModelFactory.cs



Apple Pay
=========
1. Find the Commerce Manager Apps folder, located under the Episerver Commerce Manager project folder.
2. Deploy the **ConfigurePayment.ascx** file from the root of this repository in Commerce Manager\ApplePay to the Apps\Order\Payments\Plugins\CyberSourceApplePay folder. If the CyberSourceApplePay folder does not exist, create it.

Setting up the Apple Pay payment provider in Commerce Manager
----------------------------------------------------------
Open the Episerver Commerce Manager back-end site. Then, follow these steps.

1. Go to **Administration > Order System > Payments > English**. The last option is the language in which you want to make the payment available.
2. Click **New** to create new payment method.
	- For **System keyword**, select CyberSourceApplePay, the name of the folder created during deployment.
	- For **Class Name**, select **LL.EpiserverCyberSourceConnector.Payments.ApplePay.CyberSourceApplePayGateway**
	- For **Payment Class**, select **LL.EpiserverCyberSourceConnector.Payments.ApplePay.CyberSourceApplePayPayment**
    3. Click **OK** to save the **CyberSourceApplePay** payment method.
4. Open the **CyberSourceApplePay** payment method for additional editing.
5. Go to the **Parameters** tab and enter the following:
    - **Transaction Type** - the payment buyers will perform with CyberSource Apple Pay. The default value is **Authorization**, whereby a payment is authorized only, not yet captured. If you specify **Sale**, the payment is immediately transferred from a buyer's account to the merchant's account
    - **Decision Manager Enabled** - enables Decision Manager(Advanced Fraud Screen). It will have effect only if the Decision Manager is active on the CyberSource account. If Decision Manager is enabled and **orgId** is enabled (see above) Device Fingerprinting fraud check also occurs for **Authorization** and **Sale** transactions. For more information about Device Fingerprinting fraud check the Decision Manager/Documentation/Guides Section in the CyberSource Dashboard.
    - **Apple Pay Merchant Id**, id of the merchant as generated in the Apple Developer Portal
    - **Certificate Thumbprint Key**, the Thumbprint Key of the Merchant certificate installed on the server the application is deployed after it is generated from Apple Developer portal
    - **Initiative Context**, URL address of the front-end site (Quicksilver)
    - **Display Name in Payment Sheet**, name of the company that will receive the money; this will be displayed in Apple Pay payment sheet pop-up on the Apple device
6. Open the **Markets** tab and add the expected markets for this payment.
7. In Commerce Manager, go to **Administration > Order System > Meta Classes**.
8. Click **Import/Export**, select **Import MetaData**.
9. To populate the MetaData import screen, drag and drop the meta class file to upload, from the root of this repository in Commerce Manager\GooglePay\CyberSourceApplePayPaymentMetaClass.xml.
10. Select the CybersourceApplePayPaymentMetaClass.xml in the MetaData import screen. Click **Start Import**.


Setting up the Google Pay payment provider on the front-end site
-----------------------------------------------------------------
Below is a list of files that were added or modified from the default Quicksilver solution. Please look out for comments `//CyberSource Connector Code Changes` that highlighted the changed areas.

1. Views:
    - Added: 
      - Shared\  _CyberSourceApplePayCheckout.cshtml, _CyberSourceApplePayConfirmation.cshtml, _CyberSourceApplePay.cshtml
      - App_Code\Helpers.cshtml
    - Modified: 
      - Views\Checkout\SingleShipmentCheckout.cshtml, OrderSummary.cshtml
2. Scripts:
    - Added: Scripts\cybersSource-utilities.js, google-pay-sample.js
3. Styles:
    - Added: Styles\ApplePayStyling.css
4. Controllers:
    - Added: Features\Payment\Controllers\ApplePayPaymentController.cs
    - Modified: Features\Checkout\Controllers\CheckoutController.cs
5. Models:
   - Added: Features\Payment\Models\ MerchantSessionRequest.cs, ValidateMerchantSessionModel.cs
6. Services: 
   - Added: Features\Payment\Services folder: CertificateService.cs
7. Other back-end files that were modified: Features\Checkout\Service\CheckoutService.cs, Features\Checkout\ViewModelFactories\CheckoutViewModelFactory.cs

