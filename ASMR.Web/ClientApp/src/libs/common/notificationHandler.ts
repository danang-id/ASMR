import cogoToast, { CTOptions, CTReturn } from "cogo-toast"

export interface INotificationOption {
	title?: string
	permanent?: boolean
}

export interface INotificationHandler {
	error(message?: string, options?: INotificationOption): CTReturn
	info(message?: string, options?: INotificationOption): CTReturn
	loading(message?: string, options?: INotificationOption): CTReturn
	success(message?: string, options?: INotificationOption): CTReturn
	warn(message?: string, options?: INotificationOption): CTReturn
}

const defaultCogoToastOptions: CTOptions = {
	hideAfter: 5,
	position: "top-right"
}

const defaultNotificationOptions: INotificationOption = {
	permanent: false
}

class NotificationHandler implements INotificationHandler {
	private readonly options: CTOptions

	constructor(options: CTOptions = defaultCogoToastOptions) {
		this.options = { ...defaultCogoToastOptions, ...options }
	}

	public error(message?: string, { title, permanent }: INotificationOption = defaultNotificationOptions) {
		const notification = cogoToast.error(message, {
			...this.options,
			hideAfter: permanent ? 0 : this.options.hideAfter,
			heading: title,
			onClick: () => { notification.hide && notification.hide() }
		})
		return notification
	}

	public info(message?: string, { title, permanent }: INotificationOption = defaultNotificationOptions) {
		const notification = cogoToast.info(message, {
			...this.options,
			hideAfter: permanent ? 0 : this.options.hideAfter,
			heading: title,
			onClick: () => { notification.hide && notification.hide() }
		})
		return notification
	}

	public loading(message?: string, { title, permanent }: INotificationOption = defaultNotificationOptions) {
		const notification = cogoToast.loading(message, {
			...this.options,
			hideAfter: permanent ? 0 : this.options.hideAfter,
			heading: title,
			onClick: () => { notification.hide && notification.hide() }
		})
		return notification
	}

	public success(message?: string, { title, permanent }: INotificationOption = defaultNotificationOptions) {
		const notification = cogoToast.success(message, {
			...this.options,
			hideAfter: permanent ? 0 : this.options.hideAfter,
			heading: title,
			onClick: () => { notification.hide && notification.hide() }
		})
		return notification
	}

	public warn(message?: string, { title, permanent }: INotificationOption = defaultNotificationOptions) {
		const notification = cogoToast.warn(message, {
			...this.options,
			hideAfter: permanent ? 0 : this.options.hideAfter,
			heading: title,
			onClick: () => { notification.hide && notification.hide() }
		})
		return notification
	}
}

export default NotificationHandler
