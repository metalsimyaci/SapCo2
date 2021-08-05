[![license](https://img.shields.io/badge/license-MIT-blue?)](https://opensource.org/licenses/MIT)
[![Nuget](https://img.shields.io/nuget/v/sapco2?label=Nuget&logo=Nuget&logoColor=blue)](https://www.nuget.org/packages/SapCo2/)
[![Nuget-download](https://img.shields.io/nuget/dt/sapco2?logo=nuget)](https://www.nuget.org/packages/SapCo2/)

[![Multi Build And Test ( Net5.0, Core3.1, Core2.1 )](https://github.com/metalsimyaci/SapCo2/workflows/Multi%20Build%20And%20Test%20(%20Net5.0,%20Core3.1,%20Core2.1%20)/badge.svg?branch=master)](https://github.com/metalsimyaci/SapCo2/actions?query=workflow%3A%22Multi+Build+And+Test+%28+Net5.0%2C+Core3.1%2C+Core2.1+%29%22)

[Türkçe Dökümantasyon](https://github.com/metalsimyaci/SapCo2/blob/master/README_TR.md)

# SapCo2 (SAP Connector Core)

We tried to design SAPCO2 as a library for data processing over SAP in the simplest and fastest way. We focused on quick use, simplifying it as much as possible.

Referenced by [SapNwRfc](https://github.com/huysentruitw/SapNwRfc) and [NwRfcNet](https://github.com/nunomaia/NwRfcNet) projects that were previously developed during the development phase. I recommend that you take a look at these two projects before examining the project. Especially the Wrapper parts have emerged by combining the common aspects of the two projects.

SapCo2 is support .Net Core, Net 5 ve .Net Frameworks.

## Requiretment

Our library needs .dll files developed with C ++ in SAP NetWeaver RFC Library 7.50 SDK.
For the installation of the relevant SDK package and more: You can refer to the official page of: sparkles: [SAP](https://support.sap.com/en/product/connectors/nwrfcsdk.html).

After obtaining the relevant SKD (requires an SAP licensed account from you);

- Extract your zip file to a directory of your own and show the **lib** directory in the files you extract to the environment variable ``PATH`` (Windows), ``LD_LIBRARY_PATH`` (Linux), ``DYLD_LIBRARY_PATH`` (MacOS) according to your operating system.
- or directly copy the contents of the **lib** folder to your output directory.

 To use the SDK package in the Windows operating system, you must install the 64-bit version of [Visual C ++ 2013 redistributable package](https://www.microsoft.com/en-us/download/details.aspx?id=40784).

## Instalization

PackageManager

```shell
Install-Package SapCo2
```

Or DotNet

```shell
dotnet add package SapCo2
```
or Package Referance
```xml
<PackageReference Include="SapCo2" Version="1.2.0.1" />
```

**Note:** Has dependencies [Microsoft.Extensions.DependencyInjection](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection/) and [Microsoft.Extensions.Options](https://www.nuget.org/packages/Microsoft.Extensions.Options).

## Usage

You can access all the examples described in the document with the projects under the [Samples](https://github.com/metalsimyaci/SapCo2/tree/master/src/SapCo2/Samples) directory.

To use it, we add it with `.AddSapCo2` on `Dependency injection`. It needs connection information in the add process.


### Connection 

To assign the connection information as a string

```csharp
var connectionString = "Name=ALIAS1;AppServerHost=HOST_NAME; SystemNumber=00; User=USER; Password=PASSWORD; Client=CLIENT_CODE; Language=EN; PoolSize=100; Trace=0;";
```

or multiple in **Appsettings.json**

```json
{
  "SapCo2": {
    "DefaultServer": "ALIAS1",
    "Connections": [
      {
        "Alias": "ALIAS1",
        "ConnectionPooling": {
          "Enabled": false,
          "PoolSize": 8,
          "IdleTimeout": "00:00:30",
          "IdleDetectionInterval": "00:00:01"
        },
        "ConnectionString": "Name=ALIAS1;User=USER;Password=PASSWORD;Client=CLIENT_CODE;SystemId:xxx;Language=EN;AppServerHost=HOST_NAME;SystemNumber=00;MaxPoolSize:100;PoolSize=50;IdleTimeout:600;Trace=0;",
        "ConnectionOptions": {}
      },
      {
        "Alias": "ALIAS2",
        "ConnectionPooling": {
          "Enabled": false,
          "PoolSize": 8,
          "IdleTimeout": "00:00:30",
          "IdleDetectionInterval": "00:00:01"
        },
        "ConnectionString": "",
        "ConnectionOptions": {
          "Name": "ALIAS2",
          "User": "USER",
          "Password": "PASSWORD",
          "Client": "CLIENT_CODE",
          "SystemId": "xxx",
          "Language": "TR",
          "AppServerHost": "HOST_NAME",
          "SystemNumber": "00",
          "MaxPoolSize": "100",
          "PoolSize": "50",
          "IdleTimeout": "600",
          "Trace": "0"
        }
      }
    ]
  }
}
```

in **appsetting.json**;

- You can assign connection information with `ConnectionString` or `ConnectionOptions`.
- You can define multiple connections as ALIAS1 or ALIAS2.
- Set a Default connection, it is sufficient to specify the `Alias` in the connection definitions in the `DefaultServer` part.

### Dependency Injection

Startup.cs veya Program.cs veya vb.

```csharp
IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: true)
    .AddEnvironmentVariables()
    .Build();
```

Dependencyinjection ile IServceProvider içerisinde `.AddSapCo2` fonksiyonuna IConfiguration  bilgisi atayarak kullanabiliriz.

#### IConfiguration with Appsettings

```csharp
var serviceCollection = new ServiceCollection();
serviceCollection.AddOptions<IConfiguration>();
serviceCollection.Configure<IOptions<IConfiguration>>(configuration);
serviceCollection.AddSapCo2(s=>s.ReadFromConfiguration(configuration));
var serviceProvider = serviceCollection.BuildServiceProvider();
```

#### Custom Configuration

```csharp
var serviceCollection = new ServiceCollection()
    .AddSapCo2(s =>
            {
                s.DefaultServer = "LIVE";
                s.RfcServers = new List<RfcServer>()
                {
                    new RfcServer()
                    {
                        Alias = "LIVE",
                        ConnectionString =
                            "Name=LIVE;User=USER;Password=PASSWORD;Client=CLIENT_CODE;SystemId:xxx;Language=EN;AppServerHost=HOST_NAME;SystemNumber=00;MaxPoolSize:100;PoolSize=50;IdleTimeout:600;Trace=0;",
                        ConnectionPooling = new RfcConnectionPoolingOption() {Enabled = true, PoolSize = 10}
                    }
                };
            });
var serviceProvider = serviceCollection.BuildServiceProvider();
```

### IRFCClient Implementation 

SAPCO2 can be used in 3 ways. [**Table**](####Table), [**Bapi**](####Bapi) and [**RFC**](####RFC).
In addition to these, a feature has been added where we will receive [Meta Data](####Meta-Data)  for RFC and BAPI.

#### RFC

The most basic of these is RFC (Remote Functon Call). In other stages, it is actually a customized and simplified form of it. You can perform all operations using only RFC.

It will be sufficient if you create your **input** and **output** objects before the function call and pass them as parameters.
As an example, I used the recipe function **`CS_BOM_EXPL_MAT_V2_RFC`** for RFC. associated with it
I create the **BomInputParameter** class for the parameters I want to give as a condition, and the **BomOutputParameter** class for the values ​​I want to get back.
I create these classes by selecting the fields I want to use from import and export, Table objects related to SAP `SE37`.

By marking my classes that I created with the `RfcEntityPropertAttribute` attribute, we specify the corresponding name on the **SAP** side. If we want, we can also provide an explanation of what happened.

I exclude properties that are not on the SAP side but are in my class with the `RfcEntityIgnorePropertyAttribute` attribute.

<details>
<summary>BomInputParameter Class</summary>

```csharp
public sealed class BomInputParameter: IRfcInput
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

</details>

As a condition, I define the production site code with `plantcode` and the material for which I want to get a recipe as a variable with `materialCode`.

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

For the return values, we reference the structor and the table in our **BomOutputParameter** class. We show the tables as `[]` array.

``` csharp
public sealed class BomOutputParameter: IRfcOutput
{
    [RfcEntityProperty("STB")]
    public Stb[] StbData { get; set; }

    [RfcEntityProperty("TOPMAT")]
    public Topmat Topmat { get; set; }
}
```

<details>
<summary>Table Stb Class</summary>

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

</details>

<details>
<summary>Struct Topmat Class</summary>

```csharp
public sealed class Topmat
{
    [RfcEntityProperty("MATNR", Description = "Malzeme Tanımı")]
    public string Code { get; set; }

    [RfcEntityProperty("MAKTX", Description = "Tanım")]
    public string Definition { get; set; }
}
```
</details>

We take one `IClient` object from dependency injection and call the `ExecuteRfcAsync` function.

```csharp
using IRfcClient client = _serviceProvider.GetRequiredService<IRfcClient>();
BomOutputParameter bomResult = await client.ExecuteRfcAsync<BomInputParameter, BomOutputParameter>("CS_BOM_EXPL_MAT_V2_RFC", inputParameter);
```

As a result, we can see the desired output from the `BomOutputParameter` type in the `bomResult`.

Here, the connection is made via the DefaultAlias we created in ``AppSettings``. To do this via a different alias, we need to call the ``UseServer`` function before the ``Execute`` function.

```csharp
using IRfcClient client = _serviceProvider.GetRequiredService<IRfcClient>();
client.UseServer("ALIAS_NAME");
BomOutputParameter bomResult = await client.ExecuteRfcAsync<BomInputParameter, BomOutputParameter>("CS_BOM_EXPL_MAT_V2_RFC", inputParameter);
```

<details>
<summary>The entire the code</summary>

```csharp
public BomOutputParameter GetBillOfMaterial(string materialCode, string plantCode)
{
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
    using IRfcClient client = _serviceProvider.GetRequiredService<IRfcClient>();
    BomOutputParameter bomResult = await client.ExecuteRfcAsync<BomInputParameter, BomOutputParameter>("CS_BOM_EXPL_MAT_V2_RFC", inputParameter);
    return bomResult;
}

```

</details>

#### Bapi

**BAPI** is a special type of RFC that is created by the system and is used for operations such as updating, deleting, creating, and contains a return value named `RETURN` for each operation result.

To make your bapi calls easier, you should derive your output objects from the `IBapiOutput` interface or from the `BapiOutputBase` abstrak class. This will automatically add the `RETURN` object in your output. After the operation, the MessageType value of this object is **Abort** or **Error**
error is thrown. The rest of the operations are the same as for the RFC call.

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

I am using `BBP_VENDOR_GETLIST` bapi for example. This bapi returns the code and name of the vendors in the system according to the Company code it contains.

Input

```csharp
public class VendorBapiInputParameter: IBapiInput
{
    [RfcEntityProperty("COMP_CODE")]
    public string CompanyCode { get; set; }
}
```

Output

```csharp
public class VendorBapiOutputParameter:IBapiOutput
{
    [RfcEntityProperty("RETURN")]
    public RfcBapiOutputParameter BapiReturn { get; set; }

    [RfcEntityProperty("VENDOR")]
    public Vendor[] Vendors { get; set; }
}
```

<details>
<summary>Table Vendor Class</summary>

```csharp
public class Vendor
    {
        [RfcEntityProperty("VENDOR_NO")]
        public string VendorNo { get; set; }

        [RfcEntityProperty("NAME")]
        public string Name { get; set; }
    }
```
</details>

Note that it derives from the `IRfcBapiOutput` interface and has the attribute `[RfcEntityProperty("RETURN")]`.

It happens instantly with the post RFC process. The `IRfcClient` object is superimposed and executed by calling the `ExecuteBapiAsync` function.

```csharp

    using IRfcClient sapClient = _serviceProvider.GetRequiredService<IRfcClient>();
    return await sapClient.ExecuteBapiAsync<VendorBapiInputParameter, VendorBapiOutputParameter>("BBP_VENDOR_GETLIST", inputParameter);
```

`companyCode` values set `200`

<details>
<summary>The entire the code</summary>

```csharp
public VendorBapiOutputParameter GetVerdorsByCompanyCode(string companyCode)
{
    var inputParameter = new VendorBapiInputParameter
    {
        CompanyCode = companyCode
    };

    using IRfcClient sapClient = _serviceProvider.GetRequiredService<IRfcClient>();
    return await sapClient.ExecuteBapiAsync<VendorBapiInputParameter, VendorBapiOutputParameter>("BBP_VENDOR_GETLIST", inputParameter);
    return result;
}
```
</details>

#### Table

Since we do not have direct access to the table objects, the system presented to us
 Using the `RFC_READ_TABLE` function, we perform the pull from the table.

 `RFC_READ_TABLE` has certain standard rules. We perform data extraction from the table by assigning the relevant data to those fields. We save you from all this trouble and produce our table function from the `ISapTable` interface for a simpler use.

 After the basic RFC process here, the `WA` struct in the `DATA` table, which is output by RFC_READ_TABLE, is split according to the shredding character you specify and assigned to the relevant class properties.

 Since the transformation operations during this process are handled differently from the RFC Map operation, you must mark the object you will use as a table with `RfcEntity` properties and `RfcEntityProperty` attributes. Points to the table with the `RrfcTable` name field. Again, it has the `unsafe` feature to be found as information. This generally indicates that the RFC is not up to SAP's standards and is specifically overridden. For our systems, it defines the enhancements referred to as '**Z**'. Likewise, it has `TablePropertySapType` and `Length` properties for transforming properties. In addition, **unsafe** is used to indicate whether this feature is an enhancement, in the same way as the table. `SubTypePropertyName` shows linked tables that will not be pulled in the first place. For now, we fill them by writing the `SetOption` method ourselves. We define these fields as virtual so that they are not taken into account in our normal data.

 Instead of the input parameter, the `TEXT` property of the `OPTIONS` table of `RFC_READ_TABLE` is written as text with `ABAP` of the condition statements. You can create this either by creating a text list in the form of `MATNR EQ '12344555'` in plain text, or with `Abap Query`, which we can chain by analogy to Linq. I will add details about `Abap Quey` in [Project Wiki](https://github.com/metalsimyaci/SapCo2/wiki). `FIELDS` fields are automatically assigned according to the attributes in the output class you created. Features such as `ROWCOUNT`, `ROWSKIPS`, `ROWDATA` are **optional** and can be crushed when necessary.

 Without further ado, let's look at the example.
 For the example, I will pull the `MARA` table and its sub-details (Material definition `MAKT` and Material class `ZMM24030`) from the tables.
 
 NOTE: If the Material classes table in the sub-details is not included in your SAP system because it is a development table, please replace `new MaterialQueryOptions { IncludeAll = true }` with `new MaterialQueryOptions { IncludeDefinition = true }`. In this way, you only pull the `MAKT` table. 

 ```csharp
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
 ```

 Afterwards, we create the conditions for which we will draw our painting with `AbapQuey`. We set the condition as `MATNR` material definition starting with `materialCodePrefix` and not marked as deleted.

 ```csharp
 List<string> whereClause = new AbapQuery().Set(QueryOperator.Equal("MATNR", materialCode))
                .And(QueryOperator.NotEqual("LVORM", true, RfcDataTypes.BOOLEAN_X)).GetQuery();
 ```

We create our `IRfcClient` object and with the `GetTableDataAsync` method we specify whether we want the condition we want to retrieve, the number of records and the unsafe (included with Development) fields.

 ```csharp
using IRfcClient sapClient = _serviceProvider.GetRequiredService<IRfcClient>();
List<Material> materials = await sapClient.GetTableDataAsync<Material>(whereClause, rowCount: recordCount);
return await SetOptionsAsync(materials, new MaterialQueryOptions { IncludeAll = true });
 ```
<details>
 <summary>Kodun tamamı</summary>

 ```csharp
 public async Task<List<Material>> GetMaterialsByPrefixAsync(string materialCodePrefix,
            MaterialQueryOptions options = null, bool getUnsafeFields = true, int rowCount = 0)
{
    options ??= new MaterialQueryOptions();
    List<string> whereClause = new AbapQuery().Set(QueryOperator.StartsWith("MATNR", materialCodePrefix))
        .And(QueryOperator.NotEqual("LVORM", true, RfcDataTypes.BOOLEAN_X)).GetQuery();

    using IRfcClient sapClient = _serviceProvider.GetRequiredService<IRfcClient>();
    List<Material> result = await sapClient.GetTableDataAsync<Material>(whereClause, rowCount: recordCount);

    return await SetOptionsAsync(result, options, getUnsafeFields).ConfigureAwait(false);
}
 ```
 </details>

<details>
<summary>Sub Table Operations</summary>

That's it based on the usage of your table reading event. to fetch child tables
We create a function called `setoptions` and connect the sub-tables we get with it to our main `MARA` table. This process was created as an example of using fields such as `unsafe`, `SubProperty`.

<details>
<summary>Sub Table Class</summary>

Material Definition (MAKT) Table Class

 ```csharp
[RfcEntity("MAKT", Description = "Material Definition Table")]
public class MaterialDefinition : ISapTable
{
    [RfcEntityProperty("MATNR", Description = "Material Code", SapDataType = RfcDataTypes.CHAR, Length = 18)]
    public string Code { get; set; }

    [RfcEntityProperty("MAKTX", Description = "Material Short Definition", SapDataType = RfcDataTypes.CHAR, Length = 40)]
    public string Definition { get; set; }
}
 ```

Material Category (ZMM24030) Table Class

 ```csharp
[RfcEntity("ZMM24030", Description = "Material Category Definition Table", Unsafe = true)]
public class MaterialCategoryDefinition : ISapTable
{
    #region Properties

    [RfcEntityProperty("ZZEXTWG", Description = "Material Category Code", SapDataType = RfcDataTypes.CHAR, Length = 25)]
    public string Code { get; set; }

    [RfcEntityProperty("TANIM", Description = "Material Category Definition", SapDataType = RfcDataTypes.CHAR, Length = 50)]
    public string Definition { get; set; }

    [RfcEntityProperty("ZZPARCASAYI", Description = "Material Category Unit Part Count", SapDataType = RfcDataTypes.INTEGER, Length = 10)]
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

</details>

Sub Table Operations

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

Material Defitinion Operations

```csharp
private List<Task> SetMaterialDefinitionOptionAsync(MaterialQueryOptions queryOptions, List<Material> materialList, ConcurrentQueue<MaterialDefinition> definitionList)
{
    var taskList = new List<Task>();
    if (!queryOptions.IncludeDefinition)
        return taskList;

    var list = materialList.Where(x => x.Code != null).Select(x => (object)x.Code).ToList();
    List<object>[] parts = list.Partition(PartitionCount);

    foreach (List<object> part in parts)
    {
        List<string> query = new AbapQuery()
            .Set(QueryOperator.In("MATNR", part))
            .GetQuery();

        taskList.Add(Task.Run(async () =>
        {
            using IRfcClient client = _serviceProvider.GetRequiredService<IRfcClient>();
            List<MaterialDefinition> definitions = await client.GetTableDataAsync<MaterialDefinition>(query);
            foreach (MaterialDefinition definition in definitions)
                definitionList.Enqueue(definition);
        }));
    }

    return taskList;
}
```

Material Category Operations

```csharp
private List<Task> SetMaterialCategoryOptionAsync(MaterialQueryOptions queryOptions, List<Material> materialList, ConcurrentQueue<MaterialCategoryDefinition> materialCategoryDefinitionList)
{
    var taskList = new List<Task>();
    if (!queryOptions.IncludeMaterialCategory)
        return taskList;

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
            using IRfcClient client = _serviceProvider.GetRequiredService<IRfcClient>();
            List<MaterialCategoryDefinition> definitions = await client.GetTableDataAsync<MaterialCategoryDefinition>(queryList);
            foreach (MaterialCategoryDefinition definition in definitions)
                materialCategoryDefinitionList.Enqueue(definition);
        }));
    }

    return taskList;
}
```

</details>

#### Meta Data

In order to get the metadata information of the function we are used to from SapNco, I prepared a method that will combine a Field and Parameter information in SapNetwareRFC. Method only works for BAPI and RFC. Unfortunately, we cannot access its metadata from here, as we get the tables via RFC_READ_TABLE.

In the future, I will have a study to get this metadata from tables where table definitions such as DL003 are kept.

Similar to other uses, we call the ``ReadFunctionMetaData`` function after generating the ``IRfcClient`` object. This method returns us a list of type ``ParameterMetaData``. This list holds objects in ``Import``,``Export``,``Tables`` that we list with <b>SE37</b>. We take the elements of these objects under the ``Field`` list.

```csharp
public class ParameterMetaData
{
    public string Name { get; set; }
    public string Type { get; set; }
    public string Direction { get; set; }
    public int NumericLength { get; set; }
    public int UcLength { get; set; }
    public int Decimals { get; set; }
    public string Description { get; set; }
    public string DefaultValue { get; set; }
    public bool Optional { get; set; }

    public List<FieldMetaData> Fields { get; set; }

}
```
```csharp
public class FieldMetaData
{
    public string Name { get; set; }
    public string Type { get; set; }
    public int NucLength { get; set; }
    public int NucOffset { get; set; }
    public int UcLength { get; set; }
    public int UcOffset { get; set; }
    public int Decimals { get; set; }
}
```

Just give the name of the RFC or BAPI whose metadata you want to access.

```csharp
using IRfcClient client = _serviceProvider.GetRequiredService<IRfcClient>();
List<ParameterMetaData> result = client.ReadFunctionMetaData(functionName);
```

That's all for now.
