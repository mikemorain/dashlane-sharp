// Copyright (C) 2016 Dmitry Yakimenko (detunized@gmail.com).
// Licensed under the terms of the MIT license. See LICENCE for details.

using System;
using System.IO;
using System.Xml.Linq;
using NUnit.Framework;

namespace Dashlane.Test
{
    [TestFixture]
    class ImportTest
    {
        private const string Passowrd = "password";
        private const string Uki = "local-uki";
        private const string Filename = "Fixtures/localSettings.aes";
        private const string Xml =
            "<root>" +
                "<KWLocalSettingsManager>" +
                    "<KWDataItem key='uki'>local-uki</KWDataItem>" +
                "</KWLocalSettingsManager>" +
            "</root>";

        [Test]
        public void ImportUkiFromSettingsFile_returns_uki()
        {
            Assert.That(
                Import.ImportUkiFromSettingsFile(Filename, Passowrd),
                Is.EqualTo(Uki));
        }

        [Test]
        public void ImportUkiFromSettings_as_xml_string_returns_uki()
        {
            Assert.That(
                Import.ImportUkiFromSettings(Xml),
                Is.EqualTo(Uki));
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "Invalid XML in the settings file")]
        public void ImportUkiFromSettings_as_xml_string_throws_on_invalid_xml()
        {
            Assert.That(
                Import.ImportUkiFromSettings("> not really xml <"),
                Is.EqualTo(Uki));
        }

        [Test]
        public void ImportUkiFromSettings_as_xdocument_returns_uki()
        {
            Assert.That(
                Import.ImportUkiFromSettings(XDocument.Parse(Xml)),
                Is.EqualTo(Uki));
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "The settings file doesn't contain an UKI")]
        public void ImportUkiFromSettings_as_xdocument_throws_on_wrong_xml()
        {
            Assert.That(
                Import.ImportUkiFromSettings(XDocument.Parse("<root />")),
                Is.EqualTo(Uki));
        }

        [Test]
        public void LoadSettingsFile_reads_and_decrypts_settings_xml()
        {
            Assert.That(
                Import.LoadSettingsFile(Filename, Passowrd),
                Is.StringStarting("<?xml").And.StringEnding("</root>\n"));
        }
    }
}