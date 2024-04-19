$(document).ready(function () {


    $("#checkoutButton").click(function () {

        var totalPrice = 0;

        $('.quantity-input').each(function () {
            var price = parseFloat($(this).data('price'));
            var quantity = parseInt($(this).val());
            totalPrice += price * quantity;

        });

        var productDetails = [];
        $(".card-title").each(function () {
            var productName = $(this).text();
            var quantity = $(this).closest('.card-body').find('.quantity-input').val();
            productDetails.push(productName + ":" + quantity);
        });


        $.ajax({
            type: "POST", 
            url: "/Yadnesh/SaveTotal",
            data: { total: totalPrice, products: productDetails.join(", ") }, 
            success: function (response) {
      
                console.log(response); 

                
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

