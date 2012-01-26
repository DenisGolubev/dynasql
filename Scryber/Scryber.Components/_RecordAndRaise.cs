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

namespace Scryber
{
    internal static class RecordAndRaise
    {
        /// <summary>
        /// Formats a string sinking any exceptions that are raised
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        internal static string SafeFormat(string message, object[] args)
        {
            if (null != args && args.Length > 0)
            {
                string formatted = message;
                try
                {
                    formatted = String.Format(message, args);
                    message = formatted;
                }
                catch (Exception) { }
            }
            if (null == message)
                return string.Empty;
            else
                return message;
        }

        internal static void Record(string category, Exception inner)
        {
            if (null == inner)
                return;

            PDFTraceLog log = PDFTraceContext.GetLog();
            if (null == log)
                return;

            log.Begin(TraceLevel.Error, "INNER EXCEPTION details for re-thrown exception");

            while (null != inner)
            {
                log.Add(TraceLevel.Error, category, inner.Message, inner);
                inner = inner.InnerException;
            }

            log.End(TraceLevel.Error, "INNER EXCEPTION details for re-thrown exception");
        }


        #region internal static Exception ArgumentNull(...)

        internal static Exception ArgumentNull(string param)
        {
            return new ArgumentNullException(param);
        }

        internal static Exception ArgumentNull(string param, string message, params object[] args)
        {
            message = SafeFormat(message, args);
            return new ArgumentNullException(param, message);
        }

        #endregion

        #region internal static Exception Argument(...)

        internal static Exception Argument(string param)
        {
            return new ArgumentException(param);
        }

        internal static Exception Argument(string param, string message, params object[] args)
        {
            message = SafeFormat(message, args);
            return new ArgumentException(message, param);
        }

        internal static Exception Argument(Exception ex, string param, string message, params object[] args)
        {
            Record("ARGUMENT EXCEPTION", ex);
            message = SafeFormat(message, args);
            return new ArgumentException(message, param, ex);
        }

        #endregion

        #region internal static Exception ArgumentOutOfRange(...)

        internal static Exception ArgumentOutOfRange(string param)
        {
            return new ArgumentNullException(param);
        }

        internal static Exception ArgumentOutOfRange(string param, string message)
        {
            return new ArgumentOutOfRangeException(param, message);
        }

        internal static Exception ArgumentOutOfRange(string param, string message, params object[] args)
        {
            message = SafeFormat(message, args);
            return new ArgumentOutOfRangeException(param, message);
        }

        #endregion

        #region internal static Exception Operation(...)

        internal static Exception Operation(string message, params object[] args)
        {
            message = SafeFormat(message, args);
            return new InvalidOperationException(message);
        }
      
        internal static Exception Operation(Exception ex, string message, params object[] args)
        {
            Record("OPERATION EXCEPTION", ex);
            message = SafeFormat(message, args);
            return new InvalidOperationException(message, ex);
        }

        #endregion

        #region  internal static Exception NotSupported(string message, params object[] args)

        internal static Exception NotSupported(string message, params object[] args)
        {
            message = SafeFormat(message, args);
            return new NotSupportedException(String.Format(message, args));
        }

        #endregion

        #region internal static Exception InvalidCast(...)
        
        internal static Exception InvalidCast(string message, params object[] args)
        {
            message = SafeFormat(message, args);
            return new InvalidCastException(message);
        }

        internal static Exception InvalidCast(Exception ex, string message, params object[] args)
        {
            Record("INVALID CAST EXCEPTION", ex);
            message = SafeFormat(message, args);
            return new InvalidCastException(message, ex);
        }

        #endregion

        #region public static Exception NullReference(...)

        public static Exception NullReference(string message, params object[] args)
        {
            message = SafeFormat(message, args);
            return new NullReferenceException(String.Format(message, args));
        }

        internal static Exception NullReference(string message, Exception inner)
        {
            Record("NULL REFERENCE EXCEPTION", inner);
            return new NullReferenceException(message, inner);
        }

        #endregion

        #region internal static Exception Data(...)

        internal static Exception Data(string message)
        {
            return new System.Data.DataException(message);
        }

        internal static Exception Data(string message, params object[] args)
        {
            message = SafeFormat(message, args);
            return new System.Data.DataException(message);
        }

        internal static Exception Data(Exception inner, string message)
        {
            Record("DATA EXCEPTION", inner);
            return new System.Data.DataException(message, inner);
        }

        internal static Exception Data(Exception inner, string message, params object[] args)
        {
            Record("DATA EXCEPTION", inner);
            message = SafeFormat(message, args);
            return new System.Data.DataException(message, inner);
        }

        #endregion

        #region internal static Exception LayoutException(...)

        internal static Exception LayoutException(string message)
        {
            return new PDFLayoutException(message);
        }

        internal static Exception LayoutException(Exception inner, string message)
        {
            Record("PDF LAYOUT EXCEPTION", inner);
            return new PDFLayoutException(message, inner);
        }
        internal static Exception LayoutException(string message, params object[] args)
        {
            message = SafeFormat(message, args);
            return new PDFLayoutException(message);
        }

        internal static Exception LayoutException(Exception inner, string message, params object[] args)
        {
            Record("PDF LAYOUT EXCEPTION", inner);
            message = SafeFormat(message, args); 
            return new PDFLayoutException(message, inner);
        }

        #endregion

        #region internal static Exception BindException(...)

        internal static Exception BindException(string message)
        {
            return new PDFBindException(message);
        }

        internal static Exception BindException(Exception inner, string message)
        {
            Record("PDF BIND EXCEPTION", inner);
            return new PDFBindException(message, inner);
        }
        internal static Exception BindException(string message, params object[] args)
        {
            message = SafeFormat(message, args);
            return new PDFBindException(message);
        }

        internal static Exception BindException(Exception inner, string message, params object[] args)
        {
            Record("PDF BIND EXCEPTION", inner);
            message = SafeFormat(message, args);
            return new PDFBindException(message, inner);
        }

        #endregion

        #region internal static Exception RenderException(...)

        internal static Exception RenderException(string message)
        {
            return new PDFRenderException(message);
        }

        internal static Exception RenderException(Exception inner, string message)
        {
            Record("PDF RENDER EXCEPTION", inner);
            return new PDFRenderException(message, inner);
        }
        internal static Exception RenderException(string message, params object[] args)
        {
            message = SafeFormat(message, args);
            return new PDFRenderException(message);
        }

        internal static Exception RenderException(Exception inner, string message, params object[] args)
        {
            Record("PDF RENDER EXCEPTION", inner);
            message = SafeFormat(message, args);
            return new PDFRenderException(message, inner);
        }

        #endregion

        #region internal static Exception FileNotFound(...)

        internal static Exception FileNotFound(string path)
        {
            return new System.IO.FileNotFoundException(Errors.FileNotFound, path);
        }
        #endregion

        #region internal static Exception ParserException(...)

        internal static Exception ParserException(string message, params object[] param)
        {
            message = SafeFormat(message, param);
            return new PDFParserException(message);
        }

        internal static Exception ParserException(Exception inner, string message, params object[] args)
        {
            Record("PARSER EXCEPTION", inner);
            message = SafeFormat(message, args);
            return new PDFParserException(message, inner);
        }

        #endregion

        #region internal static Exception ConfigurationException(...)

        internal static Exception ConfigurationException(string message, params object[] param)
        {
            message = SafeFormat(message, param);
            return new System.Configuration.ConfigurationErrorsException(message);
        }

        internal static Exception ConfigurationException(Exception inner, string message, params object[] param)
        {
            Record("CONFIGURATION EXCEPTION", inner); 
            message = SafeFormat(message, param);
            return new System.Configuration.ConfigurationErrorsException(message, inner);
        }

        #endregion

        #region internal static Exception NoValidLicence(string message)
        /// <summary>
        /// Raises the Licence exception
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        internal static Exception NoValidLicense(string message)
        {
            object[] all = new object[] { message };
            message = SafeFormat(Errors.NoValidLicenceFoundForOperation, all);

            return new PDFLicenceException(message);
        }

        #endregion
    }
}
