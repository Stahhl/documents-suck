import type { Actions } from './$types';
import type { DocumentRequest } from '../../utils/models';

// export async function load({ fetch }) {
// 	try {
// 		const body = {
// 			name: 'world',
// 			condition: false
// 		};

// 		const response: Response = await fetch('http://127.0.0.1:8000/template/confirmation', {
// 			method: 'POST',
// 			body: JSON.stringify(body),
// 			headers: {
// 				'Content-Type': 'application/json'
// 			}
// 		});

// 		const content = await response.text();

// 		// console.log(content)

// 		return { content };
// 	} catch (error) {}
// }

export const actions = {
	html: async (event) => {
		const data = await event.request.formData();
		// console.table([...data.entries()]);
		const input: string = data.get('input') as string;

		const request: DocumentRequest = {
			contents: input
		};

		const body = JSON.stringify(request);

		console.log(`body: ${body}`);

		const response: Response = await fetch('http://localhost:5099/document/playground/html', {
			method: 'POST',
			body: body,
			headers: {
				'Content-Type': 'application/json'
			}
		});

		const result = await response.text();

		console.log(input, result);

		return { input, result };
	},
	pdf: async (event) => {
		// const data = await event.request.formData();
		// // console.table([...data.entries()]);
		// const input: string = data.get('input') as string;

		// const request: DocumentRequest = {
		// 	contents: input
		// };

		// const body = JSON.stringify(request);

		// console.log(`body: ${body}`);

		// const response: Response = await event.fetch("api/download", {
		// 	method: 'POST',
		// 	body: body,
		// 	headers: {
		// 		'Content-Type': 'application/json'
		// 	}
		// });

		// const result = await response.text();

		// console.log(input, result);

		// return { input, result };
	}
} satisfies Actions;
