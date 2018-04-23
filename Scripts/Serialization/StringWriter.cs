using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class StringWriter : IMessageWriter
{
    private List<byte> data;

    public String Text
    {
        get { return Convert.ToBase64String(data.ToArray()); }
    }

    public StringWriter()
    {
        data = new List<byte>();
    }

    public void WriteFloat(float f)
    {
        appendBytes(BitConverter.GetBytes(f));
    }

    public void WriteString(string s)
    {
        if (s == null)
        {
            s = "";
        }

        appendBytes(BitConverter.GetBytes(s.Length));
        appendBytes(ByteConverter.GetBytes(s));
    }

    public void WriteInt(int OwnerId)
    {
        appendBytes(BitConverter.GetBytes(OwnerId));
    }

    public void WriteBool(bool b)
    {
        appendBytes(BitConverter.GetBytes(b));
    }

    public void WriteBytes(byte[] p)
    {
        appendBytes(p);
    }

    public void WriteByte(byte p)
    {
        appendBytes(p);
    }

    public void WriteSignedByte(sbyte p)
    {
        appendBytes((byte)(p + 128));
    }

    public void WriteTransmittable(ITransmittable transmittable)
    {
        transmittable.WriteData(this);
    }

    public void WriteGuid(Guid guid)
    {
        appendBytes(guid.ToByteArray());
    }

    public void WriteStringArray(string[] array)
    {
        if (array == null)
        {
            array = new string[0];
        }

        WriteInt(array.Length);

        foreach (string s in array)
        {
            WriteString(s);
        }
    }

    public void WriteIntArray(int[] array)
    {
        if (array == null)
        {
            array = new int[0];
        }

        WriteInt(array.Length);

        foreach (int s in array)
        {
            WriteInt(s);
        }
    }

    private void appendBytes(params byte[] p)
    {
        data.AddRange(p);
    }

    public void WriteDate(DateTime dateTime)
    {
        appendBytes(BitConverter.GetBytes(dateTime.ToBinary()));
    }
}

