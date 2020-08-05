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

 Windows işletm sisteminde  SDK paketini kullanabilmeniz için [Visual C++ 2013 yeniden dağıtılabilir paket](https://www.microsoft.com/en-us/download/details.aspx?id=40784)inin 64 bit sürümünü yüklemeniz gerekir. [^1] 
[^1]: SDK paket çalıştırma uyarısı

