using TinyDIContainer;

namespace Appplication;

/// <summary>
/// アプリケーションスーパークラス
/// </summary>
public abstract class ApplicationBase
{
    protected ApplicationBase()
    {
        // フィールドを検索
        var filelds = this.GetType().GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        for (int i = 0; i < filelds.Length; i++)
        {
            // インターフェイス以外は処理対象外
            if (!filelds[i].FieldType.IsInterface) continue;

            // インターフェイスのインスタンスを作成
            var instance = DIContainer.CreateInstance(filelds[i].FieldType);

            // フィールドにインターフェイスインスタンスを設定
            filelds[i].SetValue(this, instance);
        }
    }
}