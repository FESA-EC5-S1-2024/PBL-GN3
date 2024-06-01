function deleteRecord(table, id) {
    Swal.fire({
        title: 'Confirma a exclusão do registro?',
        text: "Se você excluir, não será possível recuperar este registro!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Sim, exclua!',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            location.href = table + '/Delete?id=' + id;
        } else {
            Swal.fire(
                'Cancelado',
                'Seu registro está seguro :)',
                'info'
            );
        }
    });
}