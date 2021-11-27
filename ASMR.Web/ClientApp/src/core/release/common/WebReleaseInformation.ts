import GenericReleaseInformation from "asmr/core/release/common/GenericReleaseInformation"
import StoreReleaseInformation from "asmr/core/release/common/StoreReleaseInformation"

interface WebReleaseInformation extends GenericReleaseInformation {
	Status: StoreReleaseInformation
}

export default WebReleaseInformation
