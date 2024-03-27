document.addEventListener("DOMContentLoaded", function () {
    const checkboxes = document.querySelectorAll('.defaultAddressCheckbox');

    checkboxes.forEach(function (checkbox) {
        checkbox.addEventListener('change', function () {
            const shippingDetailsId = this.dataset.id;
            const label = document.getElementById('defaultAddressLabel_' + shippingDetailsId);

            if (this.checked) {
                label.classList.add('active');
                unselectOtherCheckboxes(shippingDetailsId);
                sessionStorage.setItem('selectedShippingDetailsId', shippingDetailsId);
            } else {
                label.classList.remove('active');
                sessionStorage.removeItem('selectedShippingDetailsId');
            }

            setDefaultShippingAddress(shippingDetailsId, this.checked);
        });
    });

    function unselectOtherCheckboxes(currentId) {
        checkboxes.forEach(function (checkbox) {
            if (checkbox.dataset.id !== currentId) {
                checkbox.checked = false;
                const otherLabel = document.getElementById('defaultAddressLabel_' + checkbox.dataset.id);
                otherLabel.classList.remove('active');
            }
        });
    }

    const shippingDetailsCount = document.querySelectorAll('.defaultAddressCheckbox').length;
    if (shippingDetailsCount === 1) {
        const checkbox = document.querySelector('.defaultAddressCheckbox');
        checkbox.checked = true;
        const label = document.getElementById('defaultAddressLabel_' + checkbox.dataset.id);
        label.classList.add('active');
        setDefaultShippingAddress(checkbox.dataset.id, true);
        sessionStorage.setItem('selectedShippingDetailsId', checkbox.dataset.id);
    } else {
        const selectedShippingDetailsId = sessionStorage.getItem('selectedShippingDetailsId');
        if (selectedShippingDetailsId) {
            const checkbox = document.getElementById('defaultAddressCheckbox_' + selectedShippingDetailsId);
            const label = document.getElementById('defaultAddressLabel_' + selectedShippingDetailsId);
            checkbox.checked = true;
            label.classList.add('active');
        }
    }
});

function setDefaultShippingAddress(id, isChecked) {
    fetch('/Shipping/SetDefault/' + id, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'X-CSRF-TOKEN': document.querySelector('input[name="__RequestVerificationToken"]').value
        },
        body: JSON.stringify({ id: id, isChecked: isChecked })
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            console.log('Default shipping address set successfully');
        })
        .catch(error => {
            console.error('There was a problem setting the default shipping address:', error);
        });
}