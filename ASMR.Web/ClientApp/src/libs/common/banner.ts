import config from "@asmr/libs/common/config"
import environment from "@asmr/libs/common/environment"
import { base64ToHex, hexToASCII } from "@asmr/libs/common/encoding"

const figlet = hexToASCII(base64ToHex("Li0tLS0tLS4uLS0tLS0tLi4tLS0tLS0uLi0tLS0tLS4NCnxBLi0tLiB8fFMuLS0uIHx8TS4tLS4gfHxSLi0tLiB8DQp8IChcLykgfHwgOi9cOiB8fCAoXC8pIHx8IDooKTogfA0KfCA6XC86IHx8IDpcLzogfHwgOlwvOiB8fCAoKSgpIHwNCnwgJy0tJ0F8fCAnLS0nU3x8ICctLSdNfHwgJy0tJ1J8DQpgLS0tLS0tJ2AtLS0tLS0nYC0tLS0tLSdgLS0tLS0tJw=="))
const banner = `
${figlet}

${config.application.name} v${config.application.version}
${config.application.description}
© ${new Date().getFullYear()} Pandora Karya Digital. All right reserved.

Found a bug?
Contact: info (at) pandoradigital (dot) id

`

type BannerOptions = {
	showOnProduction: boolean
}
const DefaultBannerOptions: BannerOptions = {
	showOnProduction: true
}

function show(options: BannerOptions = DefaultBannerOptions) {
	if (!options.showOnProduction && environment.isProduction) {
		return
	}

	console.log(banner)
}

export default { show }
