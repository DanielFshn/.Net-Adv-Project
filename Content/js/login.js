function validateForm(e){
    e.preventDefault();
    var email = document.getElementById("txtEmail").value;
    var password = document.getElementById("txtPassword").value;
    var messageElement = document.getElementById('msgEmailError');

    if (email == "") {
        messageElement.innerText = "Email is empty!";
        messageElement.style.display = "block"
    }
    
    else if (!validEmail(email)) {
        messageElement.innerText = "Wrong Email";
        messageElement.style.display = "block"
    }
    else if (password == "") {
        messageElement.innerText = "Password empty!";
        messageElement.style.display = "block"
    }
    else {
        document.getElementById("formOk").style.display = "block"
        document.getElementById("msgEmailError").style.display = "none"
    }

}
function validEmail(email) {
    return String(email)
        .toLowerCase()
        .match(
            /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
        );
};