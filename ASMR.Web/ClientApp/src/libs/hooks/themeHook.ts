import { useContext } from "react"
import ThemeContext from "asmr/libs/application/ThemeContext"

function useTheme() {
	return useContext(ThemeContext)
}

export default useTheme
