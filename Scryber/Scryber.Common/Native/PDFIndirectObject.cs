﻿/*  Copyright 2012 PerceiveIT Limited
 *  This file is part of the Scryber library.
 *
 *  You can redistribute Scryber and/or modify 
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 * 
 *  Scryber is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 * 
 *  You should have received a copy of the GNU General Public License
 *  along with Scryber source code in the COPYING.txt file.  If not, see <http://www.gnu.org/licenses/>.
 * 
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Scryber.Native
{
    public class PDFIndirectObject : IFileObject, IIndirectObject, IDisposable
    {
        private bool _disposed;
        private int _number;
        private int _gen;
        private long _offset;
        private bool _deleted;
        private PDFStream _objdata;
        private PDFStream _stream = null;

        #region public PDFObjectType Type {get;}

        /// <summary>
        /// Gets the PDFObjectType for this instance
        /// </summary>
        public PDFObjectType Type 
        { 
            get { return PDFObjectTypes.IndirectObject; }
        }

        #endregion

        #region public int Number {get;set;}

        /// <summary>
        /// Gets or Sets the unique number in this generation that identifes this object
        /// </summary>
        public int Number
        {
            get { return _number; }
            set { _number = value; }
        }

        #endregion

        #region public int Generation {get;set;}

        /// <summary>
        /// Gets or sets the Generation number for this object
        /// </summary>
        public int Generation
        {
            get { return _gen; }
            set { _gen = value; }
        }

        #endregion

        #region public long Offset {get;set;}

        /// <summary>
        /// Gets or sets the current byte offset in the PDFFile for this indirect object.
        /// </summary>
        public long Offset
        {
            get { return _offset; }
            set { this._offset = value; }
        }

        #endregion

        #region public bool Deleted {get;set;}

        /// <summary>
        /// Gets or Sets a flag to identify whether this object has been deleted
        /// </summary>
        public bool Deleted
        {
            get { return _deleted; }
            set { _deleted = value; }
        }

        #endregion

        #region public PDFStream ObjectData {get;}

        /// <summary>
        /// Gets the PDFStream that contains this objects data - Dictionaries, Arrays etc.
        /// </summary>
        public PDFStream ObjectData
        {
            get { return _objdata; }
        }

        #endregion

        /// <summary>
        /// Gets the flag to identifiy if this object also has stream data associated with it
        /// </summary>
        public bool HasStream
        {
            get { return this._stream != null; }
        }

        /// <summary>
        /// Gets the stream data associated with this object, or null if the stream has not been initialized
        /// </summary>
        public PDFStream Stream
        {
            get { return this._stream; }
        }

        
        /// <summary>
        /// Creates a new non intialized Indirect object
        /// </summary>
        public PDFIndirectObject()
        {
            this._gen = -1;
            this._number = -1;
            this._offset = -1L;
            this._objdata = new PDFStream(null, this);
        }

        /// <summary>
        /// Not supported writing of object data.
        /// </summary>
        /// <param name="writer">The writer to write data to.</param>
        public void WriteData(PDFWriter writer)
        {
            throw new NotSupportedException("The writing of a PDFIndirectObject is not supported.");
        }

        /// <summary>
        /// Initializes a new stream data on this object if it does not already have one.
        /// </summary>
        /// <param name="encoding">The text encoding to use for the stream</param>
        /// <param name="filters">The filters set to use for writing the stream data</param>
        public void InitStream(IStreamFilter[] filters)
        {
            if (_disposed)
                throw new InvalidOperationException("This indirect object has already been disposed");

            if (this._stream == null)
            {
                this._stream = new PDFStream(filters, this);
            }
        }


        public byte[] GetObjectData() 
        {
            return this.ObjectData.GetStreamData();
        }

        public byte[] GetStreamData() 
        {
            return this.Stream.GetStreamData();
        }

        public override string ToString()
        {
            return this.Number + " " + this.Generation + " R";
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this._objdata != null)
                    this._objdata.Dispose();
                this._objdata = null;

                if (this._stream != null)
                    this._stream.Dispose();
                this._stream = null;

                _disposed = true;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        ~PDFIndirectObject()
        {
            this.Dispose(false);
        }

    }
}
