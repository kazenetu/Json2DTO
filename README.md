# Json2DTO
# 概要
* Jsonを元にC#ソースコードを出力する

## 実行環境
* .NET6

## 実行方法
### ```dotnet run --project Json2DTO/Presentation/Console/Console.csproj <OutputPath> <targetString>  [options]```

### ```<OutputPath>``` C#ファイル出力パス

### ```[options]```
|コマンド          | ファイルパス      |備考|
|:----------------|:-----------------|:-------------|  
|```-ns, --namespace``` | ```<NameSpace>```    |名前空間を設定|

|```-pr, --prefix``` | ```<Prefix>```    |クラス名・ファイル名に付与されるプレフィックス|
|```-su, --suffix``` | ```<Suffix>```    |クラス名・ファイル名に付与されるサフィックス|
|```rc, --rootclass``` | ```<RootClass>```    |ルートクラス名<br>※JSON文字列の場合は必須|
|```-ic, --indentCount``` | ```<IndentCount>```    |インデントのスペース数<br>省略時は４スペース|

|```--h, --help```|                         | ヘルプページを表示する|

            System.Console.WriteLine("-  Input RootClass JsonString (Required JsonString)");
            System.Console.WriteLine("t  IndentSpaceCount(ex 2 or 4)");
            System.Console.WriteLine("-h, --help  view this page");

### 実行例
* JSON文字列  
  ```
  # リポジトリルートで実行
  dotnet run --project Json2DTO/Presentation/Console/Console.csproj CSOutput "{\"a\":1}" -ns "Test.Entity" -su "_suffix" -pr "prefix_" -rc "RootJson" -ic 4
  ```

* ファイル指定  
  ```
  # リポジトリルートで実行
  dotnet run --project Json2DTO/Presentation/Console/Console.csproj CSOutput "InputJsonFile/classA.json" -ns "Test.Entity" -su "File" -ic 2
  ```

* ディレクトリ指定  
  ```
  # リポジトリルートで実行
  dotnet run --project Json2DTO/Presentation/Console/Console.csproj CSOutput "InputJsonFile" -su "Directory"
  ```

## テスト方法
* .Net CLI  
  ```
  # リポジトリルートで実行
  dotnet test Json2DTO.Test/Json2DTO.Test.csproj
  ```

## フォルダ構成
後ほど作成

