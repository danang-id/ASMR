import { ReactNode } from "react"
import AuthenticationProvider from "asmr/libs/security/AuthenticationProvider"
import ThemeProvider from "asmr/libs/application/ThemeProvider"
import BreakpointProvider from "asmr/libs/application/BreakpointProvider"
import NetworkProvider from "asmr/libs/application/NetworkProvider"
import ProgressProvider from "asmr/libs/application/ProgressProvider"

interface ApplicationProviderProps {
	children: ReactNode
}

function ApplicationProvider({ children }: ApplicationProviderProps): JSX.Element {
	return (
		<ProgressProvider>
			<NetworkProvider>
				<BreakpointProvider>
					<ThemeProvider>
						<AuthenticationProvider>{children}</AuthenticationProvider>
					</ThemeProvider>
				</BreakpointProvider>
			</NetworkProvider>
		</ProgressProvider>
	)
}

export default ApplicationProvider
