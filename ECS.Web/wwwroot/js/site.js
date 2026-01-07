// Auto-dismiss alerts after 5 seconds
$(document).ready(function () {
    setTimeout(function () {
        $('.alert').fadeOut('slow');
    }, 5000);
});

// DataTables default configuration
$.extend(true, $.fn.dataTable.defaults, {
    responsive: true,
    pageLength: 25,
    language: {
        search: "_INPUT_",
        searchPlaceholder: "Search...",
        lengthMenu: "Show _MENU_ entries"
    }
});

// Confirmation dialog helper
function confirmAction(message) {
    return confirm(message || 'Are you sure?');
}

// AJAX helper with error handling
function ajaxRequest(url, method, data, successCallback) {
    $.ajax({
        url: url,
        type: method,
        data: data,
        success: function (result) {
            if (result.success) {
                if (successCallback) {
                    successCallback(result);
                }
            } else {
                alert(result.message || 'Operation failed');
            }
        },
        error: function (xhr, status, error) {
            alert('Error: ' + error);
        }
    });
}