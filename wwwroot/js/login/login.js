const baseURL = window.location.origin;

async function login() {
    const email = document.getElementById('emailLogin').value;
    const password = document.getElementById('passwordLogin').value;

    return await postLogin(email, password);
}
document.getElementById('formLogin').addEventListener("submit", async function (event) {
    event.preventDefault();

    changeLogin();

    const request = await login();

    if (request) {
        alert("Login realizado.");
    }
    else {
        alert("No se ha completado el logueo");
    }

    changeLogin();
})

function changeLogin() {
    const button = document.getElementById('submitLogin');
    if (button.disabled) {
        button.disabled = false;
        button.classList.add('enabled');
        button.classList.remove('disabled');
    } else {
        button.disabled = true;
        button.classList.add('disabled');
        button.classList.remove('enabled');
    }
}

async function postLogin(email, password) {
    try {
        const response = await fetch(baseURL + '/Login/Authenticate', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                email: email,
                password: password
            })
        });
    } catch {
        alert('Error al logearse');
        changeLogin();
        return false;
    }


    if (!response.ok) {
        const errorResponse = await response.json();
        console.log('Error:', errorResponse.message);
        alert("xd")
        return false;
    }

    const data = await response.json(); // Procesa la respuesta JSON
    if (data.redirectUrl) {
        window.location.href = data.redirectUrl; // Redirige a la URL proporcionada
    }

    return true;
}