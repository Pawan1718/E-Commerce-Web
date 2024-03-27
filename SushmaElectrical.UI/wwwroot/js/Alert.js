Swal.fire({
    icon: 'success',
    title: 'Success!',
    text: '@TempData["success"]',
    timer: 3000, // Set the timer to close after 3 seconds
    showConfirmButton: false,
    onClose: () => {
        $("#success").remove(); // Remove the alert from DOM after it's closed
    }
});


Swal.fire({
    icon: 'warning',
    title: 'Warning!',
    text: '@TempData["warning"]',
    timer: 3000, // Set the timer to close after 3 seconds
    showConfirmButton: false,
    onClose: () => {
        $("#warningAlert").remove(); // Remove the alert from DOM after it's closed
    }
});


Swal.fire({
    icon: 'error',
    title: 'Error!',
    text: '@TempData["error"]',
    timer: 3000, // Set the timer to close after 3 seconds
    showConfirmButton: false,
    onClose: () => {
        $("#errorAlert").remove(); // Remove the alert from DOM after it's closed
    }
});

Swal.fire({
    icon: 'info',
    title: 'Info!',
    text: '@TempData["info"]',
    timer: 3000, // Set the timer to close after 3 seconds
    showConfirmButton: false,
    onClose: () => {
        $("#infoAlert").remove(); // Remove the alert from DOM after it's closed
    }
});

