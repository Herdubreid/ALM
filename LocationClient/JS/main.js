"use strict";
var __assign = (this && this.__assign) || function () {
    __assign = Object.assign || function(t) {
        for (var s, i = 1, n = arguments.length; i < n; i++) {
            s = arguments[i];
            for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
                t[p] = s[p];
        }
        return t;
    };
    return __assign.apply(this, arguments);
};
Object.defineProperty(exports, "__esModule", { value: true });
require("./style.scss");
require("bootstrap");
window.blazoredLocalisation = {
    getBrowserLocale: function () {
        return navigator.languages && navigator.languages.length ? navigator.languages[0] : navigator['userLanguage']
            || navigator.language
            || navigator['browserLanguage']
            || 'en';
    }
};
var Location = function (center, dotNet) { return window.BingSearch.reverseGeocode({
    location: center,
    callback: function (location) {
        console.log('Location:', center, location);
        dotNet.invokeMethodAsync('Location', __assign(__assign({}, location.address), center));
    },
    errorCallback: function (err) {
        console.error(err);
        dotNet.invokeMethodAsync('Error', err);
    }
}); };
window.LoadMap = function (bingKey, dotNet) {
    console.log('Geolocation');
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (position) {
            var center = new Microsoft.Maps.Location(position.coords.latitude, position.coords.longitude);
            console.log('Coordinates: ', position);
            console.log('Center: ', center);
            if (window.BingMap == null) {
                window.BingMap = new Microsoft.Maps.Map('#map', {
                    credentials: bingKey,
                    center: center
                });
                Microsoft.Maps.loadModule('Microsoft.Maps.Search', function () {
                    window.BingSearch = new Microsoft.Maps.Search.SearchManager(window.BingMap);
                    Location(center, dotNet);
                });
            }
            else {
                Location(center, dotNet);
            }
        }, function (err) {
            console.error(err);
            dotNet.invokeMethodAsync('Error', err.message);
        });
    }
    else {
        var err = 'Geolocation not supported by this browser!';
        console.error(err);
        dotNet.invokeMethodAsync('Error', err);
    }
};
//# sourceMappingURL=main.js.map