MaestroPanel Zone Import Tool
============

Bu araç Microsoft DNS sunucu yazılımının ürettiği DNS Zone dosyalarını parse eder ve MaestroPanel API'sini kullanarak yeni domain ve DNS kayıtlarını aktarır.

Parametreler
============

key : MaestroPanel API Anahtarı (Nasıl Oluştururum? http://wiki.maestropanel.com/API-Key-Nedir.ashx)
host: MaestroPanel Host
port: MaestroPanel Port
ssl: MaestroPanel SSL Connection
plan: Default Domain Plan
path: DNZ Zone dizini (Örnek: C:\Windows\System32\dns)

Kullanım
============

mpzoneimport.exe --path="O:\dns" --key=1_9bd61d3da73040c3a8b214afb25e4656 --host=localhost --port=28411 --ssl=false --plan=default