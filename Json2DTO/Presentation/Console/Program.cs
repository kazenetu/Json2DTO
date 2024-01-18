using Appplication;
using Domain.Interfaces;
using Infrastructure;
using TinyDIContainer;

namespace Presentation.Console;

internal class Program
{
    /// <summary>
    /// エントリメソッド
    /// </summary>    
    private static void Main(string[] args)
    {
        // 入力チェック
        if(args.Length < 2)
        {
            System.Console.WriteLine();
            System.Console.Write("Input parameters!");
            System.Console.Write("\"");
            System.Console.Write("OutputPath ");
            System.Console.Write("\"DirectoryPath/FilePath/JsonString\"");
            System.Console.Write("[NameSpace]");
            System.Console.Write("\"");
            System.Console.WriteLine();
            return;
        }

        // DI設定
        DIContainer.Add<IFileOutputRepository, FileOutputRepository>();
        DIContainer.Add<IJsonRepository, JsonRepository>();

        // ファイル出力設定値
        var rootPath = args[0];
        var target = args[1];
        var nameSpace = string.Empty;
        if(args.Length > 2){
            nameSpace = args[2];
        }

        // HACK 実行処理
        System.Console.WriteLine(rootPath);
        System.Console.WriteLine(target);
        System.Console.WriteLine(nameSpace);
    }
}