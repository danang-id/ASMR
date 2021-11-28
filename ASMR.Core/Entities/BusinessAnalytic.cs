using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ASMR.Core.Enumerations;
using ASMR.Core.Generic;

namespace ASMR.Core.Entities;

public class BusinessAnalytic : DefaultAbstractEntity
{
	[Required] public BusinessAnalyticKey Key { get; set; }

	public BusinessAnalyticReference Reference { get; set; }

	public string ReferenceValue { get; set; }

	public BusinessAnalyticReference AlternateReference { get; set; }

	public string AlternateReferenceValue { get; set; }

	[Column(TypeName = "decimal(20,4)")] public decimal DecimalValue { get; set; }

	public int IntValue { get; set; }

	public string StringValue { get; set; }

	[JsonPropertyName("dateTimeValue")] public DateTimeOffset? DateTimeOffsetValue { get; set; }
}