(function ($) {

	$('#listBranch').change(function () {
		$.ajax({
			type: "POST",
			url: "/Manager/Tables/ajaxRooms",
			data: { "branch": $('#listBranch').find(":selected").val() },
			success: function (response) {
				var html = "<option>Choose Room</option>";
				$.each(response, function (index, item) {
					html += '<option value="' + item.id + '">' + item.name + '</option>';

				});
				$('#listRoom').html(html);
			},
			failure: function (response) {
				alert(response.responseText);
			},
			error: function (response) {
				alert(response.responseText);
			}
		});
	});

})(jQuery);


$(document).ready(function () {
	$.ajax({
		type: "POST",
		url: "/Manager/Tables/ajaxRooms",
		data: { "branch": $('#listBranch').find(":selected").val() },
		success: function (response) {
			var html = "<option>Choose Room</option>";
			$.each(response, function (index, item) {
				html += '<option value="' + item.id + '">' + item.name + '</option>';

			});
			$('#listRoom').html(html);
		},
		failure: function (response) {
			alert(response.responseText);
		},
		error: function (response) {
			alert(response.responseText);
		}
	});
});