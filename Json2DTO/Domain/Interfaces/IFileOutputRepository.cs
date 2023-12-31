using Domain.Commands;
using Domain.Entities;
using Domain.Results;

namespace Domain.Interfaces;

/// <summary>
/// ファイル出力リポジトリインターフェース
/// </summary>
public interface IFileOutputRepository
{
    /// <summary>
    /// 複数ファイルを出力する
    /// </summary>
    /// <param name="classInstances">複数集約エンティティ</param>
    /// <param name="command">コマンドパラメータ</param>
    /// <returns>出力結果</returns>
    IReadOnlyList<FileOutputResult> OutputResults(IReadOnlyList<ClassesEntity> classInstances, FileOutputCommand command);

    /// <summary>
    /// ファイル出力する
    /// </summary>
    /// <param name="classInstance">集約エンティティ インスタンス</param>
    /// <param name="command">コマンドパラメータ</param>
    /// <returns>出力結果</returns>
    FileOutputResult OutputResult(ClassesEntity classInstance, FileOutputCommand command);
}
