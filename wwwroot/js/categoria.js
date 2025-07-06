// Variable global para almacenar la instancia de DataTable
var dataTable;

// Cuando el documento está listo, inicializa la tabla
$(document).ready(function () {
    cargarDatatable();
});

/**
 * Inicializa y configura la tabla de categorías utilizando DataTables
 * Realiza una petición AJAX para obtener los datos y configura las columnas
 */
function cargarDatatable() {
    dataTable = $("#tblCategorias").DataTable({
        "ajax": {
            "url": "/admin/categorias/GetAll",  // URL para obtener todas las categorías
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            // Columna ID - 5% del ancho
            { "data": "id", "width": "5%" },
            // Columna Nombre - 40% del ancho
            { "data": "nombre", "width": "40%" },
            // Columna Orden - 10% del ancho
            { "data": "orden", "width": "10%" },
            {
                // Columna de acciones (Editar/Borrar) - 40% del ancho
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                                <a href="/Admin/Categorias/Edit/${data}" class="btn btn-success text-white" style="cursor:pointer; width:140px;">
                                <i class="far fa-edit"></i> Editar
                                </a>
                                &nbsp;
                                <a onclick=Delete("/Admin/Categorias/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer; width:140px;">
                                <i class="far fa-trash-alt"></i> Borrar
                                </a>
                          </div>
                         `;
                }, "width": "40%"
            }
        ],
        // Configuración de idioma español para DataTables
        "language": {
            "decimal": "",
            "emptyTable": "No hay registros",
            "info": "Mostrando _START_ a _END_ de _TOTAL_ Entradas",
            "infoEmpty": "Mostrando 0 to 0 of 0 Entradas",
            "infoFiltered": "(Filtrado de _MAX_ total entradas)",
            "infoPostFix": "",
            "thousands": ",",
            "lengthMenu": "Mostrar _MENU_ Entradas",
            "loadingRecords": "Cargando...",
            "processing": "Procesando...",
            "search": "Buscar:",
            "zeroRecords": "Sin resultados encontrados",
            "paginate": {
                "first": "Primero",
                "last": "Ultimo",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        },
        "width": "100%"
    });
}

/**
 * Función para eliminar una categoría
 * @param {string} url - URL para la acción de eliminar
 * Muestra un diálogo de confirmación usando SweetAlert
 * Si se confirma, realiza una petición AJAX DELETE
 */
function Delete(url) {
    swal({
        title: "Esta seguro de borrar?",
        text: "Este contenido no se puede recuperar!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Si, borrar!",
        closeOnconfirm: true
    }, function () {
        // Petición AJAX para eliminar la categoría
        $.ajax({
            type: 'DELETE',
            url: url,
            success: function (data) {
                if (data.success) {
                    // Muestra mensaje de éxito y actualiza la tabla
                    toastr.success(data.message);
                    dataTable.ajax.reload();
                }
                else {
                    // Muestra mensaje de error si falla
                    toastr.error(data.message);
                }
            }
        });
    });
}