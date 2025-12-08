window.zipLookup = {
    getZipCode: async function () {
        return new Promise((resolve, reject) => {
            if (!navigator.geolocation) {
                reject("Geolocation not supported");
                return;
            }

            navigator.geolocation.getCurrentPosition(async (pos) => {
                const { latitude, longitude } = pos.coords;

                try {
                    // Free API, no key needed
                    const url = `https://geocode.maps.co/reverse?lat=${latitude}&lon=${longitude}&api_key=xxx`;
                    const response = await fetch(url);
                    const data = await response.json();

                    resolve(data?.address?.postcode || "");
                } catch (e) {
                    reject("Failed to fetch ZIP");
                }
            }, (err) => reject(err.message));
        });
    }
};