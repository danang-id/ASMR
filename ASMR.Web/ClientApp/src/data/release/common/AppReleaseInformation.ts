import GenericReleaseInformation from "@asmr/data/release/common/GenericReleaseInformation"
import StoreReleaseInformation from "@asmr/data/release/common/StoreReleaseInformation"

interface AppReleaseInformation extends GenericReleaseInformation {
	DirectDownload: StoreReleaseInformation
}

export default AppReleaseInformation
