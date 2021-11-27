import { createContext } from "react"

import AuthenticationContextInfo from "asmr/libs/security/AuthenticationContextInfo"

const AuthenticationContext = createContext<AuthenticationContextInfo>({
	abort: () => {},
	handleError: () => {},
	handleErrors: () => {},
	isAuthenticated: () => false,
	isAuthorized: () => false,
	signIn: () => Promise.reject(),
	signOut: () => Promise.reject(),
	updateUserData: () => Promise.reject(),
})

export default AuthenticationContext
