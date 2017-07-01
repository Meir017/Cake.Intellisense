using Cake.Core;
using Cake.Core.Annotations;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cake.IntellisenseGenerator.Core.Tests
{
    [TestClass]
    public class CakeFileIntellisenseGeneratorTests
    {
        [TestMethod]
        public void AppendPropertySignature_SimpleProperty()
        {
            // arrange
            var builder = new StringBuilder();
            var method = typeof(AppendPropertySignatureData).GetMethod(nameof(AppendPropertySignatureData.SimpleProperty));

            // act
            CakeFileIntellisenseGenerator.AppendPropertySignature(builder, method);

            // assert
            builder.ToString().Should().Be($"{Constants.AliasesModifier}System.Int32 {nameof(AppendPropertySignatureData.SimpleProperty)}{Constants.ThrowNotSupportedExceptionArrowExpression}{Environment.NewLine}");
        }

        [TestMethod]
        public void AppendPropertySignature_ComplexProperty()
        {
            // arrange
            var builder = new StringBuilder();
            var method = typeof(AppendPropertySignatureData).GetMethod(nameof(AppendPropertySignatureData.ComplexProperty));

            // act
            CakeFileIntellisenseGenerator.AppendPropertySignature(builder, method);

            // assert
            builder.ToString().Should().Be($"{Constants.AliasesModifier}System.Collections.Generic.IDictionary<System.String, System.Collections.Generic.IList<System.Int32>> {nameof(AppendPropertySignatureData.ComplexProperty)}{Constants.ThrowNotSupportedExceptionArrowExpression}{Environment.NewLine}");
        }

        [TestMethod]
        public void AppendMethodSignature_SimpleVoidMethodWithoutParameters()
        {
            AppendMethodSignatureAssertHelper(
                methodName: nameof(AppendPropertySignatureData.SimpleVoidMethodWithoutParameters),
                expectedReturnTypeRepresentation: "void",
                expectedArgumentsRepresentation: "()");
        }

        [TestMethod]
        public void AppendMethodSignature_SimpleVoidMethodWithSingleParameter()
        {
            AppendMethodSignatureAssertHelper(
                methodName: nameof(AppendPropertySignatureData.SimpleVoidMethodWithSingleParameter),
                expectedReturnTypeRepresentation: "void",
                expectedArgumentsRepresentation: "(System.String name)");
        }

        [TestMethod]
        public void AppendMethodSignature_SimpleVoidMethodWithMultipleParameters()
        {
            AppendMethodSignatureAssertHelper(
                methodName: nameof(AppendPropertySignatureData.SimpleVoidMethodWithMultipleParameters),
                expectedReturnTypeRepresentation: "void",
                expectedArgumentsRepresentation: "(System.String name, System.Int32 age)");
        }

        [TestMethod]
        public void AppendMethodSignature_SimpleMethodWithoutParameters()
        {
            AppendMethodSignatureAssertHelper(
                methodName: nameof(AppendPropertySignatureData.SimpleMethodWithoutParameters),
                expectedReturnTypeRepresentation: "System.Int32",
                expectedArgumentsRepresentation: "()");
        }

        [TestMethod]
        public void AppendMethodSignature_SimpleMethodWithSingleParameter()
        {
            AppendMethodSignatureAssertHelper(
                methodName: nameof(AppendPropertySignatureData.SimpleMethodWithSingleParameter),
                expectedReturnTypeRepresentation: "System.Int32",
                expectedArgumentsRepresentation: "(System.String name)");
        }

        [TestMethod]
        public void AppendMethodSignature_SimpleMethodWithMultipleParameters()
        {
            // arrange
            AppendMethodSignatureAssertHelper(
                methodName: nameof(AppendPropertySignatureData.SimpleMethodWithMultipleParameters),
                expectedReturnTypeRepresentation: "System.Int32",
                expectedArgumentsRepresentation: "(System.String name, System.Int32 age)");
        }

        [TestMethod]
        public void AppendMethodSignature_ComplexMethodWithoutParameters()
        {
            AppendMethodSignatureAssertHelper(
                methodName: nameof(AppendPropertySignatureData.ComplexMethodWithoutParameters),
                expectedReturnTypeRepresentation: "System.Collections.Generic.IDictionary<TKey, System.Collections.Generic.IList<TValue>>",
                expectedArgumentsRepresentation: "<TKey, TValue>()");
        }

        [TestMethod]
        public void AppendMethodSignature_ComplexMethodWithoutParametersWithSingleParameter()
        {
            AppendMethodSignatureAssertHelper(
                methodName: nameof(AppendPropertySignatureData.ComplexMethodWithoutParametersWithSingleParameter),
                expectedReturnTypeRepresentation: "System.Collections.Generic.IDictionary<TKey, System.Collections.Generic.IList<TValue>>",
                expectedArgumentsRepresentation: "<TKey, TValue>(System.String name)");
        }

        [TestMethod]
        public void AppendMethodSignature_ComplexMethodWithoutParametersWithMultipleParameters()
        {
            AppendMethodSignatureAssertHelper(
                methodName: nameof(AppendPropertySignatureData.ComplexMethodWithoutParametersWithMultipleParameters),
                expectedReturnTypeRepresentation: "System.Collections.Generic.IDictionary<TKey, System.Collections.Generic.IList<TValue>>",
                expectedArgumentsRepresentation: "<TKey, TValue>(System.String name, System.Int32 age)");
        }

        [TestMethod]
        public void AppendMethodSignature_ComplexMethodWithoutParametersWithMultipleGenericParameters()
        {
            AppendMethodSignatureAssertHelper(
                methodName: nameof(AppendPropertySignatureData.ComplexMethodWithoutParametersWithMultipleGenericParameters),
                expectedReturnTypeRepresentation: "System.Collections.Generic.IDictionary<TKey, System.Collections.Generic.IList<TValue>>",
                expectedArgumentsRepresentation: "<TKey, TValue>(TKey name, TValue age)");
        }

        [TestMethod]
        public void AppendMethodSignature_ComplexMethodWithSingleConstrait()
        {
            AppendMethodSignatureAssertHelper(
                methodName: nameof(AppendPropertySignatureData.ComplexMethodWithSingleConstrait),
                expectedReturnTypeRepresentation: "System.Collections.Generic.IDictionary<TKey, System.Collections.Generic.IList<TValue>>",
                expectedArgumentsRepresentation: "<TKey, TValue>() where TKey : System.IComparable<TKey>");
        }

        [TestMethod]
        public void AppendMethodSignature_ComplexMethodWitMultipleConstraitsForSameArgument()
        {
            AppendMethodSignatureAssertHelper(
                methodName: nameof(AppendPropertySignatureData.ComplexMethodWitMultipleConstraitsForSameArgument),
                expectedReturnTypeRepresentation: "System.Collections.Generic.IDictionary<TKey, System.Collections.Generic.IList<TValue>>",
                expectedArgumentsRepresentation: "<TKey, TValue>() where TKey : Cake.IntellisenseGenerator.Core.Tests.Foo, System.IComparable<TKey>, System.IEquatable<TKey>");
        }

        [TestMethod]
        public void AppendMethodSignature_ComplexMethodWithSingleConstraitForMultipleArguments()
        {
            AppendMethodSignatureAssertHelper(
                methodName: nameof(AppendPropertySignatureData.ComplexMethodWithSingleConstraitForMultipleArguments),
                expectedReturnTypeRepresentation: "System.Collections.Generic.IDictionary<TKey, System.Collections.Generic.IList<TValue>>",
                expectedArgumentsRepresentation: "<TKey, TValue>() where TKey : System.IComparable<TKey> where TValue : System.IEquatable<TValue>");
        }

        [TestMethod]
        public void AppendMethodSignature_ComplexMethodWitMultipleConstraitsForMultipleArgument()
        {
            AppendMethodSignatureAssertHelper(
                methodName: nameof(AppendPropertySignatureData.ComplexMethodWitMultipleConstraitsForMultipleArgument),
                expectedReturnTypeRepresentation: "System.Collections.Generic.IDictionary<TKey, System.Collections.Generic.IList<TValue>>",
                expectedArgumentsRepresentation: "<TKey, TValue>() where TKey : Cake.IntellisenseGenerator.Core.Tests.Foo, System.IComparable<TKey>, System.IEquatable<TKey> where TValue : Cake.IntellisenseGenerator.Core.Tests.Foo, System.IEquatable<TValue>");
        }

        [TestMethod]
        public void AppendMethodSignature_SimpleMethodWithNewConstraint()
        {
            AppendMethodSignatureAssertHelper(
                methodName: nameof(AppendPropertySignatureData.SimpleMethodWithNewConstraint),
                expectedReturnTypeRepresentation: "void",
                expectedArgumentsRepresentation: "<T>() where T : new()");
        }

        [TestMethod]
        public void AppendMethodSignature_SimpleMethodWithClassConstraint()
        {
            AppendMethodSignatureAssertHelper(
                methodName: nameof(AppendPropertySignatureData.SimpleMethodWithClassConstraint),
                expectedReturnTypeRepresentation: "void",
                expectedArgumentsRepresentation: "<T>() where T : class");
        }

        [TestMethod]
        public void AppendMethodSignature_SimpleMethodWithStructConstraint()
        {
            AppendMethodSignatureAssertHelper(
                methodName: nameof(AppendPropertySignatureData.SimpleMethodWithStructConstraint),
                expectedReturnTypeRepresentation: "void",
                expectedArgumentsRepresentation: "<T>() where T : struct");
        }

        [TestMethod]
        public void AppendMethodSignature_SimpleMethodWithClassAndNewConstraint()
        {
            AppendMethodSignatureAssertHelper(
                methodName: nameof(AppendPropertySignatureData.SimpleMethodWithClassAndNewConstraint),
                expectedReturnTypeRepresentation: "void",
                expectedArgumentsRepresentation: "<T>() where T : class, new()");
        }

        private void AppendMethodSignatureAssertHelper(string methodName, string expectedReturnTypeRepresentation, string expectedArgumentsRepresentation)
        {
            // arrange
            var builder = new StringBuilder();
            var method = typeof(AppendPropertySignatureData).GetMethod(methodName);

            // act
            CakeFileIntellisenseGenerator.AppendMethodSignature(builder, method);

            // assert
            builder.ToString().Should().Be($"{Constants.AliasesModifier}{expectedReturnTypeRepresentation} {methodName}{expectedArgumentsRepresentation}{Constants.ThrowNotSupportedExceptionArrowExpression}{Environment.NewLine}");
        }
    }

    public static class AppendPropertySignatureData
    {
        [CakePropertyAlias]
        public static int SimpleProperty(this ICakeContext context) => throw new Exception();
        [CakePropertyAlias]
        public static IDictionary<string, IList<int>> ComplexProperty(this ICakeContext context) => throw new Exception();

        [CakeMethodAlias]
        public static void SimpleVoidMethodWithoutParameters(this ICakeContext context) => throw new Exception();

        [CakeMethodAlias]
        public static void SimpleVoidMethodWithSingleParameter(this ICakeContext context, string name) => throw new Exception();

        [CakeMethodAlias]
        public static void SimpleVoidMethodWithMultipleParameters(this ICakeContext context, string name, int age) => throw new Exception();

        [CakeMethodAlias]
        public static int SimpleMethodWithoutParameters(this ICakeContext context) => throw new Exception();

        [CakeMethodAlias]
        public static int SimpleMethodWithSingleParameter(this ICakeContext context, string name) => throw new Exception();

        [CakeMethodAlias]
        public static int SimpleMethodWithMultipleParameters(this ICakeContext context, string name, int age) => throw new Exception();

        [CakeMethodAlias]
        public static IDictionary<TKey, IList<TValue>> ComplexMethodWithoutParameters<TKey, TValue>(this ICakeContext context) => throw new Exception();

        [CakeMethodAlias]
        public static IDictionary<TKey, IList<TValue>> ComplexMethodWithoutParametersWithSingleParameter<TKey, TValue>(this ICakeContext context, string name) => throw new Exception();

        [CakeMethodAlias]
        public static IDictionary<TKey, IList<TValue>> ComplexMethodWithoutParametersWithMultipleParameters<TKey, TValue>(this ICakeContext context, string name, int age) => throw new Exception();

        [CakeMethodAlias]
        public static IDictionary<TKey, IList<TValue>> ComplexMethodWithoutParametersWithMultipleGenericParameters<TKey, TValue>(this ICakeContext context, TKey name, TValue age) => throw new Exception();

        [CakeMethodAlias]
        public static IDictionary<TKey, IList<TValue>> ComplexMethodWithSingleConstrait<TKey, TValue>(this ICakeContext context) where TKey : IComparable<TKey> => throw new Exception();

        [CakeMethodAlias]
        public static IDictionary<TKey, IList<TValue>> ComplexMethodWitMultipleConstraitsForSameArgument<TKey, TValue>(this ICakeContext context) where TKey : Foo, IComparable<TKey>, IEquatable<TKey> => throw new Exception();

        [CakeMethodAlias]
        public static IDictionary<TKey, IList<TValue>> ComplexMethodWithSingleConstraitForMultipleArguments<TKey, TValue>(this ICakeContext context) where TKey : IComparable<TKey> where TValue : IEquatable<TValue> => throw new Exception();

        [CakeMethodAlias]
        public static IDictionary<TKey, IList<TValue>> ComplexMethodWitMultipleConstraitsForMultipleArgument<TKey, TValue>(this ICakeContext context)
            where TKey : Foo, IComparable<TKey>, IEquatable<TKey> where TValue : Foo, IEquatable<TValue> => throw new Exception();

        [CakeMethodAlias]
        public static void SimpleMethodWithNewConstraint<T>(this ICakeContext context) where T : new() => throw new Exception();

        [CakeMethodAlias]
        public static void SimpleMethodWithClassConstraint<T>(this ICakeContext context) where T : class => throw new Exception();

        [CakeMethodAlias]
        public static void SimpleMethodWithStructConstraint<T>(this ICakeContext context) where T : struct => throw new Exception();

        [CakeMethodAlias]
        public static void SimpleMethodWithClassAndNewConstraint<T>(this ICakeContext context) where T : class, new() => throw new Exception();
    }
    public class Foo { }
}
