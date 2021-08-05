
import { useEffect, useRef, useState } from "react"
import { useHistory } from "react-router-dom"
import { IoPrintOutline } from "react-icons/io5"
import { useReactToPrint } from "react-to-print"
import Button from "@asmr/components/Button"
import QuickResponseCode from "@asmr/components/QuickResponseCode"
import Bean from "@asmr/data/models/Bean"
import Product  from "@asmr/data/models/Product"
import BaseLayout from "@asmr/layouts/BaseLayout"
import config from "@asmr/libs/common/config"
import { toLocalCurrency, toLocaleUnit } from "@asmr/libs/common/locale"
import { getBaseUrl } from "@asmr/libs/common/location"
import useAuthentication from "@asmr/libs/hooks/authenticationHook"
import useDocumentTitle from "@asmr/libs/hooks/documentTitleHook"
import useInit from "@asmr/libs/hooks/initHook"
import useLogger from "@asmr/libs/hooks/loggerHook"
import useNotification from "@asmr/libs/hooks/notificationHook"
import useProgress from "@asmr/libs/hooks/progressHook"
import useServices from "@asmr/libs/hooks/servicesHook"
import PublicRoutes from "@asmr/pages/Public/PublicRoutes"
import "@asmr/pages/Public/ProductInformationPage/ProductInformationPage.scoped.css"

function ProductInformationPage(): JSX.Element {
	useDocumentTitle("Product Information")
	useInit(onInit)
	const [bean, setBean] = useState<Bean | null>(null)
	const [initialized, setInitialized] = useState(false)
	const [product, setProduct] = useState<Product | null>(null)
	const [productId, setProductId] = useState("")
	const [quickResponseCodeValue, setQuickResponseCodeValue] = useState("")
	const authentication = useAuthentication()
	const history = useHistory()
	const logger = useLogger(ProductInformationPage)
	const notification = useNotification()
	const [progress] = useProgress()
	const services = useServices()

	const printComponentRef = useRef<HTMLDivElement>(null)
	const handlePrintComponent = useReactToPrint({
		content: () => printComponentRef.current
	})

	function onInit() {
		const paths = history.location.pathname.split("/")
		const shouldBeId = paths.pop()
		if (paths.join("/") === PublicRoutes.ProductInformationPage && shouldBeId) {
			setProductId(shouldBeId)
		}
	}

	function onPrintButtonClicked() {
		if (handlePrintComponent) {
			handlePrintComponent()
		}
	}

	async function retrieveProductInformation() {
		try {
			const result = await services.product.getById(productId)
			if (result.isSuccess && result.data) {
				const url = new URL(PublicRoutes.ProductInformationPage + "/" + result.data.id, getBaseUrl())
				setProduct(result.data)
				setQuickResponseCodeValue(url.toString())
			}

			services.handleErrors(result.errors, notification, logger)
		} catch (error) {
			services.handleError(error, notification, logger)
		} finally {
			setInitialized(true)
		}
	}

	async function retrieveBeanInformation() {
		if (!product) {
			return
		}

		try {
			const result = await services.bean.getById(product.beanId)
			if (result.isSuccess && result.data) {
				setBean(result.data)
			}

			services.handleErrors(result.errors, notification, logger)
		} catch (error) {
			services.handleError(error, notification, logger)
		}
	}

	useEffect(() => {
		if (productId) {
			retrieveProductInformation().then()
		}
	}, [productId])

	useEffect(() => {
		if (product) {
			retrieveBeanInformation().then()
		}
	}, [product])

	if (progress.loading || !initialized) {
		return (
			<BaseLayout>
				<div className="container">
					<div className="content">
						<div className="information">
							<div className="product">
								<div className="name">
									Retrieving information...
								</div>
							</div>
						</div>
					</div>
				</div>
			</BaseLayout>
		)
	}

	return (
		<BaseLayout>
			<div className="container" ref={printComponentRef}>
				<div className="content">
					{quickResponseCodeValue && (
						<div className="qr-code">
							<div className="qr-code-image">
								<QuickResponseCode value={quickResponseCodeValue} />
							</div>
						</div>
					)}
					<div className="information">
						<div className="product">
							<div className="name">
								{ (bean && product) ? bean.name : "The product does not exist" }
							</div>
							<div>
								<span className="weight-per-packaging">
									{toLocaleUnit(product?.weightPerPackaging ?? 0, "gram")}
								</span>
								<span> // </span>
								<span className="price">{toLocalCurrency(product?.price ?? 0)}</span>
							</div>
							<div className="description">{bean?.description ?? ""}</div>
						</div>
						<div className="about">
							<hr />
							<div>Product Information</div>
							<div><strong>{config.application.name}</strong></div>
						</div>
					</div>
				</div>
			</div>
			{product && (
				<div className="product-detail">
					{authentication.isAuthenticated() && (
						<div className="actions">
							<Button onClick={onPrintButtonClicked} icon={IoPrintOutline}>Print</Button>
						</div>
					)}
				</div>
			)}
		</BaseLayout>
	)
}

export default ProductInformationPage
