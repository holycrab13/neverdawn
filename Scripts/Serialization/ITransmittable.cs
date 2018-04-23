using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public interface ITransmittable
{
    void ReadData(IMessageReader reader);

    void WriteData(IMessageWriter writer);
}
