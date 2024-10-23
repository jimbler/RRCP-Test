using Microsoft.VisualStudio.TestTools.UnitTesting;
using RRCP;

namespace Test
{
    [TestClass]
    public class PropertyTests
    {
        [TestMethod]
        public void TestPropertyParsing()
        {
            // Arrange
            //"(id, name, email, type(id, name, customFields(c1, c2, c3)), externalId)"
            var testInput1 = "(id, externalId)";
            var testInput2 = "(id, externalId, name(id, type))";
            var testInput3 = "(id, externalId, name(category(id, type)), text)";

            // Act
            var props1 = Property.CreatePropertiesFromString(testInput1);
            var props2 = Property.CreatePropertiesFromString(testInput2);
            var props3 = Property.CreatePropertiesFromString(testInput3);

            // Assert
            //Top-level props
            Assert.AreEqual(2, props1.Count);
            Assert.AreEqual(3, props2.Count);
            Assert.AreEqual(4, props3.Count);

            //Nested props
            Assert.AreEqual(0, props1[0].Properties.Count);
            Assert.AreEqual(0, props1[1].Properties.Count);
            Assert.AreEqual(2, props2[2].Properties.Count);
            Assert.AreEqual(1, props3[2].Properties.Count);
            Assert.AreEqual(2, props3[2].Properties[0].Properties.Count);
        }

        [TestMethod]
        public void TestPropertySorting()
        {
            // Arrange
            //"(id, name, email, type(id, name, customFields(c1, c2, c3)), externalId)"
            var testInput1 = "(id, externalId, angle)";
            var testInput2 = "(id, externalId, name(id, type, ordinal))";

            // Act
            var props1 = Property.CreatePropertiesFromString(testInput1);
            var props2 = Property.CreatePropertiesFromString(testInput2);
            var sortedProps1 = Property.SortProps(props1);
            var sortedProps2 = Property.SortProps(props2);

            // Assert
            //a,e,i
            //e,i,n
            Assert.IsTrue(sortedProps1[0].Text.StartsWith('a'));
            Assert.IsTrue(sortedProps1[1].Text.StartsWith('e'));
            Assert.IsTrue(sortedProps1[2].Text.StartsWith('i'));
            Assert.IsTrue(sortedProps2[0].Text.StartsWith('e'));
            Assert.IsTrue(sortedProps2[1].Text.StartsWith('i'));
            Assert.IsTrue(sortedProps2[2].Text.StartsWith('n'));
            Assert.IsTrue(sortedProps2[2].Properties[0].Text.StartsWith('i'));
            Assert.IsTrue(sortedProps2[2].Properties[1].Text.StartsWith('o'));
            Assert.IsTrue(sortedProps2[2].Properties[2].Text.StartsWith('t'));
        }

        [TestMethod]
        public void TestPropertyStringFormatting()
        {
            // Arrange
            //"(id, name, email, type(id, name, customFields(c1, c2, c3)), externalId)"
            var testInput1 = "(id, externalId)";
            var testInput2 = "(id, externalId, name(id, type))";
            var testInput3 = "(id, externalId, name(category(id, type)), text)";

            // Act
            var props1 = Property.CreatePropertiesFromString(testInput1);
            var props2 = Property.CreatePropertiesFromString(testInput2);
            var props3 = Property.CreatePropertiesFromString(testInput3);
            var stringProps1 = Property.FormatProps(props1);
            var stringProps2 = Property.FormatProps(props2);
            var stringProps3 = Property.FormatProps(props3);

            //- id
            //- externalId

            //- id
            //- externalId
            //- name
            // - id
            // - type

            //- id
            //- externalId
            //- name
            // - category
            //  - id
            //  - type
            //- text

            // Assert
            Assert.AreEqual("- id\n- externalId\n", stringProps1);
            Assert.AreEqual("- id\n- externalId\n- name\n - id\n - type\n", stringProps2);
            Assert.AreEqual("- id\n- externalId\n- name\n - category\n  - id\n  - type\n- text\n", stringProps3);
        }
    }
}