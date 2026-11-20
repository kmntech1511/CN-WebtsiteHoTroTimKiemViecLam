document.addEventListener('DOMContentLoaded', () => {
    const toggle = document.querySelector('.menu-toggle');
    const nav = document.querySelector('.main-nav');
    const actions = document.querySelector('.nav-actions');
    if (!toggle) return;
    toggle.addEventListener('click', () => {
        const open = nav.classList.toggle('open');
        actions.classList.toggle('open', open);
        toggle.setAttribute('aria-expanded', open.toString());
    });
});
