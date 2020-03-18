function processApplePaySubmit(event) {
    event.preventDefault();
    placeApplePayOrder();
}

function placeApplePayOrder() {

    var request = {
        countryCode: 'US',
        currencyCode: 'USD',
        supportedNetworks: ['visa', 'masterCard', 'amex', 'discover'],
        merchantCapabilities: ['supports3DS'],
        total: {
            label: 'Luminos Labs',
            amount: '10.00',
            type: 'final'
        },
        lineItems: [
            {
                label: 'Bag Subtotal',
                type: 'final',
                amount: '35.00'
            },
            {
                label: 'Free Shipping',
                amount: '0.00',
                type: 'final'
            },
            {
                label: 'Estimated Tax',
                amount: '3.06',
                type: 'final'
            }
        ]
    }

    // version, ApplePayPaymentRequest
    var session = new window.ApplePaySession(1, request);

    // onvalidatemerchant: A callback function that is automatically called when the payment sheet is displayed.
    session.onvalidatemerchant = function (event) {
        var validationData = { ValidationUrl: event.validationURL };
        $.ajax({
            url: '/ApplePayPayment/StartApplePaySession',
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(validationData)
        }).then(function (merchantSession) {
            if (merchantSession !== false) {
                session.completeMerchantValidation(JSON.parse(merchantSession));
            }
        },
            function (error) {
                console.log("Error in StartApplePaySession:");
                console.log(error);
                session.abort();
            });
    }

    // Setup handler to receive the token when payment is authorized.
    session.onpaymentauthorized = function (event) {
        addPaymentOnCart(event, session);

        //ApplePayPaymentAuthorizationResult object
        var authorizationResult = {
            status: window.ApplePaySession.STATUS_SUCCESS,
            errors: []
        };

        //session.completePayment(ApplePaySession.STATUS_SUCCESS);

        // Do something with the payment to capture funds and
        // then dismiss the Apple Pay sheet for the session with
        // the relevant status code for the payment's authorization.
        session.completePayment(authorizationResult);
    };

    session.onshippingmethodselected = function (event) {
        var newTotal;
        var newLineItems;

        if (event.shippingMethod.identifier === "collection") {
            newTotal = window.totalForCollection;
            newLineItems = window.lineItemsForCollection;
        } else {
            newTotal = window.totalForDelivery;
            newLineItems = window.lineItemsForDelivery;
        }

        var update = {
            newTotal: newTotal,
            newLineItems: newLineItems
        };

        session.completeShippingMethodSelection(update);
    };

    session.oncancel = function (event) {
        console.log("Apple Pay cancel event " + event);
    }

    session.begin();

    session.ApplePayValidateMerchantEvent = function (event) {
        console.log(event);
    };
}

function addPaymentOnCart(event, session) {
    var action = window.addPaymentOnCartEndpoint;
    var formData = new FormData(form);
    var xhr = new XMLHttpRequest();

    xhr.open('POST', action, true);
    xhr.send(formData);

    xhr.onreadystatechange = function () {
        if (this.readyState === XMLHttpRequest.DONE) {
            if (this.status === 200) {
                // Get the payment data for use to capture funds from
                // the encrypted Apple Pay token in your server.
                var paymentDataString = JSON.stringify(event.payment.token.paymentData);

                var paymentDataBase64 = btoa(paymentDataString);
                var paymentCardType = JSON.stringify(event.payment.token.paymentMethod.network);

                $.ajax({
                    url: '/ApplePayPayment/ProcessApplePayPayment',
                    method: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({
                        'paymentData': paymentDataBase64,
                        'paymentCardType': paymentCardType
                    })
                }).then(function (result) {
                    if (result === true) {
                        console.log("ProcessApplePayPayment successfully: " + result);
                        // add logic to use signature
                        window.resubmitOriginalForm(window.purchaseEndpoint);
                    } else {
                        console.log("ProcessApplePayPayment unsuccessfully: " + result);
                    }
                },
                    function (error) {
                        console.log("Error in ProcessApplePayPayment: " + JSON.stringify(error));
                        session.abort();
                    });
            } else {
                console.log("AddPaymentOnCart fail");
            }
        }
    }
}

function canUseApplePay() {
    if (window.ApplePaySession && window.ApplePaySession.supportsVersion(1)) {
        // The Apple Pay JS API is available.

        return window.ApplePaySession.canMakePayments();
    }

    return false;
}