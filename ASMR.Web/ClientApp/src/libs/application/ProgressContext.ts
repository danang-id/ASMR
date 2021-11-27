import { createContext } from "react"
import ProgressContextInfo from "asmr/libs/application/ProgressContextInfo"

const ProgressContext = createContext<ProgressContextInfo>([{ loading: false, percentage: 0 }, () => {}])

export default ProgressContext
