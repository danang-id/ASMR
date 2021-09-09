
import { useState } from "react"
import { useHistory } from "react-router-dom"
import {
	IoAddCircleOutline, IoBagOutline,
	IoCreateOutline,
	IoImageOutline,
	IoQrCodeOutline,
	IoTrashOutline
} from "react-icons/io5"
import BackButton from "@asmr/components/BackButton"
import BeanDescription from "@asmr/components/BeanDescription"
import BeanImage from "@asmr/components/BeanImage"
import BreakpointRenderer from "@asmr/components/BreakpointRenderer"
import Button from "@asmr/components/Button"
import Table from "@asmr/components/Table"
import Bean from "@asmr/data/models/Bean"
import BeanInventory from "@asmr/data/models/BeanInventory"
import CreateBeanRequestModel from "@asmr/data/request/CreateBeanRequestModel"
import UpdateBeanRequestModel from "@asmr/data/request/UpdateBeanRequestModel"
import DashboardLayout from "@asmr/layouts/DashboardLayout"
import lazy from "@asmr/libs/common/lazy"
import { toLocaleUnit } from "@asmr/libs/common/locale"
import { singleSwitchToggle } from "@asmr/libs/common/toggle"
import useDocumentTitle from "@asmr/libs/hooks/documentTitleHook"
import useInit from "@asmr/libs/hooks/initHook"
import useLogger from "@asmr/libs/hooks/loggerHook"
import useNotification from "@asmr/libs/hooks/notificationHook"
import useProgress from "@asmr/libs/hooks/progressHook"
import useServices from "@asmr/libs/hooks/servicesHook"
import DashboardRoutes from "@asmr/pages/Dashboard/DashboardRoutes"
import PublicRoutes from "@asmr/pages/Public/PublicRoutes"
import "@asmr/pages/Dashboard/BeanManagementPage/BeansManagementPage.scoped.css"

const BeanCreateModal = lazy(() => import("@asmr/pages/Dashboard/BeanManagementPage/BeanCreateModal"))
const BeanUpdateModal = lazy(() => import("@asmr/pages/Dashboard/BeanManagementPage/BeanUpdateModal"))
const BeanUpdateImageModal = lazy(() => import("@asmr/pages/Dashboard/BeanManagementPage/BeanUpdateImageModal"))
const BeanRemoveModal = lazy(() => import("@asmr/pages/Dashboard/BeanManagementPage/BeanRemoveModal"))

function BeansManagementPage(): JSX.Element {
	useDocumentTitle("Manage Beans")
	useInit(onInit)
	const [beans, setBeans] = useState<Bean[]>([])
	const [selectedBean, setSelectedBean] = useState<Bean | null>(null)
	const [beanCreateModalShown, setBeanCreateModalShown] = useState(false)
	const [beanUpdateModalShown, setBeanUpdateModalShown] = useState(false)
	const [beanUpdateImageModalShown, setBeanUpdateImageModalShown] = useState(false)
	const [beanRemoveModalShown, setBeanRemoveModalShown] = useState(false)
	const history = useHistory()
	const logger = useLogger(BeansManagementPage)
	const notification = useNotification()
	const [progress] = useProgress()
	const services = useServices()

	async function onInit() {
		await retrieveAllGreenBeans()
	}

	async function onCloseModals() {
		await singleSwitchToggle(undefined, [
			setBeanCreateModalShown,
			setBeanUpdateModalShown,
			setBeanUpdateImageModalShown,
			setBeanRemoveModalShown
		])
		setSelectedBean(null)
	}

	function onShowBeanInformationPageButtonClicked(bean: Bean) {
		window.open(`${PublicRoutes.BeanInformationPage}/${bean.id}`, "_blank")
	}

	function onShowProductManagementPageButtonClicked(bean: Bean) {
		history.push(`${DashboardRoutes.ProductsManagementPage}?beanId=${bean.id}`)
	}

	async function onShowBeanCreateModalButtonClicked() {
		await singleSwitchToggle(setBeanCreateModalShown, [
			setBeanUpdateModalShown,
			setBeanUpdateImageModalShown,
			setBeanRemoveModalShown
		])
	}

	async function onShowBeanUpdateModalButtonClicked(bean: Bean) {
		setSelectedBean(bean)
		await singleSwitchToggle(setBeanUpdateModalShown, [
			setBeanCreateModalShown,
			setBeanUpdateImageModalShown,
			setBeanRemoveModalShown
		])
	}

	async function onShowBeanUpdateImageModalButtonClicked(bean: Bean) {
		setSelectedBean(bean)
		await singleSwitchToggle(setBeanUpdateImageModalShown, [
			setBeanCreateModalShown,
			setBeanUpdateModalShown,
			setBeanRemoveModalShown
		])
	}

	async function onShowBeanRemoveModalButtonClicked(bean: Bean) {
		setSelectedBean(bean)
		await singleSwitchToggle(setBeanRemoveModalShown, [
			setBeanCreateModalShown,
			setBeanUpdateModalShown,
			setBeanUpdateImageModalShown
		])
	}

	async function createBean(requestModel: CreateBeanRequestModel, imageFile: File | null) {
		if (!requestModel) {
			return
		}

		try {
			const result = await services.bean.create(requestModel, imageFile)
			if (result.isSuccess) {
				notification.success(result.message)
				await Promise.all([retrieveAllGreenBeans(), onCloseModals()])
			}

			services.handleErrors(result.errors, notification, logger)
		} catch (error) {
			services.handleError(error as Error, notification, logger)
		}
	}

	async function updateBean(requestModel: UpdateBeanRequestModel, imageFile: File | null) {
		if (!selectedBean || !requestModel) {
			return
		}

		try {
			const result = await services.bean.modify(selectedBean.id, requestModel, imageFile)
			if (result.isSuccess) {
				notification.success(result.message)
				await Promise.all([retrieveAllGreenBeans(), onCloseModals()])
			}

			services.handleErrors(result.errors, notification, logger)
		} catch (error) {
			services.handleError(error as Error, notification, logger)
		}
	}

	async function removeBean() {
		if (!selectedBean) {
			return
		}

		try {
			const result = await services.bean.remove(selectedBean.id)
			if (result.isSuccess) {
				notification.success(result.message)
				await Promise.all([retrieveAllGreenBeans(), onCloseModals()])
			}

			services.handleErrors(result.errors, notification, logger)
		} catch (error) {
			services.handleError(error as Error, notification, logger)
		}
	}

	async function retrieveAllGreenBeans() {
		try {
			const result = await services.bean.getAll()
			if (result.isSuccess && result.data) {
				setBeans(result.data)
			}

			services.handleErrors(result.errors, notification, logger)
		} catch (error) {
			services.handleError(error as Error, notification, logger)
		}
	}

	function renderBeanInventory(beanInventory: BeanInventory): JSX.Element {
		return <div className="bean-inventory">
			<p>Green Bean: {toLocaleUnit(beanInventory.currentGreenBeanWeight, "gram")}</p>
			<p>Roasted Bean: {toLocaleUnit(beanInventory.currentRoastedBeanWeight, "gram")}</p>
		</div>
	}

	function renderBeanTableRow(bean: Bean, index: number): JSX.Element {
		function ManagementActions() {
			return <>
				<Button disabled={progress.loading} key={0} icon={IoQrCodeOutline} size="xs" style="filled"
						onClick={() => onShowBeanInformationPageButtonClicked(bean)}>Show QR Code</Button>
				<Button disabled={progress.loading} key={1} icon={IoBagOutline} size="xs" style="outline"
						onClick={() => onShowProductManagementPageButtonClicked(bean)}>Manage Products</Button>
				<Button disabled={progress.loading} key={2} icon={IoCreateOutline} size="xs" style="outline"
						onClick={() => onShowBeanUpdateModalButtonClicked(bean)}>Modify</Button>
				<Button disabled={progress.loading} key={3} icon={IoImageOutline} size="xs" style="outline"
						onClick={() => onShowBeanUpdateImageModalButtonClicked(bean)}>Change Image</Button>
				<Button disabled={progress.loading} key={4} icon={IoTrashOutline} size="xs" style="danger"
						onClick={() => onShowBeanRemoveModalButtonClicked(bean)}>Remove</Button>
			</>
		}

		return (
			<Table.Row key={index}>
				<Table.DataCell>
					<div className="information">
						<div className="bean-image-container">
							<div className="bean-image">
								<BeanImage bean={bean} clickable />
							</div>
						</div>
						<div className="detail">
							<div className="name">{bean.name}</div>
							<div className="description">
								<BeanDescription description={bean.description} />
							</div>
							<BreakpointRenderer max="md">
								{renderBeanInventory(bean.inventory)}
								<div className="management-actions">
									<ManagementActions />
								</div>
							</BreakpointRenderer>
						</div>
					</div>
				</Table.DataCell>
				<BreakpointRenderer min="lg">
					<Table.DataCell>
						{renderBeanInventory(bean.inventory)}
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
		<DashboardLayout className="page">
			<div className="header">
				<BackButton />&nbsp;&nbsp;
				Beans Management
			</div>

			<div className="click-to-actions">
				<Button disabled={progress.loading} icon={IoAddCircleOutline} size="sm"
						onClick={onShowBeanCreateModalButtonClicked}>Add Bean</Button>
			</div>

			<div className="content">
				<div className="table-container">
					<Table>
						<Table.Head>
							<Table.Row>
								<Table.DataCell head>Information</Table.DataCell>
								<BreakpointRenderer min="lg">
									<Table.DataCell head>On Inventory</Table.DataCell>
									<Table.DataCell head>&nbsp;</Table.DataCell>
								</BreakpointRenderer>
							</Table.Row>
						</Table.Head>
						<Table.Body>
							{beans.length > 0 ? beans.map(renderBeanTableRow) : (
								<Table.Row>
									<Table.DataCell colSpan={3}>
										{progress.loading ? "Retrieving data from server..." : "There are no beans at the moment."}
									</Table.DataCell>
								</Table.Row>
							)}
						</Table.Body>
					</Table>
				</div>
			</div>

			<BeanCreateModal onClose={onCloseModals}
								onCreateBean={createBean}
								progress={progress}
								show={beanCreateModalShown} />
			<BeanUpdateModal bean={selectedBean}
								onClose={onCloseModals}
								onUpdateBean={updateBean}
								progress={progress}
								show={beanUpdateModalShown} />
			<BeanUpdateImageModal bean={selectedBean}
									onClose={onCloseModals}
									onUpdateBean={updateBean}
									progress={progress}
									show={beanUpdateImageModalShown} />
			<BeanRemoveModal bean={selectedBean}
								onClose={onCloseModals}
								onRemoveBean={removeBean}
								progress={progress}
								show={beanRemoveModalShown} />
		</DashboardLayout>
	)
}

export default BeansManagementPage
