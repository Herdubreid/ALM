<script>
    import { markersStore } from './store';
    import Map from './Map.svelte';
    import MapMarker from './MapMarker.svelte';

    export let token;
</script>

<style>
    div {
        height: 100%;
        width: 100%;
    }
</style>

<div class="col p-0">
    <Map
        zoom={0}
        token={token}>
        {#each $markersStore as marker}
            <MapMarker
                    visible={marker.visible}
                    lat={marker.lat}
                    lon={marker.lon}
                    title={marker.title}
                    address={marker.address} />
        {/each}
    </Map>
</div>
