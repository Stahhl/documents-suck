<script lang="ts">
    import submitContent from '../../utils/utils.js';
	export let data;

    function handleSubmit() {
        let content = document.querySelector(".content")

        submitContent(content?.innerHTML)
    }
</script>

<div class="content">
	{@html data.content}
</div>

<form method="POST" on:submit|preventDefault={handleSubmit}>
    <button>Log in</button>
</form>

<style global>
	:global(h1) {
		color: red;
	}
</style>
