$.validator.setDefaults({
    errorClass: "",
    validClass: "",
    highlight: function (element, errorClass, validClass) {
        $(element).addClass("is-invalid").removeClass("is-valid");
        $(element.form).find("[data-valmsg-for=" + element.id + "]").addClass("alert background-warning").show();
    },

    unhighlight: function (element, errorClass, validClass) {
        $(element).addClass("is-valid").removeClass("is-invalid");
        $(element.form).find("[data-valmsg-for=" + element.id + "]").removeClass("alert background-warning").hide();
    },
});