export default async function submitContent( content: string | undefined) {
	if (content === undefined) {
		console.log('content is undefined');
		return;
	}

	console.log(content);

	const response : Response = await fetch('http://localhost:5099/foo', {
		method: 'POST',
		body: content,
		headers: {
			'Content-Type': 'text/plain'
		}
	});

	console.log(response.status)
}
