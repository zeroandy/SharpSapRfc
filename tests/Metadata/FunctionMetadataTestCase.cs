﻿using SharpSapRfc.Metadata;
using SharpSapRfc.Plain;
using SharpSapRfc.Soap;
using SharpSapRfc.Soap.Configuration;
using SharpSapRfc.Types;
using System;
using Xunit;

namespace SharpSapRfc.Test.Metadata
{
    public class Soap_FunctionMetadataTestCase : FunctionMetadataTestCase
    {
        private SapSoapRfcConnection conn;

        public override AbapMetadataCache GetMetadataCache()
        {
            this.conn = new SapSoapRfcConnection("TST-SOAP");
            return new SoapAbapMetadataCache(this.conn.WebClient);
        }

        public override void Dispose()
        {
            this.conn.Dispose();
        }
    }

    public class Plain_FunctionMetadataTestCase : FunctionMetadataTestCase
    {
        private SapPlainRfcConnection conn;

        public override AbapMetadataCache GetMetadataCache()
        {
            this.conn = new SapPlainRfcConnection("TST");
            return new PlainAbapMetadataCache(this.conn);
        }

        public override void Dispose()
        {
            this.conn.Dispose();
        }
    }

    public abstract class FunctionMetadataTestCase : IDisposable
    {
        public abstract AbapMetadataCache GetMetadataCache();

        [Fact]
        public void SimpleFunctionMetadataTest()
        {
            var cache = GetMetadataCache();
            var metadata = cache.GetFunctionMetadata("Z_SSRT_SUM");

            Assert.Equal(2, metadata.InputParameters.Length);
            Assert.Equal(1, metadata.OutputParameters.Length);
            Assert.Equal("Z_SSRT_SUM", metadata.Name);

            AssertInputParameter(metadata, "I_NUM1", AbapDataType.INTEGER);
            AssertInputParameter(metadata, "I_NUM2", AbapDataType.INTEGER);
            AssertOutputParameter(metadata, "E_RESULT", AbapDataType.INTEGER);
        }

        [Fact]
        public void InOutFunctionMetadataTest()
        {
            var cache = GetMetadataCache();
            var metadata = cache.GetFunctionMetadata("Z_SSRT_IN_OUT");
           
            Assert.Equal(8, metadata.InputParameters.Length);
            Assert.Equal(10, metadata.OutputParameters.Length);
            Assert.Equal("Z_SSRT_IN_OUT", metadata.Name);

            AssertInputParameter(metadata, "I_ID", AbapDataType.INTEGER);
            AssertInputParameter(metadata, "I_PRICE", AbapDataType.DECIMAL);
            AssertInputParameter(metadata, "I_DATUM", AbapDataType.DATE);
            AssertInputParameter(metadata, "I_UZEIT", AbapDataType.TIME);
            AssertInputParameter(metadata, "I_ACTIVE", AbapDataType.CHAR);
            AssertInputParameter(metadata, "I_MARA", AbapDataType.STRUCTURE);
            AssertInputParameter(metadata, "I_MULTIPLE_ID", AbapDataType.TABLE);
            AssertInputParameter(metadata, "I_MULTIPLE_NAME", AbapDataType.TABLE);

            AssertOutputParameter(metadata, "E_ID", AbapDataType.INTEGER);
            AssertOutputParameter(metadata, "E_PRICE", AbapDataType.DECIMAL);
            AssertOutputParameter(metadata, "E_DATUM", AbapDataType.DATE);
            AssertOutputParameter(metadata, "E_UZEIT", AbapDataType.TIME);
            AssertOutputParameter(metadata, "E_ACTIVE", AbapDataType.CHAR);
            AssertOutputParameter(metadata, "E_MARA_DATUM", AbapDataType.DATE);
            AssertOutputParameter(metadata, "E_MARA_UZEIT", AbapDataType.TIME);
            AssertOutputParameter(metadata, "E_MARA_ID", AbapDataType.INTEGER);
            AssertOutputParameter(metadata, "E_MULTIPLE_ID", AbapDataType.TABLE);
            AssertOutputParameter(metadata, "E_MULTIPLE_NAME", AbapDataType.TABLE);
        }

        private void AssertInputParameter(FunctionMetadata metadata, string name, AbapDataType dataType)
        {
            var parameter = metadata.GetInputParameter(name);
            Assert.Equal(name, parameter.Name);
            Assert.Equal(dataType, parameter.DataType);
        }

        private void AssertOutputParameter(FunctionMetadata metadata,  string name, AbapDataType dataType)
        {
            var parameter = metadata.GetOutputParameter(name);
            Assert.Equal(name, parameter.Name);
            Assert.Equal(dataType, parameter.DataType);
        }

        public abstract void Dispose();
    }
}
