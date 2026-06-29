document.addEventListener("DOMContentLoaded", function () {

    document.querySelectorAll(".agregar-carrito").forEach(function (btn) {

        btn.addEventListener("click", function () {

            let id = this.dataset.id;

            fetch("/Carrito/Agregar", {
                method: "POST",
                headers: {
                    "Content-Type": "application/x-www-form-urlencoded"
                },
                body: `id=${id}&cantidad=1`
            })
                .then(r => r.json())
                .then(data => {

                    if (data.login) {

                        window.location = "/CuentaCliente/Login";
                        return;
                    }

                    if (data.ok) {

                        let contador = document.getElementById("contadorCarrito");

                        if (contador)
                            contador.innerText = data.total;

                        Swal.fire({
                            icon: "success",
                            title: "Producto agregado",
                            timer: 1200,
                            showConfirmButton: false
                        });
                    }
                    else {

                        Swal.fire(
                            "Error",
                            "No se pudo agregar el producto.",
                            "error"
                        );
                    }

                });

        });

    });

});