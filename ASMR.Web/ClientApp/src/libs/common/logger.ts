import environment from "asmr/libs/common/environment"

type WriterFunction = (...args: any[]) => void

type LoggerOptions = {
	disableOnProduction: boolean
}

const defaultLoggerOptions: LoggerOptions = {
	disableOnProduction: true,
}

export interface ILogger {
	error(...args: any[]): void
	info(...args: any[]): void
	warn(...args: any[]): void
}

class Logger implements ILogger {
	private readonly name: string
	private readonly options: LoggerOptions

	constructor(name: string, options: LoggerOptions = defaultLoggerOptions) {
		this.name = name
		this.options = { ...defaultLoggerOptions, ...options }
	}

	private write(write: WriterFunction, ...args: string[]) {
		if (this.options.disableOnProduction && environment.isProduction) {
			return
		}

		write(`[${this.name}]`, ...args)
	}

	public error(...args: any[]) {
		this.write(console.error.bind(this), ...args)
	}

	public info(...args: any[]) {
		this.write(console.info.bind(this), ...args)
	}

	public warn(...args: any[]) {
		this.write(console.warn.bind(this), ...args)
	}
}

export class NoLogger implements ILogger {
	public error() {}
	public info() {}
	public warn() {}
}

export default Logger
