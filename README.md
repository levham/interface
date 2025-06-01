# 📂 interface

> [!TIP]
> ✨Bu program ile kendi butonlu menünüzü data.json dosyası üzerinden yazabilirsiniz.<br/>→Program şurada -> [_**bin\Debug\net8.0-windows\a16.exe**_](https://github.com/levham/interface/blob/main/bin/Debug/net8.0-windows/a16.exe). 

> [!NOTE]
> data.json içeriğini değiştirdikten sonra a16.exe'yi çalştırabilirsiniz.<br>Json dosyası içeriğini dikkatli yazın.
 

### 📂 button.json
| ✨ Parametre | 📌 Açıklama|
|---------------|-------------------|
| _**form**_      | _pencere hakkında_|
| `name: "program"`       | _pencere adı_|
| `width: 500`       | _pencere genişliği_|
| `height: 120`      | _pencere yüksekliği_|
|`location:[0, 0]`     | _pencere konumu_|
| _**menu**_  | _menuler_|
| _list_    | _menuler için seçenekler_|
| _openfile_    | _dosya çalıştır_|
| _openfolder_    | _klasör çalıştır_|
| _**buttons**_  | _butonlar_|
| _text_    | _butonlar için seçenekler_|
| _openfile_    | _dosya çalıştır_|
| _openfolder_    | _klasör çalıştır_|
<br>
<br>

> [!TIP]
>  ✨✨<ins>**Örnek data.json**</ins> 

``` 
{
    "form": {
        "name":"Arayüz16.2.x",
        "width": 550,
        "height": 120,
        "location": [0, 0]
    },
    "menu": {
        "program": [
            { "id": 1, "list": "Taskmgr", "openfile": "C:\\Windows\\System32\\taskmgr.exe" },
            { "id": 2, "list": "Kaydet", "openfile": "C:\\Windows\\System32\\mspaint.exe" }
        ],
        "dosyalarım": [
            { "id": 1, "list": "System32", "openfolder": "C:\\Windows\\System32" }
        ]
    },
    "buttons": [
        [
            { "id": 1, "text": "Cmd", "openfile": "C:\\Windows\\System32\\cmd.exe" },
            { "id": 2, "text": "Notepad", "openfile": "C:\\Windows\\System32\\notepad.exe" }
        ],
        [
            { "id": 3, "text":  "System32", "openfolder": "C:\\Windows\\System32" },
            { "id": 4, "text": "Calculator", "openfile": "C:\\Windows\\System32\\calc.exe" }
        ]
    ] 
}
```
<br>

> [!NOTE]
> 📌**Programın Çalışma Örneği**

![output](image1.png)
<br>
---
<br>
<br>
