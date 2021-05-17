[![license](https://img.shields.io/badge/license-MIT-blue?)](https://opensource.org/licenses/MIT)
[![Nuget](https://img.shields.io/nuget/v/sapco2?label=Nuget&logo=Nuget&logoColor=blue)](https://www.nuget.org/packages/SapCo2/)
[![Nuget-download](https://img.shields.io/nuget/dt/sapco2?logo=nuget)](https://www.nuget.org/packages/SapCo2/)

[![Multi Build And Test ( Net5.0, Core3.1, Core2.1 )](https://github.com/metalsimyaci/SapCo2/workflows/Multi%20Build%20And%20Test%20(%20Net5.0,%20Core3.1,%20Core2.1%20)/badge.svg?branch=master)](https://github.com/metalsimyaci/SapCo2/actions?query=workflow%3A%22Multi+Build+And+Test+%28+Net5.0%2C+Core3.1%2C+Core2.1+%29%22)

[Türkçe Döküman için](https://github.com/metalsimyaci/SapCo2/README_TR.md)

# SapCo2 (SAP Connector Core)

Referenced by [SapNwRfc](https://github.com/huysentruitw/SapNwRfc) and [NwRfcNet](https://github.com/nunomaia/NwRfcNet) projects that were previously developed during the development phase. I recommend that you take a look at these two projects before examining the project. Especially the Wrapper parts have emerged by combining the common aspects of the two projects.

SapCo2 is support .Net Core, Net 5 ve .Net Frameworks.

For detailed documentation, you can browse the [Proje Wiki](https://github.com/metalsimyaci/SapCo2/wiki) pages.

## Requiretment

Our library needs .dll files developed with C ++ in SAP NetWeaver RFC Library 7.50 SDK.
For the installation of the relevant SDK package and more: You can refer to the official page of: sparkles: [SAP](https://support.sap.com/en/product/connectors/nwrfcsdk.html).

After obtaining the relevant SKD (requires an SAP licensed account from you);

- Extract your zip file to a directory of your own and show the **lib** directory in the files you extract to the environment variable ``PATH`` (Windows), ``LD_LIBRARY_PATH`` (Linux), ``DYLD_LIBRARY_PATH`` (MacOS) according to your operating system.
- or directly copy the contents of the **lib** folder to your output directory.

 To use the SDK package in the Windows operating system, you must install the 64-bit version of [Visual C ++ 2013 redistributable package](https://www.microsoft.com/en-us/download/details.aspx?id=40784).
