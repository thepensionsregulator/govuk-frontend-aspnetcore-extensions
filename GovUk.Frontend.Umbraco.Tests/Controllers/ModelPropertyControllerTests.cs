using GovUk.Frontend.Umbraco.Validation;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GovUk.Frontend.Umbraco.Tests.Controllers
{
    public class ModelPropertyControllerTests
    {
        private ModelPropertyController _controller = null!;

        [SetUp]
        public void SetUp()
        {
            _controller = new ModelPropertyController();
        }

        [Test]
        public void CollectProperties_SimpleModel_ReturnsPrimitivePropertyNames()
        {
            // Arrange
            var propNames = new List<string>();
            var pathStack = new Stack<Type>();
            var expected = new[] { "Name", "Amount" };

            // Act
            _controller.CollectProperties(typeof(SimpleModel), string.Empty, 0, 5, pathStack, propNames);

            // Assert
            Assert.That(propNames.OrderBy(x => x), Is.EqualTo(expected.OrderBy(x => x)));
        }

        [Test]
        public void CollectProperties_ModelWithNestedObject_ReturnsDottedPropertyNames()
        {
            // Arrange
            var propNames = new List<string>();
            var pathStack = new Stack<Type>();
            var expected = new[] { "Name", "Address.Line1", "Address.City", "Address.Postcode" };

            // Act
            _controller.CollectProperties(typeof(ModelWithAddress), string.Empty, 0, 5, pathStack, propNames);

            // Assert
            Assert.That(propNames.OrderBy(x => x), Is.EqualTo(expected.OrderBy(x => x)));
        }

        [Test]
        public void CollectProperties_ModelWithArray_ReturnsCollectionPropertyName()
        {
            // Arrange
            var propNames = new List<string>();
            var pathStack = new Stack<Type>();
            var expected = new[] { "Name", "Items" };

            // Act
            _controller.CollectProperties(typeof(ModelWithArray), string.Empty, 0, 5, pathStack, propNames);

            // Assert
            Assert.That(propNames.OrderBy(x => x), Is.EqualTo(expected.OrderBy(x => x)));
        }

        [Test]
        public void CollectProperties_ModelWithGenericList_RecursesIntoElementType()
        {
            // Arrange
            var propNames = new List<string>();
            var pathStack = new Stack<Type>();
            var expected = new[] { "Name", "Tags.Value" };

            // Act
            _controller.CollectProperties(typeof(ModelWithGenericList), string.Empty, 0, 5, pathStack, propNames);

            // Assert
            Assert.That(propNames.OrderBy(x => x), Is.EqualTo(expected.OrderBy(x => x)));
        }

        [Test]
        public void CollectProperties_ModelWithInterface_ExpandsInterfaceProperties()
        {
            // Arrange
            var propNames = new List<string>();
            var pathStack = new Stack<Type>();
            var expected = new[] { "Name", "Metadata.Key", "Metadata.Value" };

            // Act
            _controller.CollectProperties(typeof(ModelWithInterface), string.Empty, 0, 5, pathStack, propNames);

            // Assert
            Assert.That(propNames.OrderBy(x => x), Is.EqualTo(expected.OrderBy(x => x)));
        }

        [Test]
        public void CollectProperties_ModelWithCircularReference_StopsRecursion()
        {
            // Arrange
            var propNames = new List<string>();
            var pathStack = new Stack<Type>();
            // CircularModelA references CircularModelB, which references back to CircularModelA
            // Should not throw StackOverflowException

            // Act
            _controller.CollectProperties(typeof(CircularModelA), string.Empty, 0, 5, pathStack, propNames);

            // Assert
            Assert.That(propNames, Is.Not.Empty);
            // Verify we have some properties but no infinite loop (sanity check)
            Assert.That(propNames.Count, Is.LessThan(10));
        }

        [Test]
        public void CollectProperties_RespectMaxDepthLimit()
        {
            // Arrange
            var propNames = new List<string>();
            var pathStack = new Stack<Type>();
            // DeeplyNestedModelL1 has 6 levels: L1 -> L2 -> L3 -> L4 -> L5 -> L6
            // With maxDepth=3, we should only see L1.L2.L3 properties

            // Act
            _controller.CollectProperties(typeof(DeeplyNestedModelL1), string.Empty, 0, 3, pathStack, propNames);

            // Assert
            // Check that we have at most 3 levels of nesting
            var maxLevels = propNames.Any() ? propNames.Max(r => r.Split('.').Length) : 0;
            Assert.That(maxLevels, Is.LessThanOrEqualTo(3));
        }

        [Test]
        public void CollectProperties_NullType_ReturnsWithoutAddingProperties()
        {
            // Arrange
            var propNames = new List<string>();
            var pathStack = new Stack<Type>();

            // Act
            _controller.CollectProperties(null!, string.Empty, 0, 5, pathStack, propNames);

            // Assert
            Assert.That(propNames, Is.Empty);
        }

        [Test]
        public void CollectProperties_TypeWithNoProperties_ReturnsEmpty()
        {
            // Arrange
            var propNames = new List<string>();
            var pathStack = new Stack<Type>();

            // Act
            _controller.CollectProperties(typeof(EmptyModel), string.Empty, 0, 5, pathStack, propNames);

            // Assert
            Assert.That(propNames, Is.Empty);
        }

        [Test]
        public void CollectProperties_IncludesDecimal_AsTerminalType()
        {
            // Arrange
            var propNames = new List<string>();
            var pathStack = new Stack<Type>();
            var expected = new[] { "Price" };

            // Act
            _controller.CollectProperties(typeof(ModelWithDecimal), string.Empty, 0, 5, pathStack, propNames);

            // Assert
            Assert.That(propNames, Is.EqualTo(expected));
        }

        [Test]
        public void CollectProperties_IncludesEnum_AsTerminalType()
        {
            // Arrange
            var propNames = new List<string>();
            var pathStack = new Stack<Type>();
            var expected = new[] { "Status" };

            // Act
            _controller.CollectProperties(typeof(ModelWithEnum), string.Empty, 0, 5, pathStack, propNames);

            // Assert
            Assert.That(propNames, Is.EqualTo(expected));
        }

        [Test]
        public void CollectProperties_ExcludesPrivateProperties()
        {
            // Arrange
            var propNames = new List<string>();
            var pathStack = new Stack<Type>();
            var expected = new[] { "PublicName" };

            // Act
            _controller.CollectProperties(typeof(ModelWithPrivateProperty), string.Empty, 0, 5, pathStack, propNames);

            // Assert
            Assert.That(propNames, Is.EqualTo(expected));
        }

        [Test]
        public void CollectProperties_ReachMaxDepth_StopsRecursion()
        {
            // Arrange
            var propNames = new List<string>();
            var pathStack = new Stack<Type>();

            // Act - call with depth=5 (equal to maxDepth), should return immediately
            _controller.CollectProperties(typeof(SimpleModel), string.Empty, 5, 5, pathStack, propNames);

            // Assert
            Assert.That(propNames, Is.Empty);
        }

        [Test]
        public void CollectProperties_WithPrefix_BuildsDottedNames()
        {
            // Arrange
            var propNames = new List<string>();
            var pathStack = new Stack<Type>();
            var expected = new[] { "Parent.Address.Line1", "Parent.Address.City", "Parent.Address.Postcode" };

            // Act
            _controller.CollectProperties(typeof(Address), "Parent.Address", 1, 5, pathStack, propNames);

            // Assert
            Assert.That(propNames.OrderBy(x => x), Is.EqualTo(expected.OrderBy(x => x)));
        }

        [Test]
        public void CollectProperties_PathStackContainsType_SkipsRecursion()
        {
            // Arrange
            var propNames = new List<string>();
            var pathStack = new Stack<Type>();
            pathStack.Push(typeof(CircularModelA));

            // Act - try to collect from CircularModelA again (already in stack)
            _controller.CollectProperties(typeof(CircularModelA), string.Empty, 0, 5, pathStack, propNames);

            // Assert - should return immediately without adding properties
            Assert.That(propNames, Is.Empty);
        }

        [Test]
        public void CollectProperties_IntegerPrimitive_IsAddedAsTerminal()
        {
            // Arrange
            var propNames = new List<string>();
            var pathStack = new Stack<Type>();

            // Act
            _controller.CollectProperties(typeof(ModelWithInt), string.Empty, 0, 5, pathStack, propNames);

            // Assert
            Assert.That(propNames, Contains.Item("Count"));
        }

        [Test]
        public void CollectProperties_BoolPrimitive_IsAddedAsTerminal()
        {
            // Arrange
            var propNames = new List<string>();
            var pathStack = new Stack<Type>();

            // Act
            _controller.CollectProperties(typeof(ModelWithBool), string.Empty, 0, 5, pathStack, propNames);

            // Assert
            Assert.That(propNames, Contains.Item("IsActive"));
        }

        [Test]
        public void CollectProperties_MultipleNestedLevels_ExploresAllPaths()
        {
            // Arrange
            var propNames = new List<string>();
            var pathStack = new Stack<Type>();

            // Act
            _controller.CollectProperties(typeof(ComplexModel), string.Empty, 0, 5, pathStack, propNames);

            // Assert
            Assert.That(propNames, Does.Contain("Name"));
            Assert.That(propNames, Does.Contain("Details.Title"));
            Assert.That(propNames, Does.Contain("Details.Items.Description"));
        }

        [Test]
        public void GetEnumerableElementType_StringArray_ReturnsString()
        {
            // Act
            var result = _controller.GetEnumerableElementType(typeof(string[]));

            // Assert
            Assert.That(result, Is.EqualTo(typeof(string)));
        }

        [Test]
        public void GetEnumerableElementType_IntArray_ReturnsInt()
        {
            // Act
            var result = _controller.GetEnumerableElementType(typeof(int[]));

            // Assert
            Assert.That(result, Is.EqualTo(typeof(int)));
        }

        [Test]
        public void GetEnumerableElementType_CustomClassArray_ReturnsCustomClass()
        {
            // Act
            var result = _controller.GetEnumerableElementType(typeof(CustomClass[]));

            // Assert
            Assert.That(result, Is.EqualTo(typeof(CustomClass)));
        }

        [Test]
        public void GetEnumerableElementType_ListOfString_ReturnsString()
        {
            // Act
            var result = _controller.GetEnumerableElementType(typeof(List<string>));

            // Assert
            Assert.That(result, Is.EqualTo(typeof(string)));
        }

        [Test]
        public void GetEnumerableElementType_ListOfInt_ReturnsInt()
        {
            // Act
            var result = _controller.GetEnumerableElementType(typeof(List<int>));

            // Assert
            Assert.That(result, Is.EqualTo(typeof(int)));
        }

        [Test]
        public void GetEnumerableElementType_ListOfCustomClass_ReturnsCustomClass()
        {
            // Act
            var result = _controller.GetEnumerableElementType(typeof(List<CustomClass>));

            // Assert
            Assert.That(result, Is.EqualTo(typeof(CustomClass)));
        }

        [Test]
        public void GetEnumerableElementType_IEnumerableOfString_ReturnsString()
        {
            // Act
            var result = _controller.GetEnumerableElementType(typeof(IEnumerable<string>));

            // Assert
            Assert.That(result, Is.EqualTo(typeof(string)));
        }

        [Test]
        public void GetEnumerableElementType_IEnumerableOfInt_ReturnsInt()
        {
            // Act
            var result = _controller.GetEnumerableElementType(typeof(IEnumerable<int>));

            // Assert
            Assert.That(result, Is.EqualTo(typeof(int)));
        }

        [Test]
        public void GetEnumerableElementType_IListOfString_ReturnsString()
        {
            // Act
            var result = _controller.GetEnumerableElementType(typeof(IList<string>));

            // Assert
            Assert.That(result, Is.EqualTo(typeof(string)));
        }

        [Test]
        public void GetEnumerableElementType_ICollectionOfCustomClass_ReturnsCustomClass()
        {
            // Act
            var result = _controller.GetEnumerableElementType(typeof(ICollection<CustomClass>));

            // Assert
            Assert.That(result, Is.EqualTo(typeof(CustomClass)));
        }

        [Test]
        public void GetEnumerableElementType_HashSetOfString_ReturnsString()
        {
            // Act
            var result = _controller.GetEnumerableElementType(typeof(HashSet<string>));

            // Assert
            Assert.That(result, Is.EqualTo(typeof(string)));
        }

        [Test]
        public void GetEnumerableElementType_NonGenericList_ReturnsNull()
        {
            // Arrange - Non-generic ArrayList has no generic arguments
            // Act
            var result = _controller.GetEnumerableElementType(typeof(ArrayList));

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetEnumerableElementType_CustomCollectionOfString_ReturnsString()
        {
            // Act
            var result = _controller.GetEnumerableElementType(typeof(CustomCollection<string>));

            // Assert
            Assert.That(result, Is.EqualTo(typeof(string)));
        }

        [Test]
        public void GetEnumerableElementType_NestedGenericList_ReturnsInnerType()
        {
            // Arrange - List<List<string>> should return List<string>
            // Act
            var result = _controller.GetEnumerableElementType(typeof(List<List<string>>));

            // Assert
            Assert.That(result, Is.EqualTo(typeof(List<string>)));
        }

        [Test]
        public void GetEnumerableElementType_NullableInt_ReturnsNullableInt()
        {
            // Arrange - IEnumerable<int?> would have Nullable<int> as element type
            // Act
            var result = _controller.GetEnumerableElementType(typeof(List<int?>));

            // Assert
            Assert.That(result, Is.EqualTo(typeof(int?)));
        }

        [Test]
        public void GetEnumerableElementType_CustomInterfaceImplementingIEnumerable_ReturnsElementType()
        {
            // Act
            var result = _controller.GetEnumerableElementType(typeof(CustomEnumerable<CustomClass>));

            // Assert
            Assert.That(result, Is.EqualTo(typeof(CustomClass)));
        }

        [Test]
        public void GetEnumerableElementType_IEnumerableOfNullableDecimal_ReturnsNullableDecimal()
        {
            // Act
            var result = _controller.GetEnumerableElementType(typeof(IEnumerable<decimal?>));

            // Assert
            Assert.That(result, Is.EqualTo(typeof(decimal?)));
        }

        [Test]
        public void GetEnumerableElementType_EnumArray_ReturnsEnum()
        {
            // Act
            var result = _controller.GetEnumerableElementType(typeof(TestEnum[]));

            // Assert
            Assert.That(result, Is.EqualTo(typeof(TestEnum)));
        }

        [Test]
        public void GetEnumerableElementType_ListOfEnum_ReturnsEnum()
        {
            // Act
            var result = _controller.GetEnumerableElementType(typeof(List<TestEnum>));

            // Assert
            Assert.That(result, Is.EqualTo(typeof(TestEnum)));
        }

        [Test]
        public void GetEnumerableElementType_QueueOfString_ReturnsString()
        {
            // Act
            var result = _controller.GetEnumerableElementType(typeof(Queue<string>));

            // Assert
            Assert.That(result, Is.EqualTo(typeof(string)));
        }

        [Test]
        public void GetEnumerableElementType_StackOfInt_ReturnsInt()
        {
            // Act
            var result = _controller.GetEnumerableElementType(typeof(Stack<int>));

            // Assert
            Assert.That(result, Is.EqualTo(typeof(int)));
        }

        [Test]
        public void GetEnumerableElementType_LinkedListOfCustomClass_ReturnsCustomClass()
        {
            // Act
            var result = _controller.GetEnumerableElementType(typeof(LinkedList<CustomClass>));

            // Assert
            Assert.That(result, Is.EqualTo(typeof(CustomClass)));
        }

        [Test]
        public void CollectProperties_ModelWithSimpleTuple_ExpandsTupleElementNames()
        {
            // Arrange
            var propNames = new List<string>();
            var pathStack = new Stack<Type>();
            var expected = new[] { "Id", "Pair.Item1", "Pair.Item2" };

            // Act
            _controller.CollectProperties(typeof(ModelWithSimpleTuple), string.Empty, 0, 5, pathStack, propNames);

            // Assert
            Assert.That(propNames.OrderBy(x => x), Is.EqualTo(expected.OrderBy(x => x)));
        }

        [Test]
        public void CollectProperties_ModelWithNamedTuple_ExpandsNamedTupleElements()
        {
            // Arrange
            var propNames = new List<string>();
            var pathStack = new Stack<Type>();
            var expected = new[] { "FirstName", "LastName", "Person.First", "Person.Last" };

            // Act
            _controller.CollectProperties(typeof(ModelWithNamedTuple), string.Empty, 0, 5, pathStack, propNames);

            // Assert
            Assert.That(propNames.OrderBy(x => x), Is.EqualTo(expected.OrderBy(x => x)));
        }

        [Test]
        public void CollectProperties_ModelWithTupleOfPrimitives_ExpandsTupleElements()
        {
            // Arrange
            var propNames = new List<string>();
            var pathStack = new Stack<Type>();
            var expected = new[] { "Coordinates.Item1", "Coordinates.Item2", "Coordinates.Item3" };

            // Act
            _controller.CollectProperties(typeof(ModelWithTupleOfPrimitives), string.Empty, 0, 5, pathStack, propNames);

            // Assert
            Assert.That(propNames.OrderBy(x => x), Is.EqualTo(expected.OrderBy(x => x)));
        }

        [Test]
        public void CollectProperties_ModelWithNestedTuple_ExpandsAllLevels()
        {
            // Arrange
            var propNames = new List<string>();
            var pathStack = new Stack<Type>();
            var expected = new[] { "Name", "Nested.Item1", "Nested.Item2" };

            // Act
            _controller.CollectProperties(typeof(ModelWithNestedTuple), string.Empty, 0, 5, pathStack, propNames);

            // Assert
            Assert.That(propNames.OrderBy(x => x), Is.EqualTo(expected.OrderBy(x => x)));
        }

        [Test]
        public void CollectProperties_ModelWithTupleList_ExpandsTupleElements()
        {
            // Arrange
            var propNames = new List<string>();
            var pathStack = new Stack<Type>();
            var expected = new[] { "Title", "Pairs.Item1", "Pairs.Item2" };

            // Act
            _controller.CollectProperties(typeof(ModelWithTupleList), string.Empty, 0, 5, pathStack, propNames);

            // Assert
            Assert.That(propNames.OrderBy(x => x), Is.EqualTo(expected.OrderBy(x => x)));
        }

        [Test]
        public void GetEnumerableElementType_TupleArray_ReturnsTupleType()
        {
            // Arrange
            var tupleType = typeof((string, int)[]);

            // Act
            var result = _controller.GetEnumerableElementType(tupleType);

            // Assert
            Assert.That(result, Is.EqualTo(typeof((string, int))));
        }

        [Test]
        public void GetEnumerableElementType_ListOfTuple_ReturnsTupleType()
        {
            // Arrange
            var tupleType = typeof(List<(string, int)>);

            // Act
            var result = _controller.GetEnumerableElementType(tupleType);

            // Assert
            Assert.That(result, Is.EqualTo(typeof((string, int))));
        }

        [Test]
        public void IsTupleType_ValueTupleOfTwoElements_ReturnsTrue()
        {
            // Act
            var result = _controller.IsTupleType(typeof((string, int)));

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsTupleType_ValueTupleOfThreeElements_ReturnsTrue()
        {
            // Act
            var result = _controller.IsTupleType(typeof((string, int, bool)));

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsTupleType_ValueTupleOfOneElement_ReturnsTrue()
        {
            // Act
            var result = _controller.IsTupleType(typeof(ValueTuple<string>));

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsTupleType_NonTupleType_ReturnsFalse()
        {
            // Act
            var result = _controller.IsTupleType(typeof(SimpleModel));

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void GetTupleElementNames_ValueTupleWithoutNames_ReturnsDefaultItemNames()
        {
            // Arrange
            var tupleType = typeof((string, int));

            // Act
            var result = _controller.GetTupleElementNames(tupleType).ToList();

            // Assert
            Assert.That(result, Is.EqualTo(new[] { "Item1", "Item2" }));
        }

        [Test]
        public void GetTupleElementNames_ValueTupleWithNames_ReturnsActualNames()
        {
            // Arrange
            var modelType = typeof(ModelWithNamedTuple);
            var property = modelType.GetProperty("Person");

            // Act
            var result = _controller.GetTupleElementNames(property!.PropertyType, property).ToList();

            // Assert
            Assert.That(result, Does.Contain("First").And.Contain("Last"));
        }

        [Test]
        public void GetTupleElementNames_ValueTupleOfThreeElements_ReturnsThreeNames()
        {
            // Arrange
            var tupleType = typeof((string, int, bool));

            // Act
            var result = _controller.GetTupleElementNames(tupleType).ToList();

            // Assert
            Assert.That(result, Has.Count.EqualTo(3));
        }

        [Test]
        public void CollectProperties_NullableInt_IsAddedAsTerminal()
        {
            // Arrange
            var propNames = new List<string>();
            var pathStack = new Stack<Type>();

            // Act
            _controller.CollectProperties(typeof(ModelWithNullableInt), string.Empty, 0, 5, pathStack, propNames);

            // Assert
            Assert.That(propNames, Contains.Item("Count"));
        }

        [Test]
        public void CollectProperties_NullableDecimal_IsAddedAsTerminal()
        {
            // Arrange
            var propNames = new List<string>();
            var pathStack = new Stack<Type>();

            // Act
            _controller.CollectProperties(typeof(ModelWithNullableDecimal), string.Empty, 0, 5, pathStack, propNames);

            // Assert
            Assert.That(propNames, Contains.Item("Price"));
        }

        [Test]
        public void CollectProperties_DateTime_IsAddedAsTerminal()
        {
            // Arrange
            var propNames = new List<string>();
            var pathStack = new Stack<Type>();

            // Act
            _controller.CollectProperties(typeof(ModelWithDateTime), string.Empty, 0, 5, pathStack, propNames);

            // Assert
            Assert.That(propNames, Contains.Item("CreatedOn"));
        }

        [Test]
        public void CollectProperties_NullableDateTime_IsAddedAsTerminal()
        {
            // Arrange
            var propNames = new List<string>();
            var pathStack = new Stack<Type>();

            // Act
            _controller.CollectProperties(typeof(ModelWithNullableDateTime), string.Empty, 0, 5, pathStack, propNames);

            // Assert
            Assert.That(propNames, Contains.Item("ModifiedOn"));
        }

        [Test]
        public void CollectProperties_DateOnly_IsAddedAsTerminal()
        {
            // Arrange
            var propNames = new List<string>();
            var pathStack = new Stack<Type>();

            // Act
            _controller.CollectProperties(typeof(ModelWithDateOnly), string.Empty, 0, 5, pathStack, propNames);

            // Assert
            Assert.That(propNames, Contains.Item("BirthDate"));
        }

        [Test]
        public void CollectProperties_NullableDateOnly_IsAddedAsTerminal()
        {
            // Arrange
            var propNames = new List<string>();
            var pathStack = new Stack<Type>();

            // Act
            _controller.CollectProperties(typeof(ModelWithNullableDateOnly), string.Empty, 0, 5, pathStack, propNames);

            // Assert
            Assert.That(propNames, Contains.Item("AnniversaryDate"));
        }

        [Test]
        public void CollectProperties_DateTimeOffset_IsAddedAsTerminal()
        {
            // Arrange
            var propNames = new List<string>();
            var pathStack = new Stack<Type>();

            // Act
            _controller.CollectProperties(typeof(ModelWithDateTimeOffset), string.Empty, 0, 5, pathStack, propNames);

            // Assert
            Assert.That(propNames, Contains.Item("PublishedAt"));
        }

        [Test]
        public void CollectProperties_NullableDateTimeOffset_IsAddedAsTerminal()
        {
            // Arrange
            var propNames = new List<string>();
            var pathStack = new Stack<Type>();

            // Act
            _controller.CollectProperties(typeof(ModelWithNullableDateTimeOffset), string.Empty, 0, 5, pathStack, propNames);

            // Assert
            Assert.That(propNames, Contains.Item("ScheduledAt"));
        }

        [Test]
        public void CollectProperties_Guid_IsAddedAsTerminal()
        {
            // Arrange
            var propNames = new List<string>();
            var pathStack = new Stack<Type>();

            // Act
            _controller.CollectProperties(typeof(ModelWithGuid), string.Empty, 0, 5, pathStack, propNames);

            // Assert
            Assert.That(propNames, Contains.Item("Id"));
        }

        [Test]
        public void CollectProperties_NullableGuid_IsAddedAsTerminal()
        {
            // Arrange
            var propNames = new List<string>();
            var pathStack = new Stack<Type>();

            // Act
            _controller.CollectProperties(typeof(ModelWithNullableGuid), string.Empty, 0, 5, pathStack, propNames);

            // Assert
            Assert.That(propNames, Contains.Item("CorrelationId"));
        }

        [Test]
        public void CollectProperties_ModelWithMultipleTerminalTypes_ReturnsAll()
        {
            // Arrange
            var propNames = new List<string>();
            var pathStack = new Stack<Type>();
            var expected = new[] { "IntValue", "DecimalValue", "DateTimeValue", "DateOnlyValue", "GuidValue" };

            // Act
            _controller.CollectProperties(typeof(ModelWithMultipleTerminalTypes), string.Empty, 0, 5, pathStack, propNames);

            // Assert
            Assert.That(propNames.OrderBy(x => x), Is.EqualTo(expected.OrderBy(x => x)));
        }

        [Test]
        public void CollectProperties_ArrayOfTuples_ExpandsTupleElements()
        {
            // Arrange
            var propNames = new List<string>();
            var pathStack = new Stack<Type>();
            var expected = new[] { "Title", "Items.Item1", "Items.Item2" };

            // Act
            _controller.CollectProperties(typeof(ModelWithTupleArray), string.Empty, 0, 5, pathStack, propNames);

            // Assert
            Assert.That(propNames.OrderBy(x => x), Is.EqualTo(expected.OrderBy(x => x)));
        }

        [Test]
        public void CollectProperties_NullableTuple_ExpandsTupleElements()
        {
            // Arrange
            var propNames = new List<string>();
            var pathStack = new Stack<Type>();
            var expected = new[] { "Name", "Location.Item1", "Location.Item2" };

            // Act
            _controller.CollectProperties(typeof(ModelWithNullableTuple), string.Empty, 0, 5, pathStack, propNames);

            // Assert
            Assert.That(propNames.OrderBy(x => x), Is.EqualTo(expected.OrderBy(x => x)));
        }

        [Test]
        public void IsNullableTerminalType_NullableInt_ReturnsTrue()
        {
            // Act
            var result = _controller.IsNullableTerminalType(typeof(int?));

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsNullableTerminalType_NullableByte_ReturnsTrue()
        {
            // Act
            var result = _controller.IsNullableTerminalType(typeof(byte?));

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsNullableTerminalType_NullableShort_ReturnsTrue()
        {
            // Act
            var result = _controller.IsNullableTerminalType(typeof(short?));

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsNullableTerminalType_NullableLong_ReturnsTrue()
        {
            // Act
            var result = _controller.IsNullableTerminalType(typeof(long?));

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsNullableTerminalType_NullableFloat_ReturnsTrue()
        {
            // Act
            var result = _controller.IsNullableTerminalType(typeof(float?));

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsNullableTerminalType_NullableDouble_ReturnsTrue()
        {
            // Act
            var result = _controller.IsNullableTerminalType(typeof(double?));

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsNullableTerminalType_NullableDecimal_ReturnsTrue()
        {
            // Act
            var result = _controller.IsNullableTerminalType(typeof(decimal?));

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsNullableTerminalType_NullableDateTime_ReturnsTrue()
        {
            // Act
            var result = _controller.IsNullableTerminalType(typeof(DateTime?));

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsNullableTerminalType_NullableDateOnly_ReturnsTrue()
        {
            // Act
            var result = _controller.IsNullableTerminalType(typeof(DateOnly?));

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsNullableTerminalType_NullableDateTimeOffset_ReturnsTrue()
        {
            // Act
            var result = _controller.IsNullableTerminalType(typeof(DateTimeOffset?));

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsNullableTerminalType_NullableGuid_ReturnsTrue()
        {
            // Act
            var result = _controller.IsNullableTerminalType(typeof(Guid?));

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsNullableTerminalType_NullableBool_ReturnsTrue()
        {
            // Act
            var result = _controller.IsNullableTerminalType(typeof(bool?));

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsNullableTerminalType_NonGenericType_ReturnsFalse()
        {
            // Act
            var result = _controller.IsNullableTerminalType(typeof(string));

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsNullableTerminalType_NonNullableInt_ReturnsFalse()
        {
            // Act
            var result = _controller.IsNullableTerminalType(typeof(int));

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsNullableTerminalType_NullableClass_ReturnsFalse()
        {
            // Act
            var result = _controller.IsNullableTerminalType(typeof(SimpleModel));

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsNullableTerminalType_NullableTuple_ReturnsFalse()
        {
            // Act
            var result = _controller.IsNullableTerminalType(typeof((string, int)?));

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsNullableTerminalType_List_ReturnsFalse()
        {
            // Act
            var result = _controller.IsNullableTerminalType(typeof(List<int>));

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsNullableTerminalType_Array_ReturnsFalse()
        {
            // Act
            var result = _controller.IsNullableTerminalType(typeof(int[]));

            // Assert
            Assert.That(result, Is.False);
        }
    }

    // ============ Test Helper Classes and Interfaces ============

    public class CustomClass
    {
        public string? Name { get; set; }
    }

    public enum TestEnum
    {
        Value1,
        Value2,
        Value3
    }

    public class CustomCollection<T> : ICollection<T>
    {
        private readonly List<T> _items = new();

        public int Count => _items.Count;
        public bool IsReadOnly => false;

        public void Add(T item) => _items.Add(item);
        public void Clear() => _items.Clear();
        public bool Contains(T item) => _items.Contains(item);
        public void CopyTo(T[] array, int arrayIndex) => _items.CopyTo(array, arrayIndex);
        public bool Remove(T item) => _items.Remove(item);
        public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();
    }

    public class CustomEnumerable<T> : IEnumerable<T>
    {
        private readonly List<T> _items = new();

        public void Add(T item) => _items.Add(item);
        public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();
    }

    // ============ Test Models ============

    public class SimpleModel
    {
        public string? Name { get; set; }
        public decimal Amount { get; set; }
    }

    public class ModelWithAddress
    {
        public string? Name { get; set; }
        public Address? Address { get; set; }
    }

    public class Address
    {
        public string? Line1 { get; set; }
        public string? City { get; set; }
        public string? Postcode { get; set; }
    }

    public class ModelWithArray
    {
        public string? Name { get; set; }
        public string[]? Items { get; set; }
    }

    public class ModelWithGenericList
    {
        public string? Name { get; set; }
        public List<Tag>? Tags { get; set; }
    }

    public class Tag
    {
        public string? Value { get; set; }
    }

    public class ModelWithInterface
    {
        public string? Name { get; set; }
        public IMetadata? Metadata { get; set; }
    }

    public interface IMetadata
    {
        string? Key { get; set; }
        string? Value { get; set; }
    }

    public class CircularModelA
    {
        public string? Name { get; set; }
        public CircularModelB? RefB { get; set; }
    }

    public class CircularModelB
    {
        public string? Title { get; set; }
        public CircularModelA? RefA { get; set; }
    }

    public class DeeplyNestedModelL1
    {
        public DeeplyNestedModelL2? L2 { get; set; }
    }

    public class DeeplyNestedModelL2
    {
        public DeeplyNestedModelL3? L3 { get; set; }
    }

    public class DeeplyNestedModelL3
    {
        public DeeplyNestedModelL4? L4 { get; set; }
    }

    public class DeeplyNestedModelL4
    {
        public DeeplyNestedModelL5? L5 { get; set; }
    }

    public class DeeplyNestedModelL5
    {
        public DeeplyNestedModelL6? L6 { get; set; }
    }

    public class DeeplyNestedModelL6
    {
        public string? DeepValue { get; set; }
    }

    public class EmptyModel
    {
    }

    public class ModelWithDecimal
    {
        public decimal Price { get; set; }
    }

    public class ModelWithEnum
    {
        public StatusEnum Status { get; set; }
    }

    public enum StatusEnum
    {
        Active,
        Inactive
    }

    public class ModelWithPrivateProperty
    {
        public string? PublicName { get; set; }

#pragma warning disable IDE0051 // Remove unused private members
        private string? PrivateName { get; set; }
#pragma warning restore IDE0051
    }

    public class ModelWithInt
    {
        public int Count { get; set; }
    }

    public class ModelWithBool
    {
        public bool IsActive { get; set; }
    }

    public class ComplexModel
    {
        public string? Name { get; set; }
        public ModelDetails? Details { get; set; }
    }

    public class ModelDetails
    {
        public string? Title { get; set; }
        public List<ItemModel>? Items { get; set; }
    }

    public class ItemModel
    {
        public string? Description { get; set; }
    }

    public class ModelWithSimpleTuple
    {
        public int Id { get; set; }
        public (string, int) Pair { get; set; }
    }

    public class ModelWithNamedTuple
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public (string First, string Last) Person { get; set; }
    }

    public class ModelWithTupleOfPrimitives
    {
        public (double, double, double) Coordinates { get; set; }
    }

    public class ModelWithNestedTuple
    {
        public string? Name { get; set; }
        public (int, string) Nested { get; set; }
    }

    public class ModelWithTupleList
    {
        public string? Title { get; set; }
        public List<(string, int)>? Pairs { get; set; }
    }

    public class ModelWithNullableInt
    {
        public int? Count { get; set; }
    }

    public class ModelWithNullableDecimal
    {
        public decimal? Price { get; set; }
    }

    public class ModelWithDateTime
    {
        public DateTime CreatedOn { get; set; }
    }

    public class ModelWithNullableDateTime
    {
        public DateTime? ModifiedOn { get; set; }
    }

    public class ModelWithDateOnly
    {
        public DateOnly BirthDate { get; set; }
    }

    public class ModelWithNullableDateOnly
    {
        public DateOnly? AnniversaryDate { get; set; }
    }

    public class ModelWithDateTimeOffset
    {
        public DateTimeOffset PublishedAt { get; set; }
    }

    public class ModelWithNullableDateTimeOffset
    {
        public DateTimeOffset? ScheduledAt { get; set; }
    }

    public class ModelWithGuid
    {
        public Guid Id { get; set; }
    }

    public class ModelWithNullableGuid
    {
        public Guid? CorrelationId { get; set; }
    }

    public class ModelWithMultipleTerminalTypes
    {
        public int IntValue { get; set; }
        public decimal DecimalValue { get; set; }
        public DateTime DateTimeValue { get; set; }
        public DateOnly DateOnlyValue { get; set; }
        public Guid GuidValue { get; set; }
    }

    public class ModelWithTupleArray
    {
        public string? Title { get; set; }
        public (string, int)[]? Items { get; set; }
    }

    public class ModelWithNullableTuple
    {
        public string? Name { get; set; }
        public (double, double)? Location { get; set; }
    }
}