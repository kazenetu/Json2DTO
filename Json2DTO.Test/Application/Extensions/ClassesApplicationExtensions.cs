using Appplication;
using Domain.Interfaces;

namespace Json2DTO.Test.Application.Extensions;

/// <summary>
/// ClassesApplicationテスト用拡張メソッド
/// </summary>
public static class ClassesApplicationExtensions
{
    /// <summary>
    /// JsonRepositoryをnullにする
    /// </summary>
    /// <param name="application">ClassesApplicationインスタンス</param>
    public static void ClearJsonRepository(this ClassesApplication application) 
    {
        var targetName = "_jsonRepository";

        // フィールドを検索
        var filelds = application.GetType().GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        for (int i = 0; i < filelds.Length; i++)
        {
            // インターフェイス以外は処理対象外
            if (!filelds[i].FieldType.IsInterface) continue;

            // targetName以外は処理対象外
            if(filelds[i].Name != targetName) continue;

            // インターフェイスフィールドをnull
            filelds[i].SetValue(application, null);
        }
    }

    /// <summary>
    /// FileOutputRepositoryをnullにする
    /// </summary>
    /// <param name="application">ClassesApplicationインスタンス</param>
    public static void ClearFileOutputRepository(this ClassesApplication application) 
    {
        var targetName = "_fileOutputRepository";

        // フィールドを検索
        var filelds = application.GetType().GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        for (int i = 0; i < filelds.Length; i++)
        {
            // インターフェイス以外は処理対象外
            if (!filelds[i].FieldType.IsInterface) continue;

            // targetName以外は処理対象外
            if(filelds[i].Name != targetName) continue;

            // インターフェイスフィールドをnull
            filelds[i].SetValue(application, null);
        }
    }
}