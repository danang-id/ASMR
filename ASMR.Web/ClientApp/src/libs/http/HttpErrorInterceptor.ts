type HttpErrorInterceptor = (error: Error) => Error | Promise<Error>

export default HttpErrorInterceptor
