var worker = null;
var loaded = 0;
const lime = document.getElementById("lime");
const lemon = document.getElementById("lemon");
const iceCube = document.getElementById("cubes");
const  ice = document.getElementById("ice");
const vodka = document.getElementById("vodka");
const straw = document.getElementById("straw");
const liqueur = document.getElementById("liqueur");
const whiterum = document.getElementById("whiterum");
const silverTequila = document.getElementById("silverTequila");
const raspberry = document.getElementById("raspberry");
const strawberry = document.getElementById("strawberry");
const cherrySyrup = document.getElementById("cherrySyrup");
const appleSyrup = document.getElementById("appleSyrup");
const angosturaBitter = document.getElementById("angosturaBitter");
const orangeBitter = document.getElementById("orangeBitter");
const pishoBitter = document.getElementById("pishoBitter");
const walnutBitter = document.getElementById("walnutBitter");

const glass = document.getElementById("glass");
const q50m = document.getElementById("q50m");
const q100m = document.getElementById("q100m");
const q150m = document.getElementById("q150m");
const q200m = document.getElementById("q200m");
const q250m = document.getElementById("q250m");
const q300m = document.getElementById("q300m");


const totalCreate = document.querySelector(".totalCreate");
let totalPrice = 0;
let achagol=0;
let syrop=0;
let aromatic =0;
let icevalue=0;
let limons=0;
vodka.addEventListener('change', () => {
    q50m.style.backgroundColor = 'transparent'; // q50m stil özelliklerini sıfırla
    if (vodka.checked) {
        q50m.style.backgroundColor = 'rgba(214, 209, 209, 44%)';
        q50m.style.height = "90px";
        q50m.style.width = "140px";
        q50m.style.position = "absolute";
        q50m.style.top = "180px";
        const vodkaValue = parseInt(vodka.value);
        achagol = vodkaValue;
        totalPrice = achagol+syrop+aromatic+limons+icevalue;

    } else {
        const vodkaValue = parseInt(vodka.value);
        totalPrice -= vodkaValue;
    }
    updateTotalPrice();
});

liqueur.addEventListener('change', () => {
    q50m.style.backgroundColor = 'transparent';
    if (liqueur.checked) {
        q50m.style.backgroundColor = 'rgba(50, 143, 194, 40%)';
        q50m.style.height = "90px";
        q50m.style.width = "140px";
        q50m.style.position = "absolute";
        q50m.style.top = "180px";
        const liqueurValue = parseInt(liqueur.value);
        achagol = liqueurValue;
        totalPrice = achagol+syrop+aromatic+limons+icevalue;


    } else {
        const liqueurValue = parseInt(liqueur.value);
        totalPrice -= liqueurValue;
    }
    updateTotalPrice();
});

whiterum.addEventListener('change', () => {
    q50m.style.backgroundColor = 'transparent'; // q50m stil özelliklerini sıfırla
    if (whiterum.checked) {
        q50m.style.backgroundColor = 'rgba(214, 209, 209, 44%)';
        q50m.style.height = "90px";
        q50m.style.width = "140px";
        q50m.style.position = "absolute";
        q50m.style.top = "180px";
        const whiterumValue = parseInt(whiterum.value);
        achagol = whiterumValue;
        totalPrice = achagol+syrop+aromatic+limons+icevalue;


    } else {
        const whiterumValue = parseInt(whiterum.value);
        totalPrice -= whiterumValue;
    }
    updateTotalPrice();
});


silverTequila.addEventListener('change', () => {
    q50m.style.backgroundColor = 'transparent'; // q50m stil özelliklerini sıfırla
    if (silverTequila.checked) {
        q50m.style.backgroundColor = 'rgba(214, 209, 209, 44%)';
        q50m.style.height = "90px";
        q50m.style.width = "140px";
        q50m.style.position = "absolute";
        q50m.style.top = "180px";
        const silverTequilaValue = parseInt(silverTequila.value);
        achagol = silverTequilaValue;
        totalPrice = achagol+syrop+aromatic+limons+icevalue;
    } else {
        const silverTequilaValue = parseInt(silverTequila.value);
        totalPrice -= silverTequilaValue;
    }
    updateTotalPrice();
});
//Syrups

strawberry.addEventListener('change',() =>{
    if (strawberry.checked) {
        q100m.style.backgroundColor = 'rgba(203, 14, 19, 40%)';
        q100m.style.height="175px";
        q100m.style.width="140px";
        q100m.style.position="absolute";
        q100m.style.top ="93px";
        const strawberryValue = parseInt(strawberry.value);
        syrop = strawberryValue;
        totalPrice = achagol+syrop+aromatic+limons+icevalue;

        updateTotalPrice();
    } else {
        q100m.style.backgroundColor = 'transparent';
        const strawberryValue = parseInt(strawberry.value);
        totalPrice -= strawberryValue;
        updateTotalPrice();
    }
});
raspberry.addEventListener('change',() =>{
    if (raspberry.checked) {
        q100m.style.backgroundColor = 'rgba(93, 13, 179, 40%)';
        q100m.style.height="175px";
        q100m.style.width="140px";
        q100m.style.position="absolute";
        q100m.style.top ="93px";
        const raspberryValue = parseInt(raspberry.value);
        syrop = raspberryValue;
        totalPrice = achagol+syrop+aromatic+limons+icevalue;

        updateTotalPrice();
    } else {
        q100m.style.backgroundColor = 'transparent';
        const raspberryValue = parseInt(raspberry.value);
        totalPrice -= raspberryValue;
        updateTotalPrice();
    }
});
appleSyrup.addEventListener('change',() =>{
    if (appleSyrup.checked) {
        q100m.style.backgroundColor = 'rgba(56, 239, 65, 40%)';
        q100m.style.height="175px";
        q100m.style.width="140px";
        q100m.style.position="absolute";
        q100m.style.top ="93px";
        const appleSyrupValue = parseInt(appleSyrup.value);
        syrop = appleSyrupValue;
        totalPrice = achagol+syrop+aromatic+limons+icevalue;

        updateTotalPrice();
    } else {
        q100m.style.backgroundColor = 'transparent';
        const cherrySyrupValue = parseInt(cherrySyrup.value);
        totalPrice -= cherrySyrupValue;
        updateTotalPrice();
    }
});

cherrySyrup.addEventListener('change',() =>{
    if (cherrySyrup.checked) {
        q100m.style.backgroundColor = 'rgba(203, 13, 19, 40%)';
        q100m.style.height="175px";
        q100m.style.width="140px";
        q100m.style.position="absolute";
        q100m.style.top ="93px";
        const cherrySyrupValue = parseInt(cherrySyrup.value);
        syrop = cherrySyrupValue;
        totalPrice = achagol+syrop+aromatic+limons+icevalue;

        updateTotalPrice();
    } else {
        q100m.style.backgroundColor = 'transparent';
        const cherrySyrupValue = parseInt(cherrySyrup.value);
        totalPrice -= cherrySyrupValue;
        updateTotalPrice();
    }
});

//Aromatic
angosturaBitter.addEventListener('change',() =>{
    if (angosturaBitter.checked) {
        q150m.style.backgroundColor = 'rgba(21, 18, 40, 40%)';
        q150m.style.height="265px";
        q150m.style.width="140px";
        q150m.style.position="absolute";
        q150m.style.top ="3px";
        const angosturaBitterValue = parseInt(angosturaBitter.value);
        aromatic =angosturaBitterValue;
        totalPrice = achagol+syrop+aromatic+limons+icevalue;

        updateTotalPrice();
    } else {
        q150m.style.backgroundColor = 'transparent';
        const angosturaBitterValue = parseInt(angosturaBitter.value);
        totalPrice -= angosturaBitterValue;
        updateTotalPrice();
    }
});
orangeBitter.addEventListener('change',() =>{
    if (orangeBitter.checked) {
        q150m.style.backgroundColor = 'rgba(232, 163, 8, 40%)';
        q150m.style.height="265px";
        q150m.style.width="140px";
        q150m.style.position="absolute";
        q150m.style.top ="3px";
        const orangeBitterValue = parseInt(orangeBitter.value);
        aromatic =orangeBitterValue;
        totalPrice = achagol+syrop+aromatic+limons+icevalue;

        updateTotalPrice();
    } else {
        q150m.style.backgroundColor = 'transparent';
        const orangeBitterValue = parseInt(orangeBitter.value);
        totalPrice -= orangeBitterValue;
        updateTotalPrice();
    }
});
pishoBitter.addEventListener('change',() =>{
    if (pishoBitter.checked) {
        q150m.style.backgroundColor = 'rgba(201, 164, 58, 40%)';
        q150m.style.height="265px";
        q150m.style.width="140px";
        q150m.style.position="absolute";
        q150m.style.top ="3px";
        const pishoBitterValue = parseInt(pishoBitter.value);
        aromatic =pishoBitterValue;
        totalPrice = achagol+syrop+aromatic+limons+icevalue;

        updateTotalPrice();
    } else {
        q150m.style.backgroundColor = 'transparent';
        const pishoBitterValue = parseInt(pishoBitter.value);
        totalPrice -= pishoBitterValue;
        updateTotalPrice();
    }
});
walnutBitter.addEventListener('change',() =>{
    if (walnutBitter.checked) {
        q150m.style.backgroundColor = 'rgba(89, 51, 7, 40%)';
        q150m.style.height="265px";
        q150m.style.width="140px";
        q150m.style.position="absolute";
        q150m.style.top ="3px";
        const walnutBitterValue = parseInt(walnutBitter.value);
        aromatic =walnutBitterValue;
        totalPrice = achagol+syrop+aromatic+limons+icevalue;

        updateTotalPrice();
    } else {
        q150m.style.backgroundColor = 'transparent';
        const walnutBitterValue = parseInt(walnutBitter.value);
        totalPrice -= walnutBitterValue;
        updateTotalPrice();
    }
});

//Ice And Lemon
lime.addEventListener('change', () => {
    if (lime.checked) {
        lemon.classList.remove("d-none");
        const limeValue = parseInt(lime.value);
        limons=limeValue
        totalPrice = achagol+syrop+aromatic+limons+icevalue;
        updateTotalPrice();
    } else {
        lemon.classList.add("d-none");
        const limeValue = parseInt(lime.value);
        totalPrice -= limeValue;
        updateTotalPrice();
    }
});
ice.addEventListener('change', () => {
    if (ice.checked) {
        iceCube.classList.remove("d-none");
        const iceValue = parseInt(ice.value);
        icevalue=iceValue;
        totalPrice = achagol+syrop+aromatic+limons+icevalue;

        updateTotalPrice();
    } else {
        iceCube.classList.add("d-none");
        const iceValue = parseInt(ice.value);
        totalPrice -= iceValue;
        updateTotalPrice();
    }
});
function updateTotalPrice() {
    totalCreate.textContent = "$" + totalPrice.toFixed(2);
  }
