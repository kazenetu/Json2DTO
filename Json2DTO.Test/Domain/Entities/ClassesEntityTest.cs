using Domain.Entities;
using Domain.ValueObjects;

namespace Json2DTO.Test.Domain;

/// <summary>
/// クラス集約エンティティのテスト
/// </summary>
public class ClassesEntityTest
{
    [Fact(DisplayName="ExceptionTest:className is null"), Trait("Category", "Domain:Entity:ClassesEntityTest")]
    public void ExceptionClassNameNull()
    {
        string? rootClassName = null;
        #pragma warning disable
        var ex = Assert.ThrowsAny<ArgumentException>(() => ClassesEntity.Create(rootClassName));
        Assert.Equal("rootClassName is null", ex.Message);
    }

    [Fact(DisplayName="ExceptionTest:className is Empty"), Trait("Category", "Domain:Entity:ClassesEntityTest")]
    public void ExceptionClassNameEmpty()
    {
        string rootClassName = string.Empty;
        var ex = Assert.ThrowsAny<ArgumentException>(() => ClassesEntity.Create(rootClassName));
        Assert.Equal("className is null", ex.Message);
    }

    [Fact(DisplayName="ExceptionTest:AddRootProperty is null"), Trait("Category", "Domain:Entity:ClassesEntityTest")]
    public void ExceptionAddRootPropertyNull()
    {
        string rootClassName = "TestRootClass";
        var classesEntity = ClassesEntity.Create(rootClassName);
        #pragma warning disable
        var ex = Assert.ThrowsAny<ArgumentException>(() => classesEntity.AddRootProperty(null));
        Assert.Equal("property is null", ex.Message);
    }

    [Fact(DisplayName="ExceptionTest:AddInnerClass is null"), Trait("Category", "Domain:Entity:ClassesEntityTest")]
    public void ExceptionAddInnerClassNull()
    {
        string rootClassName = "TestRootClass";
        var classesEntity = ClassesEntity.Create(rootClassName);
        #pragma warning disable
        var ex = Assert.ThrowsAny<ArgumentException>(() => classesEntity.AddInnerClass(null));
        Assert.Equal("innerClass is null", ex.Message);
    }

    [Fact(DisplayName="Test:RootClass"), Trait("Category", "Domain:Entity:ClassesEntityTest")]
    public void SuccessRootClassName()
    {
        string rootClassName = "TestRootClass";
        var classesEntity = ClassesEntity.Create(rootClassName);
        Assert.Equal("TestRootClass", classesEntity.Name);
    }

    [Fact(DisplayName="Test:InnerClass is none"), Trait("Category", "Domain:Entity:ClassesEntityTest")]
    public void SuccessInnerClassesNone()
    {
        string rootClassName = "TestRootClass";
        var classesEntity = ClassesEntity.Create(rootClassName);
        Assert.Equal(0, classesEntity.InnerClasses.Count);
    }

    [Fact(DisplayName="Test:InnerClass exists"), Trait("Category", "Domain:Entity:ClassesEntityTest")]
    public void SuccessInnerClassesExist()
    {
        string rootClassName = "TestRootClass";
        var classesEntity = ClassesEntity.Create(rootClassName);
        classesEntity.AddInnerClass(ClassEntity.Create("ClassName"));

        Assert.Equal(1, classesEntity.InnerClasses.Count);
        Assert.Equal("ClassName", classesEntity.InnerClasses[0].Name);
    }

    [Fact(DisplayName="Test:AddRootProperty"), Trait("Category", "Domain:Entity:ClassesEntityTest")]
    public void SuccessAddRootProperty()
    {
        string rootClassName = "TestRootClass";
        var classesEntity = ClassesEntity.Create(rootClassName);
        var proprtyType = new PropertyType(typeof(string), false);
        var propertyValueObject = new PropertyValueObject("Name", proprtyType);
        classesEntity.AddRootProperty(propertyValueObject);

        var rootClass = classesEntity.RootClass;
        Assert.Equal(1, rootClass.Properties.Count);
        Assert.Equal("Name", rootClass.Properties[0].Name);
        Assert.Equal(string.Empty, rootClass.Properties[0].PropertyTypeClassName);
        Assert.Equal(PropertyType.Kinds.String, rootClass.Properties[0].Type?.Kind);
        Assert.Equal(false, rootClass.Properties[0].Type?.IsList);
    }
}