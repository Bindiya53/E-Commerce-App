var dataTable;



$(document).ready(function () {

    console.log("test")

    loadDataTable();

})



function loadDataTable() {

    dataTable = $("#tablData").DataTable({

        "ajax": {

            "url": "/Admin/Product/GetAll"

        },

        "columns": [

            { "data": "title", "width": "15%" },

            { "data": "isbn", "width": "15%" },

            { "data": "price", "width": "15%" }

        ]

    });



}


