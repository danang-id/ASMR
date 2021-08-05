export type EventMap = Record<string, Event>
export type EventKey<T extends EventMap> = keyof T | string | symbol
export type EventListener<T> = (params: T) => void

interface IEventEmitter<Events extends EventMap, Key = EventKey<Events>> {
	addListener(event: Key, listener: EventListener<Events[Key]>): this
	on(event: Key, listener: EventListener<Events[Key]>): this
	once(event: Key, listener: EventListener<Events[Key]>): this
	removeListener(event: Key, listener: EventListener<Events[Key]>): this
	removeAllListeners(event?: Key): this
	setMaxListeners(n: number): this
	getMaxListeners(): number
	listeners(event: Key): Function[]
	emit(event: Key, params: Events[Key]): boolean
	listenerCount(type: Key): number
	prependListener(event: Key, listener: EventListener<Events[Key]>): this
	prependOnceListener(event: Key, listener: EventListener<Events[Key]>): this
	eventNames(): (Key)[]
}

export default IEventEmitter
