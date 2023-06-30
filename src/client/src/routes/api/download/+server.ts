// import { json } from '@sveltejs/kit';
// import type { RequestHandler } from './$types';
// import fs from 'fs';
// import {pipeline} from "stream";

// // 'http://localhost:5099/document/playground/pdf'

// export const POST = (async ({ request }) => {
// 	const data = await request.text();
// 	console.log(`data: ${data}`);

// 	const response: Response = await fetch('http://localhost:5099/document/playground/pdf', {
// 		method: 'POST',
// 		body: data,
// 		headers: {
// 			'Content-Type': 'application/json'
// 		}
// 	});

// 	if (response.ok) {
// 		const fileName = 'filename.ext'; // Specify the desired file name and extension
// 		const filePath = '/path/to/save/' + fileName; // Specify the desired file path

// 		const fileStream = fs.createWriteStream(filePath);

// 		pipeline(response.body, fileStream, (error) => {
// 			if (error) {
// 				console.error('Error:', error);
// 			} else {
// 				console.log('File downloaded successfully.');
// 			}
// 		});
// 	} else {
// 		console.error('Error:', response.status, response.statusText);
// 	}

// 	return json(data);
// }) satisfies RequestHandler;
