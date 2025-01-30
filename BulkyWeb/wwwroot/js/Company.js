
var CompanyDataTable;

$(document).ready(function () {
    loadCompanyData();
})


function loadCompanyData() {
    CompanyDataTable = $('#CompanyData').DataTable({
        "ajax": { url: '/Admin/Company/GetData' },
        "columns": [
            {
                "render": function (data, type, full, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                },
                "width": "1%"
            },
            { data: 'name', "width": "25%" },
            { data: 'city', "width": "15%" },
            { data: 'city', "width": "19%" },
            { data: 'postalCode', "width": "10%" },
            { data: 'phoneNumber', "width": "20%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="text-center"><a class="text-light-emphasis" href="/Admin/Company/Create?id=${data}"><i class="bi bi-pencil-square"></i></a></div>`
                },
                "width": "5%",
                "orderable": false
            },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="text-center"><a class="text-danger" onClick=deleteProduct('/Admin/Company/Delete?id=${data}')><i class="bi bi-trash3"></i></a></div>`
                },
                "width": "5%",
                "orderable": false
            }
        ]
    });
}

function deleteProduct(url) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {

            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    CompanyDataTable.ajax.reload();
                    if (data.success) {
                        toastr.success(data.message);
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })

        }
    });
}
