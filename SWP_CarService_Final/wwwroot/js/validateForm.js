$(document).ready(function () {
    $.validator.addMethod("lettersonly", function (value, element) {
        return this.optional(element) || /^[a-zA-Z]*$/.test(value);
    }, "Only letters are allowed.");

    $.validator.addMethod("positiveNumber", function (value, element) {
        return Number(value) >= 0;
    }, "Please enter a positive number.");

    $("#login").validate({
        onfocusout: true,
        onkeyup: true,
        onclick: true,
        rules: {
            "userName": {
                required: true,
            },
            "password": {
                required: true,
            },
        }
    });

    $("#register").validate({
        onfocusout: true,
        onkeyup: true,
        onclick: true,
        rules: {
            "user_name": {
                required: true,
                lettersonly: true
            },
            "fullName": {
                required: true,
                lettersonly: true
            },
            "password": {
                required: true,
            },
            "confirm_password": {
                required: true,
                equalTo: "#password"
            },
            "email": {
                required: true,
                email: true
            },
            "phone_number": {
                required: true,
                digits: true,
                minlength: 10
            },
        },
        messages: {
            "confirm_password": {
                equalTo: "Password and confirm password must match."
            },
            "phone_number": {
                minlength: "Phone number must be at least 10 digits.",
                digits: "Only numbers are allowed."
            }
        }
    });

    $("#createCustomer").validate({
        onfocusout: true,
        onkeyup: true,
        onclick: true,
        rules: {
            "user_name": {
                required: true,
                lettersonly: true
            },
            "fullName": {
                required: true,
                lettersonly: true
            },
            "password": {
                required: true,
            },
            "email": {
                required: true,
                email: true
            },
            "phone_number": {
                required: true,
                digits: true,
                minlength: 10
            },
        },
        messages: {
            "email": {
                email: "Please enter a valid email address.",
            },
            "phone_number": {
                minlength: "Phone number must be at least 10 digits.",
                digits: "Only numbers are allowed."
            }
        }
    });

    $("#editCustomer").validate({
        onfocusout: true,
        onkeyup: true,
        onclick: true,
        rules: {
            "fullname": {
                required: true,
                lettersonly: true
            },
            "email": {
                required: true,
                email: true
            }, "phone_number": {
                required: true,
                digits: true,
                minlength: 10
            },
        },
        messages: {
            "email": {
                email: "Please enter a valid email address.",
            },
            "phone_number": {
                minlength: "Phone number must be at least 10 digits.",
                digits: "Only numbers are allowed."
            }
        }
    });

    $("#editProfile").validate({
        onfocusout: true,
        onkeyup: true,
        onclick: true,
        rules: {
            "fullname": {
                required: true,
                lettersonly: true
            },
            "email": {
                required: true,
                email: true,
            },
            "phone_number": {
                required: true,
                digits: true,
                minlength: 10
            },
        },
        messages: {
            "email": {
                email: "Please enter a valid email address.",
            },
            "phone_number": {
                minlength: "Phone number must be at least 10 digits.",
                digits: "Only numbers are allowed."
            }
        }
    });

    $("#editProfileUser").validate({
        onfocusout: true,
        onkeyup: true,
        onclick: true,
        rules: {
            "User_fullname": {
                required: true,
                lettersonly: true
            },
            "email": {
                required: true,
                email: true,
            },
            "phone_number": {
                required: true,
                digits: true,
                minlength: 10
            },
        },
        messages: {
            "email": {
                email: "Please enter a valid email address.",
            },
            "phone_number": {
                minlength: "Phone number must be at least 10 digits.",
                digits: "Only numbers are allowed."
            }
        }
    });

    $("#editTeam").validate({
        onfocusout: true,
        onkeyup: true,
        onclick: true,
        rules: {
            "teamName": {
                required: true,
                lettersonly: true
            },
        },
    });

    $("#createTeam").validate({
        onfocusout: true,
        onkeyup: true,
        onclick: true,
        rules: {
            "teamName": {
                required: true,
                lettersonly: true
            },
        },
    });
  
    $("#editTaskDetail").validate({
        onfocusout: true,
        onkeyup: true,
        onclick: true,
        rules: {
            "quantity": {
                required: true,
                number: true,
                positiveNumber: true
            },
        },
        messages: {
            "quantity": {
                positiveNumber: "Please enter a positive number.",
                number: "Please enter a number."
            }
        }
    });
});