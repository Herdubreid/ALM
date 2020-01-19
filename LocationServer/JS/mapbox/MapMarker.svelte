<script>
    import { getContext } from 'svelte';
    import { mapbox, key } from './mapbox';
    import { markersStore } from './store';

    const { getMap } = getContext(key);
    const map = getMap();

    export let lat;
    export let lon;
    export let title;
    export let address;
    export let visible;

    const popup = new mapbox.Popup({ offset: 25 })
        .setHTML(`
<div class="card">
<div class="card-body">
<h6 class="card-title">${title}</h6>
<div class="row">
<div class="col">${address}</div>
</div></div></div>
`);

    const marker = new mapbox.Marker()
        .setLngLat([lon, lat])
        .setPopup(popup)
        .addTo(map);

    $: visible
        ? marker.addTo(map)
        : marker.remove();
    $: marker.setLngLat([lon, lat]);

</script>