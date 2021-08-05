
export function getBaseUrl(_window: Window = window) {
	const { location } = _window
	return location .protocol + "//" + location.host + "/" + location.pathname.split("/")[1]
}
