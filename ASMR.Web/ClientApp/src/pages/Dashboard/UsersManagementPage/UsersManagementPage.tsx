import { useState } from "react"
import { useHistory } from "react-router-dom"
import {
	IoAddCircleOutline,
	IoCreateOutline,
	IoImageOutline,
	IoKeyOutline,
	IoTrashOutline
} from "react-icons/io5"
import BackButton from "@asmr/components/BackButton"
import BreakpointRenderer from "@asmr/components/BreakpointRenderer"
import Button from "@asmr/components/Button"
import Table from "@asmr/components/Table"
import UserImage from "@asmr/components/UserImage"
import Role from "@asmr/data/enumerations/Role"
import User from "@asmr/data/models/User"
import CreateUserRequestModel from "@asmr/data/request/CreateUserRequestModel"
import UpdateUserPasswordRequestModel from "@asmr/data/request/UpdateUserPasswordRequestModel"
import UpdateUserRequestModel from "@asmr/data/request/UpdateUserRequestModel"
import DashboardLayout from "@asmr/layouts/DashboardLayout"
import lazy from "@asmr/libs/common/lazy"
import { singleSwitchToggle } from "@asmr/libs/common/toggle"
import useAuthentication from "@asmr/libs/hooks/authenticationHook"
import useBreakpoint from "@asmr/libs/hooks/breakpointHook"
import useDocumentTitle from "@asmr/libs/hooks/documentTitleHook"
import useInit from "@asmr/libs/hooks/initHook"
import useLogger from "@asmr/libs/hooks/loggerHook"
import useNotification from "@asmr/libs/hooks/notificationHook"
import useProgress from "@asmr/libs/hooks/progressHook"
import useServices from "@asmr/libs/hooks/servicesHook"
import AuthenticationRoutes from "@asmr/pages/Authentication/AuthenticationRoutes"
import "@asmr/pages/Dashboard/UsersManagementPage/UsersManagementPage.scoped.css"

const UserCreateModal = lazy(() => import("@asmr/pages/Dashboard/UsersManagementPage/UserCreateModal"))
const UserUpdateModal = lazy(() => import("@asmr/pages/Dashboard/UsersManagementPage/UserUpdateModal"))
const UserUpdateImageModal = lazy(() => import("@asmr/pages/Dashboard/UsersManagementPage/UserUpdateImageModal"))
const UserUpdatePasswordModal = lazy(() => import("@asmr/pages/Dashboard/UsersManagementPage/UserUpdatePasswordModal"))
const UserRemoveModal = lazy(() => import("@asmr/pages/Dashboard/UsersManagementPage/UserRemoveModal"))

function UsersManagementPage(): JSX.Element {
	useDocumentTitle("Manage Users")
	useInit(onInit)
	const [users, setUsers] = useState<User[]>([])
	const [selectedUser, setSelectedUser] = useState<User | null>(null)
	const [userCreateModalShown, setUserCreateModalShown] = useState(false)
	const [userUpdateModalShown, setUserUpdateModalShown] = useState(false)
	const [userUpdateImageModalShown, setUserUpdateImageModalShown] = useState(false)
	const [userUpdatePasswordModalShown, setUserUpdatePasswordModalShown] = useState(false)
	const [userRemoveModalShown, setUserRemoveModalShown] = useState(false)
	const authentication = useAuthentication()
	const breakpoint = useBreakpoint()
	const history = useHistory()
	const logger = useLogger(UsersManagementPage)
	const notification = useNotification()
	const [progress] = useProgress()
	const services = useServices()

	async function onInit() {
		await retrieveAllUsers()
	}

	async function onCloseModals() {
		await singleSwitchToggle(undefined, [
			setUserCreateModalShown, setUserUpdateModalShown, setUserUpdateImageModalShown, setUserUpdatePasswordModalShown, setUserRemoveModalShown
		])
		setSelectedUser(null)
	}

	async function onShowCreateUserModalButtonClicked() {
		await singleSwitchToggle(setUserCreateModalShown, [
			setUserUpdateModalShown, setUserUpdateImageModalShown, setUserUpdatePasswordModalShown, setUserRemoveModalShown
		])
	}

	async function onShowUpdateUserModalButtonClicked(user: User) {
		setSelectedUser(user)
		await singleSwitchToggle(setUserUpdateModalShown, [
			setUserCreateModalShown, setUserUpdateImageModalShown, setUserUpdatePasswordModalShown, setUserRemoveModalShown
		])
	}

	async function onShowUpdateUserImageModalButtonClicked(user: User) {
		setSelectedUser(user)
		await singleSwitchToggle(setUserUpdateImageModalShown, [
			setUserCreateModalShown, setUserUpdateModalShown, setUserUpdatePasswordModalShown, setUserRemoveModalShown
		])
	}

	async function onShowUpdateUserPasswordModalButtonClicked(user: User) {
		setSelectedUser(user)
		await singleSwitchToggle(setUserUpdatePasswordModalShown, [
			setUserCreateModalShown, setUserUpdateModalShown, setUserUpdateImageModalShown, setUserRemoveModalShown
		])
	}

	async function onShowRemoveUserModalButtonClicked(user: User) {
		setSelectedUser(user)
		await singleSwitchToggle(setUserRemoveModalShown, [
			setUserCreateModalShown, setUserUpdateModalShown, setUserUpdatePasswordModalShown, setUserUpdateImageModalShown
		])
	}

	async function createUser(requestModel: CreateUserRequestModel, imageFile: File | null) {
		if (!requestModel) {
			return
		}

		try {
			const result = await services.user.create(requestModel, imageFile)
			if (result.isSuccess) {
				if (result.data) {
					const newUser = result.data
					notification.success(`Successfully added ${newUser.firstName} ${newUser.lastName} as new user.`)
				}
				await Promise.all([retrieveAllUsers(), onCloseModals()])
			}

			services.handleErrors(result.errors, notification, logger)
		} catch (error) {
			services.handleError(error, notification, logger)
		}
	}

	async function updateUser(requestModel: UpdateUserRequestModel, imageFile: File | null) {
		if (!selectedUser || !requestModel) {
			return
		}

		try {
			const result = await services.user.modify(selectedUser.id, requestModel, imageFile)
			if (result.isSuccess) {
				if (result.data) {
					const modifiedUser = result.data
					notification.success(`Successfully saved profile information of ${modifiedUser.firstName} ${modifiedUser.lastName}.`)
				}
				await Promise.all([retrieveAllUsers(), onCloseModals(), authentication.updateUserData()])
			}

			services.handleErrors(result.errors, notification, logger)
		} catch (error) {
			services.handleError(error, notification, logger)
		}
	}

	async function updateUserPassword(requestModel: UpdateUserPasswordRequestModel) {
		if (!selectedUser || !requestModel) {
			return
		}

		try {
			const result = await services.user.modifyPassword(selectedUser.id, requestModel)
			if (result.isSuccess) {
				if (result.data) {
					const modifiedUser = result.data
					notification.success(`Successfully changed password for ${modifiedUser.firstName} ${modifiedUser.lastName}.`)

					if (modifiedUser.id === authentication.user?.id) {
						history.push(AuthenticationRoutes.SignOutPage)
						notification.info("Your password has been changed. Please sign-in again.")
					}
				}
				await Promise.all([retrieveAllUsers(), onCloseModals()])
			}

			services.handleErrors(result.errors, notification, logger)
		} catch (error) {
			services.handleError(error, notification, logger)
		}
	}

	async function removeUser() {
		if (!selectedUser) {
			return
		}

		try {
			const result = await services.user.remove(selectedUser.id)
			if (result.isSuccess) {
				if (result.data) {
					const removedUser = result.data
					notification.success(
						`Successfully removed ${removedUser.firstName} ${removedUser.lastName} [${removedUser.username}].`)
				}
				await Promise.all([retrieveAllUsers(), onCloseModals()])
			}

			services.handleErrors(result.errors, notification, logger)
		} catch (error) {
			services.handleError(error, notification, logger)
		}
	}

	async function retrieveAllUsers() {
		try {
			const result = await services.user.getAll()
			if (result.isSuccess && result.data) {
				const users = result.data.sort((a, b) => {
					const aIsAdministrator = a.roles.findIndex(userRole => userRole.role === Role.Administrator) !== -1
					const bIsAdministrator = b.roles.findIndex(userRole => userRole.role === Role.Administrator) !== -1
					if (aIsAdministrator && bIsAdministrator) {
						return 0
					}
					if (aIsAdministrator && !bIsAdministrator) {
						return -1
					}
					return 1
				})
				setUsers(users)
			}

			services.handleErrors(result.errors, notification, logger)
		} catch (error) {
			services.handleError(error, notification, logger)
		}
	}

	function renderUserTableRow(user: User, index: number): JSX.Element {
		const roles = user.roles
			.sort((a, b) => a.role < b.role ? -1 : a.role > b.role ? 1 : 0)
			.map(userRole => Role[userRole.role])

		function ManagementActions() {
			return <>
				<Button disabled={progress.loading} key={0} icon={IoCreateOutline} size="xs" style="outline"
						onClick={() => onShowUpdateUserModalButtonClicked(user)}>Modify</Button>
				<Button disabled={progress.loading} key={1} icon={IoImageOutline} size="xs" style="outline"
						onClick={() => onShowUpdateUserImageModalButtonClicked(user)}>Change Image</Button>
				<Button disabled={progress.loading} key={2} icon={IoKeyOutline} size="xs" style="outline"
						onClick={() => onShowUpdateUserPasswordModalButtonClicked(user)}>Change Password</Button>
				{
					(user.id !== authentication.user?.id) && (
						<Button disabled={progress.loading} key={3} icon={IoTrashOutline} size="xs" style="danger"
								onClick={() => onShowRemoveUserModalButtonClicked(user)}>Remove</Button>
					)
				}
			</>
		}

		function renderRole(role: string, index: number) {
			return <span key={index} className="role">&nbsp;{role}&nbsp;</span>
		}

		return (
			<Table.Row key={index}>
				<Table.DataCell>
					<div className="user-information">
						<div className="user-image-container">
							<UserImage circular clickable user={user} />
						</div>
						<div className="user-names-container">
							<p className="full-name">{user.firstName} {user.lastName}</p>
							<p className="email-address">{user.emailAddress}</p>
							<p className="username">{user.username}</p>
						</div>
					</div>
					<BreakpointRenderer max="md">
						<div className="user-roles">
							{roles.map(renderRole)}
						</div>
						<div className="management-actions">
							<ManagementActions />
						</div>
					</BreakpointRenderer>
				</Table.DataCell>
				<BreakpointRenderer min="lg">
					<Table.DataCell>
						<div className="user-roles">
							{roles.map(renderRole)}
						</div>
					</Table.DataCell>
					<Table.DataCell>
						<div className="management-actions">
							<ManagementActions />
						</div>
					</Table.DataCell>
				</BreakpointRenderer>
			</Table.Row>
		)
	}

	return (
		<DashboardLayout>
			<div className="header">
				<BackButton />&nbsp;&nbsp;
				Users Management
			</div>
			<div className="click-to-actions">
				<Button disabled={progress.loading} icon={IoAddCircleOutline} size="sm"
						onClick={onShowCreateUserModalButtonClicked}>Create User</Button>
			</div>
			<div className="content">
				<div className="table-container">
					<Table>
						<Table.Head>
							<Table.Row>
								<Table.DataCell head>
									{ breakpoint.includes("lg") ? "Name" : "Information" }
								</Table.DataCell>
								<BreakpointRenderer min="lg">
									<Table.DataCell head>Assigned Roles</Table.DataCell>
									<Table.DataCell head>&nbsp;</Table.DataCell>
								</BreakpointRenderer>
							</Table.Row>
						</Table.Head>
						<Table.Body>
						{ users.length > 0 ? users.map(renderUserTableRow) : (
							<Table.Row>
								<Table.DataCell colSpan={breakpoint.includes("lg") ? 3 : 1}>
									{progress.loading ? "Retrieving data from server..." : "There are no users at the moment."}
								</Table.DataCell>
							</Table.Row>
						) }
						</Table.Body>
					</Table>
				</div>
			</div>

			<UserCreateModal onClose={onCloseModals} onCreateUser={createUser} progress={progress} show={userCreateModalShown} />
			<UserUpdateModal onClose={onCloseModals} onUpdateUser={updateUser} progress={progress} show={userUpdateModalShown} user={selectedUser} />
			<UserUpdateImageModal onClose={onCloseModals} onUpdateUser={updateUser} progress={progress} show={userUpdateImageModalShown} user={selectedUser} />
			<UserUpdatePasswordModal onClose={onCloseModals} onUpdateUserPassword={updateUserPassword} progress={progress} show={userUpdatePasswordModalShown} user={selectedUser}  />
			<UserRemoveModal onClose={onCloseModals} onRemoveUser={removeUser} progress={progress} show={userRemoveModalShown} user={selectedUser} />
		</DashboardLayout>
	)
}

export default UsersManagementPage
