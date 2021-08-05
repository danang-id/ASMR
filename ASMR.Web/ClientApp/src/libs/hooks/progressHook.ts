import { useContext } from "react"
import ProgressContext from "@asmr/libs/application/ProgressContext"

function useProgress() {
	return useContext(ProgressContext)
}

export default useProgress
