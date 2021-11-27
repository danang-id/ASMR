import EntityBase from "asmr/core/common/EntityBase"
import RoastingCancellationReason from "asmr/core/enums/RoastingCancellationReason"

interface RoastingSession extends EntityBase {
	beanId: string
	userId: string
	greenBeanWeight: number
	roastedBeanWeight: number
	cancelledAt?: Date
	cancellationReason: RoastingCancellationReason
	finishedAt?: Date
}

export default RoastingSession
