window.onload = function () {
    setDefaultValues();

    //hide apple pay option if not on apple devices
    if (!window.canUseApplePay()) {
        window.hidePaymentOption("CyberSourceApplePay");
    }

    if (!getGoogleIsReadyToPayRequest()) {
        window.hidePaymentOption("CyberSourceGooglePay");
    }

    if (isCyberSourcePaymentMethodSelected("CyberSourceGooglePay")) {
        document.getElementById("placeOrder").style.visibility = 'hidden';
        var googlePayButton = document.getElementsByClassName("gpay-button");
        if (googlePayButton.length > 0) {
            googlePayButton[0].style.visibility = 'hidden';
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