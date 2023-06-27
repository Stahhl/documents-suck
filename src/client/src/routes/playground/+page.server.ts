import type { Actions } from './$types';

export async function load({ fetch }) {
	try {
		const body = {
			name: 'world',
			condition: false
		};

		const response: Response = await fetch('http://127.0.0.1:8000/template/confirmation', {
			method: 'POST',
			body: JSON.stringify(body),
			headers: {
				'Content-Type': 'application/json'
			}
		});

		const content = await response.text();

		// console.log(content)

		return { content };
	} catch (error) {}
}

export const actions = {
	template: async (event) => {
		const data = await event.request.formData();
		// console.table([...data.entries()]);
		const content = data.get('input');

		return { content };
	}
} satisfies Actions;
