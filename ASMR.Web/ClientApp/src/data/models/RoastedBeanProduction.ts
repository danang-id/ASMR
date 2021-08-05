import DefaultModel from "@asmr/data/generic/DefaultModel"
import User from "@asmr/data/models/User"
import ProductResult from "@asmr/data/models/ProductResult"

interface RoastedBeanProduction extends DefaultModel {
	roaster: User
	greenBeanWeight: number
	isFinalized: boolean
	results: ProductResult[]
}

export default RoastedBeanProduction
