loadCartItemsCount();
async function loadCartItemsCount() {
    try {
        var response = await fetch(`/Carts/GetTotalItemInCart`);
        console.log(response);
        if (response.status == 200) {
            var result = await response.json();
            var cartCountEl = document.getElementById("cartCount");
            cartCountEl.innerHTML = result;
        }

    } catch (err) {
        console.log(err);
    }
}


