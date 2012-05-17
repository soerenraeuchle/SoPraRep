$(document).ready(function () {

    /* 
    * navigiere durch die verschiedenen formularteile
    */
    $("#button1").click(function () {

        $("#auswahlRolle").css({ "display": "none" });
        $("#agb").css({ "display": "block" });

    });


    $("#agbZurueck").click(function () {
        $("#auswahlRolle").css({ "display": "block" });
        $("#agb").css({ "display": "none" });
    });

    $("#agbWeiter").click(function () {
        $("#agb").css({ "display": "none" });

        var rechte = $("input[name='rechte']:checked").val();

        $("#registerFormular").load("/User/RegisterRolle?rechte=" + rechte, function () {
            $("#registerFormular").css({ "display": "block" });

            $("#registerFormularZurueck").click(function () {
                $("#agb").css({ "display": "block" });
                $("#registerFormular").css({ "display": "none" });
            });
        });

    });


});