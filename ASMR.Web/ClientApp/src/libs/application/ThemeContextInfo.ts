export type Theme = "dark" | "light"
export type ThemeContextInfo = [Theme, (theme: Theme) => void]

export default ThemeContextInfo
