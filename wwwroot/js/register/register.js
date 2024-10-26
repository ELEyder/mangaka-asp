const baseURL = window.location.origin;

document.getElementById('formSignUp').addEventListener("submit", async function (event) {
    event.preventDefault();

    changeRegister();

    const username = document.getElementById('usernameSignUp').value;
    const email = document.getElementById('emailSignUp').value;
    const password = document.getElementById('passwordSignUp1').value;
    const repeatPassword = document.getElementById('passwordSignUp2').value;
    const request = await register(username, email, password, repeatPassword);

    if (request) {
        alert("Registro realizado.");
    }
    else {
        alert("No se ha completado el registro");
    }

    changeRegister();
})

function changeRegister() {
    const button = document.getElementById('submitSignUp');
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
async function register(username, email, password, repeatPassword) {
    var dataUser = JSON.stringify({
        username: username,
        email: email,
        password: password,
        repeatPassword: repeatPassword,
    })
    const response = await fetch(baseURL + '/Register', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: dataUser
    });

    if (!response.ok) {
        response.text().then(errorMessage => {
            alert('Error: ' + errorMessage);
        });
        return false;
    }

    const data = await response.json(); // Procesa la respuesta JSON
    if (data.redirectUrl) {
        window.location.href = data.redirectUrl; // Redirige a la URL proporcionada
    }

    return true;
}