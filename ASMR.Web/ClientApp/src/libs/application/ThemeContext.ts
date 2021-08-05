import { createContext } from "react"
import ThemeContextInfo from "@asmr/libs/application/ThemeContextInfo"

const ThemeContext = createContext<ThemeContextInfo>(["light", () => {}])

export default ThemeContext
