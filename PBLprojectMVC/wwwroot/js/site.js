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

function displayImage() {
    var oFReader = new FileReader();
    oFReader.readAsDataURL(document.getElementById("Image").files[0]);
    oFReader.onload = function (oFREvent) {
        document.getElementById("imgPreview").src = oFREvent.target.result;
    };
}

function applyAdvancedQueryDevice() {
    var name = $("#name").val();
    var type = $("#type").val();
    var transport = $("#transport").val();
    $.ajax({
        url: "/device/GetDevicesPartial",
        data: { name: name, type: type, transport: transport },
        success: function (data) {
            if (data.error != undefined) {
                alert(data.msg);
            }
            else {
                document.getElementById('queryResult').innerHTML = data;
            }
        },
        error: function (error) {
            console.error('Error fetching partial view:', error);
        }
    });
}

function applyAdvancedQueryTemperature() {
    var deviceName = $("#deviceName").val();
    var hLimit = $("#hLimit").val();
    var hOffset = $("#offSet").val();
    var dateFrom = $("#dateFrom").val();
    var dateTo = $("#dateTo").val();
    $.ajax({
        url: "/temperature/GetTemperaturePartial",
        data: { deviceName: deviceName, hLimit: hLimit, hOffset: hOffset, dateFrom: dateFrom, dateTo: dateTo },
        success: function (data) {
            if (data.error != undefined) {
                alert(data.msg);
            }
            else {
                document.getElementById('queryResultTemp').innerHTML = data;
            }
        },
        error: function (error) {
            console.error('Error fetching partial view:', error);
        }
    });
}