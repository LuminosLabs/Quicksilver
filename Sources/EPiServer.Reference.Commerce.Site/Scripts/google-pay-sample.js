/**
 * Define the version of the Google Pay API referenced when creating your
 * configuration
 *
 * @see {@link https://developers.google.com/pay/api/web/reference/request-objects#PaymentDataRequest|apiVersion in PaymentDataRequest}
     */
const baseRequest = {
    apiVersion: 2,
    apiVersionMinor: 0
};

/**
 * Card networks supported by your site and your gateway
 *
* @see {@link https://developers.google.com/pay/api/web/reference/request-objects#CardParameters|CardParameters}
  * @todo confirm card networks supported by your site and gateway
  */
const allowedCardNetworks = ["AMEX", "DISCOVER", "INTERAC", "JCB", "MASTERCARD", "VISA"];

/**
 * Card authentication methods supported by your site and your gateway
 *
* @see {@link https://developers.google.com/pay/api/web/reference/request-objects#CardParameters|CardParameters}
 * supported card networks
 *
    PAN_ONLY: This authentication method is associated with payment cards stored on file with the user's Google Account. Returned payment data includes personal account number (PAN) with the expiration month and the expiration year.
    CRYPTOGRAM_3DS: This authentication method is associated with cards stored as Android device tokens. Returned payment data includes a 3-D Secure (3DS) cryptogram generated on the device.
 */
const allowedCardAuthMethods = ["PAN_ONLY", "CRYPTOGRAM_3DS"];

/**
 * Identify your gateway and your site's gateway merchant identifier
 *
 * The Google Pay API response will return an encrypted payment method capable
 * of being charged by a supported gateway after payer authorization
 *
    identify gateway https://developers.google.com/pay/api/web/reference/request-objects#gateway
    Merchand ID used when crated CyberSource account: luminoslabs_episerver
* @see {@link https://developers.google.com/pay/api/web/reference/request-objects#gateway|PaymentMethodTokenizationSpecification}
 */
const tokenizationSpecification = {
    type: 'PAYMENT_GATEWAY',
    parameters: {
        'gateway': 'cybersource',
        'gatewayMerchantId': 'luminoslabs_episerver'
    }
};

/**
 * Describe your site's support for the CARD payment method and its required
 * fields
 *
 * @see {@link https://developers.google.com/pay/api/web/reference/request-objects#CardParameters|CardParameters}
    */
const baseCardPaymentMethod = {
    type: 'CARD',
    parameters: {
        allowedAuthMethods: allowedCardAuthMethods,
        allowedCardNetworks: allowedCardNetworks
    }
};

/**
 * Describe your site's support for the CARD payment method including optional
 * fields
 *
 * @see {@link https://developers.google.com/pay/api/web/reference/request-objects#CardParameters|CardParameters}
    */
const cardPaymentMethod = Object.assign(
    {},
    baseCardPaymentMethod,
    {
        tokenizationSpecification: tokenizationSpecification
    }
);

/**
 * An initialized google.payments.api.PaymentsClient object or null if not yet set
 *
 * @see {@link getGooglePaymentsClient}
    */
let paymentsClient = null;

/**
 * Configure your site's support for payment methods supported by the Google Pay
 * API.
 *
 * Each member of allowedPaymentMethods should contain only the required fields,
 * allowing reuse of this base request when determining a viewer's ability
 * to pay and later requesting a supported payment method
 *
* @returns {object} Google Pay API version, payment methods supported by the site
 */
function getGoogleIsReadyToPayRequest() {
    return Object.assign(
        {},
        baseRequest,
        {
            allowedPaymentMethods: [baseCardPaymentMethod]
        }
    );
}

/**
* Configure support for the Google Pay API
*
 * @see {@link https://developers.google.com/pay/api/web/reference/request-objects#PaymentDataRequest|PaymentDataRequest}
 * @returns {object} PaymentDataRequest fields
    */
function getGooglePaymentDataRequest() {
    const paymentDataRequest = Object.assign({}, baseRequest);
    paymentDataRequest.allowedPaymentMethods = [cardPaymentMethod];
    paymentDataRequest.transactionInfo = getGoogleTransactionInfo();
    paymentDataRequest.merchantInfo = {
        // @todo a merchant ID is available for a production environment after approval by Google
        // See {@link https://developers.google.com/pay/api/web/guides/test-and-deploy/integration-checklist|Integration checklist}
        // merchantId: '01234567890123456789',
        merchantName: 'Luminos Labs Merchant'
    };
    return paymentDataRequest;
}

/**
 * Return an active PaymentsClient or initialize
 *
 * @see {@link https://developers.google.com/pay/api/web/reference/client#PaymentsClient|PaymentsClient constructor}
 * @returns {google.payments.api.PaymentsClient} Google Pay API client
    */
function getGooglePaymentsClient() {
    if (paymentsClient === null) {
        paymentsClient = new google.payments.api.PaymentsClient({ environment: 'TEST' });
    }
    return paymentsClient;
}

/**
 * Initialize Google PaymentsClient after Google-hosted JavaScript has loaded
 *
 * Display a Google Pay payment button after confirmation of the viewer's
 * ability to pay.
 */
function onGooglePayLoaded() {
    const paymentsClient = getGooglePaymentsClient();
    paymentsClient.isReadyToPay(getGoogleIsReadyToPayRequest())
        .then(function (response) {
            if (response.result) {
                addGooglePayButton();
                // @todo prefetch payment data to improve performance after confirming site functionality
                // prefetchGooglePaymentData();
            }
        })
        .catch(function (err) {
            // show error in developer console for debugging
            console.error(err);
        });
}

/**
* Add a Google Pay purchase button alongside an existing checkout button
*
 * @see {@link https://developers.google.com/pay/api/web/reference/request-objects#ButtonOptions|Button options}
 * @see {@link https://developers.google.com/pay/api/web/guides/brand-guidelines|Google Pay brand guidelines}
    */
function addGooglePayButton() {
    const paymentsClient = getGooglePaymentsClient();
    const button =
        paymentsClient.createButton({ onClick: onGooglePaymentButtonClicked });
    if (!isCyberSourcePaymentMethodSelected("CyberSourceGooglePay")) {
        button.style.visibility = 'hidden';
    }
    console.log('addGooglePayButton');
    document.getElementById('submit-area').appendChild(button);
}

/**
* Provide Google Pay API with a payment amount, currency, and amount status
*
 * @see {@link https://developers.google.com/pay/api/web/reference/request-objects#TransactionInfo|TransactionInfo}
 * @returns {object} transaction info, suitable for use as transactionInfo property of PaymentDataRequest
    */
function getGoogleTransactionInfo() {
    var currencyCode = document.getElementById("CurrencyCode");
    var countryCode = document.getElementById("MarketId");
    var totalPrice = document.querySelector("#total-price .product-price__currency-marker").nextSibling;

    return {
        countryCode: countryCode.options[countryCode.selectedIndex].value,
        currencyCode: currencyCode.options[currencyCode.selectedIndex].value,
        totalPriceStatus: 'FINAL',
        // set to cart total
        totalPrice: totalPrice.textContent.trim()
    };
}

/**
* Prefetch payment data to improve performance
*
 * @see {@link https://developers.google.com/pay/api/web/reference/client#prefetchPaymentData|prefetchPaymentData()}
    */
function prefetchGooglePaymentData() {
    const paymentDataRequest = getGooglePaymentDataRequest();
    // transactionInfo must be set but does not affect cache
    paymentDataRequest.transactionInfo = {
        totalPriceStatus: 'NOT_CURRENTLY_KNOWN',
        currencyCode: 'USD'
    };
    const paymentsClient = getGooglePaymentsClient();
    paymentsClient.prefetchPaymentData(paymentDataRequest);
}

/**
* Show Google Pay payment sheet when Google Pay payment button is clicked
*/
function onGooglePaymentButtonClicked() {
    const paymentDataRequest = getGooglePaymentDataRequest();
    paymentDataRequest.transactionInfo = getGoogleTransactionInfo();

    const paymentsClient = getGooglePaymentsClient();
    paymentsClient.loadPaymentData(paymentDataRequest)
        .then(function (paymentData) {
            // handle the response
            processPayment(paymentData);
        })
        .catch(function (err) {
            // show error in developer console for debugging
            console.error(err);
        });
}

/**
* Process payment data returned by the Google Pay API
*
 * @param {object} paymentData response from Google Pay API after user approves payment
 * @see {@link https://developers.google.com/pay/api/web/reference/response-objects#PaymentData|PaymentData object reference}
    */
function processPayment(paymentData) {
    // pass payment token to your gateway to process payment
    var cardType = paymentData.paymentMethodData.info.cardNetwork;
    var paymentDataBase64 = btoa(paymentData.paymentMethodData.tokenizationData.token);

    AddPaymentOnCart(paymentDataBase64, cardType);
}

function AddPaymentOnCart(paymentDataBase64, paymentCardType) {
    var action = '/Checkout/AddPaymentOnCart';
    var formData = new FormData(form);
    var xhr = new XMLHttpRequest();

    xhr.open('POST', action, true);
    xhr.send(formData);

    xhr.onreadystatechange = function () {
        if (this.readyState === XMLHttpRequest.DONE) {
            if (this.status === 200) {
                // Get the payment data for use to encrypt card details from
                // the cards stored on Google Wallet.

                $.ajax({
                    url: '/GooglePayPayment/ProcessGooglePayPayment',
                    method: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({
                        'paymentData': paymentDataBase64,
                        'paymentCardType': paymentCardType
                    })
                }).then(function (result) {
                    if (result) {
                        console.log("ProcessGooglePayPayment successful");
                        // add logic to use signature
                        window.resubmitOriginalForm(window.purchaseEndpoint);
                    } else {
                        console.log("Error in ProcessGooglePayPayment");
                    }
                },
                    function (error) {
                        console.log("Error in ProcessGooglePayPayment");
                    });
            } else {
                console.log("AddPaymentOnCart fail");
            }
        }
    }
}