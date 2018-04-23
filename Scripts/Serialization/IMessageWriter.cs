using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public interface IMessageWriter
{
    void WriteFloat(float f);

    void WriteString(string s);

    void WriteInt(int OwnerId);

    void WriteBool(bool b);

    void WriteBytes(byte[] p);

    void WriteByte(byte p);

    void WriteSignedByte(sbyte p);

    void WriteTransmittable(ITransmittable transmittable);

    void WriteGuid(Guid guid);

    void WriteStringArray(string[] array);

    void WriteIntArray(int[] array);

    void WriteDate(DateTime dateTime);

}

