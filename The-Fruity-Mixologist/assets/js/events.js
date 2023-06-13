$('.responsiveEvent').slick({
   
    infinite: false,
    speed: 300,
  variableWidth: true,
  prevArrow: false,
  nextArrow: false,
    slidesToShow: 2,
    slidesToScroll: 1,
    responsive: [
      {
        breakpoint: 1024,
        settings: {
          slidesToShow: 3,
          slidesToScroll: 3,
          infinite: true,
        }
      },
      {
        breakpoint: 600,
        settings: {
          slidesToShow: 2,
          slidesToScroll: 2
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


//   var audioPlayer = document.getElementById("audio-player");
// var playButton = document.getElementById("play-button");

// playButton.onclick = function() {
//     audioPlayer.play();
// <i class="bi bi-pause"></i>
// }
// playButton.ondblclick = function(){
//     audioPlayer.pause();
// <i class="bi bi-play"></i>
// }

var audioPlayer = document.getElementById("audio-player");
var playButton = document.getElementById("play-button");
var audioPlayer = document.getElementById('audio-player');

function toggleAudio() {
  if (audioPlayer.paused) {
      audioPlayer.play();
      playIcon.className = 'bi bi-pause';
  } else {
      audioPlayer.pause();
      playIcon.className = 'bi bi-play';
  }
}

$('.responsivePartner').slick({
   
    infinite: true,
    speed: 300,
  variableWidth: true,
  autoplay: true,
  autoplaySpeed: 200,
  prevArrow: false,
  nextArrow: false,
    slidesToShow: 3,
    slidesToScroll: 3,
    responsive: [
      {
        breakpoint: 1024,
        settings: {
          slidesToShow: 3,
          slidesToScroll: 3,
          infinite: true,
        }
      },
      {
        breakpoint: 600,
        settings: {
          slidesToShow: 2,
          slidesToScroll: 2
        }
      },
      {
        breakpoint: 480,
        settings: {
          slidesToShow: 1,
          slidesToScroll: 1
        }
      }
    ]
  });