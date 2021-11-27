// @ts-ignore
module.exports =  {
	plugins: [
		{
			plugin: require("craco-alias"),
			options: {
				source: "options",
				baseUrl: "./",
				aliases: {
					"asmr": "./src/",
					"asmr/*": "./src/*",
				},
			},
		},
		{ plugin: require("craco-plugin-scoped-css") },
		// {
		// 	plugin: require("craco-image-optimizer-plugin"),
		// 	options: {
		// 		mozjpeg: {
		// 			progressive: true,
		// 			quality: 75,
		// 		},
		// 		optipng: {
		// 			enabled: false,
		// 		},
		// 		pngquant: {
		// 			quality: [0.65, 0.9],
		// 			speed: 4,
		// 		},
		// 		gifsicle: {
		// 			interlaced: false,
		// 		},
		// 		webp: {
		// 			quality: 75,
		// 		},
		// 	},
		// },
	],
	style: {
		postcss: {
			plugins: [
				require("postcss-import"),
				require("postcss-nested"),
				require("tailwindcss"),
				require("autoprefixer"),
			],
		},
	},
}
