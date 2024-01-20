﻿using Appplication;
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
            System.Console.WriteLine("-n, --namespace <NameSpace> Input NameSpace");
            System.Console.WriteLine("-p, --prefix    <Prefix>    Input PrefixKeyword");
            System.Console.WriteLine("-s, --suffix    <Suffix>    Input SuffixKeyword");
            System.Console.WriteLine("-r, --rootclass <RootClass> Input RootClass JsonString is (Required JsonString)");
            System.Console.WriteLine("-h, --help  view this page");
            System.Console.WriteLine();
            return;
        }

        // DI設定
        DIContainer.Add<IFileOutputRepository, FileOutputRepository>();
        DIContainer.Add<IJsonRepository, JsonRepository>();

        // ファイル出力設定値
        var rootPath = argManager.GetRequiredArg(0);
        var target = argManager.GetRequiredArg(1);
        var nameSpace = argManager.GetOptionArg(new List<string>() { "--namespace", "-n" });
        var prefix = argManager.GetOptionArg(new List<string>() { "--prefix", "-p" });
        var suffix = argManager.GetOptionArg(new List<string>() { "--suffix", "-s" });
        var rootClassName = argManager.GetOptionArg(new List<string>() { "--rootclass", "-r" });

        // HACK 実行処理
        System.Console.WriteLine($"rootPath:{rootPath}");
        System.Console.WriteLine($"target:{target}");
        System.Console.WriteLine($"nameSpace:{nameSpace}");
        System.Console.WriteLine($"prefix:{prefix}");
        System.Console.WriteLine($"suffix:{suffix}");
        System.Console.WriteLine($"rootClassName:{rootClassName}");
    }
}