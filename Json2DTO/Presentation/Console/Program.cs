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
        // パラメータ取得
        var argManager = new ArgManagers(args);

        // ヘルプモードの確認
        var isShowHelp = false;
        if (argManager.GetRequiredArgCount() <= 1)
        {
            // パラメータが不正の場合はヘルプモード
            isShowHelp = true;
        }
        if (argManager.ExistsOptionArg(new List<string>() { "--help", "-h" }))
        {
            // ヘルプオプションはヘルプモード
            isShowHelp = true;
        }

        // ヘルプ画面を表示
        if (isShowHelp)
        {
            System.Console.WriteLine();
            System.Console.WriteLine("how to use: Console <OutputPath> <targetString>  [options]");
            System.Console.WriteLine("");
            System.Console.WriteLine("<OutputPath> Generated C# OutputPath");
            System.Console.WriteLine("<targetString> \"DirectoryPath\" or \"FilePath\" or \"JsonString\"");
            System.Console.WriteLine("");
            System.Console.WriteLine("options:");
            System.Console.WriteLine("-ns, --namespace <NameSpace> Input NameSpace");
            System.Console.WriteLine("-pr, --prefix    <Prefix>    Input PrefixKeyword");
            System.Console.WriteLine("-su, --suffix    <Suffix>    Input SuffixKeyword");
            System.Console.WriteLine("-rc, --rootclass <RootClass> Input RootClass JsonString (Required JsonString)");
            System.Console.WriteLine("-ic, --indentCount <IndentCount> IndentSpaceCount(ex 2 or 4)");
            System.Console.WriteLine("-h, --help  view this page");
            System.Console.WriteLine();
            return;
        }

        // DI設定
        DIContainer.Add<IFileOutputRepository, FileOutputRepository>();
        DIContainer.Add<IJsonRepository, JsonRepository>();

        // ファイル出力設定値
        var rootPath = GetRequiredArg(argManager, 0);
        var target = GetRequiredArg(argManager, 1);

        var nameSpace = GetOptionArgToString(argManager, new List<string>() { "--namespace", "-ns" });
        var prefix = GetOptionArgToString(argManager, new List<string>() { "--prefix", "-pr" });
        var suffix = GetOptionArgToString(argManager, new List<string>() { "--suffix", "-su" });
        var rootClassName = GetOptionArgToString(argManager, new List<string>() { "--rootclass", "-rc" });
        var indentSpaceCount = GetOptionArgToInt(argManager, new List<string>() { "-ic", "--indentCount" }, 4);

        // コマンド作成
        var command = new Appplication.Commands.CSharpCommand(nameSpace, rootPath, rootClassName, indentSpaceCount, prefix, suffix);

        // 実行処理
        var csApplication = new ClassesApplication();
        var results = csApplication.ConvertJsonToCSharp(target, command);

        // 出力結果をコンソール出力
        foreach(var result in results)
        {
            System.Console.Write($"{result.FileName}...");
            if (result.Success) System.Console.WriteLine($"Success");
            else System.Console.WriteLine($"Error");
        }
    }

    /// <summary>
    /// 必須パラメータの取得
    /// </summary>
    /// <param name="argManager">パラメータ管理クラスインスタンス</param>
    /// <param name="index">パラメータインデックス</param>
    /// <returns>対象パラメータの値(パラメータ名が存在しない場合はstring.Empty)</returns>
    private static string GetRequiredArg(ArgManagers argManager, int index)
    {
        var result = argManager.GetRequiredArg(index); 

        // 設定値チェック
        if (result is null) result = string.Empty;

        return result;
    }

    /// <summary>
    /// オプションパラメータの取得:String版
    /// </summary>
    /// <param name="argManager">パラメータ管理クラスインスタンス</param>
    /// <param name="paramName">パラメータ名リスト</param>
    /// <returns>対象パラメータの値(パラメータ名が存在しない場合はstring.Empty)</returns>
    private static string GetOptionArgToString(ArgManagers argManager, List<string> paramNames)
    {
        var result = argManager.GetOptionArg(paramNames); 

        // 設定値チェック
        if (result is null) result = string.Empty;

        return result;
    }

    /// <summary>
    /// オプションパラメータの取得:int版
    /// </summary>
    /// <param name="argManager">パラメータ管理クラスインスタンス</param>
    /// <param name="paramName">パラメータ名リスト</param>
    /// <param name="defaultValue">初期値</param>
    /// <returns>対象パラメータの値(パラメータ名が存在しない場合はdefaultValue)</returns>
    private static int GetOptionArgToInt(ArgManagers argManager, List<string> paramNames, int defaultValue)
    {
        int result = defaultValue;
        var optionValue = argManager.GetOptionArg(paramNames); 

        // 設定値チェック
        if (optionValue is null) return defaultValue;
        if (!int.TryParse(optionValue, out result))  return defaultValue;

        return result;
    }
}