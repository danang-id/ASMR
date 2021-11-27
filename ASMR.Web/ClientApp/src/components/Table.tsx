import { CSSProperties, ReactNode } from "react"
import { combineClassNames } from "asmr/libs/common/styles"
import "asmr/components/styles/Table.css"

export interface TableProps {
	children?: ReactNode
	className?: string
	styles?: CSSProperties
}

function Table({ children, className, ...props }: TableProps): JSX.Element {
	className = combineClassNames("table", className)
	return (
		<table className={className} {...props}>
			{children}
		</table>
	)
}

export interface TableHeadProps {
	children?: ReactNode
	className?: string
	styles?: CSSProperties
}

Table.Head = function ({ children, className, ...props }: TableHeadProps): JSX.Element {
	className = combineClassNames("table-head", className)
	return (
		<thead className={className} {...props}>
			{children}
		</thead>
	)
}

export interface TableBodyProps {
	children?: ReactNode
	className?: string
	styles?: CSSProperties
}

Table.Body = function ({ children, className, ...props }: TableBodyProps): JSX.Element {
	className = combineClassNames("table-body", className)
	return (
		<tbody className={className} {...props}>
			{children}
		</tbody>
	)
}

export interface TableRowProps {
	children?: ReactNode
	className?: string
	styles?: CSSProperties
}

Table.Row = function ({ children, className, ...props }: TableRowProps): JSX.Element {
	className = combineClassNames("table-row", className)
	return (
		<tr className={className} {...props}>
			{children}
		</tr>
	)
}

export interface TableDataCellProps {
	children?: ReactNode
	className?: string
	colSpan?: number
	head?: boolean
	styles?: CSSProperties
}

Table.DataCell = function ({ children, className, head = false, ...props }: TableDataCellProps): JSX.Element {
	let addedClassName = combineClassNames("table-core-cell")
	if (head) {
		addedClassName = combineClassNames(addedClassName, "table-core-cell-head")
	}
	className = combineClassNames(addedClassName, className)

	return head ? (
		<th className={className} {...props}>
			{children}
		</th>
	) : (
		<td className={className} {...props}>
			{children}
		</td>
	)
}

export default Table
