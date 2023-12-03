# 簡易DIコンテナ

## はじめに
C#での簡易的なDIコンテナのプログラムです。

## 実行環境
* .NET 5  

## 実行方法
* ローカル実行  
    dotnet runで実行する。  
    ```sh
    dotnet run --project ./src/DIContainerConsole/DIContainerConsole.csproj
    ```  

* Dockerコンテナでの実行  
    Dockerコンテナ上で開発環境を構築する。  
   * 前提  
     * Docker EngineやDocker Desktopがインストール済みであること。

   * 実行手順  
     dotnetコンテナを起動する。
      1. docker_devに移動  
          ```sh
          cd docker_dev
          ```

      1. (**初回のみ**)ビルド  
          ```sh
          docker-compose build
          ```

      1. コンテナ起動  
          ```sh
          docker-compose up -d
          ```

      1. コンテナに入る  
          ```sh
          docker exec -it docker_dev_dotnet_1 /bin/bash
          ```

      1. コンテナ内で実行 
          1. dotnet runで実行する。
              ```sh
              dotnet run --project ./src/DIContainerConsole/DIContainerConsole.csproj
              ```

          1. コンテナから離脱する。
              ```sh
              exit
              ```

      1. コンテナ停止・削除  
          ```sh
          docker-compose down
          ```

## テスト方法
* ローカル実行  
    dotnet runで実行する。  
    ```sh
    dotnet test ./src/DIContainerTest/DIContainerTest.csproj
    ```  

* Dockerコンテナでの実行  
    Dockerコンテナ上で開発環境を構築する。  
   * 前提  
     * Docker EngineやDocker Desktopがインストール済みであること。

   * 実行手順  
     dotnetコンテナを起動する。
      1. docker_devに移動  
          ```sh
          cd docker_dev
          ```

      1. (**初回のみ**)ビルド  
          ```sh
          docker-compose build
          ```

      1. コンテナ起動  
          ```sh
          docker-compose up -d
          ```

      1. コンテナに入る  
          ```sh
          docker exec -it docker_dev_dotnet_1 /bin/bash
          ```

      1. コンテナ内で実行 
          1. dotnet runで実行する。
              ```sh
              dotnet test ./src/DIContainerTest/DIContainerTest.csproj
              ```

          1. コンテナから離脱する。
              ```sh
              exit
              ```

      1. コンテナ停止・削除  
          ```sh
          docker-compose down
          ```
