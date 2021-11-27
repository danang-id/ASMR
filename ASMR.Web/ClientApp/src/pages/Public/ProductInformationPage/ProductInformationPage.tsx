import { useEffect, useRef, useState } from "react"
import { useParams } from "react-router-dom"
import { IoPrintOutline } from "react-icons/io5"
import { useReactToPrint } from "react-to-print"
import BeanDescription from "asmr/components/BeanDescription"
import Button from "asmr/components/Button"
import QuickResponseCode from "asmr/components/QuickResponseCode"
import Bean from "asmr/core/entities/Bean"
import Product from "asmr/core/entities/Product"
import BaseLayout from "asmr/layouts/BaseLayout"
import config from "asmr/libs/common/config"
import { toLocalCurrency, toLocaleUnit } from "asmr/libs/common/locale"
import { getBaseUrl } from "asmr/libs/common/location"
import useAuthentication from "asmr/libs/hooks/authenticationHook"
import useDocumentTitle from "asmr/libs/hooks/documentTitleHook"
import useLogger from "asmr/libs/hooks/loggerHook"
import useNotification from "asmr/libs/hooks/notificationHook"
import useProgress from "asmr/libs/hooks/progressHook"
import useServices from "asmr/libs/hooks/servicesHook"
import "asmr/pages/Public/ProductInformationPage/ProductInformationPage.scoped.css"
import BeanImage from "asmr/components/BeanImage"

function ProductInformationPage(): JSX.Element {
	useDocumentTitle("Product Information")
	const { productId } = useParams<"productId">()
	const [bean, setBean] = useState<Bean | null>(null)
	const [initialized, setInitialized] = useState(false)
	const [product, setProduct] = useState<Product | null>(null)
	const [quickResponseCodeValue, setQuickResponseCodeValue] = useState("")
	const authentication = useAuthentication()
	const logger = useLogger(ProductInformationPage)
	const notification = useNotification()
	const [progress] = useProgress()
	const services = useServices()

	const printComponentRef = useRef<HTMLDivElement>(null)
	const handlePrintComponent = useReactToPrint({
		content: () => printComponentRef.current,
	})

	function onPrintButtonClicked() {
		if (handlePrintComponent) {
			handlePrintComponent()
		}
	}

	async function retrieveProductInformation() {
		try {
			const result = await services.product.getById(productId ?? "")
			if (result.isSuccess && result.data) {
				const url = new URL("/pub/product/" + result.data.id, getBaseUrl())
				setProduct(result.data)
				setQuickResponseCodeValue(url.toString())
			}

			services.handleErrors(result.errors, notification, logger)
		} catch (error) {
			services.handleError(error as Error, notification, logger)
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
			services.handleError(error as Error, notification, logger)
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
								<div className="name">Retrieving information...</div>
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
					<div className="top-container">
						{quickResponseCodeValue && (
							<div className="qr-code">
								<div className="qr-code-image">
									<QuickResponseCode value={quickResponseCodeValue} />
								</div>
							</div>
						)}
						<div className="information">
							<div className="product">
								<div className="name">{bean?.name ?? "The product does not exist"}</div>
								<div>
									<span className="weight-per-packaging">
										{toLocaleUnit(
											product?.weightPerPackaging ? product.weightPerPackaging : 0,
											"gram"
										)}
									</span>
									<span> // </span>
									<span className="price">{toLocalCurrency(product?.price ? product.price : 0)}</span>
								</div>
								<div className="description">
									<BeanDescription description={bean?.description} />
								</div>
							</div>
						</div>
					</div>
					<div className="about">
						<hr />
						<div>Product Information</div>
						<div>
							<strong>{config.application.name}</strong>
						</div>
					</div>
				</div>
			</div>
			{product && (
				<div className="product-detail">
					<div className="bean-image-container">
						<BeanImage bean={bean} />
					</div>
					{authentication.isAuthenticated() && (
						<div className="actions">
							<Button onClick={onPrintButtonClicked} icon={IoPrintOutline}>
								Print
							</Button>
						</div>
					)}
				</div>
			)}
		</BaseLayout>
	)
}

export default ProductInformationPage
