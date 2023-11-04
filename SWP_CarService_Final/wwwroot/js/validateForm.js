$(document).ready(function () {
    function textOnly(value, element) {
        return this.optional(element) || /^[a-zA-Z\s\u00C0-\u024F]+$/.test(value);
    }

    $.validator.addMethod("textonly", textOnly, "Only text characters are allowed.");

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
                textonly: true
            },
            "fullName": {
                required: true,
                textonly: true
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
                textonly: true
            },
            "fullName": {
                required: true,
                textonly: true
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
                textonly: true
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
                textonly: true
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
                textonly: true
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
                textonly: true
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
                textonly: true
            },
        },
    });

    $("#addCategory").validate({
        onfocusout: true,
        onkeyup: true,
        onclick: true,
        rules: {
            "name": {
                required: true,
                textonly: true
            },
        },
    });

    $("#editCategory").validate({
        onfocusout: true,
        onkeyup: true,
        onclick: true,
        rules: {
            "name": {
                required: true,
                textonly: true
            },
        },
    });

    $("#addComponent").validate({
        onfocusout: true,
        onkeyup: true,
        onclick: true,
        rules: {
            "name": {
                required: true,
                textonly: true
            },
            "price": {
                required: true,
                number: true,
                positiveNumber: true,
                min: 1
            },
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

    $("#editComponent").validate({
        onfocusout: true,
        onkeyup: true,
        onclick: true,
        rules: {
            "name": {
                required: true,
                textonly: true
            },
            "price": {
                required: true,
                number: true,
                positiveNumber: true,
                min: 1
            },
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

    $("#createUserAcccount").validate({
        onfocusout: true,
        onkeyup: true,
        onclick: true,
        rules: {
            "username": {
                required: true,
            },
            "password": {
                required: true,
            },
            "fullname": {
                required: true,
                textonly: true,
            },
            "email": {
                required: true,
                email: true,
            },
            "phonenumber": {
                required: true,
                digits: true,
                minlength: 10,
                maxlength: 11,
            },
        },
        messages: {
            "username": {
                required: "Please enter your username.",
            },
            "password": {
                required: "Please enter your password.",
            },
            "email": {
                required: "Please enter your email.",
                email: "Please enter a valid email address.",
            },
            "phonenumber": {
                required: "Please enter your phone number.",
                minlength: "Phone number must be at least 10 digits.",
                maxlength: "Phone number must not exceed 11 digits",
                digits: "Only numbers are allowed."
            },
            "fullname": {
                required: "Please enter your fullname.",
            },
        }
    });

    $("#editUserAccount").validate({
        onfocusout: true,
        onkeyup: true,
        onclick: true,
        rules: {
            "password": {
                required: true,
            },
            "fullname": {
                required: true,
                textonly: true,
            },
            "email": {
                required: true,
                email: true,
            },
            "phonenumber": {
                required: true,
                digits: true,
                minlength: 10,
                maxlength: 11,
            },
        },
        messages: {
            "password": {
                required: "Please enter your password.",
            },
            "email": {
                required: "Please enter your email.",
                email: "Please enter a valid email address.",
            },
            "phonenumber": {
                required: "Please enter your phone number.",
                minlength: "Phone number must be at least 10 digits.",
                maxlength: "Phone number must not exceed 11 digits",
                digits: "Only numbers are allowed."
            },
            "fullname": {
                required: "Please enter your fullname.",
            },
        },
    });

    $("#editService").validate({
        onfocusout: true,
        onkeyup: true,
        onclick: true,
        rules: {
            "ServiceName": {
                required: true,
            },
            "Price": {
                required: true,
                positiveNumber: true,
                min: 1,
            },
            "description": {
                required: true,
            },
        },
        messages: {
            "ServiceName": {
                required: "Please enter the Service Name",
            },
            "Price": {
                required: "Please enter the Price.",
            },
            "description": {
                required: "Please enter the Description.",
            },
        },
    });

    $("#addService").validate({
        onfocusout: true,
        onkeyup: true,
        onclick: true,
        rules: {
            "ServiceName": {
                required: true,
            },
            "Price": {
                required: true,
                positiveNumber: true,
                min: 1,
            },
            "description": {
                required: true,
            },
        },
        messages: {
            "ServiceName": {
                required: "Please enter the Service Name",
            },
            "Price": {
                required: "Please enter the Price.",
            },
            "description": {
                required: "Please enter the Description.",
            },
        },
    });

});