/* eslint-disable */
module.exports = {
	mode: "jit",
	purge: [
		"./public/**/*.html",
		"./src/**/*.{js,jsx,ts,tsx}",
	],
	darkMode: "class", // or "media" or "class"
	theme: {
		fontFamily: {
			brand: ["Quicksand"],
			serif: ["ui-serif", "Georgia", "Cambria", "'Times New Roman'", "Times", "serif"],
			sans: [
				"'Titillium Web'",
				"ui-sans-serif",
				"system-ui",
				"-apple-system",
				"BlinkMacSystemFont",
				"'Segoe UI'",
				"Roboto",
				"'Helvetica Neue'",
				"Arial",
				"'Noto Sans'",
				"Noto Sans",
				"'Apple Color Emoji'",
				"'Segoe UI Emoji'",
				"'Segoe UI Symbol'",
				"'Noto Color Emoji'"
			],
			mono: ["ui-monospace", "SFMono-Regular", "Menlo", "Monaco", "Consolas", "'Liberation Mono'", "'Courier New'", "monospace"]
		},
		extend: {
			colors: {
				action: "#1D71F9",
				"action-accent": "#2F7DFA",
				danger: "#F32013",
				bean: "#794028",
				primary: "#432D27",
				secondary: "#D7B7A3",
				darkless: "#3A3A3D",
				dark: "#222224",
				darker: "#161617",
				lightless: "#D4CFCF",
				light: "#EAE7E7",
				lighter: "#F5F2F2"
			},
			maxWidth: {
				"2xs": "10rem",
			},
			screens: {
				"print": { raw: "print" },
			},
		},

	},
	variants: {
		extend: {},
	},
	plugins: [
		require("@tailwindcss/forms"),
	],
}
