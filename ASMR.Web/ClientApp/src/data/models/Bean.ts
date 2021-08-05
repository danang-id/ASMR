import DefaultModel from "@asmr/data/generic/DefaultModel"
import BeanInventory from "@asmr/data/models/BeanInventory"
import IncomingGreenBean from "@asmr/data/models/IncomingGreenBean"
import RoastedBeanProduction from "@asmr/data/models/RoastedBeanProduction"
import Product from "@asmr/data/models/Product"

interface Bean extends DefaultModel {
	name: string
	description: string
	image: string
	inventory: BeanInventory
	incomingGreenBeans?: IncomingGreenBean[]
	roastedBeanProductions?: RoastedBeanProduction[]
	products?: Product[]
}

export default Bean
