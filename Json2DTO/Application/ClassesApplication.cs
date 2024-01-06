using Appplication.Commands;
using Appplication.Models;
using Domain.Entities;
using Domain.Commands;
using Domain.Interfaces;

namespace Appplication;

/// <summary>
/// クラスエンティティ集約アプリケーション
/// </summary>
public class ClassesApplication : ApplicationBase
{
    /// <summary>
    /// Json解析リポジトリクラスインスタンス
    /// </summary>
    private IJsonRepository? JsonRepository = null;

    /// <summary>
    /// ファイル出力リポジトリクラスインスタンス
    /// </summary>
    private IFileOutputRepository? FileOutputRepository = null;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <remarks>DI機能でRepositoryインスタンスを取得</remarks>
    public ClassesApplication() : base()
    {
    }

    /// <summary>
    /// C#ソースコードに変換しファイル作成する
    /// </summary>
    /// <param name="target">対象文字列(Json文字列/ファイルパス/ディレクトリパス)</param>
    /// <param name="command">C#変換コマンドクラスインスタンスリスト</param>
    /// <returns>処理結果</returns>
    public IReadOnlyList<ConvertResultModel> ConvertJsonToCSharp(string target, CSharpCommand command)
    {
        // パラメータチェック
        if (string.IsNullOrEmpty(target)) throw new ArgumentException($"{nameof(target)} is null or Empty");
        if (command is null) throw new ArgumentException($"{nameof(command)} is null");
        if (string.IsNullOrEmpty(command?.RootClassName)) throw new ArgumentException($"{nameof(command.RootClassName)} is null");

        // リポジトリ設定チェック
        if(JsonRepository is null) throw new NullReferenceException($"{nameof(JsonRepository)} is null");
        if(FileOutputRepository is null) throw new NullReferenceException($"{nameof(FileOutputRepository)} is null");

        var classesEnties = new List<ClassesEntity>();

        // 判定処理
        if(JsonRepository.IsJsonString(target)){
            classesEnties.Add(JsonRepository.CreateClassEntityFromString(target, command.RootClassName));
        }
        else if(JsonRepository.IsFilePath(target))
        {
            classesEnties.Add(JsonRepository.CreateClassEntityFromFile(target));
        }
        else
        {
            classesEnties.AddRange(JsonRepository.CreateClassEntityFromFiles(target));
        }

        // ファイル出力
        var CommandParams = new Dictionary<ParamKeys, string>
        {
            {ParamKeys.CS_NameSpace, command.NameSpace},
            {ParamKeys.Prefix, command.Prefix},
            {ParamKeys.Suffix, command.Suffix},
        };
        var fileCommand = new FileOutputCommand(command.RootPath, OutputLanguageType.CS, command.IndentSpaceCount, CommandParams);
        var results = FileOutputRepository.OutputResults(classesEnties, fileCommand);

        var resultModels = new List<ConvertResultModel>();
        foreach (var result in results)
        {
            if (result.Success)
            {
                // 変換成功
                resultModels.Add(new ConvertResultModel(true, result.FileName, result.SourceCode));
            }
            else
            {
                // 変換失敗
                resultModels.Add(new ConvertResultModel(false, result.FileName));
            }
        }
        return resultModels;
    }
}
