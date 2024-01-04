$(document).ready(function () {
    let html = "";

    $.ajax({
        cache: false,
        type: "GET",
        url: "../Exotel/GetExotelUserData",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        // headers: {'X-Requested-With': 'XMLHttpRequest'},
        success: function (obj) {
            if (obj != null) {
                html += '<thead class="table-active">';
                html += '<tr>';
                html += '<th class="w-20" scope="col">Id</th>';
                html += '<th class="w-20" scope="col">Names</th>';
                html += '<th class="w-20" scope="col">User Role</th>';
                html += '<th class="w-20" scope="col">email</th>';
                html += '<th class="w-20" scope="col">Phone Number</th>';
                html += '<th class="w-20" scope="col">Status</th>';
                html += '</tr>';
                html += '</thead>';
                html += '<tbody>';

                if (obj.response) {
                    let tblData = obj.response;
                    tblData.forEach((item) => {
                        html += '<tr>';
                        html += '<td class="w-20">' + item.data.devices[0].id + '</td>';
                        html += '<td class="w-20">' + item.data.first_name + ' ' + item.data.last_name + '</td>';
                        html += '<td class="w-20">' + item.data.role + '</td>';
                        html += '<td class="w-20">' + item.data.email + '</td>';
                        html += '<td class="w-20">' + item.data.devices[0].contact_uri + '</td>';
                        html += '<td class="w-20">' + (item.data.devices[0].available ?
                            '<span class="statusActive">Active</span>' :
                            '<span class="statusDeactive">Deactive</span>') + '</td>';
                        html += '</tr>';
                    });
                }
                html += '</tbody>';

                $(".tblExotel").html(html);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.error("AJAX request failed: " + textStatus, errorThrown);
            console.log("Response: ", jqXHR.responseText);
        }
    });

    // SHOW CHART
    const ctx = document.getElementById("workingHoursChart");
    new Chart(ctx, {
        type: "doughnut",
        data: {
            labels: ["Hours working"],
            datasets: [{
                label: "Working Hours",
                data: [70, 30], // Adjust the values to represent the desired percentages
                backgroundColor: ["rgb(12, 62, 147)", "white"]
            }]
        },
        options: {
            responsive: false,
            maintainAspectRatio: false,
            plugins: {
                beforeDraw: function (chart) {
                    var width = chart.chart.width,
                        height = chart.chart.height,
                        ctx = chart.chart.ctx;

                    ctx.restore();
                    var fontSize = (height / 114).toFixed(2);
                    ctx.font = fontSize + "em sans-serif";
                    ctx.textBaseline = "middle";

                    var text = "70%"; // Your text here
                    var textX = Math.round((width - ctx.measureText(text).width) / 2);
                    var textY = height / 2;

                    ctx.fillText(text, textX, textY);
                    ctx.save();
                }
            }
        }
    });

});