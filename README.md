Quicksilver 
===========
[![GitHub version](https://badge.fury.io/gh/episerver%2FQuicksilver.svg)](https://github.com/episerver/Quicksilver)
[![License](http://img.shields.io/:license-apache-blue.svg?style=flat-square)](http://www.apache.org/licenses/LICENSE-2.0.html)

This repository is fork from official EpiServer Quicksilver repository (https://github.com/episerver/Quicksilver)

This repository serves as a sample instalation of the LL.EpiserverCyberSourceConnector NuGet with all the front-end and back-end files.

The available payment methods in CyberSource are the following: Credit Card, PayPal, Apple Pay, Google Pay.

!!! Important !!! In the current version LL.EpiserverCyberSourceConnector NuGet applies only to Episerver solutions without Serialized Carts.


Installation
------------

1.  Configure Visual Studio to add this package sources: http://nuget.episerver.com/feed/packages.svc/ and http://nuget.luminoslabs.com/nuget This allows missing packages to be downloaded, when the solution is built.
2.  Open solution and build to download nuget package dependencies.
3.  Search the solution for "ChangeThis" and review/update as described.
4.  Run Setup\SetupDatabases.cmd to create the databases *. In the unlucky event of errors please check the logs.  
5.  Start the site (Debug-Start from Visual studio) and browse to http://localhost:50244 to finish installation. Login with admin@example.com/store.
6.	On the site change in web.config appSettings <add key="cybs.keysDirectory" value="CHANGE-THIS" /> to your path on disk for the Cybs license key file

*By default SetupDatabases.cmd use the default SQL Server instance. Change this line `set sql=sqlcmd -S . -E` by replacing `.` with the instance name to use different instance.

Note: SQL scripts are executed using Windows authentication so make sure your user has sufficient permissions.

Styling
-------

The styling of the site is done in [less](http://lesscss.org/). In order to be able to recompile the less files to css you will need to
install [nodejs](https://nodejs.org/). If you have nodejs the less files will be recompiled into css on every build. From the command line
you can also execute the following command in folder "Sources\EPiServer.Reference.Commerce.Site\":

```
msbuild -t:BuildLessFiles
```

Compiling the razor views
-------------------------

If you want to build the views to validate their correctness you can set the MvcBuildViews parameter to true.

```
msbuild -p:MvcBuildViews=true
```


SQL Server authentication
-------------------------

If you don't have mixed mode authentication enabled you can edit this line in SetupDatabases.cmd and provide username and password.

```
set sql=sqlcmd -S . -U username -P password
```

Commerce Manager Setup
----------------------

Credit Card
-----------
1. Find the Commerce Manager Apps folder, located under the Episerver Commerce Manager project folder.
2. Deploy the **ConfigurePayment.ascx** file from root this repository in Commerce Manager\CreditCard to the Apps\Order\Payments\Plugins\CyberSourceCreditCard folder. If the CyberSourceCreditCard folder does not exist, create it.

Setting up the Credit Card payment provider in Commerce Manager
----------------------------------------------------------
Open the Episerver Commerce Manager back-end site. Then, follow these steps.

1. Go to **Administration > Order System > Payments > English**. The last option is the language in which you want to make the payment available.
2. Click **New** to create new payment method.
	- For **System keyword**, select **CyberSourceCreditCard**, the name of the folder created during deployment.
	- For **Class Name**, select **LL.EpiserverCyberSourceConnector.Payments.CreditCard.CyberSourceCreditCardGateway**
	- For **Payment Class**, select **LL.EpiserverCyberSourceConnector.Payments.CreditCard.CyberSourceCreditCardPayment**
3. Click **OK** to save the CyberSourceCreditCard payment method.
4. Open the **CyberSourceCreditCard** payment method for additional editing.
5. Go to the **Parameter** tab and enter the following:
	- **Merchant Id** - the Merchant Id of the CyberSource account
    - **Transaction Type** - the payment buyers will perform with CyberSource Credit Card. The default value is **Authorization**, whereby a payment is authorized only, not yet captured. If you specify **Sale**, the payment is immediately transferred from a buyer's account to the merchant's account
    - **Decision Manager Enabled** - this checkbox enables Decision Manager. It will have effect only if the Decision Manager is active on the CyberSource account.
    - **Secure Acceptance Secret Key** - obtained from the CyberSource dashboard during the Secure Acceptance setup. Secure Acceptance API is used to obtain the payment token from CyberSource without the sensitive Credit Card data hitting the back-end.
    - **Secure Acceptance Access Key** - obtained from the CyberSource dashboard during the Secure Acceptance setup
    - **Secure Acceptance Profile Id** - obtained from the CyberSource dashboard after a Secure Acceptance profile has been created
    - **Secure Acceptance Unsigned Data Field Names** - sensitive credit card details sent to Secure Acceptance. Default value is **card_type,card_number,card_expiry_date**. 
    - **Add Phone Number To Secure Acceptance Request** - checkbox to add the otional customer phone number field to the Secure Acceptance request
    - **Secure Acceptance Transaction Type** - default value is **create_payment_token** for creating a payment token only. This token then is used for **Authorization** or **Sale** transaction type mentioned above. Secure Acceptance is capable of also Authorization and Sale transaction types, but the back-end checkout flow must be changed to accomodate this change.
6. Open the **Markets** tab and add the expected markets for this payment.
7. In Commerce Manager, go to **Administration > Order System > Meta Classes**.
8. Click **Import/Export**, select **Import MetaData**.
9. To populate the MetaData import screen, drag and drop the meta class file to upload, \Sample\CreditCard\CybersourceCreditCardPaymentMetaClass.xml.
10. Select the CybersourceCreditCardPaymentMetaClass.xml in the MetaData import screen. Click **Start Import**.

PayPal
-----------
1. Find the Commerce Manager Apps folder, located under the Episerver Commerce Manager project folder.
2. Deploy the **ConfigurePayment.ascx** file from this repository in Commerce Manager\PayPal to the Apps\Order\Payments\Plugins\CyberSourcePayPal folder. If the CyberSourcePayPal folder does not exist, create it.

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
5. Go to the **Parameter** tab and enter the following:
	- **Merchant Id** - the Merchant Id of the CyberSource account
    - **Transaction Type** - the payment buyers will perform with CyberSource PayPal. The default value is **Authorization**, whereby a payment is authorized only, not yet captured. If you specify **Sale**, the payment is immediately transferred from a buyer's account to the merchant's account
    - **Success Url** - endpoint that PayPal will call after the user presses Continue on the PayPal Page. Please see **PayPalPaymentController.Purchase** action as a sample
    - **Cancel Url** - endpoint that PayPal will call after the user presses Cancel and return to seller's website on the PayPal Page
6. Open the **Markets** tab and add the expected markets for this payment.
7. In Commerce Manager, go to **Administration > Order System > Meta Classes**.
8. Click **Import/Export**, select **Import MetaData**.
9. To populate the MetaData import screen, drag and drop the meta class file to upload, PayPal\CybersourcePayPalPaymentMetaClass.xml.
10. Select the CybersourcePayPalPaymentMetaClass.xml in the MetaData import screen. Click **Start Import**.

Google Pay
-----------
1. Find the Commerce Manager Apps folder, located under the Episerver Commerce Manager project folder.
2. Deploy the **ConfigurePayment.ascx** file from this repository in Commerce Manager\GooglePay to the Apps\Order\Payments\Plugins\CyberSourceGooglePay folder. If the CyberSourceGooglePay folder does not exist, create it.

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
5. Go to the **Parameter** tab and enter the following:
	- **Merchant Id** - the Merchant Id of the CyberSource account
    - **Transaction Type** - the payment buyers will perform with CyberSource Google Pay. The default value is **Authorization**, whereby a payment is authorized only, not yet captured. If you specify **Sale**, the payment is immediately transferred from a buyer's account to the merchant's account
6. Open the **Markets** tab and add the expected markets for this payment.
7. In Commerce Manager, go to **Administration > Order System > Meta Classes**.
8. Click **Import/Export**, select **Import MetaData**.
9. To populate the MetaData import screen, drag and drop the meta class file to upload, GooglePay\CyberSourceGooglePayPaymentMetaClass.xml.
10. Select the CybersourceGooglePayPaymentMetaClass.xml in the MetaData import screen. Click **Start Import**.

Apple Pay
-----------
1. Find the Commerce Manager Apps folder, located under the Episerver Commerce Manager project folder.
2. Deploy the **ConfigurePayment.ascx** file from this repository in Commerce Manager\ApplePay to the Apps\Order\Payments\Plugins\CyberSourceApplePay folder. If the CyberSourceApplePay folder does not exist, create it.

Setting up the Apple Pay payment provider in Commerce Manager
----------------------------------------------------------
Open the Episerver Commerce Manager back-end site. Then, follow these steps.

1. Go to **Administration > Order System > Payments > English**. The last option is the language in which you want to make the payment available.
2. Click **New** to create new payment method.
	- For **System keyword**, select CyberSourceApplePay, the name of the folder created during deployment.
	- For **Class Name**, select **LL.EpiserverCyberSourceConnector.Payments.ApplePay.CyberSourceApplePayGateway**
	- For **Payment Class**, select **LL.EpiserverCyberSourceConnector.Payments.ApplePay.CyberSourceApplePayPayment**
    - For **Apple Pay Merchant Id**, id of the merchant as generated in the apple developer portal
    - For **Certificate Thumbprint Key**, the Thumbprint Key of the Merchant certificate seen after generated from Apple Developer portal
    - For **Initiative Context**, website address of the apple pay integration
    - For **Display Name in Payment Sheet**, name of the company that will receive the money, this will be displayed in Apple Pay payment sheet
3. Click **OK** to save the **CyberSourceApplePay** payment method.
4. Open the **CyberSourceApplePay** payment method for additional editing.
5. Go to the **Parameter** tab and enter the following:
	- **Merchant Id** - the Merchant Id of the CyberSource account
    - **Transaction Type** - the payment buyers will perform with CyberSource Apple Pay. The default value is **Authorization**, whereby a payment is authorized only, not yet captured. If you specify **Sale**, the payment is immediately transferred from a buyer's account to the merchant's account
6. Open the **Markets** tab and add the expected markets for this payment.
7. In Commerce Manager, go to **Administration > Order System > Meta Classes**.
8. Click **Import/Export**, select **Import MetaData**.
9. To populate the MetaData import screen, drag and drop the meta class file to upload, GooglePay\CyberSourceApplePayPaymentMetaClass.xml.
10. Select the CybersourceApplePayPaymentMetaClass.xml in the MetaData import screen. Click **Start Import**.

