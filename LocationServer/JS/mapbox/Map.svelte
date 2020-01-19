<script>
    import { onMount, setContext } from 'svelte';
    import { mapbox, key } from './mapbox';
    import { zoomStore } from './store';

    setContext(key, {
        getMap: () => map
    });

    export let token;

    mapbox.accessToken = token;

    let container;
    let map;

    $: if (map !== undefined) {
        map.flyTo({
            zoom: $zoomStore.zoom,
            center: $zoomStore.center
        });
    }

    onMount(() => {
        map = new mapbox.Map({
            container,
            style: 'mapbox://styles/mapbox/streets-v11',
            renderWorldCopies: true
        });
    });
</script>

<style>
    div {
        width: 100%;
        height: 100%;
    }
</style>

<div bind:this={container}>
    {#if map}
        <slot></slot>
    {/if}
</div>
