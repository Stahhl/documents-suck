export default function submitContent(content: string | undefined) {
	if (content === undefined) {
		console.log('content is undefined');
		return;
	}

	console.log(content);
}
