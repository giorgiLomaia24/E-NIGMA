$(document).ready(function () {
    loadDataTable();
    
    // Attach click event to delete buttons
    $(document).on('click', '.btn-danger', function () {
        var form = $(this).closest("form");
        var formAction = form.attr("action");
        var formId = form.find('input[name="id"]').val();
        var modalAction = formAction + "?id=" + formId;
  
        showDeleteModal(modalAction);
    });
  
    function showDeleteModal(action) {
        $('#confirmDeleteBtn').data("modal-action", action);
        $('#deleteModal').modal('show');
    }
  
    // Attach click event to confirm delete button
    $('#confirmDeleteBtn').click(function () {
        var action = $(this).data("modal-action");
        if (action) {
            // Prevent default form submission
            event.preventDefault();
  
            // Make an API call to delete the item
            $.ajax({
                url: action,
                type: 'DELETE',
                success: function (response) {
                    // Handle the success response
                    if (response.success) {
                        // Reload the data table or perform any other action
                        dataTable.ajax.reload();
                    } else {
                        // Handle the error response
                        alert("Error: " + response.message);
                    }
                },
                error: function () {
                    // Handle errors
                    alert("Error: Something went wrong");
                }
            });
            
            // Hide the modal
            $('#deleteModal').modal('hide');
        }
    });
  });

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": '/Admin/product/getall',
        "columns": [
            { data: 'title', "width": "15%" },
            { data: 'isbn' },
            { data: 'author' },
            { data: 'price' },
            { data: 'category.name' },
            {
                data: 'id',
                "render": function(data) {
                    return `<div class="w-75 btn-group" role="group">   
                            <a href="/Admin/product/upsert?id=${data}" class="btn btn-info" style="cursor: pointer;">EDIT</a>
                            <button type="button" class="btn btn-danger">Delete</button>
                            </div>`;
                }
            }
        ]
    });
}
