import GenericReleaseInformation from "@asmr/data/release/common/GenericReleaseInformation"
import StoreReleaseInformation from "@asmr/data/release/common/StoreReleaseInformation"

interface WebReleaseInformation extends GenericReleaseInformation {
	Status: StoreReleaseInformation
}

export default WebReleaseInformation
