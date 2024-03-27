function performSearch() {
    var searchQuery = document.getElementById("search-input").value;
    // Redirect to search results page with the search query
    window.location.href = "/search?query=" + encodeURIComponent(searchQuery);
}

// Event listener for search button click
document.getElementById("search-button").addEventListener("click", function () {
    performSearch();
});

// Event listener for pressing Enter in the search input field
document.getElementById("search-input").addEventListener("keypress", function (event) {
    if (event.key === "Enter") {
        performSearch();
    }
});
