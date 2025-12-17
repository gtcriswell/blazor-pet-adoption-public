$(document).ready(function () {

     const trackerUrl = "https://netdevnow.com/blazor/adoptapi/api/User/AddTracker";
     //const trackerUrl = "https://localhost:7042/api/user/addtracker";

    function buildBaseData() {
        return {
            referer: document.referrer || '',
            href: window.location.href || '',
            title: document.title || ''
        };
    }

    function sendTracker(data) {
        $.ajax({
            type: 'POST',
            url: trackerUrl,
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            dataType: 'json',
            data: JSON.stringify(data)
        });
    }

    // Attempt full ipinfo lookup
    $.getJSON('https://ipinfo.io/json?token=xxx')
        .done(function (ipData) {
            const data = {
                ...ipData,
                ...buildBaseData()
            };

            sendTracker(data);
        })
        .fail(function () {
            // Fallback: IP only
            $.get('https://api.ipify.org?format=json')
                .done(function (ipOnly) {
                    const data = {
                        ip: ipOnly.ip,
                        ...buildBaseData()
                    };

                    sendTracker(data);
                });
        });

});

