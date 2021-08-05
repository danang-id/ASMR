import * as React from "react";
import { ForwardedRef, forwardRef } from "react"

interface ImageProps {
	source: string
	fallback: string
	alt: string
	type?: string
	className?: string
	style?: React.CSSProperties
}

function Image({ source, alt, type, fallback, ...props }: ImageProps, ref?: ForwardedRef<HTMLImageElement>): JSX.Element {
	return <picture>
		<source srcSet={source} type={type} />
		<img src={fallback} alt={alt} ref={ref} {...props} />
	</picture>
}

export default forwardRef(Image)
