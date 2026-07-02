document.addEventListener("DOMContentLoaded", () => {

    const boton = document.getElementById("btnAsistente");
    const estado = document.getElementById("estadoVoz");

    if (!boton || !estado)
        return;

    const SpeechRecognition =
        window.SpeechRecognition ||
        window.webkitSpeechRecognition;

    if (!SpeechRecognition) {

        estado.style.display = "block";
        estado.innerHTML = "Tu navegador no soporta reconocimiento de voz.";

        return;
    }

    const reconocimiento = new SpeechRecognition();

    reconocimiento.lang = "es-ES";
    reconocimiento.continuous = false;
    reconocimiento.interimResults = false;

    boton.onclick = function () {

        estado.style.display = "block";
        estado.innerHTML = "🎤 Escuchando...";
        boton.classList.add("escuchando");

        reconocimiento.start();

    };

    reconocimiento.onresult = function (event) {

        boton.classList.remove("escuchando");

        let texto = event.results[0][0].transcript.toLowerCase();

        estado.innerHTML = texto;

        ejecutarComando(texto);

        setTimeout(() => {

            estado.style.display = "none";

        }, 3000);

    };

    reconocimiento.onerror = function () {

        boton.classList.remove("escuchando");

        estado.innerHTML = "No pude entender.";

        setTimeout(() => {

            estado.style.display = "none";

        }, 3000);

    };

    function ejecutarComando(texto) {

        if (texto.includes("inicio")) {

            location.href = "/Tienda";

            return;

        }

        if (texto.includes("producto")) {

            location.href = "/Tienda/Productos";

            return;

        }

        if (texto.includes("categor")) {

            location.href = "/Tienda/Categorias";

            return;

        }

        if (texto.includes("Promociones")) {

            location.href = "/Tienda/Promociones";

            return;

        }

        if (texto.includes("contacto")) {

            location.href = "/Tienda/Contacto";

            return;

        }

        if (texto.includes("carrito")) {

            location.href = "/Carrito";

            return;

        }

        if (texto.startsWith("buscar")) {

            let buscar = texto.replace("buscar", "").trim();

            location.href = "/Tienda/Productos?buscar=" + encodeURIComponent(buscar);

            return;

        }

    }

});