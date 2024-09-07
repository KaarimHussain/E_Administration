document.addEventListener('DOMContentLoaded', function () {
    var loginButtonSubmit = document.getElementById('login-button-submit');
    loginButtonSubmit.addEventListener('click', () => {
        loginButtonSubmit.innerHTML = `
        <div class="spinner-border spinner-border-sm" role="status">
            <span class="visually-hidden">Loading...</span>
        </div>
        `;
    })
})