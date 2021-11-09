export function getFileFromCanvas(
	canvas: HTMLCanvasElement,
	name: string,
	type?: string,
	quality?: any
): Promise<File | null> {
	return new Promise((resolve) => {
		canvas.toBlob(
			(blob) => {
				if (!blob) {
					resolve(null)
					return
				}

				const file: any = blob
				file.lastModified = new Date()
				file.name = name
				resolve(file as File)
			},
			type,
			quality
		)
	})
}
