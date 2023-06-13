let accountIcon = document.getElementById("accountIcon");
let personDiv = document.querySelector(".personDiv");


accountIcon.addEventListener("click", function () {
    personDiv.classList.toggle("active");
  });


const basketIcon = document.getElementById("basketIcon");
const basketDiv = document.querySelector(".basketDiv");
  const canselButton = document.querySelector(".close");


  basketIcon.addEventListener("click", () => {
    
    basketDiv.classList.toggle("active");
  });
  canselButton.addEventListener("click", () => {
    console.log("Sa");
    basketDiv.classList.remove("active");
  });

  const heartIcon = document.getElementById("heartIcon");
  const heartDiv = document.querySelector(".heartDiv");
    const closeClick = document.querySelector(".closeHeartClick");

    
    heartIcon.addEventListener("click", () => {
    console.log("Sdsd");
      heartDiv.classList.remove("d-none");
    });
  closeClick.addEventListener("click", () => {
    console.log("Sa");
    heartDiv.classList.add("d-none");
  });
  

  const hamburgerIcon = document.getElementById("hamburgerIcon");
  const hamburgerDiv = document.querySelector(".hamburgerDivList");
  const closeHamburgerDiv = document.querySelector(".close");


  hamburgerIcon.addEventListener("click",() =>{
    console.log("islyr");
    hamburgerDiv.classList.remove("d-none");
  });

  closeHamburgerDiv.addEventListener("click",() =>{
    console.log("islyr");
    hamburgerDiv.classList.add("d-none");
  });


  const SearchResponsiveIcon = document.getElementById("SearchRespIcon");
  const inputResponsiveSearch = document.querySelector(".inputResponsiveSearch");
  const Logo = document.querySelector(".LogoCol");

  SearchResponsiveIcon.addEventListener("click",() => {
    Logo.classList.toggle("d-none");
    inputResponsiveSearch.classList.toggle("d-none");
  })