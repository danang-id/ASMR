import { useState } from "react"
import { IoAddCircleOutline, IoCreateOutline, IoImageOutline, IoKeyOutline, IoTrashOutline } from "react-icons/io5"
import BackButton from "@asmr/components/BackButton"
import BreakpointRenderer from "@asmr/components/BreakpointRenderer"
import Button from "@asmr/components/Button"
import Table from "@asmr/components/Table"
import UserImage from "@asmr/components/UserImage"
import Role from "@asmr/data/enumerations/Role"
import User from "@asmr/data/models/User"
import CreateUserRequestModel from "@asmr/data/request/CreateUserRequestModel"
import UpdateUserRequestModel from "@asmr/data/request/UpdateUserRequestModel"
import ApproveRegistrationRequestModel from "@asmr/data/request/ApproveRegistrationRequestModel"
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
import "@asmr/pages/Dashboard/UsersManagementPage/UsersManagementPage.scoped.css"

const UserCreateModal = lazy(() => import("@asmr/pages/Dashboard/UsersManagementPage/UserCreateModal"))
const UserUpdateModal = lazy(() => import("@asmr/pages/Dashboard/UsersManagementPage/UserUpdateModal"))
const UserUpdateImageModal = lazy(() => import("@asmr/pages/Dashboard/UsersManagementPage/UserUpdateImageModal"))
const UserResetPasswordModal = lazy(() => import("@asmr/pages/Dashboard/UsersManagementPage/UserResetPasswordModal"))
const UserRemoveModal = lazy(() => import("@asmr/pages/Dashboard/UsersManagementPage/UserRemoveModal"))
const RegistrationApproveModal = lazy(
	() => import("@asmr/pages/Dashboard/UsersManagementPage/RegistrationApproveModal")
)
const RegistrationRejectModal = lazy(() => import("@asmr/pages/Dashboard/UsersManagementPage/RegistrationRejectModal"))

function UsersManagementPage(): JSX.Element {
	useDocumentTitle("Manage Users")
	useInit(onInit)
	const [users, setUsers] = useState<User[]>([])
	const [selectedUser, setSelectedUser] = useState<User | null>(null)
	const [userCreateModalShown, setUserCreateModalShown] = useState(false)
	const [userUpdateModalShown, setUserUpdateModalShown] = useState(false)
	const [userUpdateImageModalShown, setUserUpdateImageModalShown] = useState(false)
	const [userUpdatePasswordModalShown, setUserResetPasswordModalShown] = useState(false)
	const [userRemoveModalShown, setUserRemoveModalShown] = useState(false)
	const [registrationApproveModalShown, setRegistrationApproveModalShown] = useState(false)
	const [registrationRejectModalShown, setRegistrationRejectModalShown] = useState(false)
	const authentication = useAuthentication()
	const breakpoint = useBreakpoint()
	const logger = useLogger(UsersManagementPage)
	const notification = useNotification()
	const [progress] = useProgress()
	const services = useServices()

	async function onInit() {
		await retrieveAllUsers()
	}

	async function onCloseModals() {
		await singleSwitchToggle(undefined, [
			setUserCreateModalShown,
			setUserUpdateModalShown,
			setUserUpdateImageModalShown,
			setUserResetPasswordModalShown,
			setUserRemoveModalShown,
			setRegistrationApproveModalShown,
			setRegistrationRejectModalShown,
		])
		setSelectedUser(null)
	}

	async function onShowCreateUserModalButtonClicked() {
		await singleSwitchToggle(setUserCreateModalShown, [
			setUserUpdateModalShown,
			setUserUpdateImageModalShown,
			setUserResetPasswordModalShown,
			setUserRemoveModalShown,
			setRegistrationApproveModalShown,
			setRegistrationRejectModalShown,
		])
	}

	async function onShowUpdateUserModalButtonClicked(user: User) {
		setSelectedUser(user)
		await singleSwitchToggle(setUserUpdateModalShown, [
			setUserCreateModalShown,
			setUserUpdateImageModalShown,
			setUserResetPasswordModalShown,
			setUserRemoveModalShown,
			setRegistrationApproveModalShown,
			setRegistrationRejectModalShown,
		])
	}

	async function onShowUpdateUserImageModalButtonClicked(user: User) {
		setSelectedUser(user)
		await singleSwitchToggle(setUserUpdateImageModalShown, [
			setUserCreateModalShown,
			setUserUpdateModalShown,
			setUserResetPasswordModalShown,
			setUserRemoveModalShown,
			setRegistrationApproveModalShown,
			setRegistrationRejectModalShown,
		])
	}

	async function onShowResetUserPasswordModalButtonClicked(user: User) {
		setSelectedUser(user)
		await singleSwitchToggle(setUserResetPasswordModalShown, [
			setUserCreateModalShown,
			setUserUpdateModalShown,
			setUserUpdateImageModalShown,
			setUserRemoveModalShown,
			setRegistrationApproveModalShown,
			setRegistrationRejectModalShown,
		])
	}

	async function onShowRemoveUserModalButtonClicked(user: User) {
		setSelectedUser(user)
		await singleSwitchToggle(setUserRemoveModalShown, [
			setUserCreateModalShown,
			setUserUpdateModalShown,
			setUserResetPasswordModalShown,
			setUserUpdateImageModalShown,
			setRegistrationApproveModalShown,
			setRegistrationRejectModalShown,
		])
	}

	async function onShowApproveRegistrationModalButtonClicked(user: User) {
		setSelectedUser(user)
		await singleSwitchToggle(setRegistrationApproveModalShown, [
			setUserCreateModalShown,
			setUserUpdateModalShown,
			setUserResetPasswordModalShown,
			setUserUpdateImageModalShown,
			setUserRemoveModalShown,
			setRegistrationRejectModalShown,
		])
	}

	async function onShowRejectRegistrationModalButtonClicked(user: User) {
		setSelectedUser(user)
		await singleSwitchToggle(setRegistrationRejectModalShown, [
			setUserCreateModalShown,
			setUserUpdateModalShown,
			setUserResetPasswordModalShown,
			setUserUpdateImageModalShown,
			setUserRemoveModalShown,
			setRegistrationApproveModalShown,
		])
	}

	async function createUser(requestModel: CreateUserRequestModel, imageFile: File | null) {
		if (!requestModel) {
			return
		}

		try {
			const result = await services.user.create(requestModel, imageFile)
			if (result.isSuccess) {
				notification.success(result.message)
				await Promise.all([retrieveAllUsers(), onCloseModals()])
			}

			services.handleErrors(result.errors, notification, logger)
		} catch (error) {
			services.handleError(error as Error, notification, logger)
		}
	}

	async function updateUser(requestModel: UpdateUserRequestModel, imageFile: File | null) {
		if (!selectedUser || !requestModel) {
			return
		}

		try {
			const result = await services.user.modify(selectedUser.id, requestModel, imageFile)
			if (result.isSuccess) {
				notification.success(result.message)
				await Promise.all([retrieveAllUsers(), onCloseModals(), authentication.updateUserData()])
			}

			services.handleErrors(result.errors, notification, logger)
		} catch (error) {
			services.handleError(error as Error, notification, logger)
		}
	}

	async function resetUserPassword() {
		if (!selectedUser) {
			return
		}

		try {
			const result = await services.user.resetPassword(selectedUser.id)
			if (result.isSuccess) {
				notification.success(result.message)
				await Promise.all([retrieveAllUsers(), onCloseModals()])
			}

			services.handleErrors(result.errors, notification, logger)
		} catch (error) {
			services.handleError(error as Error, notification, logger)
		}
	}

	async function removeUser() {
		if (!selectedUser) {
			return
		}

		try {
			const result = await services.user.remove(selectedUser.id)
			if (result.isSuccess) {
				notification.success(result.message)
				await Promise.all([retrieveAllUsers(), onCloseModals()])
			}

			services.handleErrors(result.errors, notification, logger)
		} catch (error) {
			services.handleError(error as Error, notification, logger)
		}
	}

	async function approveRegistration(requestModel: ApproveRegistrationRequestModel) {
		if (!selectedUser) {
			return
		}

		try {
			const result = await services.user.approve(selectedUser.id, requestModel)
			if (result.isSuccess) {
				notification.success(result.message)
				await Promise.all([retrieveAllUsers(), onCloseModals()])
			}

			services.handleErrors(result.errors, notification, logger)
		} catch (error) {
			services.handleError(error as Error, notification, logger)
		}
	}

	async function rejectRegistration() {
		if (!selectedUser) {
			return
		}

		try {
			const result = await services.user.reject(selectedUser.id)
			if (result.isSuccess) {
				notification.success(result.message)
				await Promise.all([retrieveAllUsers(), onCloseModals()])
			}

			services.handleErrors(result.errors, notification, logger)
		} catch (error) {
			services.handleError(error as Error, notification, logger)
		}
	}

	async function retrieveAllUsers() {
		try {
			const result = await services.user.getAll()
			if (result.isSuccess && result.data) {
				const users = result.data.sort((a, b) => {
					const aIsAdministrator =
						a.roles.findIndex((userRole) => userRole.role === Role.Administrator) !== -1
					const bIsAdministrator =
						b.roles.findIndex((userRole) => userRole.role === Role.Administrator) !== -1
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
			services.handleError(error as Error, notification, logger)
		}
	}

	function renderUserTableRow(user: User, index: number): JSX.Element {
		const roles = user.roles
			.sort((a, b) => (a.role < b.role ? -1 : a.role > b.role ? 1 : 0))
			.map((userRole) => Role[userRole.role])

		function ManagementActions() {
			if (!user.isEmailConfirmed) {
				return <></>
			}

			if (user.isWaitingForApproval) {
				return (
					<>
						<Button
							disabled={progress.loading}
							key={0}
							icon={IoCreateOutline}
							size="xs"
							style="outline"
							onClick={() => onShowRejectRegistrationModalButtonClicked(user)}
						>
							Reject
						</Button>
						<Button
							disabled={progress.loading}
							key={1}
							icon={IoCreateOutline}
							size="xs"
							style="filled"
							onClick={() => onShowApproveRegistrationModalButtonClicked(user)}
						>
							Approve
						</Button>
					</>
				)
			}

			if (!user.isApproved) {
				return (
					<>
						<Button
							disabled={progress.loading}
							key={0}
							icon={IoCreateOutline}
							size="xs"
							style="filled"
							onClick={() => onShowApproveRegistrationModalButtonClicked(user)}
						>
							Approve
						</Button>
						{user.id !== authentication.user?.id && (
							<Button
								disabled={progress.loading}
								key={1}
								icon={IoTrashOutline}
								size="xs"
								style="danger"
								onClick={() => onShowRemoveUserModalButtonClicked(user)}
							>
								Remove
							</Button>
						)}
					</>
				)
			}

			return (
				<>
					<Button
						disabled={progress.loading}
						key={0}
						icon={IoCreateOutline}
						size="xs"
						style="outline"
						onClick={() => onShowUpdateUserModalButtonClicked(user)}
					>
						Modify
					</Button>
					<Button
						disabled={progress.loading}
						key={1}
						icon={IoImageOutline}
						size="xs"
						style="outline"
						onClick={() => onShowUpdateUserImageModalButtonClicked(user)}
					>
						Change Image
					</Button>
					<Button
						disabled={progress.loading}
						key={2}
						icon={IoKeyOutline}
						size="xs"
						style="outline"
						onClick={() => onShowResetUserPasswordModalButtonClicked(user)}
					>
						Reset Password
					</Button>
					{user.id !== authentication.user?.id && (
						<Button
							disabled={progress.loading}
							key={3}
							icon={IoTrashOutline}
							size="xs"
							style="danger"
							onClick={() => onShowRemoveUserModalButtonClicked(user)}
						>
							Remove
						</Button>
					)}
				</>
			)
		}

		function renderRole(role: string, index: number) {
			return (
				<span key={index} className="role">
					&nbsp;{role}&nbsp;
				</span>
			)
		}

		function renderUserRole() {
			if (!user.isEmailConfirmed) {
				return <span className="approval approval-email">Email Not Confirmed</span>
			}

			return !user.isWaitingForApproval ? (
				user.isApproved ? (
					roles.map(renderRole)
				) : (
					<span className="approval approval-rejected">Registration Rejected</span>
				)
			) : (
				<span className="approval approval-waiting">Waiting for Approval</span>
			)
		}

		return (
			<Table.Row key={index}>
				<Table.DataCell>
					<div className="user-information">
						<div className="user-image-container">
							<UserImage circular clickable user={user} />
						</div>
						<div className="user-names-container">
							<p className="full-name">
								{user.firstName} {user.lastName}
							</p>
							<p className="email-address">{user.emailAddress}</p>
							<p className="username">{user.username}</p>
						</div>
					</div>
					<BreakpointRenderer max="md">
						<div className="user-roles">{renderUserRole()}</div>
						<div className="management-actions">
							<ManagementActions />
						</div>
					</BreakpointRenderer>
				</Table.DataCell>
				<BreakpointRenderer min="lg">
					<Table.DataCell>
						<div className="user-roles">{renderUserRole()}</div>
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
				<BackButton />
				&nbsp;&nbsp; Users Management
			</div>
			<div className="click-to-actions">
				<Button
					disabled={progress.loading}
					icon={IoAddCircleOutline}
					size="sm"
					onClick={onShowCreateUserModalButtonClicked}
				>
					Create User
				</Button>
			</div>
			<div className="content">
				<div className="table-container">
					<Table>
						<Table.Head>
							<Table.Row>
								<Table.DataCell head>
									{breakpoint.includes("lg") ? "Name" : "Information"}
								</Table.DataCell>
								<BreakpointRenderer min="lg">
									<Table.DataCell head>Status / Assigned Roles</Table.DataCell>
									<Table.DataCell head>&nbsp;</Table.DataCell>
								</BreakpointRenderer>
							</Table.Row>
						</Table.Head>
						<Table.Body>
							{users.length > 0 ? (
								users.map(renderUserTableRow)
							) : (
								<Table.Row>
									<Table.DataCell colSpan={breakpoint.includes("lg") ? 3 : 1}>
										{progress.loading
											? "Retrieving data from server..."
											: "There are no users at the moment."}
									</Table.DataCell>
								</Table.Row>
							)}
						</Table.Body>
					</Table>
				</div>
			</div>

			<UserCreateModal
				onClose={onCloseModals}
				onCreateUser={createUser}
				progress={progress}
				show={userCreateModalShown}
			/>
			<UserUpdateModal
				onClose={onCloseModals}
				onUpdateUser={updateUser}
				progress={progress}
				show={userUpdateModalShown}
				user={selectedUser}
			/>
			<UserUpdateImageModal
				onClose={onCloseModals}
				onUpdateUser={updateUser}
				progress={progress}
				show={userUpdateImageModalShown}
				user={selectedUser}
			/>
			<UserResetPasswordModal
				onClose={onCloseModals}
				onResetUserPassword={resetUserPassword}
				progress={progress}
				show={userUpdatePasswordModalShown}
				user={selectedUser}
			/>
			<UserRemoveModal
				onClose={onCloseModals}
				onRemoveUser={removeUser}
				progress={progress}
				show={userRemoveModalShown}
				user={selectedUser}
			/>
			<RegistrationApproveModal
				onClose={onCloseModals}
				onApproveRegistration={approveRegistration}
				progress={progress}
				show={registrationApproveModalShown}
				user={selectedUser}
			/>
			<RegistrationRejectModal
				onClose={onCloseModals}
				onRejectRegistration={rejectRegistration}
				progress={progress}
				show={registrationRejectModalShown}
				user={selectedUser}
			/>
		</DashboardLayout>
	)
}

export default UsersManagementPage
