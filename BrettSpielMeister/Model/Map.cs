using System;
using System.Collections.Generic;

namespace BrettSpielMeister.Model
{
    public class Map
    {
        public Field[] Fields { get; private set; }

        public bool[][] FieldConnections { get; private set; }

        public virtual void Create()
        {
        }

        public void SetFields(Field[] fields)
        {
            Fields = fields;
            FieldConnections = new bool[Fields.Length][];
            for (var n = 0; n < Fields.Length; n++) FieldConnections[n] = new bool[Fields.Length];
        }

        public void AddConnection(Field from, Field to)
        {
            var fromIndex = Array.IndexOf(Fields, from);
            var toIndex = Array.IndexOf(Fields, to);
            if (fromIndex == -1 || toIndex == -1) return;

            FieldConnections[fromIndex][toIndex] = true;
        }

        public bool IsConnected(Field from, Field to)
        {
            var fromIndex = Array.IndexOf(Fields, from);
            var toIndex = Array.IndexOf(Fields, to);
            if (fromIndex == -1 || toIndex == -1) return false;

            return FieldConnections[fromIndex][toIndex];
        }

        public IEnumerable<Field> GetConnections(Field from)
        {
            var fromIndex = Array.IndexOf(Fields, from);
            for (var n = 0; n < Fields.Length; n++)
                if (FieldConnections[fromIndex][n])
                    yield return Fields[n];
        }

        public void Clear()
        {
            FieldConnections = null;
            Fields = null;
        }
    }
}