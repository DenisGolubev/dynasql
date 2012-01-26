using System;
using System.Collections.Generic;
using System.Text;

namespace Scryber.OpenType
{
    public abstract class UIntBitRange
    {
        uint[] _data;
        int _len;

        public int MaxBitIndex
        {
            get { return (_len * 32) - 1; }
        }

        protected UIntBitRange(uint[] data, int length)
        {
            if (data == null || data.Length != length)
                throw new ArgumentOutOfRangeException("The range bits can only be set to a valid (non-empty) array of uint's of length " + length.ToString());
            this._data = data;
            this._len = length;
        }

        protected bool IsBitSet(int bitIndex)
        {
            int offset = GetOffset(ref bitIndex);


            uint bitval = 1;
            bitval = bitval << bitIndex;

            return (_data[offset] & bitval) > 0;
        }

        protected void SetBit(int bitIndex)
        {
            int offset = GetOffset(ref bitIndex);

            uint bitval = 1;
            bitval = bitval << bitIndex;

            _data[offset] = _data[offset] | bitval;
        }

        protected void ClearBit(int bitIndex)
        {
            int offset = GetOffset(ref bitIndex);

            uint bitval = 1;
            bitval = bitval << bitIndex;

            _data[offset] = _data[offset] & ~bitval;
        }

        public void Clear()
        {
            for (int i = 0; i < this._len; i++)
            {
                _data[i] = 0;
            }
        }

        private int GetOffset(ref int rangeBit)
        {
            int offset = 0;
            while (rangeBit >= 32)
            {
                offset++;
                rangeBit -= 32;

            }
            if (offset >= _len)
                throw new ArgumentOutOfRangeException("The rangeBit exceeds the maximum number of bits");



            return offset;
        }

        protected string BuildString(Type enumtype, string separator)
        {
            Array arry = Enum.GetValues(enumtype);
            StringBuilder sb = new StringBuilder();
            foreach (int bitindex in arry)
            {
                if (this.IsBitSet(bitindex))
                {
                    if (sb.Length > 0)
                        sb.Append(separator);
                    sb.Append(Enum.GetName(enumtype, bitindex));
                }
            }
            return sb.ToString();
        }

    }
}
