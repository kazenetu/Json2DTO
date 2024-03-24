# Json2DTO
# 概要
* Jsonを元にC#ソースコードを出力する

## 実行環境
* .NET6

## 実行方法
 ```dotnet run --project Json2DTO/Presentation/Console/Console.csproj <OutputPath> <targetString>  [options]```

*  ```<OutputPath>``` C#ファイル出力パス

* ```[options]```  
  |コマンド          | ファイルパス      |備考|
  |:----------------|:-----------------|:-------------|  
  |```-ns, --namespace``` | ```<NameSpace>```    |名前空間を設定|
  |```-pr, --prefix``` | ```<Prefix>```    |クラス名・ファイル名に付与されるプレフィックス|
  |```-su, --suffix``` | ```<Suffix>```    |クラス名・ファイル名に付与されるサフィックス|
  |```rc, --rootclass``` | ```<RootClass>```    |ルートクラス名<br>※JSON文字列の場合は必須|
  |```-ic, --indentCount``` | ```<IndentCount>```    |インデントのスペース数<br>省略時は４スペース|
  |```--h, --help```|                         | ヘルプページを表示する|

### 実行例
* JSON文字列  
  ```
  # リポジトリルートで実行(dotnet6)
  dotnet run --project Json2DTO/Presentation/Console/Consol6.csproj CSOutput "{\"a\":1}" -ns "Test.Entity" -su "_suffix" -pr "prefix_" -rc "RootJson" -ic 4
  ```

* ファイル指定  
  ```
  # リポジトリルートで実行(dotnet6)
  dotnet run --project Json2DTO/Presentation/Console/Console6.csproj CSOutput "InputJsonFile/classA.json" -ns "Test.Entity" -su "File" -ic 2
  ```

* ディレクトリ指定  
  ```
  # リポジトリルートで実行(dotnet6)
  dotnet run --project Json2DTO/Presentation/Console/Console6.csproj CSOutput "InputJsonFile" -su "Directory"
  ```

## テスト方法
* .Net CLI  
  ```
  # リポジトリルートで実行(dotnet6)
  dotnet test Json2DTO.Test/Json2DTO.Test6.csproj
  ```

## フォルダ構成
* Json2DTO  
  プログラム本体
  * Appplication
    * Commands  
      * CommonCommand.cs  
      * CSharpCommand.cs ※言語変換コマンドクラス(C#用)
    * Models  
      * ConvertResultModel.cs  
       ソースコード変換結果モデルクラス  
       (DomainのFileOutputResultを元に作成)
    * ApplicationBase.cs  
      アプリケーションサービスクラスのスーパークラス  
      DIコンテナ経由でインターフェイスにインスタンスを設定
    * ClassesApplication.cs  
      ソース変換アプリケーションサービスクラス
    * Application.csproj  
      アプリケーション層のプロジェクトファイル

  * Domain  
    ValueObject・EntityとRepositoryのインターフェイス
    * Entities
      * ClassesEntity.cs  
        集約クラス  
        * ClassEntityリスト：インナークラスリスト
        * ルートクラス：メインクラス

      * ClassEntity.cs  
        クラスエンティティ
        * プロパティリスト：クラス内のプロパティリスト

    * ValueObjects
      * PropertyValueObject.cs  
        プロパティValueObject
        * プロパティ型：プロパティの型

      * PropertyType.cs  
        プロパティ型ValueObject

    * Interfaces
      * IJsonRepository.cs  
        JSON読み込みリポジトリインターフェース

      * IFileOutputRepository.cs  
        ファイル出力リポジトリインターフェース

    * Commands
      * FileOutputCommand.cs  
        ファイル出力コマンドクラス  
        名前空間や出力先などを設定するクラス

    * Results
      * FileOutputResult.cs  
        ソースコード変換結果クラス  
        処理結果を格納するクラス

    * Domain.csproj  
      ドメイン層のプロジェクトファイル

  * Infrastructure  
    インフラ層  
    * FileOutputRepository.cs  
      ファイル出力リポジトリ  
      集約クラスからソースコードを出力するためのクラス
      
    * JsonRepository.cs  
      JSON読み込みリポジトリ  
      JSON文字列を読み込んでドメイン集約クラス返す
      * JsonProperties：プライベートプロパティ  
        IJsonPropertyのリスト  
        System.Linq.Whereを使って対象のJsonPropertyを選択する

    * JsonProperties  
      ストラテジパターンで実装されており、JsonRepositoryから呼ばれる。
      JsonValueKindを元にJsonPropertyResultを返す

      * IJsonProperty.cs  
        Jsonプロパティインターフェイス  

      * JsonPropertyArray.cs  
        IJsonPropertyの実装クラス  
        JsonValueKind.Array用  
        配列型に変換する(配列要素は再起的に取得する)

      * JsonPropertyFalse.cs  
        IJsonPropertyの実装クラス  
        JsonValueKind.False用  
        boolに変換する
        
      * JsonPropertyTrue.cs  
        IJsonPropertyの実装クラス  
        JsonValueKind.True用  
        boolに変換する
        
      * JsonPropertyNull.cs  
        IJsonPropertyの実装クラス  
        JsonValueKind.Null用  
        Nullableに変換する
        
      * JsonPropertyNumber.cs  
        IJsonPropertyの実装クラス  
        JsonValueKind.Number用  
        decimalに変換する
        
      * JsonPropertyObject.cs  
        IJsonPropertyの実装クラス  
        JsonValueKind.Object用  
        インナークラスに変換する
        
      * JsonPropertyString.cs  
        IJsonPropertyの実装クラス  
        JsonValueKind.String用  
        stringに変換する

      * JsonPropertyResult.cs  
        ドメイン層のPropertyValueObjectとサブクラス情報を内包したクラス

    * Extensions  
      拡張クラス  
      * NamingRules.cs  
        各言語の規約に則ったクラス・プロパティ変換を行う。  
        リポジトリや各言語用ソースコード変換クラスから呼ばれることを想定

    * Utils  
      ソースコード変換ユーティリティ
      * IConverter.cs  
        ソース変換用インターフェース

      * SoruceConverter.cs  
        ソース変換ユーティリティのエントリクラス  
        各言語用ソースコード変換変換メソッドを実装する  

      * CSConverter.cs  
        C#ソースコード変換クラス  

    * Infrastructure.csproj  
      インフラ層のプロジェクトファイル

* Lib/TinyDIContainer
  * DIContainer.cs  
    拙作「[TinyDIContainer](https://github.com/kazenetu/DIContainer)」を改良した簡易DIコンテナ

* Presentation
  * ArgManagers.cs    
    パラメータ管理クラス  
    必須パラメータや任意パラメータを定義する
  * Program.cs    
    Json文字列とJson解析を実施するエントリクラス
  * Console.csproj  
      プレゼンテーション層のプロジェクトファイル

* Json2DTO.Test  
  テスト
  * Appplication
    * Extensions
      * ClassesApplicationExtensions.cs  
        ClassesApplicationテスト用拡張メソッド  
        例外テストのためにリポジトリインターフェイスをnullに設定する
    * Stub
      * FileOutputRepositoryStub.cs  
        ファイル出力リポジトリクラスのスタブ
      * JsonRepositoryStub.cs  
        JSON読み込みリポジトリのスタブ
    * ClassesApplicationTest.cs  
      アプリケーション層のテスト

  * Domain  
    * Entities  
      エンティティクラスのテスト
      * ClassEntityTest.cs
      * ClassesEntityTest.cs
    * ValueObjects  
      値オブジェクトのテスト
      * PropertyTypeTest.cs
      * PropertyValueObjectTest.cs

  * Infrastructure  
    リポジトリのテスト
    * FileOutputRepositoryTest.cs
    * JsonRepositoryTest.cs

  * Usings.cs  
    テスト全体で使用するusing定義

  * Json2DTO.Test.csproj  
    テストプロジェクト

