<script lang="ts">
	import type { DocumentRequest } from '../../utils/models.js';
	import type { ActionData, PageData, RequestEvent } from './$types.js';

	export let form: ActionData;

	let input = form?.input ?? '';
	let result = form?.result ?? input;

	async function download() {
		const input = document.getElementById('input') as HTMLTextAreaElement;

		const request: DocumentRequest = {
			contents: input?.value
		};

		const body = JSON.stringify(request);

		console.log(`body: ${body}`);

		const response: Response = await fetch('http://localhost:5099/document/playground/pdf', {
			method: 'POST',
			body: body,
			headers: {
				'Content-Type': 'application/json',
				'Access-Control-Allow-Origin': '*'
			}
		});

		console.log(response.status);

		if (response.ok) {
			const filename = 'document.txt'; // Set the desired filename here
			const blob = await response.blob();
			const url = URL.createObjectURL(blob);

			const a = document.createElement('a');
			a.href = url;
			a.download = filename;
			a.click();

			URL.revokeObjectURL(url);
		} else {
			// Handle error response
		}
		// console.table([...data.entries()]);
	}
</script>

<div class="flex h-screen">
	<form method="post" class="w-1/2 bg-gray-200 relative flex flex-row">
		<div class="absolute top-0 right-0 mt-2 mr-2">
			<div class="flex flex-col">
				<button
					class="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded"
					formaction="?/html">Html</button
				>
				<button
					class="bg-green-500 hover:bg-green-700 text-white font-bold py-2 px-4 rounded mt-2"
					on:click|preventDefault={download}>Pdf</button
				>
			</div>
		</div>
		<textarea name="input" id="input" class="w-full text-left align-top p-5">{input}</textarea>
	</form>
	<div class="w-1/2 bg-gray-300 p-5">
		<article class="prose lg:prose-xl">
			{@html result}
		</article>
	</div>
</div>
