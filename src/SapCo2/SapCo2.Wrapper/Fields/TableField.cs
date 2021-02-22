using System;
using System.Linq;
using SapCo2.Wrapper.Abstract;
using SapCo2.Wrapper.Enumeration;
using SapCo2.Wrapper.Extension;
using SapCo2.Wrapper.Fields.Abstract;
using SapCo2.Wrapper.Interop;
using SapCo2.Wrapper.Mappers;
using SapCo2.Wrapper.Struct;

namespace SapCo2.Wrapper.Fields
{
    internal sealed class TableField<TItem> : Field<TItem[]>
    {
        public TableField(string name, TItem[] value)
            : base(name, value)
        {
        }

        public override void Apply(IRfcInterop interop, IntPtr dataHandle)
        {
            RfcResultCodes resultCode = interop.GetTable(
                dataHandle: dataHandle,
                name: Name,
                out IntPtr tableHandle,
                out RfcErrorInfo errorInfo);

            resultCode.ThrowOnError(errorInfo);

            if(Value==null)
                return;

            foreach (TItem row in Value)
            {
                IntPtr lineHandle = interop.AppendNewRow(tableHandle, out errorInfo);
                errorInfo.ThrowOnError();
                InputMapper.Apply(interop, lineHandle, row);
            }
        }

        public static TableField<T> Extract<T>(IRfcInterop interop, IntPtr dataHandle, string name)
        {
            RfcResultCodes resultCode = interop.GetTable(
                dataHandle: dataHandle,
                name: name,
                tableHandle: out IntPtr tableHandle,
                errorInfo: out RfcErrorInfo errorInfo);

            resultCode.ThrowOnError(errorInfo);

            resultCode = interop.GetRowCount(
                tableHandle: tableHandle,
                out uint rowCount,
                out errorInfo);

            resultCode.ThrowOnError(errorInfo);

            var rows = new T[rowCount];

            for (int i = 0; i < rowCount; i++)
            {
                IntPtr rowHandle = interop.GetCurrentRow(
                    tableHandle: tableHandle,
                    errorInfo: out errorInfo);

                errorInfo.ThrowOnError();

                rows[i] = OutputMapper.Extract<T>(interop, rowHandle);

                resultCode = interop.MoveToNextRow(
                    tableHandle: tableHandle,
                    errorInfo: out errorInfo);

                if (resultCode == RfcResultCodes.RFC_TABLE_MOVE_EOF)
                    return new TableField<T>(name, rows.Take(i + 1).ToArray());

                resultCode.ThrowOnError(errorInfo);
            }

            return new TableField<T>(name, rows);
        }

        public override string ToString()
            => string.Join(Environment.NewLine, Value.Select((row, index) => $"[{index}] {row}"));
    }
}
