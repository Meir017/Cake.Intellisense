using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using System.Collections.Generic;
using Cake.Core.Annotations;
using Cake.Core;
using FluentAssertions.Execution;
using System.Diagnostics;

namespace Cake.IntellisenseGenerator.Core.Tests
{
    [TestClass]
    public class UtilitiesTests
    {
        [TestMethod]
        public void IsCakeAliasMethod_NonStaticMethod_ShouldBeFalse()
        {
            IsCakeAliasMethodAssertHelper(
                type: typeof(TestData),
                methodName: nameof(TestData.NonStaticMethod),
                isAlias: false);
        }

        [TestMethod]
        public void IsCakeAliasMethod_StaticMethodWithoutAttributes_ShouldBeFalse()
        {
            IsCakeAliasMethodAssertHelper(
                type: typeof(StaticTestData),
                methodName: nameof(StaticTestData.StaticMethodWithoutAttributes),
                isAlias: false);
        }

        [TestMethod]
        public void IsCakeAliasMethod_StaticMethodWithMethodAttributeWithoutParameters_ShouldBeFalse()
        {
            IsCakeAliasMethodAssertHelper(
                type: typeof(StaticTestData),
                methodName: nameof(StaticTestData.StaticMethodWithMethodAttributeWithoutParameters),
                isAlias: false);
        }

        [TestMethod]
        public void IsCakeAliasMethod_StaticMethodWithPropertyAttributeWithoutParameters_ShouldBeFalse()
        {
            IsCakeAliasMethodAssertHelper(
                type: typeof(StaticTestData),
                methodName: nameof(StaticTestData.StaticMethodWithPropertyAttributeWithoutParameters),
                isAlias: false);
        }

        [TestMethod]
        public void IsCakeAliasMethod_StaticMethodWithMethodAttributeWithContextParameter_ShouldBeFalse()
        {
            IsCakeAliasMethodAssertHelper(
                type: typeof(StaticTestData),
                methodName: nameof(StaticTestData.StaticMethodWithMethodAttributeWithContextParameter),
                isAlias: false);
        }

        [TestMethod]
        public void IsCakeAliasMethod_StaticMethodWithPropertyAttributeWitContextParameter_ShouldBeFalse()
        {
            IsCakeAliasMethodAssertHelper(
                type: typeof(StaticTestData),
                methodName: nameof(StaticTestData.StaticMethodWithPropertyAttributeWitContextParameter),
                isAlias: false);
        }

        [TestMethod]
        public void IsCakeAliasMethod_StaticMethodWithoutAttributeWithThisContextParameter_ShouldBeFalse()
        {
            IsCakeAliasMethodAssertHelper(
                type: typeof(StaticTestData),
                methodName: nameof(StaticTestData.StaticMethodWithoutAttributeWithThisContextParameter),
                isAlias: false);
        }

        [TestMethod]
        public void IsCakeAliasMethod_StaticMethodWithMethodAttributeWithThisContextParameter_ShouldBeTrue()
        {
            IsCakeAliasMethodAssertHelper(
                type: typeof(StaticTestData),
                methodName: nameof(StaticTestData.StaticMethodWithMethodAttributeWithThisContextParameter),
                isAlias: true);
        }

        [TestMethod]
        public void IsCakeAliasMethod_StaticMethodWithPropertyAttributeWitThisContextParameter_ShouldBeTrue()
        {
            IsCakeAliasMethodAssertHelper(
                type: typeof(StaticTestData),
                methodName: nameof(StaticTestData.StaticMethodWithPropertyAttributeWitThisContextParameter),
                isAlias: true);
        }

        [TestMethod]
        public void IsCakeAliasMethod_StaticMethodWithMethodAttributeWithThisContextParameterAndMoreParameters_ShouldBeTrue()
        {
            IsCakeAliasMethodAssertHelper(
                type: typeof(StaticTestData),
                methodName: nameof(StaticTestData.StaticMethodWithMethodAttributeWithThisContextParameterAndMoreParameters),
                isAlias: true);
        }

        [TestMethod]
        public void GetCakeAliases_ShouldReturnCorrectMethods()
        {
            // arrange
            var type = typeof(StaticTestData).GetTypeInfo();

            // act
            var aliases = Utilities.GetCakeAliases(type);

            // assert
            aliases.Select(alias => alias.Name)
                .ShouldAllBeEquivalentTo(new[]
                {
                    nameof(StaticTestData.StaticMethodWithMethodAttributeWithThisContextParameterAndMoreParameters),
                    nameof(StaticTestData.StaticMethodWithPropertyAttributeWitThisContextParameter),
                    nameof(StaticTestData.StaticMethodWithMethodAttributeWithThisContextParameter)
                });
        }

        [TestMethod]
        public void GetParameterRepresentation_ForSimpleType()
        {
            // arrange
            var parameters = typeof(GetParameterRepresentationData)
                .GetMethod(nameof(GetParameterRepresentationData.MethodWithPrimitiveType))
                .GetParameters();

            // act
            var representations = parameters.Select(Utilities.GetParameterRepresentation);
            
            // assert
            representations.Should().Equal(new[]
            {
                "out System.Int32",
                "ref System.Char",
                "out System.Char",
                "System.String",
                "params System.Int64[]"
            });
        }

        [TestMethod]
        public void GetParameterRepresentation_ForComplexType()
        {
            GetParameterRepresentationAssertHelper(
                method: nameof(GetParameterRepresentationData.MethodWithComplexType),
                expectedRepresentation: "params System.Diagnostics.ProcessStartInfo[]");
        }

        [TestMethod]
        public void GetParameterRepresentation_ForGenericType()
        {
            GetParameterRepresentationAssertHelper(
                method: nameof(GetParameterRepresentationData.MethodWithGenericType),
                expectedRepresentation: "G[]");
        }

        [TestMethod]
        public void GetParameterRepresentation_ForGenericOutType()
        {
            GetParameterRepresentationAssertHelper(
                method: nameof(GetParameterRepresentationData.MethodWithGenericOutKeyward),
                expectedRepresentation: "out TKey");
        }

        [TestMethod]
        public void GetParameterRepresentation_ForMethodWithGenericObject()
        {
            GetParameterRepresentationAssertHelper(
                method: nameof(GetParameterRepresentationData.MethodWithGenericObject),
                expectedRepresentation: "System.Collections.Generic.IList<TKey>");
        }

        [TestMethod]
        public void GetParameterRepresentation_ForMethodWithGenericNestedObjectKeyward()
        {
            GetParameterRepresentationAssertHelper(
                method: nameof(GetParameterRepresentationData.MethodWithGenericNestedObjectKeyward),
                expectedRepresentation: "System.Collections.Generic.IDictionary<System.String, System.Collections.Generic.IList<System.String[]>>");
        }

        private void GetParameterRepresentationAssertHelper(string method, string expectedRepresentation)
        {
            // arrange
            var parameter = typeof(GetParameterRepresentationData).GetMethod(method).GetParameters()[0];

            // act
            var representation = Utilities.GetParameterRepresentation(parameter);

            // assert
            representation.Should().Be(expectedRepresentation);
        }
        private void IsCakeAliasMethodAssertHelper(Type type, string methodName, bool isAlias)
        {
            // arrange
            var method = type.GetMethod(methodName);

            // act
            var result = Utilities.IsCakeAliasMethod(method);

            // assert
            result.Should().Be(isAlias);
        }
    }

    class TestData
    {
        public void NonStaticMethod() { }
    }
    static class StaticTestData
    {
        public static void StaticMethodWithoutAttributes() { }
        [CakeMethodAlias]
        public static void StaticMethodWithMethodAttributeWithoutParameters() { }
        [CakePropertyAlias]
        public static void StaticMethodWithPropertyAttributeWithoutParameters() { }
        public static void StaticMethodWithoutAttributeWithThisContextParameter(this ICakeContext context) { }
        [CakeMethodAlias]
        public static void StaticMethodWithMethodAttributeWithContextParameter(ICakeContext context) { }
        [CakePropertyAlias]
        public static void StaticMethodWithPropertyAttributeWitContextParameter(ICakeContext context) { }
        [CakeMethodAlias]
        public static void StaticMethodWithMethodAttributeWithThisContextParameter(this ICakeContext context) { }
        [CakePropertyAlias]
        public static void StaticMethodWithPropertyAttributeWitThisContextParameter(this ICakeContext context) { }
        [CakeMethodAlias]
        public static void StaticMethodWithMethodAttributeWithThisContextParameterAndMoreParameters(this ICakeContext context, string a, int b) { }
    }
    class GetParameterRepresentationData
    {
        public void MethodWithPrimitiveType(out int a, ref char b, out char c, string d, params long[] e) => throw new Exception();
        public void MethodWithComplexType(params ProcessStartInfo[] settings) { }
        public void MethodWithGenericType<G>(G[] value) { }
        public void MethodWithGenericOutKeyward<TKey>(out TKey a) => throw new Exception();
        public void MethodWithGenericObject<TKey>(IList<TKey> a) => throw new Exception();
        public void MethodWithGenericNestedObjectKeyward(IDictionary<string, IList<string[]>> a) => throw new Exception();
    }
}
