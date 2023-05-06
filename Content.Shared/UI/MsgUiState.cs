using System.IO;
using Lidgren.Network;
using Robust.Shared.Network;
using Robust.Shared.Serialization;

namespace Content.Shared.UI
{
	/// <summary>
	///		Packet message for transmitting UI-related data.
	/// </summary>
    public sealed class MsgUiState : NetMessage
    {
		/// <summary>
		///		UI messages are sent reliably but unordered.
		/// </summary>
        public override MsgGroups MsgGroup => MsgGroups.Command;

		/// <summary>
		///		Specifies which UI this message is for.
		/// </summary>
		public uint Id;

		/// <summary>
		///		The contents of the message.
		/// </summary>
		public BaseUiStateMessage Message = default!;

		public override void ReadFromBuffer(NetIncomingMessage buffer, IRobustSerializer ser)
        {
            Id = buffer.ReadUInt32();

            var len = buffer.ReadVariableInt32();
            var stream = buffer.ReadAlignedMemory(len);
			Message = ser.Deserialize<BaseUiStateMessage>(stream);
        }

        public override void WriteToBuffer(NetOutgoingMessage buffer, IRobustSerializer ser)
        {
            buffer.Write(Id);
            var stream = new MemoryStream();

            ser.Serialize(stream, Message);
            var length = (int)stream.Length;
            buffer.WriteVariableInt32(length);
            buffer.Write(stream.GetBuffer().AsSpan(0, length));
        }
    }
}
