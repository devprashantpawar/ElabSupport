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
                html += '<thead  class="table-active">'
                html += '<tr>'
                html += '<th scope="col">Id</th>'
                html += '<th scope="col">Names</th>'
                html += '<th scope="col">User Role</th>'
                html += '<th scope="col">email</th>'
                html += '<th scope="col">Phone Number</th>'
                html += '</tr>'
                html += '</thead>'
                html += '<tbody>'

                if (obj.response) {
                    let tblData = obj.response
                    tblData.forEach((item) => {
                        html += '<tr>'
                        html += '<td>' + item.data.devices[0].id+'</td>'
                        html += '<td>' + item.data.first_name + ' ' + item.data.last_name +'</td>'
                        html += '<td>' + item.data.role+'</td>'
                        html += '<td>' + item.data.email +'</td>'
                        html += '<td>' + item.data.devices[0].id +'</td>'
                        html += '</tr>'
                    });
                }
                html += '</tbody>'

                $(".tblExotel").html(html)
            }
        },
    });

    // SHOW CHART
    const ctx = document.getElementById("workingHoursChart");
    new Chart(ctx, {
        type: "doughnut",
        data: {
            labels: ["Hours working"],
            datasets: [{
                label: "Working Hours",
                data: [50],
                backgroundColor: "rgb(12, 62, 147)"
            }]
        },
        options: {
            responsive: false,
            maintainAspectRatio: false
        }
    });

});