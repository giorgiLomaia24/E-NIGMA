
$(document).ready(function () {
    var dataTable = null; // Initialize the dataTable variable

    loadDataTable(); // Load the initial DataTable

    var url = window.location.search;
    if (url.includes("pending")) {
        loadDataTable("pending");
    } else if (url.includes("inprocess")) {
        loadDataTable("inprocess");
    } else if (url.includes("completed")) {
        loadDataTable("completed");
    } else if (url.includes("approved")) {
        loadDataTable("approved");
    } else {
        loadDataTable("all");
    }

    function loadDataTable(status) {
        if (dataTable) {
            // If a DataTable instance already exists, destroy it
            dataTable.destroy();
        }

        dataTable = $('#tblData').DataTable({
            "ajax": '/Admin/order/getall?status=' + status,
            "columns": [
                { data: 'id', "width": "15%" },
                { data: 'name' },
                { data: 'phoneNumber' },
                { data: 'applicationUser.email' },
                { data: 'orderStatus' },
                { data: 'orderTotal' },
                {
                    data: 'id',
                    "render": function (data) {
                        return `<div class="w-75 btn-group" role="group">  
                        <a href="/Admin/order/details?orderId=${data}" class="btn btn-info">Details</a>
 
                                </div>`;
                    }
                }
            ]
        });
    }
});
