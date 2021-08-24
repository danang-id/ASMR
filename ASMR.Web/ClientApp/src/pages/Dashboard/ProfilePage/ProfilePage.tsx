import { useEffect, useState } from "react"
import { Link } from "react-router-dom"
import dayjs from "dayjs"
import BackButton from "@asmr/components/BackButton"
import UserImage from "@asmr/components/UserImage"
import Role from "@asmr/data/enumerations/Role"
import User, { EmptyUser } from "@asmr/data/models/User"
import DashboardLayout from "@asmr/layouts/DashboardLayout"
import useAuthentication from "@asmr/libs/hooks/authenticationHook"
import useDocumentTitle from "@asmr/libs/hooks/documentTitleHook"
import useInit from "@asmr/libs/hooks/initHook"
import DashboardRoutes from "@asmr/pages/Dashboard/DashboardRoutes"
import "@asmr/pages/Dashboard/ProfilePage/ProfilePage.scoped.css"

function ProfilePage(): JSX.Element {
	useDocumentTitle("Profile")
	useInit(onInit)
	const [isAdministrator, setIsAdministrator] = useState(false)
	const [roles, setRoles] = useState<string[]>([])
	const [user, setUser] = useState<User>(EmptyUser)
	const authentication = useAuthentication()

	async function onInit() {
		await authentication.updateUserData()
	}

	useEffect(() => {
		const newUser = authentication.user ?? EmptyUser
		const newRoles = newUser.roles
			.sort((a, b) => a.role < b.role ? -1 : a.role > b.role ? 1 : 0)
			.map(userRole => Role[userRole.role])
		setIsAdministrator(newRoles.includes(Role[Role.Administrator]))
		setRoles(newRoles)
		setUser(newUser)
	}, [authentication.user])

	function renderRole(role: string, index: number) {
		return <span key={index} className="role">{role}</span>
	}

	return (
		<DashboardLayout>
			<div className="header">
				<BackButton />&nbsp;&nbsp;
				My Profile
			</div>
			<div className="content">
				<div className="profile">
					<div className="profile-image">
						<UserImage clickable user={user} />
					</div>
					<div className="profile-information">
						<div className="full-name">{user.firstName} {user.lastName}</div>
						<div className="email-address">{user.emailAddress}</div>
						<div className="username">{user.username}</div>
						<div className="roles">
							{roles && roles.map(renderRole)}
						</div>
					</div>
				</div>
				<div className="notes">
					<span className="registration-information">
						Registered on {dayjs(user.createdAt).format("dddd, D MMMM YYYY [at] hh:mm:ss A ([UTC]Z)")}.
					</span>
					<span className="update-information">
						{isAdministrator ? <>
							Please use the <Link to={DashboardRoutes.UsersManagementPage}>User Management</Link> panel to update your information.
						</> : <>
							To update your profile information (including your password), please contact the Administrator.
						</>}
					</span>
				</div>
			</div>
		</DashboardLayout>
	)
}

export default ProfilePage
