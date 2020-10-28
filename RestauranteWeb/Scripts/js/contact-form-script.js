$("#contactForm").validator().on("submit", function (event) {
    if (event.isDefaultPrevented()) {
        // handle the invalid form...
        formError();
        submitMSG(false, "Llene todos los campos");
    } else {
        // everything looks good!
        event.preventDefault();
        submitForm();
    }
});
/* Validacion de formulario de logeo*/
$("#LoginForm").validator().on("submit", function (event) {
    if (event.isDefaultPrevented()) {
        // handle the invalid form...
        formErrorLogin();
        submitMSGLogin(false, "Llene todos los campos para Iniciar Sesion");
    } else {
        // everything looks good!
        event.preventDefault();
        alert('Se Guardo Existosamente');
    }
});


/*Valicacion de formulario de registro*/
$("#RegistrarmeForm").validator().on("submit", function (event) {
    if (event.isDefaultPrevented()) {
        // handle the invalid form...
        formErrorRegistrarte();
        submitMSGRegistrate(false, "Llene todos los campos para registrarte");
    } else {
        // everything looks good!
        event.preventDefault();
        alert('Se Guardo Existosamente');
    }
});

/*Validacion de formulario de recuperacion de contraseña*/
$("#RecuperarEmailForm").validator().on("submit", function (event) {
    if (event.isDefaultPrevented()) {
        // handle the invalid form...
        formErrorRecuperar();
        submitMSGRecuperarEmail(false, "Llene todos los campos para Recuperar tu correo electronico");
    } else {
        // everything looks good!
        event.preventDefault();
        alert('Se Guardo Existosamente');
    }
});

function submitForm(){
    // Initiate Variables With Form Content
    var name = $("#name").val();
    var email = $("#email").val();
    var msg_subject = $("#msg_subject").val();
    var message = $("#message").val();


    $.ajax({
        type: "POST",
        url: "php/form-process.php",
        data: "name=" + name + "&email=" + email + "&msg_subject=" + msg_subject + "&message=" + message,
        success : function(text){
            if (text == "success"){
                formSuccess();
            } else {
                formError();
                submitMSG(false,text);
            }
        }
    });
}

function formSuccess(){
    $("#contactForm")[0].reset();
    submitMSG(true, "Message Submitted!")
}

function formError(){
    $("#contactForm").removeClass().addClass('shake animated').one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function(){
        $(this).removeClass();
    });
}

function formErrorLogin() {
    $("#LoginForm").removeClass().addClass('shake animated').one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function () {
        $(this).removeClass();
    });
}

function formErrorRegistrarte() {
    $("#RegistrarmeForm").removeClass().addClass('shake animated').one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function () {
        $(this).removeClass();
    });
}

function formErrorRecuperar() {
    $("#RecuperarEmailForm").removeClass().addClass('shake animated').one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function () {
        $(this).removeClass();
    });
}

function submitMSG(valid, msg){
    if(valid){
        var msgClasses = "h3 text-center tada animated text-success";
    } else {
        var msgClasses = "h3 text-center text-danger";
    }
    $("#msgSubmit").removeClass().addClass(msgClasses).text(msg);
}

function submitMSGLogin(valid, msg) {
    if (valid) {
        var msgClasses = "h3 text-center tada animated text-success";
    } else {
        var msgClasses = "h3 text-center text-danger";
    }
    $("#msgSubmitLogin").removeClass().addClass(msgClasses).text(msg);
}

function submitMSGRegistrate(valid, msg) {
    if (valid) {
        var msgClasses = "h3 text-center tada animated text-success";
    } else {
        var msgClasses = "h3 text-center text-danger";
    }
    $("#msgSubmitRegistrate").removeClass().addClass(msgClasses).text(msg);
}

function submitMSGRecuperarEmail(valid, msg) {
    if (valid) {
        var msgClasses = "h3 text-center tada animated text-success";
    } else {
        var msgClasses = "h3 text-center text-danger";
    }
    $("#msgSubmitREcuperarEmail").removeClass().addClass(msgClasses).text(msg);
}