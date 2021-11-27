import HttpRequestOptions from "asmr/libs/http/HttpRequestOptions"

interface HttpClientOptions extends HttpRequestOptions {
	baseUrl?: string
}

export const DefaultHttpClientOptions: HttpClientOptions = {
	baseUrl: window.location.origin,
	cache: "default",
	credentials: "same-origin",
	headers: {},
	mode: "same-origin",
	params: new URLSearchParams({}),
}

export default HttpClientOptions
