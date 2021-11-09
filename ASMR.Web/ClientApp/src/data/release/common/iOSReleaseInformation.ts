import AppReleaseInformation from "@asmr/data/release/common/AppReleaseInformation"
import StoreReleaseInformation from "@asmr/data/release/common/StoreReleaseInformation"

interface iOSReleaseInformation extends AppReleaseInformation {
	AppStore: StoreReleaseInformation
}

export default iOSReleaseInformation
