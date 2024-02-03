using Appplication.Commands;
using Appplication.Models;
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
    private IJsonRepository? _jsonRepository = null;

    /// <summary>
    /// ファイル出力リポジトリクラスインスタンス
    /// </summary>
    private IFileOutputRepository? _fileOutputRepository = null;

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
        // リポジトリ設定チェック
        if(_jsonRepository is null) throw new NullReferenceException($"{nameof(_jsonRepository)} is null");
        if(_fileOutputRepository is null) throw new NullReferenceException($"{nameof(_fileOutputRepository)} is null");

        // パラメータチェック
        if (string.IsNullOrEmpty(target)) throw new ArgumentException($"{nameof(target)} is null or Empty");
        if (command is null) throw new ArgumentException($"{nameof(command)} is null");
        if (_jsonRepository.IsJsonString(target) && string.IsNullOrEmpty(command?.RootClassName)) throw new ArgumentException($"{nameof(command.RootClassName)} is null");

        // Json取得
        var classesEnties = _jsonRepository.CreateClassEntity(target, command.RootClassName);

        // ファイル出力
        var CommandParams = new Dictionary<ParamKeys, string>
        {
            {ParamKeys.CS_NameSpace, command.NameSpace},
            {ParamKeys.Prefix, command.Prefix},
            {ParamKeys.Suffix, command.Suffix},
        };
        var fileCommand = new FileOutputCommand(command.RootPath, OutputLanguageType.CS, command.IndentSpaceCount, CommandParams);
        var results = _fileOutputRepository.OutputResults(classesEnties, fileCommand);

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
