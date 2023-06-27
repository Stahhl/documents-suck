<script lang="ts">
	import type { ActionData, PageData } from './$types.js';

	export let data: PageData;
	export let form: ActionData;

	let input = form?.content ?? '';
</script>

<div class="flex h-screen">
	<form method="post" class="w-1/2 bg-gray-200 relative flex flex-row">
		<div class="absolute top-0 right-0 mt-2 mr-2">
			<div class="flex flex-col">
				<button
					class="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded"
					formaction="?/template">Template</button
				>
				<button
					class="bg-green-500 hover:bg-green-700 text-white font-bold py-2 px-4 rounded mt-2"
					formaction="?/template">Download</button
				>
			</div>
		</div>
		<textarea name="input" id="input" class="w-full text-left align-top p-5"
			>{input}</textarea
		>
	</form>
	<div class="w-1/2 bg-gray-300 p-5">
		<article class="prose lg:prose-xl">
			{@html input}
		</article>
	</div>
</div>
