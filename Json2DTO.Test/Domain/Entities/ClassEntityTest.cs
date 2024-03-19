using Domain.Entities;
using Domain.ValueObjects;

namespace Json2DTO.Test.Domain;

/// <summary>
/// クラスエンティティのテスト
/// </summary>
public class ClassEntityTest
{
    [Fact(DisplayName="ExceptionTest:className is null"), Trait("Category", "Domain:Entity:ClassEntityTest")]
    public void ExceptionClassNameNull()
    {
        string? className = null;
        #pragma warning disable
        var ex = Assert.ThrowsAny<ArgumentException>(() => ClassEntity.Create(className));
        Assert.Equal("className is null", ex.Message);
    }

    [Fact(DisplayName="ExceptionTest:className is Empty"), Trait("Category", "Domain:Entity:ClassEntityTest")]
    public void ExceptionClassNameEmpty()
    {
        string className = string.Empty;
        var ex = Assert.ThrowsAny<ArgumentException>(() => ClassEntity.Create(className));
        Assert.Equal("className is null", ex.Message);
    }

    [Fact(DisplayName="ExceptionTest:property is null"), Trait("Category", "Domain:Entity:ClassEntityTest")]
    public void ExceptionPropertiesNull()
    {
        string className = "ClassName";
        var classEntity = ClassEntity.Create(className);
        #pragma warning disable
        var ex = Assert.ThrowsAny<ArgumentException>(() => classEntity.AddProperty(null));
        Assert.Equal("property is null", ex.Message);
    }

    [Fact(DisplayName="Test:property is none"), Trait("Category", "Domain:Entity:ClassEntityTest")]
    public void SuccessPropertiesNone()
    {
        string className = "ClassName";
        var classEntity = ClassEntity.Create(className);
        Assert.Equal("ClassName", classEntity.Name);
        Assert.Equal(0, classEntity.Properties.Count);
    }

    [Fact(DisplayName="Test:property is exist"), Trait("Category", "Domain:Entity:ClassEntityTest")]
    public void SuccessPropertiesExist()
    {
        string className = "ClassName";
        var classEntity = ClassEntity.Create(className);
        var proprtyType = new PropertyType(typeof(string), false);
        var propertyValueObject = new PropertyValueObject("Name", proprtyType);

        classEntity.AddProperty(propertyValueObject);

        Assert.Equal("ClassName", classEntity.Name);
        Assert.Equal(1, classEntity.Properties.Count);
        Assert.Equal("Name", classEntity.Properties[0].Name);
        Assert.Equal(string.Empty, classEntity.Properties[0].PropertyTypeClassName);
        Assert.Equal(PropertyType.Kinds.String, classEntity.Properties[0].Type?.Kind);
        Assert.Equal(false, classEntity.Properties[0].Type?.IsList);
    }
}
