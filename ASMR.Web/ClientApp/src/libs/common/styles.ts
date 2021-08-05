export function combineClassNames(...classNames: Array<string | undefined>) {
	let classList: string[] = []
	for (const className of classNames) {
		if (className) {
			classList = classList.concat(className.split(" "))
		}
	}
	return classList.join(" ")
}
