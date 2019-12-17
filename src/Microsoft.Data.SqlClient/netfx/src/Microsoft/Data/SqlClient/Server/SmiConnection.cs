// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Data;

namespace PwC.Data.SqlClient.Server
{

    internal abstract class SmiConnection : IDisposable
    {

        //
        // Miscellaneous directives / accessors
        //

        internal abstract string GetCurrentDatabase(
            SmiEventSink eventSink
        );

        internal abstract void SetCurrentDatabase(
            string databaseName,
            SmiEventSink eventSink
        );

        //
        // IDisposable
        //
        public virtual void Dispose()
        {
            // Obsoleting from SMI -- use Close( SmiEventSink ) instead.
            //  Intended to be removed (along with inheriting IDisposable) prior to RTM.

            // Implement body with throw because there are only a couple of ways to get to this code:
            //  1) Client is calling this method even though the server negotiated for V3+ and dropped support for V2-.
            //  2) Server didn't implement V2- on some interface and negotiated V2-.
            PwC.Data.Common.ADP.InternalError(PwC.Data.Common.ADP.InternalErrorCode.UnimplementedSMIMethod);
        }

        public virtual void Close(
            SmiEventSink eventSink
        )
        {
            // Adding as of V3

            // Implement body with throw because there are only a couple of ways to get to this code:
            //  1) Client is calling this method even though the server negotiated for V2- and hasn't implemented V3 yet.
            //  2) Server didn't implement V3 on some interface, but negotiated V3+.
            PwC.Data.Common.ADP.InternalError(PwC.Data.Common.ADP.InternalErrorCode.UnimplementedSMIMethod);
        }


        //
        // Transaction API (should we encapsulate in it's own class or interface?)
        //
        internal abstract void BeginTransaction(
            string name,
            IsolationLevel level,
            SmiEventSink eventSink
        );

        internal abstract void CommitTransaction(
            long transactionId,
            SmiEventSink eventSink
        );

        internal abstract void CreateTransactionSavePoint(
            long transactionId,
            string name,
            SmiEventSink eventSink
        );

        internal abstract byte[] GetDTCAddress( // better buffer management needed?  I.e. non-allocating call needed/possible?
            SmiEventSink eventSink
        );

        internal abstract void EnlistTransaction(
            byte[] token,                // better buffer management needed?  I.e. non-allocating call needed/possible?
            SmiEventSink eventSink
        );

        internal abstract byte[] PromoteTransaction( // better buffer management needed?  I.e. non-allocating call needed/possible?
            long transactionId,
            SmiEventSink eventSink
        );

        internal abstract void RollbackTransaction(
            long transactionId,
            string savePointName,        // only roll back to save point if name non-null
            SmiEventSink eventSink
        );

    }
}




