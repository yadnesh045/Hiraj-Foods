﻿document.addEventListener('DOMContentLoaded', function () {


    var distinctFlavors = new Set(flavors);
    document.getElementById('flavorsCount').innerText = "Total Distinct Flavors: " + distinctFlavors.size;


    // Product prices chart
    // Assuming productNames and productPrices are defined elsewhere in your code.

    var ctx1 = document.getElementById('productPricesChart').getContext('2d');
    var productPricesChart = new Chart(ctx1, {
        type: 'bar',
        data: {
            labels: productNames, // Use actual product names
            datasets: [{
                label: 'Product Prices',
                data: productPrices,
                backgroundColor: [
                    'rgba(255, 99, 132, 0.2)', // red
                    'rgba(255, 159, 64, 0.2)', // orange
                    'rgba(255, 205, 86, 0.2)', // yellow
                    'rgba(75, 192, 192, 0.2)', // green
                    'rgba(54, 162, 235, 0.2)', // blue
                    'rgba(153, 102, 255, 0.2)', // purple
                    'rgba(201, 203, 207, 0.2)'  // grey
                ],
                borderColor: [
                    'rgb(255, 99, 132)',
                    'rgb(255, 159, 64)',
                    'rgb(255, 205, 86)',
                    'rgb(75, 192, 192)',
                    'rgb(54, 162, 235)',
                    'rgb(153, 102, 255)',
                    'rgb(201, 203, 207)'
                ],
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
                        text: 'Price (₹)'
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
        type: 'pie', // Maintaining pie chart type; you could also use 'polarArea' if you want to change the chart type
        data: {
            labels: flavorLabels,
            datasets: [{
                data: flavorData,
                backgroundColor: [
                    'rgb(255, 99, 132)', // Red
                    'rgb(75, 192, 192)', // Green
                    'rgb(255, 205, 86)', // Yellow
                    'rgb(201, 203, 207)', // Grey
                    'rgb(54, 162, 235)', // Blue
                    // Additional colors for more than five entries if needed
                    'rgb(255, 159, 64)', // Orange
                    'rgb(153, 102, 255)' // Purple
                ],
                hoverOffset: 4
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: true
        }
    });




    // Energy values chart
    var ctx3 = document.getElementById('energyChart').getContext('2d');
    var energyChart = new Chart(ctx3, {
        type: 'line',
        data: {
            labels: productNames, // Assuming the same products as the price chart
            datasets: [{
                label: 'Energy per Product',
                data: energyValues,
                backgroundColor: 'rgba(54, 162, 235, 0.5)',
                borderColor: 'rgb(54, 162, 235)',
                fill: true
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
                        text: 'Energy (kcal)'
                    }
                },
                x: {
                    title: {
                        display: true,
                        text: 'Products'
                    }
                }
            }
        }
    });




});
