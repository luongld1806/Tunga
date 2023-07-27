var host = window.location.origin;

$(".redirect-on-click").on("click", function () {
    var redirectUrl = $(this).attr("href");
    
    window.location.href = host + "/" + redirectUrl;
})

$(".image-input-update").on("change", function () {
    var file = this.files[0];
    console.log(file);
    var imgSrc = window.URL.createObjectURL(file);
    var targetId= $(this).attr("target-id");
    $("#" + targetId).attr("src", imgSrc);
})

