window.onload = function () {
    setDefaultValues();
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