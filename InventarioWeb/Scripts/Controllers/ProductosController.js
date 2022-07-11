angular.module('AppInventario', [])
    .controller('ProductosController', function ($http, $scope) {
        $http.get("api/Proveedores")
            .then(function (response) {
                // First function handles success
                $scope.proveedores = response.data;
            }, function (response) {
                // Second function handles error
                $scope.content = "Something went wrong";
            });
        $http.get("api/Almacen")
            .then(function (response) {
                // First function handles success
                $scope.almacenes = response.data;
            }, function (response) {
                // Second function handles error
                $scope.content = "Something went wrong";
            });
        $scope.nuevaAlta = function () {
            // Fetch all the forms we want to apply custom Bootstrap validation styles to
            var forms = document.getElementsByClassName('needs-validation');
            // Loop over them and prevent submission
            Array.prototype.filter.call(forms, function (form) {
                if (form.checkValidity() === true) {
                    alert(JSON.stringify($scope.producto));
                }
            });
        }
        $scope.editarItem = function (idNegocio, idItem) {
            var element = document.getElementById("example1");

        }
    });