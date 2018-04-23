using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class StringReader : IMessageReader
{
    private byte[] data;
    private int index;
    private string text;

    public StringReader(string text)
    {
        this.data = Convert.FromBase64String(text);
        this.index = 0;
        this.text = text;
    }

    public int GetSender()
    {
        return 0;
    }

    public int GetLength()
    {
        return data.Length;
    }

    public IMessageReader Clone()
    {
        return new StringReader(text);
    }

    public int ReadInt()
    {
        int i = BitConverter.ToInt32(data, index);
        index += 4;
        return i;
    }

    public short ReadShort()
    {
        short i = BitConverter.ToInt16(data, index);
        index += 2;
        return i;
    }

    public byte ReadByte()
    {
        byte b = data[index];
        index += 1;
        return b;
    }

    public byte[] ReadBytes(int length)
    {
        byte[] b = new byte[length];

        for (int i = 0; i < length; i++)
        {
            b[i] = data[index];
            index += 1;
        }

        return b;
    }


    public bool ReadBool()
    {
        bool b = BitConverter.ToBoolean(data, index);
        index += 1;
        return b;
    }


    public sbyte ReadSignedByte()
    {
        byte c = (byte)data[index];
        index += 1;
        return (sbyte)(c - 128);
    }

    public Guid ReadGuid()
    {
        byte[] guidData = new byte[16];

        for (int i = 0; i < 16; i++)
        {
            guidData[i] = data[index + i];
        }

        index += 16;
        return new Guid(guidData);
    }

    public float ReadFloat()
    {
        float f = BitConverter.ToSingle(data, index);
        index += 4;
        return f;
    }

    public string ReadString()
    {
        int length = BitConverter.ToInt32(data, index);
        index += 4;

        string result = ByteConverter.ToString(data, index, length);
        index += length;
        return result;
    }

    public string[] ReadStringArray()
    {
        string[] result = new string[ReadInt()];

        for (int i = 0; i < result.Length; i++)
        {
            result[i] = ReadString();
        }

        return result;
    }

    public int[] ReadIntArray()
    {
        int[] result = new int[ReadInt()];

        for (int i = 0; i < result.Length; i++)
        {
            result[i] = ReadInt();
        }

        return result;
    }



    public DateTime ReadDate()
    {
        long dbl = BitConverter.ToInt64(data, index);
        index += 8;
        return DateTime.FromBinary(dbl);
    }
}


