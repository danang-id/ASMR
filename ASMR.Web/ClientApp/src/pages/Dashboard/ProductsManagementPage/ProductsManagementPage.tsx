import { useEffect, useState } from "react"
import { useLocation } from "react-router-dom"
import { IoAddCircleOutline, IoCreateOutline, IoInformation, IoQrCodeOutline, IoTrashOutline } from "react-icons/io5"
import BackButton from "@asmr/components/BackButton"
import BreakpointRenderer from "@asmr/components/BreakpointRenderer"
import Button from "@asmr/components/Button"
import ListSelector, { ListOption } from "@asmr/components/ListSelector"
import Table from "@asmr/components/Table"
import Bean from "@asmr/data/models/Bean"
import Product from "@asmr/data/models/Product"
import CreateProductRequestModel from "@asmr/data/request/CreateProductRequestModel"
import UpdateProductRequestModel from "@asmr/data/request/UpdateProductRequestModel"
import DashboardLayout from "@asmr/layouts/DashboardLayout"
import lazy from "@asmr/libs/common/lazy"
import { toLocalCurrency, toLocaleUnit } from "@asmr/libs/common/locale"
import { singleSwitchToggle } from "@asmr/libs/common/toggle"
import useBreakpoint from "@asmr/libs/hooks/breakpointHook"
import useDocumentTitle from "@asmr/libs/hooks/documentTitleHook"
import useInit from "@asmr/libs/hooks/initHook"
import useLogger from "@asmr/libs/hooks/loggerHook"
import useNotification from "@asmr/libs/hooks/notificationHook"
import useProgress from "@asmr/libs/hooks/progressHook"
import useServices from "@asmr/libs/hooks/servicesHook"
import "@asmr/pages/Dashboard/ProductsManagementPage/ProductsManagementPage.scoped.css"

const ProductCreateModal = lazy(() => import("@asmr/pages/Dashboard/ProductsManagementPage/ProductCreateModal"))
const ProductUpdateModal = lazy(() => import("@asmr/pages/Dashboard/ProductsManagementPage/ProductUpdateModal"))
const ProductRemoveModal = lazy(() => import("@asmr/pages/Dashboard/ProductsManagementPage/ProductRemoveModal"))

function ProductsManagementPage(): JSX.Element {
	useDocumentTitle("Manage Products")
	useInit(onInit)
	const [beans, setBeans] = useState<Bean[]>([])
	const [bean, setBean] = useState<Bean | null>(null)
	const [products, setProducts] = useState<Product[]>([])
	const [selectedProduct, setSelectedProduct] = useState<Product | null>(null)
	const [productCreateModalShown, setProductCreateModalShown] = useState(false)
	const [productUpdateModalShown, setProductUpdateModalShown] = useState(false)
	const [productRemoveModalShown, setProductRemoveModalShown] = useState(false)
	const breakpoint = useBreakpoint()
	const location = useLocation()
	const logger = useLogger(ProductsManagementPage)
	const notification = useNotification()
	const [progress] = useProgress()
	const services = useServices()

	async function onInit() {
		const urlSearchParam = new URLSearchParams(location.search)
		const beanId = urlSearchParam.get("beanId")
		await retrieveAllBeans(beanId)
	}

	async function onCloseModals() {
		await singleSwitchToggle(undefined, [
			setProductCreateModalShown,
			setProductUpdateModalShown,
			setProductRemoveModalShown,
		])
		setSelectedProduct(null)
	}

	function onShowProductInformationPageButtonClicked(product: Product) {
		window.open(`/pub/product/${product.id}`, "_blank")
	}

	function onShowBeanInformationPageButtonClicked() {
		if (!bean) {
			return
		}

		window.open(`/pub/bean/${bean.id}`, "_blank")
	}

	async function onShowProductCreateModalButtonClicked() {
		await singleSwitchToggle(setProductCreateModalShown, [setProductUpdateModalShown, setProductRemoveModalShown])
	}

	async function onShowProductUpdateModalButtonClicked(Product: Product) {
		setSelectedProduct(Product)
		await singleSwitchToggle(setProductUpdateModalShown, [setProductCreateModalShown, setProductRemoveModalShown])
	}

	async function onShowProductRemoveModalButtonClicked(Product: Product) {
		setSelectedProduct(Product)
		await singleSwitchToggle(setProductRemoveModalShown, [setProductCreateModalShown, setProductUpdateModalShown])
	}

	async function createProduct(requestModel: CreateProductRequestModel) {
		if (!bean || !requestModel) {
			return
		}

		try {
			const result = await services.product.create(requestModel)
			if (result.isSuccess) {
				notification.success(result.message)
				await Promise.all([retrieveProducts(), onCloseModals()])
			}

			services.handleErrors(result.errors, notification, logger)
		} catch (error) {
			services.handleError(error as Error, notification, logger)
		}
	}

	async function updateProduct(requestModel: UpdateProductRequestModel) {
		if (!bean || !selectedProduct || !requestModel) {
			return
		}

		try {
			const result = await services.product.modify(selectedProduct.id, requestModel)
			if (result.isSuccess) {
				notification.success(result.message)
				await Promise.all([retrieveProducts(), onCloseModals()])
			}

			services.handleErrors(result.errors, notification, logger)
		} catch (error) {
			services.handleError(error as Error, notification, logger)
		}
	}

	async function removeProduct() {
		if (!bean || !selectedProduct) {
			return
		}

		try {
			const result = await services.product.remove(selectedProduct.id)
			if (result.isSuccess) {
				notification.success(result.message)
				await Promise.all([retrieveProducts(), onCloseModals()])
			}

			services.handleErrors(result.errors, notification, logger)
		} catch (error) {
			services.handleError(error as Error, notification, logger)
		}
	}

	async function retrieveAllBeans(autoSelectBeanId?: string | null) {
		try {
			const result = await services.bean.getAll()
			if (result.isSuccess && result.data) {
				setBeans(result.data)
				if (autoSelectBeanId) {
					for (const bean of result.data) {
						if (bean.id === autoSelectBeanId) {
							setBean(bean)
						}
					}
				}
			}

			services.handleErrors(result.errors, notification, logger)
		} catch (error) {
			services.handleError(error as Error, notification, logger)
		}
	}

	async function retrieveProducts() {
		if (!bean) {
			setProducts([])
			return
		}

		try {
			const result = await services.product.getByBeanId(bean.id)
			if (result.isSuccess && result.data) {
				setProducts(result.data)
			}

			services.handleErrors(result.errors, notification, logger)
		} catch (error) {
			services.handleError(error as Error, notification, logger)
		}
	}

	useEffect(() => {
		retrieveProducts().then()
	}, [bean])

	function beanValueRenderer(bean?: Bean | null): string {
		return bean ? bean.name : "Please select a bean..."
	}

	function renderProductTableRow(product: Product, index: number): JSX.Element {
		function ManagementActions() {
			return (
				<>
					<Button
						disabled={progress.loading}
						key={0}
						icon={IoQrCodeOutline}
						size="xs"
						style="filled"
						onClick={() => onShowProductInformationPageButtonClicked(product)}
					>
						Show QR Code
					</Button>
					<Button
						disabled={progress.loading}
						key={1}
						icon={IoCreateOutline}
						size="xs"
						style="outline"
						onClick={() => onShowProductUpdateModalButtonClicked(product)}
					>
						Modify
					</Button>
					<Button
						disabled={progress.loading}
						key={2}
						icon={IoTrashOutline}
						size="xs"
						style="danger"
						onClick={() => onShowProductRemoveModalButtonClicked(product)}
					>
						Remove
					</Button>
				</>
			)
		}

		return (
			<Table.Row key={index}>
				<Table.DataCell>
					<div className="information">
						<div className="weight-per-packaging">{toLocaleUnit(product.weightPerPackaging, "gram")}</div>
						<BreakpointRenderer max="xs">
							<div className="price">{toLocalCurrency(product.price)}</div>
							<div className="product-inventory">In Inventory: {product.currentInventoryQuantity}</div>
							<div className="management-actions">
								<ManagementActions />
							</div>
						</BreakpointRenderer>
					</div>
				</Table.DataCell>
				<BreakpointRenderer min="sm">
					<Table.DataCell>
						<div className="price">{toLocalCurrency(product.price)}</div>
					</Table.DataCell>
					<Table.DataCell>
						<div className="product-inventory">In Inventory: {product.currentInventoryQuantity}</div>
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
				<BackButton />
				&nbsp;&nbsp; Products Management
			</div>

			<div className="click-to-actions">
				{bean && (
					<Button
						disabled={progress.loading}
						size={breakpoint.includes("sm") ? "sm" : "md"}
						style="filled"
						onClick={onShowProductCreateModalButtonClicked}
					>
						<IoAddCircleOutline />
						<BreakpointRenderer min="sm">&nbsp;Add Product</BreakpointRenderer>
					</Button>
				)}
				{bean && (
					<Button
						disabled={progress.loading}
						size={breakpoint.includes("sm") ? "sm" : "md"}
						style="outline"
						onClick={onShowBeanInformationPageButtonClicked}
					>
						<IoInformation />
						<BreakpointRenderer min="sm">&nbsp;Bean Information</BreakpointRenderer>
					</Button>
				)}
				<div className="bean-selector">
					<ListSelector
						emptyOption="No beans available"
						onChange={setBean}
						value={bean}
						valueRenderer={beanValueRenderer}
					>
						{beans.length > 0 &&
							beans.map((value, index) => (
								<ListOption key={index} index={index} value={value} valueRenderer={beanValueRenderer} />
							))}
					</ListSelector>
				</div>
			</div>

			<div className="content">
				<div className="table-container">
					<Table>
						<Table.Head>
							<Table.Row>
								<Table.DataCell head>
									{breakpoint.includes("sm") ? "Weight per Packaging" : "Product Package"}
								</Table.DataCell>
								<BreakpointRenderer min="sm">
									<Table.DataCell head>Price</Table.DataCell>
									<Table.DataCell head>Quantity</Table.DataCell>
									<Table.DataCell head>&nbsp;</Table.DataCell>
								</BreakpointRenderer>
							</Table.Row>
						</Table.Head>
						<Table.Body>
							{products.length > 0 ? (
								products.map(renderProductTableRow)
							) : (
								<Table.Row>
									<Table.DataCell colSpan={breakpoint.includes("sm") ? 4 : 1}>
										<span className="table-data-information">
											{progress.loading
												? "Retrieving data from server..."
												: beans.length <= 0
												? "There are no beans at the moment. You may add bean from Bean Management Panel."
												: bean
												? `There are no products for ${bean.name} bean at the moment.`
												: "Please select a bean to show products."}
										</span>
									</Table.DataCell>
								</Table.Row>
							)}
						</Table.Body>
					</Table>
				</div>
			</div>

			<ProductCreateModal
				bean={bean}
				onClose={onCloseModals}
				onCreateProduct={createProduct}
				progress={progress}
				show={productCreateModalShown}
			/>
			<ProductUpdateModal
				bean={bean}
				product={selectedProduct}
				onClose={onCloseModals}
				onUpdateProduct={updateProduct}
				progress={progress}
				show={productUpdateModalShown}
			/>
			<ProductRemoveModal
				bean={bean}
				product={selectedProduct}
				onClose={onCloseModals}
				onRemoveProduct={removeProduct}
				progress={progress}
				show={productRemoveModalShown}
			/>
		</DashboardLayout>
	)
}

export default ProductsManagementPage
