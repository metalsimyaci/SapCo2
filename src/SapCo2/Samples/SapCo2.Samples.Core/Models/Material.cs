using SapCo2.Abstraction;
using SapCo2.Abstraction.Attributes;
using SapCo2.Abstraction.Enumerations;

namespace SapCo2.Samples.Core.Models
{
	[RfcEntity("MARA", Description = "Material General Table")]
	public class Material : ISapTable
	{
		[RfcEntityProperty("MATNR", Description = "Material Code", SapDataType = RfcDataTypes.CHAR, Length = 18)]
		public string Code { get; set; }

		[RfcEntityProperty("MATNR", Description = "Material Definition", SapDataType = RfcDataTypes.CHAR, Length = 18, SubTypePropertyName = "Code")]
		public virtual MaterialDefinition Definition { get; set; }

		[RfcEntityProperty("ZZEXTWG", Description = "Material Category Code", SapDataType = RfcDataTypes.CHAR, Length = 25, Unsafe = true)]
		public string MaterialCategoryCode { get; set; }

		[RfcEntityProperty("ZZEXTWG", Description = "Ürün Sınıfı", SapDataType = RfcDataTypes.CHAR, Length = 25, SubTypePropertyName = "ZZEXTWG", Unsafe = true)]
		public virtual MaterialCategoryDefinition MaterialCategory { get; set; }
	}
}
