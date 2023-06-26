<script lang="ts">
	import submitContent from '../../utils/utils.js';
	export let data;

	function handleSubmit() {
		let content = document.querySelector('.content');

		submitContent(content?.innerHTML);
	}
</script>

<div id="header">
	<h1>Playground</h1>
</div>
<div class="container">
	<div class="box" id="editor" style="background-color: lightgray;">
		<form method="POST" on:submit|preventDefault={handleSubmit}>
			<button class="button">Run</button>
		</form>
		<form method="POST" on:submit|preventDefault={handleSubmit}>
			<button class="button">Foo</button>
		</form>
	</div>
	<div id="result">
		<div class="content">
			{@html data.content}
		</div>
	</div>
</div>

<style>
	#header {
		background-color: #e10098;
		color: #ffffff;
		padding: 10px;
		text-align: center;
	}

	.box {
		display: flex;
		flex-direction: row;
	}

	.container {
		display: flex;
		flex-direction: row;
		height: 88vh;
	}

	#sidebar {
		background-color: #f6f8fa;
		width: 20%;
		padding: 20px;
	}

	#editor {
		flex: 1;
		padding: 20px;
		position: relative;
	}

	#result {
		flex: 1;
		padding: 20px;
		background-color: #ffffff;
	}

	.button {
		position: absolute;
		top: 10px;
		right: 10px;
		padding: 15px 25px;
		font-size: 24px;
		text-align: center;
		cursor: pointer;
		outline: none;
		color: #fff;
		background-color: #04aa6d;
		border: none;
		border-radius: 15px;
		box-shadow: 0 9px #999;
	}

	.button:hover {
		background-color: #3e8e41;
	}

	.button:active {
		background-color: #3e8e41;
		box-shadow: 0 5px #666;
		transform: translateY(4px);
	}
</style>
