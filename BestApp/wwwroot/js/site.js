
// ====================== SEARCH ================================ //

document.addEventListener('DOMContentLoaded', () => {
    const searchButton = document.getElementById('search-button'),
        closeButton = document.getElementById('search-close'),
        searchContent = document.getElementById('search-content');

    // show menu
    // validate if constant exists or not

    if (searchButton) {
        searchButton.addEventListener('click', () => {
            searchContent.classList.add('show_search');
        });
    }

    // hide  menu
    // validate if  constant exists

    if (closeButton) {
        closeButton.addEventListener('click', () => {
            searchContent.classList.remove('show_search');
        });
    }


    // =========================== Show Login & Sign Up ======================== //

    const loginButton = document.getElementById('login-button'),
        closeLoginButton = document.getElementById('login_close'), // Renamed the constant
        authenticationContent = document.getElementById('login-content'),
        showLoginButton = document.getElementById('show_login_button'),
        showRegisterButton = document.getElementById('show_register_button'),
        signupForm = document.getElementById('signup_form'),
        loginForm = document.getElementById('login_form');

    // show menu
    // validate if constant exists or not

    if (loginButton) {
        loginButton.addEventListener('click', () => {
            authenticationContent.classList.add('show_login');
        });
    }

    // hide  menu
    // validate if  constant exists

    if (closeLoginButton) {
        closeLoginButton.addEventListener('click', () => {
            authenticationContent.classList.remove('show_login');
        });
    }

    // hidng login & signup forms
    if (showRegisterButton) {
        showRegisterButton.addEventListener('click', () => {
            loginForm.classList.add('hide_form');
            signupForm.classList.remove('hide_form');
            signupForm.classList.add('show_form');
        });
    }
    
    if (showLoginButton) {
        showLoginButton.addEventListener('click', () => {
            signupForm.classList.add('hide_form');
            loginForm.classList.remove('hide_form');
          
        });
    }

    // ====================== ADD SHADOW HEADER ======================== //
    const shadowHeader = () => {
        const header = document.getElementById('header');

        this.scrollY >= 50 ? header.classList.add('shadow-header') : header.classList.remove('shadow-header');
    }
    
    window.addEventListener('scroll', shadowHeader);

    // ====================== ADD SHADOW HEADER ========================= //

    

    let swiper = new Swiper('.home_swiper', {
        // Optional parameters
        loop: true,
        spaceBetween: -24,
        grabCursor: true,
        slidesPerView: 'auto', // Corrected from slidesPreview
        centeredSlides: 'auto', // Corrected from 'auto'
      
        // If we need pagination
        autoplay: {
            delay: 3000,
            disableOnInteraction: false
        },
      
        // Navigation arrows
        breakpoints: {
            576: {
                slidesPerView: 3,
                spaceBetween: 24, // Adjust as needed
            },
            // When window width is >= 768px (medium devices), use default settings (3 slides)
            768: {
                slidesPerView: 3,
                spaceBetween: 24, // Adjust as needed
            },
            // When window width is >= 992px (large devices), use default settings (3 slides)
            992: {
                slidesPerView: 3,
                spaceBetween: 24, // Adjust as needed
            },
            // When window width is >= 1200px (extra large devices), use default settings (3 slides)
            1200: {
                slidesPerView: 3,
                spaceBetween: 24, // Adjust as needed
            },
        }
    });



    // ================== FEATURED SWIPER ========================== ///
    
    let swiperFeatured = new Swiper('.featured_swiper', {
        // Optional parameters
        loop: true,
        spaceBetween: 16,
        grabCursor: true,
        slidesPerView: 'auto', // Corrected from slidesPreview
        centeredSlides: 'auto',

        navigation: {
            nextEl: '.swiper-button-next',
            prevEl: '.swiper-button-prev',
          },
        
      
        // If we need pagination
        autoplay: {
            delay: 6000,
            disableOnInteraction: false
        },
      
        // Navigation arrows
        breakpoints: {
            576: {
                slidesPerView: 2,
                spaceBetween: 24, // Adjust as needed
            },
            // When window width is >= 768px (medium devices), use default settings (3 slides)
            768: {
                slidesPerView: 2,
                spaceBetween: 24, // Adjust as needed
            },
            // When window width is >= 992px (large devices), use default settings (3 slides)
            992: {
                slidesPerView: 3,
                spaceBetween: 24, // Adjust as needed
            },
            // When window width is >= 1200px (extra large devices), use default settings (3 slides)
            1200: {
                slidesPerView: 4,
                spaceBetween: 24,
                centeredSlides: false// Adjust as needed
            },
        }
    });

    // ===================== NEW SLIDER ======================== //


    let swiperNew = new Swiper('.new_swiper', {
        // Optional parameters
        loop: true,
        spaceBetween: 16,
        grabCursor: true,
        slidesPerView: 'auto', // Corrected from slidesPreview
        centeredSlides: 'auto',

        navigation: {
            nextEl: '.swiper-button-next',
            prevEl: '.swiper-button-prev',
          },
        
      
        // If we need pagination
        autoplay: {
            delay: 6000,
            disableOnInteraction: false
        },
      
        // Navigation arrows
        breakpoints: {
            576: {
                slidesPerView: 2,
                spaceBetween: 24, // Adjust as needed
            },
            // When window width is >= 768px (medium devices), use default settings (3 slides)
            768: {
                slidesPerView: 2,
                spaceBetween: 24, // Adjust as needed
            },
            // When window width is >= 992px (large devices), use default settings (3 slides)
            992: {
                slidesPerView: 3,
                spaceBetween: 24, // Adjust as needed
            },
            // When window width is >= 1200px (extra large devices), use default settings (3 slides)
            1200: {
                slidesPerView: 4,
                spaceBetween: 24,
                centeredSlides: false// Adjust as needed
            },
        }
    });


    // =========================== Author Slide =============================

    let swiperAuthor = new Swiper('.testimonial_swiper', {
        // Optional parameters
        loop: true,
        spaceBetween: 16,
        grabCursor: true,
        slidesPerView: 'auto', // Corrected from slidesPreview
        centeredSlides: 'auto',

        navigation: {
            nextEl: '.swiper-button-next',
            prevEl: '.swiper-button-prev',
          },
        
      
        // If we need pagination
        autoplay: {
            delay: 6000,
            disableOnInteraction: false
        },
      
        // Navigation arrows
        breakpoints: {
            576: {
                slidesPerView: 2,
                spaceBetween: 24, // Adjust as needed
            },
            // When window width is >= 768px (medium devices), use default settings (3 slides)
            768: {
                slidesPerView: 2,
                spaceBetween: 24, // Adjust as needed
            },
            // When window width is >= 992px (large devices), use default settings (3 slides)
            992: {
                slidesPerView: 3,
                spaceBetween: 24, // Adjust as needed
            },
            // When window width is >= 1200px (extra large devices), use default settings (3 slides)
            1200: {
                slidesPerView: 4,
                spaceBetween: 24,
                centeredSlides: false// Adjust as needed
            },
        }
    });


    // ==========================  DARK & LIGHT THEME ============================ //

    const themeButton = document.getElementById('change-theme');
    const darkTheme = 'dark-theme';
    const iconTheme = 'ri-sun-line';

    const selectedTheme = localStorage.getItem('selected-theme');
    const selectedIcon = localStorage.getItem('selected-icon');

    const getCurrentTheme = () => document.body.classList.contains('dark-theme') ? 'dark' : 'light';
    const getCurrentIcon = () => themeButton.classList.contains(iconTheme) ? 'ri-moon-line' : 'ri-sun-line';

    if (selectedTheme) {
        document.body.classList[selectedTheme == 'dark' ? 'add' : 'remove'](darkTheme);
        themeButton.classList[selectedIcon == 'ri-moon-line' ? 'add' : 'remove'](iconTheme);

    }

    themeButton.addEventListener('click', () => {
        document.body.classList.toggle(darkTheme);
        themeButton.classList.toggle(iconTheme);

        localStorage.setItem('selected-theme', getCurrentTheme());
        localStorage.setItem('selected-icon', getCurrentIcon());

    })



    // ================================  SCROLL REVEAL ANIMATION ========================== //

    const sr = ScrollReveal({
        origin: 'top',
        distance: '60px',
        duration: 1400,
        delay: 400,
        reset: true
    });

    sr.reveal(`.home_data,  .featured_container, .new_container, .testimonial_container`);
    sr.reveal(`.home_images`, { delay: 600 });
    sr.reveal(`.servicies_card`, { interval: 100 });
    sr.reveal(`.discount_data`, { origin: 'left' });
    sr.reveal(`.discount_images`, { origin: 'right' })
    



    // ==================== Search Animation ========================== ////
    const searchInput = document.querySelector('.search_input');
    const search_Cards = document.querySelectorAll('.search_card');
    
    searchInput.addEventListener('input', searchCards); // Corrected function name
    
    function searchCards() {
        search_Cards.forEach((card, i) => {
            let search_result = card.textContent.toLocaleLowerCase();
            let searchData = searchInput.value.toLocaleLowerCase(); 
    
            card.classList.toggle('hideC', search_result.indexOf(searchData) < 0);
            card.style.setProperty('--delay',i/25 + 's')
        });
    }


    // =============== auth show ==================
    const loginYButton = document.getElementById('login-button');
    const authDropdown = document.getElementById('auth_dropdown');
    const profile_pic = document.getElementById('profile_pic');
    const profile_dropdown =  document.getElementById('profile_dropdwon')
    
    if (loginYButton) {
        loginYButton.addEventListener('click', () => {
            authDropdown.classList.toggle('auth_show');
        });
    }

    if (profile_pic) {
        profile_pic.addEventListener('click', () => {
            profile_dropdown.classList.toggle('profile_show');
        });
    }



    // details =================== details 


const imgs = document.querySelectorAll('.img-select a');
const imgBtns = [...imgs];
let imgId = 1;

imgBtns.forEach((imgItem) => {
    imgItem.addEventListener('click', (event) => {
        event.preventDefault();
        imgId = imgItem.dataset.id;
        slideImage();
    });
});

function slideImage(){
    const displayWidth = document.querySelector('.img-showcase img:first-child').clientWidth;

    document.querySelector('.img-showcase').style.transform = `translateX(${- (imgId - 1) * displayWidth}px)`;
}

    window.addEventListener('resize', slideImage);
    

    // addToCart

    


});
 








 


