$('.responsive').slick({
   
    infinite: false,
    speed: 300,
  variableWidth: true,
  prevArrow: false,
  nextArrow: false,
    slidesToShow: 3,
    slidesToScroll: 3,
    responsive: [
      {
        breakpoint: 1024,
        settings: {
          slidesToShow: 2,
          slidesToScroll: 1,
          infinite: true,
        }
      },
      {
        breakpoint: 600,
        settings: {
          slidesToShow: 2,
          slidesToScroll: 1
        }
      },
      {
        breakpoint: 480,
        settings: {
          slidesToShow: 1,
          slidesToScroll: 1
        }
      }
      // You can unslick at a given breakpoint now by adding:
      // settings: "unslick"
      // instead of a settings object
    ]
  });




  $(document).ready(function() {
    $('.comments').slick({
      centerMode: true,
      centerPadding: '20px',
      slidesToShow: 2,
      variableWidth:true,
      slidesToScroll: 1,
      speed: 2000 ,
      arrows:false,
      responsive: [
        {
          breakpoint: 768,
          settings: {
            arrows: false,
            centerMode: true,
            centerPadding: '40px',
            slidesToShow: 1 
          }
        }
      ]
    });
    
    // Comment'a click etdikde, o comment'in slaytta gösterilmesi ucun
    $('.comment').on('click', function() {
      var index = $(this).index();
      $('.comments').slick('slickGoTo', index);
    });
  });
 
  $('.responsivePartner').slick({
   
    infinite: true,
    speed: 1000,
  variableWidth: true,
  autoplay: true,
  autoplaySpeed: 60,
  prevArrow: false,
  nextArrow: false,
    slidesToShow: 3,
    slidesToScroll: 3,
    responsive: [
      {
        breakpoint: 1024,
        settings: {
          slidesToShow: 1,
          slidesToScroll: 1,
          infinite: true,
        }
      },
      {
        breakpoint: 600,
        settings: {
          slidesToShow: 1,
          slidesToScroll: 1
        }
      },
      {
        breakpoint: 390,
        settings: {
          slidesToShow: 1,
          slidesToScroll: 1
        }
      }
    ]
  });
  

  const modeToggle = document.querySelector("#mode-toggle");
const body = document.querySelector("body");

modeToggle.addEventListener("click", () => {
  body.classList.toggle("light-mode");
});
  