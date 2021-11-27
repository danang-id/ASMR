import Logger, { ILogger, NoLogger } from "asmr/libs/common/logger"

function useLogger(fun: Function): ILogger
function useLogger(name: string): ILogger
function useLogger(nameOrFunction: Function | string): ILogger {
	if (process.env.NODE_ENV === "production") {
		return new NoLogger()
	}

	return new Logger(typeof nameOrFunction === "string" ? nameOrFunction : nameOrFunction.name)
}

export default useLogger
