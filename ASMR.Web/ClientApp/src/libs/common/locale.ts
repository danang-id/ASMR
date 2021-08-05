export function toLocale(value: number) {
	return new Intl.NumberFormat("id-ID", {
		style: "decimal",
		maximumFractionDigits: 2,
	}).format(value)
}

export function toLocalCurrency(value: number) {
	return "Rp" + toLocale(value)
}

export function getCurrentLocale(): string {
	if (window.navigator.languages) {
		return window.navigator.languages[0]
	}

	return (window.navigator as any).userLanguage || window.navigator.language
}

export function toLocaleUnit(value: number, unit: string): string {
	const isGreaterThanThousand = value >= 1000
	if (isGreaterThanThousand) {
		value = value / 1000
		unit = `kilo${unit}`
	}
	const isSingular = value === 1 || value === 0 || value === -1
	return `${toLocale(value)} ${unit}${!isSingular ? "s" : ""}`
}
