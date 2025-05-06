# Aloe.Utils.ArgsHelper

`Aloe.Utils.ArgsHelper` は、コマンドライン引数を柔軟に前処理する軽量ユーティリティです。  
`--flag` のようなブールオプションの補完や、`-uadmin` のような短縮オプションと値の連結にも対応しています。  
.NET の `IConfigurationBuilder.AddCommandLine(...)` と組み合わせて使用することで、アプリケーションの設定構成が簡潔かつ堅牢になります。

## 主な機能

* --flag のような単独オプションを true に補完
* -uadmin → -u admin に分割
* プラグイン不要・ゼロ依存の軽量ユーティリティ
* DI / IConfiguration / appsettings.json などとの統合に最適

## 対応環境

* .NET 6 以降（.NET Core 3.1 でも動作可）
* Microsoft.Extensions.Configuration と併用可能

## Install

NuGet パッケージマネージャーからインストールします：

```cmd
Install-Package Aloe.Utils.ArgsHelper
```

あるいは、.NET CLI で：

```cmd
dotnet add package Aloe.Utils.ArgsHelper
```

## Usage

### 1. `ArgsHelper.PreprocessArgs` を使って、コマンドライン引数を整形

```csharp
using Aloe.Utils.ArgsHelper;

var processedArgs = ArgsHelper.PreprocessArgs(
    args,
    flagArgs: new[] { "--debug", "--standalone" },
    shortArgs: new[] { "-u", "-p" }
);
```

入力例

```cmd
myapp.exe -uadmin --debug
```

出力結果

```csharp
new[] { "-u", "admin", "--debug", "true" }
```

