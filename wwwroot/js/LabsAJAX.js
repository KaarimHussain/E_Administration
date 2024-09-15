$(document).ready(function () {
    var SelectLab = $('#SelectLab');
    var selectedLabId = $(SelectLab).val();
    console.log(selectedLabId);
    fetchLabs(selectedLabId);

    $(SelectLab).on('input', function () {
        var newSelectLab = $('#SelectLab');
        var newselectedLabId = $(newSelectLab).val();
        console.log("Called" + newselectedLabId);
        fetchLabs(newselectedLabId);
    });

    function fetchLabs(floorId) {
        $.ajax({
            url: '/Admins/GetLabs',
            type: 'GET',
            data: { floorId: floorId },
            success: function (data) {
                var responseBody = $('#lab-response-container');
                $(responseBody).empty();

                if (data.length > 0) {
                    $.each(data, function (index, lab) {
                        console.log("INDEX: " + index, "Lab ID: " + lab);
                        // Append the HTML to the responseBody
                        $(responseBody).append(`
                            <div class="col-lg-4 col-md-6 col-sm-12 col-12 mb-4">
                                <div class="border border-3 rounded-4 p-3">
                                    <h1 class="fw-bold font-body">
                                        Lab ${index + 1}
                                    </h1>
                                </div>
                            </div>
                        `);
                    });
                } else {
                    responseBody.html("<h4 class='text-center text-black-50 fw-bold font-body'>No Labs Found</h4>");
                }
            },
            error: function (xhr, status, error) {
                console.error('Error fetching data:', error);
            }
        });
    }
});
