$(function () {
    $('#btn-add-desc').on('click', function () {
        debugger;
        var getValue = $('#txt-gist-description').val().trim();
        if (getValue) {

            var discriptionData = {
                "description": getValue,
                "public": true,
                "files": {
                    "devendra.txt": {
                        "content": getValue
                    }
                }
            };

            $.ajax({
                url: 'https://api.github.com/gists',
                data: JSON.stringify(discriptionData),
                type: 'POST',
                headers: {
                    'Authorization': authHeader
                },
                success: function (result) {
                    $('#txt-gist-description').val('');
                    alert('Description added successfully.');
                    location.reload();
                 

                },
                error: function (result) {
                    alert('Description could not be added.');
                }
            })
        } else {
            alert('Please enter description.')
        }
    });

});

var clientSideDataTableConfigure = {
    "serverSide": false,
    "orderCellsTop": true,
    "processing": true,
    "autoWidth": false,
    "lengthChange": true,
    "language": {
        "zeroRecords": "No record found.",
        //"processing": '<img src="' + loaderImgPath + '">',
        "infoFiltered": " ",
        "searchPlaceholder": "Search records"

    },
    "columnDefs": [{
        "targets": 'no-sort',
        "orderable": false
    },
    {
        "targets": 'no-search',
        "searchable": false
    }, {
        "targets": 'numric',
        "type": "num"
    }, {
        "targets": 'col-hide',
        "visible": false,
    }],
    "lengthMenu": [[10, 20, 50, -1], [10, 20, 50, "All"]]
};


var descriptionList = {
    dt: null,
    init: function () {
        $('#tbl-description-list').css("width", "100%");
        clientSideDataTableConfigure.order = [
            [3, 'desc']
        ];
        dt = $('#tbl-description-list').DataTable(clientSideDataTableConfigure);
        dt.on('order.dt search.dt', function () {
            dt.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                cell.innerHTML = i + 1;
            });
        }).draw();
        $('#tbl-description-list #filter-row th').each(function () {
            var column = this;
            var title = $(this).text();
            var getdatatype = $(this).attr('data-type');
            if (getdatatype !== undefined && (getdatatype === 'action')) { } else {
                $(this).html('<input type="text" onclick="stopPropagation(event);" class="form-control txt-searchable input-sm" placeholder="' + title + '" />');
            }
            dt.columns().eq(0).each(function (index) {
                $('#tbl-description-list thead tr:eq(1) th:eq(' + index + ') input.txt-searchable').on('keyup change', function () {
                    dt.column($(this).parent().index() + ':visible')
                        .search(this.value)
                        .draw();
                });
            });
        });
    }
};
