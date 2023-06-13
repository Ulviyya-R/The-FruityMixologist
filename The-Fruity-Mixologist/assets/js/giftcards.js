const images = document.querySelectorAll('.blackcard');
const checkbox = document.querySelectorAll('.black_check');
images.forEach((img, index) => {
    img.addEventListener('click', () => {
      checkbox[index].click();
      checkbox[index].checked = true;
      checkbox.forEach((check, i) => {
        if (i !== index) {
            check.checked = false;
            check.parentElement.classList.remove('border-green');
        }
      });
      checkbox[index].parentElement.classList.add('border-green');
    });
  });
  

const priceSpans = document.querySelectorAll('.choosePricee');
const checkboxes = document.querySelectorAll('.chooseCheck');
priceSpans.forEach((span, index) => {
  span.addEventListener('click', () => {
    checkboxes[index].click();
    checkboxes[index].checked = true;
    checkboxes.forEach((checkbox, i) => {
      if (i !== index) {
        checkbox.checked = false;
        checkbox.parentElement.classList.remove('selected');
      }
    });

    checkboxes[index].parentElement.classList.add('selected');
  });
});


const buyGiftCardClick = document.querySelector(".buyGiftCardClick");
const addButton = document.querySelector(".addCartBasket");
const canselButton = document.querySelector(".cancel");

addButton.addEventListener("click", () => {
  buyGiftCardClick.classList.remove("d-none");
});
canselButton.addEventListener("click", () => {
  buyGiftCardClick.classList.add("d-none");
});
