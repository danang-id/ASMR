class HttpResponse<TResponse = unknown> {
	private readonly response: Response

	constructor(response: Response, body: TResponse | FormData | string | undefined) {
		this.response = response
		this.body = body

		this.headers = this.response.headers
		this.ok = this.response.ok
		this.statusCode = this.response.status
	}

	public readonly headers: Headers
	public readonly body: TResponse | FormData | string | undefined
	public readonly ok: boolean
	public readonly statusCode: number

	public asArrayBuffer(): Promise<ArrayBuffer> {
		return this.response.arrayBuffer()
	}
	public asBlob(): Promise<Blob> {
		return this.response.blob()
	}

	public asFormData(): Promise<FormData> {
		return this.response.formData()
	}

	public asObject(): Promise<TResponse> {
		return this.response.json()
	}

	public asString(): Promise<string> {
		return this.response.text()
	}
}

export default HttpResponse
