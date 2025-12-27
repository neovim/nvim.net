using Microsoft.VisualStudio.TestTools.UnitTesting;
using NvimClient.NvimMsgpack.Models;
using System;

namespace NvimClient.APIGenerator.Test;


[TestClass]
public class NvimAPIGeneratorTests {
    [TestMethod]
    public void GetMetaData() {
        NvimAPIMetadata? mdata = NvimAPIGenerator.GetAPIMetadata();

        Assert.IsNotNull(mdata);
        Console.WriteLine(mdata.Version);
    }

}