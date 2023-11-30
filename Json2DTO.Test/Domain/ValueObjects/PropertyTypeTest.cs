using Domain.ValueObjects;

namespace Json2DTO.Test.Domain.ValueObjects;

/// <summary>
/// プロパティ型ValueObjectのテスト
/// </summary>
public class PropertyTypeTest
{
    /// <summary>
    /// Setup
    /// </summary>
    public PropertyTypeTest()
    {
    }

    /// <summary>
    /// Teardown
    /// </summary>
    public void Dispose()
    {
    }

    [Fact]
    public void ExceptionTypeObject()
    {
        var isList = false;
        var type = typeof(Object);
        var ex = Assert.ThrowsAny<ArgumentException>(() => new PropertyType(type, isList));
        Assert.Equal(ex.Message, $"{nameof(type)}({type.Name}) is null");

        isList = true;
        ex = Assert.ThrowsAny<ArgumentException>(() => new PropertyType(type, isList));
        Assert.Equal(ex.Message, $"{nameof(type)}({type.Name}) is null");
    }

    [Fact]
    public void ExceptionClassNoMinus()
    {
        var isList = false;
        var classNo = -1;
        var ex = Assert.ThrowsAny<ArgumentException>(() => new PropertyType(classNo, isList));
        Assert.Equal(ex.Message, $"{nameof(classNo)} is negative value");

        isList = true;
        ex = Assert.ThrowsAny<ArgumentException>(() => new PropertyType(classNo, isList));
        Assert.Equal(ex.Message, $"{nameof(classNo)} is negative value");
    }

    [Fact]
    public void SuccessType()
    {
        var isList = false;
        Type type = typeof(string);
        PropertyType propertyType = new PropertyType(type, isList);
        Assert.Equal(propertyType.Kind, PropertyType.Kinds.String);
        Assert.Equal(propertyType.ClassName, string.Empty);
        Assert.Equal(propertyType.IsList, isList);

        type = typeof(decimal);
        propertyType = new PropertyType(type, isList);
        Assert.Equal(propertyType.Kind, PropertyType.Kinds.Decimal);
        Assert.Equal(propertyType.ClassName, string.Empty);
        Assert.Equal(propertyType.IsList, isList);

        type = typeof(bool);
        propertyType = new PropertyType(type, isList);
        Assert.Equal(propertyType.Kind, PropertyType.Kinds.Bool);
        Assert.Equal(propertyType.ClassName, string.Empty);
        Assert.Equal(propertyType.IsList, isList);

        type = typeof(Nullable);
        propertyType = new PropertyType(type, isList);
        Assert.Equal(propertyType.Kind, PropertyType.Kinds.Null);
        Assert.Equal(propertyType.ClassName, string.Empty);
        Assert.Equal(propertyType.IsList, isList);

        isList = true;
        type = typeof(string);
        propertyType = new PropertyType(type, isList);
        Assert.Equal(propertyType.Kind, PropertyType.Kinds.String);
        Assert.Equal(propertyType.ClassName, string.Empty);
        Assert.Equal(propertyType.IsList, isList);

        type = typeof(decimal);
        propertyType = new PropertyType(type, isList);
        Assert.Equal(propertyType.Kind, PropertyType.Kinds.Decimal);
        Assert.Equal(propertyType.ClassName, string.Empty);
        Assert.Equal(propertyType.IsList, isList);

        type = typeof(bool);
        propertyType = new PropertyType(type, isList);
        Assert.Equal(propertyType.Kind, PropertyType.Kinds.Bool);
        Assert.Equal(propertyType.ClassName, string.Empty);
        Assert.Equal(propertyType.IsList, isList);

        type = typeof(Nullable);
        propertyType = new PropertyType(type, isList);
        Assert.Equal(propertyType.Kind, PropertyType.Kinds.Null);
        Assert.Equal(propertyType.ClassName, string.Empty);
        Assert.Equal(propertyType.IsList, isList);
    }

    [Fact]
    public void SuccessClass()
    {
        var isList = false;
        var classNo = 0;
        PropertyType propertyType = new PropertyType(classNo, isList);
        Assert.Equal(propertyType.Kind, PropertyType.Kinds.Class);
        Assert.Equal(propertyType.ClassName, "InnerClass");
        Assert.Equal(propertyType.IsList, isList);

        classNo = 1;
        propertyType = new PropertyType(classNo, isList);
        Assert.Equal(propertyType.Kind, PropertyType.Kinds.Class);
        Assert.Equal(propertyType.ClassName, "InnerClass");
        Assert.Equal(propertyType.IsList, isList);

        classNo = 2;
        propertyType = new PropertyType(classNo, isList);
        Assert.Equal(propertyType.Kind, PropertyType.Kinds.Class);
        Assert.Equal(propertyType.ClassName, "InnerClassA");
        Assert.Equal(propertyType.IsList, isList);

        classNo = 3;
        propertyType = new PropertyType(classNo, isList);
        Assert.Equal(propertyType.Kind, PropertyType.Kinds.Class);
        Assert.Equal(propertyType.ClassName, "InnerClassB");
        Assert.Equal(propertyType.IsList, isList);

        isList = true;
        classNo = 0;
        propertyType = new PropertyType(classNo, isList);
        Assert.Equal(propertyType.Kind, PropertyType.Kinds.Class);
        Assert.Equal(propertyType.ClassName, "InnerClass");
        Assert.Equal(propertyType.IsList, isList);

        classNo = 1;
        propertyType = new PropertyType(classNo, isList);
        Assert.Equal(propertyType.Kind, PropertyType.Kinds.Class);
        Assert.Equal(propertyType.ClassName, "InnerClass");
        Assert.Equal(propertyType.IsList, isList);

        classNo = 2;
        propertyType = new PropertyType(classNo, isList);
        Assert.Equal(propertyType.Kind, PropertyType.Kinds.Class);
        Assert.Equal(propertyType.ClassName, "InnerClassA");
        Assert.Equal(propertyType.IsList, isList);

        classNo = 3;
        propertyType = new PropertyType(classNo, isList);
        Assert.Equal(propertyType.Kind, PropertyType.Kinds.Class);
        Assert.Equal(propertyType.ClassName, "InnerClassB");
        Assert.Equal(propertyType.IsList, isList);
    }
}
