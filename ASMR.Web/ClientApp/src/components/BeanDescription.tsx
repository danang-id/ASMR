interface BeanDescriptionProps {
	description?: string
}

function BeanDescription({ description }: BeanDescriptionProps) {
	if (!description) {
		return <></>
	}

	return (
		<>
			{description.split("\n").map((line, index) => (
				<p key={index}>{line}</p>
			))}
		</>
	)
}

export default BeanDescription
