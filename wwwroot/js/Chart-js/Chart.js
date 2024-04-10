document.addEventListener('DOMContentLoaded', function () {


    var distinctFlavors = new Set(flavors);
    document.getElementById('flavorsCount').innerText = "Total Distinct Flavors: " + distinctFlavors.size;


    // Product prices chart
    var ctx1 = document.getElementById('productPricesChart').getContext('2d');
    var productPricesChart = new Chart(ctx1, {
        type: 'bar',
        data: {
            labels: productPrices.map((price, index) => 'Product ' + (index + 1)),
            datasets: [{
                label: 'Product Prices',
                data: productPrices,
                backgroundColor: 'rgba(54, 162, 235, 0.5)', // Blue color for bars
                borderColor: 'rgba(54, 162, 235, 1)', // Blue color for border
                borderWidth: 1
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                y: {
                    beginAtZero: true,
                    title: {
                        display: true,
                        text: 'Price ($)'
                    }
                },
                x: {
                    title: {
                        display: true,
                        text: 'Product'
                    }
                }
            }
        }
    });

    // Flavors chart
    var flavorsCount = {};

    flavors.forEach(function (flavor) {
        flavorsCount[flavor] = (flavorsCount[flavor] || 0) + 1;
    });
    var flavorLabels = Object.keys(flavorsCount);
    var flavorData = Object.values(flavorsCount);

    var ctx2 = document.getElementById('flavorsChart').getContext('2d');
    var flavorsChart = new Chart(ctx2, {
        type: 'pie',
        data: {
            labels: flavorLabels,
            datasets: [{
                data: flavorData,
                backgroundColor: [
                    '#FF6384',
                    '#36A2EB',
                    '#FFCE56',
                    '#FF8C00',
                    '#FFD700',
                    '#FF4500',
                    '#FF1493'
                ],
                hoverOffset: 4
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: true
        }
    });
});
