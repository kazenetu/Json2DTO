using Appplication;
using Appplication.Commands;
using Appplication.Models;
using Domain.Commands;
using Domain.Interfaces;
using TinyDIContainer;
using Json2DTO.Test.Application.Stub;
using Json2DTO.Test.Application.Extensions;

namespace Json2DTO.Test.Application;

/// <summary>
/// クラスエンティティ集約アプリケーションのテスト
/// </summary>
public class ClassesApplicationTest
{
    /// <summary>
    /// セットアップ済みか否か
    /// </summary>
    static bool setuped = false;

    /// <summary>
    /// Setup
    /// </summary>
    public ClassesApplicationTest()
    {
        if(setuped) return;

        // DI設定
        DIContainer.Add<IFileOutputRepository, FileOutputRepositoryStub>();
        DIContainer.Add<IJsonRepository, JsonRepositoryStub>();

        setuped = true;
    }

    [Fact(DisplayName="ExceptionTest:JsonParam is null"), Trait("Category", "ApplicationTest")]
    public void ExceptionTargetParamNull()
    {
        string nameSpace = "Test";
        string rootPath = "testApplication";
        string rootClassName = "rootClassName";
        int indentSpaceCount = 2;
        string prefix = "prefix";
        string suffix = "suffix";
        var command = 
            new Appplication.Commands.CSharpCommand(
                nameSpace, rootPath, rootClassName, indentSpaceCount,prefix, suffix
            );

        var csApplication = new ClassesApplication();

        #pragma warning disable
        var ex = Assert.ThrowsAny<ArgumentException>(() => csApplication.ConvertJsonToCSharp(null, command));
        Assert.Equal("target is null or Empty", ex.Message);
    }

    [Fact(DisplayName="ExceptionTest:JsonParam is Empty"), Trait("Category", "ApplicationTest")]
    public void ExceptionTargetParamEmpty()
    {
        string nameSpace = "Test";
        string rootPath = "testApplication";
        string rootClassName = "rootClassName";
        int indentSpaceCount = 2;
        string prefix = "prefix";
        string suffix = "suffix";
        var command = 
            new Appplication.Commands.CSharpCommand(
                nameSpace, rootPath, rootClassName, indentSpaceCount,prefix, suffix
            );

        var csApplication = new ClassesApplication();

        #pragma warning disable
        var ex = Assert.ThrowsAny<ArgumentException>(() => csApplication.ConvertJsonToCSharp(string.Empty, command));
        Assert.Equal("target is null or Empty", ex.Message);
    }

    [Fact(DisplayName="ExceptionTest:CommandParam is null"), Trait("Category", "ApplicationTest")]
    public void ExceptionCommandParamNull()
    {
        string nameSpace = "Test";
        string rootPath = "testApplication";
        string rootClassName = "rootClassName";
        int indentSpaceCount = 2;
        string prefix = "prefix";
        string suffix = "suffix";
        var command = 
            new Appplication.Commands.CSharpCommand(
                nameSpace, rootPath, rootClassName, indentSpaceCount,prefix, suffix
            );

        var json = @"{
            ""prop_string"" : ""string""
        }";

        var csApplication = new ClassesApplication();

        #pragma warning disable
        var ex = Assert.ThrowsAny<ArgumentException>(() => csApplication.ConvertJsonToCSharp(json, null));
        Assert.Equal("command is null", ex.Message);
    }

    public void ExceptionCommandRootClassNull()
    {
        string nameSpace = "Test";
        string rootPath = null;
        string rootClassName = "TestClass";
        int indentSpaceCount = 2;
        string prefix = "prefix";
        string suffix = "suffix";
        var command = 
            new Appplication.Commands.CSharpCommand(
                nameSpace, rootPath, rootClassName, indentSpaceCount,prefix, suffix
            );

        var json = @"{
            ""prop_string"" : ""string""
        }";

        var csApplication = new ClassesApplication();

        #pragma warning disable
        var ex = Assert.ThrowsAny<ArgumentException>(() => csApplication.ConvertJsonToCSharp(json, null));
        Assert.Equal("command is null", ex.Message);
    }

    [Fact]
    public void ExceptionJsonRespositoryNull()
    {
        string nameSpace = "Test";
        string rootPath = null;
        string rootClassName = "TestClass";
        int indentSpaceCount = 2;
        string prefix = "prefix";
        string suffix = "suffix";
        var command = 
            new Appplication.Commands.CSharpCommand(
                nameSpace, rootPath, rootClassName, indentSpaceCount,prefix, suffix
            );

        var json = @"{
            ""prop_string"" : ""string""
        }";

        var csApplication = new ClassesApplication();
        csApplication.ClearJsonRepository();

        #pragma warning disable
        var ex = Assert.ThrowsAny<NullReferenceException>(() => csApplication.ConvertJsonToCSharp(json, command));
        Assert.Equal("_jsonRepository is null", ex.Message);
    }

    [Fact]
    public void ExceptionFileOutputRepositoryNull()
    {
        string nameSpace = "Test";
        string rootPath = null;
        string rootClassName = "TestClass";
        int indentSpaceCount = 2;
        string prefix = "prefix";
        string suffix = "suffix";
        var command = 
            new Appplication.Commands.CSharpCommand(
                nameSpace, rootPath, rootClassName, indentSpaceCount,prefix, suffix
            );

        var json = @"{
            ""prop_string"" : ""string""
        }";

        var csApplication = new ClassesApplication();
        csApplication.ClearFileOutputRepository();

        #pragma warning disable
        var ex = Assert.ThrowsAny<NullReferenceException>(() => csApplication.ConvertJsonToCSharp(json, command));
        Assert.Equal("_fileOutputRepository is null", ex.Message);
    }

    [Fact]
    public void SuccessJsonString()
    {
        JsonRepositoryStub.IsResult = JsonRepositoryStub.Mode.JsonString;
        FileOutputRepositoryStub.ResultSuccess = true;

        string nameSpace = "Test";
        string rootPath = "testApplication";
        string rootClassName = "rootClassName";
        int indentSpaceCount = 2;
        string prefix = "prefix";
        string suffix = "suffix";
        var command = 
            new Appplication.Commands.CSharpCommand(
                nameSpace, rootPath, rootClassName, indentSpaceCount, prefix, suffix
            );

        var json = @"{
            ""prop_string"" : ""string""
        }";

        var csApplication = new ClassesApplication();
        var results = csApplication.ConvertJsonToCSharp(json, command);

        Assert.Equal(1, results.Count);

        var result = results[0];
        Assert.Equal(true, result.Success);
        Assert.Equal("PrefixRootClassNameSuffix", result.FileName);

        var expectedSourceCode = 
        @"using System.Text;
using System.Text.Json.Serialization;

namespace Test
{
  public class PrefixRootClassNameSuffix
  {
    public string Name { set; get; } = string.Empty;
  }
}";
        Assert.Equal(expectedSourceCode, result.SourceCode);
    }

    [Fact]
    public void SuccessJsonStringResultFalse()
    {
        JsonRepositoryStub.IsResult = JsonRepositoryStub.Mode.JsonString;
        FileOutputRepositoryStub.ResultSuccess = false;

        string nameSpace = "Test";
        string rootPath = "testApplication";
        string rootClassName = "rootClassName";
        int indentSpaceCount = 2;
        string prefix = "prefix";
        string suffix = "suffix";
        var command = 
            new Appplication.Commands.CSharpCommand(
                nameSpace, rootPath, rootClassName, indentSpaceCount, prefix, suffix
            );

        var json = @"{
            ""prop_string"" : ""string""
        }";

        var csApplication = new ClassesApplication();
        var results = csApplication.ConvertJsonToCSharp(json, command);

        Assert.Equal(1, results.Count);

        var result = results[0];
        Assert.Equal(false, result.Success);
        Assert.Equal(string.Empty, result.FileName);
        Assert.Equal(string.Empty, result.SourceCode);
    }

    [Fact]
    public void SuccessFilePath()
    {
        JsonRepositoryStub.IsResult = JsonRepositoryStub.Mode.FilePath;
        FileOutputRepositoryStub.ResultSuccess = true;

        string nameSpace = "Test";
        string rootPath = "testApplication";
        string rootClassName = string.Empty;
        int indentSpaceCount = 2;
        string prefix = "prefix";
        string suffix = "suffix";
        var command = 
            new Appplication.Commands.CSharpCommand(
                nameSpace, rootPath, rootClassName, indentSpaceCount, prefix, suffix
            );

        var path = "application/PrefixRootClassNameSuffix.json";

        var csApplication = new ClassesApplication();
        var results = csApplication.ConvertJsonToCSharp(path, command);

        Assert.Equal(1, results.Count);

        var result = results[0];
        Assert.Equal(true, result.Success);
        Assert.Equal("PrefixRootClassNameASuffix", result.FileName);

        var expectedSourceCode = 
        @"using System.Text;
using System.Text.Json.Serialization;

namespace Test
{
  public class PrefixRootClassNameASuffix
  {
    [JsonPropertyName(""snake_name"")]
    public string SnakeName { set; get; } = string.Empty;
  }
}";
        Assert.Equal(expectedSourceCode, result.SourceCode);
    }

    [Fact]
    public void SuccessFilePathResultFalse()
    {
        JsonRepositoryStub.IsResult = JsonRepositoryStub.Mode.FilePath;
        FileOutputRepositoryStub.ResultSuccess = false;

        string nameSpace = "Test";
        string rootPath = "testApplication";
        string rootClassName = string.Empty;
        int indentSpaceCount = 2;
        string prefix = "prefix";
        string suffix = "suffix";
        var command = 
            new Appplication.Commands.CSharpCommand(
                nameSpace, rootPath, rootClassName, indentSpaceCount, prefix, suffix
            );

        var path = "application/PrefixRootClassNameASuffix.json";

        var csApplication = new ClassesApplication();
        var results = csApplication.ConvertJsonToCSharp(path, command);

        Assert.Equal(1, results.Count);

        var result = results[0];
        Assert.Equal(false, result.Success);
        Assert.Equal(string.Empty, result.FileName);
        Assert.Equal(string.Empty, result.SourceCode);
    }

    [Fact]
    public void SuccessIsDirectoryPath()
    {
        JsonRepositoryStub.IsResult = JsonRepositoryStub.Mode.DirectoryPath;
        FileOutputRepositoryStub.ResultSuccess = true;

        string nameSpace = "Test";
        string rootPath = "testApplication";
        string rootClassName = string.Empty;
        int indentSpaceCount = 2;
        string prefix = "prefix";
        string suffix = "suffix";
        var command = 
            new Appplication.Commands.CSharpCommand(
                nameSpace, rootPath, rootClassName, indentSpaceCount, prefix, suffix
            );

        var path = "application";

        var csApplication = new ClassesApplication();
        var results = csApplication.ConvertJsonToCSharp(path, command);

        Assert.Equal(2, results.Count);

        var result = results[0];
        Assert.Equal(true, result.Success);
        Assert.Equal("PrefixRootClassNameSuffix", result.FileName);

        var expectedSourceCode1 = 
        @"using System.Text;
using System.Text.Json.Serialization;

namespace Test
{
  public class PrefixRootClassNameSuffix
  {
    public string Name { set; get; } = string.Empty;
  }
}";
        Assert.Equal(expectedSourceCode1, result.SourceCode);

        result = results[1];
        Assert.Equal(true, result.Success);
        Assert.Equal("PrefixRootClassNameASuffix", result.FileName);

        var expectedSourceCode2 = 
        @"using System.Text;
using System.Text.Json.Serialization;

namespace Test
{
  public class PrefixRootClassNameASuffix
  {
    [JsonPropertyName(""snake_name"")]
    public string SnakeName { set; get; } = string.Empty;
  }
}";
        Assert.Equal(expectedSourceCode2, result.SourceCode);
    }

    [Fact]
    public void SuccessIsDirectoryPathResultFalse()
    {
        JsonRepositoryStub.IsResult = JsonRepositoryStub.Mode.DirectoryPath;
        FileOutputRepositoryStub.ResultSuccess = false;

        string nameSpace = "Test";
        string rootPath = "testApplication";
        string rootClassName = string.Empty;
        int indentSpaceCount = 2;
        string prefix = "prefix";
        string suffix = "suffix";
        var command = 
            new Appplication.Commands.CSharpCommand(
                nameSpace, rootPath, rootClassName, indentSpaceCount, prefix, suffix
            );

        var path = "application";

        var csApplication = new ClassesApplication();
        var results = csApplication.ConvertJsonToCSharp(path, command);

        Assert.Equal(2, results.Count);

        var result = results[0];
        Assert.Equal(false, result.Success);
        Assert.Equal(string.Empty, result.FileName);
        Assert.Equal(string.Empty, result.SourceCode);

        result = results[1];
        Assert.Equal(false, result.Success);
        Assert.Equal(string.Empty, result.FileName);
        Assert.Equal(string.Empty, result.SourceCode);
    }
}
