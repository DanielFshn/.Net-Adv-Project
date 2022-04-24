function validateRegister(e){
    e.preventDefault();
    var name = document.getElementById("txtName").value;
    var email = document.getElementById("txtEmail").value;
    var password = document.getElementById("txtPassword").value;
    var confirmPassword = document.getElementById("txtConfirmPassword").value;
    var messageElement = document.getElementById('msgError');
    messageElement.style.display = "none"
    if(name == ""){
        messageElement.innerHTML = "Name is empty!";
        messageElement.style.display = "block"
    }
    else if(email==""){
        messageElement.innerHTML = "Email is empty!";
        messageElement.style.display= "block"
    }
    else if(!validEmail(email)){
        messageElement.innerHTML = "Wrong email!";
        messageElement.style.display = "block"
    }
  
    else if(password==""){
        messageElement.innerHTML = "Password is empty!";
        messageElement.style.display = "block"
    }
    else if(confirmPassword ==""){
        messageElement.innerHTML = "Confirm Password is empty!";
        messageElement.style.display = "block"
    }
    else if(password != confirmPassword){
        messageElement.innerHTML = "Passwords do not match";
        messageElement.style.display = "block"
    }
    else{
        document.getElementById("validForm").style.display = "block"
        
    }
}

function validEmail(email) {
    return String(email)
        .toLowerCase()
        .match(
            /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
        );
};