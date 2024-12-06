document.addEventListener("DOMContentLoaded", function () {
    const quantityInput = document.getElementById("quantity");
    const btnMinus = document.querySelector(".quantity-left-minus");
    const btnPlus = document.querySelector(".quantity-right-plus");

    // Xử lý nút trừ (-)
    btnMinus.addEventListener("click", function (e) {
        e.preventDefault();
        let currentValue = parseInt(quantityInput.value);
        if (currentValue > parseInt(quantityInput.min)) {
            quantityInput.value = currentValue - 1;
        }
    });

    // Xử lý nút cộng (+)
    btnPlus.addEventListener("click", function (e) {
        e.preventDefault();
        let currentValue = parseInt(quantityInput.value);
        if (currentValue < parseInt(quantityInput.max)) {
            quantityInput.value = currentValue + 1;
        }
    });
});
