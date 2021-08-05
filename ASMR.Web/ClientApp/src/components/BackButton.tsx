import { useHistory } from "react-router-dom"
import { IoArrowBack } from "react-icons/io5"

function BackButton(): JSX.Element {
	const history = useHistory()

	function onBackButtonClicked() {
		history.goBack()
	}

	return <button onClick={onBackButtonClicked}><IoArrowBack/></button>
}

export default BackButton
