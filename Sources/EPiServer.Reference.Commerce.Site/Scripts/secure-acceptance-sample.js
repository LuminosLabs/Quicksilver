function sign(data) {
    var accessKey = { "access_key": data.accessKey };
    var profileId = { "profile_id": data.profileId };
    var transactionType = { "transaction_type": data.transactionType };
    var transactionUuid = { "transaction_uuid": data.transactionUuid };
    var signedDateTime = { "signed_date_time": data.transactionDateTime };
    var signedFieldNames = { "signed_field_names": data.signedFieldNames };
    var unsignedFieldNames = { "unsigned_field_names": data.unsignedFieldNames };
    var locale = { "locale": data.locale };
    var referenceNumber = { "reference_number": new Date().getTime() };
    var amount = { "amount": data.amount.replace(/\r?\n|\r/, '') };
    var currency = { "currency": data.currency };
    var paymentMethod = { "payment_method": "card" };
    var billToForename = { "bill_to_forename": data.firstName };
    var billToSurname = { "bill_to_surname": data.lastName };
    var billToEmail = { "bill_to_email": data.email };
    var billToAddressLine1 = { "bill_to_address_line1": data.addressLine1 };
    var billToAddressCity = { "bill_to_address_city": data.city };
    var billToAddressState = { "bill_to_address_state": data.state };
    var billToAddressCountry = { "bill_to_address_country": data.country };
    var billToAddressPostalCode = { "bill_to_address_postal_code": data.postalCode };

    var request = Object.assign({},
        accessKey,
        profileId,
        transactionUuid,
        signedFieldNames,
        unsignedFieldNames,
        signedDateTime,
        locale,
        transactionType,
        referenceNumber,
        amount,
        currency,
        paymentMethod,
        billToForename,
        billToSurname,
        billToEmail,
        billToAddressLine1,
        billToAddressCity,
        billToAddressState,
        billToAddressCountry,
        billToAddressPostalCode);

    var xhr = new XMLHttpRequest();
    xhr.open('POST', data.signFieldsUrl, true);
    xhr.setRequestHeader("Content-Type", "application/json");
    xhr.send(JSON.stringify({ fields: request }));

    xhr.onreadystatechange = function () {
        if (this.readyState === XMLHttpRequest.DONE) {
            if (this.status === 200) {
                var response = JSON.parse(this.response);
                request["signature"] = response.result;
                submitSecureAcceptanceData(data, request);
            } else {
                console.log("Get signature fail");
            }
        }
    }
}

function submitSecureAcceptanceData(data, request) {
    var cardType = { "card_type": data.cardType };
    var cardNumber = { "card_number": data.cardNumber };
    var cardExpiryDateValue = data.cardExpireMonth + "-" + data.cardExpireYear;
    var cardExpiryDate = { "card_expiry_date": cardExpiryDateValue };
    Object.assign(request, cardType, cardNumber, cardExpiryDate);

    var form = document.getElementById(data.secureAcceptanceFormId);
    var input;
    for (var property in request) {
        if (request.hasOwnProperty(property)) {
            input = createInputElement(property, request[property]);
            form.appendChild(input);
        }
    }

    form.submit();
}

function createInputElement(name, value) {
    var inputElement = document.createElement("input");
    inputElement.setAttribute("type", "hidden");
    inputElement.setAttribute("name", name);
    inputElement.setAttribute("value", value);

    return inputElement;
}