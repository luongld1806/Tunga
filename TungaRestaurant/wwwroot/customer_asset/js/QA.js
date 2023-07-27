$(document).ready(function () {
    $("#minus").click(function () {
        var m = parseInt($("#quantity").val())-1;
        $("#quantity").val(m);
    });

    $("#plus").click(function () {
        var p = parseInt($("#quantity").val())+1;
        $("#quantity").val(p);
    });
})


