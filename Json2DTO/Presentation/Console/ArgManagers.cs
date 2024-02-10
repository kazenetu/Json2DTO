namespace Presentation.Console;

/// <summary>
/// パラメータ管理クラス
/// </summary>
public class ArgManagers
{
    /// <summary>
    /// オプションパラメータ(名前あり)のコレクション
    /// </summary>
    private Dictionary<string, string?> _optionPramArgs = new Dictionary<string, string?>();

    /// <summary>
    /// 必須パラメーター(名前なし)のコレクション
    /// </summary>
    private List<string> _requiredPramArgs = new List<string>();

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="args">パラメータ</param>
    /// <remarks>パラメータのリストを作成する</remarks>
    public ArgManagers(string[] args)
    {
        var paramName = string.Empty;
        foreach (var arg in args)
        {
            switch (arg[0])
            {
                case '-':
                case '/':
                    if (!string.IsNullOrEmpty(paramName))
                    {
                        // パラメータ名だけの場合はパラメータ名だけ追加
                        _optionPramArgs.Add(paramName, null);
                    }

                    // パラメータ名を設定
                    paramName = arg.Substring(0, 1).Replace("/", "-", System.StringComparison.CurrentCulture) +
                        arg.Substring(1);
                    break;

                default:
                    // パラメータの追加
                    if (string.IsNullOrEmpty(paramName))
                    {
                        // パラメータ名なし
                        _requiredPramArgs.Add(arg);
                    }
                    else
                    {
                        // パラメータ名あり
                        _optionPramArgs.Add(paramName, arg);
                    }

                    // パラメータ名をクリア
                    paramName = string.Empty;
                    break;
            }
        }

        // パラメータ名だけの場合はパラメータ名だけ追加
        if (!string.IsNullOrEmpty(paramName))
        {
            _optionPramArgs.Add(paramName, null);
        }
    }

    /// <summary>
    /// 必須パラメータの取得
    /// </summary>
    /// <param name="index">取得パラメータのインデックス</param>
    /// <returns>対象パラメータの値(インデックスが存在しない場合はnull)</returns>
    public string? GetRequiredArg(int index)
    {
        if (_requiredPramArgs.Count <= index)
        {
            return null;
        }

        return _requiredPramArgs[index];
    }

    /// <summary>
    /// 必須パラメータ数の取得
    /// </summary>
    /// <returns>必須パラメータ数</returns>
    public int GetRequiredArgCount()
    {
        return _requiredPramArgs.Count;
    }

    /// <summary>
    /// オプションパラメータの取得
    /// </summary>
    /// <param name="paramName">パラメータ名</param>
    /// <returns>対象パラメータの値(パラメータ名が存在しない場合はnull)</returns>
    public string? GetOptionArg(string paramName)
    {
        if (!_optionPramArgs.ContainsKey(paramName))
        {
            return null;
        }

        return _optionPramArgs[paramName];
    }

    /// <summary>
    /// オプションパラメータの取得
    /// </summary>
    /// <param name="paramName">パラメータ名リスト</param>
    /// <returns>対象パラメータの値(パラメータ名が存在しない場合はnull)</returns>
    public string? GetOptionArg(List<string> paramNames)
    {
        string? result = null;
        foreach (var pramName in paramNames)
        {
            result = GetOptionArg(pramName);
            if (result != null)
            {
                break;
            }
        }
        return result;
    }

    /// <summary>
    /// オプションパラメータ名の存在確認
    /// </summary>
    /// <param name="paramName">パラメータ名</param>
    /// <returns>確認結果</returns>
    public bool ExistsOptionArg(string paramName)
    {
        return _optionPramArgs.ContainsKey(paramName);
    }

    /// <summary>
    /// オプションパラメータ名の存在確認
    /// </summary>
    /// <param name="paramName">パラメータ名リスト</param>
    /// <returns>確認結果</returns>
    public bool ExistsOptionArg(List<string> paramNames)
    {
        var result = false;
        foreach (var pramName in paramNames)
        {
            result = ExistsOptionArg(pramName);
            if (result)
            {
                break;
            }
        }
        return result;
    }
}
