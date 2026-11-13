// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    // Configurações específicas para a tabela de contatos
    // Trecho ajustado no JavaScript (site.js)
    var configContatos = {
        columnDefs: [
            {
                targets: 3, // Índice 3 corresponde à 4ª coluna (Celular)
                defaultContent: "",
                render: function (data, type, row) {
                    if (type === 'display' && data) {
                        var num = String(data).replace(/\D/g, '');
                        if (num.length === 11) {
                            return num.replace(/^(\d{2})(\d{5})(\d{4})$/, '($1) $2-$3');
                        } else if (num.length === 10) {
                            return num.replace(/^(\d{2})(\d{4})(\d{4})$/, '($1) $2-$3');
                        }
                    }
                    return data;
                }
            }
        ]
    };

    // Configurações específicas para a tabela de usuários
    // Altere ou adicione as colunas conforme a realidade da tabela de usuários
    var configUsuarios = {
        columnDefs: [
            {
                targets: '_all', // Aplica a todas as colunas para evitar o erro de dados nulos
                defaultContent: ""
            }
            // Se a tabela de usuários também tiver botões de ação em outro índice (ex: coluna 3), mapeie aqui:
            // { targets: 3, orderable: false, searching: false }
        ]
    };

    // Inicializa passando as configurações customizadas
    getDataTable('#table-contatos', configContatos);
    getDataTable('#table-usuarios', configUsuarios);

    // Use $(document).on para funcionar mesmo após paginar ou pesquisar no DataTable
    $(document).on('click', '.btn-total-contatos', function () {
        var usuarioId = $(this).attr('usuario-id');

        $.ajax({
            type: "GET",
            url: '/Usuario/ListarContatosPorUsuarioId/' + usuarioId,
            success: function (result) {
                $("#listaContatosUsuario").html(result);
                $("#modalContatosUsuario").modal();
                getDataTable('#table-contatos-usuario');
            }
        });

        $('#modalContatosUsuario').modal('show');
    });
});

// A função agora aceita um objeto 'customConfig'
function getDataTable(id, customConfig) {
    // Configurações padrão que valem para QUALQUER tabela do seu sistema
    var baseConfig = {
        ordering: true,
        paging: true,
        searching: true,
        language: {
            emptyTable: "Nenhum registro encontrado na tabela",
            info: "Mostrar _START_ até _END_ de _TOTAL_ registros",
            infoEmpty: "Mostrar 0 até 0 de 0 Registros",
            infoFiltered: "(Filtrar de _MAX_ total registros)",
            infoPostFix: "",
            thousands: ".",
            lengthMenu: "Mostrar _MENU_ registros por página",
            loadingRecords: "Carregando...",
            processing: "Processando...",
            zeroRecords: "Nenhum registro encontrado",
            search: "Pesquisar:",
            paginate: {
                next: "Próximo",
                previous: "Anterior",
                first: "Primeiro",
                last: "Último"
            },
            aria: {
                orderable: ": Ordenar colunas de forma ascendente",
                orderableReverse: ": Ordenar colunas de forma descendente"
            }
        }
    };

    // Une as configurações globais com as específicas de cada tabela
    var finalConfig = $.extend(true, {}, baseConfig, customConfig);

    $(id).DataTable(finalConfig);
}


$('.close-alert').click(function () {
    $('alert').hide('hide');
});