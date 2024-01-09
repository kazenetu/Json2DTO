using Domain.Commands;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Results;
using Infrastructure;

namespace Json2DTO.Test.Application.Stub;

/// <summary>
/// ファイル出力リポジトリクラスのスタブ
/// </summary>
public class FileOutputRepositoryStub : IFileOutputRepository
{
    /// <summary>
    /// 結果の状態
    /// </summary>
    public static bool ResultSuccess{ set;get; } = true;

    /// <summary>
    /// 複数ファイルを出力する
    /// </summary>
    /// <param name="classInstances">複数集約エンティティ</param>
    /// <param name="command">コマンドパラメータ</param>
    /// <returns>出力結果</returns>
    public IReadOnlyList<FileOutputResult> OutputResults(IReadOnlyList<ClassesEntity> classInstances, FileOutputCommand command)
    {
        var result = new List<FileOutputResult>();

        string[] paths = {
            "PrefixRootClassNameSuffix",
            "PrefixRootClassNameASuffix",
        };
        string[] expectedSourceCodes ={
        @"using System.Text;
using System.Text.Json.Serialization;

namespace Test
{
  public class PrefixRootClassNameSuffix
  {
    public string Name { set; get; } = string.Empty;
  }
}",
@"using System.Text;
using System.Text.Json.Serialization;

namespace Test
{
  public class PrefixRootClassNameASuffix
  {
    [JsonPropertyName(""snake_name"")]
    public string SnakeName { set; get; } = string.Empty;
  }
}",
    };

        switch(JsonRepositoryStub.IsResult)
        {
            case JsonRepositoryStub.Mode.JsonString:
                if (ResultSuccess) result.Add(new FileOutputResult(true, paths[0], expectedSourceCodes[0]));
                else result.Add(new FileOutputResult(false, string.Empty, string.Empty));
                return result;
            case JsonRepositoryStub.Mode.FilePath:
                if (ResultSuccess) result.Add(new FileOutputResult(true, paths[1], expectedSourceCodes[1]));
                else result.Add(new FileOutputResult(false, string.Empty, string.Empty));
                return result;
        }

        // ファイル出力
        var index =0;
        foreach(var sourceCode in expectedSourceCodes)
        {
            if (ResultSuccess) {
                result.Add(new FileOutputResult(true, paths[index], sourceCode));
            }
            else{
                result.Add(new FileOutputResult(false, string.Empty, string.Empty));
            }
            index++;
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
        var expectedSourceCode = 
        @"using System.Text;
using System.Text.Json.Serialization;

namespace Test
{
  public class PrefixTestRootClassSuffix
  {
    public string Name { set; get; } = string.Empty;
  }
}";

        if (ResultSuccess) {
           return new FileOutputResult(true, "PrefixTestRootClassSuffix", expectedSourceCode);
        }
        return new FileOutputResult(false, string.Empty, string.Empty);
    }

}