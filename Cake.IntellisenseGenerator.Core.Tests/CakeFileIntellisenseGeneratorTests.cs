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
            builder.ToString().Should().Be($"{CakeFileIntellisenseGenerator.AliasesModifier}System.Int32 {nameof(AppendPropertySignatureData.SimpleProperty)}{CakeFileIntellisenseGenerator.ThrowNotSupportedExceptionArrowExpression}{Environment.NewLine}");
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
            builder.ToString().Should().Be($"{CakeFileIntellisenseGenerator.AliasesModifier}System.Collections.Generic.IDictionary<System.String, System.Collections.Generic.IList<System.Int32>> {nameof(AppendPropertySignatureData.ComplexProperty)}{CakeFileIntellisenseGenerator.ThrowNotSupportedExceptionArrowExpression}{Environment.NewLine}");
        }

        [TestMethod]
        public void AppendMethodSignature_SimpleMethodWithoutParameters()
        {
            // arrange
            var builder = new StringBuilder();
            var method = typeof(AppendPropertySignatureData).GetMethod(nameof(AppendPropertySignatureData.SimpleMethodWithoutParameters));

            // act
            CakeFileIntellisenseGenerator.AppendMethodSignature(builder, method);

            // assert
            builder.ToString().Should().Be($"{CakeFileIntellisenseGenerator.AliasesModifier}System.Int32 {nameof(AppendPropertySignatureData.SimpleMethodWithoutParameters)}(){CakeFileIntellisenseGenerator.ThrowNotSupportedExceptionArrowExpression}{Environment.NewLine}");
        }

        [TestMethod]
        public void AppendMethodSignature_SimpleMethodWithSingleParameter()
        {
            // arrange
            var builder = new StringBuilder();
            var method = typeof(AppendPropertySignatureData).GetMethod(nameof(AppendPropertySignatureData.SimpleMethodWithSingleParameter));

            // act
            CakeFileIntellisenseGenerator.AppendMethodSignature(builder, method);

            // assert
            builder.ToString().Should().Be($"{CakeFileIntellisenseGenerator.AliasesModifier}System.Int32 {nameof(AppendPropertySignatureData.SimpleMethodWithSingleParameter)}(System.String name){CakeFileIntellisenseGenerator.ThrowNotSupportedExceptionArrowExpression}{Environment.NewLine}");
        }

        [TestMethod]
        public void AppendMethodSignature_SimpleMethodWithMultipleParameters()
        {
            // arrange
            var builder = new StringBuilder();
            var method = typeof(AppendPropertySignatureData).GetMethod(nameof(AppendPropertySignatureData.SimpleMethodWithMultipleParameters));

            // act
            CakeFileIntellisenseGenerator.AppendMethodSignature(builder, method);

            // assert
            builder.ToString().Should().Be($"{CakeFileIntellisenseGenerator.AliasesModifier}System.Int32 {nameof(AppendPropertySignatureData.SimpleMethodWithMultipleParameters)}(System.String name, System.Int32 age){CakeFileIntellisenseGenerator.ThrowNotSupportedExceptionArrowExpression}{Environment.NewLine}");
        }

        [TestMethod]
        public void AppendMethodSignature_ComplexMethodWithoutParameters()
        {
            // arrange
            var builder = new StringBuilder();
            var method = typeof(AppendPropertySignatureData).GetMethod(nameof(AppendPropertySignatureData.ComplexMethodWithoutParameters));

            // act
            CakeFileIntellisenseGenerator.AppendMethodSignature(builder, method);

            // assert
            builder.ToString().Should().Be($"{CakeFileIntellisenseGenerator.AliasesModifier}System.Collections.Generic.IDictionary<Tkey, System.Collections.Generic.IList<TValue>> {nameof(AppendPropertySignatureData.ComplexMethodWithoutParameters)}<Tkey, TValue>(){CakeFileIntellisenseGenerator.ThrowNotSupportedExceptionArrowExpression}{Environment.NewLine}");
        }
    }

    public static class AppendPropertySignatureData
    {
        [CakePropertyAlias]
        public static int SimpleProperty(this ICakeContext context) => throw new Exception();
        [CakePropertyAlias]
        public static IDictionary<string, IList<int>> ComplexProperty(this ICakeContext context) => throw new Exception();

        [CakeMethodAlias]
        public static int SimpleMethodWithoutParameters(this ICakeContext context) => throw new Exception();

        [CakeMethodAlias]
        public static int SimpleMethodWithSingleParameter(this ICakeContext context, string name) => throw new Exception();

        [CakeMethodAlias]
        public static int SimpleMethodWithMultipleParameters(this ICakeContext context, string name, int age) => throw new Exception();

        [CakeMethodAlias]
        public static IDictionary<Tkey, IList<TValue>> ComplexMethodWithoutParameters<Tkey, TValue>(this ICakeContext context) => throw new Exception();
    }
}
