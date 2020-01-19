//
import { writable } from 'svelte/store';

const zoom = () => {
    const { subscribe, update } = writable({
        zoom: 0,
        center: [0, 0]
    });
    return {
        subscribe,
        zoom: (zoom, center) => update(o => {
            o.zoom = zoom;
            o.center = center == undefined ? o.center : center;
            return o;
        })
    };
}

const markers = () => {
    const { subscribe, update } = writable([])
    return {
        subscribe,
        add: marker => update (a => {
            let i = a.findIndex(r => r.numb === marker.numb);
            if (i === -1) {
                a.push(marker);
            } else {
                a[i].lat = marker.lat;
                a[i].lon = marker.lon;
                a[i].address = marker.address;
            }
            return a;
        }),
        toggle: numb => update(a => {
            let i = a.findIndex(r => r.numb === numb);
            if (i !== -1) {
                a[i].visible = !a[i].visible;
            }
        }),
        move: (numb, lat, lon) => update(a => {
            let i = a.findIndex(r => r.numb === numb);
            if (i !== -1) {
                a[i].lat = lat;
                a[i].lon = lon;
            }
            return a;
        })
    };
}

export const zoomStore = zoom();
export const markersStore = markers();