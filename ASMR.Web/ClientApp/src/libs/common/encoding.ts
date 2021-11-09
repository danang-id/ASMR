export function hexToASCII(hex: string) {
	let result = ""
	for (let i = 0; i < hex.length; i += 2) {
		result += String.fromCharCode(parseInt(hex.substr(i, 2), 16))
	}
	return result
}

export function base64ToHex(base64: string) {
	const raw = atob(base64)
	let result = ""
	for (let i = 0; i < raw.length; i++) {
		const hex = raw.charCodeAt(i).toString(16)
		result += hex.length === 2 ? hex : "0" + hex
	}
	return result.toUpperCase()
}
