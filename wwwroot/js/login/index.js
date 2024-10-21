const wrapper = document.querySelector('.wrapper');
const signUpLink = document.querySelector('.signUp-link');
const signInLink = document.querySelector('.signIn-link');
const toggleLogin = document.querySelector('.toggle-login');
const close = document.querySelector('.close');

//toggleLogin.addEventListener('click', () => {
//toggleLogin.classList.toggle('active');
wrapper.classList.toggle('active');
//})

signUpLink.addEventListener('click', () => {
    wrapper.classList.toggle('show-sign');
    wrapper.classList.add('show-bg');
})

signInLink.addEventListener('click', () => {
    wrapper.classList.toggle('show-sign');
    wrapper.classList.add('show-bg');
})

//close.addEventListener('click', () => {
//    toggleLogin.classList.toggle('active');
//    wrapper.classList.toggle('active');
//    wrapper.classList.remove('show-bg');
//})