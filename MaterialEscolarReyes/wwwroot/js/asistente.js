const boton = document.getElementById("btnAsistente");

const estado = document.getElementById("estadoVoz");

console.log("Asistente cargado");
// ===============================
// ASISTENTE DE VOZ
// ===============================

const boton = document.getElementById("btnAsistente");
const estado = document.getElementById("estadoVoz");

if ('webkitSpeechRecognition' in window || 'SpeechRecognition' in window) {

    const SpeechRecognition = window.SpeechRecognition || window.webkitSpeechRecognition;

    const reconocimiento = new SpeechRecognition();

    reconocimiento.lang = "es-ES";
    reconocimiento.continuous = false;
    reconocimiento.interimResults = false;

    boton.addEventListener("click", () => {

        estado.style.display = "block";
        estado.innerHTML = "🎤 Escuchando...";
        boton.classList.add("escuchando");

        reconocimiento.start();

    });

    reconocimiento.onresult = function (event) {

        let texto = event.results[0][0].transcript.toLowerCase();

        estado.innerHTML = "✔ " + texto;

        ejecutarComando(texto);

    };

    reconocimiento.onerror = function () {

        estado.innerHTML = "❌ No se entendió";

        boton.classList.remove("escuchando");

        setTimeout(() => {

            estado.style.display = "none";

        }, 2000);

    };

    reconocimiento.onend = function () {

        boton.classList.remove("escuchando");

        setTimeout(() => {

            estado.style.display = "none";

        }, 2000);

    };

}
else {

    alert("Este navegador no soporta reconocimiento de voz.");

}



// ===============================
// COMANDOS
// ===============================

function ejecutarComando(texto) {

    // INICIO
    if (texto.includes("inicio")) {

        window.location = "/Tienda";

        return;

    }

    // PRODUCTOS
    if (texto.includes("producto")) {

        window.location = "/Tienda/Productos";

        return;

    }

    // CATEGORÍAS
    if (texto.includes("categoría") || texto.includes("categorias")) {

        window.location = "/Tienda/Categorias";

        return;

    }

    // OFERTAS
    if (texto.includes("oferta")) {

        window.location = "/Tienda/Ofertas";

        return;

    }

    // CONTACTO
    if (texto.includes("contacto")) {

        window.location = "/Tienda/Contacto";

        return;

    }

    // CARRITO
    if (texto.includes("carrito")) {

        window.location = "/Carrito";

        return;

    }

    // LOGIN
    if (texto.includes("iniciar sesión") || texto.includes("mi cuenta")) {

        window.location = "/CuentaCliente/Login";

        return;

    }

    // BUSCAR
    if (texto.startsWith("buscar ")) {

        let buscar = texto.replace("buscar ", "");

        window.location = "/Tienda/Productos?buscar=" + encodeURIComponent(buscar);

        return;

    }

    estado.innerHTML = "No conozco ese comando.";

}