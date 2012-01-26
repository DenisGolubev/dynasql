using System;
using System.Collections.Generic;
using System.Text;

namespace Scryber.OpenType.SubTables
{
    public class OS2Table : TTFTable
    {
        public OS2Table(long offset)
            : base(offset)
        {
        }


        private OS2TableVersion _vers;

        public OS2TableVersion Version
        {
            get { return _vers; }
            set { _vers = value; }
        }

        private short _xavgcharwidth;

        public short XAverageCharWidth
        {
            get { return _xavgcharwidth; }
            set { _xavgcharwidth = value; }
        }

        private WeightClass _usweightclass;

        public WeightClass WeightClass
        {
            get { return _usweightclass; }
            set { _usweightclass = value; }
        }

        private WidthClass _uswidth;

        public WidthClass WidthClass
        {
            get { return _uswidth; }
            set { _uswidth = value; }
        }

        private FontRestrictions _fstype;

        public FontRestrictions FSType
        {
            get { return _fstype; }
            set { _fstype = value; }
        }

        private short _ySubscriptXSize;

        public short SubscriptXSize
        {
            get { return _ySubscriptXSize; }
            set { _ySubscriptXSize = value; }
        }

        private short _ySubYSize;

        public short SubscriptYSize
        {
            get { return _ySubYSize; }
            set { _ySubYSize = value; }
        }

        private short _ySubsciptXOff;

        public short SubscriptXOffset
        {
            get { return _ySubsciptXOff; }
            set { _ySubsciptXOff = value; }
        }

        private short _ysubyoff;

        public short SubscriptYOffset
        {
            get { return _ysubyoff; }
            set { _ysubyoff = value; }
        }

        private short _ysupxsize;

        public short SuperScriptXSize
        {
            get { return _ysupxsize; }
            set { _ysupxsize = value; }
        }

        private short _ysupysize;

        public short SuperScriptYSize
        {
            get { return _ysupysize; }
            set { _ysupysize = value; }
        }

        private short _ysupxoff;

        public short SuperscriptXOffset
        {
            get { return _ysupxoff; }
            set { _ysupxoff = value; }
        }

        private short _ysupyoff;

        public short SuperscriptYOffset
        {
            get { return _ysupyoff; }
            set { _ysupyoff = value; }
        }

        private short _ystrikesize;

        public short StrikeoutSize
        {
            get { return _ystrikesize; }
            set { _ystrikesize = value; }
        }

        private short _ystrikeoff;

        public short StrikeoutPosition
        {
            get { return _ystrikeoff; }
            set { _ystrikeoff = value; }
        }

        private IBMFontClass _sfamilyclass;

        public IBMFontClass FamilyClass
        {
            get { return _sfamilyclass; }
            set { _sfamilyclass = value; }
        }

        private PanoseArray _panose;

        public PanoseArray Panose
        {
            get { return _panose; }
            set { _panose = value; }
        }

        private UnicodeRanges _ranges;

        public UnicodeRanges UnicodeRanges
        {
            get { return _ranges; }
            set { _ranges = value; }
        }


        private string _vendif;

        public string VendorID
        {
            get { return _vendif; }
            set { _vendif = value; }
        }

        private FontSelection _fssel;

        public FontSelection Selection
        {
            get { return _fssel; }
            set { _fssel = value; }
        }

        private ushort _firstcharindex;

        public ushort FirstCharIndex
        {
            get { return _firstcharindex; }
            set { _firstcharindex = value; }
        }

        private ushort _lastcharindex;

        public ushort LastCharIndex
        {
            get { return _lastcharindex; }
            set { _lastcharindex = value; }
        }

        private short _typoAsc;

        public short TypoAscender
        {
            get { return _typoAsc; }
            set { _typoAsc = value; }
        }

        private short _typodesc;

        public short TypoDescender
        {
            get { return _typodesc; }
            set { _typodesc = value; }
        }

        private short _tpyoline;

        public short TypoLineGap
        {
            get { return _tpyoline; }
            set { _tpyoline = value; }
        }

        private ushort _winascent;

        public ushort WinAscent
        {
            get { return _winascent; }
            set { _winascent = value; }
        }

        private ushort _windesc;

        public ushort WinDescent
        {
            get { return _windesc; }
            set { _windesc = value; }
        }

        private CodePageRange _cprange;

        public CodePageRange CodePageRanges
        {
            get { return _cprange; }
            set { _cprange = value; }
        }



        private short _h;

        public short Height
        {
            get { return _h; }
            set { _h = value; }
        }

        private short _caph;

        public short CapHeight
        {
            get { return _caph; }
            set { _caph = value; }
        }

        private ushort _defchar;

        public ushort DefaultChar
        {
            get { return _defchar; }
            set { _defchar = value; }
        }

        private ushort _breakchar;

        public ushort BreakChar
        {
            get { return _breakchar; }
            set { _breakchar = value; }
        }

        private ushort _maxcon;

        public ushort MaxContext
        {
            get { return _maxcon; }
            set { _maxcon = value; }
        }




    }
}
