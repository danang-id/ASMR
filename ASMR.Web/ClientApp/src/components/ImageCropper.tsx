import { useEffect, useRef } from "react"
import Cropper from "cropperjs"
import "cropperjs/dist/cropper.min.css"
import "@asmr/components/styles/ImageCropper.css"

interface ImageCropperProps {
	alt: string
	aspectRatio?: number
	source: string | ArrayBuffer | null
	onCropped?: (cropper: Cropper) => void
}

function ImageCropper({ alt, aspectRatio = 1, source, onCropped }: ImageCropperProps): JSX.Element {
	let cropper: Cropper
	const imageRef = useRef<HTMLImageElement>(null)

	useEffect(() => {
		if (!imageRef || !imageRef.current) {
			return
		}

		cropper = new Cropper(imageRef.current, {
			aspectRatio,
			scalable: false,
			zoomable: false,
			ready: () => {
				if (onCropped) {
					onCropped(cropper)
				}
			},
			cropend: () => {
				if (onCropped) {
					onCropped(cropper)
				}
			},
		})
	}, [])

	return <img className="image-cropper" src={source as string} alt={alt} ref={imageRef} />
}

export default ImageCropper
