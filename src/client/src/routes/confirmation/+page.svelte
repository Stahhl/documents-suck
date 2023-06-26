<script lang="ts">
	export let data;

    console.log(data)
</script>

{@html data.content}

<style>
	:global(h1) {
		color: red;
	}
</style>
