import AppReleaseInformation from "@asmr/data/release/common/AppReleaseInformation"
import StoreReleaseInformation from "@asmr/data/release/common/StoreReleaseInformation"

interface AndroidReleaseInformation extends AppReleaseInformation {
	PlayStore: StoreReleaseInformation
}

export default AndroidReleaseInformation
