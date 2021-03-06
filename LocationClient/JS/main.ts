import './style.scss';
import 'bootstrap';

window.blazoredLocalisation = {
    getBrowserLocale: function () {
        return navigator.languages && navigator.languages.length ? navigator.languages[0] : navigator['userLanguage']
            || navigator.language
            || navigator['browserLanguage']
            || 'en';
    }
};

const Location = (center, dotNet) => window.BingSearch.reverseGeocode({
    location: center,
    callback: location => {
        console.log('Location:', center, location);
        dotNet.invokeMethodAsync('Location',
            {
                ...location.address,
                ...center
            }
        );
    },
    errorCallback: err => {
        console.error(err);
        dotNet.invokeMethodAsync('Error', err);
    }
});

window.GeoLocation = (dotNet: DotNet) => {
    console.log('Get GeoLocation');
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(
            position => {
                dotNet.invokeMethodAsync("GeoLocation", {
                    latitude: position.coords.latitude,
                    longitude: position.coords.longitude
                });
                console.log('Coords: ', position.coords);
            },
            err => {
                console.log(err);
                dotNet.invokeMethodAsync('Error', err.message);
            }
        );
    }
}

window.LoadMap = (bingKey: string, dotNet: DotNet) => {
    console.log('Geolocation');
    if (navigator.geolocation && navigator.onLine) {
        navigator.geolocation.getCurrentPosition(
            position => {
                const center = new Microsoft.Maps.Location(position.coords.latitude, position.coords.longitude);
                console.log('Coordinates: ', position);
                console.log('Center: ', center);
                if (window.BingMap == null) {
                    window.BingMap = new Microsoft.Maps.Map('#map', {
                        credentials: bingKey,
                        center
                    });
                    Microsoft.Maps.loadModule('Microsoft.Maps.Search', () => {
                        window.BingSearch = new Microsoft.Maps.Search.SearchManager(window.BingMap);
                        Location(center, dotNet);
                    });
                } else {
                    Location(center, dotNet);
                }
            },
            err => {
                console.error(err);
                dotNet.invokeMethodAsync('Error', err.message);
            }
        );
    } else {
        let err;
        if (!navigator.onLine) {
            err = 'Not connected to the internet!';
        } else {
            err = 'Geolocation not supported by this browser!';
        }
        console.error(err);
        dotNet.invokeMethodAsync('Error', err);
    }
}
