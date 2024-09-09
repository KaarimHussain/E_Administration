$(document).ready(function () {
    var SelectLab = $('#SelectLab');
    var selectedLabId = $(SelectLab).val();
    fetchLabs(selectedLabId);
    $(SelectLab).on('change', function () {
        console.log("Called");
        fetchLabs(selectedLabId);
    })
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
                        responseBody.html(`
                            <div class="col-lg-4 col-md-6 col-sm-12 col-12 mb-4">
                                <div class="border border-3 rounded-4 p-3">
                                    <small class="text-black-50">
                                            ${data.PcName}
                                    </small>
                                    <h1 class="fw-bold font-body">
                                            Lab 1
                                    </h1>
                                    <small class="text-black">
                                          <i class="bi bi-pc-display" ></i> 29 PC
                                    </small>
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
    function getPcCount(labId) {
        $.ajax({
            url: '/Admins/GetPcCount',
            type: 'GET',
            data: { labId: labId },
            success: function (data) {
                if (data.length > 0) {
                    return data;
                }
                else {
                    return "No Pc Found"
                }
            },
            error: function (xhr, status, error) {
                console.error('Error fetching data:', error);
            }
        })
    }
});