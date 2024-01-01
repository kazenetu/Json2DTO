using Domain.Entities;

namespace Infrastructure.Utils;

/// <summary>
/// ソース変換インターフェース
/// </summary>
public interface IConverter
{
    /// <summary>
    /// インスタンス生成
    /// </summary>
    /// <param name="classInstance">集約クラス</param>
    /// <param name="param">パラメータ</param>
    /// <returns>インスタンス<returns>
    static IConverter Create(ClassesEntity classInstance, Dictionary<string, string> param)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// コード変更処理
    /// </summary>
    /// <returns>ソースコード文字列</returns>
    string Convert();
}