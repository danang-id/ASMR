interface HttpRequestOptions extends RequestInit {
	params?: URLSearchParams | Record<string, any>
}

export default HttpRequestOptions
