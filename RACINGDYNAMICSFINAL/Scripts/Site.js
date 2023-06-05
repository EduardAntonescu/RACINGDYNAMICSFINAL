const navSlide = () => {
    const burger = document.querySelector('.burger');
    const divnav = document.querySelector('.div-nav');
    const carslink = document.querySelector('.lista-cars p');
    const listacars = document.querySelector('.lista-cars-ul');
    

    burger.addEventListener('click', () => {
        divnav.classList.toggle('div-nav-active');
        burger.classList.toggle('burger-active');

        if (!divnav.classList.contains('div-nav-active')) {
            listacars.classList.remove('lista-cars-ul-active');
        }
    });

    carslink.addEventListener('click', () => {
        listacars.classList.toggle('lista-cars-ul-active');
    });

    document.addEventListener('click', (event) => {
        if (!event.target.closest('.lista-cars') && listacars.classList.contains('lista-cars-ul-active')) {
            listacars.classList.remove('lista-cars-ul-active');
        }

        if (!event.target.closest('.burger') && !event.target.closest('.div-nav') && divnav.classList.contains('div-nav-active')) {
            divnav.classList.remove('div-nav-active');
            burger.classList.remove('burger-active');
            listacars.classList.remove('lista-cars-ul-active');
        }
    });
}

navSlide();