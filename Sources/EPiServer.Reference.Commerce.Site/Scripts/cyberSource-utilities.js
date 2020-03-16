window.onload = function () {
    setDefaultValues();

    // hide apple pay option if not on apple devices
    if (!window.canUseApplePay()) {
        window.hidePaymentOption("CyberSourceApplePay");
    }

    // hide google pay option if client is not ready to pay
    if (!window.getGoogleIsReadyToPayRequest()) {
        window.hidePaymentOption("CyberSourceGooglePay");
    }

    if (isCyberSourcePaymentMethodSelected("CyberSourceGooglePay")) {
        document.getElementById("placeOrder").style.visibility = 'hidden';
        var googlePayButton = document.getElementsByClassName("gpay-button");
        if (googlePayButton.length > 0) {
            googlePayButton[0].style.visibility = 'visible';
        }
    }

}

function setDefaultValues() {
    var firstName = document.getElementById("BillingAddress_FirstName");
    if (firstName) {
        firstName.value = "John";
    } else return;
    document.getElementById("BillingAddress_LastName").value = "TEST";
    document.getElementById("BillingAddress_Email").value = "null3@test.com";
    document.getElementById("BillingAddress_Line1").value = "Street 1";
    document.getElementById("BillingAddress_City").value = "Los Angeles";
    document.getElementById("BillingAddress_PostalCode").value = "90045";
    var stateField = document.getElementById("BillingAddress_CountryRegion_Region");
    stateField.value = "California";
}

var paymentSelectors = document.querySelectorAll(".payment-methods .radio .jsChangePayment");

function isCyberSourcePaymentMethodSelected(paymentMethodSystemName) {
    for (var i = 0; i < paymentSelectors.length; i++) {
        if (paymentSelectors[i]
            && paymentSelectors[i].value === paymentMethodSystemName
            && paymentSelectors[i].checked)

            return true;
    }
    return false;
}

function hidePaymentOption(paymentOptionValue) {
    var paymentOptions = document.getElementsByClassName("jsChangePayment");
    for (var i = 0; i < paymentOptions.length; i++) {
        if (paymentOptions.item(i).value === paymentOptionValue) {
            paymentOptions.item(i).parentNode.parentNode.style.display = "none";
        }
    }
}

function resubmitOriginalForm(url) {
    form.action = url;
    form.submit();
}

function getCardType(number) {

    // number must not contain any whitespaces for the regex to work
    number = number.replace(/\s/g, '');

    // visa
    var re = new RegExp("^4");
    if (number.match(re) != null)
        return "Visa";

    // Mastercard
    // Updated for Mastercard 2017 BINs expansion
    if (/^(5[1-5][0-9]{14}|2(22[1-9][0-9]{12}|2[3-9][0-9]{13}|[3-6][0-9]{14}|7[0-1][0-9]{13}|720[0-9]{12}))$/.test(number))
        return "MasterCard";

    // American Express
    re = new RegExp("^3[47]");
    if (number.match(re) != null)
        return "Amex";

    // Discover
    re = new RegExp("^(6011|622(12[6-9]|1[3-9][0-9]|[2-8][0-9]{2}|9[0-1][0-9]|92[0-5]|64[4-9])|65)");
    if (number.match(re) != null)
        return "Discover";

    // Diners
    re = new RegExp("^36");
    if (number.match(re) != null)
        return "Diners Club";

    // Diners - Carte Blanche
    re = new RegExp("^30[0-5]");
    if (number.match(re) != null)
        return "Carte Blanche";

    // JCB
    re = new RegExp("^35(2[89]|[3-8][0-9])");
    if (number.match(re) != null)
        return "JCB";

    // Maestro UK
    re = new RegExp("^(67(59|677[04]))");
    if (number.match(re) != null)
        return "Maestro UK Domestic";

    // Maestro
    re = new RegExp("^(5[06-9])|^(6[0-9])");
    if (number.match(re) != null)
        return "Maestro International";

    //UATP
    var re = new RegExp("^1");
    if (number.match(re) != null)
        return "UATP";

    return "";
}
