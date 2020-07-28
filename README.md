# SapCo2 (SAP Conector Core)

Geliştirme aşamasında daha önceden geliştirlmiş olan [SapNwRfc](https://github.com/huysentruitw/SapNwRfc) ve [NwRfcNet](https://github.com/nunomaia/NwRfcNet) projelerinden ilham alınmıştır. Projeyi incelemeden önce bu iki projeye göz atmanızı tavsiye ederim.


Kütüphanemiz SAPNco kütüphanesinin .NET Core Ortanımda çalıştırılamaması üzerine **SAP NetWeaver RFC SDK 7.50** paketi üzerine geliştirilmiştir.

Paketimiz .Net Core 3.1, Net Core 2.1 and .Net Framework kütüphanelerinde kullanılabilir.

Windows, Linux ve MacOs işletim sistemlerinde çalıştırılabilir.


## Gereksinimler

Bu kütüphane SAP NetWeaver RFC Library 7.50 SDK içerisindeki C++ ile geliştirilmiş .dll dosyalarına ihtiyaç duyar.
İlgili SDK paketinin kurulumu ve daha fazlası için :sparkles: [SAP'nin](https://support.sap.com/en/product/connectors/nwrfcsdk.html) connector bakabilirsiniz.

İlgili SKD yı temin ettikten sonra;
- zip doyasınızı kendinize göre bir dizine çıkarıp işletim sisteminize göre `PATH` (Windows), `LD_LIBRARY_PATH` (Linux), `DYLD_LIBRARY_PATH` (MacOS)  ortam değişkenine çıkardığınız dosyalar içerisindeki **lib** dizinini gösterin.
- ve ya direkt **lib** klasörünün içeriğini output dizininize kopyalayın. 

 Windows işletm sisteminde  SDK paketini kullanabilmeniz için [Visual C++ 2013 yeniden dağıtılabilir paket](https://www.microsoft.com/en-us/download/details.aspx?id=40784)inin 64 bit sürümünü yüklemeniz gerekir. [^1] 
[^1]: SDK paket çalıştırma uyarısı

