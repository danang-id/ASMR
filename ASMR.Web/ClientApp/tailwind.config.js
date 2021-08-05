/* eslint-disable */
module.exports = {
	purge: [
		"./public/**/*.html",
		"./src/**/*.{js,jsx,ts,tsx}",
	],
	darkMode: "class", // or "media" or "class"
	theme: {
		fontFamily: {
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
				primary: {
					light: "#846A3E",
					DEFAULT: "#4A3C23",
					dark: "#2F2616",
				},
				secondary: {
					light: "#FBF9F9",
					DEFAULT: "#F2EAEA",
					dark: "#CBAAAA"
				},
				dark: "#2B2B2D",
				light: "#F9FAFB"
			},
			backgroundColor: ["disabled"],
			borderColor: ["disabled"],
			maxWidth: {
				"2xs": "10rem",
			},
			screens: {
				"print": { raw: "print" },
			},
			textColor: ["disabled"],
		},

	},
	variants: {
		extend: {},
	},
	plugins: [
		require("@tailwindcss/forms"),
	],
}
