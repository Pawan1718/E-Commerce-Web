document.addEventListener("DOMContentLoaded", function () {
    const netPriceInput = document.getElementById("NetPrice");
    const discountSelect = document.getElementById("Discount");
    const taxSelect = document.getElementById("Tax");
    const grossPriceInput = document.getElementById("GrossPrice");

    const calculateGrossPrice = () => {
        const netPrice = parseFloat(netPriceInput.value) || 0;
        const discountRate = parseFloat(discountSelect.value) || 0;
        const taxRate = parseFloat(taxSelect.value) || 0;

        const discountedPrice = netPrice - (netPrice * (discountRate / 100));
        const grossPrice = discountedPrice + (discountedPrice * (taxRate / 100));

        grossPriceInput.value = grossPrice.toFixed(2); // Display with two decimal places

        // Store values in session storage
        sessionStorage.setItem('netPrice', netPrice);
        sessionStorage.setItem('discountRate', discountRate);
        sessionStorage.setItem('taxRate', taxRate);
    };

    // Retrieve values from session storage on page load for edit view
    const storedNetPrice = sessionStorage.getItem('netPrice');
    const storedDiscountRate = sessionStorage.getItem('discountRate');
    const storedTaxRate = sessionStorage.getItem('taxRate');

    if (storedNetPrice && storedDiscountRate && storedTaxRate) {
        netPriceInput.value = storedNetPrice;
        discountSelect.value = storedDiscountRate;
        taxSelect.value = storedTaxRate;

        // Calculate gross price when values are retrieved from session storage
        calculateGrossPrice();
    }

    // Reset price component for create view
    const resetPriceComponent = () => {
        netPriceInput.value = '';
        discountSelect.value = '';
        taxSelect.value = '';
        grossPriceInput.value = '';
        sessionStorage.removeItem('netPrice');
        sessionStorage.removeItem('discountRate');
        sessionStorage.removeItem('taxRate');
    };

    // Check if it's create view and reset price component
    if (window.location.pathname.includes("CreateProduct")) {
        resetPriceComponent();
    }

    // Calculate gross price on input change
    netPriceInput.addEventListener("input", calculateGrossPrice);
    discountSelect.addEventListener("change", calculateGrossPrice);
    taxSelect.addEventListener("change", calculateGrossPrice);
});
