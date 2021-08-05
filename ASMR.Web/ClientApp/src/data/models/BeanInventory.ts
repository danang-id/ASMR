import DefaultModel from "@asmr/data/generic/DefaultModel"

interface BeanInventory extends DefaultModel {
	currentGreenBeanWeight: number
	currentRoastedBeanWeight: number
}

export default BeanInventory
