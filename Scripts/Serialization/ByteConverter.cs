using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;


class ByteConverter
{
    public static string ToString(byte[] data, int startIndex)
    {
        ASCIIEncoding enc = new ASCIIEncoding();
        return enc.GetString(data, startIndex, data.Length - startIndex);
    }

    public static string ToString(byte[] data, int startIndex, int length)
    {
        ASCIIEncoding enc = new ASCIIEncoding();
        return enc.GetString(data, startIndex, length);
    }

    public static string[] ToLogin(byte[] data, int startIndex)
    {
        string s = ToString(data, startIndex);
        return s.Split(new char[] { ':' });
    }

    public static byte[] GetBytes(string p)
    {
        ASCIIEncoding enc = new ASCIIEncoding();
        return enc.GetBytes(p);
    }

    public static byte[] GetBytes(int p)
    {
        return BitConverter.GetBytes(p);
    }



    public static byte[] Compose(params byte[][] arrays)
    {
        int size = 0;

        for (int i = 0; i < arrays.Length; i++)
        {
            size += arrays[i].Length;
        }

        byte[] result = new byte[size];
        int offset = 0;

        for (int i = 0; i < arrays.Length; i++)
        {
            arrays[i].CopyTo(result, offset);
            offset += arrays[i].Length;
        }

        return result;
    }

    public static T[] SubArray<T>(T[] data, int index, int length)
    {
        T[] result = new T[length];
        Array.Copy(data, index, result, 0, length);
        return result;
    }

    internal static BitArray ToBitField(byte[] data, int p, int size)
    {
        byte[] result = new byte[size];

        for (int i = 0; i < size; i++)
        {
            result[i] = data[p + i];
        }

        return new BitArray(result);
    }

    internal static byte[] GetBytes(BitArray value, int size)
    {
        if (value == null)
        {
            return new byte[size];
        }

        byte[] result = new byte[size];
        value.CopyTo(result, 0);
        return result;
    }
}

