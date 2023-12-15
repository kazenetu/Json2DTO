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
    /// ファイル出力する
    /// </summary>
    /// <param name="classInstance">集約エンティティ インスタンス</param>
    /// <param name="command">コマンドパラメータ</param>
    /// <returns>出力結果</returns>
    FileOutputResult OutputResult(ClassesEntity classInstance, FileOutputCommand command);
}
