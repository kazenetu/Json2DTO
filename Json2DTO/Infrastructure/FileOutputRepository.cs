using Domain.Commands;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Results;
using Infrastructure.Extensions;

namespace Infrastructure;

/// <summary>
/// ファイル出力リポジトリクラス
/// </summary>
public class FileOutputRepository : IFileOutputRepository
{
    /// <summary>
    /// 複数ファイルを出力する
    /// </summary>
    /// <param name="classInstances">複数集約エンティティ</param>
    /// <param name="command">コマンドパラメータ</param>
    /// <returns>出力結果</returns>
    public IReadOnlyList<FileOutputResult> OutputResults(IReadOnlyList<ClassesEntity> classInstances, FileOutputCommand command)
    {
        var result = new List<FileOutputResult>();

        //必須パラメータチェック
        var isSuccess = true;
        if (classInstances is null) isSuccess = false;
        if (classInstances?.Count <= 0) isSuccess = false;
        if(!isSuccess)
        {
            result.Add(new FileOutputResult(false, string.Empty, string.Empty));
            return result;
        }

        // ファイル出力
        foreach(var classInstance in classInstances)
        {
            result.Add(OutputResult(classInstance, command));
        }

        return result;
    }
 

    /// <summary>
    /// ファイル出力する
    /// </summary>
    /// <param name="classInstance">集約エンティティ インスタンス</param>
    /// <param name="command">コマンドパラメータ</param>
    /// <returns>出力結果</returns>
    public FileOutputResult OutputResult(ClassesEntity classInstance, FileOutputCommand command)
    {
        //必須パラメータチェック
        if (classInstance is null) return new FileOutputResult(false, string.Empty, string.Empty);
        if (command is null) return new FileOutputResult(false, string.Empty, string.Empty);
        if (command.RootPath is null) return new FileOutputResult(false, string.Empty, string.Empty);
        if (command.Params is null) return new FileOutputResult(false, string.Empty, string.Empty);

        // フォルダの存在確認とフォルダ作成
        if (!Directory.Exists(command.RootPath))
        {
            Directory.CreateDirectory(command.RootPath);
        }

        // 拡張子取得
        var ext = command.LanguageType switch
        {
            OutputLanguageType.CS => "cs",
            _ => throw new Exception("ext error")
        };

        // 固定プレフィックス
        var prefix = string.Empty;
        if (command.Params.ContainsKey(ParamKeys.Prefix))
        {
            var target = command.Params[ParamKeys.Prefix];
            prefix = command.LanguageType switch
            {
                OutputLanguageType.CS => target.ToCSharpNaming(),
                _ => throw new Exception("ext error")
            };
        }

        // 固定サフィックス
        var suffix = string.Empty;
        if (command.Params.ContainsKey(ParamKeys.Suffix))
        {
            var target = command.Params[ParamKeys.Suffix];
            suffix = command.LanguageType switch
            {
                OutputLanguageType.CS => target.ToCSharpNaming(),
                _ => throw new Exception("ext error")
            };
        }

        // ファイルパス作成
        var filePath = Path.Combine(command.RootPath, $"{prefix}{classInstance.Name.ToCSharpNaming()}{suffix}.{ext}");

        // ソースコードを作成
        var sourceCode = command.LanguageType switch
        {
            OutputLanguageType.CS => GetCSCode(classInstance, command),
            _ => throw new Exception("ext error")
        };

        // ファイル出力
        File.WriteAllText(filePath, sourceCode);

        return new FileOutputResult(true, filePath, sourceCode);
    }

    /// <summary>
    /// C# ソースコード生成
    /// </summary>
    /// <param name="classInstance">集約エンティティ インスタンス</param>
    /// <param name="command">コマンドパラメータ</param>
    /// <returns>ソースコード</returns>
    private static string GetCSCode(ClassesEntity classInstance, FileOutputCommand command)
    {
        // 名前空間
        var nameSpace = string.Empty;
        if (command.Params.ContainsKey(ParamKeys.CS_NameSpace))
        {
            nameSpace = command.Params[ParamKeys.CS_NameSpace];
        }

        // 固定プレフィックス
        var prefix = string.Empty;
        if (command.Params.ContainsKey(ParamKeys.Prefix))
        {
            prefix = command.Params[ParamKeys.Prefix].ToCSharpNaming();
        }

        // 固定サフィックス
        var suffix = string.Empty;
        if (command.Params.ContainsKey(ParamKeys.Suffix))
        {
            suffix = command.Params[ParamKeys.Suffix].ToCSharpNaming();
        }

        var initialSpaceIndex = 0;
        // 名前空間が設定していない場合はインデントを調整する
        if (nameSpace == string.Empty)
        {
            initialSpaceIndex = 1;
        }

        // Entityからソースコードの変換
        return Utils.SoruceConverter.ToCsCode(classInstance, initialSpaceIndex, nameSpace, command.IndentSpaceCount, prefix, suffix);
    }
}