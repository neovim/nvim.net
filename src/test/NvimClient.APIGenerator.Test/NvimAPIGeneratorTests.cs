using Microsoft.VisualStudio.TestTools.UnitTesting;
using NvimClient.Models.Nvim;
using System;

namespace NvimClient.APIGenerator.Test;


[TestClass]
public class NvimAPIGeneratorTests {
    [TestMethod]
    public void GetMetaData() {
        NvimAPIMetadata? mdata = MetaDataProvider.GetAPIMetadata();

        Assert.IsNotNull(mdata);
        Console.WriteLine(mdata.Version);
    }

}