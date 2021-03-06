﻿using System.Collections.Generic;
using System.Linq;
using PeNet.Utilities;

namespace PeNet.Structures
{
    /// <summary>
    /// One ore more StringTable structures, where each tables szKey indicates
    /// the language and code page for displaying the text in the StringTable.
    /// </summary>
    public class StringTable : AbstractStructure
    {
        private TString[] _children;

        public StringTable(byte[] buff, uint offset) 
            : base(buff, offset)
        {
        }

        /// <summary>
        /// Length of the StringTable structure in bytes,
        /// including its children.
        /// </summary>
        public ushort wLength
        {
            get => Buff.BytesToUInt16(Offset);
            set => Buff.SetUInt16(Offset, value);
        }

        /// <summary>
        /// Always zero.
        /// </summary>
        public ushort wValueLength
        {
            get => Buff.BytesToUInt16(Offset + 0x2);
            set => Buff.SetUInt16(Offset + 0x2, value);
        }

        /// <summary>
        /// Type of the data in the version resource. Contains a 1 if the data
        /// is text data and a 0 if it contains binary data.
        /// </summary>
        public ushort wType
        {
            get => Buff.BytesToUInt16(Offset + 0x4);
            set => Buff.SetUInt16(Offset + 0x4, value);
        }

        /// <summary>
        /// Unicode string which contains a 8-digit hexadecimal number.
        /// The most significant digits represent the language identifier and
        /// the four least significant digits the code page for which the
        /// data is formatted.
        /// </summary>
        public string szKey => Buff.GetUnicodeString(Offset + 0x6);

        /// <summary>
        /// Array of String structures.
        /// </summary>
        public TString[] String {
            get
            {
                _children ??= ReadChildren();
                return _children;
            }
        }

        /// <summary>
        /// Additional information that should be displayed for diagnostic purposes.
        /// </summary>
        public string Comments => GetValue(nameof(Comments));

        /// <summary>
        /// Name of the company that produced the file, e.g. "Microsoft Corporation".
        /// </summary>
        public string CompanyName => GetValue(nameof(CompanyName));

        /// <summary>
        /// Description of the file for users, e.g. "Keyboard driver for IBM keyboards".
        /// </summary>
        public string FileDescription => GetValue(nameof(FileDescription));

        /// <summary>
        /// Version of the file, e.g. "6.00.21".
        /// </summary>
        public string FileVersion => GetValue(nameof(FileVersion));

        /// <summary>
        /// Internal name of the file, like a module or DLL name.
        /// </summary>
        public string InternalName => GetValue(nameof(InternalName));

        /// <summary>
        /// Contains all legal copyright notices, registered trademarks
        /// and copyright dates for the file. E.g. "Copyright Microsoft Corp. 1990".
        /// </summary>
        public string LegalCopyright => GetValue(nameof(LegalCopyright));

        /// <summary>
        /// Contains all legal trademarks that apply to the file, e.g.
        /// "Windows is a trademark of Microsoft Corporation".
        /// </summary>
        public string LegalTrademarks => GetValue(nameof(LegalTrademarks));

        /// <summary>
        /// The original file name not including the path. Used to identify
        /// if a file was renamed by the user.
        /// </summary>
        public string OriginalFilename => GetValue(nameof(OriginalFilename));

        /// <summary>
        /// Contains by whom, where, and why this private version of the file was built.
        /// Should only be present if the VS_FF_PRIVATEBUILD flag is set in the dwFileFlags member of the VS_FIXEDFILEINFO structure.
        /// </summary>
        public string PrivateBuild => GetValue(nameof(PrivateBuild));

        /// <summary>
        /// Name of the product with which the file was distributed, e.g. "Firefox".
        /// </summary>
        public string ProductName => GetValue(nameof(ProductName));

        /// <summary>
        /// Contains the version of the product with which the file was distributed.
        /// </summary>
        public string ProductVersion => GetValue(nameof(ProductVersion));

        /// <summary>
        /// Contains how this version of the file differs from the normal version.
        /// This entry should only be present if the VS_FF_SPECIALBUILD flag is set in the dwFileFlags member of the VS_FIXEDFILEINFO structure.
        /// </summary>
        public string SpecialBuild => GetValue(nameof(SpecialBuild));

        private string GetValue(string value) 
            => String.FirstOrDefault(s => s.szKey == value)?.Value;

        private TString[] ReadChildren()
        {
            var currentOffset = Offset + 6 + (szKey.Length * 2 + 2) +
                                (Offset + 6 + (szKey.Length * 2 + 2)).PaddingBytes(32);
            var children = new List<TString>();

            while (currentOffset < Offset + wLength)
            {
                currentOffset += currentOffset.PaddingBytes(32);
                children.Add(new TString(Buff, (uint) currentOffset));
                currentOffset += children.Last().wLength;
            }

            return children.ToArray();
        }
    }
}