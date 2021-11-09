import { useNavigate } from "react-router-dom"
import { IoArrowBack } from "react-icons/io5"

function BackButton(): JSX.Element {
	const navigate = useNavigate()

	function onBackButtonClicked() {
		navigate(-1)
	}

	return (
		<button onClick={onBackButtonClicked}>
			<IoArrowBack />
		</button>
	)
}

export default BackButton
