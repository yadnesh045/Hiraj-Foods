$(document).ready(function () {
    // Add event listener to the checkout button
    $("#checkoutButton").click(function () {
        // Get total price
        var totalPrice = parseFloat($("#totalAmount").text().replace('$', ''));

        // Get product names and quantities
        var productDetails = [];
        $(".card-title").each(function () {
            var productName = $(this).text();
            var quantity = $(this).siblings('.input-group').find('.quantity-input').val();
            productDetails.push(productName + ":" + quantity);
        });


        // Use AJAX to send the total price and product details to the server
        $.ajax({
            type: "POST", // Use POST method
            url: "/Yadnesh/SaveTotal", // URL of your controller method
            data: { total: totalPrice, products: productDetails.join(", ") }, // Data to send
            success: function (response) {
                // Handle success response
                console.log(response); // For example, log the response to console

                // Store the quantity in the session
                //   sessionStorage.setItem('productDetails', productDetails.join(", "));


                // Redirect to Checkout page
                window.location.href = "/Yadnesh/Checkout";
            },
            error: function (xhr, status, error) {
                // Handle error
                console.error(error); // Log the error to console
            }
        });
    });

    // Add event listeners for quantity inputs
    $('.quantity-input').on('input', function () {
        updateTotalPrice();
    }).on('change', function () {
        validateQuantity($(this));
    });

    // Function to update total price
    function updateTotalPrice() {
        var totalPrice = 0;
        $('.quantity-input').each(function () {
            var price = parseFloat($(this).data('price'));
            var quantity = parseInt($(this).val());
            totalPrice += price * quantity;

        });
        $('#totalAmount').text('Rs.' + totalPrice.toFixed(2));
    }

    // Call updateTotalPrice immediately after defining it
    updateTotalPrice();




    // Function to validate quantity
    function validateQuantity(input) {
        var quantity = parseInt(input.val());
        if (quantity < 1) {
            input.val(1); // Reset quantity to 1 if less than 1
        }
        updateTotalPrice(); // Update total price after validating quantity
    }
});

