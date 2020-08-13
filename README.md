[![license](https://img.shields.io/badge/license-MIT-blue?)](https://opensource.org/licenses/MIT)
[![Nuget](https://img.shields.io/nuget/v/sapco2?label=Nuget&logo=Nuget&logoColor=blue)](https://www.nuget.org/packages/SapCo2/)
[![Nuget-download](https://img.shields.io/nuget/dt/sapco2?logo=nuget)](https://www.nuget.org/packages/SapCo2/)
[![Build Core 2.2 And 3.1](https://github.com/metalsimyaci/SapCo2/workflows/Build%20Core%202.2%20And%203.1/badge.svg?branch=master)](https://github.com/metalsimyaci/sapco2/actions?query=workflow%3A%22Build+Core+2.2+And+3.1%22)

# SapCo2 (SAP Conector Core)

Geliştirme aşamasında daha önceden geliştirilmiş olan [SapNwRfc](https://github.com/huysentruitw/SapNwRfc) ve [NwRfcNet](https://github.com/nunomaia/NwRfcNet) projelerinden referans alınmıştır. Projeyi incelemeden önce bu iki projeye göz atmanızı tavsiye ederim. Özellikle Wrapper kısımları iki projenin ortak yanlarının birleştirilmesi ile ortaya çıkmıştır.

Kütüphanemiz SAPNco kütüphanesinin .NET Core Ortamında çalıştırılamaması üzerine **SAP NetWeaver RFC SDK 7.50** paketi üzerine geliştirilmiştir.

Kütüphanemizi .Net Core 3.1, Net Core 2.1 and .Net Frameworklerinde kullanılabilirsiniz.

.Net Stadand 2.0 ve .Net Standard 2.1 ile hazırlanması sebebi ile Windows, Linux ve MacOs işletim sistemlerinde çalıştırılabilir.

Özel kullanım ve örnek anlatım için aşağıya dökümantasyon içinse [Proje Wiki](https://github.com/metalsimyaci/SapCo2/wiki) sayfalarına bakabilirsiniz.

## Gereksinimler

Kütüphanemiz SAP NetWeaver RFC Library 7.50 SDK içerisindeki C++ ile geliştirilmiş .dll dosyalarına ihtiyaç duyar.
İlgili SDK paketinin kurulumu ve daha fazlası için :sparkles: [SAP'nin](https://support.sap.com/en/product/connectors/nwrfcsdk.html) offical sayfasına bakabilirsiniz.

İlgili SKD yı temin ettikten sonra (Sizden bir SAP lisanlı hesabı istiyor);

- zip doyasınızı kendinize göre bir dizine çıkarıp işletim sisteminize göre `PATH` (Windows), `LD_LIBRARY_PATH` (Linux), `DYLD_LIBRARY_PATH` (MacOS)  ortam değişkenine çıkardığınız dosyalar içerisindeki **lib** dizinini gösterin.
- ve ya direkt **lib** klasörünün içeriğini output dizininize kopyalayın.

 Windows işletm sisteminde  SDK paketini kullanabilmeniz için [Visual C++ 2013 yeniden dağıtılabilir paket](https://www.microsoft.com/en-us/download/details.aspx?id=40784)inin 64 bit sürümünü yüklemeniz gerekir.

## Kurulum

PackageManager

```shell
Install-Package SapCo2
```

veya DotNet

```shell
dotnet add package SapCo2
```
veya Paket Referansı
```xml
<PackageReference Include="SapCo2" Version="1.0.0.5" />
```

**Note:** [Microsoft.Extensions.DependencyInjection](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection/) ve [Microsoft.Extensions.Options](https://www.nuget.org/packages/Microsoft.Extensions.Options) bağlılığına sahiptir.

## Kullanım

Kullanım işlemlerindeki tüm detaylar örnek proje içerisinde anlatılmıştır. Kod olarak incelemek isterseniz direkt olarak `src/samples/SapCo2.Samples.NetCore` isimli projeye bakabilirsiniz.

Temel prensip olarak dependency injection üzerine `.AddSapCo2` ile aktif hale getiriyoruz. Ayağa kalmadan önce bağlantı bilgisine ihtiyaç duyuyor.

Bağlantı bilgisini direkt string olarak 

```csharp
var connectionString = "AppServerHost=HOST_NAME; SystemNumber=00; User=USER; Password=PASSWORD; Client=CLIENT_CODE; Language=EN; PoolSize=100; Trace=0;";
```

veya appsettings.json dosyası

```json
{
  "SapServerConnections": {
    "Sap": "AppServerHost=HOST_NAME; SystemNumber=00; User=USER; Password=PASSWORD; Client=CLIENT_CODE; Language=EN; PoolSize=100; Trace=0;"
  }
}
```

içerisinden

```csharp
IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

var connectionString = configuration.GetSection("SapServerConnections:Sap").Value;
```

şeklinde alabilirsiniz.

Dependencyinjection ile IServceProvider içerisinde `.AddSapCo2` fonksiyonuna bir adet bağlantı bilgisi atayarak kullanabilriz.

```csharp
var serviceCollection = new ServiceCollection()
    .AddSapCo2(connectionString);
var serviceProvider = serviceCollection.BuildServiceProvider();
```

Dependency injection ile `IServiceProvider` nesnemisi ayağa kaldırdıktan sonra bağlantı nesnemizi `IRfcConnection` türündne talep ediyoruz ve bağlanıyoruz.

```csharp
using var connection = serviceProvider.GetService<IRfcConnection>();
connection.Connect();
```

***Note:*** burada `connection.Connect();` demezseniz çalışma zamanında `RFC CONNECTION ERROR` alırsınız.

Temelde 3 şekilde SAP den veri işliyorum. **Table**, **Bapi** ve **RFC** olarak.

### **ReadRfc**

Bunların en temeli RFC (Remote Functon Call)'dır. Diğer aşamalarda aslında bunun özelleşmiştrilmiş ve kolaylaştırılmış şeklidir. Sadece RFC y kullanarak tüm işlemleri gerçekleştirebilirsiniz.

Fonksiyon çağrısı öncesi **input** ve **output** nesnelerinizi oluşturup parametre olarak geçtiğiniz taktirde yeterli olacaktır.
Ben RFC için **`CS_BOM_EXPL_MAT_V2_RFC`** reçete fonksiyonunu kullandım. Bununla ilgili
koşul olarak vermek istediğim parametreler için **BomInputParameter** sınıfını geri dönüşte almak istediğim değelerler içinde **BomOutputParameter** sınıfını oluşturuyorum.
Bu sınıfları SAP `SE37` ile ilgili import ve export, Table nesnelerinden kullanmak istediklerim seçerek oluşturuyorum.

Oluşturduğum sınıflarımı `RfcEntityPropertAttribute` veya `RfcEntityIgnorePropertyAttribute` attribute ile işaretleyerek **SAP** tarafındaki karşılığını isim olarak belirtiyoruz. İstersek ne olduğu ile ilgili açıklama da belirtebiliyoruz.

```csharp
public sealed class BomInputParameter
{
    [RfcEntityProperty("AUMGB")]
    public string Aumgb { get; set; }

    [RfcEntityProperty("CAPID")]
    public string Capid { get; set; }

    [RfcEntityProperty("DATUV")]
    public DateTime Datuv { get; set; }

    [RfcEntityProperty("EMENG")]
    public string Emeng { get; set; }

    [RfcEntityProperty("MKTLS")]
    public string Mktls { get; set; }

    [RfcEntityProperty("MEHRS")]
    public string Mehrs { get; set; }

    [RfcEntityProperty("STPST")]
    public string Stpst { get; set; }

    [RfcEntityProperty("SVWVO")]
    public string Svwvo { get; set; }

    [RfcEntityProperty("WERKS")]
    public string Werks { get; set; }

    [RfcEntityProperty("VRSVO")]
    public string Vrsvo { get; set; }

    [RfcEntityProperty("STLAN")]
    public string Stlan { get; set; }

    [RfcEntityProperty("STLAL")]
    public string Stlal { get; set; }

    [RfcEntityProperty("MTNRV")]
    public string Mtnrv { get; set; }
}
```

ve bu nesne ile atadığım koşullar. Burada `plantcode` ile üretim yeri kodunu, `materialCode` ile de reçetesini almak istediğim malzemeyi değişken olarak tanımlıyorum.

```csharp
var inputParameter = new BomInputParameter {
    Aumgb = "0",
    Capid = "PP01",
    Datuv = DateTime.Now,
    Emeng = "1",
    Mktls = "x",
    Mehrs = "x",
    Stpst = "0",
    Svwvo = "x",
    Werks = plantCode,
    Vrsvo = "x",
    Stlal = "1",
    Stlan = "1",
    Mtnrv = materialCode
};
```

Şimdi geri dönüş değeleri için **BomOutputParameter** sınıfımızda 1 tane structor ve bir tane de tabloya referans veriyoruz. Tabloları `[]` array olarak gösteriyoruz.

``` csharp
public sealed class BomOutputParameter
{
    [RfcEntityProperty("STB")]
    public Stb[] StbData { get; set; }

    [RfcEntityProperty("TOPMAT")]
    public Topmat Topmat { get; set; }
}
```

``` csharp
public sealed class Stb
{
    [RfcEntityProperty("STUFE", Description = "Seviye")]
    public int Level { get; set; }

    [RfcEntityProperty("OJTXB", Description = "Nesne kısa metni (bileşen grubu)")]
    public string Name { get; set; }

    [RfcEntityProperty("OJTXP", Description = "Nesne kısa metni (kalem)")]
    public string SubItemName { get; set; }

    [RfcEntityProperty("MMEIN", Description = "Temel ölçü birimi")]
    public string Unit { get; set; }

    [RfcEntityProperty("MNGLG", Description = "Temel ölçü biriminde hesaplanan bileşen miktarı")]
    public decimal UnitAmount { get; set; }

    [RfcEntityProperty("MNGKO", Description = "Bileşen ölçü biriminde hesaplanan bileşen miktarı")]
    public decimal Amount { get; set; }

    [RfcEntityProperty("IDNRK", Description = "Ürün ağacı bileşenleri")]
    public string SubItemCode { get; set; }

    [RfcEntityProperty("MENGE", Description = "Bileşen miktarı")]
    public decimal ComponentAmount { get; set; }

    [RfcEntityProperty("STLTY", Description = "Ürün ağacı tipi")]
    public string TreeType { get; set; }

    [RfcEntityProperty("STLKN", Description = "Ürün ağacı kalemi düğüm numarası")]
    public decimal TreeNodeNumber { get; set; }

    [RfcEntityProperty("STPOZ", Description = "Dahili Sayaç")]
    public decimal InternalCounter { get; set; }

    [RfcEntityProperty("STLNR", Description = "Üst Seviye")]
    public int UpperTreeId { get; set; }

    [RfcEntityProperty("XTLNR", Description = "Alt Seviye")]
    public int SubTreeId { get; set; }

    [RfcEntityProperty("MATMK", Description = "Mal Grubu")]
    public string MaterialGroup { get; set; }
}
```

ve Struct

```csharp
public sealed class Topmat
{
    [RfcEntityProperty("MATNR", Description = "Malzeme Tanımı")]
    public string Code { get; set; }

    [RfcEntityProperty("MAKTX", Description = "Tanım")]
    public string Definition { get; set; }
}
```

tanımlamalar sonrasında bir tane `IReadRfc` nesnesi üretim `GetRfc` fonksiyonunu çağırıyoruz.

```csharp
 using IReadRfc rfcFunction = _serviceProvider.GetService<IReadRfc>();
BomOutputParameter bomResult = rfcFunction.GetRfc<BomOutputParameter, BomInputParameter>(connection, "CS_BOM_EXPL_MAT_V2_RFC", inputParameter);
```

`bomResult` içerisinde `BomOutputParameter` türünden istediğimiz çıktıyı görebiliriz.

Fonsiyonun tamamı aşağıdaki şekilde oluyor.

```csharp
public BomOutputParameter GetBillOfMaterial(string materialCode, string plantCode)
{
    using IRfcConnection connection = _serviceProvider.GetService<IRfcConnection>();
    connection.Connect();

    var inputParameter = new BomInputParameter
    {
        Aumgb = "0",
        Capid = "PP01",
        Datuv = DateTime.Now,
        Emeng = "1",
        Mktls = "x",
        Mehrs = "x",
        Stpst = "0",
        Svwvo = "x",
        Werks = plantCode,
        Vrsvo = "x",
        Stlal = "1",
        Stlan = "1",
        Mtnrv = materialCode
    };
    using IReadRfc rfcFunction = _serviceProvider.GetService<IReadRfc>();
    BomOutputParameter bomResult = rfcFunction.GetRfc<BomOutputParameter, BomInputParameter>(connection, "CS_BOM_EXPL_MAT_V2_RFC", inputParameter);
    return bomResult;
}

```

### **ReadBapi**

**BAPI** temel olarak sistem tarafında oluşturulmuş ve güncelleme, silme, oluşturma gibi işlemlerde kullanılan ve her işlem sonucu için `RETURN` isimli bir sonucu içeriğinde barındıran özel bir RFC türüdür.

Bapi çağırılarınızda işinizi kolaylaştırmak için çıktı nesnelerinizi `IBapiOutput` ara yüzünden veya `BapiOutputBase` abstrak sınıfından türetmeniz gerekmektedir. Bu işlem size çıktılarınız içinde otomatik `RETURN` nesnesini ekler. İşlem sonrasında bu nesnenin dönen değerlerinden MessageType değerinin **Abort** veya **Error** olması 
durumunda hata fırlatılır.Geri kalan işlemler RFC çağırısı ile aynıdır.

```csharp
public sealed class RfcBapiOutputParameter
{
    [RfcEntityProperty("CODE")]
    public string Code { get; set; }

    [RfcEntityProperty("TYPE")]
    public string MessageType { get; set; }

    [RfcEntityProperty("MESSAGE")]
    public string Message { get; set; }

    [RfcEntityProperty("LOG_NO")]
    public string LogNo { get; set; }

    [RfcEntityProperty("LOG_MSG_NO")]
    public string LogMessageNumber { get; set; }

    [RfcEntityProperty("MESSAGE_V1")]
    public string MessageV1 { get; set; }

    [RfcEntityProperty("MESSAGE_V2")]
    public string MessageV2 { get; set; }

    [RfcEntityProperty("MESSAGE_V3")]
    public string MessageV3 { get; set; }

    [RfcEntityProperty("MESSAGE_V4")]
    public string MessageV4 { get; set; }
}
```

 Ben örnek için `BBP_VENDOR_GETLIST` Bapisini çağırıyorum. Bu bapi içerisine aldığı Şirket koduna göre sistemdeki satıcıların kodu ve adı bilgisini dönüyor.

 input ve output nesneleri oluşturalım.

```csharp
public class VendorBapiInputParameter
{
    [RfcEntityProperty("COMP_CODE")]
    public string CompanyCode { get; set; }
}
```

```csharp
var inputParameter = new VendorBapiInputParameter
{
    CompanyCode = companyCode
};
```

Burada `companyCode` değerini üst birimden `200` olarak atıyorum.


```csharp
public class VendorBapiOutputParameter:IRfcBapiOutput
{
    [RfcEntityProperty("RETURN")]
    public RfcBapiOutputParameter BapiReturn { get; set; }

    [RfcEntityProperty("VENDOR")]
    public VendorModel[] Vendors { get; set; }
}
```

`IRfcBapiOutput` arayüzünden türediğine ve `[RfcEntityProperty("RETURN")]` attribute'üne sahip olduğuna dikkat edin. Bapiyi bapi yapan arkadaş budur.

```csharp
public class VendorModel
{
    [RfcEntityProperty("VENDOR_NO")]
    public string VendorNo { get; set; }

    [RfcEntityProperty("NAME")]
    public string Name { get; set; }
}
```

Sonrası RFC işlemi ile anı şekilde gerçekleşir. Bir tane bağlantı nesnesi oluşturulur ve bağlantı açılır.`IReadBapi` nesnes üzerilir ve `GetBapi` fonksiyonu çağrılarak çalıştırılır.

```csharp
public VendorBapiOutputParameter GetVerdorsByCompanyCode(string companyCode)
{
    using IRfcConnection connection = _serviceProvider.GetService<IRfcConnection>();
    connection.Connect();

    var inputParameter = new VendorBapiInputParameter
    {
        CompanyCode = companyCode
    };
    using IReadBapi<VendorBapiOutputParameter> rfcFunction = _serviceProvider.GetService<IReadBapi<VendorBapiOutputParameter>>();
    VendorBapiOutputParameter result = rfcFunction.GetBapi(connection, "BBP_VENDOR_GETLIST", inputParameter);
    return result;
}
```

### **ReadTable**

Table nesnelerine direk erişmimiz olmadığı için sistem tarafından bizlere sunulan
 `RFC_READ_TABLE` fonksiyonunu kullanarak tablodan çekim işlemini gerçekleştiririz.

 `RFC_READ_TABLE` ın standart belli kuralları vardır. O alanlara ilgili verileri atayarak tablodan veri çekim işlemini gerçekleştiririz. Buradaki tüm bu kütfetten siz kurtarıp daha basit bir kullanım için `IReadTable` ara yüzünden table fonksiyonumuzu üretiyoruz.

 Buradaki temel RFC işleminden sonra RFC_READ_TABLE tarafından çıktı olarak sunulan `DATA` tablosundaki `WA` struct'ı sizin belirlediğiniz parçalama karakterine göre parçalanarak ilgili sınıf propertylerine atanır.

 Bu işlem sırasındaki dönüşüm işlemleri RFC Map işleminden farklı olarak işlendiği için table olarak kullanacağınız nesneyi `RrfcTable` propertyleri ise `RfcTableProperty` attribüteleri ile işaretlemeniz gerekmektedir. `RrfcTable` name alanı ile tabloyu işaret eder. Yine bilgi olarak bulunması için `unsafe` özelliği taşır. Bu genelde RFCnin SAP ın standartlarında olmayıp özel olarak üzerildiğini belirtir. Bizim sistemler için '**Z**' li diye ifade edilen geliştirmeleri tanımlar. Aynı şekilde propertylerde dönüşüm için `TablePropertySapType` ve `Length` özelliklerini taşır. Bunun yanında tablo ile aynı şekilde bu özelliğin bir geliştirme olup olmadığını belirtmek için **unsafe** kullanılır.`SubTypePropertyName` ise ilk etapta çekilmeyecek ama bağlantılı tabloları gösterir. şuan için `SetOption` methodunu kendimiz yazarak bunları dolduruyoruz. Bu alanları virtual olarak tanımlıyoruz ki normal datamızda dikkate alınmasın.

 input parameter yerine `RFC_READ_TABLE` ın `OPTIONS` tablosunun `TEXT` özelliğine metin olarak koşul ifadelerinin `ABAP` ile yazılır. Bunu ister düz metin şeklinde `MATNR EQ '12344555'` şeklinde metin listesi oluşturarak, istersenizde Linq ya benzeterek zincirleyebildiğimiz `Abap Query` ile oluşturabilirsiniz. `Abap Quey` ile ilgili detayları [Proje Wiki](https://github.com/metalsimyaci/SapCo2/wiki) kısmına ekleyeceğim. `FIELDS` alanları otomatik olarak oluşturdunuz çıktı sınıfındaki attrütelere göre atanmaktadır. `ROWCOUNT`,`ROWSKIPS`, `ROWDATA` gibi özellikler **opsiyonel** olup gerektiğinde ezilebilmektedir.

 Fazla uzatmadan örneğe bakalım.
 Örnek için `MARA` tablosunu ve alt detaylarını (Malzeme tanımı `MAKT` ve Malzeme sınıfı `ZMM24030`) tablolarından çekeceğim. 
 
 NOT: Alt deyatlar kısmındaki Malzeme sınıfları tablosu bir geliştirme tablosu olduğu için SAP sisteminizde yer almıyorsa lütfen ` new MaterialQueryOptions { IncludeAll = true }` ifadesini `new MaterialQueryOptions { IncludeDefinition = true }` ifadesi ile değiştirin. Bu sayede sadece `MAKT` tablosunu çekersiniz. 

 ```csharp
  [RfcTable("MARA", Description = "Material General Table")]
  public class Material
  {
      [RfcTableProperty("MATNR", Description = "Material Code", TablePropertySapType = RfcTablePropertySapTypes.CHAR, Length = 18)]
      public string Code { get; set; }

      [RfcTableProperty("MATNR", Description = "Material Definition", TablePropertySapType = RfcTablePropertySapTypes.CHAR, Length = 18, SubTypePropertyName = "Code")]
      public virtual MaterialDefinition Definition { get; set; }

      [RfcTableProperty("ZZEXTWG", Description = "Material Category Code", TablePropertySapType = RfcTablePropertySapTypes.CHAR, Length = 25, Unsafe = true)]
      public string MaterialCategoryCode { get; set; }

      [RfcTableProperty("ZZEXTWG", Description = "Ürün Sınıfı", TablePropertySapType = RfcTablePropertySapTypes.CHAR, Length = 25, SubTypePropertyName = "Code", Unsafe = true)]
      public virtual MaterialCategoryDefinition MaterialCategory { get; set; }
  }
 ```

 alt tablolar

 ```csharp
[RfcTable("MAKT", Description = "Material Definition Table")]
public class MaterialDefinition
{
    [RfcTableProperty("MATNR", Description = "Material Code", TablePropertySapType = RfcTablePropertySapTypes.CHAR, Length = 18)]
    public string Code { get; set; }

    [RfcTableProperty("MAKTX", Description = "Material Short Definition", TablePropertySapType = RfcTablePropertySapTypes.CHAR, Length = 40)]
    public string Definition { get; set; }
}
 ```

 ```csharp
 [RfcTable("ZMM24030", Description = "Material Category Definition Table", Unsafe = true)]
public class MaterialCategoryDefinition
{

    #region Properties

    [RfcTableProperty("ZZEXTWG", Description = "Material Category Code", TablePropertySapType = RfcTablePropertySapTypes.CHAR, Length = 25)]
    public string Code { get; set; }

    [RfcTableProperty("TANIM", Description = "Material Category Definition", TablePropertySapType = RfcTablePropertySapTypes.CHAR, Length = 50)]
    public string Definition { get; set; }

    [RfcTableProperty("ZZPARCASAYI", Description = "Material Category Unit Part Count", TablePropertySapType = RfcTablePropertySapTypes.INTEGER, Length = 10)]
    public int PartCount { get; set; }

    #endregion

    #region Methods

    public override string ToString()
    {
        return Definition;
    }

    #endregion

}
 ```

 Sornasında tablomuzu çekeceğimiz koşullarımızı `AbapQuey` ile oluşturuyoruz. Koşulu `MATNR` malzeme tanımının `materialCodePrefix` ile başlayan ve silinmiş olarak işaretlenmeyen kalemler olsun olarak belirliyoruz.

 ```csharp
 var query = new AbapQuery().Set(QueryOperator.StartsWith("MATNR", materialCodePrefix))
                .And(QueryOperator.NotEqual("LVORM", true, RfcTablePropertySapTypes.BOOLEAN_X)).GetQuery();
 ```
RFC de olduğu gibi bağlantı nesnesini türetip bağlanıyoruz.

```csharp
using IRfcConnection connection = _serviceProvider.GetService<IRfcConnection>();
          connection.Connect();
```

 Sonra `IReadTable` ara yüzünden çıktı olarak kullanacağımız veri modeline göre üretilmiş bir generic sınıf alıyoruz.Aldığımız sınıfnın `GetTable` methodu ile çekmek istediğmiz koşul, kayıt sayısı ve güvenli olayan (Geliştirme ile gelen) alanları isteyip istemediğimizi belirtiyoruz.

 ```csharp
using IReadTable<Material> tableFunction = _serviceProvider.GetService<IReadTable<Material>>();

List<Material> result = tableFunction.GetTable(connection, query, rowCount: rowCount, getUnsafeFields: getUnsafeFields);

 ```

 Kodun tamamı şu şekilde olacaktır.

 ```csharp
 public async Task<List<Material>> GetMaterialsByPrefixAsync(string materialCodePrefix,
            MaterialQueryOptions options = null, bool getUnsafeFields = true, int rowCount = 0)
        {
            options ??= new MaterialQueryOptions();
            var query = new AbapQuery().Set(QueryOperator.StartsWith("MATNR", materialCodePrefix))
                .And(QueryOperator.NotEqual("LVORM", true, RfcTablePropertySapTypes.BOOLEAN_X)).GetQuery();

            using IRfcConnection connection = _serviceProvider.GetService<IRfcConnection>();
            connection.Connect();

            using IReadTable<Material> tableFunction = _serviceProvider.GetService<IReadTable<Material>>();
            List<Material> result = tableFunction.GetTable(connection, query, rowCount: rowCount, getUnsafeFields: getUnsafeFields);

            return await SetOptionsAsync(result, options, getUnsafeFields).ConfigureAwait(false);
        }
 ```


Tablo okuma olayımınız kullanımı temelinde bu kadar. alt tablolarıda getirebilmek için
`setoptions` isimli bir fonksiyon oluşturup onunla aldığımız alt tabloları ana `MARA` tablomuza bapliyoruz. Bu işlem daha çok `unsafe`, `SubProperty` gibi alanların kullanımına örnek olması için oluşturuldu.

```csharp
private async Task<List<Material>> SetOptionsAsync(List<Material> materialList,
           MaterialQueryOptions queryOptions, bool getUnsafeFields = true)
{
    if (!materialList.Any())
        return materialList;

    var taskList = new List<Task>();
    var definitionList = new ConcurrentQueue<MaterialDefinition>();
    var materialCategoryDefinitionList = new ConcurrentQueue<MaterialCategoryDefinition>();

    List<Task> materialDefinitionTaskList = SetMaterialDefinitionOptionAsync(queryOptions, materialList, definitionList);
    if (materialDefinitionTaskList.Any())
        taskList.AddRange(materialDefinitionTaskList);

    List<Task> materialCategoryTaskList =
        SetMaterialCategoryOptionAsync(queryOptions, materialList, materialCategoryDefinitionList);
    if (materialCategoryTaskList.Any())
        taskList.AddRange(materialCategoryTaskList);

    if (!taskList.Any())
        return materialList;

    await Task.WhenAll(taskList).ConfigureAwait(false);

    ILookup<string, MaterialDefinition> materialDefinitionLookup = definitionList.ToLookup(x => x.Code);
    ILookup<string, MaterialCategoryDefinition> materialCategoryLookup =
        materialCategoryDefinitionList.ToLookup(x => x.Code);

    foreach (Material material in materialList)
    {
        material.Definition = materialDefinitionLookup.Contains(material.Code)
            ? materialDefinitionLookup[material.Code].FirstOrDefault()
            : null;
        material.MaterialCategory = materialCategoryLookup.Contains(material.MaterialCategoryCode)
            ? materialCategoryLookup[material.MaterialCategoryCode].FirstOrDefault()
            : null;
    }

    return materialList;
}
```

```csharp
private List<Task> SetMaterialDefinitionOptionAsync(MaterialQueryOptions queryOptions, List<Material> materialList,
            ConcurrentQueue<MaterialDefinition> definitionList)
{
    var taskList = new List<Task>();
    if (queryOptions.IncludeDefinition)
    {
        var list = materialList.Where(x => x.Code != null).Select(x => (object)x.Code).ToList();
        List<object>[] parts = list.Partition(PartitionCount);

        foreach (List<object> part in parts)
        {
            List<string> query = new AbapQuery()
                .Set(QueryOperator.In("MATNR", part))
                .GetQuery();

            taskList.Add(Task.Run(() =>
            {
                using var connection = _serviceProvider.GetService<IRfcConnection>();
                connection.Connect();
                using var tableFunction = _serviceProvider.GetService<IReadTable<MaterialDefinition>>();
                var definitions = tableFunction.GetTable(connection, query);
                foreach (MaterialDefinition definition in definitions)
                    definitionList.Enqueue(definition);
            }));
        }
    }

    return taskList;
}
```

```csharp
private List<Task> SetMaterialCategoryOptionAsync(MaterialQueryOptions queryOptions, List<Material> materialList,
            ConcurrentQueue<MaterialCategoryDefinition> materialCategoryDefinitionList)
{
    var taskList = new List<Task>();
    if (queryOptions.IncludeMaterialCategory)
    {
        var materialCategoryList =
            materialList.Where(x => x.MaterialCategoryCode != null)
                .Select(x => (object)x.MaterialCategoryCode)
                .Distinct()
                .ToList();

        List<object>[] materialCategoryParts = materialCategoryList.Partition(PartitionCount);

        foreach (List<object> part in materialCategoryParts)
        {
            List<string> queryList = new AbapQuery()
                .Set(QueryOperator.In("ZZEXTWG", part))
                .GetQuery();

            taskList.Add(Task.Run(async () =>
            {
                using var connection = _serviceProvider.GetService<IRfcConnection>();
                connection.Connect();
                using var tableFunction = _serviceProvider.GetService<IReadTable<MaterialCategoryDefinition>>();
                var definitions = tableFunction.GetTable(connection, queryList);
                foreach (MaterialCategoryDefinition definition in definitions)
                    materialCategoryDefinitionList.Enqueue(definition);
            }));
        }
    }

    return taskList;
}
```


Hepsi bu kadar. Paket henüz yeni ve bazı testleri eksik. En kısa sürede o testleri tamamlamaya çalışacağım. Genel kullanım için çok büyük bir sıkıntısi görünmüyor. Dökümantasyon olarakta proje Wik yi daha detaylı bir şekilde bölüm bölüm açıklamaya çalışacağım.