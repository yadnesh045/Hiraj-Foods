document.addEventListener('DOMContentLoaded', function () {

    // Notification chart
    var ctx = document.getElementById('Notification').getContext('2d');
    var Notification = new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: ["Contact", "Enquiry", "Feedback"],
            datasets: [{
                label: 'Data',
                data: [contact, enquiry, feedback], // Data for 'Contact', 'Enquiry', 'Feedback'
                backgroundColor: [
                    'rgb(255, 99, 132)', // Color for 'Contact'
                    'rgb(54, 162, 235)', // Color for 'Enquiry'
                    'rgb(255, 205, 86)'  // Color for 'Feedback'
                ],
                hoverOffset: 4
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false
        }

    });



    // payment chart


    var ctx = document.getElementById('paymentstatus').getContext('2d');
    var Notification = new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: ["Paid", "Pending"],
            datasets: [{
                label: 'Data',
                data: [paid, pending], // Data for 'Contact', 'Enquiry', 'Feedback'
                backgroundColor: [
                    'rgb(255, 99, 132)', // Color for 'Contact'
                    'rgb(54, 162, 235)', // Color for 'Enquiry'
                    'rgb(255, 205, 86)'  // Color for 'Feedback'
                ],
                hoverOffset: 4
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false
        }

    });
});
