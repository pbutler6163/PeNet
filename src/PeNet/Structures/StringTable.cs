﻿using PeNet.Utilities;

namespace PeNet.Structures
{
    /// <summary>
    /// One ore more StringTable structures, where each tables szKey indicates
    /// the language and code page for displaying the text in the StringTable.
    /// </summary>
    public class StringTable : AbstractStructure
    {
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
    }
}