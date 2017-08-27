using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WechatReader
{
    // In `WCDB_Contacts.db`, a couple of columns are stored as a `BLOB` and contain some serialized data.
    // This class attempts to decode them. We are not aware of the entire format spec yet. Please let me
    // know if there are new possible fields / types etc.
    // We now enforce strict checks and will complain whenever the input is different from the spec below.
    // Speculative  format:
    // A `blob` is some `sections` concatenated.
    // A `section` is a `name`, a `number`, and an optional `string`.
    // `number` is a little-endian variable-length quantity.
    // A `name` is a byte.
    //   If `name & 0x2 == 0`, `number` is the value of this section.
    //   Else, the `number` bytes afterwards are a utf-8 string, the value of this section.
    public static class BlobParser
    {
        public static List<Section> Parse(Stream stream)
        {
            var sections = new List<Section>();
                while (true)
                {
                    var name = stream.ReadByte();
                    if (name == -1) break;
                    var number = ReadNumber(stream);
                    if ((name & 7) != 2 && (name & 7) != 0) throw new Exception("Unknown type.");
                    if ((name & 0x2) == 0)
                    {
                        sections.Add(new Section { Name = (byte)name, Type = Section.ValueType.Number, Number = number });
                    }
                    else
                    {
                        var buffer = new byte[number];
                        var read = stream.Read(buffer, 0, (int)number);
                        if (read != number) throw new Exception("String length did not match.");
                        var str = Encoding.UTF8.GetString(buffer);
                        sections.Add(new Section { Name = (byte)name, Type = Section.ValueType.String, String = str });
                    }
                }
            return sections;
        }

        private static long ReadNumber(Stream stream)
        {
            var num = 0L;
            var lift = 0;
            while (true)
            {
                var next = stream.ReadByte();
                if (next == -1) throw new Exception("Stream ended unexpectly.");
                num |= (long)(next & 0x7f) << lift;
                if ((next & 0x80) == 0) break;
                lift += 7;
            }
            return num;
        }

        public static string TryGetString(List<Section> sections, byte name)
        {
            foreach (var section in sections)
            {
                if (section.Name == name && section.Type == Section.ValueType.String) return section.String;
            }
            return null;
        }
        public static long TryGetNumber(List<Section> sections, byte name)
        {
            foreach (var section in sections)
            {
                if (section.Name == name && section.Type == Section.ValueType.Number) return section.Number;
            }
            return 0;
        }
    }

    public class Section
    {
        public enum ValueType { String, Number }

        public byte Name;
        public ValueType Type;
        public long Number;
        public string String;
    }
}
