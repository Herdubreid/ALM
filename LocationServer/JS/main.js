import 'bootstrap';
import MapBox from './mapbox/MapBox.svelte';
import { zoomStore, markersStore } from './mapbox/store';

window.map = {
    init: (id, token) => {
        const target = document.getElementById(id);
        new MapBox({
            target,
            props: {
                token
            }
        });
    },
    addMarker: marker => {
        markersStore.add(marker);
    },
    moveMarker: (numb, lat, lon) => update(a => {
        markersStore.move(numb, lat, lon);
    }),
    zoom: (zoom, center) => {
        zoomStore.zoom(zoom, center);
    }
}