$(document).ready(function () {
    try {

        $.getJSON('https://ipinfo.io/json?token=xxx', function (data) {

            data.referer = '';
            data.href = '';
            data.title = '';

            let href = $(location).attr('href');

            if (document.referrer != null) {
                data.referer = document.referrer;
            }
            if (href != null) {
                data.href = href;
            }
            if (document.title != null) {
                data.title = document.title;
            }
            console.log(data);

            $.ajax({
                type: 'POST',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                url: "xxx",
                dataType: 'json',
                data: JSON.stringify(data)
            });
        });
    } catch (err) {
        console.log(err);
    }
});
