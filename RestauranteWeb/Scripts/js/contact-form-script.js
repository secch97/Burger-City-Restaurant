$("#contactForm").validator().on("submit", function (event) {
    if (event.isDefaultPrevented()) {
        // handle the invalid form...
        formError();
        submitMSG(false, "Complete todos los campos");
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
        submitMSGLogin(false, "Complete todos los campos para iniciar sesion");
    } else {
        // everything looks good!
        event.preventDefault();
        submitFormLogin();
    }
});



/*Valicacion de formulario de registro*/
$("#RegistrarmeForm").validator().on("submit", function (event) {
    if (event.isDefaultPrevented()) {
        // handle the invalid form...
        formErrorRegistrarte();
        submitMSGRegistrate(false, "Complete todos los campos para registrarse");
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
        submitMSGRecuperarEmail(false, "Escriba su correo electronico para reestablecer su clave");
    } else {
        // everything looks good!
        event.preventDefault();
        submitFormRecuperar();
    }
});



/*Validacion de formulario de cambiar contraseña*/
$("#CambiarClaveForm").validator().on("submit", function (event) {
    
    if (event.isDefaultPrevented()) {
        // handle the invalid form...
        formErrorCambiarClave();
        submitMSGCambiarClave(false, "Complete todos los campos de clave");


    } else if ($("#txtContraCambiar").val().length < 6) {

        event.preventDefault();
        formErrorCambiarClave();
        submitMSGCambiarClave(false, "La clave debe poseer como minimo 6 caracteres");


    } else if ($("#txtContraCambiarConfir").val().length < 6) {

        event.preventDefault();
        formErrorCambiarClave();
        submitMSGCambiarClave(false, "La clave debe poseer como minimo 6 caracteres");

    

    } else if ($("#txtContraCambiar").val() != $("#txtContraCambiarConfir").val()) {

        event.preventDefault();
        formErrorCambiarClave();
        submitMSGCambiarClave(false, "Las claves deben ser iguales");
    
    } else {
        event.preventDefault();
        // everything looks good!
        
        submitFormCambiarClave();
    }
});



function submitForm() {
    // Initiate Variables With Form Content
    var name = $("#name").val();
    var email = $("#email").val();
    var msg_subject = $("#msg_subject").val();
    var message = $("#message").val();


    $.ajax({
        type: "POST",
        url: "php/form-process.php",
        data: "name=" + name + "&email=" + email + "&msg_subject=" + msg_subject + "&message=" + message,
        success: function (text) {
            if (text == "success") {
                formSuccess();
            } else {
                formError();
                submitMSG(false, text);
            }
        }
    });
}


//Validar enviar a controlador 
function submitFormLogin() {
    // Initiate Variables With Form Content
    var Correo = $("#txtCorreoLogin").val();
    var Clave = $("#txtPassworkLogin").val();
    var recordarme = $("#txtRecuerdame").val();

    var login = { Correo: Correo, Clave: Clave, recordarme: recordarme };

    var url = $("#LoginForm").attr("action");

    $.ajax({
        type: "POST",
        url: url,
        data: login,
        success: function (text) {
            if (text == "1") {
                formSuccessLogin();
                var url2 = $(location).attr('href');
                if (url2 != "https://localhost:44360/") {
                    $(location).attr('href', 'Inicio');
                } else {
                    location.reload();
                }
            } else if (text == "5") {
                formSuccessLogin();
                var url2 = $(location).attr('href');
                if (url2 != "https://localhost:44360/") {
                    $(location).attr('href', 'MandarRestaurante');
                } else {
                    $(location).attr('href', 'Cliente/MandarRestaurante');
                }
            } else if (text == "6") {
                formSuccessLogin();
                var url2 = $(location).attr('href');
                if (url2 != "https://localhost:44360/") {
                    $(location).attr('href', 'MandarDelivery');
                } else {
                    $(location).attr('href', 'Cliente/MandarDelivery');
                }
            } else if (text == "2") {
                formSuccessLogin();
                var url2 = $(location).attr('href');
                if (url2 != "https://localhost:44360/") {
                    $(location).attr('href', 'MandarAdmin');
                } else {
                    $(location).attr('href', 'Cliente/MandarAdmin');
                }
            } else {
                if (text == 3) {
                    formErrorLogin();
                    submitMSGLogin(false, "Credenciales incorrectas");
                } else if (text == 4) {
                    formErrorLogin();
                    submitMSGLogin(false, "El correo electronico no se encuentra asociado a ninguna cuenta");
                } else {
                    formErrorLogin();
                    submitMSGLogin(false, "Credenciales incorrectas");
                }
            }
        }
    });
}



/*Enviar datos a controlador Cliente metodo recuperar*/
function submitFormRecuperar() {
    // Initiate Variables With Form Content
    var Correo = $("#txtCorreoRecuperar").val();

    var url = $("#RecuperarEmailForm").attr("action");

    $.ajax({
        type: "POST",
        url: url,
        data: {
            Correo: Correo
        },
        success: function (text) {
            if (text == "1") {
               /* alert('Se le envio al correo una validacion');*/
                formSuccessRecuperar();
            } else {
                formErrorLogin();
                submitMSGRecuperarEmail(false, "El correo electronico no se encuentra asociado a ninguna cuenta");
            }
        }
    });
}




/*Enviar datos a controlador Cliente metodo CAmbiar clave*/
function submitFormCambiarClave() {
    // Initiate Variables With Form Content
    var Clave = $("#txtContraCambiar").val();
    var id = $("#txtIdCliente").val();

    var url = $("#CambiarClaveForm").attr("action");

    $.ajax({
        type: "POST",
        url: url,
        data: {
            contra: Clave, id: id
        },
        success: function (text) {
            if (text == "1") {
                
                formSuccessCambiarClave();
            } else {
                formErrorCambiarClave();
                submitMSGCambiarClave(false, "Error: no se pudo actualizar los datos");
            }
        }
    });
}



function formSuccess() {
    $("#contactForm")[0].reset();
    submitMSG(true, "")
}


/*Existoso para Recuperar cuenta*/
function formSuccessRecuperar() {
    $("#RecuperarEmailForm")[0].reset();
    submitMSGRecuperarEmail(true, "Se ha enviado a su correo electronico una solicitud para reestablecer su clave")
}



/*Existoso para cambiar contraseña*/
function formSuccessCambiarClave() {
    $("#CambiarClaveForm")[0].reset();
    submitMSGCambiarClave(true, "La clave ha sido reestablecida exitosamente. Redireccionandolo a la página principal...");
    window.setTimeout(direccionar_inicio, 5000);
}




/*Existoso para login*/
function formSuccessLogin() {
    $("#LoginForm")[0].reset();
    submitMSGLogin(true, "")
}




function formError() {
    $("#contactForm").removeClass().addClass('shake animated').one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function () {
        $(this).removeClass();
    });
}


/*Error de login*/
function formErrorLogin() {
    $("#LoginForm").removeClass().addClass('shake animated').one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function () {
        $(this).removeClass();
    });
}


/*Error de Registrarte*/
function formErrorRegistrarte() {
    $("#RegistrarmeForm").removeClass().addClass('shake animated').one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function () {
        $(this).removeClass();
    });
}

/*Error de recueprar contraseña*/
function formErrorRecuperar() {
    $("#RecuperarEmailForm").removeClass().addClass('shake animated').one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function () {
        $(this).removeClass();
    });
}


/*Error de recueprar Cambiar clave*/
function formErrorCambiarClave() {
    $("#CambiarClaveForm").removeClass().addClass('shake animated').one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function () {
        $(this).removeClass();
    });
}


/*Mesaje de error de */
function submitMSG(valid, msg) {
    if (valid) {
        var msgClasses = "h3 text-center tada animated text-success";
    } else {
        var msgClasses = "h3 text-center text-danger";
    }
    $("#msgSubmit").removeClass().addClass(msgClasses).text(msg);
}


/*Mesaje de error de Login*/
function submitMSGLogin(valid, msg) {
    if (valid) {
        var msgClasses = "h3 text-center tada animated text-success";
    } else {
        var msgClasses = "h3 text-center text-danger";
    }
    $("#msgSubmitLogin").removeClass().addClass(msgClasses).text(msg);
}


/*Mesaje de error de Registrarte */
function submitMSGRegistrate(valid, msg) {
    if (valid) {
        var msgClasses = "h3 text-center tada animated text-success";
    } else {
        var msgClasses = "h3 text-center text-danger";
    }
    $("#msgSubmitRegistrate").removeClass().addClass(msgClasses).text(msg);
}


/*Mesaje de error de Recuperar Email*/
function submitMSGRecuperarEmail(valid, msg) {
    if (valid) {
        var msgClasses = "h3 text-center tada animated text-success";
    } else {
        var msgClasses = "h3 text-center text-danger";
    }
    $("#msgSubmitREcuperarEmail").removeClass().addClass(msgClasses).text(msg);
}


/*Mesaje de error al cambiar clave*/
function submitMSGCambiarClave(valid, msg) {
    if (valid) {
        var msgClasses = "h3 text-center tada animated text-success";
    } else {
        var msgClasses = "h3 text-center text-danger";
    }
    $("#msgSubmitCambiarClaveForm").removeClass().addClass(msgClasses).text(msg);
}

function direccionar_inicio() {
    $(location).attr('href', 'inicio');
};
