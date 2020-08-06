# SapCo2 (SAP Conector Core)

Geliştirme aşamasında daha önceden geliştirilmiş olan [SapNwRfc](https://github.com/huysentruitw/SapNwRfc) ve [NwRfcNet](https://github.com/nunomaia/NwRfcNet) projelerinden referans alınmıştır. Projeyi incelemeden önce bu iki projeye göz atmanızı tavsiye ederim. Özellikle Wrapper kısımları iki projenin ortak yanlarının birleştirilmesi ile ortaya çıkmıştır.


Kütüphanemiz SAPNco kütüphanesinin .NET Core Ortamında çalıştırılamaması üzerine **SAP NetWeaver RFC SDK 7.50** paketi üzerine geliştirilmiştir.

Kütüphanemizi .Net Core 3.1, Net Core 2.1 and .Net Frameworklerinde kullanılabilirsiniz.

.Net Stadand 2.0 ve .Net Standard 2.1 ile hazırlanması sebebi ile Windows, Linux ve MacOs işletim sistemlerinde çalıştırılabilir.


## Gereksinimler

Kütüphanemiz SAP NetWeaver RFC Library 7.50 SDK içerisindeki C++ ile geliştirilmiş .dll dosyalarına ihtiyaç duyar.
İlgili SDK paketinin kurulumu ve daha fazlası için :sparkles: [SAP'nin](https://support.sap.com/en/product/connectors/nwrfcsdk.html) offical sayfasına bakabilirsiniz.

İlgili SKD yı temin ettikten sonra (Sizden bir SAP lisanlı hesabı istiyor);
- zip doyasınızı kendinize göre bir dizine çıkarıp işletim sisteminize göre `PATH` (Windows), `LD_LIBRARY_PATH` (Linux), `DYLD_LIBRARY_PATH` (MacOS)  ortam değişkenine çıkardığınız dosyalar içerisindeki **lib** dizinini gösterin.
- ve ya direkt **lib** klasörünün içeriğini output dizininize kopyalayın. 

 Windows işletm sisteminde  SDK paketini kullanabilmeniz için [Visual C++ 2013 yeniden dağıtılabilir paket](https://www.microsoft.com/en-us/download/details.aspx?id=40784)inin 64 bit sürümünü yüklemeniz gerekir.

## Kurulum
Şuan için BETA sürümünde olan paketleri deneyebilmek için

PackageManager
```shell
Install-Package SapCo2
```
or
DotNet
```shell
dotnet add package SapCo2
```

**Note:** Paket [Microsoft.Extensions.DependencyInjection](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection/) ve [Microsoft.Extensions.Options](https://www.nuget.org/packages/Microsoft.Extensions.Options) bağlılığına sahiptir.

## Kullanım
kullanımını ağırlıklı olarak dependencyInjection üzerine kullandım. Size de bu şekilde kullanımını tavsiye ediyorum. Zaten bu kullanımı pratik hale getirmek için tüm paketlerin içerisinde extensions hazırladım.

Bağlantı cümlesini direkt string olarak
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


Dependencyinjection ile IServceProvider içerisinde .AddSapCo2 fonksiyonuna bir adet connection set ederek kullanabilriz.
```csharp
var serviceCollection = new ServiceCollection()
    .AddSapCo2(connectionString);
var serviceProvider = serviceCollection.BuildServiceProvider();
```
paketimizi yükledikten sonra bir adet connection açıyoruz. Bu işlem şimdilik pool ile yönetilemediği için her SAP isteğinde bu connection'ı açmamız gerekiyor.
```csharp
using var connection = serviceProvider.GetService<IRfcConnection>();
connection.Connect();
```
***Note:*** burada `connection.Connect();` demezseniz çalışma zamanında `RFC CONNECTION ERROR` alırsınız.

ilk aşamada `RFC_READ_TABLE` kullanarak tablodan çekim işleminin nasıl yapıldığıa bakalım.

Öncelikle çekeceğimiz veriye ait Entitymizi oluşturuyoruz. Entity'yi oluşturuken tekrar  tablo isimleriyle uğraşmamak için ilgili entity'ye bir `[RfcClass("LFA1", Description = "Vendor Table", Unsafe = false)]` attribute bağladım. `Name alanı LFA1` ile veri almak istediğimiz tabloyu belirliyor. `unsafe` property'si ise çekilecek tablo içerisindeki alanların sonradan oluşturulmuş olan "Z" li geliştirmelerin dahil olup olmadığını anlatıyor.
`RfcProperty` atribütü ise ise Entity içerisindeki property'lerin tablodaki Field'lar ile eşleştirmemize yarıyor.Bu eşleştirme işleminde `Name` alanı field alanını işaret etmektedir. `DataType` alanı ise çekilecek property'nin ne geleceğini hakkında bir fikir belirtip bu tipe göre tekrar tür Entity türüne dönüşüm işlemi yapılıyor.


Satıcı Tanımlarını almak için aşağıdaki gibi bir entity tanımlıyorum.
```csharp
[RfcClass("LFA1", Description = "Vendor Table", Unsafe = false)]
public class Vendor
{

    [RfcProperty("LIFNR", DataType = RfcDataTypes.Char)]
    public string VendorCode { get; set; }

    [RfcProperty("KUNNR", DataType = RfcDataTypes.Char)]
    public string CustomerCode { get; set; }

    [RfcProperty("NAME1", DataType = RfcDataTypes.Char)]
    public string Name1 { get; set; }

    [RfcProperty("NAME2", DataType = RfcDataTypes.Char)]
    public string Name2 { get; set; }
}
```


Bunun için bir tane `IReadTable` nesnesi alıyoruz ve çekmek istediğimiz entity oluşturuyoruz.
```csharp
var rowCount = 5;
var query = new AbapQuery().Set(QueryOperator.Equal("BRSCH", "SD00")).GetQuery();

using var tableFunction = _serviceProvider.GetService<IReadTable<Vendor>>();
var result = tableFunction.GetTable(connection, query, rowCount: rowCount);
```
