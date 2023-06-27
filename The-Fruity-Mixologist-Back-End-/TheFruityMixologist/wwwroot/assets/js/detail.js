var input = document.querySelector('#qty');
var btnminus = document.querySelector('.qtyminus');
var btnplus = document.querySelector('.qtyplus');

if (input !== undefined && btnminus !== undefined && btnplus !== undefined && input !== null && btnminus !== null && btnplus !== null) {

	var min = Number(input.getAttribute('min'));
	var max = Number(input.getAttribute('max'));
	var step = Number(input.getAttribute('step'));

	function qtyminus(e) {
		var current = Number(input.value);
		var newval = (current - step);
		if (newval < min) {
			newval = min;
		} else if (newval > max) {
			newval = max;
		}
		input.value = Number(newval);
		e.preventDefault();
	}

	function qtyplus(e) {
		var current = Number(input.value);
		var newval = (current + step);
		if (newval > max) newval = max;
		input.value = Number(newval);
		e.preventDefault();
	}

	btnminus.addEventListener('click', qtyminus);
	btnplus.addEventListener('click', qtyplus);

}
let likeIcons = document.querySelector('.likeIcon');
let removelikeIcons = document.querySelector('.removelikeIcon');

likeIcons.addEventListener('click', () => {
	console.log("salam");
	removelikeIcons.classList.add('hidden');
	likeIcons.classList.add('d-none');

});
removelikeIcons.addEventListener('click', () => {
	likeIcons.classList.remove('d-none');
	removelikeIcons.classList.remove('hidden');

});


let like = document.querySelectorAll('#like');
let removelike = document.querySelectorAll('#removelike');

like.forEach((btn) => {
	btn.addEventListener('click', () => {
		let recipeIdd = btn.getAttribute('data-recipe-id');

		let formData = new FormData();
		formData.append('recipeId', recipeIdd);

		fetch('/Wishlist/AddToWishList', {
			method: 'POST',
			body: formData
		})

	});
});

removelike.forEach((btn) => {
	btn.addEventListener('click', () => {
		let recipeId = btn.getAttribute('data-recipe-id');

		let formData = new FormData();
		formData.append('wishListItemId', recipeId);

		fetch('/Wishlist/RemoveFromWishList', {
			method: 'POST',
			body: formData
		})
	});
});