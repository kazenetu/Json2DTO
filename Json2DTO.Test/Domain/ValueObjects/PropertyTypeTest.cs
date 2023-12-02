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
        Assert.Equal($"{nameof(type)}({type.Name}) is null", ex.Message);

        isList = true;
        ex = Assert.ThrowsAny<ArgumentException>(() => new PropertyType(type, isList));
        Assert.Equal($"{nameof(type)}({type.Name}) is null", ex.Message);
    }

    [Fact]
    public void ExceptionClassNoMinus()
    {
        var isList = false;
        var classNo = -1;
        var ex = Assert.ThrowsAny<ArgumentException>(() => new PropertyType(classNo, isList));
        Assert.Equal($"{nameof(classNo)} is negative value", ex.Message);

        isList = true;
        ex = Assert.ThrowsAny<ArgumentException>(() => new PropertyType(classNo, isList));
        Assert.Equal($"{nameof(classNo)} is negative value", ex.Message);
    }

    [Fact]
    public void SuccessType()
    {
        var isList = false;
        Type type = typeof(string);
        PropertyType propertyType = new PropertyType(type, isList);
        Assert.Equal(PropertyType.Kinds.String, propertyType.Kind);
        Assert.Equal(string.Empty, propertyType.ClassName);
        Assert.Equal(isList, propertyType.IsList);

        type = typeof(decimal);
        propertyType = new PropertyType(type, isList);
        Assert.Equal(PropertyType.Kinds.Decimal, propertyType.Kind);
        Assert.Equal(string.Empty, propertyType.ClassName);
        Assert.Equal(isList, propertyType.IsList);

        type = typeof(bool);
        propertyType = new PropertyType(type, isList);
        Assert.Equal(PropertyType.Kinds.Bool, propertyType.Kind);
        Assert.Equal(string.Empty, propertyType.ClassName);
        Assert.Equal(isList, propertyType.IsList);

        type = typeof(Nullable);
        propertyType = new PropertyType(type, isList);
        Assert.Equal(PropertyType.Kinds.Null, propertyType.Kind);
        Assert.Equal(string.Empty, propertyType.ClassName);
        Assert.Equal(isList, propertyType.IsList);

        isList = true;
        type = typeof(string);
        propertyType = new PropertyType(type, isList);
        Assert.Equal(PropertyType.Kinds.String, propertyType.Kind);
        Assert.Equal(string.Empty, propertyType.ClassName);
        Assert.Equal(isList, propertyType.IsList);

        type = typeof(decimal);
        propertyType = new PropertyType(type, isList);
        Assert.Equal(PropertyType.Kinds.Decimal, propertyType.Kind);
        Assert.Equal(string.Empty, propertyType.ClassName);
        Assert.Equal(isList, propertyType.IsList);

        type = typeof(bool);
        propertyType = new PropertyType(type, isList);
        Assert.Equal(PropertyType.Kinds.Bool, propertyType.Kind);
        Assert.Equal(string.Empty, propertyType.ClassName);
        Assert.Equal(isList, propertyType.IsList);

        type = typeof(Nullable);
        propertyType = new PropertyType(type, isList);
        Assert.Equal(PropertyType.Kinds.Null, propertyType.Kind);
        Assert.Equal(string.Empty, propertyType.ClassName);
        Assert.Equal(isList, propertyType.IsList);
    }

    [Fact]
    public void SuccessClass()
    {
        var isList = false;
        var classNo = 0;
        PropertyType propertyType = new PropertyType(classNo, isList);
        Assert.Equal(PropertyType.Kinds.Class, propertyType.Kind);
        Assert.Equal("InnerClass", propertyType.ClassName);
        Assert.Equal(isList, propertyType.IsList);

        classNo = 1;
        propertyType = new PropertyType(classNo, isList);
        Assert.Equal(PropertyType.Kinds.Class, propertyType.Kind);
        Assert.Equal("InnerClass", propertyType.ClassName);
        Assert.Equal(isList, propertyType.IsList);

        classNo = 2;
        propertyType = new PropertyType(classNo, isList);
        Assert.Equal(PropertyType.Kinds.Class, propertyType.Kind);
        Assert.Equal("InnerClassA", propertyType.ClassName);
        Assert.Equal(isList, propertyType.IsList);

        classNo = 3;
        propertyType = new PropertyType(classNo, isList);
        Assert.Equal(PropertyType.Kinds.Class, propertyType.Kind);
        Assert.Equal("InnerClassB", propertyType.ClassName);
        Assert.Equal(isList, propertyType.IsList);

        isList = true;
        classNo = 0;
        propertyType = new PropertyType(classNo, isList);
        Assert.Equal(PropertyType.Kinds.Class, propertyType.Kind);
        Assert.Equal("InnerClass", propertyType.ClassName);
        Assert.Equal(isList, propertyType.IsList);

        classNo = 1;
        propertyType = new PropertyType(classNo, isList);
        Assert.Equal(PropertyType.Kinds.Class, propertyType.Kind);
        Assert.Equal("InnerClass", propertyType.ClassName);
        Assert.Equal(isList, propertyType.IsList);

        classNo = 2;
        propertyType = new PropertyType(classNo, isList);
        Assert.Equal(PropertyType.Kinds.Class, propertyType.Kind);
        Assert.Equal("InnerClassA", propertyType.ClassName);
        Assert.Equal(isList, propertyType.IsList);

        classNo = 3;
        propertyType = new PropertyType(classNo, isList);
        Assert.Equal(PropertyType.Kinds.Class, propertyType.Kind);
        Assert.Equal("InnerClassB", propertyType.ClassName);
        Assert.Equal(isList, propertyType.IsList);
    }
}
