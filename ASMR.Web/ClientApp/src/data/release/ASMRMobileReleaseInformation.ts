import AndroidReleaseInformation from "@asmr/data/release/common/AndroidReleaseInformation"
import iOSReleaseInformation from "@asmr/data/release/common/iOSReleaseInformation"

interface ASMRMobileReleaseInformation {
	Android: AndroidReleaseInformation
	iOS: iOSReleaseInformation
}

export default ASMRMobileReleaseInformation
