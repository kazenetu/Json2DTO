namespace Appplication.Commands;

/// <summary>
/// 共通レコードクラス
/// </summary>
/// <param name="Prefix">固定プレフィックス</param>
/// <param name="Suffix">固定サフィックス</param>
public abstract record CommonCommand(string Prefix, string Suffix);
