using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public interface IMessageReader
{
    int ReadInt();

    bool ReadBool();

    short ReadShort();

    float ReadFloat();

    string ReadString();

    byte ReadByte();

    byte[] ReadBytes(int length);

    sbyte ReadSignedByte();

    Guid ReadGuid();

    string[] ReadStringArray();

    int[] ReadIntArray();

    DateTime ReadDate();
}

